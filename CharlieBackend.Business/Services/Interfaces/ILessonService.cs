using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ILessonService
    {
        Task<Result<LessonDto>> CreateLessonAsync(CreateLessonDto lessonModel);

        Task<Result<IList<LessonDto>>> GetAllLessonsForMentor(long mentorId);

        Task<Result<IList<LessonDto>>> GetAllLessonsAsync();

        Task<Result<Lesson>> AssignMentorToLessonAsync(AssignMentorToLessonDto ids);

        Task<Result<LessonDto>> UpdateLessonAsync(long id, UpdateLessonDto lessonModel);

        Task<Result<IList<StudentLessonDto>>> GetStudentLessonsAsync(long studentId);

        Task<IList<LessonDto>> GetLessonsForMentorAsync(FilterLessonsRequestDto filterModel);

        Task<IList<LessonDto>> GetLessonsForStudentAsync(FilterLessonsRequestDto filterModel);

        Task<Result<LessonDto>> GetLessonByIdAsync(long lessonId);

        Task<Result<bool>> IsLessonDoneAsync(long id);

    }
}
