using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public class ContextForCreateScheduleDTOValidatorTests : TestBase
    {
        private readonly ContextForCreateScheduleDTOValidator _validator;

        public ContextForCreateScheduleDTOValidatorTests()
        {
            _validator = new ContextForCreateScheduleDTOValidator();
        }

        public ContextForCreateScheduleDTO GetDTO(
            long groupId = 0,
            long? themeId = null,
            long? mentorId = null, int color = 0)
        {
            return new ContextForCreateScheduleDTO
            {
                GroupID = groupId,
                ThemeID = themeId,
                MentorID = mentorId,
                Color = color
            };
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID, ScheduleTestValidationConstants.ValidColorValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ContextForCreateScheduletDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task ContextForCreateScheduleDTOAsync_EmptyDataExceptGroupId_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID, null, null, ScheduleTestValidationConstants.ValidColorValue);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidGroupID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidThemeID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_NotValidMentorID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.ValidEntityID,
                ScheduleTestValidationConstants.NotValidEntityID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
