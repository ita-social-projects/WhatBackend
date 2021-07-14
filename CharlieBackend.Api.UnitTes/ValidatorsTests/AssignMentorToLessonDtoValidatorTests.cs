using CharlieBackend.Api.Validators.LessonDTOValidators;
using CharlieBackend.Core.DTO.Lesson;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class AssignMentorToLessonDtoValidatorTests : TestBase
    {
        private AssignMentorToLessonDtoValidator _validator;
        private readonly long validMentorId = 70;
        private readonly long validLessonId = 42;
        private readonly long notValidMentorId = 0;
        private readonly long notValidLessonId = 0;

        public AssignMentorToLessonDtoValidatorTests()
        {
            _validator = new AssignMentorToLessonDtoValidator();
        }

        public AssignMentorToLessonDto GetDTO(
            long mentorId = 0,
            long lessonId = 0)
        {
            return new AssignMentorToLessonDto
            {
                MentorId = mentorId,
                LessonId = lessonId
            };
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorId,
                    validLessonId);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task AssignMentorToLessonDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidMentorId,
                    notValidLessonId);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidMentorID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidMentorId,
                    validLessonId);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidLessonID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorId,
                    notValidLessonId);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}