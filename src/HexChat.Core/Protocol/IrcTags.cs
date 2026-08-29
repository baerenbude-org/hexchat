using System.Text;

namespace HexChat.Core.Protocol;

/// <summary>
/// Constants for standardized IRCv3 message tags and helper functions for escaping/unescaping.
/// </summary>
public static class IrcTags
{
    public const string ServerTime = "time";
    public const string MessageId = "msgid";
    public const string Account = "account";
    public const string Batch = "batch";
    public const string Label = "label";
    public const string Typing = "+typing";
    public const string DraftTyping = "+draft/typing";
    public const string React = "+draft/react";
    public const string Multiline = "+draft/multiline";
    public const string Bot = "bot";

    /// <summary>
    /// Unescapes an IRCv3 tag value according to the IRCv3 message-tags specification.
    /// </summary>
    public static string UnescapeTagValue(ReadOnlySpan<char> escapedValue)
    {
        if (escapedValue.IndexOf('\\') < 0)
        {
            return escapedValue.ToString();
        }

        var sb = new StringBuilder(escapedValue.Length);
        for (int i = 0; i < escapedValue.Length; i++)
        {
            char c = escapedValue[i];
            if (c == '\\' && i + 1 < escapedValue.Length)
            {
                i++;
                char next = escapedValue[i];
                switch (next)
                {
                    case ':':
                        sb.Append(';');
                        break;
                    case 's':
                        sb.Append(' ');
                        break;
                    case '\\':
                        sb.Append('\\');
                        break;
                    case 'r':
                        sb.Append('\r');
                        break;
                    case 'n':
                        sb.Append('\n');
                        break;
                    default:
                        sb.Append(next);
                        break;
                }
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Escapes an IRCv3 tag value according to the IRCv3 message-tags specification.
    /// </summary>
    public static string EscapeTagValue(string rawValue)
    {
        if (string.IsNullOrEmpty(rawValue)) return string.Empty;

        var sb = new StringBuilder(rawValue.Length + 4);
        foreach (char c in rawValue)
        {
            switch (c)
            {
                case ';':
                    sb.Append(@"\:");
                    break;
                case ' ':
                    sb.Append(@"\s");
                    break;
                case '\\':
                    sb.Append(@"\\");
                    break;
                case '\r':
                    sb.Append(@"\r");
                    break;
                case '\n':
                    sb.Append(@"\n");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }
}
