using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class MentorService : IMentorService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IMapper _mapper;

        public MentorService(IApiUtil apiUtil, IOptions<ApplicationSettings> config, IMapper mapper)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;
        }

        public async Task<MentorDto> AddMentorAsync(long id, string accessToken)
        {
            return await
                _apiUtil.CreateAsync<MentorDto>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", null, accessToken);
        }

        public async Task<MentorDto> DisableMentorAsync(long id, string accessToken)
        {
            return await 
                _apiUtil.DeleteAsync<MentorDto>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", accessToken);
        }

        public async Task<IList<MentorViewModel>> GetAllMentorsAsync(string accessToken)
        {
            var allMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors", accessToken);
            var activeMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors/active", accessToken);

            foreach (var mentor in allMentors)
            {
                mentor.IsActive = activeMentors.Any(x => x.Id == mentor.Id);
            }

            return allMentors;
        }

        public async Task<MentorEditViewModel> GetMentorByIdAsync(long id, string accessToken)
        {
            var mentor = await
                            _apiUtil.GetAsync<MentorEditViewModel>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", accessToken);
            var mentorGroups = await
                            _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"{_config.Value.Urls.Api.Https}/api/student_groups", accessToken);

            mentor.AllGroups = mentorGroups;

            return mentor;
        }

        public async Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto, string accessToken)
        {
            return await 
                _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", UpdateDto, accessToken);
        }
    }
}