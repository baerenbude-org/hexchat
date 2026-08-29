using HexChat.Core.State;

namespace HexChat.Core.Commands;

public sealed class CommandExecutionContext
{
    public required IrcClient Client { get; init; }
    public required IrcChannel ActiveChannel { get; init; }
    public required Action<string> OnSystemMessage { get; init; }
    public required Action OnClearScreen { get; init; }
}

/// <summary>
/// Handles user-entered input lines and executes standard HexChat slash commands.
/// </summary>
public static class HexChatCommandHandler
{
    public static async Task ExecuteAsync(string input, CommandExecutionContext ctx, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input)) return;

        if (!input.StartsWith('/'))
        {
            // Regular chat message sent to active channel or query
            if (ctx.ActiveChannel.Type == ChannelType.Server)
            {
                ctx.OnSystemMessage("Im Server-Buffer können keine direkten Chatnachrichten gesendet werden. Tritt einem Channel bei mit /join #channel");
                return;
            }

            await ctx.Client.SendMessageAsync(ctx.ActiveChannel.Name, input, cancellationToken).ConfigureAwait(false);
            return;
        }

        // Parse command and args
        string line = input[1..];
        int spaceIdx = line.IndexOf(' ');
        string cmd = spaceIdx < 0 ? line.ToLowerInvariant() : line[..spaceIdx].ToLowerInvariant();
        string args = spaceIdx < 0 ? string.Empty : line[(spaceIdx + 1)..].Trim();

        switch (cmd)
        {
            case "j" or "join":
                if (string.IsNullOrEmpty(args))
                {
                    ctx.OnSystemMessage("Verwendung: /join <#channel> [passwort]");
                    return;
                }
                var joinParts = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string chName = joinParts[0].StartsWith('#') || joinParts[0].StartsWith('&') ? joinParts[0] : $"#{joinParts[0]}";
                string? chKey = joinParts.Length > 1 ? joinParts[1] : null;
                await ctx.Client.JoinChannelAsync(chName, chKey, cancellationToken).ConfigureAwait(false);
                break;

            case "part" or "p":
                string partChannel = ctx.ActiveChannel.Name;
                string? partReason = null;
                if (!string.IsNullOrEmpty(args))
                {
                    if (args.StartsWith('#') || args.StartsWith('&'))
                    {
                        var pParts = args.Split(' ', 2);
                        partChannel = pParts[0];
                        partReason = pParts.Length > 1 ? pParts[1] : null;
                    }
                    else
                    {
                        partReason = args;
                    }
                }
                await ctx.Client.PartChannelAsync(partChannel, partReason, cancellationToken).ConfigureAwait(false);
                break;

            case "close" or "wc":
                if (ctx.ActiveChannel.Type != ChannelType.Server)
                {
                    await ctx.Client.PartChannelAsync(ctx.ActiveChannel.Name, null, cancellationToken).ConfigureAwait(false);
                }
                break;

            case "msg":
                var msgParts = args.Split(' ', 2);
                if (msgParts.Length < 2)
                {
                    ctx.OnSystemMessage("Verwendung: /msg <ziel> <nachricht>");
                    return;
                }
                await ctx.Client.SendMessageAsync(msgParts[0], msgParts[1], cancellationToken).ConfigureAwait(false);
                break;

            case "me":
                if (string.IsNullOrEmpty(args))
                {
                    ctx.OnSystemMessage("Verwendung: /me <aktion>");
                    return;
                }
                if (ctx.ActiveChannel.Type != ChannelType.Server)
                {
                    string actionMsg = $"\x01ACTION {args}\x01";
                    await ctx.Client.SendMessageAsync(ctx.ActiveChannel.Name, actionMsg, cancellationToken).ConfigureAwait(false);
                }
                break;

            case "query" or "q":
                if (string.IsNullOrEmpty(args))
                {
                    ctx.OnSystemMessage("Verwendung: /query <nick> [nachricht]");
                    return;
                }
                var qParts = args.Split(' ', 2);
                string targetNick = qParts[0];
                if (qParts.Length > 1)
                {
                    await ctx.Client.SendMessageAsync(targetNick, qParts[1], cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    // Create query channel buffer
                    await ctx.Client.SendRawAsync($"WHOIS {targetNick}", cancellationToken).ConfigureAwait(false);
                }
                break;

            case "nick":
                if (string.IsNullOrEmpty(args))
                {
                    ctx.OnSystemMessage("Verwendung: /nick <neuer_nick>");
                    return;
                }
                await ctx.Client.SendRawAsync($"NICK {args.Split(' ')[0]}", cancellationToken).ConfigureAwait(false);
                break;

            case "topic":
                if (ctx.ActiveChannel.Type == ChannelType.Channel)
                {
                    string topicCmd = string.IsNullOrEmpty(args)
                        ? $"TOPIC {ctx.ActiveChannel.Name}"
                        : $"TOPIC {ctx.ActiveChannel.Name} :{args}";
                    await ctx.Client.SendRawAsync(topicCmd, cancellationToken).ConfigureAwait(false);
                }
                break;

            case "raw" or "quote":
                if (string.IsNullOrEmpty(args))
                {
                    ctx.OnSystemMessage("Verwendung: /raw <IRC-Befehl>");
                    return;
                }
                await ctx.Client.SendRawAsync(args, cancellationToken).ConfigureAwait(false);
                break;

            case "ns" or "nickserv":
                await ctx.Client.SendMessageAsync("NickServ", args, cancellationToken).ConfigureAwait(false);
                break;

            case "cs" or "chanserv":
                await ctx.Client.SendMessageAsync("ChanServ", args, cancellationToken).ConfigureAwait(false);
                break;

            case "clear":
                ctx.OnClearScreen();
                break;

            case "quit":
                string qReason = string.IsNullOrEmpty(args) ? "Leaving" : args;
                await ctx.Client.SendRawAsync($"QUIT :{qReason}", cancellationToken).ConfigureAwait(false);
                break;

            default:
                // Fallback: send as raw uppercase command
                await ctx.Client.SendRawAsync($"{cmd.ToUpperInvariant()} {args}", cancellationToken).ConfigureAwait(false);
                break;
        }
    }
}
