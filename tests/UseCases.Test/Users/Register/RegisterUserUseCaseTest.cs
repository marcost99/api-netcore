﻿using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;
using Xunit;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            
            var useCase = CreateUseCase();

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_REQUIRED));
        }

        [Fact]
        public async Task Error_Email_Exists()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UsersWriteOnlyRepositoryBuilder.Build();
            var passwordEncripter = new PasswordEncrypterBuilder().Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if (string.IsNullOrWhiteSpace(email) == false)
            {
                readRepository.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(readRepository.Build(), mapper, passwordEncripter, writeRepository, unitOfWork, accessTokenGenerator);
        }
    }
}
