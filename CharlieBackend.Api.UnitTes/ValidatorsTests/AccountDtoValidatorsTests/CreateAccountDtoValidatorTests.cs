using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.AccountDtoValidatorsTests
{
    public class CreateAccountDtoValidatorTests : TestBase
    {
        private readonly CreateAccountDtoValidator _validator;

        public CreateAccountDtoValidatorTests()
        {
            _validator = new CreateAccountDtoValidator();
        }

        public CreateAccountDto GetDTO(
            string email = null,
            string firstName = null,
            string lastName = null,
            string currentPassword = null,
            string confirmPassword = null)
        {
            return new CreateAccountDto
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = currentPassword,
                ConfirmPassword = confirmPassword
            };
        }

        [Fact]
        public async Task CreateAccountDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidName,
                TestAccountValidationConstants.ValidName,
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
        public async Task CreateAccountDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task CreateAccountDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.NotValidEmail,
                TestAccountValidationConstants.ValidName,
                TestAccountValidationConstants.ValidName,
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
        public async Task CreateAccountDTOAsync_NotValidFirstName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.NotValidName,
                TestAccountValidationConstants.ValidName, 
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
        public async Task CreateAccountDTOAsync_NotValidLastName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidName, 
                TestAccountValidationConstants.NotValidName,
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
        public async Task CreateAccountDTOAsync_NotValidPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidName,
                TestAccountValidationConstants.ValidName,
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
        public async Task CreateAccountDTOAsync_NotEqualConfirmPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(TestAccountValidationConstants.ValidEmail,
                TestAccountValidationConstants.ValidName,
                TestAccountValidationConstants.ValidName,
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