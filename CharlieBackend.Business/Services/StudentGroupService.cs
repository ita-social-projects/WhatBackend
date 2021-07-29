﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.Extensions;

namespace CharlieBackend.Business.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentGroupService> _logger;

        public StudentGroupService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentGroupService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StudentGroupDto>> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupDto)
        {
            try
            {
                if (studentGroupDto == null)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "StudentGroupDto is null");
                }

                if (await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(studentGroupDto.Name))
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists");
                }

                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(studentGroupDto.CourseId))
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "CourseId does not exist");
                }

                if (studentGroupDto.StudentIds != null)
                {
                    var dublicatesStudent = studentGroupDto.StudentIds.Dublicates();

                    if (dublicatesStudent.Any())
                    {
                        return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, $"Such student ids: {string.Join(" ", dublicatesStudent)} are not unique");
                    }
                }

                if (studentGroupDto.MentorIds != null)
                {
                    var dublicatesMentor = studentGroupDto.MentorIds.Dublicates();

                    if (dublicatesMentor.Any())
                    {
                        return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, $"Such mentor ids: {string.Join(" ", dublicatesMentor)} are not unique");
                    }
                }

                if (studentGroupDto.StartDate > studentGroupDto.FinishDate)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "Start date must be less than finish date");
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
                    var notExistStudentIds = await _unitOfWork.StudentRepository.GetNotExistEntitiesIdsAsync(studentGroupDto.StudentIds);

                    if (notExistStudentIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(errorCode: ErrorCode.ValidationError,
                                                                errorMessage: GenerateErrorMessage("Student", notExistStudentIds));
                    }

                    var students = await _unitOfWork.StudentRepository.GetStudentsByIdsAsync(studentGroupDto.StudentIds);
                    studentGroup.StudentsOfStudentGroups = new List<StudentOfStudentGroup>();

                    for (int i = 0; i < students.Count; i++)
                    {
                        studentGroup.StudentsOfStudentGroups.Add(new StudentOfStudentGroup
                        {
                            StudentId = students[i].Id,
                            Student = students[i]
                        });
                    }
                }

                if (studentGroupDto.MentorIds?.Count > 0)
                {
                    var notExistMentorsIds = await _unitOfWork.MentorRepository.GetNotExistEntitiesIdsAsync(studentGroupDto.MentorIds);

                    if (notExistMentorsIds.Any())
                    {
                        return Result<StudentGroupDto>.GetError(errorCode: ErrorCode.ValidationError,
                                                                errorMessage: GenerateErrorMessage("Mentor", notExistMentorsIds));
                    }

                    var mentors = await _unitOfWork.MentorRepository.GetMentorsByIdsAsync(studentGroupDto.MentorIds);
                    studentGroup.MentorsOfStudentGroups = new List<MentorOfStudentGroup>();

                    for (int i = 0; i < mentors.Count; i++)
                    {
                        studentGroup.MentorsOfStudentGroups.Add(new MentorOfStudentGroup
                        {
                            MentorId = mentors[i].Id,
                            Mentor = mentors[i]
                        }); ;
                    }
                }

                await _unitOfWork.CommitAsync();

                return Result<StudentGroupDto>.GetSuccess(_mapper.Map<StudentGroupDto>(studentGroup));

            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<StudentGroupDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
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
