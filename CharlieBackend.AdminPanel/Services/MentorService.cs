using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class MentorService : IMentorService
    {
        private readonly IApiUtil _apiUtil;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly IDataProtector _protector;
        private readonly string _accessToken;

        public MentorService(IApiUtil apiUtil, 
                             IOptions<ApplicationSettings> config, 
                             IHttpContextAccessor httpContextAccessor,
                             IDataProtectionProvider provider)
        {
            _apiUtil = apiUtil;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);

            _accessToken = _protector.Unprotect(httpContextAccessor.HttpContext.Request.Cookies["accessToken"]);
        }

        public async Task<MentorDto> AddMentorAsync(long id)
        {
            return await
                _apiUtil.CreateAsync<MentorDto>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", null, _accessToken);
        }

        public async Task<MentorDto> DisableMentorAsync(long id)
        {
            return await 
                _apiUtil.DeleteAsync<MentorDto>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", _accessToken);
        }

        public async Task<IList<MentorViewModel>> GetAllMentorsAsync()
        {
            var allMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors", _accessToken);

            var activeMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"{_config.Value.Urls.Api.Https}/api/mentors/active", _accessToken);

            foreach (var mentor in allMentors)
            {
                mentor.IsActive = activeMentors.Any(x => x.Id == mentor.Id);
            }

            return allMentors;
        }

        public async Task<MentorEditViewModel> GetMentorByIdAsync(long id)
        {
            var mentorTask = _apiUtil.GetAsync<MentorEditViewModel>($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", _accessToken);
            var coursesTask = _apiUtil.GetAsync<IList<CourseViewModel>>($"{_config.Value.Urls.Api.Https}/api/courses", _accessToken);
            var studentGroupTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"{_config.Value.Urls.Api.Https}/api/student_groups", _accessToken);

            var mentor = await mentorTask;

            mentor.AllGroups = await studentGroupTask;
            mentor.AllCourses = await coursesTask;

            return mentor;
        }

        public async Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto)
        {
            return await 
                _apiUtil.PutAsync($"{_config.Value.Urls.Api.Https}/api/mentors/{id}", UpdateDto, _accessToken);
        }
    }
}