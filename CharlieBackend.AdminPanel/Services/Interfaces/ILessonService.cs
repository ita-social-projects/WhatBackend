using CharlieBackend.AdminPanel.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IList<LessonViewModel>> GetAllLessons();
    }
}
