using CharlieBackend.Panel.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IList<LessonViewModel>> GetLessonsByDate();
    }
}
