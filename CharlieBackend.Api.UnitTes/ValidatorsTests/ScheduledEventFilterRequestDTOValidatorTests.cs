using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class ScheduledEventFilterRequestDTOValidatorTests : TestBase
    {
        private ScheduledEventFilterRequestDTOValidator _validator;

        private readonly long? validCourseID = 1;
        private readonly long? validMentorID = 1;
        private readonly long? validGroupID = 1;
        private readonly long? validThemeID = 1;
        private readonly long? validStudentAccountID = 1;
        private readonly long? validEventOccurenceID = 1;
        private readonly DateTime? validStartDate = new DateTime(2020, 1, 1 );
        private readonly DateTime? validFinishtDate = new DateTime(2020, 1, 2);
        private readonly long? notValidCourseID = 0;
        private readonly long? notValidMentorID = 0;
        private readonly long? notValidGroupID = 0;
        private readonly long? notValidThemeID = 0;
        private readonly long? notValidStudentAccountID = 0;
        private readonly long? notValidEventOccurenceID = 0;
        private readonly DateTime? notValidStartDate = new DateTime(2021, 1, 1);
        private readonly DateTime? notValidFinishtDate = new DateTime(2020, 1, 2);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                validGroupID,
                validThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                notValidCourseID,
                notValidMentorID,
                notValidGroupID,
                notValidThemeID,
                notValidStudentAccountID,
                notValidEventOccurenceID,
                notValidStartDate,
                notValidFinishtDate);

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
            var dto = GetDTO(
                notValidCourseID,
                validMentorID,
                validGroupID,
                validThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                notValidMentorID,
                validGroupID,
                validThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                notValidGroupID,
                validThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                validGroupID,
                notValidThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                validGroupID,
                validThemeID,
                notValidStudentAccountID,
                validEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                validGroupID,
                validThemeID,
                validStudentAccountID,
                notValidEventOccurenceID,
                validStartDate,
                validFinishtDate);

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
            var dto = GetDTO(
                validCourseID,
                validMentorID,
                validGroupID,
                validThemeID,
                validStudentAccountID,
                validEventOccurenceID,
                notValidStartDate,
                notValidFinishtDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
