using CharlieBackend.Api.Validators.HomeworkDTOValidators;
using CharlieBackend.Core.DTO.Homework;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateMarkRequestDtoValidatorTests : TestBase
    {
        private UpdateMarkRequestDtoValidator _validator;
        private readonly long? validStudentHomeworkID = 1;
        private readonly int? validStudentMark = 100;

        private readonly long? notValidStudentHomeworkID = 0;
        private readonly int? notValidStudentMark = 112;

        public UpdateMarkRequestDtoValidatorTests()
        {
            _validator = new UpdateMarkRequestDtoValidator();
        }

        public UpdateMarkRequestDto GetDTO(
            long? studentHomeworkID = null,
            int? studentMark = null)
        {
            return new UpdateMarkRequestDto
            {
                StudentHomeworkId = studentHomeworkID,
                StudentMark = studentMark
            };
        }

        [Fact]
        public async Task UpdateMarkRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validStudentHomeworkID,
                validStudentMark);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateMarkRequestDTOAsync_EmptyData_ShouldReturnTrue()
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
        public async Task UpdateMarkRequestDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidStudentHomeworkID,
                notValidStudentMark);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMarkRequestDTOAsync_NotValidHomeworkID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidStudentHomeworkID,
                validStudentMark);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMarkRequestDTOAsync_NotValidMark_ShouldReturnMark()
        {
            // Arrange
            var dto = GetDTO(
                validStudentHomeworkID,
                notValidStudentMark);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
