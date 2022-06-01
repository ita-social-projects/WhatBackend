using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<HomeworkService> _logger;

        public LessonService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<HomeworkService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
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

                if (createdLessonEntity.LessonDate > DateTime.Now)
                {
                    return Result<LessonDto>.GetError(ErrorCode.ValidationError, "Lesson date is incorrect");
                }

                _unitOfWork.LessonRepository.Add(createdLessonEntity);

                var curDate = DateTime.Now;

                if (lessonDto.LessonVisits != null)
                {
                    for (int i = 0; i < createdLessonEntity.Visits.Count; i++)
                    {
                        createdLessonEntity.Visits[i].Mark.Type = MarkType.Visit;
                        createdLessonEntity.Visits[i].Mark.EvaluationDate = curDate;
                        createdLessonEntity.Visits[i].Mark.EvaluatedBy = _currentUserService.AccountId;
                        _unitOfWork.MarkRepository.Add(createdLessonEntity.Visits[i].Mark);
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

        public async Task<Result<IList<LessonDto>>> GetLessonsByDate(DateTime? startDate, DateTime? finishDate)
        {
            var lastLesson = await _unitOfWork.LessonRepository.GetLastLesson();
            const long daysWeek = -30;

            if (startDate == null && finishDate == null)
            {
                startDate = lastLesson.LessonDate.AddDays(daysWeek);
                finishDate = lastLesson.LessonDate;
            }

            if (startDate > finishDate)
            {
                return Result<IList<LessonDto>>.GetError(ErrorCode.ValidationError, "FinishDate less than StartDate");
            }

            var lessons = await _unitOfWork.LessonRepository.GetLessonsByDate(startDate, finishDate);

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

        public async Task<Result<IList<LessonDto>>> GetAllLessonsForStudentGroup(long studentGroupId)
        {
            if (studentGroupId == default)
            {
                return Result<IList<LessonDto>>.GetError(ErrorCode.ValidationError, "Write Student Group Id please");
            }

            if (await _unitOfWork.StudentGroupRepository.GetByIdAsync(studentGroupId) == null)
            {
                return Result<IList<LessonDto>>.GetError(ErrorCode.ValidationError, $"Student Group with id {studentGroupId} is not Found");
            }

            var lessons = await _unitOfWork.LessonRepository.GetAllLessonsForStudentGroup(studentGroupId);

            return Result<IList<LessonDto>>.GetSuccess(_mapper.Map<IList<LessonDto>>(lessons));
        }

        public async Task<IList<LessonDto>> GetLessonsForMentorAsync(FilterLessonsRequestDto filterModel)
        {
            long accountId = _currentUserService.AccountId;
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
            var mentorToAssign = await _unitOfWork.MentorRepository.GetMentorByIdAsync(ids.MentorId);

            if (mentorToAssign == null)
            {
                return Result<Lesson>.GetError(ErrorCode.NotFound, $"Mentor with id {ids.MentorId} is not found");
            }
            var foundLesson = await _unitOfWork.LessonRepository.GetByIdAsync(ids.LessonId);

            if (foundLesson == null)
            {
                return Result<Lesson>.GetError(ErrorCode.NotFound, $"Lesson with id {ids.LessonId} is not found");
            }

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
               
                if (lessonModel.LessonDate > DateTime.Now)
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

                    for (int i = 0; i < lessonModel.LessonVisits.Count; i++)
                    {
                        var visit = foundLesson.Visits.SingleOrDefault(x => x.StudentId == lessonModel.LessonVisits[i].StudentId);
                        if(visit != null)
                        {                            
                            visit.Presence = lessonModel.LessonVisits[i].Presence;

                            //update marks
                            var mark = foundLesson.Visits.SingleOrDefault(x => x.MarkId == lessonModel.LessonVisits[i].MarkId).Mark;
                            if(mark != null)
                            {
                                mark.Value = lessonModel.LessonVisits[i].Mark.GetValueOrDefault();
                                mark.Comment = lessonModel.LessonVisits[i].Comment;

                                _unitOfWork.MarkRepository.Update(mark);
                            }

                            _unitOfWork.VisitRepository.Update(visit);
                        }
                    }
                }

                await _unitOfWork.CommitAsync();

                return Result<LessonDto>.GetSuccess(_mapper.Map<LessonDto>(foundLesson));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);

                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<Result<IList<StudentLessonDto>>> GetStudentLessonsAsync(
                long studentId)
        {
            Result<IList<StudentLessonDto>> result;
            if ((_currentUserService.Role.Is(UserRole.Student))
                    && (_currentUserService.EntityId != studentId))
            {
                result = Result<IList<StudentLessonDto>>.GetError(
                        ErrorCode.Unauthorized,
                        "Student can get only his lessons");
            }
            else
            {
                var studentLessonModels = await _unitOfWork.LessonRepository
                        .GetStudentInfoAsync(studentId);

                if (studentLessonModels == null)
                {
                    result = Result<IList<StudentLessonDto>>.GetError(
                            ErrorCode.NotFound, "Not found");
                }
                else
                {
                    result = Result<IList<StudentLessonDto>>.GetSuccess(
                            _mapper.Map<IList<StudentLessonDto>>(studentLessonModels));
                }
            }

            return result;
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

        public async Task<Result<IList<LessonDto>>> GetLessonsForStudentAsync(
                FilterLessonsRequestDto filterModel)
        {
            var groupsOfStudent = await _unitOfWork.StudentGroupRepository
                    .GetStudentGroupsIdsByStudentId(
                            (long)filterModel.StudentGroupId);

            if (groupsOfStudent.Contains(filterModel.StudentGroupId))
            {
                var lessonsForStudent = await _unitOfWork.LessonRepository
                        .GetLessonsForStudentAsync(filterModel.StudentGroupId,
                                filterModel.StartDate, filterModel.FinishDate,
                                _currentUserService.EntityId);

                return Result<IList<LessonDto>>.GetSuccess(
                        _mapper.Map<IList<LessonDto>>(lessonsForStudent));
            }

            return Result<IList<LessonDto>>.GetError(ErrorCode.Unauthorized,
                   "student can get lessons only that group what he belongs");
        }

        public async Task<Result<LessonDto>> GetLessonByIdAsync(long lessonId)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonId);

            if (lesson == null)
            {
                return Result<LessonDto>.GetError(ErrorCode.NotFound, $"This id = {lessonId} does not exist in database");
            }

            var lessonDto = _mapper.Map<LessonDto>(lesson);

            return Result<LessonDto>.GetSuccess(lessonDto);
        }

        public async Task<Result<bool>> IsLessonDoneAsync(long id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);

            if (lesson is null)
            {
                throw new NotFoundException("Given lesson not found");
            }

            return Result<bool>.GetSuccess(lesson.Visits.Where(visit => visit.Presence == true).Any());
        }
    }
}
