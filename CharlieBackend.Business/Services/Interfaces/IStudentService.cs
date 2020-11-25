using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Result<StudentDto>> CreateStudentAsync(long accountId);

        Task<IList<StudentDto>> GetAllStudentsAsync();

        Task<IList<StudentDto>> GetAllActiveStudentsAsync();

        Task<long?> GetAccountId(long studentId);

        Task<Result<StudentDto>> UpdateStudentAsync(long id, UpdateStudentDto studentModel);

        Task<StudentDto> GetStudentByAccountIdAsync(long accountId);

        Task<StudentDto> GetStudentByIdAsync(long studentId);

        Task<StudentDto> GetStudentByEmailAsync(string email);
    }
}
