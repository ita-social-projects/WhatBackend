using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.UnitTest.ValidatorsTests.ScheduleDTOValidatorsTests
{
    public static class ScheduleTestValidationConstants
    {
        #region Dates

        public static readonly DateTime ValidStartDate = new DateTime(2020, 01, 01);
        public static readonly DateTime ValidFinishDate = new DateTime(2020, 01, 02);
        public static readonly DateTime NotValidFinishDate = new DateTime(2019, 01, 01);
        public static readonly List<int> ValidDates = new List<int> { 19, 23, 26, 30 };
        public static readonly List<int> NotValidDatesBelowOne = new List<int> { 0, 23, 26 };
        public static readonly List<int> NotValidDatesAboveThirtyOne = new List<int> { 19, 23, 36 };

        #endregion

        #region Enums

        public const MonthIndex SecondMonthIndex = MonthIndex.Second;
        public const MonthIndex NotValidMonthIndex = (MonthIndex)8;

        public const PatternType WeeklyPatternType = PatternType.Weekly;
        public const PatternType NotValidPatternType = (PatternType)10;

        public static readonly List<DayOfWeek> ValidDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday };
        public static readonly List<DayOfWeek> NotValidDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday, (DayOfWeek)10 };

        #endregion

        #region IDs

        public const long ValidEntityID = 1;
        public const long NotValidEntityID = 0;
        public const int ValidColorValue = 3447003;

        #endregion

        public const int ValidInterval = 2;
        public const int NotValidInterval = 0;

        public static readonly TimeSpan ValidLessonStart = new TimeSpan(12, 0, 0);
        public static readonly TimeSpan ValidLessonEnd = new TimeSpan(14, 0, 0);
        public static readonly TimeSpan NotValidLessonStart = new TimeSpan(14, 30, 0);
        public static readonly TimeSpan NotValidLessonEnd = new TimeSpan(14, 0, 0);

        public static readonly uint? ValidDayNumber = 31;
        public static readonly uint? NotValidDayNumber = 33;

        public static readonly PatternForCreateScheduleDTO ValidPattern = new PatternForCreateScheduleDTO
            {
                Type = WeeklyPatternType,
                Interval = ValidInterval,
                DaysOfWeek = ValidDaysOfWeek,
                Index = SecondMonthIndex,
                Dates = ValidDates
            };

        public static readonly OccurenceRange ValidOccurenceRange = new OccurenceRange
        {
            StartDate = ValidStartDate,
            FinishDate = ValidFinishDate
        };

        public static readonly ContextForCreateScheduleDTO ValidContext = new ContextForCreateScheduleDTO
        {
            GroupID = ValidEntityID,
            ThemeID = ValidEntityID,
            MentorID = ValidEntityID,
            Color = ValidColorValue
        };

        public static readonly PatternForCreateScheduleDTO NotValidPattern = new PatternForCreateScheduleDTO
        {
            Type = WeeklyPatternType,
            Index = SecondMonthIndex,
            Interval = ValidInterval,
            DaysOfWeek = ValidDaysOfWeek,
            Dates = NotValidDatesBelowOne
        };

        public static readonly OccurenceRange NotValidOccurenceRange = new OccurenceRange
        {
            StartDate = ValidStartDate,
            FinishDate = NotValidFinishDate
        };

        public static readonly ContextForCreateScheduleDTO NotValidContext = new ContextForCreateScheduleDTO
        {
            GroupID = NotValidEntityID,
            ThemeID = ValidEntityID,
            MentorID = ValidEntityID
        };

        public static readonly ScheduledEventFilterRequestDTO ValidFilter = new ScheduledEventFilterRequestDTO
        {
            CourseID = ValidEntityID,
            MentorID = ValidEntityID,
            GroupID = ValidEntityID,
            ThemeID = ValidEntityID,
            StudentAccountID = ValidEntityID,
            EventOccurrenceID = ValidEntityID,
            StartDate = ValidStartDate,
            FinishDate = ValidFinishDate
        };

        public static readonly UpdateScheduledEventDto ValidRequest = new UpdateScheduledEventDto
        {
            StudentGroupId = ValidEntityID,
            ThemeId = ValidEntityID,
            MentorId = ValidEntityID,
            EventStart = ValidStartDate,
            EventEnd = ValidFinishDate
        };

        public static readonly ScheduledEventFilterRequestDTO NotValidFilter = new ScheduledEventFilterRequestDTO
        {
            CourseID = ValidEntityID,
            MentorID = ValidEntityID,
            GroupID = ValidEntityID,
            ThemeID = NotValidEntityID,
            StudentAccountID = ValidEntityID,
            EventOccurrenceID = ValidEntityID,
            StartDate = ValidStartDate,
            FinishDate = ValidFinishDate
        };

        public static readonly UpdateScheduledEventDto NotValidRequest = new UpdateScheduledEventDto
        {
            StudentGroupId = ValidEntityID,
            ThemeId = ValidEntityID,
            MentorId = ValidEntityID,
            EventStart = ValidStartDate,
            EventEnd = NotValidFinishDate
        };
    }
}
