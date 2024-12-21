using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using Xunit;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();

            request.Email = user.Email;
            
            var useCase = CreateUseCase(user, request.Password);

            var result = await useCase.Execute(request);
            
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_User_Not_Exists()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<UnauthorizedException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.LOGIN_UNAUTHORIZED));
        }

        [Fact]
        public async Task Error_Password_Not_Match()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();

            request.Email = user.Email;

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<UnauthorizedException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.LOGIN_UNAUTHORIZED));
        }

        private DoLoginUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
        {
            var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readRepository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();

            return new DoLoginUseCase(readRepository, passwordEncripter, accessTokenGenerator);
        }
    }
}
