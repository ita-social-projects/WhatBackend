using CharlieBackend.Api.Validators.DashboardDTOValidators;
using CharlieBackend.Core.DTO.Dashboard;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class StudentsRequestDtoValidatorTests : TestBase
    {
        private StudentsRequestDtoValidator<ClassbookResultType> _validator;
        private readonly ClassbookResultType[] classbookResultType = new ClassbookResultType[]
        {
            ClassbookResultType.StudentMarks
        };
        private readonly long? validCourseID = 1;
        private readonly long? validStudentGroupID = 1;
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validEndDate = new DateTime(2020, 01, 01);

        private readonly long? notValidCourseID = 0;
        private readonly long? notValidStudentGroupID = 0;
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidEndDate = new DateTime(2019, 01, 01);

        public StudentsRequestDtoValidatorTests()
        {
            _validator = new StudentsRequestDtoValidator<ClassbookResultType>();
        }

        public StudentsRequestDto<ClassbookResultType> GetDTO(
            DateTime startDate = default(DateTime),
            DateTime finishDate = default(DateTime),
            long? courseID = null,
            long? studentGroupId = null)
        {
            return new StudentsRequestDto<ClassbookResultType>
            {
                CourseId = courseID,
                StudentGroupId = studentGroupId,
                StartDate = startDate,
                FinishDate = finishDate,
                IncludeAnalytics = classbookResultType
            };
        }

        [Fact]
        public async Task StudentsRequestDTOAsync_ValidData_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validStartDate,
                validEndDate,
                validCourseID,
                validStudentGroupID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task StudentsRequestDTOAsync_EmptyData_ShouldReturnFalse()
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
        public async Task StudentsRequestDTOAsync_EmptyIDs_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(validStartDate,validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task StudentsRequestDTOAsync_NotValidGroupID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validStartDate,
                validEndDate,
                validCourseID,
                notValidStudentGroupID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task StudentsRequestDTOAsync_NotValidCourseID_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                validStartDate,
                validEndDate,
                notValidCourseID,
                validStudentGroupID);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task StudentsRequestDTOAsync_NotValidDates_ShouldReturnFalse()
        {
            // Arrange
            var dto = GetDTO(
                notValidStartDate,
                notValidEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
