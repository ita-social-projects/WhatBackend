using CharlieBackend.Api.Validators.MentorDTOValidators;
using CharlieBackend.Core.DTO.Mentor;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.MentorDtoValidatorsTests
{
    public class UpdateMentorDtoValidatorTests : TestBase
    {
        private readonly UpdateMentorDtoValidator _validator;

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs(),
                TestValidationConstants.GetValidIDs());

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
            var dto = GetDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.NotValidLastName,
                TestValidationConstants.GetNotValidIDs(),
                TestValidationConstants.GetNotValidIDs());

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
            var dto = GetDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs(),
                TestValidationConstants.GetValidIDs());

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs(),
                TestValidationConstants.GetValidIDs());

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.NotValidLastName,
                TestValidationConstants.GetValidIDs(),
                TestValidationConstants.GetValidIDs());

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetNotValidIDs(),
                TestValidationConstants.GetValidIDs());

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
            var dto = GetDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs(),
                TestValidationConstants.GetNotValidIDs());

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
