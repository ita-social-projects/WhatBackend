using CharlieBackend.Api.Validators.MentorDTOValidators;
using CharlieBackend.Core.DTO.Mentor;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateMentorDtoValidatorTests : TestBase
    {
        private UpdateMentorDtoValidator _validator;

        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validFirstName = "Validfirstname";
        private readonly string validLastName = "Validlastname";
        private readonly List<long> validCourseIDs = new List<long> {1, 21, 30, 42, 54, 73 };
        private readonly List<long> validStudentGroupIDs = new List<long> { 1, 21, 30, 42, 54, 73 };

        private readonly string notValidEmail = "NOTValidEmail";
        private readonly string notValidFirstName = "TooLooooooooooooooooooooongName";
        private readonly string notValidLastName = "TooLooooooooooooooooooooongName";
        private readonly List<long> notValidCourseIDs = new List<long> { 0, 21, 30, 42, 54, 70 };
        private readonly List<long> notValidStudentGroupIDs = new List<long> { 0, 21, 30, 42, 54, 73 };

        public UpdateMentorDtoValidatorTests()
        {
            _validator = new UpdateMentorDtoValidator();
        }

        public UpdateMentorDto GetDTO(
            string email = null,
            string firstName = null,
            string lastName = null,
            List<long> courseIDs = null,
            List<long> studentGroupIDs = null)
        {
            return new UpdateMentorDto
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CourseIds = courseIDs,
                StudentGroupIds = studentGroupIDs
            };
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validFirstName,
                    validLastName,
                    validCourseIDs,
                    validStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_EmptyData_ShouldReturnTrue()
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
        public async Task UpdateMentorDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidEmail,
                    notValidFirstName,
                    notValidLastName,
                    notValidCourseIDs,
                    notValidStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    notValidEmail,
                    validFirstName,
                    validLastName,
                    validCourseIDs,
                    validStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_NotValidFirstName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    notValidFirstName,
                    validLastName,
                    validCourseIDs,
                    validStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_NotValidLastName_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validFirstName,
                    notValidLastName,
                    validCourseIDs,
                    validStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_NotValidCourseIDs_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validFirstName,
                    validLastName,
                    notValidCourseIDs,
                    validStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateMentorDTOAsync_NotValidStudentGroupsIDs_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                    validEmail,
                    validFirstName,
                    validLastName,
                    validCourseIDs,
                    notValidStudentGroupIDs);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
