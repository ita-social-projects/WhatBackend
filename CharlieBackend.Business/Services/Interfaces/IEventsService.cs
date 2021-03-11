using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IEventsService
    {
        public Task<Result<ScheduledEventDTO>> UpdateScheduledEventByID(long scheduledEvendID, UpdateScheduledEventDto scheduleModel);

        public Task<Result<bool>> DeleteConcreteScheduleByIdAsync(long id);

        public Task<Result<ScheduledEventDTO>> GetConcreteScheduleByIdAsync(long eventId);

    }
}
