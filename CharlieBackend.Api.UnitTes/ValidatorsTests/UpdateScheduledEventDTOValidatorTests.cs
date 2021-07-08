using CharlieBackend.Api.Validators.ScheduledEventDTOValidators;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateScheduledEventDTOValidatorTests : TestBase
    {
        private UpdateScheduledEventDTOValidator _validator;
        private readonly long validStudentGroupId = 1;
        private readonly long validMentorId = 1;
        private readonly long validThemeId = 1;
        private readonly long notValidStudentGroupId = 0;
        private readonly long notValidMentorId = 0;
        private readonly long notValidThemeId = 0;
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validEndDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidEndDate = new DateTime(2019, 01, 01);
        
        public UpdateScheduledEventDTOValidatorTests()
        {
            _validator = new UpdateScheduledEventDTOValidator();
        }

        public UpdateScheduledEventDto Get_UpdateScheduledEventDTO(
            long? mentorId = null,
            long? studentGroupId = null,
            long? themeId = null,
            DateTime? eventStart = null,
            DateTime? eventEnd = null)
        {
            return new UpdateScheduledEventDto
            {
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                ThemeId = themeId,
                EventStart = eventStart,
                EventEnd = eventEnd
            };
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_ValidDataWithoutDates_ShouldReturnTrue()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_EmptyData_ShouldReturnTrue()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO();

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    notValidStudentGroupId,
                    notValidMentorId,
                    notValidThemeId,
                    notValidStartDate,
                    notValidEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_NotValidMentor_ShouldReturnFalse()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    notValidMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_NotValidTheme_ShouldReturnFalse()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    notValidThemeId,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_NotValidGroup_ShouldReturnFalse()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    notValidStudentGroupId,
                    validMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_NotValidDatesRange_ShouldReturnFalse()
        {
            // Arrange
            var schedule = Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId,
                    notValidStartDate,
                    notValidEndDate);

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
