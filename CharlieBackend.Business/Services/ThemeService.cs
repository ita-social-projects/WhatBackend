using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
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

        public async Task<Result<ThemeDto>> CreateThemeAsync(CreateThemeDto themeDto)
        {
            try
            {
                var createdThemeEntity = _mapper.Map<Theme>(themeDto);

                var Check = _unitOfWork.ThemeRepository.GetThemeByNameAsync(createdThemeEntity.Name).Result;


                if (Check == null)
                {
                    _unitOfWork.ThemeRepository.Add(createdThemeEntity);

                    await _unitOfWork.CommitAsync();

                    return Result<ThemeDto>
                        .GetSuccess(_mapper.Map<ThemeDto>(createdThemeEntity));
                }
                else
                {
                    return Result<ThemeDto>.GetError(ErrorCode.ValidationError, "Validation error");
                }

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ThemeDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<Result<ThemeDto>> DeleteThemeAsync(long themeId)
        {
            var themeEntity = await _unitOfWork.ThemeRepository.GetByIdAsync(themeId);

            if (themeEntity != null)
            {
                if (await _unitOfWork.ThemeRepository.IsThemeUsed(themeEntity.Id))
                {
                    return Result<ThemeDto>.GetError(ErrorCode.ValidationError, "Some lesson used this theme");
                }
                var mappedTheme = _mapper.Map<ThemeDto>(themeEntity);
                await _unitOfWork.ThemeRepository.DeleteAsync(themeId);

                await _unitOfWork.CommitAsync();

                return Result<ThemeDto>.GetSuccess(mappedTheme);
            }

            return Result<ThemeDto>.GetError(ErrorCode.NotFound,
                $"Theme with id={themeId} does not exist");

        }

        public async Task<Result<IList<ThemeDto>>> GetAllThemesAsync()
        {
            var themes = await _unitOfWork.ThemeRepository.GetAllAsync();

            return Result<IList<ThemeDto>>
                .GetSuccess(_mapper.Map<IList<ThemeDto>>(themes));
        }

        public async Task<Result<ThemeDto>> GetThemeByNameAsync(string name)
        {
            var theme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(name);

            if (theme == null)
            {
                return Result<ThemeDto>
                 .GetError(ErrorCode.NotFound,
                    $"Schedule with name = {name} does not exist");
            }

            return Result<ThemeDto>
                .GetSuccess(_mapper.Map<ThemeDto>(theme));
        }

        public async Task<Result<ThemeDto>> UpdateThemeAsync(long themeId, UpdateThemeDto themeDto)
        {
            try
            {
                if (themeDto == null)
                {
                    return Result<ThemeDto>.GetError(ErrorCode.NotFound, "UpdateThemeDto is null");
                }
                var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByIdAsync(themeId);
                if (foundTheme == null)
                {
                    return Result<ThemeDto>.GetError(ErrorCode.NotFound,
                        $"Theme with id={themeId} does not exist");
                }
                foundTheme.Name = themeDto.Name;

                await _unitOfWork.CommitAsync();

                return Result<ThemeDto>.GetSuccess(_mapper.Map<ThemeDto>(foundTheme));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<ThemeDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }
    }
}
