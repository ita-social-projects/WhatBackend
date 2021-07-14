using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateScheduleDtoValidatorTests : TestBase
    {
        private UpdateScheduleDtoValidator _validator;

        private readonly PatternType repeatRate = PatternType.Weekly;

        private readonly TimeSpan validLessonStart = new TimeSpan(12,0,0);
        private readonly TimeSpan validLessonEnd = new TimeSpan(14,0,0);
        private readonly uint? validDayNumber = 31;

        private readonly TimeSpan notValidLessonStart = new TimeSpan(14, 30, 0);
        private readonly TimeSpan notValidLessonEnd = new TimeSpan(14, 0, 0);
        private readonly uint? notValidDayNumber = 33;

        public UpdateScheduleDtoValidatorTests()
        {
            _validator = new UpdateScheduleDtoValidator();
        }

        public UpdateScheduleDto GetDTO(
            TimeSpan lessonStart = default(TimeSpan),
            TimeSpan lessonEnd = default(TimeSpan),
            uint? dayNumber = null)
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
            var dto = GetDTO(
                    validLessonStart,
                    validLessonEnd,
                    validDayNumber);

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
            var dto = GetDTO(
                    validLessonStart,
                    validLessonEnd);

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
            var dto = GetDTO(
                    notValidLessonStart,
                    notValidLessonEnd,
                    notValidDayNumber);

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
            var dto = GetDTO(
                    notValidLessonStart,
                    notValidLessonEnd,
                    validDayNumber);

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
            var dto = GetDTO(
                    validLessonStart,
                    validLessonEnd,
                    notValidDayNumber);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
