using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IEventsService
    {
        public Task<ScheduledEventDTO> UpdateAsync(long id, UpdateScheduledEventDto scheduleModel);

        public Task<Result<bool>> DeleteAsync(long id);

        public Task<ScheduledEventDTO> GetAsync(long id);

        public Task<Result<ScheduledEventDTO>> ConnectScheduleToLessonById(long eventId, long lessonId);
    }
}
