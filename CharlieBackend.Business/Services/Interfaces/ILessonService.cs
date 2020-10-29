using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<LessonModel> CreateLessonAsync(CreateLessonModel lessonModel);

        public Task<IList<LessonModel>> GetAllLessonsAsync();

        public Task<Lesson> AssignMentorToLessonAsync(AssignMentorToLessonModel ids);

        public Task<LessonModel> UpdateLessonAsync(UpdateLessonModel lessonModel);

        public Task<IList<StudentLessonModel>> GetStudentLessonsAsync(long studentId);
        
    }
}
