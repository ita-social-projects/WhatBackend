using CharlieBackend.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<MentorModel> CreateMentorAsync(MentorModel mentorModel);
        public Task<List<MentorModel>> GetAllMentorsAsync();
        public Task<MentorModel> UpdateMentorAsync(MentorModel mentorModel);
    }
}
