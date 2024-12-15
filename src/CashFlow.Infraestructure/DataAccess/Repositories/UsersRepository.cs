using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infraestructure.DataAccess.Repositories
{
    public class UsersRepository : IUsersWriteOnlyRepository, IUsersReadOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;
        public UsersRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(User user)
        {
            await _dbContext.User.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.User.AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
