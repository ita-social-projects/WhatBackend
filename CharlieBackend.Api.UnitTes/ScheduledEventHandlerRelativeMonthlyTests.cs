using System;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace CharlieBackend.Api.UnitTest
{
    public class ScheduledEventHandlerRelativeMonthlyTests
    {
        EventOccurrence _source;
        PatternForCreateScheduleDTO _pattern;
        ContextForCreateScheduleDTO _context;
        ScheduledEventHandlerRelativeMonthly scheduledEventHandlerRelativeMonthly;

        public ScheduledEventHandlerRelativeMonthlyTests()
        {
            _source = new EventOccurrence
            {
                StudentGroupId = 1,
                EventStart = new DateTime(year: 2021, month: 1, day: 6, hour: 12, minute: 0, second: 0),
                EventFinish = new DateTime(year: 2021, month: 1, day: 7, hour: 13, minute: 45, second: 0),
                Pattern = PatternType.Weekly,
                Storage = 10
            };

            _context = new ContextForCreateScheduleDTO
            {
                GroupID = 1,
                MentorID = 1,
                ThemeID = 5
            };

            _pattern = new PatternForCreateScheduleDTO
            {
                Type = PatternType.Weekly,
                Interval = 1,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Saturday },
                Dates = new List<int> { 1, 2, 3, 31 },
                Index = WeekIndex.First
            };
            scheduledEventHandlerRelativeMonthly = new ScheduledEventHandlerRelativeMonthly(_pattern);

        }

        [Fact]
        public void GetEvents_ShouldReturnIEnumerable()
        {
            var result = scheduledEventHandlerRelativeMonthly.GetEvents(_source, _context).ToList();
            var expectedEvents = new List<ScheduledEvent>()
            {
                new ScheduledEvent
                {
                    EventOccurrenceId = 0,
                    EventOccurrence = _source,
                    EventStart = _source.EventStart,
                    EventFinish = new DateTime(year: 2021, month: 1, day: 6, hour: 13, minute: 45, second: 0),
                    MentorId = 1,
                    StudentGroupId = 1,
                    ThemeId = 5
                }
            };

            result.Should().NotBeNull();
            result.ElementAt(0).Should().BeEquivalentTo(expectedEvents.ElementAt(0));
        }
    }
}
