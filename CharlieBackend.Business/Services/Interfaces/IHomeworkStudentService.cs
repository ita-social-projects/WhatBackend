using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IHomeworkStudentService
    {
        Task<Result<HomeworkStudentDto>> CreateHomeworkFromStudentAsync(HomeworkStudentRequestDto homeworkStudent, ClaimsPrincipal userContext);

        Task<IList<HomeworkStudentDto>> GetHomeworkStudentForMentorByHomeworkId(long homeworkId, ClaimsPrincipal userContext);

        Task<IList<HomeworkStudentDto>> GetHomeworkStudentForStudent(ClaimsPrincipal userContext);
    }
}
