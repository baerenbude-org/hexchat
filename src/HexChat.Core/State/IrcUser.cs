namespace HexChat.Core.State;

public sealed class IrcUser
{
    public string Nick { get; internal set; }
    public string? Ident { get; internal set; }
    public string? Host { get; internal set; }
    public string? RealName { get; internal set; }
    public string? Account { get; internal set; }
    public bool IsAway { get; internal set; }
    public string? AwayMessage { get; internal set; }
    public bool IsBot { get; internal set; }

    public IrcUser(string nick, string? ident = null, string? host = null)
    {
        Nick = nick;
        Ident = ident;
        Host = host;
    }

    public override string ToString() => string.IsNullOrEmpty(Ident) || string.IsNullOrEmpty(Host)
        ? Nick
        : $"{Nick}!{Ident}@{Host}";
}

public sealed class ChannelUser
{
    public IrcUser User { get; }
    public char? HighestPrefix { get; internal set; }
    public string Modes { get; internal set; } = string.Empty;

    public ChannelUser(IrcUser user, char? prefix = null)
    {
        User = user;
        HighestPrefix = prefix;
    }

    public int RankOrder => HighestPrefix switch
    {
        '~' or 'q' => 1, // Owner
        '&' or 'a' => 2, // Admin / Protect
        '@' or 'o' => 3, // Operator
        '%' or 'h' => 4, // Half-Op
        '+' or 'v' => 5, // Voice
        _ => 99          // Normal user
    };

    public string DisplayName => HighestPrefix.HasValue ? $"{HighestPrefix}{User.Nick}" : User.Nick;

    public override string ToString() => DisplayName;
}
