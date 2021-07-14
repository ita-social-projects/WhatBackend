using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class ResetPasswordDtoValidatorTests : TestBase
    {
        private ResetPasswordDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validPassword = "validPassword_12";
        private readonly string notValidEmail = "@ValidEmailgmail.com";
        private readonly string notValidPassword = "VP_12";
        private readonly string notValidConfirmPassword = "notEqualPassword";

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
            var dto = GetDTO(
                    validEmail,
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
            var dto = GetDTO(
                    notValidEmail,
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
        public async Task ResetPasswordDTOAsync_NotValidPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
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
        public async Task ResetPasswordDTOAsync_NotEqualPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
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