using CharlieBackend.Api.Validators.HomeworkStudentDTOValidators;
using CharlieBackend.Core.DTO.HomeworkStudent;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class HomeworkStudentRequestDtoValidatorTests : TestBase
    {
        private HomeworkStudentRequestDtoValidator _validator;
        private readonly long validHomeworkId = 21;
        private readonly string validHomeworkText = "Valid homework text - task description, some examples, useful links and so on.";
        private readonly List<long> validAttachmentIds = new List<long>() { 1, 27, 42 };
        private readonly long notValidHomeworkId = 0;
        private readonly string notValidHomeworkText = new string('*', 65536);
        private readonly List<long> notValidAttachmentIds = new List<long>() { 0, 27, 42 };

        public HomeworkStudentRequestDtoValidatorTests()
        {
            _validator = new HomeworkStudentRequestDtoValidator();
        }

        public HomeworkStudentRequestDto GetDTO(
            long homeworkId = 0,
            string homeworkText = null,
            List<long> attachmentIds = null)
        {
            return new HomeworkStudentRequestDto
            {
                HomeworkId = homeworkId,
                HomeworkText = homeworkText,
                AttachmentIds = attachmentIds
            };
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validHomeworkId,
                    validHomeworkText,
                    validAttachmentIds);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task HomeworkStudentRequestDTOAsync_EmptyAttachmentIds_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validHomeworkId,
                    validHomeworkText);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidHomeworkId,
                    notValidHomeworkText,
                    notValidAttachmentIds);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_NotValidHomeworkId_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidHomeworkId,
                    validHomeworkText,
                    validAttachmentIds);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_NotValidHomeworkText_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validHomeworkId,
                    notValidHomeworkText,
                    validAttachmentIds);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task HomeworkStudentRequestDTOAsync_NotValidAttachmentIds_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validHomeworkId,
                    validHomeworkText,
                    notValidAttachmentIds);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
