using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services
{
    static class ScheduledEventsHandler
    {
        public static IEnumerable<ScheduledEvent> GetEvents(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            PatternForCreateScheduleDTO data = EventOccuranceStorageParser.GetFullDataFromStorage(source.Storage);

            EventDetail details = new EventDetail()
            {
                startDate = source.EventStart
            };

            switch (source.Pattern)
            {
                case PatternType.Daily:
                    details.index = 1;
                    details.IsMonthlyRelated = false;
                    details.del = GetStartDateDaily;
                    break;
                case PatternType.Weekly:
                    details.count = data.DaysOfWeek.Count;
                    details.index = 7;
                    details.IsMonthlyRelated = false;
                    details.del = GetStartDateWeekly;
                    break;
                case PatternType.AbsoluteMonthly:
                    details.count = data.Dates.Count;
                    details.index = 1;
                    details.IsMonthlyRelated = true;
                    details.del = GetStartDateAbsoluteMonthly;
                    break;
                case PatternType.RelativeMonthly:
                    details.count = data.DaysOfWeek.Count;
                    details.index = 1;
                    details.IsMonthlyRelated = true;
                    details.del = GetStartDateRelativeMonthly;
                    details.startDate = new DateTime(source.EventStart.Year, source.EventStart.Month, 7 * (int)data.Index,
                source.EventStart.Hour, source.EventStart.Minute, source.EventStart.Second);
                    break;
                default:
                    break;
            }

            return GetEventsCollection(data, details, source, context);
        }

        private static IEnumerable<ScheduledEvent> GetEventsCollection(PatternForCreateScheduleDTO data, EventDetail details, 
            EventOccurence source, ContextForCreateScheduleDTO context)
        {
            for (int i = 0; i < details.count; i++)
            {
                DateTime targetStartDate = details.del(details.startDate, i, data);

                DateTime targetFinishDate = new DateTime(targetStartDate.Year, targetStartDate.Month, targetStartDate.Day,
                    source.EventFinish.Value.Hour, source.EventFinish.Value.Minute, source.EventFinish.Value.Second);

                while (targetFinishDate <= source.EventFinish)
                {
                    yield return CreateEvent(targetStartDate, targetFinishDate, source, context);

                    if (details.IsMonthlyRelated)
                    {
                        details.startDate.AddMonths(details.index * data.Interval);
                        details.startDate.AddMonths(details.index * data.Interval);
                    }
                    else
                    {
                        details.startDate.AddDays(details.index * data.Interval);
                        details.startDate.AddDays(details.index * data.Interval);
                    }
                }
            }
        }

        private static ScheduledEvent CreateEvent(DateTime targetStartDate, DateTime targetFinishDate, 
            EventOccurence source, ContextForCreateScheduleDTO context)
        {
            return new ScheduledEvent
            {
                EventOccurence = source,
                EventOccurenceId = source.Id,
                StudentGroupId = source.StudentGroupId,
                EventStart = targetStartDate,
                EventFinish = targetFinishDate,
                MentorId = context.MentorID,
                ThemeId = context.ThemeID
            };
        }

        private static DateTime GetStartDateWeekly(DateTime startDate, int i, PatternForCreateScheduleDTO data)
        {
            return startDate.AddDays(startDate.DayOfWeek <= data.DaysOfWeek[i]
                    ? data.DaysOfWeek[i] - startDate.DayOfWeek
                    : 7 - (int)startDate.DayOfWeek + (int)data.DaysOfWeek[i]);
        }

        private static DateTime GetStartDateAbsoluteMonthly(DateTime startDate, int i, PatternForCreateScheduleDTO data)
        {
            return startDate.Day <= data.Dates[i] ?
                    new DateTime(startDate.Year, startDate.Month, data.Dates[i],
                    startDate.Hour, startDate.Minute, startDate.Second) :
                    new DateTime(startDate.Year, startDate.Month, data.Dates[i],
                    startDate.Hour, startDate.Minute, startDate.Second).AddMonths(1);
        }

        private static DateTime GetStartDateRelativeMonthly(DateTime startDate, int i, PatternForCreateScheduleDTO data)
        {
            return startDate.AddDays(startDate.DayOfWeek <= data.DaysOfWeek[i]
                       ? data.DaysOfWeek[i] - startDate.DayOfWeek
                       : 7 - (int)startDate.DayOfWeek + (int)data.DaysOfWeek[i]);
        }

        private static DateTime GetStartDateDaily(DateTime startDate, int i, PatternForCreateScheduleDTO data)
        {
            return startDate;
        }

        private delegate DateTime GetFirstDateDelegate(DateTime startDate, int index, PatternForCreateScheduleDTO data);
        private struct EventDetail
        {
            public GetFirstDateDelegate del;
            public DateTime startDate;
            public bool IsMonthlyRelated;
            public int index;
            public int count;
        }
    }
}
