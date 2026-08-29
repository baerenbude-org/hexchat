using System.Text.RegularExpressions;

namespace HexChat.Core.Formatting;

public readonly record struct IrcColor(byte R, byte G, byte B, byte A = 255)
{
    public static readonly IrcColor White = new(255, 255, 255);
    public static readonly IrcColor Black = new(0, 0, 0);
    public static readonly IrcColor Blue = new(0, 0, 127);
    public static readonly IrcColor Green = new(0, 147, 0);
    public static readonly IrcColor LightRed = new(255, 0, 0);
    public static readonly IrcColor Brown = new(127, 0, 0);
    public static readonly IrcColor Purple = new(156, 0, 156);
    public static readonly IrcColor Orange = new(252, 127, 0);
    public static readonly IrcColor Yellow = new(255, 255, 0);
    public static readonly IrcColor LightGreen = new(0, 252, 0);
    public static readonly IrcColor Cyan = new(0, 147, 147);
    public static readonly IrcColor LightCyan = new(0, 255, 255);
    public static readonly IrcColor LightBlue = new(0, 0, 252);
    public static readonly IrcColor Pink = new(255, 0, 255);
    public static readonly IrcColor Grey = new(127, 127, 127);
    public static readonly IrcColor LightGrey = new(210, 210, 210);

    public static readonly IReadOnlyList<IrcColor> StandardPalette = new[]
    {
        White, Black, Blue, Green, LightRed, Brown, Purple, Orange,
        Yellow, LightGreen, Cyan, LightCyan, LightBlue, Pink, Grey, LightGrey
    };

    public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";
}

public sealed record FormattedSpan(
    string Text,
    IrcColor? Foreground = null,
    IrcColor? Background = null,
    bool IsBold = false,
    bool IsItalic = false,
    bool IsUnderline = false,
    bool IsStrikethrough = false,
    bool IsMonospace = false,
    string? LinkUrl = null
);

/// <summary>
/// High-performance parser for mIRC colors, RGB hex colors, and IRC text formatting codes.
/// </summary>
public static partial class MircColorParser
{
    public const char CodeColor = '\x03';
    public const char CodeHexColor = '\x04';
    public const char CodeBold = '\x02';
    public const char CodeItalic = '\x1D';
    public const char CodeUnderline = '\x1F';
    public const char CodeStrikethrough = '\x1E';
    public const char CodeMonospace = '\x11';
    public const char CodeReverse = '\x16';
    public const char CodeReset = '\x0F';

