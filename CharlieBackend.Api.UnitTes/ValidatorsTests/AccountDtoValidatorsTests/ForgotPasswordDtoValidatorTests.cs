using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public class ForgotPasswordDtoValidatorTests : TestBase
    {
        private readonly ForgotPasswordDtoValidator _validator;

        public ForgotPasswordDtoValidatorTests()
        {
            _validator = new ForgotPasswordDtoValidator();
        }

        public ForgotPasswordDto GetDTO(
            string email = null,
            string formURL = null)
        {
            return new ForgotPasswordDto
            {
                Email = email,
                FormUrl = formURL
            };
        }

        [Fact]
        public async Task ForgotPasswordDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidFormURL);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ForgotPasswordDTOAsync_EmptyData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO();

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ForgotPasswordDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.NotValidEmail,
                TestAccountValidationConstants.ValidFormURL);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ForgotPasswordDTOAsync_NotValidFormURL_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.NotValidFormURL);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}