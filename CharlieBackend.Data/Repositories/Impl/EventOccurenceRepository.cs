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
    public class EventOccurenceRepository : Repository<EventOccurence>, IEventOccurenceRepository
    {
        public EventOccurenceRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }

        public new async Task<List<EventOccurence>> GetAllAsync()
        {
            return await _applicationContext.EventOccurences
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }

        public async Task<List<EventOccurence>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
            return await _applicationContext.EventOccurences
                 .Where(schedule => schedule.StudentGroupId == studentGroupId)
                 .Include(schedule => schedule.StudentGroup)
                 .ToListAsync();
        }

        public async Task<List<EventOccurence>> GetEventOccurenceByDateAsync(DateTime startTime, DateTime finishTime)
        {
            return await _applicationContext.EventOccurences
                .AsNoTracking()
                .Where(x => x.EventStart < finishTime && x.EventStart >= startTime ||
                       x.EventFinish > startTime && x.EventFinish <= finishTime)
                .OrderBy(x => x.EventStart)
                .ToListAsync();
        }
    }
}
