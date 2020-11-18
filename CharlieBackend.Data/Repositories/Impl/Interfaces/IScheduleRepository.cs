using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        public new Task<List<Schedule>> GetAllAsync();
        Task<List<Schedule>> GetSchedulesByStudentGroupIdAsync(long id);
    }
}
