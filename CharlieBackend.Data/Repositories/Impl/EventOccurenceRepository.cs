using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class EventOccurrenceRepository : Repository<EventOccurrence>, IEventOccurrenceRepository
    {
        public EventOccurrenceRepository(ApplicationContext applicationContext) 
                : base(applicationContext)
        {
        }

        public new Task<List<EventOccurrence>> GetAllAsync()
        {
            return _applicationContext.EventOccurrences
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public Task<List<EventOccurrence>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
           return _applicationContext.EventOccurrences
                .Where(schedule => schedule.StudentGroupId == studentGroupId)
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public new Task<EventOccurrence> GetByIdAsync(long id)
        {
            return _applicationContext.EventOccurrences
                .Include(x => x.ScheduledEvents)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
