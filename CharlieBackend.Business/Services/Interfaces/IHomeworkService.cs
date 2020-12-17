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
        Task<Result<HometaskDto>> CreateHometaskAsync(CreateHometaskDto courseModel);

        Task<Result<IList<HometaskDto>>> GetHometaskOfCourseAsync(long courseId);

        Task<Result<HometaskDto>> GetHometaskByIdAsync(long hometaskId);
    }
}
