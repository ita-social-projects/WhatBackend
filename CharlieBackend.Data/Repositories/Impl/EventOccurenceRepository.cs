using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class EventOccurenceRepository : Repository<EventOccurrence>, IEventOccurenceRepository
    {
        public EventOccurenceRepository(ApplicationContext applicationContext) 
                : base(applicationContext)
        {
        }

        public new Task<List<EventOccurrence>> GetAllAsync()
        {
            return _applicationContext.EventOccurences
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public Task<List<EventOccurrence>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
           return _applicationContext.EventOccurences
                .Where(schedule => schedule.StudentGroupId == studentGroupId)
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public new Task<EventOccurence> GetByIdAsync(long id)
        {
            return _applicationContext.EventOccurences
                .Include(x => x.ScheduledEvents)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
