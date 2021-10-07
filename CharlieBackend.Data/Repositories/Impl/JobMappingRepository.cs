using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public class JobMappingRepository : Repository<JobMapping>, IJobMappingRepository
    {
        public JobMappingRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }
    }
}
