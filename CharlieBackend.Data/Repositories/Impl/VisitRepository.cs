using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task DeleteWhereLessonIdAsync(long id)
        {
            var visitsToDelete = await _applicationContext.Visits.Where(visit => visit.LessonId == id).ToListAsync();
            _applicationContext.Visits.RemoveRange(visitsToDelete);
        }
    }
}
