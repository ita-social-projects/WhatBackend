using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public class ChangeCurrentPasswordDtoValidatorTests : TestBase
    {
        private readonly ChangeCurrentPasswordDtoValidator _validator;

        public ChangeCurrentPasswordDtoValidatorTests()
        {
            _validator = new ChangeCurrentPasswordDtoValidator();
        }

        public ChangeCurrentPasswordDto GetDTO(
            string currentPassword = null,
            string newPassword = null,
            string confirmPassword = null)
        {
            return new ChangeCurrentPasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword
            };
        }

        [Fact]
        public async Task ChangeCurrentPasswordDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidPassword,
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
        public async Task ChangeCurrentPasswordDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task ChangeCurrentPasswordDTOAsync_NotValidCurrentPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.NotValidPassword,
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
        public async Task ChangeCurrentPasswordDTOAsync_NotValidNewPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidPassword,
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
        public async Task ChangeCurrentPasswordDTOAsync_NotEqualConfirmPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidPassword,
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
