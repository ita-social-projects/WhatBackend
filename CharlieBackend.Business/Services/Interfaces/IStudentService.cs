using CharlieBackend.Core.Models.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<StudentModel> CreateStudentAsync(StudentModel studentModel);
        public Task<List<StudentModel>> GetAllStudentsAsync();
        public Task<StudentModel> UpdateStudentAsync(StudentModel mentorModel);
    }
}
