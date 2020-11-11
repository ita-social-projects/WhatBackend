using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Data.Helpers;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationContext applicationContext) 
                : base(applicationContext)
        {
        }

        public new Task<List<Schedule>> GetAllAsync()
        {
            return _applicationContext.Schedules
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }
        public new Task<Schedule> GetByIdAsync(long id)
        {
            return _applicationContext.Schedules
                .Include(schedule => schedule.StudentGroup)
                .FirstOrDefaultAsync(schedule => schedule.Id == id);
        }

        public Task<List<Schedule>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
           return _applicationContext.Schedules
                .Where(schedule => schedule.StudentGroupId == studentGroupId)
                .Include(schedule => schedule.StudentGroup)
                .ToListAsync();
        }
    }
}
