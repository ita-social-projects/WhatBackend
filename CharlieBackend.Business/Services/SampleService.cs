using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class SampleService : ISampleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SampleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(Sample sample)
        {
            try
            {
                _unitOfWork.SampleRepository.Delete(sample);
                _unitOfWork.Commit();
            } catch { _unitOfWork.Rollback(); }
        }

        public Task<List<Sample>> GetAllAsync()
        {
            return _unitOfWork.SampleRepository.GetAllAsync();
        }
        public async Task<Sample> InsertAsync(Sample sample)
        {
            try
            {
                var inserted = await _unitOfWork.SampleRepository.InsertAsync(sample);
                _unitOfWork.Commit();
                return inserted;
            } catch { _unitOfWork.Rollback(); return sample; }
        }
    }
}
