using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public class JobMappingRepository : Repository<JobMapping>, IJobMappingRepository
    {
        public JobMappingRepository(ApplicationContext applicationContext)
                : base(applicationContext)
        {
        }

        public async Task<List<string>> GetHangfireIdsByCustomJobId(string customJobId)
        {
            return await _applicationContext.JobMappings
                .Where(x => x.CustomJobID == customJobId)
                .Select(x => x.HangfireJobID)
                .ToListAsync();
        }

        public async Task DeleteByCustomJobId(string customJobId)
        {
            _applicationContext.JobMappings
              .RemoveRange( _applicationContext.JobMappings.Where(x => x.CustomJobID == customJobId));
        }

        public async Task<bool> DeleteByCustomJobId2(string customJobId)
        {
            _applicationContext.JobMappings
              .RemoveRange(_applicationContext.JobMappings.Where(x => x.CustomJobID == customJobId));

            return true;
        }
    }
}
