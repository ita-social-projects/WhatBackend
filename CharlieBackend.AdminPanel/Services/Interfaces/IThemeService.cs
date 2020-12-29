using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.Core.DTO.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IThemeService
    {
        Task AddThemeAsync(CreateThemeDto themeDto);

        Task<IList<ThemeViewModel>> GetAllThemesAsync();

        Task DeleteTheme(long id);

        Task UpdateTheme(long id, UpdateThemeDto UpdateDto);
    }
}
