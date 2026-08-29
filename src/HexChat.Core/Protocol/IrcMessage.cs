using System.Collections.Frozen;
using System.Text;

namespace HexChat.Core.Protocol;

/// <summary>
/// Represents a structured, fully-parsed IRC/IRCv3 message.
/// Supports message tags, source prefix (nick!user@host or server), command / numeric, parameters, and trailing message.
/// </summary>
public sealed class IrcMessage
{
    public IReadOnlyDictionary<string, string> Tags { get; }
    public string? Prefix { get; }
    public string? Nick { get; }
    public string? User { get; }
    public string? Host { get; }
    public string Command { get; }
    public int? Numeric { get; }
    public IReadOnlyList<string> Parameters { get; }
    public string? Trailing { get; }
    public string Raw { get; }

    /// <summary>
    /// Gets the server-time parsed from the @time tag if available, or null.
    /// </summary>
    public DateTimeOffset? ServerTime
    {
        get
        {
            if (Tags.TryGetValue(IrcTags.ServerTime, out var timeStr) &&
                DateTimeOffset.TryParse(timeStr, out var dto))
            {
                return dto;
            }
            return null;
        }
    }

    public IrcMessage(
        string raw,
        IReadOnlyDictionary<string, string> tags,
        string? prefix,
        string command,
        IReadOnlyList<string> parameters,
        string? trailing)
    {
        Raw = raw;
        Tags = tags;
        Prefix = prefix;
        Command = command;
        Parameters = parameters;
        Trailing = trailing;

        if (int.TryParse(command, out var num))
        {
            Numeric = num;
        }

        if (!string.IsNullOrEmpty(prefix))
        {
            int nickEnd = prefix.IndexOf('!');
            if (nickEnd >= 0)
            {
                Nick = prefix[..nickEnd];
                int userEnd = prefix.IndexOf('@', nickEnd + 1);
                if (userEnd >= 0)
                {
                    User = prefix[(nickEnd + 1)..userEnd];
                    Host = prefix[(userEnd + 1)..];
                }
                else
                {
                    User = prefix[(nickEnd + 1)..];
                }
            }
            else
            {
                int hostStart = prefix.IndexOf('@');
                if (hostStart >= 0)
                {
                    Nick = prefix[..hostStart];
                    Host = prefix[(hostStart + 1)..];
                }
                else
                {
                    Nick = prefix;
                }
            }
        }
    }

    /// <summary>
    /// Parses a raw IRC line into a structured <see cref="IrcMessage"/>.
    /// </summary>
    public static IrcMessage Parse(string rawLine)
    {
        ArgumentNullException.ThrowIfNull(rawLine);

        var span = rawLine.AsSpan().Trim();
        var tags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string? prefix = null;
        string command;
        var parameters = new List<string>();
        string? trailing = null;

        // 1. Parse IRCv3 Tags (@...)
        if (span.StartsWith("@"))
        {
            int tagEnd = span.IndexOf(' ');
            if (tagEnd < 0)
            {
                throw new FormatException("Invalid IRC message format: tag without command.");
            }

            var tagsSpan = span[1..tagEnd];
            span = span[(tagEnd + 1)..].TrimStart();

            ParseTags(tagsSpan, tags);
        }

        // 2. Parse Prefix (:...)
        if (span.StartsWith(":"))
        {
            int prefixEnd = span.IndexOf(' ');
            if (prefixEnd < 0)
            {
                throw new FormatException("Invalid IRC message format: prefix without command.");
            }

            prefix = span[1..prefixEnd].ToString();
            span = span[(prefixEnd + 1)..].TrimStart();
        }

        // 3. Parse Command
        int commandEnd = span.IndexOf(' ');
        if (commandEnd < 0)
        {
            command = span.ToString();
            span = ReadOnlySpan<char>.Empty;
        }
        else
        {
            command = span[..commandEnd].ToString();
            span = span[(commandEnd + 1)..].TrimStart();
        }

        // 4. Parse Parameters & Trailing
        while (!span.IsEmpty)
        {
            if (span.StartsWith(":"))
            {
                trailing = span[1..].ToString();
                break;
            }

            int nextSpace = span.IndexOf(' ');
            if (nextSpace < 0)
            {
                parameters.Add(span.ToString());
                break;
            }

            parameters.Add(span[..nextSpace].ToString());
            span = span[(nextSpace + 1)..].TrimStart();
        }

        return new IrcMessage(rawLine, tags, prefix, command, parameters, trailing);
    }

    private static void ParseTags(ReadOnlySpan<char> tagsSpan, Dictionary<string, string> tags)
    {
        while (!tagsSpan.IsEmpty)
        {
            int semiIndex = tagsSpan.IndexOf(';');
            ReadOnlySpan<char> tagPair = semiIndex >= 0 ? tagsSpan[..semiIndex] : tagsSpan;
            tagsSpan = semiIndex >= 0 ? tagsSpan[(semiIndex + 1)..] : ReadOnlySpan<char>.Empty;

            if (tagPair.IsEmpty) continue;

            int eqIndex = tagPair.IndexOf('=');
            if (eqIndex >= 0)
            {
                string key = tagPair[..eqIndex].ToString();
                string val = IrcTags.UnescapeTagValue(tagPair[(eqIndex + 1)..]);
                tags[key] = val;
            }
            else
            {
                tags[tagPair.ToString()] = string.Empty;
            }
        }
    }

    /// <summary>
    /// Formats an IRC message to standard wire format.
    /// </summary>
    public static string Format(
        string command,
        IEnumerable<string>? parameters = null,
        string? trailing = null,
        IDictionary<string, string>? tags = null)
    {
        var sb = new StringBuilder();

        if (tags != null && tags.Count > 0)
        {
            sb.Append('@');
            bool first = true;
            foreach (var kvp in tags)
            {
                if (!first) sb.Append(';');
                sb.Append(kvp.Key);
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    sb.Append('=');
                    sb.Append(IrcTags.EscapeTagValue(kvp.Value));
                }
                first = false;
            }
            sb.Append(' ');
        }

        sb.Append(command);

        if (parameters != null)
        {
            foreach (var p in parameters)
            {
                sb.Append(' ');
                sb.Append(p);
            }
        }

        if (trailing != null)
        {
            sb.Append(" :");
            sb.Append(trailing);
        }

        return sb.ToString();
    }

    public override string ToString() => Raw;
}
