using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IMapper _mapper;

        public StudentGroupService(IApiUtil apiUtil, IOptions<ApplicationSettings> config, IMapper mapper)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IList<StudentGroupViewModel>> GetAllStudentGroups(string accessToken)
        {
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupDto>>($"{_config.Value.Urls.Api.Https}/api/student_groups", accessToken);
            var studentsTask = _apiUtil.GetAsync<IList<StudentViewModel>>($"{_config.Value.Urls.Api.Https}/api/students", accessToken);
            var mentorsTask = _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors", accessToken);

            var studentGroups =  _mapper.Map<IList<StudentGroupViewModel>>(await studentGroupsTask);
            var students = await studentsTask;
            var mentors = await mentorsTask;

            foreach (var item in studentGroups)
            {
                item.Students = item.Students.Select(student =>
                {
                    var foundStudent = students.FirstOrDefault(x => x.Id == student.Id);

                    if (foundStudent != null)
                    {
                        return _mapper.Map<StudentViewModel>(foundStudent);
                    }
                    else
                    {
                        return student;
                    }
                }).ToList();

                item.Mentors = item.Mentors.Select(mentor =>
                {
                    var foundMentor = mentors.FirstOrDefault(x => x.Id == mentor.Id);

                    if (foundMentor != null)
                    {
                        return _mapper.Map<MentorViewModel>(foundMentor);
                    }
                    else
                    {
                        return mentor;
                    }
                }).ToList();

            }

            return studentGroups;
        }

    }
}
