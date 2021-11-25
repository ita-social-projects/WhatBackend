using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.ScheduledEvent;
using CharlieBackend.Panel.Models.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IList<LessonViewModel>> GetLessonsByDate();
        Task<LessonDto> GetLessonById(long id);
        Task AddLessonEndpoint(LessonCreateViewModel createLesson);
        Task UpdateLessonEndpoint(long id, LessonUpdateViewModel lessonToUpdate);
        Task<LessonCreateViewModel> PrepareLessonAddAsync(long stGrId);
        Task<LessonVisitModel> LessonVisits(long id);
        Task<IList<ScheduledEventViewModel>> Get2DaysEvents();
        Task<IList<LessonViewModel>> GetAllLessonsForMentor(long mentorId);
        Task<IList<StudentLessonViewModel>> GetStudentLessonsAsync(long studentId);
        Task<LessonUpdateViewModel> PrepareLessonUpdateAsync(long id);
    }
}
