using System;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers
{
    class SchedulesUpdater
    {
        public static ScheduledEvent UpdateFields(ScheduledEvent item, UpdateScheduledEventDto request)
        {
            item.MentorId = request.MentorId ?? item.MentorId;
            item.StudentGroupId = request.StudentGroupId ?? item.StudentGroupId;
            item.ThemeId = request.ThemeId ?? item.ThemeId;
            item.EventStart = request.EventStart.HasValue ? new DateTime(item.EventStart.Year, item.EventStart.Month, item.EventStart.Day,
                request.EventStart.Value.Hour, request.EventStart.Value.Minute, request.EventStart.Value.Second) : item.EventStart;
            item.EventFinish = request.EventEnd.HasValue ? new DateTime(item.EventFinish.Year, item.EventFinish.Month, item.EventFinish.Day,
                request.EventEnd.Value.Hour, request.EventEnd.Value.Minute, request.EventEnd.Value.Second) : item.EventFinish;

            return item;
        }
    }
}
