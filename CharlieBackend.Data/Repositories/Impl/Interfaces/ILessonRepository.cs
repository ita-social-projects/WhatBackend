using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        public Task<IList<StudentLessonModel>> GetStudentInfoAsync(long studentId);
    }
}
