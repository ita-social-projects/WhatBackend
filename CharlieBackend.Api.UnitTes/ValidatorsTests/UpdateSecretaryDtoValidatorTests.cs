using CharlieBackend.Api.Validators.SecretaryDTOValidators;
using CharlieBackend.Core.DTO.Secretary;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateSecretaryDtoValidatorTests : TestBase
    {
        private UpdateSecretaryDtoValidator _validator;
        private readonly string validEmail = "ValidEmail@gmail.com";
        private readonly string validFirstName = "Validfirstname";
        private readonly string validLastName = "Validlastname";
        private readonly string notValidEmail = "NotValidEmail";
        private readonly string notValidFirstName = "TooLoooooooooooooooongFirstName";
        private readonly string notValidLastName = "TooLooooooooooooooooongLastName";

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
            var secretary = Get_UpdateSecretaryEventDTO(
                    validEmail,
                    validFirstName,
                    validLastName);

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
            var secretary = Get_UpdateSecretaryEventDTO(
                    notValidEmail,
                    notValidFirstName,
                    notValidLastName);

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
            var secretary = Get_UpdateSecretaryEventDTO(
                    notValidEmail,
                    validFirstName,
                    validLastName);

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
            var secretary = Get_UpdateSecretaryEventDTO(
                    validEmail,
                    notValidFirstName,
                    validLastName);

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
            var secretary = Get_UpdateSecretaryEventDTO(
                    validEmail,
                    validFirstName,
                    notValidLastName);

            // Act
            var result = await _validator.ValidateAsync(secretary);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
