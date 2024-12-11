using System.Security.Cryptography;
using System.Text;

namespace Security;

public class TokenGenerator : ITokenGenerator
{
    public const string TOKEN_PREFIX = "ctp_";

    public (string token, string hash) GenerateToken()
    {
        var randomBytes = new byte[30];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var tokenWithoutPrefix = Convert.ToBase64String(randomBytes)
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=", "")
            [..(40 - TOKEN_PREFIX.Length)];

        var token = TOKEN_PREFIX + tokenWithoutPrefix;
        var hash = HashToken(tokenWithoutPrefix);

        return (token, hash);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));

        return Convert.ToBase64String(hashedBytes);
    }
}
