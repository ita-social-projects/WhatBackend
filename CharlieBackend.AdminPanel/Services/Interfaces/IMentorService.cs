using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.Core.DTO.Mentor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IMentorService
    {
        Task<IList<MentorViewModel>> GetAllMentorsAsync(string accessToken);

        Task<MentorEditViewModel> GetMentorByIdAsync(long id, string accessToken);

        Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto, string accessToken);

        Task<MentorDto> AddMentorAsync(long id, string accessToken);

        Task<MentorDto> DisableMentorAsync(long id, string accessToken);
    }
}
