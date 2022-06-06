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
    public class UpdateLessonDtoValidatorTests : TestBase
    {
        private UpdateLessonDtoValidator _validator;

        private readonly string validThemeName = "Valid Theme Name";
        private readonly List<VisitDto> validLessonVisits = new List<VisitDto>
        {
            new VisitDto { StudentId = 1, StudentMark = 100, Presence = true, Comment = "Some comment" },
            new VisitDto { StudentId = 21, StudentMark = 100, Presence = false, Comment = "Some comment"},
            new VisitDto { StudentId = 22, StudentMark = 11, Presence = true, Comment = "Some comment" }
        };
        private readonly DateTime validLessonDate = new DateTime(2020, 01, 01);

        private readonly string notValidThemeName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";
        private readonly List<VisitDto> notValidLessonVisits = new List<VisitDto>
        {
            new VisitDto { StudentId = 0, StudentMark = 0, Presence = true, Comment = "Some comment" },
            new VisitDto { StudentId = 21, StudentMark = 101, Presence = false, Comment = "Some comment"},
            new VisitDto { StudentId = 22, StudentMark = 11, Presence = true, Comment = "Some comment" },
            null
        };
        private readonly DateTime notValidLessonDate = default(DateTime);

        public UpdateLessonDtoValidatorTests()
        {
            _validator = new UpdateLessonDtoValidator();
        }

        public UpdateLessonDto GetDTO(
            string themeName = null,
            List<VisitDto> lessonVisits = null,
            DateTime lessonDate = default(DateTime))
        {
            return new UpdateLessonDto
            {
                ThemeName = themeName,
                LessonVisits = lessonVisits,
                LessonDate = lessonDate
            };
        }

        [Fact]
        public async Task UpdateLessonDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validThemeName,
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
        public async Task UpdateLessonDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task UpdateLessonDTOAsync_EmptyDataExeptLessonDate_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(null, null, validLessonDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateLessonDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidThemeName,
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
        public async Task UpdateLessonDTOAsync_NotValidThemeName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidThemeName,
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
        public async Task UpdateLessonDTOAsync_NotValidLessonVisits_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validThemeName,
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
        public async Task UpdateLessonDTOAsync_NotValidLessonDate_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validThemeName,
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