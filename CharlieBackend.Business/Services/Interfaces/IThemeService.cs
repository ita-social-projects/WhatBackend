using CharlieBackend.Core.DTO.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IThemeService
    {
        Task<ThemeDto> CreateThemeAsync(CreateThemeDto theme);

        public Task<IList<ThemeDto>> GetAllThemesAsync();

        public Task<ThemeDto> GetThemeByNameAsync(string name);
    }
}
