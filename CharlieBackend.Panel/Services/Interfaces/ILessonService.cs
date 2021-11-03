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
        Task<LessonViewModel> GetLessonById(long id);
        Task AddLessonEndpoint(LessonCreateViewModel createLesson);
        Task UpdateLessonEndpoint(long id, LessonDto lessonDto);
        Task<LessonCreateViewModel> PrepareLessonAddAsync();
        Task<LessonVisitModel> LessonVisits(long id);
        Task<IList<ScheduledEventViewModel>> Get2MounthEvents();
        //Task<IList<StudentViewModel>> StudentsPartial(long id);
        Task<IList<LessonViewModel>> GetLessonsForMentorAsync(FilterLessonsRequestDto filterModel);
    }
}
