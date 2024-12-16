using CashFlow.Application.UseCases.Expenses;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions; //library that verifies the results of validations
using Xunit;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new ExpenseValidator(); //initializes the class of validation
            var request = RequestRegisterExpenseJsonBuilder.Build(); //simulates a request

            // Act
            var result = validator.Validate(request); //validates the datas of request

            // Assert
            result.IsValid.Should().BeTrue(); //verifies the results of validations
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Error_Title_Empty(string title)
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = title;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
        }
    }
}
