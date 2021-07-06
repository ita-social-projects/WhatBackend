using CharlieBackend.Api.Validators.StudentDTOValidators;
using CharlieBackend.Core.DTO.Student;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateStudentDtoValidatorTests: TestBase
    {
        private UpdateStudentDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validFirstName = "Validfirstname";
        private readonly string validLastName = "Validlastname";
        private readonly List<long> validStudentGroupIds = new List<long>() { 1, 2, 36};
        private readonly string notValidEmail = "NotValidEmail";
        private readonly string notValidFirstName = "TooLoooooooooooooooongFirstName";
        private readonly string notValidLastName = "TooLooooooooooooooooongLastName";
        private readonly List<long> notValidStudentGroupIds = new List<long>() { 0, 2, 36 };

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
            var student = Get_UpdateStudentDTO(
                    validEmail,
                    validFirstName,
                    validLastName,
                    validStudentGroupIds);

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
            var student = Get_UpdateStudentDTO(
                    notValidEmail,
                    notValidFirstName,
                    notValidLastName,
                    notValidStudentGroupIds);

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
            var student = Get_UpdateStudentDTO(
                    notValidEmail,
                    validFirstName,
                    validLastName,
                    validStudentGroupIds);

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
            var student = Get_UpdateStudentDTO(
                    validEmail,
                    notValidFirstName,
                    validLastName,
                    validStudentGroupIds);

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
            var student = Get_UpdateStudentDTO(
                    validEmail,
                    validFirstName,
                    notValidLastName,
                    validStudentGroupIds);

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
            var student = Get_UpdateStudentDTO(
                    validEmail,
                    validFirstName,
                    validLastName,
                    notValidStudentGroupIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
