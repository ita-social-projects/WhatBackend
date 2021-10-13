using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IJobMappingRepository : IRepository<JobMapping>
    {
        Task<List<string>> GetHangfireIdsByCustomJobId(string customJobId);

        Task DeleteByCustomJobId(string customJobId);

        Task<bool> DeleteByCustomJobId2(string customJobId);
    }
}
