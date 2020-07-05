using CharlieBackend.Core.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<LessonModel> CreateLessonAsync(LessonModel lessonModel);
        public Task<List<LessonModel>> GetAllLessonsAsync();
    }
}
