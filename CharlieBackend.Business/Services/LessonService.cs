using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IThemeService _themeService;

        public LessonService(IUnitOfWork unitOfWork, IThemeService themeService)
        {
            _unitOfWork = unitOfWork;
            _themeService = themeService;
        }

        public async Task<LessonModel> CreateLessonAsync(LessonModel lessonModel)
        {
            try
            {
                // TODO: add mentor id, theme id
                //var createdTheme = await _themeService.CreateThemeAsync(new Theme { Name = lessonModel.name });
                _unitOfWork.LessonRepository.Add(lessonModel.ToLesson());
                await _unitOfWork.CommitAsync();
                return lessonModel;
            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<List<LessonModel>> GetAllLessonsAsync()
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync();

            var lessonsModels = new List<LessonModel>();
            foreach (var lesson in lessons) { lessonsModels.Add(lesson.ToLessonModel()); }

            return lessonsModels;
        }
    }
}
