using CharlieBackend.Api.Validators.Schedule;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public class EventUpdateRangeDTOValidatorTests : TestBase
    {
        private readonly EventUpdateRangeDTOValidator _validator;

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
            var dto = GetDTO(ScheduleTestValidationConstants.ValidFilter,
                ScheduleTestValidationConstants.ValidRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task EventUpdateRangeDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task EventUpdateRangeDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidFilter,
                ScheduleTestValidationConstants.NotValidRequest);

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
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidFilter,
                ScheduleTestValidationConstants.ValidRequest);

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
            var dto = GetDTO(ScheduleTestValidationConstants.ValidFilter,
                ScheduleTestValidationConstants.NotValidRequest);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}