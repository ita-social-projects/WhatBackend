using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IEventColorRepository: IRepository<EventColor>
    {
        public new Task<List<EventColor>> GetAllAsync();

        public new Task<EventColor> GetByIdAsync(long id);
    }
}
