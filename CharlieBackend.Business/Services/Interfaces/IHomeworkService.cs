﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Visit;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<Result<HomeworkDto>> CreateHomeworkAsync(HomeworkRequestDto homeworkDto);

        Task<Result<HomeworkDto>> GetHomeworkByIdAsync(long homeworkId);

        Task<Result<HomeworkDto>> UpdateHomeworkAsync(long homeworkId, HomeworkRequestDto updateHomeworkDto);

        Task<Result<IList<HomeworkDto>>> GetHomeworksByLessonId(long studentGroupId);

        Task<Result<VisitDto>> UpdateMarkAsync(UpdateMarkRequestDto request);

        Task<Result<IList<HomeworkDto>>> GetMentorFilteredHW(long mentorId,
                HomeworkFilterDto filter);

    }
}
