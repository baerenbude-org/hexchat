using System.Collections.Concurrent;
using HexChat.Core.Formatting;
using HexChat.Core.Network;
using HexChat.Core.Protocol;
using HexChat.Core.Security;

namespace HexChat.Core.State;

public sealed class IrcClientOptions
{
    public required string Server { get; init; }
    public required int Port { get; init; } = 6697;
    public bool UseTls { get; init; } = true;
    public required string Nickname { get; init; }
    public string? RealName { get; init; }
    public string? UserName { get; init; }
    public string? ServerPassword { get; init; }
    public string? SaslUsername { get; init; }
    public string? SaslPassword { get; init; }
    public string SaslMechanism { get; init; } = SaslScramSha256.MechanismName;
    public IReadOnlyList<string> AutoJoinChannels { get; init; } = Array.Empty<string>();
}

/// <summary>
/// High-level IRCv3 client managing connection, CAP negotiation, SASL, channels, users, and events.
/// </summary>
public sealed class IrcClient : IAsyncDisposable
{
    private readonly IrcConnection _connection = new();
    private IrcClientOptions? _options;
    private SaslScramSha256? _scramAuth;

    private readonly ConcurrentDictionary<string, IrcChannel> _channels = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _availableCaps = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _enabledCaps = new(StringComparer.OrdinalIgnoreCase);

    public string Nickname { get; private set; } = string.Empty;
    public bool IsRegistered { get; private set; }
    public bool IsConnected => _connection.IsConnected;
    public IReadOnlyDictionary<string, IrcChannel> Channels => _channels;
    public IrcChannel ServerBuffer { get; } = new("Server", ChannelType.Server);

    public event Action<IrcChannel, ChatMessage>? MessageReceived;
    public event Action<IrcChannel>? ChannelJoined;
    public event Action<IrcChannel, string?>? ChannelParted;
    public event Action<IrcChannel>? UserListUpdated;
    public event Action<IrcChannel, string?>? TopicUpdated;
    public event Action? Connected;
    public event Action<Exception?>? Disconnected;

    public IrcClient()
    {
        _connection.LineReceived += OnLineReceivedAsync;
        _connection.Disconnected += OnDisconnectedAsync;
        _channels["Server"] = ServerBuffer;
    }

    public async Task ConnectAsync(IrcClientOptions options, CancellationToken cancellationToken = default)
    {
        _options = options;
        Nickname = options.Nickname;
        IsRegistered = false;
        _availableCaps.Clear();
        _enabledCaps.Clear();

        await _connection.ConnectAsync(new IrcConnectionOptions
        {
            Host = options.Server,
            Port = options.Port,
            UseTls = options.UseTls
        }, cancellationToken).ConfigureAwait(false);

        // Initiate IRCv3 Capability Negotiation
        await _connection.SendLineAsync("CAP LS 302", cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(options.ServerPassword))
        {
            await _connection.SendLineAsync($"PASS {options.ServerPassword}", cancellationToken).ConfigureAwait(false);
        }

        await _connection.SendLineAsync($"NICK {options.Nickname}", cancellationToken).ConfigureAwait(false);
        string ident = string.IsNullOrEmpty(options.UserName) ? "hexchat" : options.UserName;
        string real = string.IsNullOrEmpty(options.RealName) ? "HexChat Avalonia Client" : options.RealName;
        await _connection.SendLineAsync($"USER {ident} 0 * :{real}", cancellationToken).ConfigureAwait(false);
    }

