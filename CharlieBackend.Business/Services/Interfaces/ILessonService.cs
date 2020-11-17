﻿using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<LessonDto> CreateLessonAsync(CreateLessonDto lessonModel);

        public Task<IList<LessonDto>> GetAllLessonsAsync();

        public Task<Result<Lesson>> AssignMentorToLessonAsync(AssignMentorToLessonDto ids);

        public Task<LessonDto> UpdateLessonAsync(UpdateLessonDto lessonModel);

        public Task<IList<StudentLessonDto>> GetStudentLessonsAsync(long studentId);
    }
}
