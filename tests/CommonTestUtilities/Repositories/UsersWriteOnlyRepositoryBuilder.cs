using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UsersWriteOnlyRepositoryBuilder
    {
        public static IUsersWriteOnlyRepository Build()
        {
            var moc = new Mock<IUsersWriteOnlyRepository>();
            return moc.Object;
        }
    }
}
