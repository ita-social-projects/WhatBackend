using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Panel.Models.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IThemeService
    {
        Task AddThemeAsync(CreateThemeDto themeDto);

        Task<IList<ThemeViewModel>> GetAllThemesAsync();

        Task DeleteTheme(long id);

        Task UpdateTheme(long id, UpdateThemeDto UpdateDto);
    }
}
