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
                case PatternType.RelativeMonthly:
                case PatternType.Daily:
                case PatternType.AbsoluteMonthly:
                default:
                    break;
            }

            return result;
        }

        static long GetPatternStorageValueWeekly(PatternForCreateScheduleDTO source)
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
    }
}
