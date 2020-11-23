using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
                _apiUtil.DeleteAsync<ThemeViewModel>($"{_config.Value.Urls.Api.Https}/api/themes", accessToken);
        }

        public async Task<IList<ThemeViewModel>> GetAllThemesAsync(string accessToken)
        {
            return await
                _apiUtil.GetAsync<IList<ThemeViewModel>>($"{_config.Value.Urls.Api.Https}/api/themes", accessToken);
        }
    }
}
