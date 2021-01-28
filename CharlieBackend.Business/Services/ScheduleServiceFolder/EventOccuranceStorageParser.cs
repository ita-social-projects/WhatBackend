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

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public static  class EventOccuranceStorageParser
    {
        private static int _daysInWeek = 7;
        private static int _maxDaysInMonth = 31;
        private static int _possibleMonthIndexOptionsNumber = 5;

        public static long GetPatternStorageValue(PatternForCreateScheduleDTO source)
        {
            (long result, long position) data = (0, 1);

            return data.AddWeekDays(source)
                .AddMonthIndex(source)
                .AddDates(source)
                .AddInterval(source);
        }

        private static (long result, long position) AddWeekDays(this (long result, long index) data, PatternForCreateScheduleDTO source)
        {
            long defaultValue = data.index;

            if (source.DaysOfWeek != null)
            {
                foreach (DayOfWeek item in source.DaysOfWeek)
                {
                    data.index <<= (int)item;
                    data.result |= data.index;
                    data.index = defaultValue;
                }
            }

            data.index <<= _daysInWeek;

            return data;
        }

        private static (long result, long index) AddMonthIndex(this (long result, long index) data, PatternForCreateScheduleDTO source)
        {
            if (source.Index.HasValue)
            {
                data.result |= data.index << (int)source.Index;
            }

            data.index <<= _possibleMonthIndexOptionsNumber;

            return data;
        }

        private static (long result, long index) AddDates(this (long result, long index) data, PatternForCreateScheduleDTO source)
        {
            long defaultValue = data.index;

            if (source.Dates != null)
            {
                foreach (int item in source.Dates)
                {
                    data.index <<= item;
                    data.result |= data.index;
                    data.index = defaultValue;
                }
            }

            data.index <<= _maxDaysInMonth;

            return data;
        }

        private static long AddInterval(this (long result, long index) data, PatternForCreateScheduleDTO source)
        {
            ///Should be used last to set up final length
            data.result |= data.index << source.Interval;

            return data.result;
        }

        internal static PatternForCreateScheduleDTO GetFullDataFromStorage(long source)
        {
            string stringRepresentationOfStorage = Convert.ToString(source, 2);

            string daysString = stringRepresentationOfStorage
                .Substring(stringRepresentationOfStorage.Length - _daysInWeek);

            string indexString = stringRepresentationOfStorage
                .Substring(stringRepresentationOfStorage.Length - _daysInWeek - _possibleMonthIndexOptionsNumber, _possibleMonthIndexOptionsNumber);

            string datesString = stringRepresentationOfStorage
                .Substring(stringRepresentationOfStorage.Length - _daysInWeek - _possibleMonthIndexOptionsNumber - _maxDaysInMonth, _maxDaysInMonth);

            string intervalString = stringRepresentationOfStorage
                .Substring(0, stringRepresentationOfStorage.Length - _daysInWeek - _possibleMonthIndexOptionsNumber - _maxDaysInMonth);

            return new PatternForCreateScheduleDTO
            {
                Interval = GetInterval(intervalString),
                Index = GetMonthIndex(indexString),
                Dates = GetDatesList(datesString),
                DaysOfWeek = GetDaysList(daysString)
            };
        }

        private static IList<DayOfWeek> GetDaysList(string daysString)
        {
            IList<DayOfWeek> daysCollection = new List<DayOfWeek>();

            for (int i = daysString.Length - 1; i >= 0; i--)
            {
                if (daysString[i] == '1')
                {
                    daysCollection.Add((DayOfWeek)(daysString.Length - i - 1));
                }
            }

            return daysCollection;
        }

        private static MonthIndex GetMonthIndex(string indexString)
        {
            return (MonthIndex)(indexString.Length - indexString.LastIndexOf('1') - 1);
        }

        private static IList<int> GetDatesList(string datesString)
        {
            IList<int> datesCollection = new List<int>();

            for (int i = datesString.Length - 1; i >= 0; i--)
            {
                if (datesString[i] == '1')
                {
                    datesCollection.Add(i - 1);
                }
            }

            return datesCollection;
        }

        private static int GetInterval(string intervalString)
        {
            return intervalString.Length - intervalString.LastIndexOf('1') - 1;
        }
    }
}
