using CharlieBackend.Core.Models.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IThemeService
    {
        Task<ThemeModel> CreateThemeAsync(ThemeModel theme);
        public Task<List<ThemeModel>> GetAllThemesAsync();
        public Task<ThemeModel> GetThemeByNameAsync(string name);
    }
}
