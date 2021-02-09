using CharlieBackend.Business.Services.ScheduleServiceFolder;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace CharlieBackend.Api.UnitTest
{
    public class EventOcurranceStorageParserTests
    {
        [Fact]
        public void GetPatternStorageValue_ValidPattern_ShouldReturnSuccessResult()
        {
            PatternForCreateScheduleDTO patternForCreateScheduleDTO = new PatternForCreateScheduleDTO
            {
                Dates = new List<int>()
                {
                    1, 2
                },
                Type = PatternType.Daily,
                Interval = 1,
                Index = MonthIndex.First,
                DaysOfWeek = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday
                }
            };

            var storage = EventOccuranceStorageParser.GetPatternStorageValue(patternForCreateScheduleDTO);

            var result = EventOccuranceStorageParser.GetFullDataFromStorage(storage);

            result.Should().BeEquivalentTo(patternForCreateScheduleDTO);
        }
    }
}
