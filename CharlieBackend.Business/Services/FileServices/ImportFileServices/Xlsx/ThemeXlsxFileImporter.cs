using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ImportFileServices.Xlsx
{
    public class ThemeXlsxFileImporter
    {
        private readonly IThemeService _themeService;

        public ThemeXlsxFileImporter(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(string filePath)
        {
            var themes = new List<ThemeDto>();

            using (IXLWorkbook book = new XLWorkbook(filePath))
            {
                foreach (IXLRow row in book.Worksheet(1).RowsUsed().Skip(1))
                {
                    var theme = await _themeService.CreateThemeAsync(new CreateThemeDto
                    {
                        Name = row.Cell(1).GetValue<string>(),
                    });

                    if (theme.Error != null)
                    {
                        return Result<IEnumerable<ThemeDto>>.GetError(theme.Error.Code, theme.Error.Message);
                    }

                    themes.Add(theme.Data);
                }
            }

            return Result<IEnumerable<ThemeDto>>.GetSuccess(themes);
        }

    }
}
