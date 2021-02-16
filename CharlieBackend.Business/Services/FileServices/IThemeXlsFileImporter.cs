using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices
{
    public interface IThemeXlsFileImporter
    {
        Task<Result<IEnumerable<ThemeDto>>> ImportThemesAsync(IFormFile file);
    }
}
