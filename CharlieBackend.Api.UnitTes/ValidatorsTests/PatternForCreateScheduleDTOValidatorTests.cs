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
    public class PatternForCreateScheduleDTOValidatorTests : TestBase
    {
        private PatternForCreateScheduleDTOValidator _validator;

        private readonly PatternType type = PatternType.Weekly;
        private readonly List<DayOfWeek> daysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday };
        private readonly MonthIndex? index = MonthIndex.Second;

        private readonly int validInterval = 2;
        private readonly List<int> validDates = new List<int> {19, 23, 26, 30};

        private readonly int notValidInterval = 0;
        private readonly List<int> notValidDatesBelowOne = new List<int> { 0, 23, 26 };
        private readonly List<int> notValidDatesAboveThirtyOne = new List<int> { 19, 23, 36 };

        public PatternForCreateScheduleDTOValidatorTests()
        {
            _validator = new PatternForCreateScheduleDTOValidator();
        }

        public PatternForCreateScheduleDTO GetDTO(
            int interval = 0,
            List<int> dates = null)
        {
            return new PatternForCreateScheduleDTO
            {
                Interval = interval,
                Dates = dates,
                Type = type,
                DaysOfWeek = daysOfWeek,
                Index = index
            };
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validInterval,
                    validDates);

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
            var dto = GetDTO(
                    notValidInterval,
                    validDates);

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
            var dto = GetDTO(
                    validInterval,
                    notValidDatesBelowOne);

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
            var dto = GetDTO(
                    validInterval,
                    notValidDatesAboveThirtyOne);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
