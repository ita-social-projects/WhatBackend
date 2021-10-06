using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Csv
{
    public class ThemeCsvFileImporter
    {
        private readonly IThemeService _themeService;

        public ThemeCsvFileImporter(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(
            string filePath)
        {
            var themes = new List<ThemeDto>();

            using StreamReader stream = new StreamReader(filePath);

            string[] themeInfo = null;

            while (stream.Peek() != -1)
            {
                themeInfo = stream.ReadLine().Trim('\n').Split(';');

                var theme = await _themeService.CreateThemeAsync(
                    new CreateThemeDto() { Name = themeInfo[0] });

                if (theme.Error != null)
                {
                    return Result<IEnumerable<ThemeDto>>.GetError(
                        theme.Error.Code, theme.Error.Message);
                }

                themes.Add(theme.Data);
            }

            return Result<IEnumerable<ThemeDto>>.GetSuccess(themes);
        }
    }
}
