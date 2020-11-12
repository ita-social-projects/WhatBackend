using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<Result<MentorDto>> CreateMentorAsync(long accountId);

        public Task<IList<MentorDto>> GetAllMentorsAsync();

        public Task<long?> GetAccountId(long mentorId);

        public Task<Result<MentorDto>> UpdateMentorAsync(long id, UpdateMentorDto mentorModel);

        public Task<MentorDto> GetMentorByAccountIdAsync(long accountId);

        public Task<MentorDto> GetMentorByIdAsync(long mentorId);
    }
}
