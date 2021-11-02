using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Importers
{
    public class ThemeImporter
    {
        private readonly IThemeService _themeService;

        public ThemeImporter(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public async Task<Result<IList<ThemeDto>>> ImportThemesAsync(IList<CreateThemeDto> createdThemes)
        {
            var themes = new List<ThemeDto>();

            foreach (var createdTheme in createdThemes)
            {
                var theme = await _themeService.CreateThemeAsync(createdTheme);

                if (theme.Error != null)
                {
                    return Result<IList<ThemeDto>>.GetError(theme.Error.Code, theme.Error.Message);
                }

                themes.Add(theme.Data);
            }

            return Result<IList<ThemeDto>>.GetSuccess(themes);
        }
    }
}

