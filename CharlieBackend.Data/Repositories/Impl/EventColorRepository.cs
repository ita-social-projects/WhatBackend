using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class EventColorRepository: Repository<EventColor>, IEventColorRepository
    {
        public EventColorRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {

        }

        public new async Task<List<EventColor>> GetAllAsync()
        {
            return await _applicationContext.EventColors
                .Include(c => c.EventOccurances).ToListAsync();
        }

        public new async Task<EventColor> GetByIdAsync(long id)
        {
            return await _applicationContext.EventColors
               .FirstOrDefaultAsync(c => c.Id.Equals(id));
        }
    }
}
