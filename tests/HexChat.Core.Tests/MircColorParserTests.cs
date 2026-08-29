using HexChat.Core.Formatting;
using Xunit;

namespace HexChat.Core.Tests;

public class MircColorParserTests
{
    [Fact]
    public void StripFormatting_RemovesColorsAndStyles()
    {
        string raw = "\u0002Bold \u000304Red\u0003 and \u001DItalic\u001D \u001FUnderline\u001F \u000FPlain";
        string stripped = MircColorParser.StripFormatting(raw);

        Assert.Equal("Bold Red and Italic Underline Plain", stripped);
    }

    [Fact]
    public void Parse_ColorsAndFormatting_ProducesSpans()
    {
        string raw = "\u0002Bold\u0002 and \u000303Green\u000F";
        var spans = MircColorParser.Parse(raw);

        Assert.True(spans.Count >= 3);
        Assert.Equal("Bold", spans[0].Text);
        Assert.True(spans[0].IsBold);

        Assert.Equal(" and ", spans[1].Text);
        Assert.False(spans[1].IsBold);

        Assert.Equal("Green", spans[2].Text);
        Assert.Equal(IrcColor.Green, spans[2].Foreground);
    }

    [Fact]
    public void Parse_Urls_DetectsLinks()
    {
        string raw = "Check out https://hexchat.net for info and https://avaloniaui.net";
        var spans = MircColorParser.Parse(raw);

        var links = spans.FindAll(s => s.LinkUrl != null);
        Assert.Equal(2, links.Count);
        Assert.Equal("https://hexchat.net", links[0].LinkUrl);
        Assert.Equal("https://avaloniaui.net", links[1].LinkUrl);
    }
}
