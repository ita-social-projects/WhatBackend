using CharlieBackend.Api.Validators.CourseDTOValidators;
using CharlieBackend.Core.DTO.Course;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateCourseDTOValidatorTests : TestBase
    {
        private UpdateCourseDTOValidator _validator;
        private readonly string validName = new string('*', 100);
        private readonly string notValidName = new string('*', 101);
        public UpdateCourseDTOValidatorTests()
        {
            _validator = new UpdateCourseDTOValidator();
        }

        public UpdateCourseDto GetDTO(string name = null)
        {
            return new UpdateCourseDto
            {
                Name = name
            };
        }

        [Fact]
        public async Task UpdateCourseDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(validName);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }
        [Fact]
        public async Task UpdateCourseDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task UpdateCourseDTOAsync_NotValidName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(notValidName);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
