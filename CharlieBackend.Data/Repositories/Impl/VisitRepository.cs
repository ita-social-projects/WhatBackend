using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        { 
        }

        public Task<List<Visit>> GetStudentVisits(long studentId)
        {
            return _applicationContext.Visits
                    .Include(visit => visit.Lesson)
                    .ThenInclude(lesson => lesson.Theme)
                    .Where(visit => visit.StudentId == studentId)
                    .ToListAsync();
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
