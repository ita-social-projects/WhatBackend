using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IHomeworkStudentService
    {
        Task<Result<HomeworkStudentDto>> CreateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent);

        Task<Result<IList<HomeworkStudentDto>>> GetHomeworkStudentForMentor(long homeworkId);
        Task<Result<IList<HomeworkStudentDto>>> GetStudentHomeworkInGroup(HomeworkStudentFilter homeworkForStudent);

        Task<IList<HomeworkStudentDto>> GetHomeworkStudentForMentor(long homeworkId);

        Task<IList<HomeworkStudentDto>> GetHomeworkStudentForStudent();

        Task<IList<HomeworkStudentDto>> GetHomeworkStudentHistoryByHomeworkStudentId(long homeworkStudentId);

        Task<Result<HomeworkStudentDto>> UpdateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent, long homeworkId);

        Task<Result<HomeworkStudentDto>> UpdateMarkAsync(UpdateMarkRequestDto request);
    }
}
