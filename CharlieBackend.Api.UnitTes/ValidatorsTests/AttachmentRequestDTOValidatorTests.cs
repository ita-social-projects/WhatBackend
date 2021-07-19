using CharlieBackend.Api.Validators.AttachmentDTOValidators;
using CharlieBackend.Core.DTO.Attachment;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class AttachmentRequestDTOValidatorTests : TestBase
    {
        private AttachmentRequestDTOValidator _validator;
        private readonly long? validMentorID = 21;
        private readonly long? validCourseID = 42;
        private readonly long? validGroupID = 54;
        private readonly long? validStudentAccountID = 81;
        private readonly DateTime? validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime? validEndDate = new DateTime(2020, 01, 01);
        private readonly long? notValidMentorID = 0;
        private readonly long? notValidCourseID = 0;
        private readonly long? notValidGroupID = 0;
        private readonly long? notValidStudentAccountID = 0;
        private readonly DateTime? notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime? notValidEndDate = new DateTime(2019, 01, 01);

        public AttachmentRequestDTOValidatorTests()
        {
            _validator = new AttachmentRequestDTOValidator();
        }

        public AttachmentRequestDto GetDTO(
            long? mentorID = null,
            long? courseID = null,
            long? groupID = null,
            long? studentAccountID = null,
            DateTime? startDate = null,
            DateTime? finishDate = null)
        {
            return new AttachmentRequestDto
            {
               MentorID = mentorID,
               CourseID = courseID,
               GroupID = groupID,
               StudentAccountID = studentAccountID,
               StartDate = startDate,
               FinishDate = finishDate
            };
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorID,
                    validCourseID,
                    validGroupID,
                    validStudentAccountID,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_EmptyData_ShouldReturnTrue()
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
        public async Task AttachmentRequestDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidMentorID,
                    notValidCourseID,
                    notValidGroupID,
                    notValidStudentAccountID,
                    notValidStartDate,
                    notValidEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_NotValidMentorID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidMentorID,
                    validCourseID,
                    validGroupID,
                    validStudentAccountID,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_NotValidCourseID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorID,
                    notValidCourseID,
                    validGroupID,
                    validStudentAccountID,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_NotValidGroupID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorID,
                    validCourseID,
                    notValidGroupID,
                    validStudentAccountID,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_NotValidStudentAccountID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorID,
                    validCourseID,
                    validGroupID,
                    notValidStudentAccountID,
                    validStartDate,
                    validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task AttachmentRequestDTOAsync_NotValidDates_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validMentorID,
                    validCourseID,
                    validGroupID,
                    validStudentAccountID,
                    notValidStartDate,
                    notValidEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
