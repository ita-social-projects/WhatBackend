using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentGroupService> _logger;

        public StudentGroupService(IUnitOfWork unitOfWork, IMapper mapper,
                ILogger<StudentGroupService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StudentGroupDto>> CreateStudentGroupAsync
                (CreateStudentGroupDto studentGroupDto)
        {
            try
            {
                var result = await CheckCreateStudentGroupDto(studentGroupDto);

                if (result.Error != null)
                {
                    return result;
                }

                var studentGroup = new StudentGroup
                {
                    Name = studentGroupDto.Name,
                    CourseId = studentGroupDto.CourseId,
                    StartDate = studentGroupDto.StartDate,
                    FinishDate = studentGroupDto.FinishDate,
                };

                _unitOfWork.StudentGroupRepository.Add(studentGroup);

                if (studentGroupDto.StudentIds?.Count > 0)
                {
                    var notExistStudentIds = await _unitOfWork.StudentRepository.
                            GetNotExistEntitiesIdsAsync(
                                    studentGroupDto.StudentIds);

                    if (notExistStudentIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(
                                ErrorCode.ValidationError,
                                GenerateErrorMessage("Student",
                                        notExistStudentIds));
                    }

                    var students = await _unitOfWork.StudentRepository.
                            GetStudentsByIdsAsync(studentGroupDto.StudentIds);

                    studentGroup.StudentsOfStudentGroups =
                            new List<StudentOfStudentGroup>();

                    foreach (var s in students)
                    {
                        studentGroup.StudentsOfStudentGroups.Add(
                                new StudentOfStudentGroup
                                { StudentId = s.Id, Student = s });
                    }
                }

                if (studentGroupDto.MentorIds?.Count > 0)
                {
                    var notExistMentorsIds = await _unitOfWork.MentorRepository.
                            GetNotExistEntitiesIdsAsync(
                                    studentGroupDto.MentorIds);

                    if (notExistMentorsIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(
                                ErrorCode.ValidationError,
                                GenerateErrorMessage("Mentor",
                                        notExistMentorsIds));
                    }

                    var mentors = await _unitOfWork.MentorRepository.
                            GetMentorsByIdsAsync(studentGroupDto.MentorIds);

                    studentGroup.MentorsOfStudentGroups =
                            new List<MentorOfStudentGroup>();

                    foreach (var m in mentors)
                    {
                        studentGroup.MentorsOfStudentGroups.Add(
                                new MentorOfStudentGroup
                                { MentorId = m.Id, Mentor = m });
                    }
                }

                var studentId = await IsStudentHisOwnMentor(studentGroup);

                if (studentId != null)
                {
                    return Result<StudentGroupDto>.GetError(
                             ErrorCode.Conflict, "The student accountId " +
                             $"{studentId} is his own mentor");
                }

                await _unitOfWork.CommitAsync();

                return Result<StudentGroupDto>.GetSuccess(
                            _mapper.Map<StudentGroupDto>(studentGroup));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentGroupDto>.GetError(
                            ErrorCode.InternalServerError, "Internal error");
            }
        }

        private async Task<Result<StudentGroupDto>> CheckCreateStudentGroupDto(
             CreateStudentGroupDto studentGroupDto)
        {
            if (studentGroupDto == null)
            {
                return Result<StudentGroupDto>.GetError(
                        ErrorCode.ValidationError,
                        "StudentGroupDto is null");
            }

            if (await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(
                        studentGroupDto.Name))
            {
                return Result<StudentGroupDto>.GetError(
                        ErrorCode.UnprocessableEntity,
                        "Group name already exists");
            }

            if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(
                    studentGroupDto.CourseId))
            {
                return Result<StudentGroupDto>.GetError(
                        ErrorCode.ValidationError,
                        "CourseId does not exist");
            }

            if (studentGroupDto.StartDate > studentGroupDto.FinishDate)
            {
                return Result<StudentGroupDto>.GetError(
                        ErrorCode.ValidationError,
                        "Start date must be less than finish date");
            }

            if (studentGroupDto.MentorIds != null)
            {
                var dublicates = studentGroupDto.MentorIds.Dublicates();

                if (dublicates.Any())
                {
                    return Result<StudentGroupDto>.GetError(
                             ErrorCode.ValidationError, $"Such MentorIds: " +
                             $"{string.Join(' ', dublicates)} are not unique");
                }
            }

            if (studentGroupDto.StudentIds != null)
            {
                var dublicates = studentGroupDto.StudentIds.Dublicates();

                if (dublicates.Any())
                {
                    return Result<StudentGroupDto>.GetError(
                             ErrorCode.ValidationError, $"Such StudentIds: " +
                             $"{string.Join(' ', dublicates)} are not unique");
                }
            }

            return new Result<StudentGroupDto>();
        }

        /// <summary>
        /// Checks that: is some Mentor.AccountId equal to 
        /// some Student.AccountId in group.  
        /// </summary>
        /// <param name="group">Group where we do checking</param>
        /// <returns>AccountId of mentor if it has equality
        /// or long with result null if it hasn't</returns>
        private Task<long?> IsStudentHisOwnMentor(StudentGroup group)
        {
            if (group.MentorsOfStudentGroups == null
                        || group.StudentsOfStudentGroups == null)
            {
                long? result = null;

                return Task.FromResult(result);
            }

            return Task.FromResult(group.MentorsOfStudentGroups
                    .Select(m => m.Mentor.AccountId)
                    .Where(id => id != null)
                    .FirstOrDefault(id => group.StudentsOfStudentGroups
                            .Where(s => s.Student.AccountId != null)
                            .Any(s => s.Student.AccountId == id)));
        }

        public async Task<Result<bool>> IsGroupNameExistAsync(string name)
        {
            if (name == null)
            {
                return Result<bool>.GetError(ErrorCode.ValidationError, "Name is null");
            }

            var res = await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(name);

            return Result<bool>.GetSuccess(res);
        }

        public async Task<Result<IList<StudentGroupDto>>> GetAllStudentGroupsAsync(DateTime? startDate = null, DateTime? finishDate = null)
        {
            if (!(startDate is null) || !(finishDate is null))
            {
                return await GetStudentGroupsByDateAsync(startDate, finishDate);
            }
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllActiveAsync(startDate, finishDate);
            var studentGroupDto = _mapper.Map<List<StudentGroupDto>>(studentGroup);

            return Result<IList<StudentGroupDto>>.GetSuccess(studentGroupDto);
        }

        public async Task<bool> DeleteStudentGroupAsync(long StudentGroupId)
        {
            var lessonsOfStudentGroup = _unitOfWork.LessonRepository.GetAllLessonsForStudentGroup(StudentGroupId);
            bool result;

            if (lessonsOfStudentGroup.Result.Count == 0)
            {
                result = _unitOfWork.StudentGroupRepository.DeleteStudentGroupAsync(StudentGroupId).Result;
            }
            else
            {
                result =  _unitOfWork.StudentGroupRepository.DeactivateStudentGroupAsync(StudentGroupId).Result;
            }

            await _unitOfWork.CommitAsync();

            return result;
        }

        // if we set StudentIds or MentorsIds to null, they won't update
        public async Task<Result<StudentGroupDto>> UpdateStudentGroupAsync(long groupId, UpdateStudentGroupDto updatedStudentGroupDto)
        {
            try
            {
                if (updatedStudentGroupDto == null)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "UpdateStudentGroupDto is null");
                }

                var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(groupId);

                if (foundStudentGroup == null)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.NotFound, "Student Group not found");
                }

                if (string.IsNullOrWhiteSpace(updatedStudentGroupDto.Name))
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "Student group name can not be empty");
                }

                if (!updatedStudentGroupDto.Name.Equals(foundStudentGroup.Name))
                {
                    if (await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(updatedStudentGroupDto.Name))
                    {
                        return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists");
                    }

                    foundStudentGroup.Name = updatedStudentGroupDto.Name;
                }

                var dublicatesStudent = updatedStudentGroupDto.StudentIds.Dublicates();

                if (dublicatesStudent.Any())
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, $"Such student ids: {string.Join(" ", dublicatesStudent)} are not unique");
                }

                var dublicatesMentor = updatedStudentGroupDto.MentorIds.Dublicates();

                if (dublicatesMentor.Any())
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, $"Such mentor ids: {string.Join(" ", dublicatesMentor)} are not unique");
                }

                if (updatedStudentGroupDto.StartDate > updatedStudentGroupDto.FinishDate)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "Start date must be less than finish date");
                }

                if (updatedStudentGroupDto.StartDate != null)
                {
                    foundStudentGroup.StartDate = (DateTime?)(updatedStudentGroupDto.StartDate) ?? foundStudentGroup.StartDate;
                }

                if (updatedStudentGroupDto.FinishDate != null)
                {
                    foundStudentGroup.FinishDate = (DateTime?)(updatedStudentGroupDto.FinishDate) ?? foundStudentGroup.FinishDate;
                }

                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(updatedStudentGroupDto.CourseId))
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "CourseId does not exist");
                }

                foundStudentGroup.Course = await _unitOfWork.CourseRepository.GetByIdAsync(updatedStudentGroupDto.CourseId);

                if (updatedStudentGroupDto.StudentIds != null)
                {
                    var notExistStudentsIds = await _unitOfWork.StudentRepository.GetNotExistEntitiesIdsAsync(updatedStudentGroupDto.StudentIds);

                    if (notExistStudentsIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(errorCode: ErrorCode.ValidationError,
                                                                errorMessage: GenerateErrorMessage("Student", notExistStudentsIds));
                    }

                    var newStudentsOfStudentGroup = updatedStudentGroupDto.StudentIds.Select(x => new StudentOfStudentGroup
                    {
                        StudentGroupId = foundStudentGroup.Id,
                        StudentId = x
                    }).ToList();

                    _unitOfWork.StudentGroupRepository.UpdateManyToMany(foundStudentGroup.StudentsOfStudentGroups, newStudentsOfStudentGroup);
                }

                if (updatedStudentGroupDto.MentorIds != null)
                {
                    var notExistMentorsIds = await _unitOfWork.MentorRepository.GetNotExistEntitiesIdsAsync(updatedStudentGroupDto.MentorIds);

                    if (notExistMentorsIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(errorCode: ErrorCode.ValidationError,
                                                                errorMessage: GenerateErrorMessage("Mentor", notExistMentorsIds));
                    }

                    var newMentorsOfStudentGroup = updatedStudentGroupDto.MentorIds.Select(x => new MentorOfStudentGroup
                    {
                        StudentGroupId = foundStudentGroup.Id,
                        MentorId = x
                    }).ToList();

                    _unitOfWork.MentorRepository.UpdateMentorGroups(foundStudentGroup.MentorsOfStudentGroups, newMentorsOfStudentGroup);
                }

                await _unitOfWork.CommitAsync();

                return Result<StudentGroupDto>.GetSuccess(_mapper.Map<StudentGroupDto>(foundStudentGroup));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                _unitOfWork.Rollback();

                return Result<StudentGroupDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<Result<StudentGroupDto>> GetStudentGroupByIdAsync(long id)
        {
            var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetActiveStudentGroupByIdAsync(id);

            if (foundStudentGroup == null)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.NotFound, "Student group not found");
            }

            return Result<StudentGroupDto>.GetSuccess(_mapper.Map<StudentGroupDto>(foundStudentGroup));
        }

        public async Task<Result<IList<StudentGroupDto>>> GetStudentGroupsByDateAsync(DateTime? startDate, DateTime? finishDate)
        {
            if (startDate > finishDate)
            {
                return Result<IList<StudentGroupDto>>.GetError(ErrorCode.ValidationError, "Start date is later then finish date.");
            }

            var studentGroups = await _unitOfWork.StudentGroupRepository.GetAllActiveAsync(startDate, finishDate);

            return Result<IList<StudentGroupDto>>.GetSuccess(_mapper.Map<List<StudentGroupDto>>(studentGroups));
        }

        public void AddStudentOfStudentGroups(IEnumerable<StudentOfStudentGroup> items)
        {
            _unitOfWork.StudentGroupRepository.AddStudentOfStudentGroups(items);
        }

        private string GenerateErrorMessage(string EntityName, IEnumerable<long> ids)
        {
            var isMoreThenOneId = ids.Count() > 1;

            return string.Format("{0}{1} with id{1}: {3} do{2} not exist",
                                 EntityName,
                                 isMoreThenOneId ? "s" : null,
                                 isMoreThenOneId ? null : "es",
                                 string.Join(", ", ids));
        }
    }
}
