﻿using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMentorService
    {
        Task<Result<MentorDto>> CreateMentorAsync(long accountId);

        Task<Result<IList<MentorDto>>> GetAllActiveMentorsAsync();

        Task<IList<MentorDto>> GetAllMentorsAsync();

        Task<Result<IList<MentorStudyGroupsDto>>> GetMentorStudyGroupsByMentorIdAsync(long id);

        Task<Result<IList<MentorCoursesDto>>> GetMentorCoursesByMentorIdAsync(long id);

        Task<long?> GetAccountId(long mentorId);

        Task<Result<MentorDto>> UpdateMentorAsync(long id, UpdateMentorDto mentorModel);

        Task<MentorDto> GetMentorByAccountIdAsync(long accountId);

        Task<MentorDto> GetMentorByIdAsync(long mentorId);
    }
}
