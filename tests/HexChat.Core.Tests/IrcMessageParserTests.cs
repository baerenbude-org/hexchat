using HexChat.Core.Protocol;
using Xunit;

namespace HexChat.Core.Tests;

public class IrcMessageParserTests
{
    [Fact]
    public void Parse_SimplePing_ReturnsCorrectCommand()
    {
        string raw = "PING :irc.libera.chat";
        var msg = IrcMessage.Parse(raw);

        Assert.Equal("PING", msg.Command);
        Assert.Equal("irc.libera.chat", msg.Trailing);
        Assert.Null(msg.Prefix);
        Assert.Empty(msg.Tags);
    }

    [Fact]
    public void Parse_StandardPrivmsg_ParsesPrefixAndParams()
    {
        string raw = ":alice!alice@user/alice PRIVMSG #hexchat :Hello, everyone!";
        var msg = IrcMessage.Parse(raw);

        Assert.Equal("alice!alice@user/alice", msg.Prefix);
        Assert.Equal("alice", msg.Nick);
        Assert.Equal("alice", msg.User);
        Assert.Equal("user/alice", msg.Host);
        Assert.Equal("PRIVMSG", msg.Command);
        Assert.Single(msg.Parameters);
        Assert.Equal("#hexchat", msg.Parameters[0]);
        Assert.Equal("Hello, everyone!", msg.Trailing);
    }

    [Fact]
    public void Parse_NumericReply_ParsesNumericCode()
    {
        string raw = ":irc.example.com 001 TestNick :Welcome to the IRC Network TestNick";
        var msg = IrcMessage.Parse(raw);

        Assert.Equal("001", msg.Command);
        Assert.Equal(1, msg.Numeric);
        Assert.Equal("irc.example.com", msg.Prefix);
        Assert.Equal("TestNick", msg.Parameters[0]);
        Assert.Equal("Welcome to the IRC Network TestNick", msg.Trailing);
    }

    [Fact]
    public void Parse_Ircv3MessageWithTags_ParsesAllTagsCorrectly()
    {
        string raw = @"@time=2026-08-29T00:00:00.000Z;msgid=abc-123;account=alice;+typing=active :alice!alice@host PRIVMSG #hexchat :Typing message";
        var msg = IrcMessage.Parse(raw);

        Assert.Equal(4, msg.Tags.Count);
        Assert.Equal("2026-08-29T00:00:00.000Z", msg.Tags["time"]);
        Assert.Equal("abc-123", msg.Tags["msgid"]);
        Assert.Equal("alice", msg.Tags["account"]);
        Assert.Equal("active", msg.Tags["+typing"]);

        Assert.NotNull(msg.ServerTime);
        Assert.Equal(2026, msg.ServerTime.Value.Year);

        Assert.Equal("PRIVMSG", msg.Command);
        Assert.Equal("#hexchat", msg.Parameters[0]);
        Assert.Equal("Typing message", msg.Trailing);
    }

    [Fact]
    public void Parse_EscapedTags_UnescapesValuesCorrectly()
    {
        string raw = @"@tag1=hello\sworld;tag2=semicolon\:and\\backslash :server NOTICE * :Text";
        var msg = IrcMessage.Parse(raw);

        Assert.Equal("hello world", msg.Tags["tag1"]);
        Assert.Equal("semicolon;and\\backslash", msg.Tags["tag2"]);
    }

    [Fact]
    public void Format_ConstructsValidWireString()
    {
        var tags = new Dictionary<string, string> { { "label", "xyz" } };
        string formatted = IrcMessage.Format("PRIVMSG", new[] { "#hexchat" }, "Hello world", tags);

        Assert.Equal("@label=xyz PRIVMSG #hexchat :Hello world", formatted);
    }
}
