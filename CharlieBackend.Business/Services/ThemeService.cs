using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThemeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ThemeModel> CreateThemeAsync(ThemeModel theme)
        {
            try
            {
                _unitOfWork.ThemeRepository.Add(theme.ToTheme());
                await _unitOfWork.CommitAsync();
                return theme;
            } catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<List<ThemeModel>> GetAllThemesAsync()
        {
            var themes = await _unitOfWork.ThemeRepository.GetAllAsync();

            var themeModels = new List<ThemeModel>();
            foreach (var theme in themes) { themeModels.Add(theme.ToThemeModel()); }

            return themeModels;
        }
    }
}
