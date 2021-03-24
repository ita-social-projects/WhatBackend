using CharlieBackend.Api.Validators.ScheduledEventDTOValidators;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateScheduledEventDTOValidatorTests : TestBase
    {
        private UpdateScheduledEventDTOValidator _validator;
        private readonly long validStudentGroupId = 1;
        private readonly long validMentorId = 1;
        private readonly long validThemeId = 1;
        private readonly long notValidStudentGroupId = 0;
        private readonly long notValidMentorId = 0;
        private readonly long notValidThemeId = 0;
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validEndDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidEndDate = new DateTime(2019, 01, 01);
        
        public UpdateScheduledEventDTOValidatorTests()
        {
            _validator = new UpdateScheduledEventDTOValidator();
        }

        public UpdateScheduledEventDto Get_UpdateScheduledEventDTO(
            long? mentorId = null,
            long? studentGroupId = null,
            long? themeId = null,
            DateTime? eventStart = null,
            DateTime? eventEnd = null)
        {
            return new UpdateScheduledEventDto
            {
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                ThemeId = themeId,
                EventStart = eventStart,
                EventEnd = eventEnd
            };
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_ValidData()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate)))
                .IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_ValidData_WithoutDates()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId)))
                .IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_EmptyData()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO()))
                .IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_InvalidData()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    notValidStudentGroupId,
                    notValidMentorId,
                    notValidThemeId,
                    notValidStartDate,
                    notValidEndDate)))
                .IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_InvalidMentor()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    notValidMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate)))
                .IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_InvalidTheme()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    notValidThemeId,
                    validStartDate,
                    validEndDate)))
                .IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_InvalidGroup()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    notValidStudentGroupId,
                    validMentorId,
                    validThemeId,
                    validStartDate,
                    validEndDate)))
                .IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateScheduledEventDTOAsync_InvalidDates()
        {
            (await _validator.ValidateAsync(
                Get_UpdateScheduledEventDTO(
                    validStudentGroupId,
                    validMentorId,
                    validThemeId,
                    notValidStartDate,
                    notValidEndDate)))
                .IsValid
                .Should()
                .BeFalse();
        }
    }
}
