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
        public Task<Result<ScheduleDto>> CreateScheduleAsync(CreateScheduleDto scheduleModel);

        public Task<IList<ScheduleDto>> GetAllSchedulesAsync();

        public Task<Result<UpdateScheduleDto>> UpdateStudentGroupAsync(UpdateScheduleDto scheduleModel);

        public Task<Result<ScheduleDto>> GetScheduleByIdAsync(long id);

        public Task<IList<ScheduleDto>> GetSchedulesByStudentGroupIdAsync(long studentGroupId);

        public bool DeleteSchedule(long id);
    }
}
