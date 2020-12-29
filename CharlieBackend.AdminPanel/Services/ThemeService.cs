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
        private readonly IOptions<ApplicationSettings> _config;
        private readonly IDataProtector _protector;
        private readonly string _accessToken;

        public ThemeService(IApiUtil apiUtil, 
                            IOptions<ApplicationSettings> config, 
                            IHttpContextAccessor httpContextAccessor,
                            IDataProtectionProvider provider)
        {
            _apiUtil = apiUtil;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

            _accessToken = _protector.Unprotect(httpContextAccessor.HttpContext.Request.Cookies["accessToken"]);
        }

        public async Task DeleteTheme(long id)
        {
            await
                _apiUtil.DeleteAsync<ThemeViewModel>($"{_config.Value.Urls.Api.Https}/api/themes/{id}", _accessToken);
        }

        public async Task UpdateTheme(long id, UpdateThemeDto UpdateDto)
        {
            await
                _apiUtil.PutAsync<UpdateThemeDto>($"{_config.Value.Urls.Api.Https}/api/themes/{id}", UpdateDto, _accessToken);
        }

        public async Task<IList<ThemeViewModel>> GetAllThemesAsync()
        {
            return await
                _apiUtil.GetAsync<IList<ThemeViewModel>>($"{_config.Value.Urls.Api.Https}/api/themes", _accessToken);
        }

        public async Task AddThemeAsync(CreateThemeDto themeDto)
        {
            await
                _apiUtil.CreateAsync<CreateThemeDto>($"{_config.Value.Urls.Api.Https}/api/themes", themeDto, _accessToken);
        }
    }
}
