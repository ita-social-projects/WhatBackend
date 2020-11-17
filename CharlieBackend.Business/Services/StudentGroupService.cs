using CharlieBackend.Business.Services.Interfaces;
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

namespace CharlieBackend.Business.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<StudentGroupDto>> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupDto)
        {
            try
            {
                if (studentGroupDto == null)
                {
                    return Result<StudentGroupDto>.GetError(ErrorCode.ValidationError, "StudentGroupDto is null");
                }

                var studentGroup = new StudentGroup
                {
                    Name = studentGroupDto.Name,
                    CourseId = studentGroupDto.CourseId,
                    StartDate = studentGroupDto.StartDate,
                    FinishDate = studentGroupDto.FinishDate,
                };


                _unitOfWork.StudentGroupRepository.Add(studentGroup);

                if (studentGroupDto?.StudentIds.Count != 0)
                {
                    var students = await _unitOfWork.StudentRepository.GetStudentsByIdsAsync(studentGroupDto.StudentIds);
                    studentGroup.StudentsOfStudentGroups = new List<StudentOfStudentGroup>();

                    for (int i = 0; i < students.Count; i++)
                    {
                        studentGroup.StudentsOfStudentGroups.Add(new StudentOfStudentGroup
                        {
                            Student = students[i]
                        });
                    }
                }

                if (studentGroupDto?.MentorIds.Count != 0)
                {
                    var mentors = await _unitOfWork.MentorRepository.GetMentorsByIdsAsync(studentGroupDto.MentorIds);
                    studentGroup.MentorsOfStudentGroups = new List<MentorOfStudentGroup>();

                    for (int i = 0; i < mentors.Count; i++)
                    {
                        studentGroup.MentorsOfStudentGroups.Add(new MentorOfStudentGroup
                        {
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
            if(name == null)
            {
                return Result<bool>.GetError(ErrorCode.ValidationError, "Name is null");
            }

            var res = await _unitOfWork.StudentGroupRepository.IsGroupNameExistAsync(name);

            return Result<bool>.GetSuccess(res);
        }

        public async Task<IList<StudentGroupDto>> GetAllStudentGroupsAsync()
        {
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();

            return _mapper.Map<List<StudentGroupDto>>(studentGroup);
        }

        public bool DeleteStudentGrop(long StudentGroupId)
        {
            return _unitOfWork.StudentGroupRepository.DeleteStudentGroup(StudentGroupId);
        }

        public async Task<Result<UpdateStudentGroupDto>> UpdateStudentGroupAsync(long groupId, UpdateStudentGroupDto studentGroupDto) 
        {
            try
            {
                if (studentGroupDto == null)
                {
                    return Result<UpdateStudentGroupDto>.GetError(ErrorCode.ValidationError, "UpdateStudentGroupDto is null");
                }

                var updatedEntity = _mapper.Map<StudentGroup>(studentGroupDto);


                var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(groupId);

                if (foundStudentGroup == null)
                {
                    return null;
                }

                foundStudentGroup.Name = updatedEntity.Name ?? foundStudentGroup.Name;

                if (updatedEntity.StartDate != null)
                {
                    foundStudentGroup.StartDate = (DateTime?)(updatedEntity.StartDate) ?? foundStudentGroup.StartDate;
                }

                if (updatedEntity.FinishDate != null)
                {
                    foundStudentGroup.FinishDate = (DateTime?)(updatedEntity.FinishDate) ?? foundStudentGroup.FinishDate;
                }

                if (updatedEntity.CourseId != 0)
                {
                    foundStudentGroup.Course = await _unitOfWork.CourseRepository.GetByIdAsync(updatedEntity.CourseId);
                }

                await _unitOfWork.CommitAsync();

                return Result<UpdateStudentGroupDto>.GetSuccess(_mapper.Map<UpdateStudentGroupDto>(updatedEntity));
            }
            catch 
            {
                _unitOfWork.Rollback();

                return Result<UpdateStudentGroupDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<Result<UpdateStudentsForStudentGroup>> UpdateStudentsForStudentGroupAsync(long id, UpdateStudentsForStudentGroup studentGroupModel) 
        {
            try
            {
                if (studentGroupModel == null)
                {
                    return Result<UpdateStudentsForStudentGroup>.GetError(ErrorCode.ValidationError, "UpdateStudentGroupDto is null");
                }

                var updatedEntity = _mapper.Map<StudentGroup>(studentGroupModel); 

                var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

                if (foundStudentGroup == null)
                {
                    return null;
                }

                var newStudentsOfStudentGroup = updatedEntity.StudentsOfStudentGroups.Select(x => new StudentOfStudentGroup
                {
                    StudentGroupId = foundStudentGroup.Id,
                    StudentId = x.StudentId
                }).ToList();

                _unitOfWork.StudentGroupRepository.UpdateManyToMany(foundStudentGroup.StudentsOfStudentGroups, newStudentsOfStudentGroup);

                await _unitOfWork.CommitAsync();

                return Result<UpdateStudentsForStudentGroup>.GetSuccess(_mapper.Map<UpdateStudentsForStudentGroup>(updatedEntity));
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<UpdateStudentsForStudentGroup>.GetError(ErrorCode.InternalServerError, "Internal error");
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

       
    }
}
