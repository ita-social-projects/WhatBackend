using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<Result<StudentDto>> CreateStudentAsync(long accountId);

        public Task<IList<StudentDto>> GetAllStudentsAsync();

        public Task<long?> GetAccountId(long studentId);

        //public Task<StudentDto> UpdateStudentAsync(UpdateStudentDto mentorModel);

        public Task<StudentDto> GetStudentByAccountIdAsync(long accountId);

        public Task<StudentDto> GetStudentByIdAsync(long studentId);

        public Task<StudentDto> GetStudentByEmailAsync(string email);
    }
}
