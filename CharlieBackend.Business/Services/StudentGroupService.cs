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
using CharlieBackend.Core.Models.StudentGroup;

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

        public async Task<StudentGroupDto> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupDto)
        {
            try
            {
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

                return _mapper.Map<StudentGroupDto>(studentGroup);

            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public Task<bool> IsGroupNameTakenAsync(string name)
        {
            return _unitOfWork.StudentGroupRepository.IsGroupNameChangableAsync(name);
        }

        public async Task<IList<StudentGroupDto>> GetAllStudentGroupsAsync()
        {
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();

            return _mapper.Map<IList<StudentGroupDto>>(studentGroup);
        }

        public bool DeleteStudentGrop(long StudentGroupId)
        {
            return _unitOfWork.StudentGroupRepository.DeleteStudentGroup(StudentGroupId);
        }

        public async Task<StudentGroup> UpdateStudentGroupAsync(UpdateStudentGroupModel studentGroupModel)
        {
            try
            {
                var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(studentGroupModel.Id);

                if (foundStudentGroup == null)
                {
                    return null;
                }

                foundStudentGroup.Name = studentGroupModel.Name ?? foundStudentGroup.Name;

                if (studentGroupModel.StartDate != null)
                {
                    foundStudentGroup.StartDate = (DateTime?)(studentGroupModel.StartDate) ?? foundStudentGroup.StartDate;
                }

                if (studentGroupModel.FinishDate != null)
                {
                    foundStudentGroup.FinishDate = (DateTime?)(studentGroupModel.FinishDate) ?? foundStudentGroup.FinishDate;
                }

                if (studentGroupModel.CourseId != 0)
                {
                    var foundCourse = await _unitOfWork.CourseRepository.GetByIdAsync(studentGroupModel.CourseId);

                    foundStudentGroup.Course = foundCourse;
                }

                if (studentGroupModel.StudentIds != null)
                {
                    //var foundStudents = await _unitOfWork.StudentRepository.GetStudentsByIdsAsync(studentGroupModel.StudentIds);
                    var currentStudentsOfStudentGroup = foundStudentGroup.StudentsOfStudentGroups;
                    var newStudentsOfStudentGroup = new List<StudentOfStudentGroup>();

                    //for (int i = 0; i < studentGroupModel.StudentIds.Count; i++)
                    //    foundStudentGroup.StudentsOfStudentGroups.Add(new StudentOfStudentGroup { StudentId = foundStudents[i] });

                    foreach (var newStudentId in studentGroupModel.StudentIds)
                    {
                        newStudentsOfStudentGroup.Add(new StudentOfStudentGroup
                        {
                            StudentGroupId = foundStudentGroup.Id,
                            StudentId = newStudentId
                        });

                        _unitOfWork.StudentGroupRepository.UpdateManyToMany(currentStudentsOfStudentGroup, newStudentsOfStudentGroup);
                    }
                }

                await _unitOfWork.CommitAsync();

                return null; //------------------
            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<StudentGroupById> GetStudentGroupByIdAsync(long id)
        {
            var foundStudentGroup = await _unitOfWork.StudentGroupRepository.GetByIdAsync(id);

            if (foundStudentGroup == null)
            {
                return null;
            }

            var startDate = (DateTime)foundStudentGroup.StartDate;
            var finishDate = (DateTime)foundStudentGroup.FinishDate;

            return new StudentGroupById
            {
                FinishDate = finishDate.ToString("yyyy-MM-dd"),
                StartDate = startDate.ToString("yyyy-MM-dd"),
                StudentIds = foundStudentGroup.StudentsOfStudentGroups.Select(studentOfStudentGroup => studentOfStudentGroup.StudentId).ToList(),
                GroupName = foundStudentGroup.Name,
                MentorIds = foundStudentGroup.MentorsOfStudentGroups.Select(mentorOfStudentGroup => mentorOfStudentGroup.MentorId).ToList()
            };
        }
    }
}
