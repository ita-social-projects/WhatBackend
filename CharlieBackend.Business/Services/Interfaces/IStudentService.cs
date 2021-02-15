using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Result<StudentDto>> CreateStudentAsync(long accountId);

        Task<Result<IList<StudentDto>>> GetAllStudentsAsync();

        Task<Result<IList<StudentDto>>> GetAllActiveStudentsAsync();

        Task<Result<IList<StudentStudyGroupsDto>>> GetStudentStudyGroupsByStudentIdAsync(long id);

        Task<long?> GetAccountId(long studentId);

        Task<Result<StudentDto>> UpdateStudentAsync(long id, UpdateStudentDto studentModel);

        Task<Result<StudentDto>> GetStudentByAccountIdAsync(long accountId);

        Task<Result<StudentDto>> GetStudentByIdAsync(long studentId);

        Task<Result<StudentDto>> GetStudentByEmailAsync(string email);

        Task<Result<StudentDto>> DisableStudentAsync(long studentId);

        Task<Result<StudentDto>> EnableStudentAsync(long studentId);
    }
}
