using CharlieBackend.AdminPanel.Models.Mentor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<IList<MentorViewModel>> GetAllMentorsAsync(string accessToken);
    }
}
