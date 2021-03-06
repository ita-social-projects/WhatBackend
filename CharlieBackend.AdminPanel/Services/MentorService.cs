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

        public MentorService(IApiUtil apiUtil)
        {
            _apiUtil = apiUtil;    
        }

        public async Task<MentorDto> AddMentorAsync(long id)
        {
            return await
                _apiUtil.CreateAsync<MentorDto>($"api/mentors/{id}", null);
        }

        public async Task<bool> DisableMentorAsync(long id)
        {
            return await _apiUtil.DeleteAsync<bool>($"api/mentors/{id}");
        }

        public async Task<IList<MentorViewModel>> GetAllMentorsAsync()
        {
            var allMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"api/mentors");

            var activeMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>($"api/mentors/active");

            foreach (var mentor in allMentors)
            {
                mentor.IsActive = activeMentors.Any(x => x.Id == mentor.Id);
            }

            return allMentors;
        }

        public async Task<MentorEditViewModel> GetMentorByIdAsync(long id)
        {
            var coursesTask = _apiUtil.GetAsync<IList<CourseViewModel>>($"api/courses/isActive");
            var studentGroupTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>($"api/student_groups");
            var mentor = await _apiUtil.GetAsync<MentorEditViewModel>($"api/mentors/{id}");

            mentor.AllGroups = await studentGroupTask;
            mentor.AllCourses = await coursesTask;

            return mentor;
        }

        public async Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto)
        {
            return await 
                _apiUtil.PutAsync($"api/mentors/{id}", UpdateDto);
        }
    }
}