using System.Collections.Concurrent;
using HexChat.Core.Formatting;

namespace HexChat.Core.State;

public enum ChannelType
{
    Channel,
    Query,
    Server
}

public sealed record ChatMessage(
    DateTimeOffset Timestamp,
    string? Sender,
    string Text,
    IReadOnlyList<FormattedSpan> Spans,
    bool IsHighlight = false,
    bool IsAction = false,
    bool IsSystemNotice = false,
    string? MessageId = null
);

public sealed class IrcChannel
{
    public string Name { get; }
    public ChannelType Type { get; }
    public string? Topic { get; internal set; }
    public string? TopicSetBy { get; internal set; }
    public DateTimeOffset? TopicSetAt { get; internal set; }
    public string? Modes { get; internal set; }

    private readonly ConcurrentDictionary<string, ChannelUser> _users = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<ChatMessage> _messages = new();
    private readonly object _lock = new();

    public int UnreadCount { get; internal set; }
    public int HighlightCount { get; internal set; }

    public IReadOnlyDictionary<string, ChannelUser> Users => _users;
    public IReadOnlyList<ChatMessage> Messages
    {
        get
        {
            lock (_lock)
            {
                return _messages.ToArray();
            }
        }
    }

    public IrcChannel(string name, ChannelType type = ChannelType.Channel)
    {
        Name = name;
        Type = type;
    }

    public void AddUser(ChannelUser user)
    {
        _users[user.User.Nick] = user;
    }

    public bool RemoveUser(string nick)
    {
        return _users.TryRemove(nick, out _);
    }

    public bool TryGetUser(string nick, out ChannelUser? user)
    {
        return _users.TryGetValue(nick, out user);
    }

    public void RenameUser(string oldNick, string newNick)
    {
        if (_users.TryRemove(oldNick, out var user))
        {
            user.User.Nick = newNick;
            _users[newNick] = user;
        }
    }

    public void ClearUsers()
    {
        _users.Clear();
    }

    public void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _messages.Add(message);
            if (message.IsHighlight)
            {
                HighlightCount++;
            }
            else if (!message.IsSystemNotice)
            {
                UnreadCount++;
            }
        }
    }

    public void MarkRead()
    {
        lock (_lock)
        {
            UnreadCount = 0;
            HighlightCount = 0;
        }
    }

    public override string ToString() => Name;
}
