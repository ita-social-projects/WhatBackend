using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class StudentService: IStudentService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        public StudentService(IApiUtil apiUtil, IOptions<ApplicationSettings> config)
        {
            _apiUtil = apiUtil;
            _config = config;
        }

        public async Task<IList<StudentViewModel>> GetAllStudentsAsync(string accessToken)
        {
            var allStudentsTask =  _apiUtil.GetAsync<IList<StudentViewModel>>($"{_config.Value.Urls.Api.Https}/api/students", accessToken);
            var activeStudentsTask = _apiUtil.GetAsync<IList<StudentViewModel>>($"{_config.Value.Urls.Api.Https}/api/students/active", accessToken);

            var allStudents = await allStudentsTask;
            var activeStudents = await activeStudentsTask;

            foreach (var student in allStudents)
            {
                student.IsActive = activeStudents.Any(x => x.Id == student.Id);
            }

            return allStudents;
        }

        public async Task<StudentEditViewModel> GetStudentByIdAsync(long id, string accessToken)
        {
            var studentTask =  _apiUtil.GetAsync<StudentEditViewModel>($"{_config.Value.Urls.Api.Https}/api/students/{id}", accessToken);
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"{_config.Value.Urls.Api.Https}/api/student_groups", accessToken);

            var student = await studentTask;

            student.AllGroups = await studentGroupsTask;

            return student;
        }

        public async Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto, string accessToken)
        {
            var updatedStudent = await _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/students/{id}", UpdateDto, accessToken);
            
            return updatedStudent;
        }

        public async Task<StudentDto> AddStudentAsync(long id,  string accessToken)
        {
            var addedStudentTask = await _apiUtil.CreateAsync<StudentDto>($"{_config.Value.Urls.Api.Https}/api/students/{id}", null, accessToken);

            return addedStudentTask;
        }

        public async Task<StudentDto> DisableStudentAsync(long id, string accessToken)
        {
            var disabledStudent = await _apiUtil.DeleteAsync<StudentDto>($"{_config.Value.Urls.Api.Https}/api/students/{id}", accessToken);

            return disabledStudent;
        }
    }

}
