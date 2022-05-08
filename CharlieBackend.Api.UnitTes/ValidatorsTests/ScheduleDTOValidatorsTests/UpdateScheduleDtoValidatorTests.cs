using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public class UpdateScheduleDtoValidatorTests : TestBase
    {
        private readonly UpdateScheduleDtoValidator _validator;

        public UpdateScheduleDtoValidatorTests()
        {
            _validator = new UpdateScheduleDtoValidator();
        }

        public UpdateScheduleDto GetDTO(TimeSpan lessonStart = default,
            TimeSpan lessonEnd = default,
            uint? dayNumber = null,
            PatternType repeatRate = PatternType.Daily)
        {
            return new UpdateScheduleDto
            {
                LessonStart = lessonStart,
                LessonEnd = lessonEnd,
                DayNumber = dayNumber,
                RepeatRate = repeatRate
            };
        }

        [Fact]
        public async Task UpdateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidLessonStart,
                ScheduleTestValidationConstants.ValidLessonEnd,
                ScheduleTestValidationConstants.ValidDayNumber,
                ScheduleTestValidationConstants.WeeklyPatternType);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduleDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task UpdateScheduleDTOAsync_ValidDataWithEmptyDayNumber_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidLessonStart,
                ScheduleTestValidationConstants.ValidLessonEnd,
                repeatRate : ScheduleTestValidationConstants.WeeklyPatternType);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduleDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidLessonStart,
                ScheduleTestValidationConstants.NotValidLessonEnd,
                ScheduleTestValidationConstants.NotValidDayNumber,
                ScheduleTestValidationConstants.WeeklyPatternType);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduleDTOAsync_NotValidTimespan_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidLessonStart,
                ScheduleTestValidationConstants.NotValidLessonEnd,
                ScheduleTestValidationConstants.ValidDayNumber,
                ScheduleTestValidationConstants.WeeklyPatternType);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduleDTOAsync_NotValidDayNumber_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidLessonStart,
                ScheduleTestValidationConstants.ValidLessonEnd,
                ScheduleTestValidationConstants.NotValidDayNumber,
                ScheduleTestValidationConstants.WeeklyPatternType);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
