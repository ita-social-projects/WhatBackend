using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IEventOccurenceRepository : IRepository<EventOccurence>
    {
        public new Task<List<EventOccurence>> GetAllAsync();

        Task<List<EventOccurence>> GetSchedulesByStudentGroupIdAsync(long id);

        Task<List<EventOccurence>> GetEventOccurenceByDateAsync(DateTime startTime, DateTime finishTime);
    }
}
