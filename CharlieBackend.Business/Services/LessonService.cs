using System;
using AutoMapper;
using CharlieBackend.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Diagnostics;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Lesson;

namespace CharlieBackend.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LessonDto> CreateLessonAsync(CreateLessonDto lessonDto)
        {
            try
            {
                Theme theme;
                var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(lessonDto.ThemeName);

                if (foundTheme != null)
                {
                    theme = foundTheme;
                }
                else
                {
                    theme = new Theme { Name = lessonDto.ThemeName };

                    _unitOfWork.ThemeRepository.Add(theme);
                }

                var lesson = new Lesson
                {
                    MentorId = lessonDto.MentorId,
                    StudentGroupId = lessonDto.StudentGroupId,
                    LessonDate = lessonDto.LessonDate,
                    Theme = theme
                };

                _unitOfWork.LessonRepository.Add(lesson);

                if (lessonDto.LessonVisits != null)
                {
                    for (int i = 0; i < lessonDto.LessonVisits.Count; i++)
                    {
                        var visit = new Visit
                        {
                            Lesson = lesson,
                            StudentId = lessonDto.LessonVisits[i].StudentId,
                            Comment = lessonDto.LessonVisits[i].Comment,
                            Presence = lessonDto.LessonVisits[i].Presence,
                            StudentMark = lessonDto.LessonVisits[i].StudentMark
                        };

                        _unitOfWork.VisitRepository.Add(visit);
                    }

                }

                await _unitOfWork.CommitAsync();

                return _mapper.Map<LessonDto>(lessonDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                Console.WriteLine("Debug info exception details: " + ex);
                return null;
            }
        }

        public async Task<IList<LessonDto>> GetAllLessonsAsync()
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync();

            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<Lesson> AssignMentorToLessonAsync(AssignMentorToLessonDto ids)
        {
            var mentorToAssign = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(ids.MentorId);

            if (mentorToAssign == null)
            {
                throw new NullReferenceException();
            }
            var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(ids.LessonId);

            foundLesson.MentorId = ids.MentorId;

            await _unitOfWork.CommitAsync();

            return foundLesson;
        }

        public async Task<LessonDto> UpdateLessonAsync(UpdateLessonDto lessonModel)
        {
            try
            {
                var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonModel.Id);

                if (foundLesson == null)
                {
                    return null;
                }

                if (!String.IsNullOrEmpty(lessonModel.ThemeName))
                {
                    var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(lessonModel.ThemeName);

                    if (foundTheme != null)
                    {
                        foundLesson.Theme = foundTheme;
                    }
                    else
                    {
                        var theme = new Theme
                        {
                            Name = lessonModel.ThemeName
                        };

                        _unitOfWork.ThemeRepository.Add(theme);
                        foundLesson.Theme = theme;
                    }
                }

                if (lessonModel.LessonDate != default(DateTime))
                {
                    foundLesson.LessonDate = lessonModel.LessonDate;
                }

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

                return _mapper.Map<LessonDto>(lessonModel);

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<IList<StudentLessonDto>> GetStudentLessonsAsync(long studentId)
        {
            var studentLessonModels = await _unitOfWork.LessonRepository.GetStudentInfoAsync(studentId);

            return _mapper.Map<IList<StudentLessonDto>>(studentLessonModels) ?? null;
        }

    }
}
