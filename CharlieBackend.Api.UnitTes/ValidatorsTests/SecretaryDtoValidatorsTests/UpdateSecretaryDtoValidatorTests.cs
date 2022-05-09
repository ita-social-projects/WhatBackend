using CharlieBackend.Api.Validators.SecretaryDTOValidators;
using CharlieBackend.Core.DTO.Secretary;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.SecretaryDtoValidatorsTests
{
    public class UpdateSecretaryDtoValidatorTests : TestBase
    {
        private readonly UpdateSecretaryDtoValidator _validator;

        public UpdateSecretaryDtoValidatorTests()
        {
            _validator = new UpdateSecretaryDtoValidator();
        }

        public UpdateSecretaryDto Get_UpdateSecretaryEventDTO(
            string email = null,
            string firstName = null,
            string lastName = null)
        {
            return new UpdateSecretaryDto
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        [Fact]
        public async Task UpdateSecretaryDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var secretary = Get_UpdateSecretaryEventDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateSecretaryDTOAsync_EmptyData_ShouldReturnTrue()
        {
            // Arrange
            var schedule = Get_UpdateSecretaryEventDTO();

            // Act
            var result = await _validator.ValidateAsync(schedule);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateSecretaryDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var secretary = Get_UpdateSecretaryEventDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.NotValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateSecretaryEventDTOAsync_NotValidEmail_ShouldReturnFalse()
        {
            // Arrange
            var secretary = Get_UpdateSecretaryEventDTO(TestValidationConstants.NotValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.ValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateSecretaryEventDTOAsync_NotValidFirstName_ShouldReturnFalse()
        {
            // Arrange
            var secretary = Get_UpdateSecretaryEventDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.NotValidFirstName,
                TestValidationConstants.ValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateSecretaryEventDTOAsync_NotValidLastName_ShouldReturnFalse()
        {
            // Arrange
            var secretary = Get_UpdateSecretaryEventDTO(TestValidationConstants.ValidEmail,
                TestValidationConstants.ValidFirstName,
                TestValidationConstants.NotValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
