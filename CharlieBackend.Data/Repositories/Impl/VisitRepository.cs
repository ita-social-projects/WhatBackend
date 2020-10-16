using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        { 
        }

        public async Task DeleteWhereLessonIdAsync(long id)
        {
            var visitsToDelete = await _applicationContext.Visits
                    .Where(visit => visit.LessonId == id)
                    .ToListAsync();
            _applicationContext.Visits.RemoveRange(visitsToDelete);
        }
    }
}
