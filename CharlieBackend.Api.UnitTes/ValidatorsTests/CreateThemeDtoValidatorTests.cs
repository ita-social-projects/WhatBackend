using CharlieBackend.Api.Validators.ThemeDTOValidators;
using CharlieBackend.Core.DTO.Theme;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class CreateThemeDtoValidatorTests : TestBase
    {
        private CreateThemeDtoValidator _validator;

        private readonly string validName = "ValidName";
        private readonly string notValidName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";

        public CreateThemeDtoValidatorTests()
        {
            _validator = new CreateThemeDtoValidator();
        }

        public CreateThemeDto Get_CreateThemeDto(
            string name = null)
        {
            return new CreateThemeDto
            {
                Name = name
            };
        }

        [Fact]
        public async Task CreateThemeDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var theme = Get_CreateThemeDto(
                    validName);

            // Act
            var result = await _validator.ValidateAsync(theme);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task CreateThemeDTOAsync_EmptyData_ShouldReturnFalse()
        {
            // Arrange
            var theme = Get_CreateThemeDto();

            // Act
            var result = await _validator.ValidateAsync(theme);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task CreateThemeDTOAsync_notValidData_ShouldReturnFalse()
        {
            // Arrange
            var theme = Get_CreateThemeDto(
                    notValidName);

            // Act
            var result = await _validator.ValidateAsync(theme);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}