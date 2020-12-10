using CharlieBackend.Core.Entities;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IThemeRepository : IRepository<Theme>
    {
        public Task<Theme> GetThemeByNameAsync(string name);
        public Task<Theme> GetThemeByIdAsync(long themeId);
        Task<bool> IsThemeUsed(long themeId);
    }
}
