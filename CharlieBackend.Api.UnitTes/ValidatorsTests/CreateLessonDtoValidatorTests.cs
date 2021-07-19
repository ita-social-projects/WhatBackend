using CharlieBackend.Api.Validators.LessonDTOValidators;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class CreateLessonDtoValidatorTests : TestBase
    {
        private CreateLessonDtoValidator _validator;

        private readonly string validThemeName = "Valid Theme Name";
        private readonly long validMentorId = 42;
        private readonly long validStudentGroupId = 21;
        private readonly List<VisitDto> validLessonVisits = new List<VisitDto>
        {
            new VisitDto { StudentId = 1, StudentMark = 100, Presence = true, Comment = "Some comment" },
            new VisitDto { StudentId = 21, StudentMark = 100, Presence = false, Comment = "Some comment"},
            new VisitDto { StudentId = 22, StudentMark = 11, Presence = true, Comment = "Some comment" }
        };
        private readonly DateTime validLessonDate = new DateTime(2020, 01, 01);

        private readonly string notValidThemeName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";
        private readonly long notValidMentorId = 0;
        private readonly long notValidStudentGroupId = 0;
        private readonly List<VisitDto> notValidLessonVisits = new List<VisitDto>
        {
            new VisitDto { StudentId = 0, StudentMark = 0, Presence = true, Comment = "Some comment" },
            new VisitDto { StudentId = 21, StudentMark = 101, Presence = false, Comment = "Some comment"},
            new VisitDto { StudentId = 22, StudentMark = 11, Presence = true, Comment = "Some comment" },
            null
        };
        private readonly DateTime notValidLessonDate = default(DateTime);

        public CreateLessonDtoValidatorTests()
        {
            _validator = new CreateLessonDtoValidator();
        }

        public CreateLessonDto GetDTO(
            string themeName = null,
            long mentorId = 0,
            long studentgroupId = 0,
            List<VisitDto> lessonVisits = null,
            DateTime lessonDate = default(DateTime))
        {
            return new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentgroupId,
                LessonVisits = lessonVisits,
                LessonDate = lessonDate
            };
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
                    validMentorId,
                    validStudentGroupId,
                    validLessonVisits,
                    validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task AssignMentorToLessonDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidThemeName,
                    notValidMentorId,
                    notValidStudentGroupId,
                    notValidLessonVisits,
                    notValidLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidThemeName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidThemeName,
                    validMentorId,
                    validStudentGroupId,
                    validLessonVisits,
                    validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidMentorId_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
                    notValidMentorId,
                    validStudentGroupId,
                    validLessonVisits,
                    validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidStudentGroupId_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
                    validMentorId,
                    notValidStudentGroupId,
                    validLessonVisits,
                    validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidLessonVisits_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
                    validMentorId,
                    validStudentGroupId,
                    notValidLessonVisits,
                    validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AssignMentorToLessonDTOAsync_NotValidLessonDate_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
                    validMentorId,
                    validStudentGroupId,
                    validLessonVisits,
                    notValidLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
