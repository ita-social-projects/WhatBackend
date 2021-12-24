using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.ScheduledEvent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IList<LessonViewModel>> GetLessonsByDateAsync();
        Task<LessonDto> GetLessonByIdAsync(long id);
        Task AddLessonEndpointAsync(LessonCreateViewModel createLesson);
        Task UpdateLessonEndpointAsync(long id, LessonUpdateViewModel lessonToUpdate);
        Task<LessonCreateViewModel> PrepareLessonAddAsync(long stGrId);
        Task<LessonVisitModel> LessonVisitsAsync(long id);
        Task<IList<ScheduledEventViewModel>> Get2DaysEventsAsync();
        Task<IList<LessonViewModel>> GetAllLessonsForMentorAsync(long mentorId);
        Task<IList<StudentLessonViewModel>> GetStudentLessonsAsync(long studentId);
        Task<LessonUpdateViewModel> PrepareLessonUpdateAsync(long id);
    }
}
