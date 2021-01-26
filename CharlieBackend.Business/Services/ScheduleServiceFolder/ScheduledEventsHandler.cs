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
                case PatternType.Daily:
                    return GetRelatedEventsDaily(source, context);               
                case PatternType.AbsoluteMonthly:
                    return GetRelatedEventsAbsoluteMonthly(source, context);
                case PatternType.RelativeMonthly:
                    return GetRelatedEventsRelativeMonthly(source, context);
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

        private static IEnumerable<ScheduledEvent> GetRelatedEventsDaily(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            int interval = EventOccuranceStorageParser.GetDataForDaily(source.Storage);

            DateTime targetStartDate = source.EventStart;

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

                targetStartDate += new TimeSpan(interval, 0, 0, 0);
                targetFinishDate += new TimeSpan(interval, 0, 0, 0);
            }
        }

        private static IEnumerable<ScheduledEvent> GetRelatedEventsAbsoluteMonthly(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            (int interval, IList<int> daysCollection) data = EventOccuranceStorageParser.GetDataForAbsoluteMonthly(source.Storage);

            for (int i = 0; i < data.daysCollection.Count; i++)
            {
                DateTime targetStartDate = source.EventStart
                    + new TimeSpan(source.EventStart.Day <= data.daysCollection[i]
                    ? data.daysCollection[i] - source.EventStart.Day
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

                    targetStartDate.AddMonths(data.interval);
                    targetFinishDate.AddMonths(data.interval);
                }
            }
        }

        private static IEnumerable<ScheduledEvent> GetRelatedEventsRelativeMonthly(EventOccurence source, ContextForCreateScheduleDTO context)
        {
            (int interval, MonthIndex index, IList<DayOfWeek> daysCollection) data 
                = EventOccuranceStorageParser.GetDataForRelativeMonthly(source.Storage);

            DateTime relativeDate = new DateTime(source.EventStart.Year, source.EventStart.Month, 7 * (int)data.index, 
                source.EventStart.Hour, source.EventStart.Minute, source.EventStart.Second); 

            for (int i = 0; i < data.daysCollection.Count; i++)
            {
                DateTime targetStartDate = relativeDate.AddDays(relativeDate.DayOfWeek <= data.daysCollection[i]
                       ? data.daysCollection[i] - relativeDate.DayOfWeek
                       : 7 - (int)relativeDate.DayOfWeek + (int)data.daysCollection[i]);

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
                }

                relativeDate.AddMonths(data.interval);
                relativeDate.AddMonths(data.interval);
            }
        }
    }
}
