using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<Result<HomeworkDto>> CreateHomeworkAsync(CreateHomeworkDto homeworkDto);

        Task<Result<HomeworkDto>> GetHomeworkByIdAsync(long homeworkId);
    }
}
