using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class OccurenceRangeValidatorTests : TestBase
    {
        private OccurenceRangeValidator _validator;
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validFinishDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidFinishDate = new DateTime(2019, 01, 01);

        public OccurenceRangeValidatorTests()
        {
            _validator = new OccurenceRangeValidator();
        }

        public OccurenceRange GetDTO(
            DateTime startDate = default(DateTime),
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
            var dto = GetDTO(
                    validStartDate,
                    validFinishDate);

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
            var dto = GetDTO(validStartDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task OccurenceRangeAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidStartDate,
                    notValidFinishDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
