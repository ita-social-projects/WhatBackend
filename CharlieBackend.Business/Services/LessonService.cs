using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LessonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LessonModel> CreateLessonAsync(CreateLessonModel lessonModel, long mentorId)
        {
            try
            {
                Theme theme;
                var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(lessonModel.ThemeName);
                if (foundTheme != null) theme = foundTheme;
                else
                {
                    theme = new Theme { Name = lessonModel.ThemeName };
                    _unitOfWork.ThemeRepository.Add(theme);
                }

                var lesson = new Lesson
                {
                    MentorId = mentorId,
                    StudentGroupId = lessonModel.StudentGroupId,
                    LessonDate = Convert.ToDateTime(lessonModel.LessonDate),
                    Theme = theme
                };
                _unitOfWork.LessonRepository.Add(lesson);

                if (lessonModel.LessonVisits != null)
                {
                    for (int i = 0; i < lessonModel.LessonVisits.Count; i++)
                    {
                        var visit = new Visit
                        {
                            Lesson = lesson,
                            StudentId = lessonModel.LessonVisits[i].StudentId,
                            Comment = lessonModel.LessonVisits[i].Comment,
                            Presence = lessonModel.LessonVisits[i].Presence,
                            StudentMark = lessonModel.LessonVisits[i].StudentMark
                        };
                        _unitOfWork.VisitRepository.Add(visit);
                    }

                }
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

        public async Task<LessonModel> UpdateMentorAsync(UpdateLessonModel lessonModel)
        {
            try
            {
                var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonModel.Id);
                if (foundLesson == null) return null;

                if (!String.IsNullOrEmpty(lessonModel.ThemeName))
                {
                    var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(lessonModel.ThemeName);
                    if (foundTheme != null) foundLesson.Theme = foundTheme;
                    else
                    {
                        var theme = new Theme { Name = lessonModel.ThemeName };
                        _unitOfWork.ThemeRepository.Add(theme);
                        foundLesson.Theme = theme;
                    }
                }

                if (!String.IsNullOrEmpty(lessonModel.LessonDate))
                    foundLesson.LessonDate = Convert.ToDateTime(lessonModel.LessonDate);

                if (lessonModel.LessonVisits != null)
                {
                    await _unitOfWork.VisitRepository.DeleteWhereLessonIdAsync(foundLesson.Id);

                    for (int i = 0; i < lessonModel.LessonVisits.Count; i++)
                    {
                        var visit = new Visit
                        {
                            Lesson = foundLesson,
                            StudentId = lessonModel.LessonVisits[i].StudentId,
                            Comment = lessonModel.LessonVisits[i].Comment,
                            Presence = lessonModel.LessonVisits[i].Presence,
                            StudentMark = lessonModel.LessonVisits[i].StudentMark
                        };
                        _unitOfWork.VisitRepository.Add(visit);
                    }
                }
                await _unitOfWork.CommitAsync();
                return lessonModel;

            }
            catch { _unitOfWork.Rollback(); return null; }
        }
    }
}
