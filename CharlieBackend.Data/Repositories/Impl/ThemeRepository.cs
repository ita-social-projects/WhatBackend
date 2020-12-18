using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class ThemeRepository : Repository<Theme>, IThemeRepository
    {
        public ThemeRepository(ApplicationContext applicationContext) 
            : base(applicationContext) 
        {
        }

        public Task<Theme> GetThemeByNameAsync(string name)
        {
            return _applicationContext.Themes
                    .FirstOrDefaultAsync(theme => theme.Name == name);
        }

        public Task<Theme> GetThemeByIdAsync(long themeId)
        {
            return _applicationContext.Themes
                    .FirstOrDefaultAsync(theme => theme.Id == themeId);
        }

        public Task<bool> IsThemeUsed(long themeId)
        {
            return _applicationContext.Lessons
                    .AnyAsync(les => les.ThemeId == themeId);
        }
    }
}
