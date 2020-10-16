using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IVisitRepository : IRepository<Visit>
    {
        public Task DeleteWhereLessonIdAsync(long id);
    }
}
