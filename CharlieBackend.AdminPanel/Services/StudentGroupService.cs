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
        private readonly IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _config;

        private readonly IMentorService _mentorService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public StudentGroupService(IApiUtil apiUtil,
                                   IMapper mapper,
                                   IOptions<ApplicationSettings> config,
                                   IMentorService mentorService,
                                   IStudentService studentService,
                                   ICourseService courseService)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;

            _mentorService = mentorService;
            _studentService = studentService;
            _courseService = courseService;
        }

        public async Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync(string accessToken)
        {
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupDto>>($"{_config.Value.Urls.Api.Https}/api/student_groups", accessToken);
            var studentsTask = _studentService.GetAllStudentsAsync(accessToken);
            var mentorsTask = _mentorService.GetAllMentorsAsync(accessToken);

            var studentGroups = _mapper.Map<IList<StudentGroupViewModel>>(await studentGroupsTask);
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

        public async Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id, string accessToken)
        {
            var studentGroupsTask = _apiUtil.GetAsync<StudentGroupDto>($"{_config.Value.Urls.Api.Https}/api/student_groups/{id}", accessToken);
            var studentsTask = _studentService.GetAllStudentsAsync(accessToken);
            var mentorsTask = _mentorService.GetAllMentorsAsync(accessToken);
            var coursesTask = _courseService.GetAllCoursesAsync(accessToken);

            var studentGroup = _mapper.Map<StudentGroupEditViewModel>(await studentGroupsTask);
            studentGroup.AllCourses = await coursesTask;
            studentGroup.AllStudents = await studentsTask;
            studentGroup.AllMentors = await mentorsTask;

            return studentGroup;
        }

        public async Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto UpdateDto, string accessToken)
        {
            var updateStudentGroupTask = _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/student_groups/{id}", _mapper.Map<UpdateStudentGroupDto>(UpdateDto), accessToken);
            var updateStudentsForStudentGroupTask = _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/student_groups/{id}/students", _mapper.Map<UpdateStudentsForStudentGroup>(UpdateDto), accessToken);

            var updateStudentGroup = await updateStudentGroupTask;
            var updateStudentsForStudentGroup = await updateStudentsForStudentGroupTask;

            return UpdateDto;
        }
    }
}
