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
        public const PatternType WeeklyType = PatternType.Weekly;
        public static readonly List<DayOfWeek> ValidDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday };

        #endregion

        public const int ValidInterval = 2;
        public const int NotValidInterval = 0;

        #region IDs

        public const long ValidEntityID = 1;
        public const long NotValidEntityID = 0;

        #endregion

        public static readonly PatternForCreateScheduleDTO ValidPattern = new PatternForCreateScheduleDTO
            {
                Type = WeeklyType,
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
            MentorID = ValidEntityID
        };

        public static readonly PatternForCreateScheduleDTO NotValidPattern = new PatternForCreateScheduleDTO
        {
            Type = WeeklyType,
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
    }
}
