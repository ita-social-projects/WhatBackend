using CharlieBackend.Core.Models.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<StudentModel> CreateStudentAsync(CreateStudentModel studentModel);
        public Task<List<StudentModel>> GetAllStudentsAsync();
        public Task<long?> GetAccountId(long studentId);
        public Task<StudentModel> UpdateStudentAsync(UpdateStudentModel mentorModel);
        public Task<StudentModel> GetStudentByAccountIdAsync(long accountId);
        public Task<StudentModel> GetStudentByIdAsync(long studentId);
    }
}
