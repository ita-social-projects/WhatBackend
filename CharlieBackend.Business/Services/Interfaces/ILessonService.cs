using CharlieBackend.Core.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<LessonModel> CreateLessonAsync(CreateLessonModel lessonModel, long mentorId);

        public Task<IList<LessonModel>> GetAllLessonsAsync();

        public Task<LessonModel> UpdateLessonAsync(UpdateLessonModel lessonModel);

        public Task<IList<StudentLessonModel>> GetStudentLessonsAsync(long studentId);
    }
}
