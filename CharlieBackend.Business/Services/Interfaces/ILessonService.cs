using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<Result<LessonDto>> CreateLessonAsync(CreateLessonDto lessonModel);

        public Task<Result<IList<LessonDto>>> GetAllLessonsAsync();

        public Task<Result<Lesson>> AssignMentorToLessonAsync(AssignMentorToLessonDto ids);

        public Task<Result<LessonDto>> UpdateLessonAsync(long id, UpdateLessonDto lessonModel);

        public Task<Result<IList<StudentLessonDto>>> GetStudentLessonsAsync(long studentId);
        
    }
}
