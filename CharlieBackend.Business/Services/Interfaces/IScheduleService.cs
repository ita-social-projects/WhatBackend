using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<Result<EventOccurrenceDTO>> CreateScheduleAsync(CreateScheduleDto scheduleModel);

        public Task<Result<EventOccurrenceDTO>> GetEventOccurrenceByIdAsync(long id);

        public Task<Result<ScheduledEventDTO>> UpdateScheduledEventByID(long scheduledEvendID, UpdateScheduledEventDto scheduleModel);

        public Task<Result<IList<ScheduledEventDTO>>> UpdateEventsRange(long eventOccuranceID, DateTime? startDate, DateTime? finishDate, UpdateScheduledEventDto request);

        public Task<Result<IList<ScheduledEventDTO>>> GetEventsFiltered(ScheduledEventFilterRequestDTO request);

        public Task<Result<EventOccurenceDTO>> DeleteScheduleByIdAsync(long studentGroupId, DateTime? startDate, DateTime? finishDate);
    }
}
