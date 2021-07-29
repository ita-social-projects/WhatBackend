using CharlieBackend.Api.Validators.HomeworkDTOValidators;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Homework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class HomeworkRequestDtoValidatorTests : TestBase
    {
        private HomeworkRequestDtoValidator _validator;

        private readonly DateTime dueDateValue = new DateTime();

        private readonly string validTaskText =  new string('*', ValidationConstants.MaxLengthTaskText);
        private readonly long validLessonID = 1;
        private readonly List<long> validAttachmentIDs = new List<long> { 1, 2, 3, 4, 5 };

        private readonly string notValidTaskText = new string('*', ValidationConstants.MaxLengthTaskText + 1);
        private readonly long notValidLessonID = 0;
        private readonly List<long> notValidAttachmentIDs = new List<long> { 0, 2, 3, 4, 5 };

        public HomeworkRequestDtoValidatorTests()
        {
            _validator = new HomeworkRequestDtoValidator();
        }

        public HomeworkRequestDto GetDTO(
            string taskText = null,
            long lessonID = 0,
            List<long> attachmentIDs = null,
            DateTime? dueDate = null)
        {
            return new HomeworkRequestDto
            {
                DueDate = dueDate,
                TaskText = taskText,
                LessonId = lessonID,
                AttachmentIds = attachmentIDs,
            };
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validTaskText,
                validLessonID,
                validAttachmentIDs,
                dueDateValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task HomeworkRequestDTOAsync_EmptyDueDate_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validTaskText,
                validLessonID,
                validAttachmentIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidTaskText,
                notValidLessonID,
                notValidAttachmentIDs,
                dueDateValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_NotValidTaskText_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidTaskText,
                validLessonID,
                validAttachmentIDs,
                dueDateValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_NotValidLessonId_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validTaskText,
                notValidLessonID,
                validAttachmentIDs,
                dueDateValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkRequestDTOAsync_NotValidAttachmentIDs_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validTaskText,
                validLessonID,
                notValidAttachmentIDs,
                dueDateValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
