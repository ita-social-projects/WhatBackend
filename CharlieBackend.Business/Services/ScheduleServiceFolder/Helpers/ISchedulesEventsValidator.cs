using CharlieBackend.Core.DTO.Schedule;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers
{
    public interface ISchedulesEventsValidator
    {
        Task<string> ValidateCreateScheduleRequestAsync(CreateScheduleDto createScheduleRequest);
        Task<string> ValidateEventOccuranceId(long id);
        Task<string> ValidateGetEventsFilteredRequest(ScheduledEventFilterRequestDTO request);
    }
}