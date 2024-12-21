namespace CashFlow.Domain.Security.Cryptography
{
    public interface IPasswordEncrypter
    {
        string Encrypty(string password);
        bool Verify(string password, string passwordHash);
    }
}
