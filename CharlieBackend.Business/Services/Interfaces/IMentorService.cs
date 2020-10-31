using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Mentor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
       // public Task<MentorDto> CreateMentorAsync(CreateMentorDto mentorModel);

        public Task<IList<MentorDto>> GetAllMentorsAsync();

        public Task<long?> GetAccountId(long mentorId);

       // public Task<MentorDto> UpdateMentorAsync(UpdateMentorDto mentorModel);

        public Task<MentorDto> GetMentorByAccountIdAsync(long accountId);
    }
}
