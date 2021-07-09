using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class ChangeCurrentPasswordDtoValidatorTests : TestBase
    {
        private ChangeCurrentPasswordDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validPassword = "validPassword_12";
        private readonly string notValidEmail = "@ValidEmailgmail.com";
        private readonly string notValidPassword = "VP_12";
        private readonly string notValidConfirmPassword = "notEqualPassword";

        public ChangeCurrentPasswordDtoValidatorTests()
        {
            _validator = new ChangeCurrentPasswordDtoValidator();
        }

        public ChangeCurrentPasswordDto GetDTO(
            string email = null,
            string currentPassword = null,
            string newPassword = null,
            string confirmPassword = null)
        {
            return new ChangeCurrentPasswordDto
            {
                Email = email,
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword
            };
        }

        [Fact]
        public async Task ChangeCurrentPasswordDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validPassword,
                    validPassword,
                    validPassword);

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
        public async Task ChangeCurrentPasswordDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidEmail,
                    validPassword,
                    validPassword,
                    validPassword);

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
            var dto = GetDTO(
                    validEmail,
                    notValidPassword,
                    validPassword,
                    validPassword);

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
            var dto = GetDTO(
                    validEmail,
                    validPassword,
                    notValidPassword,
                    notValidPassword);

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
            var dto = GetDTO(
                    validEmail,
                    validPassword,
                    validPassword,
                    notValidConfirmPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }


}
