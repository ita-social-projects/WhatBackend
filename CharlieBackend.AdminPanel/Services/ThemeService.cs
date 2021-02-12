using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IApiUtil _apiUtil;

        public ThemeService(IApiUtil apiUtil)
        {
            _apiUtil = apiUtil;
        }

        public async Task DeleteTheme(long id)
        {
            await
                _apiUtil.DeleteAsync<ThemeViewModel>($"api/themes/{id}");
        }

        public async Task UpdateTheme(long id, UpdateThemeDto UpdateDto)
        {
            await
                _apiUtil.PutAsync<UpdateThemeDto>($"api/themes/{id}", UpdateDto);
        }

        public async Task<IList<ThemeViewModel>> GetAllThemesAsync()
        {
            return await
                _apiUtil.GetAsync<IList<ThemeViewModel>>($"api/themes");
        }

        public async Task AddThemeAsync(CreateThemeDto themeDto)
        {
            await
                _apiUtil.CreateAsync<CreateThemeDto>($"api/themes", themeDto);
        }
    }
}