    public async Task SendRawAsync(string rawLine, CancellationToken cancellationToken = default)
    {
        await _connection.SendLineAsync(rawLine, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendMessageAsync(string target, string messageText, CancellationToken cancellationToken = default)
    {
        await _connection.SendLineAsync($"PRIVMSG {target} :{messageText}", cancellationToken).ConfigureAwait(false);

        // If echo-message is NOT enabled, manually echo message to our local buffer
        if (!_enabledCaps.Contains("echo-message"))
        {
            var targetChannel = GetOrCreateChannel(target);
            var chatMsg = new ChatMessage(
                DateTimeOffset.UtcNow,
                Nickname,
                messageText,
                MircColorParser.Parse(messageText)
            );
            targetChannel.AddMessage(chatMsg);
            MessageReceived?.Invoke(targetChannel, chatMsg);
        }
    }

    public async Task JoinChannelAsync(string channelName, string? key = null, CancellationToken cancellationToken = default)
    {
        string cmd = string.IsNullOrEmpty(key) ? $"JOIN {channelName}" : $"JOIN {channelName} {key}";
        await _connection.SendLineAsync(cmd, cancellationToken).ConfigureAwait(false);
    }

    public async Task PartChannelAsync(string channelName, string? reason = null, CancellationToken cancellationToken = default)
    {
        string cmd = string.IsNullOrEmpty(reason) ? $"PART {channelName}" : $"PART {channelName} :{reason}";
        await _connection.SendLineAsync(cmd, cancellationToken).ConfigureAwait(false);
    }

    private async Task OnLineReceivedAsync(string rawLine)
    {
        IrcMessage msg;
        try
        {
            msg = IrcMessage.Parse(rawLine);
        }
        catch
        {
            return;
        }

        switch (msg.Command.ToUpperInvariant())
        {
            case "PING":
                await HandlePingAsync(msg).ConfigureAwait(false);
                break;
            case "CAP":
                await HandleCapAsync(msg).ConfigureAwait(false);
                break;
            case "AUTHENTICATE":
                await HandleAuthenticateAsync(msg).ConfigureAwait(false);
                break;
            case "900": // RPL_LOGGEDIN
            case "903": // RPL_SASLSUCCESS
                await _connection.SendLineAsync("CAP END").ConfigureAwait(false);
                break;
            case "904": // ERR_SASLFAIL
                ServerBuffer.AddMessage(new ChatMessage(DateTimeOffset.UtcNow, null, "SASL-Authentifizierung fehlgeschlagen.", Array.Empty<FormattedSpan>(), IsSystemNotice: true));
                await _connection.SendLineAsync("CAP END").ConfigureAwait(false);
                break;
            case "001": // RPL_WELCOME
                IsRegistered = true;
                if (msg.Parameters.Count > 0) Nickname = msg.Parameters[0];
                Connected?.Invoke();
                await OnRegisteredAsync().ConfigureAwait(false);
                break;
            case "JOIN":
                HandleJoin(msg);
                break;
            case "PART":
                HandlePart(msg);
                break;
            case "QUIT":
                HandleQuit(msg);
                break;
            case "NICK":
                HandleNickChange(msg);
                break;
            case "TOPIC":
            case "332": // RPL_TOPIC
                HandleTopic(msg);
                break;
            case "353": // RPL_NAMREPLY
                HandleNamReply(msg);
                break;
            case "366": // RPL_ENDOFNAMES
                HandleEndOfNames(msg);
                break;
            case "PRIVMSG":
                HandlePrivmsg(msg);
                break;
            case "NOTICE":
                HandleNotice(msg);
                break;
            default:
                HandleGenericMessage(msg);
                break;
        }
    }

    private async Task HandlePingAsync(IrcMessage msg)
    {
        string target = msg.Trailing ?? (msg.Parameters.Count > 0 ? msg.Parameters[0] : string.Empty);
        await _connection.SendLineAsync($"PONG :{target}").ConfigureAwait(false);
    }

    private async Task HandleCapAsync(IrcMessage msg)
    {
        if (msg.Parameters.Count < 2) return;
        string subCommand = msg.Parameters[1].ToUpperInvariant();

        switch (subCommand)
        {
            case "LS":
                string capsString = msg.Trailing ?? string.Empty;
                var offeredCaps = capsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var c in offeredCaps)
                {
                    int eq = c.IndexOf('=');
                    _availableCaps.Add(eq >= 0 ? c[..eq] : c);
                }

                // If multi-line CAP LS (contains *), wait for next line
                if (msg.Parameters.Count > 2 && msg.Parameters[2] == "*") return;

                // Request desired modern IRCv3 caps
                var desired = new List<string>();
                string[] wanted = { "server-time", "message-tags", "echo-message", "batch", "account-notify", "away-notify", "chghost", "extended-join", "multi-prefix", "draft/typing", "draft/react" };
                foreach (var w in wanted)
                {
                    if (_availableCaps.Contains(w)) desired.Add(w);
                }

                bool wantsSasl = !string.IsNullOrEmpty(_options?.SaslUsername) && !string.IsNullOrEmpty(_options?.SaslPassword);
                if (wantsSasl && _availableCaps.Contains("sasl"))
                {
                    desired.Add("sasl");
                }

                if (desired.Count > 0)
                {
                    await _connection.SendLineAsync($"CAP REQ :{string.Join(' ', desired)}").ConfigureAwait(false);
                }
                else
                {
                    await _connection.SendLineAsync("CAP END").ConfigureAwait(false);
                }
                break;

            case "ACK":
                string ackCaps = msg.Trailing ?? string.Empty;
                foreach (var ac in ackCaps.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    _enabledCaps.Add(ac);
                }

                if (_enabledCaps.Contains("sasl") && !string.IsNullOrEmpty(_options?.SaslUsername))
                {
                    string mech = _options.SaslMechanism == SaslScramSha256.MechanismName ? "SCRAM-SHA-256" : "PLAIN";
                    if (mech == "SCRAM-SHA-256")
                    {
                        _scramAuth = new SaslScramSha256(_options.SaslUsername, _options.SaslPassword!);
                    }
                    await _connection.SendLineAsync($"AUTHENTICATE {mech}").ConfigureAwait(false);
                }
                else
                {
                    await _connection.SendLineAsync("CAP END").ConfigureAwait(false);
                }
                break;

            case "NAK":
                await _connection.SendLineAsync("CAP END").ConfigureAwait(false);
                break;
        }
    }

    private async Task HandleAuthenticateAsync(IrcMessage msg)
    {
        if (_options == null || string.IsNullOrEmpty(_options.SaslUsername) || string.IsNullOrEmpty(_options.SaslPassword))
        {
            await _connection.SendLineAsync("AUTHENTICATE *").ConfigureAwait(false);
            return;
        }

        string param = msg.Parameters.Count > 0 ? msg.Parameters[0] : (msg.Trailing ?? string.Empty);

        if (param == "+")
        {
            // Initial server challenge
            if (_options.SaslMechanism == SaslScramSha256.MechanismName && _scramAuth != null)
            {
                string firstMsg = _scramAuth.GenerateClientFirstMessage();
                await _connection.SendLineAsync($"AUTHENTICATE {firstMsg}").ConfigureAwait(false);
            }
            else
            {
                string plainPayload = SaslPlain.GeneratePayload(_options.SaslUsername, _options.SaslPassword);
                await _connection.SendLineAsync($"AUTHENTICATE {plainPayload}").ConfigureAwait(false);
            }
        }
        else if (_scramAuth != null)
        {
            // Server SCRAM challenge response
            string clientFinal = _scramAuth.ProcessServerFirstMessage(param);
            await _connection.SendLineAsync($"AUTHENTICATE {clientFinal}").ConfigureAwait(false);
        }
    }

    private async Task OnRegisteredAsync()
    {
        if (_options?.AutoJoinChannels != null)
        {
            foreach (var ch in _options.AutoJoinChannels)
            {
                await JoinChannelAsync(ch).ConfigureAwait(false);
            }
        }
    }

    private void HandleJoin(IrcMessage msg)
    {
        string channelName = msg.Parameters.Count > 0 ? msg.Parameters[0] : (msg.Trailing ?? string.Empty);
        if (string.IsNullOrEmpty(channelName)) return;

        var channel = GetOrCreateChannel(channelName);
        string nick = msg.Nick ?? string.Empty;

        if (string.Equals(nick, Nickname, StringComparison.OrdinalIgnoreCase))
        {
            ChannelJoined?.Invoke(channel);
        }

        var user = new IrcUser(nick, msg.User, msg.Host);
        channel.AddUser(new ChannelUser(user));
        UserListUpdated?.Invoke(channel);
    }

    private void HandlePart(IrcMessage msg)
    {
        string channelName = msg.Parameters.Count > 0 ? msg.Parameters[0] : (msg.Trailing ?? string.Empty);
        if (string.IsNullOrEmpty(channelName)) return;

        string nick = msg.Nick ?? string.Empty;
        if (_channels.TryGetValue(channelName, out var channel))
        {
            channel.RemoveUser(nick);
            UserListUpdated?.Invoke(channel);

            if (string.Equals(nick, Nickname, StringComparison.OrdinalIgnoreCase))
            {
                _channels.TryRemove(channelName, out _);
                ChannelParted?.Invoke(channel, msg.Trailing);
            }
        }
    }

    private void HandleQuit(IrcMessage msg)
    {
        string nick = msg.Nick ?? string.Empty;
        foreach (var ch in _channels.Values)
        {
            if (ch.RemoveUser(nick))
            {
                UserListUpdated?.Invoke(ch);
            }
        }
    }

    private void HandleNickChange(IrcMessage msg)
    {
        string oldNick = msg.Nick ?? string.Empty;
        string newNick = msg.Trailing ?? (msg.Parameters.Count > 0 ? msg.Parameters[0] : string.Empty);

        if (string.Equals(oldNick, Nickname, StringComparison.OrdinalIgnoreCase))
        {
            Nickname = newNick;
        }

        foreach (var ch in _channels.Values)
        {
            ch.RenameUser(oldNick, newNick);
            UserListUpdated?.Invoke(ch);
        }
    }

    private void HandleTopic(IrcMessage msg)
    {
        string channelName = msg.Parameters.Count > 0 ? msg.Parameters[0] : string.Empty;
        if (msg.Command == "332" && msg.Parameters.Count > 1)
        {
            channelName = msg.Parameters[1];
        }

        if (_channels.TryGetValue(channelName, out var ch))
        {
            ch.Topic = msg.Trailing;
            TopicUpdated?.Invoke(ch, ch.Topic);
        }
    }

    private void HandleNamReply(IrcMessage msg)
    {
        if (msg.Parameters.Count < 3) return;
        string channelName = msg.Parameters[2];
        var channel = GetOrCreateChannel(channelName);

        string namesStr = msg.Trailing ?? string.Empty;
        foreach (var entry in namesStr.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            char? prefix = null;
            string nick = entry;
            if (entry.Length > 1 && "~&@%+".Contains(entry[0]))
            {
                prefix = entry[0];
                nick = entry[1..];
            }

            channel.AddUser(new ChannelUser(new IrcUser(nick), prefix));
        }
    }

    private void HandleEndOfNames(IrcMessage msg)
    {
        if (msg.Parameters.Count < 2) return;
        string channelName = msg.Parameters[1];
        if (_channels.TryGetValue(channelName, out var channel))
        {
            UserListUpdated?.Invoke(channel);
        }
    }

    private void HandlePrivmsg(IrcMessage msg)
    {
        if (msg.Parameters.Count < 1) return;
        string target = msg.Parameters[0];
        string text = msg.Trailing ?? string.Empty;

        // Is it a channel message or a private query?
        string bufferKey = target.StartsWith('#') || target.StartsWith('&')
            ? target
            : (string.Equals(target, Nickname, StringComparison.OrdinalIgnoreCase) ? (msg.Nick ?? target) : target);

        var channel = GetOrCreateChannel(bufferKey, target.StartsWith('#') || target.StartsWith('&') ? ChannelType.Channel : ChannelType.Query);

        bool isHighlight = !string.IsNullOrEmpty(Nickname) && text.Contains(Nickname, StringComparison.OrdinalIgnoreCase);
        bool isAction = text.StartsWith("\x01ACTION ") && text.EndsWith("\x01");

        string cleanText = isAction ? text[8..^1] : text;
        var chatMsg = new ChatMessage(
            msg.ServerTime ?? DateTimeOffset.UtcNow,
            msg.Nick,
            cleanText,
            MircColorParser.Parse(cleanText),
            IsHighlight: isHighlight,
            IsAction: isAction,
            MessageId: msg.Tags.TryGetValue(IrcTags.MessageId, out var mid) ? mid : null
        );

        channel.AddMessage(chatMsg);
        MessageReceived?.Invoke(channel, chatMsg);
    }

    private void HandleNotice(IrcMessage msg)
    {
        string target = msg.Parameters.Count > 0 ? msg.Parameters[0] : "Server";
        string text = msg.Trailing ?? string.Empty;

        var channel = GetOrCreateChannel(target.StartsWith('#') ? target : "Server");
        var chatMsg = new ChatMessage(
            msg.ServerTime ?? DateTimeOffset.UtcNow,
            msg.Nick ?? "Notice",
            text,
            MircColorParser.Parse(text),
            IsSystemNotice: true
        );
        channel.AddMessage(chatMsg);
        MessageReceived?.Invoke(channel, chatMsg);
    }

    private void HandleGenericMessage(IrcMessage msg)
    {
        if (!string.IsNullOrEmpty(msg.Trailing) && (msg.Numeric.HasValue || msg.Command == "NOTICE"))
        {
            var chatMsg = new ChatMessage(
                DateTimeOffset.UtcNow,
                null,
                msg.Trailing,
                MircColorParser.Parse(msg.Trailing),
                IsSystemNotice: true
            );
            ServerBuffer.AddMessage(chatMsg);
            MessageReceived?.Invoke(ServerBuffer, chatMsg);
        }
    }

    private IrcChannel GetOrCreateChannel(string name, ChannelType type = ChannelType.Channel)
    {
        return _channels.GetOrAdd(name, n => new IrcChannel(n, type));
    }

    private async Task OnDisconnectedAsync(Exception? ex)
    {
        IsRegistered = false;
        Disconnected?.Invoke(ex);
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync().ConfigureAwait(false);
    }
}
