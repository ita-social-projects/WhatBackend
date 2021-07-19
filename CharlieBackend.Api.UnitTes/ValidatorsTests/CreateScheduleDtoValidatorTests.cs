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
    public class CreateScheduleDtoValidatorTests : TestBase
    {
        private CreateScheduleDtoValidator _validator;

        private readonly PatternForCreateScheduleDTO validPattern = new PatternForCreateScheduleDTO 
        {
            Type = PatternType.Weekly,
            Interval = 2,
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday },
            Index = MonthIndex.Fourth,
            Dates = new List<int> {1, 2, 3}
        };
        private readonly OccurenceRange validOccurenceRange = new OccurenceRange
        {
            StartDate = new DateTime(2020, 01, 01),
            FinishDate = new DateTime(2020, 01, 02)
        };
        private readonly ContextForCreateScheduleDTO validContext = new ContextForCreateScheduleDTO
        {
            GroupID = 21,
            ThemeID = 48,
            MentorID = 21
        };
        private readonly PatternForCreateScheduleDTO notValidPattern = new PatternForCreateScheduleDTO
        {
            Type = PatternType.Weekly,
            Index = MonthIndex.Fourth,
            Interval = 2,
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday },
            Dates = new List<int> { 0, 2, 3 }
        };
        private readonly OccurenceRange notValidOccurenceRange = new OccurenceRange
        {
            StartDate = new DateTime(2020, 01, 01),
            FinishDate = new DateTime(2019, 01, 02)
        };
        private readonly ContextForCreateScheduleDTO notValidContext = new ContextForCreateScheduleDTO
        {
            GroupID = 0,
            ThemeID = 48,
            MentorID = 21
        };

        public CreateScheduleDtoValidatorTests()
        {
            _validator = new CreateScheduleDtoValidator();
        }

        public CreateScheduleDto GetDTO(
            PatternForCreateScheduleDTO pattern = null,
            OccurenceRange occurenceRange = null,
            ContextForCreateScheduleDTO context = null)
        {
            return new CreateScheduleDto
            {
                Pattern = pattern,
                Range = occurenceRange,
                Context = context
            };
        }

        [Fact]
        public async Task CreateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validPattern,
                    validOccurenceRange,
                    validContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task CreateScheduleDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task CreateScheduleDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidPattern,
                    notValidOccurenceRange,
                    notValidContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task CreateScheduleDTOAsync_NotValidPattern_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidPattern,
                    validOccurenceRange,
                    validContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task CreateScheduleDTOAsync_NotValidRange_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validPattern,
                    notValidOccurenceRange,
                    validContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task CreateScheduleDTOAsync_NotValidContext_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validPattern,
                    validOccurenceRange,
                    notValidContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
