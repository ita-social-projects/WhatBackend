using CharlieBackend.Api.Validators.DashboardDTOValidators;
using CharlieBackend.Core.DTO.Dashboard;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests
{
    public class DashboardAnalyticsRequestDtoValdiatorTests : TestBase
    {
        private DashboardAnalyticsRequestDtoValidators<ClassbookResultType> _validator;
        private readonly ClassbookResultType[] classbookResultType = new ClassbookResultType[]
        {
            0
        };
        private readonly DateTime validStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime validEndDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidStartDate = new DateTime(2020, 01, 01);
        private readonly DateTime notValidEndDate = new DateTime(2019, 01, 01);

        public DashboardAnalyticsRequestDtoValdiatorTests()
        {
            _validator = new DashboardAnalyticsRequestDtoValidators<ClassbookResultType>();
        }

        public DashboardAnalyticsRequestDto<ClassbookResultType> GetDTO(
            DateTime startDate = default(DateTime),
            DateTime finishDate = default(DateTime))
        {
            return new DashboardAnalyticsRequestDto<ClassbookResultType>
            {
                StartDate = startDate,
                FinishDate = finishDate,
                IncludeAnalytics = classbookResultType
            };
        }

        [Fact]
        public async Task DashboardAnalyticsDTOAsync_ValidDates_ShouldReturnTrue()
        {
            // Arrange
            var dto = GetDTO(
                validStartDate,
                validEndDate);

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            result.IsValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task DashboardAnalyticsDTOAsync_EmptyDates_ShouldReturnFalse()
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
        public async Task DashboardAnalyticsDTOAsync_NotValidDates_ShouldReturnFalse()
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
