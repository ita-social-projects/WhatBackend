using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThemeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ThemeDto> CreateThemeAsync(CreateThemeDto themeDto)
        {
            try
            {
                var createdThemeEntity = _mapper.Map<Theme>(themeDto);

                _unitOfWork.ThemeRepository.Add(createdThemeEntity);

                await _unitOfWork.CommitAsync();

                return _mapper.Map<ThemeDto>(createdThemeEntity);
            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<IList<ThemeDto>> GetAllThemesAsync()
        {
            var themes = await _unitOfWork.ThemeRepository.GetAllAsync();


            return _mapper.Map<IList<ThemeDto>>(themes);
        }

        public async Task<ThemeDto> GetThemeByNameAsync(string name)
        {
            var theme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(name);

            return _mapper.Map<ThemeDto>(theme);
        }
    }
}
