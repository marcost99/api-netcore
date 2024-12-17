using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build() 
        {
            var moc = new Mock<IUnitOfWork>();
            return moc.Object;
        }
    }
}
