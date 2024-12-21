﻿using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUsersReadOnlyRepository> _repository;
        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUsersReadOnlyRepository>();
        }
        public void ExistActiveUserWithEmail(string email) 
        { 
            _repository.Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }
        public UserReadOnlyRepositoryBuilder GetByEmail(CashFlow.Domain.Entities.User user)
        {
            _repository.Setup(userRepository => userRepository.GetByEmail(user.Email)).ReturnsAsync(user);
            
            return this;
        }
        public IUsersReadOnlyRepository Build() => _repository.Object;
    }
}
