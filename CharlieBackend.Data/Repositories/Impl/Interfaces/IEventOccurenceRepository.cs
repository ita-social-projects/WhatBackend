using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IEventOccurrenceRepository : IRepository<EventOccurrence>
    {
        public new Task<List<EventOccurrence>> GetAllAsync();

        Task<List<EventOccurrence>> GetSchedulesByStudentGroupIdAsync(long id);
    }
}
