using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            mock.Setup(passwordEncripter => passwordEncripter.Encrypty(It.IsAny<string>())).Returns("!1Aaaaaa");

            return mock.Object;
        }
    }
}
