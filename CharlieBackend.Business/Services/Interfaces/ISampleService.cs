using CharlieBackend.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ISampleService
    {
        Task<List<Sample>> GetAllAsync();
        Task<Sample> InsertAsync(Sample sample);
        void Delete(Sample sample);
    }
}
