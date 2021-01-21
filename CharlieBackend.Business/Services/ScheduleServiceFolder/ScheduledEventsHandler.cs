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
    class ScheduledEventsHandler
    {
        public static IEnumerable<ScheduledEvent> GetEvents(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            switch (source.Pattern)
            {                
                case PatternType.Weekly:
                    return GetRelatedEventsWeekly(source, context);               
                case PatternType.RelativeMonthly:
                case PatternType.Daily:
                case PatternType.AbsoluteMonthly:
                default:
                    break;
            }

            return null;
        }

        private static IEnumerable<ScheduledEvent> GetRelatedEventsWeekly(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            (int interval, IList<DayOfWeek> daysCollection) data = EventOccuranceStorageParser.GetDataForWeekly(source.Storage);

            for (int i = 0; i < data.daysCollection.Count; i++)
            {
                DateTime targetStartDate = source.EventStart
                    + new TimeSpan(source.EventStart.DayOfWeek <= data.daysCollection[i]
                    ? data.daysCollection[i] - source.EventStart.DayOfWeek
                    : 7 - (int)source.EventStart.DayOfWeek + (int)data.daysCollection[i], 0, 0, 0);

                DateTime targetFinishDate = new DateTime(targetStartDate.Year, targetStartDate.Month, targetStartDate.Day,
                    source.EventFinish.Value.Hour, source.EventFinish.Value.Minute, source.EventFinish.Value.Second);

                while (targetFinishDate <= source.EventFinish)
                {
                    yield return new ScheduledEvent
                    {
                        EventOccurence = source,
                        EventOccurenceId = source.Id,
                        StudentGroupId = source.StudentGroupId,
                        EventStart = targetStartDate,
                        EventFinish = targetFinishDate,
                        MentorId = context.MentorID,
                        ThemeId = context.ThemeID
                    };

                    targetStartDate += new TimeSpan(7 * data.interval, 0, 0, 0);
                    targetFinishDate += new TimeSpan(7 * data.interval, 0, 0, 0);
                }
            }
        }
    }
}
