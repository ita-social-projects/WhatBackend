using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public class ResetPasswordDtoValidatorTests : TestBase
    {
        private readonly ResetPasswordDtoValidator _validator;

        public ResetPasswordDtoValidatorTests()
        {
            _validator = new ResetPasswordDtoValidator();
        }

        public ResetPasswordDto GetDTO(
            string email = null,
            string newPassword = null,
            string confirmPassword = null)
        {
            return new ResetPasswordDto
            {
                Email = email,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword
            };
        }

        [Fact]
        public async Task ResetPasswordDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidPassword,
                TestAccountValidationConstants.ValidPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ResetPasswordDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task ResetPasswordDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.NotValidEmail,
                TestAccountValidationConstants.ValidPassword,
                TestAccountValidationConstants.ValidPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ResetPasswordDTOAsync_NotValidPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.NotValidPassword,
                TestAccountValidationConstants.NotValidPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ResetPasswordDTOAsync_NotEqualPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidPassword,
                TestAccountValidationConstants.NotValidConfirmPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}