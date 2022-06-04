using CharlieBackend.Api.Validators.HomeworkDTOValidators;
using CharlieBackend.Core.DTO.Homework;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.HomeworkDtoValidatorsTests
{
    public class GetHomeworksRequestDtoValidatorTests : TestBase
    {
        private readonly GetHomeworksRequestDtoValidator _validator;

        public static IEnumerable<object[]> GetHomeworkRequestDtoAsync_ValidData
        {
            get
            {
                yield return new object[] { 1, null, null, DateTime.Parse("1/1/2019"), DateTime.Parse("2/2/2019") };
                yield return new object[] { null, 1, null, DateTime.Parse("3/3/2020"), DateTime.Parse("3/4/2020") };
                yield return new object[] { null, null, 1, DateTime.Parse("2/1/2019"), DateTime.Parse("3/2/2019") };
            }
        }

        public static IEnumerable<object[]> GetHomeworkRequestDtoAsync_InvalidData
        {
            get
            {
                yield return new object[] { null, null, null, DateTime.Parse("1/1/2019"), DateTime.Parse("2/2/2019") };
                yield return new object[] { null, 1, null, DateTime.Parse("3/3/2020"), null };
                yield return new object[] { null, null, 1, DateTime.Parse("5/5/2019"), DateTime.Parse("2/2/2019") };
            }
        }

        public GetHomeworksRequestDtoValidatorTests()
        {
            _validator = new GetHomeworksRequestDtoValidator();
        }

        [Theory]
        [MemberData(nameof(GetHomeworkRequestDtoAsync_ValidData))]
        public async Task GetHomeworkRequestDtoAsync_ValidData_ShouldReturnTrue
            (int? courseId, int? groupId, int? themeId, DateTime start, DateTime finish)
        {
            // Arrange
            GetHomeworkRequestDto dto = new GetHomeworkRequestDto
            {
                CourseId = courseId,
                GroupId = groupId,
                ThemeId = themeId,
                StartDate = start,
                FinishDate = finish
            };

            // Act
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetHomeworkRequestDtoAsync_InvalidData))]
        public async Task GetHomeworkRequestDtoAsync_InvalidData_ShouldReturnTrue
            (int? courseId, int? groupId, int? themeId, DateTime start, DateTime finish)
        {
            // Arrange
            GetHomeworkRequestDto dto = new GetHomeworkRequestDto
            {
                CourseId = courseId,
                GroupId = groupId,
                ThemeId = themeId,
                StartDate = start,
                FinishDate = finish
            };

            // Act
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeFalse();
        }
    }
}
