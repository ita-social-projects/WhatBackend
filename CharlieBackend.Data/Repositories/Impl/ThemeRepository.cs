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

        public async Task<Theme> GetThemeByNameAsync(string name)
        {
            return await _applicationContext.Themes
                    .FirstOrDefaultAsync(theme => theme.Name == name);
        }

        public async Task<Theme> GetThemeByIdAsync(long themeId)
        {
            return await _applicationContext.Themes
                    .FirstOrDefaultAsync(theme => theme.Id == themeId);
        }

        public async Task<bool> IsThemeUsed(long themeId)
        {
            return await _applicationContext.Lessons
                    .AnyAsync(les => les.ThemeId == themeId);
        }
    }
}
