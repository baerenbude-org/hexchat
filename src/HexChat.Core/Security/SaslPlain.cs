using System.Text;

namespace HexChat.Core.Security;

/// <summary>
/// Implements the SASL PLAIN authentication mechanism.
/// Payload format: \0authcid\0password or authzid\0authcid\0password
/// </summary>
public static class SaslPlain
{
    public const string MechanismName = "PLAIN";

    public static string GeneratePayload(string username, string password, string? authzId = null)
    {
        ArgumentNullException.ThrowIfNull(username);
        ArgumentNullException.ThrowIfNull(password);

        string zId = authzId ?? string.Empty;
        var bytes = new List<byte>();

        bytes.AddRange(Encoding.UTF8.GetBytes(zId));
        bytes.Add(0);
        bytes.AddRange(Encoding.UTF8.GetBytes(username));
        bytes.Add(0);
        bytes.AddRange(Encoding.UTF8.GetBytes(password));

        return Convert.ToBase64String(bytes.ToArray());
    }
}
