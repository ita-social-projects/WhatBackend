using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Theme;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
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

        public async Task<ThemeModel> CreateThemeAsync(ThemeModel theme)
        {
            try
            {

                _unitOfWork.ThemeRepository.Add(_mapper.Map<Theme>(theme));

                await _unitOfWork.CommitAsync();

                return theme;
            }
            catch 
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<IList<ThemeModel>> GetAllThemesAsync()
        {
            var themes = await _unitOfWork.ThemeRepository.GetAllAsync();

            var themeModels = new List<ThemeModel>();

            foreach (var theme in themes)
            {
                themeModels.Add(_mapper.Map<ThemeModel>(theme));
            }

            return themeModels;
        }

        public async Task<ThemeModel> GetThemeByNameAsync(string name)
        {
            var theme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(name);

            return _mapper.Map<ThemeModel>(theme);
        }
    }
}
