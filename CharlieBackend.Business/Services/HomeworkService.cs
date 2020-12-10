using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeworkService> _logger;

        public HomeworkService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeworkService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<HometaskDto>> CreateHometaskAsync(CreateHometaskDto createHometaskDto)
        {
            try
            {
                if (createHometaskDto == null)
                {
                    return Result<HometaskDto>.GetError(ErrorCode.NotFound,
                        "No hometask data received");
                }

                var newHometask = new Hometask
                {
                    Comment = createHometaskDto.Comment,
                    Common = createHometaskDto.Common,
                    DeadlineDays = createHometaskDto.DeadlineDays,
                    MentorId = createHometaskDto.MentorId,
                    TaskText = createHometaskDto.TaskText,
                    ThemeId = createHometaskDto.ThemeId,
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
                            StudentId = students[i].Id,
                            Student = students[i]
                        });
                    }
                }


            }
            catch
            {

            }
        }
    }
}