    [GeneratedRegex(@"(https?://[^\s<>""]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex UrlRegex();

    /// <summary>
    /// Strips all formatting and color codes from a raw IRC text string.
    /// </summary>
    public static string StripFormatting(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var sb = new System.Text.StringBuilder(input.Length);
        int i = 0;
        while (i < input.Length)
        {
            char c = input[i];
            switch (c)
            {
                case CodeColor:
                    i++;
                    // skip foreground
                    while (i < input.Length && char.IsAsciiDigit(input[i])) i++;
                    // skip background
                    if (i < input.Length && input[i] == ',')
                    {
                        i++;
                        while (i < input.Length && char.IsAsciiDigit(input[i])) i++;
                    }
                    break;
                case CodeHexColor:
                    i++;
                    // skip 6 hex digits
                    int hexCount = 0;
                    while (i < input.Length && char.IsAsciiHexDigit(input[i]) && hexCount < 6)
                    {
                        i++;
                        hexCount++;
                    }
                    if (i < input.Length && input[i] == ',')
                    {
                        i++;
                        int bgHexCount = 0;
                        while (i < input.Length && char.IsAsciiHexDigit(input[i]) && bgHexCount < 6)
                        {
                            i++;
                            bgHexCount++;
                        }
                    }
                    break;
                case CodeBold:
                case CodeItalic:
                case CodeUnderline:
                case CodeStrikethrough:
                case CodeMonospace:
                case CodeReverse:
                case CodeReset:
                    i++;
                    break;
                default:
                    sb.Append(c);
                    i++;
                    break;
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Parses formatted IRC text into structured spans with styling and detected URLs.
    /// </summary>
    public static List<FormattedSpan> Parse(string input)
    {
        var result = new List<FormattedSpan>();
        if (string.IsNullOrEmpty(input)) return result;

        bool isBold = false;
        bool isItalic = false;
        bool isUnderline = false;
        bool isStrikethrough = false;
        bool isMonospace = false;
        IrcColor? fg = null;
        IrcColor? bg = null;

        var sb = new System.Text.StringBuilder();

        void FlushCurrentSpan()
        {
            if (sb.Length == 0) return;
            string text = sb.ToString();
            sb.Clear();

            // Link detection within span
            var matches = UrlRegex().Matches(text);
            if (matches.Count == 0)
            {
                result.Add(new FormattedSpan(text, fg, bg, isBold, isItalic, isUnderline, isStrikethrough, isMonospace));
            }
            else
            {
                int lastIdx = 0;
                foreach (Match m in matches)
                {
                    if (m.Index > lastIdx)
                    {
                        result.Add(new FormattedSpan(text[lastIdx..m.Index], fg, bg, isBold, isItalic, isUnderline, isStrikethrough, isMonospace));
                    }
                    result.Add(new FormattedSpan(m.Value, fg ?? IrcColor.LightBlue, bg, isBold, isItalic, true, isStrikethrough, isMonospace, LinkUrl: m.Value));
                    lastIdx = m.Index + m.Length;
                }
                if (lastIdx < text.Length)
                {
                    result.Add(new FormattedSpan(text[lastIdx..], fg, bg, isBold, isItalic, isUnderline, isStrikethrough, isMonospace));
                }
            }
        }

        int i = 0;
        while (i < input.Length)
        {
            char c = input[i];
            switch (c)
            {
                case CodeColor:
                    FlushCurrentSpan();
                    i++;
                    int fgStart = i;
                    while (i < input.Length && char.IsAsciiDigit(input[i]) && i - fgStart < 2) i++;
                    if (i > fgStart && int.TryParse(input[fgStart..i], out int fgCode))
                    {
                        fg = GetPaletteColor(fgCode);
                    }
                    else
                    {
                        fg = null;
                        bg = null;
                    }

                    if (i < input.Length && input[i] == ',')
                    {
                        i++;
                        int bgStart = i;
                        while (i < input.Length && char.IsAsciiDigit(input[i]) && i - bgStart < 2) i++;
                        if (i > bgStart && int.TryParse(input[bgStart..i], out int bgCode))
                        {
                            bg = GetPaletteColor(bgCode);
                        }
                    }
                    break;

                case CodeBold:
                    FlushCurrentSpan();
                    isBold = !isBold;
                    i++;
                    break;
                case CodeItalic:
                    FlushCurrentSpan();
                    isItalic = !isItalic;
                    i++;
                    break;
                case CodeUnderline:
                    FlushCurrentSpan();
                    isUnderline = !isUnderline;
                    i++;
                    break;
                case CodeStrikethrough:
                    FlushCurrentSpan();
                    isStrikethrough = !isStrikethrough;
                    i++;
                    break;
                case CodeMonospace:
                    FlushCurrentSpan();
                    isMonospace = !isMonospace;
                    i++;
                    break;
                case CodeReset:
                    FlushCurrentSpan();
                    isBold = false;
                    isItalic = false;
                    isUnderline = false;
                    isStrikethrough = false;
                    isMonospace = false;
                    fg = null;
                    bg = null;
                    i++;
                    break;

                default:
                    sb.Append(c);
                    i++;
                    break;
            }
        }

        FlushCurrentSpan();
        return result;
    }

    private static IrcColor GetPaletteColor(int code)
    {
        if (code >= 0 && code < IrcColor.StandardPalette.Count)
        {
            return IrcColor.StandardPalette[code];
        }
        // Extended mIRC 99 palette fallback modulo 16
        return IrcColor.StandardPalette[code % 16];
    }
}
