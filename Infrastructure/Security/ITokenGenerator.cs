namespace Security;

public interface ITokenGenerator
{
    (string token, string hash) GenerateToken();
    string HashToken(string token);
}
