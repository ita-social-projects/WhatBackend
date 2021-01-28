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
        public Task<Result<EventOccurenceDTO>> CreateScheduleAsync(CreateScheduleDto scheduleModel);

        public Task<Result<IList<EventOccurenceDTO>>> GetAllSchedulesAsync();

        public Task<Result<EventOccurenceDTO>> UpdateStudentGroupAsync(long scheduleId, UpdateScheduleDto scheduleModel);

        public Task<Result<IList<EventOccurenceDTO>>> GetSchedulesByStudentGroupIdAsync(long studentGroupId);

        public Task<Result<EventOccurenceDTO>> DeleteScheduleByIdAsync(long studentGroupId);

        Task<Result<IList<EventOccurenceDTO>>> GetEventsByDateAsync(DateTime startTime, DateTime finishTime);
    }
}
