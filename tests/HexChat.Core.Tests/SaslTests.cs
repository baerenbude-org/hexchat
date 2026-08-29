using System.Text;
using HexChat.Core.Security;
using Xunit;

namespace HexChat.Core.Tests;

public class SaslTests
{
    [Fact]
    public void SaslPlain_GeneratesCorrectBase64()
    {
        string username = "testuser";
        string password = "secretpassword";

        string payloadBase64 = SaslPlain.GeneratePayload(username, password);
        byte[] decoded = Convert.FromBase64String(payloadBase64);

        // format: \0username\0password
        string expectedRaw = $"\0{username}\0{password}";
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expectedRaw);

        Assert.Equal(expectedBytes, decoded);
    }

    [Fact]
    public void SaslScramSha256_GeneratesValidClientFirstMessage()
    {
        var scram = new SaslScramSha256("user", "pencil", "fyko+d2lbbFgONRv9qkxdawL");
        string firstMsgBase64 = scram.GenerateClientFirstMessage();
        string firstMsg = Encoding.UTF8.GetString(Convert.FromBase64String(firstMsgBase64));

        Assert.Equal("n,,n=user,r=fyko+d2lbbFgONRv9qkxdawL", firstMsg);
    }
}
