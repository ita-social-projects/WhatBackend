using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class ThemeRepository: Repository<Theme>, IThemeRepository
    {
        public ThemeRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
