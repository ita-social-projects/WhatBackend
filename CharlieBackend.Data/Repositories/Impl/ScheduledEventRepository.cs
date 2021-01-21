using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Repositories.Impl
{
    class ScheduledEventRepository : Repository<ScheduledEvent>, IScheduledEventRepository
    {
        public ScheduledEventRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }
    }
}
