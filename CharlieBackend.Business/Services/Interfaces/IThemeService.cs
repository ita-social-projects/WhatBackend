using CharlieBackend.Core.Models.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IThemeService
    {
        Task<ThemeModel> CreateThemeAsync(ThemeModel theme);

        public Task<IList<ThemeModel>> GetAllThemesAsync();

        public Task<ThemeModel> GetThemeByNameAsync(string name);
    }
}
