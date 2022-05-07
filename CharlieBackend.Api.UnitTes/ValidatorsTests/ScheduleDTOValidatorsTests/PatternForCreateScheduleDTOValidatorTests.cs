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
    public class PatternForCreateScheduleDTOValidatorTests : TestBase
    {
        private readonly PatternForCreateScheduleDTOValidator _validator;

        public PatternForCreateScheduleDTOValidatorTests()
        {
            _validator = new PatternForCreateScheduleDTOValidator();
        }

        public PatternForCreateScheduleDTO GetDTO(int interval = 0,
            List<int> dates = null,
            PatternType type = PatternType.Daily,
            IList<DayOfWeek> daysOfWeek = null,
            MonthIndex index = MonthIndex.First)
        {
            return new PatternForCreateScheduleDTO
            {
                Interval = interval,
                Dates = dates,
                Type = type,
                DaysOfWeek = daysOfWeek ?? ScheduleTestValidationConstants.ValidDaysOfWeek,
                Index = index
            };
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidInterval,
                ScheduleTestValidationConstants.ValidDates);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidInterval_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidInterval,
                ScheduleTestValidationConstants.ValidDates);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidDatesBelowOne_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidInterval,
                ScheduleTestValidationConstants.NotValidDatesBelowOne);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidDatesAbovethirtyOne_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidInterval,
                ScheduleTestValidationConstants.NotValidDatesAboveThirtyOne);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }


        // TODO: Add unit tests according new validation rules
    }
}
