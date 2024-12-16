using CashFlow.Application.UseCases.Users;
using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Validators.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")]
        [InlineData("AAAAAAAA")]
        [InlineData("Aaaaaaaa")]
        [InlineData("Aaaaaaa1")]
        public void Error_Password_Invalid(string password)
        {
            var validator = new PasswordValidator<RequestRegisterUserJsonBuilder>();

            var result = validator.IsValid(new ValidationContext<RequestRegisterUserJsonBuilder>(new RequestRegisterUserJsonBuilder()), password);

            result.Should().BeFalse();
        }
    }
}
