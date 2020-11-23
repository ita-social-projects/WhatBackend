using CharlieBackend.AdminPanel.Models.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IThemeService
    {
        Task<IList<ThemeViewModel>> GetAllThemesAsync(string accessToken);
        Task DeleteTheme(long id, string accessToken);
    }
}
