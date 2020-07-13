using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Mentor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<MentorModel> CreateMentorAsync(CreateMentorModel mentorModel);
        public Task<List<MentorModel>> GetAllMentorsAsync();
        public Task<long?> GetAccountId(long mentorId);
        public Task<MentorModel> UpdateMentorAsync(UpdateMentorModel mentorModel);
        public Task<MentorModel> GetMentorByAccountIdAsync(long accountId);
    }
}
