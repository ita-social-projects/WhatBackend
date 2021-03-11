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

        public StudentService(IApiUtil apiUtil)
        {
            _apiUtil = apiUtil;
        }

        public async Task<IList<StudentViewModel>> GetAllStudentsAsync()
        {
            var allStudentsTask =  _apiUtil.GetAsync<IList<StudentViewModel>>($"api/students");
            var activeStudentsTask = _apiUtil.GetAsync<IList<StudentViewModel>>($"api/students/active");

            var allStudents = await allStudentsTask;
            var activeStudents = await activeStudentsTask;

            foreach (var student in allStudents)
            {
                student.IsActive = activeStudents.Any(x => x.Id == student.Id);
            }

            return allStudents;
        }

        public async Task<StudentEditViewModel> GetStudentByIdAsync(long id)
        {
            var studentTask =  _apiUtil.GetAsync<StudentEditViewModel>($"api/students/{id}");
            var studentGroupsTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"api/student_groups");

            var student = await studentTask;
            var studentGroup = await studentGroupsTask;

            student.AllGroups = studentGroup;

            return student;
        }

        public async Task<UpdateStudentDto> UpdateStudentAsync(long id, UpdateStudentDto UpdateDto)
        {
            var updatedStudent = await _apiUtil.PutAsync($"api/students/{id}", UpdateDto);
            
            return updatedStudent;
        }

        public async Task<StudentDto> AddStudentAsync(long id)
        {
            var addedStudentTask = await _apiUtil.CreateAsync<StudentDto>($"api/students/{id}", null);

            return addedStudentTask;
        }

        public async Task<bool> DisableStudentAsync(long id)
        {
            return await _apiUtil.DeleteAsync<bool>($"api/students/{id}");
        }

        public async Task<bool> EnableStudentAsync(long id)
        {
            return await _apiUtil.EnableAsync<bool>($"api/students/{id}");
        }
    }

}
