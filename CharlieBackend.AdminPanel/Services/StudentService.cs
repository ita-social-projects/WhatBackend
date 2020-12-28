using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class StudentService: IStudentService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;
        private readonly IDataProtector _protector;
        private readonly string _accessToken;

        public StudentService(IApiUtil apiUtil,
                              IOptions<ApplicationSettings> config,
                              IHttpContextAccessor httpContextAccessor,
                              IDataProtectionProvider provider)
        {
            _apiUtil = apiUtil;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

            _accessToken = _protector.Unprotect(httpContextAccessor.HttpContext.Request.Cookies["accessToken"]);
        }

        public async Task<IList<StudentViewModel>> GetAllStudentsAsync()
        {
            var allStudentsTask =  _apiUtil.GetAsync<Result<IList<StudentViewModel>>>($"{_config.Value.Urls.Api.Https}/api/students", _accessToken);
            var activeStudentsTask = _apiUtil.GetAsync< Result<IList<StudentViewModel>>>($"{_config.Value.Urls.Api.Https}/api/students/active", _accessToken);

            var allStudents = await allStudentsTask;
            var activeStudents = await activeStudentsTask;

            foreach (var student in allStudents.Data)
            {
                student.IsActive = activeStudents.Data.Any(x => x.Id == student.Id);
            }

            return allStudents.Data;
        }

        public async Task<StudentEditViewModel> GetStudentByIdAsync(long id)
        {
            var studentTask =  _apiUtil.GetAsync< Result<StudentEditViewModel>>($"{_config.Value.Urls.Api.Https}/api/students/{id}", _accessToken);
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"{_config.Value.Urls.Api.Https}/api/student_groups", _accessToken);

            var student = await studentTask;
            var studentGroup = await studentGroupsTask;

            student.Data.AllGroups = studentGroup;

            return student.Data;
        }

        public async Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto)
        {
            var updatedStudent = await _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/students/{id}", UpdateDto, _accessToken);
            
            return updatedStudent;
        }

        public async Task<StudentDto> AddStudentAsync(long id)
        {
            var addedStudentTask = await _apiUtil.CreateAsync<StudentDto>($"{_config.Value.Urls.Api.Https}/api/students/{id}", null, _accessToken);

            return addedStudentTask;
        }

        public async Task<StudentDto> DisableStudentAsync(long id)
        {
            var disabledStudent = await _apiUtil.DeleteAsync<StudentDto>($"{_config.Value.Urls.Api.Https}/api/students/{id}", _accessToken);

            return disabledStudent;
        }
    }

}
