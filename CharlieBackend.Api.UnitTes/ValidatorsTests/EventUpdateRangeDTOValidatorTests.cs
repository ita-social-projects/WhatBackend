using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class EventUpdateRangeDTOValidatorTests : TestBase
    {
        private EventUpdateRangeDTOValidator _validator;

        private readonly ScheduledEventFilterRequestDTO validFilter = new ScheduledEventFilterRequestDTO
        {
            CourseID = 21,
            MentorID = 21,
            GroupID = 21,
            ThemeID = 21,
            StudentAccountID = 21,
            EventOccurrenceID = 21,
            StartDate = new DateTime(2020, 1, 1),
            FinishDate = new DateTime(2020, 1, 2)
        };
        private readonly UpdateScheduledEventDto validRequest = new UpdateScheduledEventDto
        {
            StudentGroupId = 21,
            ThemeId = 21,
            MentorId = 21,
            EventStart = new DateTime(2012, 1, 1),
            EventEnd = new DateTime(2020, 1, 1)
        };

        private readonly ScheduledEventFilterRequestDTO notValidFilter = new ScheduledEventFilterRequestDTO
        {
            CourseID = 21,
            MentorID = 21,
            GroupID = 21,
            ThemeID = 0,
            StudentAccountID = 21,
            EventOccurrenceID = 21,
            StartDate = new DateTime(2020, 1, 1),
            FinishDate = new DateTime(2020, 1, 2)
        };
        private readonly UpdateScheduledEventDto notValidRequest = new UpdateScheduledEventDto
        {
            StudentGroupId = 21,
            ThemeId = 21,
            MentorId = 21,
            EventStart = new DateTime(2021, 1, 1),
            EventEnd = new DateTime(2020, 1, 1)
        };


        public EventUpdateRangeDTOValidatorTests()
        {
            _validator = new EventUpdateRangeDTOValidator();
        }

        public EventUpdateRangeDTO GetDTO(
            ScheduledEventFilterRequestDTO filter = null,
            UpdateScheduledEventDto request = null)
        {
            return new EventUpdateRangeDTO
            {
                Filter = filter,
                Request = request
            };
        }

        [Fact]
        public async Task EventUpdateRangeDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validFilter,
                validRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task EventUpdateRangeDTOAsync_EmptyData_ShouldReturnTrue()
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
        public async Task EventUpdateRangeDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidFilter,
                notValidRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task EventUpdateRangeDTOAsync_NotValidFilter_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidFilter,
                validRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task EventUpdateRangeDTOAsync_NotValidRequest_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validFilter,
                notValidRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}