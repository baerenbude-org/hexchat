using System.Security.Cryptography;
using System.Text;

namespace HexChat.Core.Security;

/// <summary>
/// Implements standard SCRAM-SHA-256 (RFC 5802 / RFC 7677) client authentication for IRCv3 SASL 3.2.
/// </summary>
public sealed class SaslScramSha256
{
    public const string MechanismName = "SCRAM-SHA-256";

    private readonly string _username;
    private readonly string _password;
    private readonly string _clientNonce;
    private string? _clientFirstMessageBare;
    private string? _authMessage;
    private byte[]? _serverKey;

    public SaslScramSha256(string username, string password, string? clientNonce = null)
    {
        _username = username.Replace("=", "=3D").Replace(",", "=2C");
        _password = password;
        _clientNonce = clientNonce ?? GenerateNonce();
    }

    private static string GenerateNonce()
    {
        byte[] nonceBytes = new byte[18];
        RandomNumberGenerator.Fill(nonceBytes);
        return Convert.ToBase64String(nonceBytes);
    }

    /// <summary>
    /// Step 1: Generates the base64-encoded client-first-message (n,,n=user,r=nonce).
    /// </summary>
    public string GenerateClientFirstMessage()
    {
        _clientFirstMessageBare = $"n={_username},r={_clientNonce}";
        string clientFirstMessage = $"n,,{_clientFirstMessageBare}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(clientFirstMessage));
    }

    /// <summary>
    /// Step 2: Processes the base64-encoded server-first-message and generates the base64-encoded client-final-message.
    /// </summary>
    public string ProcessServerFirstMessage(string serverFirstBase64)
    {
        if (string.IsNullOrEmpty(_clientFirstMessageBare))
        {
            throw new InvalidOperationException("Client-first message must be sent before processing server-first message.");
        }

        string serverFirst = Encoding.UTF8.GetString(Convert.FromBase64String(serverFirstBase64));

        var parts = serverFirst.Split(',');
        string? combinedNonce = null;
        byte[]? salt = null;
        int iterations = 4096;

        foreach (var part in parts)
        {
            if (part.StartsWith("r=")) combinedNonce = part[2..];
            else if (part.StartsWith("s=")) salt = Convert.FromBase64String(part[2..]);
            else if (part.StartsWith("i=") && int.TryParse(part[2..], out int it)) iterations = it;
        }

        if (string.IsNullOrEmpty(combinedNonce) || !combinedNonce.StartsWith(_clientNonce))
        {
            throw new CryptographicException("Server nonce does not match client nonce.");
        }
        if (salt == null)
        {
            throw new CryptographicException("Server did not provide a valid salt.");
        }

        string clientFinalWithoutProof = $"c=biws,r={combinedNonce}";
        _authMessage = $"{_clientFirstMessageBare},{serverFirst},{clientFinalWithoutProof}";

        // SaltedPassword := Hi(Normalize(password), salt, i)
        byte[] saltedPassword = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(_password),
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32);

        // ClientKey := HMAC(SaltedPassword, "Client Key")
        byte[] clientKey = HMACSHA256.HashData(saltedPassword, "Client Key"u8);

        // StoredKey := HASH(ClientKey)
        byte[] storedKey = SHA256.HashData(clientKey);

        // ClientSignature := HMAC(StoredKey, AuthMessage)
        byte[] clientSignature = HMACSHA256.HashData(storedKey, Encoding.UTF8.GetBytes(_authMessage));

        // ClientProof := ClientKey XOR ClientSignature
        byte[] clientProof = new byte[clientKey.Length];
        for (int j = 0; j < clientKey.Length; j++)
        {
            clientProof[j] = (byte)(clientKey[j] ^ clientSignature[j]);
        }

        // ServerKey := HMAC(SaltedPassword, "Server Key")
        _serverKey = HMACSHA256.HashData(saltedPassword, "Server Key"u8);

        string clientFinal = $"{clientFinalWithoutProof},p={Convert.ToBase64String(clientProof)}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(clientFinal));
    }

    /// <summary>
    /// Step 3: Validates the base64-encoded server-final-message (v=ServerSignature).
    /// </summary>
    public bool VerifyServerFinalMessage(string serverFinalBase64)
    {
        if (_serverKey == null || _authMessage == null)
        {
            return false;
        }

        string serverFinal = Encoding.UTF8.GetString(Convert.FromBase64String(serverFinalBase64));
        if (!serverFinal.StartsWith("v=")) return false;

        string serverSignatureBase64 = serverFinal[2..];
        byte[] expectedServerSignature = HMACSHA256.HashData(_serverKey, Encoding.UTF8.GetBytes(_authMessage));

        return Convert.ToBase64String(expectedServerSignature) == serverSignatureBase64;
    }
}
