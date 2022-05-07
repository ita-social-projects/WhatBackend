using CharlieBackend.Api.Validators.StudentDTOValidators;
using CharlieBackend.Core.DTO.Student;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.StudentDTOValidatorsTests
{
    public class UpdateStudentDtoValidatorTests: TestBase
    {
        private readonly UpdateStudentDtoValidator _validator;

        public UpdateStudentDtoValidatorTests()
        {
            _validator = new UpdateStudentDtoValidator();
        }

        public UpdateStudentDto Get_UpdateStudentDTO(
            string email = null,
            string firstName = null,
            string lastName = null,
            IList<long> studentGroupsIds = null)
        {
            return new UpdateStudentDto
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                StudentGroupIds = studentGroupsIds
            };
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_EmptyData_ShouldReturnTrue()
        {
            // Arrange
            var student = Get_UpdateStudentDTO();

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.NotValidLastName,
                TestValidationConstants.GetNotValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidFirstName_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidLastName_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.NotValidLastName,
                TestValidationConstants.GetValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidStudentGroupIds_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName,
                TestValidationConstants.GetNotValidIDs());

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
