using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class AuthenticationDtoValidatorTests : TestBase
    {
        private AuthenticationDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validPassword = "validPassword_12";
        private readonly string notValidEmail = "@ValidEmailgmail.com";
        private readonly string tooShortPassword = "VP_12";
        private readonly string tooLongPassword = "VeryValidAndEvenMoreEasyToRememberPassword12";
        private readonly string noSpecialSymbolsPassword = "validPassword12";
        private readonly string noNumbersPassword = "validPassword12";
        private readonly string noUpperCasePassword = "validpassword12";

        public AuthenticationDtoValidatorTests()
        {
            _validator = new AuthenticationDtoValidator();
        }

        public AuthenticationDto GetDTO(
            string email = null,
            string password = null)
        {
            return new AuthenticationDto
            {
                Email = email,
                Password = password
            };
        }

        [Fact]
        public async Task AuthenticationDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task AuthenticationDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task AuthenticationDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidEmail,
                    validPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AuthenticationDTOAsync_TooShortPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    tooShortPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AuthenticationDTOAsync_TooLongPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    tooLongPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
        [Fact]
        public async Task AuthenticationDTOAsync_NoNumbersPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    noNumbersPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AuthenticationDTOAsync_NoUppercasePassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    noUpperCasePassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AuthenticationDTOAsync_NoSpecialSymbolsPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    noSpecialSymbolsPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
