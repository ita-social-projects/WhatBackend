using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public class ThemeXlsFileImporter : IThemeXlsFileImporter
    {
        private readonly IBaseFileService _baseFileService;
        private readonly IThemeService _themeService;
        private readonly IUnitOfWork _unitOfWork;

        public ThemeXlsFileImporter(IBaseFileService baseFileService,
                                 IThemeService themeService,
                                 IUnitOfWork unitOfWork)
        {
            _baseFileService = baseFileService;
            _themeService = themeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file)
        {
            if (file == null)
            {
                return Result<IEnumerable<ThemeDto>>.GetError(ErrorCode.ValidationError, "File was not provided");
            }

            if (!_baseFileService.IsFileExtensionValid(file))
            {
                return Result<IEnumerable<ThemeDto>>.GetError(ErrorCode.ValidationError, "File extension not supported");
            }

            var filePath = await _baseFileService.UploadFileAsync(file);

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

            File.Delete(filePath);

            await _unitOfWork.CommitAsync();

            return Result<IEnumerable<ThemeDto>>.GetSuccess(themes);
        }
    }
}
