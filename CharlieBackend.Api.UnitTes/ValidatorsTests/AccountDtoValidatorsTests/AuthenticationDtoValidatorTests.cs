using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public class AuthenticationDtoValidatorTests : TestBase
    {
        private AuthenticationDtoValidator _validator;

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
            var dto = GetDTO(TestValidationConstants.ValidEmail, 
                TestValidationConstants.ValidPassword);

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
            var dto = GetDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.ValidPassword);

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.TooShortPassword);

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
            var dto = GetDTO(TestValidationConstants.ValidEmail, 
                TestValidationConstants.TooLongPassword);

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.NoNumbersPassword);

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
            var dto = GetDTO(TestValidationConstants.ValidEmail, 
                TestValidationConstants.NoUpperCasePassword);

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
            var dto = GetDTO(TestValidationConstants.ValidEmail, TestValidationConstants.NoSpecialSymbolsPassword);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
