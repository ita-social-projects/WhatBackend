using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class CreateAccountDtoValidatorTests : TestBase
    {
        private CreateAccountDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validName = "Validname";
        private readonly string validPassword = "validPassword_12";
        private readonly string notValidEmail = "@ValidEmailgmail.com";
        private readonly string notValidName = "TooLoooooooooooooooooooooooooooooooongname";
        private readonly string notValidPassword = "VP_12";
        private readonly string notValidConfirmPassword = "notEqualPassword";

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
            var dto = GetDTO(
                    validEmail,
                    validName,
                    validName,
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
            var dto = GetDTO(
                    notValidEmail,
                    validName,
                    validName,
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
        public async Task CreateAccountDTOAsync_NotValidFirstName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    notValidName,
                    validName,
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
        public async Task CreateAccountDTOAsync_NotValidLastName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validName,
                    notValidName,
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
        public async Task CreateAccountDTOAsync_NotValidPasswords_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validName,
                    validName,
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
        public async Task CreateAccountDTOAsync_NotEqualConfirmPassword_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validName,
                    validName,
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