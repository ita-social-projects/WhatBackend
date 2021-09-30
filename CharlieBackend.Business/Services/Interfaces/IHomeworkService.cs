using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.DTO.HomeworkStudent;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<Result<IList<HomeworkDto>>> GetHomeworks();
        Task<Result<HomeworkDto>> CreateHomeworkAsync(HomeworkRequestDto homeworkDto);

        Task<Result<HomeworkDto>> GetHomeworkByIdAsync(long homeworkId);

        Task<Result<HomeworkDto>> UpdateHomeworkAsync(long homeworkId, HomeworkRequestDto updateHomeworkDto);

        Task<Result<IList<HomeworkDto>>> GetHomeworksByLessonId(long studentGroupId);

    }
}
