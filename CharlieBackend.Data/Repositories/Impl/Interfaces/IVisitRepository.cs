using CharlieBackend.Core.Entities;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IVisitRepository : IRepository<Visit>
    {
        public Task DeleteWhereLessonIdAsync(long id);
    }
}
