using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
        Task<Result<MentorDto>> CreateMentorAsync(long accountId);

        Task<IList<MentorDto>> GetAllActiveMentorsAsync();

        Task<IList<MentorDto>> GetAllMentorsAsync();

        Task<IList<MentorStudyGroupsDto>> GetMentorStudyGroupsByMentorIdAsync(long id);

        Task<IList<MentorCoursesDto>> GetMentorCoursesByMentorIdAsync(long id);

        Task<long?> GetAccountId(long mentorId);

        Task<Result<MentorDto>> UpdateMentorAsync(long id, UpdateMentorDto mentorModel);

        Task<Result<MentorDto>> GetMentorByAccountIdAsync(long accountId);

        Task<Result<MentorDto>> GetMentorByIdAsync(long mentorId);

        Task<Result<MentorDto>> DisableMentorAsync(long mentorId);
    }
}
