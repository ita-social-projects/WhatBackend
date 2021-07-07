using CharlieBackend.Api.Validators.StudentGroupsDTOValidators;
using CharlieBackend.Core.DTO.StudentGroups;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class UpdateStudentGroupDtoValidatorTests : TestBase
    {
        private UpdateStudentGroupDtoValidator _validator;

        private readonly string validName = "ValidName";
        private readonly long validCourseId = 70;
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validFinishDate = new DateTime(2020, 01, 01);
        private readonly List<long> validStudentIds = new List<long>() { 1, 21, 48 };
        private readonly List<long> validMentorIds = new List<long>() { 1, 27, 54 };
        private readonly string notValidName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";
        private readonly long notValidCourseId = 0;
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidFinishDate = new DateTime(2019, 01, 01);
        private readonly List<long> notValidStudentIds = new List<long>() { 0, 21, 48 };
        private readonly List<long> notValidMentorIds = new List<long>() { 0, 27, 54 };

        public UpdateStudentGroupDtoValidatorTests()
        {
            _validator = new UpdateStudentGroupDtoValidator();
        }

        public UpdateStudentGroupDto Get_UpdateStudentGroupDto(
            string name = null,
            long courseId = 0,
            DateTime startDate = default(DateTime),
            DateTime finishDate = default(DateTime),
            List<long> studentIds = null,
            List<long> mentorIds = null)
        {
            return new UpdateStudentGroupDto
            {
                Name = name,
                CourseId = courseId,
                StartDate = startDate,
                FinishDate = finishDate,
                StudentIds = studentIds,
                MentorIds = mentorIds
            };
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    validName,
                    validCourseId,
                    validStartDate,
                    validFinishDate,
                    validStudentIds,
                    validMentorIds);

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
            var student = Get_UpdateStudentGroupDto();

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
            var student = Get_UpdateStudentGroupDto(
                    notValidName,
                    notValidCourseId,
                    notValidStartDate,
                    notValidFinishDate,
                    notValidStudentIds,
                    notValidMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidCourseId_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    validName,
                    notValidCourseId,
                    validStartDate,
                    validFinishDate,
                    validStudentIds,
                    validMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidName_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    notValidName,
                    validCourseId,
                    validStartDate,
                    validFinishDate,
                    validStudentIds,
                    validMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidDates_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    validName,
                    validCourseId,
                    notValidStartDate,
                    notValidFinishDate,
                    validStudentIds,
                    validMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidStudentIds_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    validName,
                    validCourseId,
                    validStartDate,
                    validFinishDate,
                    notValidStudentIds,
                    validMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task UpdateStudentDTOAsync_NotValidMentorIds_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_UpdateStudentGroupDto(
                    validName,
                    validCourseId,
                    validStartDate,
                    validFinishDate,
                    validStudentIds,
                    notValidMentorIds);

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
