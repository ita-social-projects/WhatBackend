using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private ISampleRepository _sampleRepository;
        public UnitOfWork(ApplicationContext applicationContext, ISampleRepository sampleRepository)
        {
            _applicationContext = applicationContext;
            _sampleRepository = sampleRepository;
        }
        public ISampleRepository SampleRepository
        {
            get { return _sampleRepository = _sampleRepository ?? new SampleRepository(_applicationContext); }
        }

        public void Commit()
        {
            _applicationContext.SaveChanges();
        }

        public void Rollback()
        {
            _applicationContext.Dispose();
        }
    }
}
