using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<LessonModel> CreateLessonAsync(LessonModel lessonModel);
        public Task<List<LessonModel>> GetAllLessonsAsync();
    }
}
