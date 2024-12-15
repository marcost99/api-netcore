namespace CashFlow.Domain.Security.Cryptography
{
    public interface IPasswordEncripter
    {
        string Encrypty(string password);
        bool Verifiy(string password, string passwordHash);
    }
}
