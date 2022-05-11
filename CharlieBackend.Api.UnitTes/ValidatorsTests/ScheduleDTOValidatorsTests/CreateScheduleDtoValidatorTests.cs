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
    public class CreateScheduleDtoValidatorTests : TestBase
    {
        private readonly CreateScheduleDtoValidator _validator;

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
            var dto = GetDTO(ScheduleTestValidationConstants.ValidPattern,
                ScheduleTestValidationConstants.ValidOccurenceRange,
                ScheduleTestValidationConstants.ValidContext);

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
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidPattern,
                ScheduleTestValidationConstants.NotValidOccurenceRange,
                ScheduleTestValidationConstants.NotValidContext);

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
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidPattern,
                ScheduleTestValidationConstants.ValidOccurenceRange,
                ScheduleTestValidationConstants.ValidContext);

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
            var dto = GetDTO(ScheduleTestValidationConstants.ValidPattern,
                ScheduleTestValidationConstants.NotValidOccurenceRange,
                ScheduleTestValidationConstants.ValidContext);

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
            var dto = GetDTO(ScheduleTestValidationConstants.ValidPattern,
                ScheduleTestValidationConstants.ValidOccurenceRange,
                ScheduleTestValidationConstants.NotValidContext);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
