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
    static class EventOccuranceStorageParser
    {
        private static int _daysInWeek = 7;
        private static int _maxDaysInMonth = 31;
        private static int _possibleMonthIndexOptionsNumber = 5;

        internal static long GetPatternStorageValue(PatternForCreateScheduleDTO source)
        {
            long result = 0;
            long index = 1;

            if (source.DaysOfWeek != null)
                foreach (DayOfWeek item in source.DaysOfWeek)
            {
                index <<= (int)item;
                result |= index;
                index = 1;
            }

            if (source.Index.HasValue)
                result |= index << (_daysInWeek + (int)source.Index);

            index <<= _daysInWeek + _possibleMonthIndexOptionsNumber;

            if (source.Dates != null)
                foreach (int item in source.Dates)
            {
                index <<= item;
                result |= index;
                index = _daysInWeek + _possibleMonthIndexOptionsNumber;
            }

            index = 1;
            result |= index << (_daysInWeek + _possibleMonthIndexOptionsNumber + _maxDaysInMonth + source.Interval);

            return result;
        }

        internal static PatternForCreateScheduleDTO GetFullDataFromStorage(long source)
        {
            string check = Convert.ToString(source, 2);

            string daysString = check.Substring(check.Length - _daysInWeek);

            IList<DayOfWeek> daysCollection = new List<DayOfWeek>();

            for (int i = daysString.Length - 1; i >= 0; i--)
            {
                if (daysString[i] == '1')
                {
                    daysCollection.Add((DayOfWeek)(daysString.Length - i - 1));
                }
            }

            string indexString = check.Substring(check.Length - _daysInWeek - _possibleMonthIndexOptionsNumber, _possibleMonthIndexOptionsNumber);

            MonthIndex index = (MonthIndex)(indexString.Length - indexString.LastIndexOf('1') - 1);

            string datesString = check.Substring(check.Length - _daysInWeek - _possibleMonthIndexOptionsNumber - _maxDaysInMonth, 
                _maxDaysInMonth);

            IList<int> datesCollection = new List<int>();

            for (int i = datesString.Length - 1; i >= 0; i--)
            {
                if (datesString[i] == '1')
                {
                    datesCollection.Add(i - 1);
                }
            }

            string intervalString = check.Substring(0, check.Length - _daysInWeek - _possibleMonthIndexOptionsNumber - _maxDaysInMonth);

            int interval = intervalString.Length - intervalString.LastIndexOf('1') - 1;

            return new PatternForCreateScheduleDTO
            {
                Interval = interval,
                Index = index,
                Dates = datesCollection,
                DaysOfWeek = daysCollection
            };
        }
    }
}
