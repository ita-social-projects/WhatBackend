using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Panel.Models.ScheduledEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IEventsService
    {
        Task ConnectScheduleToLessonById(long eventId, LessonDto lesson);
    }
}
