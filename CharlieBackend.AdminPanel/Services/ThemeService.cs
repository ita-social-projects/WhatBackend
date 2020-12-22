using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        public ThemeService(IApiUtil apiUtil, IOptions<ApplicationSettings> config)
        {
            _apiUtil = apiUtil;
            _config = config;
        }

        public async Task DeleteTheme(long id, string accessToken)
        {
            await
                _apiUtil.DeleteAsync<ThemeViewModel>($"{_config.Value.Urls.Api.Https}/api/themes/{id}", accessToken);
        }

        public async Task UpdateTheme(long id, UpdateThemeDto UpdateDto, string accessToken)
        {
            await
                _apiUtil.PutAsync<UpdateThemeDto>($"{_config.Value.Urls.Api.Https}/api/themes/{id}", UpdateDto, accessToken);
        }

        public async Task<IList<ThemeViewModel>> GetAllThemesAsync(string accessToken)
        {
            return await
                _apiUtil.GetAsync<IList<ThemeViewModel>>($"{_config.Value.Urls.Api.Https}/api/themes", accessToken);
        }

        public async Task AddThemeAsync(CreateThemeDto themeDto, string accessToken)
        {
            await
                _apiUtil.CreateAsync<CreateThemeDto>($"{_config.Value.Urls.Api.Https}/api/themes", themeDto, accessToken);
        }
    }
}
