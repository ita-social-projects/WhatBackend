using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IThemeService
    {
        Task<ThemeDto> CreateThemeAsync(CreateThemeDto theme);

        public Task<IList<ThemeDto>> GetAllThemesAsync();

        public Task<ThemeDto> GetThemeByNameAsync(string name);

        public Task<Result<ThemeDto>> UpdateThemeAsync(long themeId, UpdateThemeDto themeDTO);

        public Task<Result<ThemeDto>> DeleteThemeAsync(long themeId);
    }
}
