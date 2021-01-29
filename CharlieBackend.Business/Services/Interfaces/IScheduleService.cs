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

        public Task<Result<IList<EventOccurrenceDTO>>> GetAllSchedulesAsync();

        public Task<Result<EventOccurrenceDTO>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleModel);

        public Task<Result<IList<ScheduledEventDTO>>> GetEventsFiltered(ScheduledEventFilterRequestDTO request);

        public Task<Result<EventOccurrenceDTO>> DeleteScheduleByIdAsync(long studentGroupId);
    }
}
