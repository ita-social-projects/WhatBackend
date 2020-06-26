using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class SampleRepository: Repository<Sample>, ISampleRepository
    {
        public SampleRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
