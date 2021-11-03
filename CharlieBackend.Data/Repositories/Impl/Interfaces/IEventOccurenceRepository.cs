using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IEventOccurrenceRepository : IRepository<EventOccurrence>
    {
        public new Task<List<EventOccurrence>> GetAllAsync();

        Task<List<EventOccurrence>> GetByStudentGroupIdAsync(long studentGroupId);

        Task<List<EventOccurrence>> GetSchedulesByStudentGroupIdAsync(long id);
    }
}
