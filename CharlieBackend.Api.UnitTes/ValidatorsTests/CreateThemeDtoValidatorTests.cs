using CharlieBackend.Api.Validators.StudentGroupsDTOValidators;
using CharlieBackend.Api.Validators.ThemeDTOValidators;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using FluentAssertions;
using System;
using System.Collections.Generic;
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

        public CreateThemeDto Get_UpdateStudentGroupDto(
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
            var theme = Get_UpdateStudentGroupDto(
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
            var theme = Get_UpdateStudentGroupDto();

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
            var theme = Get_UpdateStudentGroupDto(
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