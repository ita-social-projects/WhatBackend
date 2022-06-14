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
            item.EventStart = request.EventStart.HasValue ? new DateTime(request.EventStart.Value.Year, request.EventStart.Value.Month, request.EventStart.Value.Day,
                request.EventStart.Value.Hour, request.EventStart.Value.Minute, request.EventStart.Value.Second) : item.EventStart;
            item.EventFinish = request.EventEnd.HasValue ? new DateTime(request.EventEnd.Value.Year, request.EventEnd.Value.Month, request.EventEnd.Value.Day,
                request.EventEnd.Value.Hour, request.EventEnd.Value.Minute, request.EventEnd.Value.Second) : item.EventFinish;
            item.Description = request.Description ?? item.Description;
            item.Link = request.Link ?? item.Link;
            item.Color = request.Color;

            return item;
        }
    }
}
