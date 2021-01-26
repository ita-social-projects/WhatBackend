using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services
{
    class EventOccuranceStorageParser
    {
        internal static long GetPatternStorageValue(PatternForCreateScheduleDTO source)
        {
            long result = 0;

            switch (source.Type)
            {               
                case PatternType.Weekly:
                    result = GetPatternStorageValueWeekly(source);
                    break;
                case PatternType.Daily:
                    result = GetPatternStorageValueDaily(source);
                    break;
                case PatternType.AbsoluteMonthly:
                    result = GetPatternStorageValueAbsoluteMonthly(source);
                    break;
                case PatternType.RelativeMonthly:
                    result = GetPatternStorageValueRelativeMonthly(source);
                    break;
                default:
                    break;
            }

            return result;
        }

        private static long GetPatternStorageValueWeekly(PatternForCreateScheduleDTO source)
        {
            long result = 0;
            long index = 1;

            foreach (DayOfWeek item in source.DaysOfWeek)
            {
                index <<= (int)item;
                result |= index;
                index = 1;
            }

            result |= index << (7 + source.Interval);

            return result;
        }

        private static long GetPatternStorageValueAbsoluteMonthly(PatternForCreateScheduleDTO source)
        {
            long result = 0;
            long index = 1;

            foreach (DayOfWeek item in source.DaysOfWeek)
            {
                index <<= (int)item;
                result |= index;
                index = 1;
            }

            result |= index << (31 + source.Interval);

            return result;
        }

        private static long GetPatternStorageValueDaily(PatternForCreateScheduleDTO source)
        {
            return source.Interval;
        }

        private static long GetPatternStorageValueRelativeMonthly(PatternForCreateScheduleDTO source)
        {
            long result = 0;
            long index = 1;

            foreach (DayOfWeek item in source.DaysOfWeek)
            {
                index <<= (int)item;
                result |= index;
                index = 1;
            }

            result |= index << (7 + (int)source.Index);

            result |= index << (7 + 5 + (int)source.Interval);

            return result;
        }

        internal static (int , IList<DayOfWeek>) GetDataForWeekly(long source)
        {
            string check = Convert.ToString(source, 2);

            string daysString = check.Substring(check.Length - 7);

            IList<DayOfWeek> daysCollection = new List<DayOfWeek>();

            for (int i = daysString.Length - 1; i >= 0; i--)
            {
                if (daysString[i] == '1')
                {
                    daysCollection.Add((DayOfWeek)(daysString.Length - i - 1));
                }
            }

            string intervalString = check.Substring(0, check.Length - 7);

            int interval = intervalString.Length - intervalString.LastIndexOf('1') - 1;

            return (interval, daysCollection);
        }

        internal static (int, IList<int>) GetDataForAbsoluteMonthly(long source)
        {
            string check = Convert.ToString(source, 2);

            string daysString = check.Substring(check.Length - 31);

            IList<int> daysCollection = new List<int>();

            for (int i = daysString.Length - 1; i >= 0; i--)
            {
                if (daysString[i] == '1')
                {
                    daysCollection.Add(i - 1);
                }
            }

            string intervalString = check.Substring(0, check.Length - 31);

            int interval = intervalString.Length - intervalString.LastIndexOf('1') - 1;

            return (interval, daysCollection);
        }

        internal static int GetDataForDaily(long source)
        {
            return (int)source;
        }

        internal static (int interval, MonthIndex index, IList<DayOfWeek> daysCollection) GetDataForRelativeMonthly(long source)
        {
            string check = Convert.ToString(source, 2);

            string daysString = check.Substring(check.Length - 7);

            IList<DayOfWeek> daysCollection = new List<DayOfWeek>();

            for (int i = daysString.Length - 1; i >= 0; i--)
            {
                if (daysString[i] == '1')
                {
                    daysCollection.Add((DayOfWeek)(daysString.Length - i - 1));
                }
            }

            string indexString = check.Substring(check.Length - 7 - 5, check.Length - 7);

            int index = indexString.Length - indexString.LastIndexOf('1') - 1;

            string intervalString = check.Substring(0, check.Length - 7 - 5);

            int interval = indexString.Length - indexString.LastIndexOf('1') - 1;

            return (interval, (MonthIndex)index, daysCollection);
        }
    }
}
