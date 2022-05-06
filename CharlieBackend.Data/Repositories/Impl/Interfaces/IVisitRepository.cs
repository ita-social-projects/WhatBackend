using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IVisitRepository : IRepository<Visit>
    {
        public Task DeleteWhereLessonIdAsync(long id);

        public Task<List<Visit>> GetStudentVisits(long studentId);
    }
}
