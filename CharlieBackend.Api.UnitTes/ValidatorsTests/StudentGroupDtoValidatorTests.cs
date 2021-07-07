using CharlieBackend.Api.Validators.StudentGroupsDTOValidators;
using CharlieBackend.Core.DTO.StudentGroups;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class StudentGroupDtoValidatorTests : TestBase
    {
        private StudentGroupDtoValidator _validator;

        private readonly long validId = 50;
        private readonly long validCourseId = 70;
        private readonly string validName = "ValidName";
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validFinishDate = new DateTime(2020, 01, 01);
        private readonly List<long> validStudentIds = new List<long>() { 1, 21, 48 };
        private readonly List<long> validMentorIds = new List<long>() { 1, 27, 54 };
        private readonly long notValidId = 0;
        private readonly long notValidCourseId = 0;
        private readonly string notValidName = "TooLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongName";
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidFinishDate = new DateTime(2019, 01, 01);
        private readonly List<long> notValidStudentIds = new List<long>() { 0, 21, 48 };
        private readonly List<long> notValidMentorIds = new List<long>() { 0, 27, 54 };

        public StudentGroupDtoValidatorTests()
        {
            _validator = new StudentGroupDtoValidator();
        }

        public StudentGroupDto Get_StudentGroupDto(
            long id = 0,
            long courseId = 0,
            string name = null,
            DateTime startDate = default(DateTime),
            DateTime finishDate = default(DateTime),
            List<long> studentIds = null,
            List<long> mentorIds = null)
        {
            return new StudentGroupDto
            {
                Id = id,
                CourseId = courseId,
                Name = name,
                StartDate = startDate,
                FinishDate = finishDate,
                StudentIds = studentIds,
                MentorIds = mentorIds
            };
        }

        [Fact]
        public async Task StudentDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    validCourseId,
                    validName,
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
        public async Task StudentDTOAsync_EmptyData_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto();

            // Act
            var result = await _validator.ValidateAsync(student);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task StudentDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    notValidId,
                    notValidCourseId,
                    notValidName,
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
        public async Task StudentDTOAsync_NotValidId_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    notValidId,
                    validCourseId,
                    validName,
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
        public async Task StudentDTOAsync_NotValidCourseId_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    notValidCourseId,
                    validName,
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
        public async Task StudentDTOAsync_NotValidName_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    validCourseId,
                    notValidName,
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
        public async Task StudentDTOAsync_NotValidDates_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    validCourseId,
                    validName,
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
        public async Task StudentDTOAsync_NotValidStudentIds_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    validCourseId,
                    notValidName,
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
        public async Task StudentDTOAsync_NotValidMentorIds_ShouldReturnFalse()
        {
            // Arrange
            var student = Get_StudentGroupDto(
                    validId,
                    validCourseId,
                    notValidName,
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
