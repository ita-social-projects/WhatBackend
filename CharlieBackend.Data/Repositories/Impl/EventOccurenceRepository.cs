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

        public new async Task<List<EventOccurrence>> GetAllAsync()
        {
            return await _applicationContext.EventOccurrences
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public async Task<List<EventOccurrence>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
           return await _applicationContext.EventOccurrences
                .Where(schedule => schedule.StudentGroupId == studentGroupId)
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public new async Task<EventOccurrence> GetByIdAsync(long id)
        {
            return await _applicationContext.EventOccurrences
                .Include(x => x.ScheduledEvents)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
