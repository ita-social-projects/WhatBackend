using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        public Task<List<StudentLessonModel>> GetStudentInfoAsync(long studentId);
    }
}
