using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Dashboard;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<Result<DashboardResultDto>> GetStudentsResultAsync(DashboardRequestDto request);
    }
}
