using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
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

        public async Task<IList<MentorViewModel>> GetAllMentorsAsync(string accessToken)
        {
            var courses = await _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors", accessToken);

            return courses;
        }
    }
}
