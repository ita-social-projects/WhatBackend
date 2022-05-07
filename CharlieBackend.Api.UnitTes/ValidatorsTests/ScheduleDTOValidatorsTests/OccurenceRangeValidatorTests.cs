using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public class OccurenceRangeValidatorTests : TestBase
    {
        private readonly OccurenceRangeValidator _validator;

        public OccurenceRangeValidatorTests()
        {
            _validator = new OccurenceRangeValidator();
        }

        public OccurenceRange GetDTO(
            DateTime startDate = default,
            DateTime? finishDate = null)
        {
            return new OccurenceRange
            {
                StartDate = startDate,
                FinishDate = finishDate
            };
        }

        [Fact]
        public async Task OccurenceRangeAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidStartDate,
                ScheduleTestValidationConstants.ValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task OccurenceRangeAsync_EmptyData_ShouldReturnFalse()
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
        public async Task OccurenceRangeAsync_EmptyFinishDate_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidStartDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task OccurenceRangeAsync_NotValidFinishDate_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidStartDate,
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
