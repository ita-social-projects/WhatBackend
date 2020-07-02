using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IThemeService
    {
        Task<ThemeModel> CreateThemeAsync(ThemeModel theme);
        public Task<List<ThemeModel>> GetAllThemesAsync();
    }
}
