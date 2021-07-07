using CharlieBackend.Api.Validators.ThemeDTOValidators;
using CharlieBackend.Core.DTO.Theme;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateThemeDtoValidatorTests : TestBase
    {
        private UpdateThemeDtoValidator _validator;

        private readonly string validName = "ValidName";
        private readonly string notValidName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";

        public UpdateThemeDtoValidatorTests()
        {
            _validator = new UpdateThemeDtoValidator();
        }

        public UpdateThemeDto Get_UpdateThemeDto(
            string name = null)
        {
            return new UpdateThemeDto
            {
                Name = name
            };
        }

        [Fact]
        public async Task UpdateThemeDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var theme = Get_UpdateThemeDto(
                    validName);

            // Act
            var result = await _validator.ValidateAsync(theme);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateThemeDTOAsync_EmptyData_ShouldReturnFalse()
        {
            // Arrange
            var theme = Get_UpdateThemeDto();

            // Act
            var result = await _validator.ValidateAsync(theme);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateThemeDTOAsync_notValidData_ShouldReturnFalse()
        {
            // Arrange
            var theme = Get_UpdateThemeDto(
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