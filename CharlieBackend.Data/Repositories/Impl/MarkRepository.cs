using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }
    }
}
