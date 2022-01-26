using CharlieBackend.Core.DTO.Lesson;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IEventsService
    {
        Task ConnectScheduleToLessonById(long eventId, LessonDto lesson);
    }
}
