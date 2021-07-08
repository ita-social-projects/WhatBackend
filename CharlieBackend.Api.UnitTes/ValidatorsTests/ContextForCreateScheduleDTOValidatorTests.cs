using CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO;
using CharlieBackend.Core.DTO.Schedule;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class ContextForCreateScheduleDTOValidatorTests : TestBase
    {
        private ContextForCreateScheduleDTOValidator _validator;
        private readonly long validGroupId = 1;
        private readonly long validThemeId = 1;
        private readonly long validMentorId = 1;
        private readonly long notValidGroupId = 0;
        private readonly long notValidThemeId = 0;
        private readonly long notValidMentorId = 0;

        public ContextForCreateScheduleDTOValidatorTests()
        {
            _validator = new ContextForCreateScheduleDTOValidator();
        }

        public ContextForCreateScheduleDTO GetDTO(
            long groupId = 0,
            long? themeId = null,
            long? mentorId = null)
        {
            return new ContextForCreateScheduleDTO
            {
                GroupID = groupId,
                ThemeID = themeId,
                MentorID = mentorId
            };
        }

        [Fact]
        public async Task ContextForCreateScheduleDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                    validGroupId,
                    validThemeId,
                    validMentorId);

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
            var dto = GetDTO(validGroupId);

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
            var dto = GetDTO(
                    notValidGroupId,
                    notValidThemeId,
                    notValidMentorId);

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
            var dto = GetDTO(
                    notValidGroupId,
                    validThemeId,
                    validMentorId);

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
            var dto = GetDTO(
                    validGroupId,
                    notValidThemeId,
                    validMentorId);

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
            var dto = GetDTO(
                    validGroupId,
                    validThemeId,
                    notValidMentorId);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

    }
}
