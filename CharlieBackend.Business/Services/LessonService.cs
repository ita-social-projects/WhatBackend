using System;
using AutoMapper;
using CharlieBackend.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Diagnostics;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Visit;
using System.Linq;
using System.Security.Claims;

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

        public async Task<Result<LessonDto>> CreateLessonAsync(CreateLessonDto lessonDto)
        {
            try
            {
                var createdLessonEntity = _mapper.Map<Lesson>(lessonDto);

                var foundTheme = await _unitOfWork.ThemeRepository.GetThemeByNameAsync(createdLessonEntity.Theme?.Name);

                if (foundTheme == null)
                {
                    _unitOfWork.ThemeRepository.Add(createdLessonEntity.Theme);
                }
                else
                {
                    createdLessonEntity.Theme = foundTheme;
                }
                
                var foundMentor = await _unitOfWork.MentorRepository.GetMentorByIdAsync(lessonDto.MentorId);

                if (foundMentor == null)
                {
                    return Result<LessonDto>.GetError(ErrorCode.NotFound, "Mentor is not found"); // return Result<T>
                }

                var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(lessonDto.StudentGroupId);

                if (foundStudentGroup == null)
                {
                    return Result<LessonDto>.GetError(ErrorCode.NotFound, "Student group is not found");
                }

                if (foundStudentGroup.StudentsOfStudentGroups.Count != lessonDto.LessonVisits.Count)
                {
                    return Result<LessonDto>.GetError(ErrorCode.ValidationError, $"You sent Lesson Visit with {lessonDto.LessonVisits.Count} student(s)" +
                        $" but in group included {foundStudentGroup.StudentsOfStudentGroups.Count} student(s). Please check your Lesson Visit");
                }
                var checkStudentInGroup = await _unitOfWork.StudentGroupRepository.GetGroupStudentsIds(foundStudentGroup.Id);
                var checkStudents = GetStudentsIDNotIncludeInGroup(lessonDto.LessonVisits, checkStudentInGroup);
                if (checkStudents.Count > 0)
                {
                    return Result<LessonDto>.GetError(ErrorCode.NotFound, $"Student(s) with Id(s) {string.Join(" ,", checkStudents)} not included in this group({lessonDto.StudentGroupId})");
                }

                if (createdLessonEntity.LessonDate < DateTime.Now)
                {
                    return Result<LessonDto>.GetError(ErrorCode.ValidationError, "Lesson date is incorrect");
                }

                _unitOfWork.LessonRepository.Add(createdLessonEntity);

                if (lessonDto.LessonVisits != null)
                {
                    for (int i = 0; i < createdLessonEntity.Visits.Count; i++)
                    {
                        createdLessonEntity.Visits[i].Lesson = createdLessonEntity;

                        _unitOfWork.VisitRepository.Add(createdLessonEntity.Visits[i]);
                    }
                }

                await _unitOfWork.CommitAsync();

                return Result<LessonDto>.GetSuccess(_mapper.Map<LessonDto>(createdLessonEntity));
            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<Result<IList<LessonDto>>> GetAllLessonsAsync()
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync();
            var listLessons = _mapper.Map<IList<LessonDto>>(lessons);

            return Result<IList<LessonDto>>.GetSuccess(listLessons);
        }

        public async Task<Result<IList<LessonDto>>> GetAllLessonsForMentor(long mentorId)
        {
            if (mentorId == default)
            {
                return Result<IList<LessonDto>>.GetError(ErrorCode.ValidationError, "Write Mentor Id please");
            }
            if (await _unitOfWork.MentorRepository.GetByIdAsync(mentorId) == null)
            {
                return Result<IList<LessonDto>>.GetError(ErrorCode.ValidationError, $"Mentor with id {mentorId} is not Found");
            }

            var lessons = await _unitOfWork.LessonRepository.GetAllLessonsForMentor(mentorId);

            return Result<IList<LessonDto>>.GetSuccess(_mapper.Map<IList<LessonDto>>(lessons));
        }

        public async Task<IList<LessonDto>> GetLessonsForMentorAsync(FilterLessonsRequestDto filterModel, ClaimsPrincipal userContext)
        {
            long accountId = Convert.ToInt32(userContext.Claims.First(x => x.Type.EndsWith("AccountId")).Value);
            var mentor = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(accountId);
                      
            if (filterModel == default)
            {
                return _mapper.Map<IList<LessonDto>>(await _unitOfWork.LessonRepository.GetAllLessonsForMentor(mentor.Id));
            }

            var lessonsForMentro = await _unitOfWork.LessonRepository.GetLessonsForMentorAsync(filterModel.StudentGroupId, filterModel.StartDate, filterModel.FinishDate, mentor.Id);

            return _mapper.Map<IList<LessonDto>>(lessonsForMentro);
        }
         
        public async Task<Result<Lesson>> AssignMentorToLessonAsync(AssignMentorToLessonDto ids)
        {
            var mentorToAssign = await _unitOfWork.MentorRepository.GetMentorByAccountIdAsync(ids.MentorId);

            if (mentorToAssign == null)
            {
                throw new NullReferenceException();
            }
            var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(ids.LessonId);

            foundLesson.MentorId = ids.MentorId;

            await _unitOfWork.CommitAsync();

            return Result<Lesson>.GetSuccess(foundLesson);
        }

        public async Task<Result<LessonDto>> UpdateLessonAsync(long id, UpdateLessonDto lessonModel)
        {
            try
            {
                var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);

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
               
                if (lessonModel.LessonDate < DateTime.Now)
                {
                    return Result<LessonDto>.GetError(ErrorCode.ValidationError, "Lesson date is incorrect");
                }

                if (lessonModel.LessonDate != default(DateTime) )
                {
                    foundLesson.LessonDate = lessonModel.LessonDate;
                }

                if (lessonModel.LessonVisits != null)
                {
                    var checkStudensId = GetStudentsIDNotIncludeInGroup(lessonModel.LessonVisits,
                        await _unitOfWork.StudentGroupRepository.GetGroupStudentsIds((long)foundLesson.StudentGroupId));
                    if (checkStudensId.Count > 0)
                    {
                        return Result<LessonDto>.GetError(ErrorCode.NotFound, $"Student(s) with Id(s) {string.Join(" ,", checkStudensId)} not included in this group({foundLesson.StudentGroupId})");
                    }
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

                return Result<LessonDto>.GetSuccess(_mapper.Map<LessonDto>(foundLesson));

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<Result<IList<StudentLessonDto>>> GetStudentLessonsAsync(long studentId)
        {
            var studentLessonModels = await _unitOfWork.LessonRepository.GetStudentInfoAsync(studentId);

            if (studentLessonModels == null)
            {
                return Result<IList<StudentLessonDto>>.GetError(ErrorCode.NotFound, "Not found");
            }

            return Result<IList<StudentLessonDto>>.GetSuccess( _mapper.Map<IList<StudentLessonDto>>(studentLessonModels));
        }

        private List<long> GetStudentsIDNotIncludeInGroup(IList<VisitDto> visit, IList<long?> groupStudentsId)
        {
            List<long> result = new List<long>();
            var studentsId = visit.Select(x => x.StudentId).ToList();

            foreach (var item in studentsId)
            {
                if (!groupStudentsId.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

    }
}
