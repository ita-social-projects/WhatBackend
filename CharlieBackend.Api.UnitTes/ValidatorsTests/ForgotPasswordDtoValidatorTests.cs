using CharlieBackend.Api.Validators.AccountDTOValidators;
using CharlieBackend.Core.DTO.Account;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class ForgotPasswordDtoValidatorTests : TestBase
    {
        private ForgotPasswordDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validFormURL = "http://example.org/foo/bar.html";
        private readonly string notValidEmail = "@ValidEmailgmail.com";
        private readonly string notValidFormURL = "http:\\example.org/foo/bar.html";

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
            var dto = GetDTO(
                    validEmail,
                    validFormURL);

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
            var dto = GetDTO(
                    notValidEmail,
                    validFormURL);

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
            var dto = GetDTO(
                    validEmail,
                    notValidFormURL);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}