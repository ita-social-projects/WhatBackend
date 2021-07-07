using CharlieBackend.Api.Validators.VisitDTOValidators;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Visit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class VisitDtoValidatorTests : TestBase
    {
        private VisitDtoValidator _validator;

        private readonly long validStudendId = 21;
        private readonly sbyte validStudentMark = 100;
        private readonly string validComment = "ValidComment - Comment is totally valid and validation is working correctly. Yey.";
        private readonly long notValidStudendId = 0;
        private readonly sbyte notValidStudentMark = 101;
        private readonly string notValidComment = new string('*', 1025);


        public VisitDtoValidatorTests()
        {
            _validator = new VisitDtoValidator();
        }

        public VisitDto Get_VisitDto(
            long studentId = 0,
            sbyte? studentMark = null,
            bool presence = false,
            string comment = null)
        {
            return new VisitDto
            {
                StudentId = studentId,
                StudentMark = studentMark,
                Presence = presence,
                Comment = comment
            };
        }

        [Fact]
        public async Task VisitDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var visit = Get_VisitDto(
                    validStudendId,
                    validStudentMark,
                    true,
                    validComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task VisitDTOAsync_EmptyStudentMark_ShouldReturnTrue()
        {
            // Arrange
            var visit = Get_VisitDto(
                    validStudendId,
                    null,
                    true,
                    validComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task VisitDTOAsync_EmptyComment_ShouldReturnTrue()
        {
            // Arrange
            var visit = Get_VisitDto(
                    validStudendId,
                    validStudentMark,
                    true,
                    null);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task VisitDTOAsync_EmptyData_ShouldReturnFalse()
        {
            // Arrange
            var visit = Get_VisitDto();

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task VisitDTOAsync_NotValidData_ShouldReturnFalse()
        {
            // Arrange
            var visit = Get_VisitDto(
                    notValidStudendId,
                    notValidStudentMark,
                    true,
                    notValidComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task VisitDTOAsync_NotValidStudentId_ShouldReturnFalse()
        {
            // Arrange
            var visit = Get_VisitDto(
                    notValidStudendId,
                    validStudentMark,
                    true,
                    validComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task VisitDTOAsync_NotValidStudentMark_ShouldReturnFalse()
        {
            // Arrange
            var visit = Get_VisitDto(
                    validStudendId,
                    notValidStudentMark,
                    true,
                    validComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task VisitDTOAsync_NotValidComment_ShouldReturnFalse()
        {
            // Arrange
            var visit = Get_VisitDto(
                    validStudendId,
                    validStudentMark,
                    true,
                    notValidComment);

            // Act
            var result = await _validator.ValidateAsync(visit);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}