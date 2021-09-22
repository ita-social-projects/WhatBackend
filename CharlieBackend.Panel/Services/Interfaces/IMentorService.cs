﻿using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Core.DTO.Mentor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IMentorService
    {
        Task<IList<MentorViewModel>> GetAllMentorsAsync();

        Task<MentorEditViewModel> GetMentorByIdAsync(long id);

        Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto);

        Task<MentorDto> AddMentorAsync(long id);

        Task<bool> DisableMentorAsync(long id);

        Task<bool> EnableMentorAsync(long id);

    }
}
