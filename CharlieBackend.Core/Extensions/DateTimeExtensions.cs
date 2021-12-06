using System;

namespace CharlieBackend.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public const int DayInWeekCount = 7;

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (DayInWeekCount + (dt.DayOfWeek - startOfWeek)) % DayInWeekCount;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (DayInWeekCount + (endOfWeek - dt.DayOfWeek)) % DayInWeekCount;
            return dt.AddDays(diff).Date;
        }
    }
}
