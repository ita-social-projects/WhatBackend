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

        #region Create student group
        public async Task<Result<StudentGroupDto>> CreateStudentGroupAsync(
                CreateStudentGroupDto studentGroupDto)
        {
            Result<StudentGroupDto> result = null;

            try
            {
                if (studentGroupDto == null)
                {
                    result = Result<StudentGroupDto>.GetError(
                            ErrorCode.ValidationError,
                            "StudentGroupDto is null");
                }

                result = await CheckCreateStudentGroupDto(studentGroupDto,
                        result);

                if (result == null)
                {
                    var studentGroup = new StudentGroup
                    {
                        Name = studentGroupDto.Name,
                        CourseId = studentGroupDto.CourseId,
                        StartDate = studentGroupDto.StartDate,
                        FinishDate = studentGroupDto.FinishDate,
                    };

                    _unitOfWork.StudentGroupRepository.Add(studentGroup);

                    if (studentGroupDto.MentorIds?.Count > 0)
                    {
                        result = await CheckExistingEntityIds(
                                _unitOfWork.MentorRepository,
                                studentGroupDto.MentorIds);

                        await AddEntitiesToStudentGroup(studentGroupDto, 
                                _unitOfWork.MentorRepository, studentGroup);
                    }

                    if (studentGroupDto.StudentIds?.Count > 0)
                    {
                        result = await CheckExistingEntityIds(
                                _unitOfWork.StudentRepository,
                                studentGroupDto.StudentIds);

                        await AddEntitiesToStudentGroup(studentGroupDto,
                            _unitOfWork.StudentRepository, studentGroup);
                    }

                    result = await IsStudenHisOwnMentor(studentGroup);

                    if (result == null)
                    {
                        await _unitOfWork.CommitAsync();

                        result = Result<StudentGroupDto>.GetSuccess(
                                _mapper.Map<StudentGroupDto>(studentGroup));
                    }                
                }
            }
            catch
            {
                _unitOfWork.Rollback();

                result = Result<StudentGroupDto>.GetError(
                        ErrorCode.InternalServerError, "Internal error");
            }

            return result;
        }

        private async Task<Result<StudentGroupDto>> CheckCreateStudentGroupDto(
              CreateStudentGroupDto studentGroupDto,
              Result<StudentGroupDto> result)
        {
            if (result == null)
            {
                if (await _unitOfWork.StudentGroupRepository
                            .IsGroupNameExistAsync(studentGroupDto.Name))
                {
                    result = Result<StudentGroupDto>.GetError(
                            ErrorCode.UnprocessableEntity,
                            "Group name already exists");
                }
            }

            if (result == null)
            {
                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(
                        studentGroupDto.CourseId))
                {
                    result = Result<StudentGroupDto>.GetError(
                            ErrorCode.ValidationError,
                            "CourseId does not exist");
                }
            }

            if (result == null)
            {
                if (studentGroupDto.StartDate > studentGroupDto.FinishDate)
                {
                    result = Result<StudentGroupDto>.GetError(
                            ErrorCode.ValidationError,
                            "Start date must be less than finish date");
                }
            }

            if (result == null)
            {
                result = await CheckDublicate(studentGroupDto.StudentIds);
            }

            if (result == null)
            {
                result = await CheckDublicate(studentGroupDto.MentorIds);
            }

            return result;
        }

        private Task<Result<StudentGroupDto>> CheckDublicate(IList<long> list)
        {
            Result<StudentGroupDto> result = null;

            if (list != null)
            {
                var dublicates = list.Dublicates();

                if (dublicates.Any())
                {
                    result = Result<StudentGroupDto>.GetError(
                            ErrorCode.ValidationError, $"Such ids: " +
                            $"{string.Join(' ', dublicates)} are not unique");
                }
            }

            return Task.FromResult(result);
        }

        private async Task<Result<StudentGroupDto>> CheckExistingEntityIds<T>(
                IRepository<T> repository,
                IList<long> ids) where T : BaseEntity
        {
            Result<StudentGroupDto> result = null;

            var notExistingIds = await repository
                    .GetNotExistEntitiesIdsAsync(ids);

            if (notExistingIds.Any())
            {
                if (repository is IRepository<Student>)
                {
                    result = Result<StudentGroupDto>.GetError(
                           ErrorCode.ValidationError,
                           GenerateErrorMessage("Student", notExistingIds));
                }

                if (repository is IRepository<Mentor>)
                {
                    result = Result<StudentGroupDto>.GetError(
                           ErrorCode.ValidationError,
                           GenerateErrorMessage("Mentor", notExistingIds));
                }
            }

            return result;
        }

        private async Task AddEntitiesToStudentGroup<T>(
                CreateStudentGroupDto studentGroupDto,
                IRepository<T> repository,
                StudentGroup studentGroup) where T : BaseEntity
        {
            if (repository is IMentorRepository)
            {
                var mentors = await ((IMentorRepository)repository)
                        .GetMentorsByIdsAsync(studentGroupDto.MentorIds);

                studentGroup.MentorsOfStudentGroups = new
                        List<MentorOfStudentGroup>();

                foreach (var mentor in mentors)
                {
                    studentGroup.MentorsOfStudentGroups.Add(
                            new MentorOfStudentGroup
                            { MentorId = mentor.Id, Mentor = mentor });
                }
            }

            if (repository is IStudentRepository)
            {
                var students = await ((IStudentRepository)repository)
                        .GetStudentsByIdsAsync(studentGroupDto.StudentIds);

                studentGroup.StudentsOfStudentGroups = new 
                        List<StudentOfStudentGroup>();

                foreach (var student in students)
                {
                    studentGroup.StudentsOfStudentGroups.Add(
                            new StudentOfStudentGroup
                            { StudentId = student.Id, Student = student });
                }
            }
        }

        private Task<Result<StudentGroupDto>> IsStudenHisOwnMentor(
                StudentGroup group) 
        {
            Result<StudentGroupDto> result = null;

            if ((group.MentorsOfStudentGroups != null)
                        && (group.StudentsOfStudentGroups != null))
            {
                foreach (var mentorOfSG in group.MentorsOfStudentGroups)
                {
                    foreach (var studentOfSG in group.StudentsOfStudentGroups)
                    {
                        if ((studentOfSG.Student.AccountId != null)
                                    && (mentorOfSG.Mentor.AccountId != null)
                                    && (studentOfSG.Student.AccountId
                                    == mentorOfSG.Mentor.AccountId))
                        {
                            result = Result<StudentGroupDto>.GetError(
                             ErrorCode.Conflict, "The student id " +
                             $"{studentOfSG.Student.Id} is his own mentor");

                            break;
                        }
                    }

                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return Task.FromResult(result);
        }
        #endregion

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
                return await GetStudentGroupsByDateAsyns(startDate, finishDate);
            }
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();
            var studentGroupDto = _mapper.Map<List<StudentGroupDto>>(studentGroup);

            return Result<IList<StudentGroupDto>>.GetSuccess(studentGroupDto);
        }

        public bool DeleteStudentGrop(long StudentGroupId)
        {
            return _unitOfWork.StudentGroupRepository.DeleteStudentGroup(StudentGroupId);
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
            var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

            if (foundStudentGroup == null)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.NotFound, "Student group not found");
            }

            return Result<StudentGroupDto>.GetSuccess(_mapper.Map<StudentGroupDto>(foundStudentGroup));
        }

        public async Task<Result<IList<StudentGroupDto>>> GetStudentGroupsByDateAsyns(DateTime? startDate, DateTime? finishDate)
        {
            if (startDate > finishDate)
            {
                return Result<IList<StudentGroupDto>>.GetError(ErrorCode.ValidationError, "Start date is later then finish date.");
            }

            var studentGroups = await _unitOfWork.StudentGroupRepository.GetStudentGroupsByDateAsync(startDate, finishDate);

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
