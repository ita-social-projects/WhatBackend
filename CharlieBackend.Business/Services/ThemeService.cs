using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
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

        public async Task<Theme> CreateThemeAsync(Theme theme)
        {
            try
            {
                await _unitOfWork.ThemeRepository.PostAsync(theme);
                _unitOfWork.Commit();
                return theme;
            } catch { _unitOfWork.Rollback(); return null; }
        }
    }
}
