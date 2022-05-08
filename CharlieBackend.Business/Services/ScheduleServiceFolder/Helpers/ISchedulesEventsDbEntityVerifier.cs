using CharlieBackend.Core.DTO.Event;
using CharlieBackend.Core.DTO.Schedule;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers
{
    /// <summary>
    /// Interface that implies implementing class to make requests to database to verify context IDs
    /// </summary>
    public interface ISchedulesEventsDbEntityVerifier
    {
        Task<string> ValidateCreateScheduleRequestAsync(CreateScheduleDto createScheduleRequest);
        Task<string> ValidateEventOccuranceId(long id);
        Task<string> ValidateGetEventsFilteredRequest(ScheduledEventFilterRequestDTO request);
        Task<string> ValidateCreateScheduleEventRequestAsync(CreateSingleEventDto request);
    }
}