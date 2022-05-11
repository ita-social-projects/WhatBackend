using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public class ScheduledEventFilterRequestDTOValidatorTests : TestBase
    {
        private readonly ScheduledEventFilterRequestDTOValidator _validator;

        public ScheduledEventFilterRequestDTOValidatorTests()
        {
            _validator = new ScheduledEventFilterRequestDTOValidator();
        }

        public ScheduledEventFilterRequestDTO GetDTO(
            long? courseID = null,
            long? mentorID = null,
            long? groupID = null,
            long? themeID = null,
            long? studentGroupID = null,
            long? eventOccurenceID = null,
            DateTime? startDate = null,
            DateTime? finishDate = null)
        {
            return new ScheduledEventFilterRequestDTO
            {
                CourseID = courseID,
                MentorID = mentorID,
                GroupID = groupID,
                ThemeID = themeID,
                StudentAccountID = studentGroupID,
                EventOccurrenceID = eventOccurenceID,
                StartDate = startDate,
                FinishDate= finishDate
            };
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_EmptyData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO();

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.NotValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidCourseID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidMentorID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidGroupID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidThemeID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidStudentAccountID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidEventOccurence_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ScheduledEventFilterRequestDTOAsync_NotValidDates_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.NotValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
