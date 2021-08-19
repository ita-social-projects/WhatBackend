using CharlieBackend.Api.Validators.HomeworkStudentDTOValidators;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateMarkRequestDtoValidatorTests : TestBase
    {
        private UpdateMarkRequestDtoValidator _validator;
        private readonly long validStudentHomeworkID = 1;
        private readonly int validStudentMark = 100;

        private readonly long notValidStudentHomeworkID = 0;
        private readonly int notValidStudentMark = 112;

        private string mentorComment = "There is an error at line 52";
        private MarkType markType = MarkType.Homework;

        public UpdateMarkRequestDtoValidatorTests()
        {
            _validator = new UpdateMarkRequestDtoValidator();
        }

        [Fact]
        public async Task UpdateMarkRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = new UpdateMarkRequestDto
            { 
                StudentHomeworkId = validStudentHomeworkID,
                StudentMark = validStudentMark,
                MentorComment = mentorComment,
                MarkType = markType
            };

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
            var dto = new UpdateMarkRequestDto
            {
                StudentHomeworkId = notValidStudentHomeworkID,
                StudentMark = notValidStudentMark,
                MentorComment = mentorComment,
                MarkType = markType
            };

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
            var dto = new UpdateMarkRequestDto
            {
                StudentHomeworkId = notValidStudentHomeworkID,
                StudentMark = validStudentMark,
                MentorComment = mentorComment,
                MarkType = markType
            };

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
            var dto = new UpdateMarkRequestDto
            {
                StudentHomeworkId = validStudentHomeworkID,
                StudentMark = notValidStudentMark,
                MentorComment = mentorComment,
                MarkType = markType
            };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
