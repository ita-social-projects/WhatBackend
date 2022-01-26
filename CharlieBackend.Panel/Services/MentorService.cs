using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class MentorService : IMentorService
    {
        private readonly IApiUtil _apiUtil;
        private readonly MentorsApiEndpoints _mentorsApiEndpoints;
        private readonly CoursesApiEndpoints _coursesApiEndpoints;
        private readonly StudentGroupsApiEndpoints _studentGroupsApiEndpoints;

        public MentorService(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;

            _mentorsApiEndpoints = options.Value.Urls.ApiEndpoints.Mentors;
            _coursesApiEndpoints = options.Value.Urls.ApiEndpoints.Courses;
            _studentGroupsApiEndpoints = options.Value.Urls.ApiEndpoints.StudentGroups;
        }

        public async Task<MentorDto> AddMentorAsync(long id)
        {
            var addMentorEndpoint = string
                .Format(_mentorsApiEndpoints.AddMentorEndpoint, id);

            return await
                _apiUtil.CreateAsync<MentorDto>(addMentorEndpoint, null);
        }

        public async Task<bool> DisableMentorAsync(long id)
        {
            var disableMentorEndpoint = string
                .Format(_mentorsApiEndpoints.DisableMentorEndpoint, id);

            return await _apiUtil.DeleteAsync<bool>(disableMentorEndpoint);
        }

        public async Task<bool> EnableMentorAsync(long id)
        {
            var enableMentorEndpoint = string
                .Format(_mentorsApiEndpoints.EnableMentorEndpoint, id);

            return await _apiUtil.EnableAsync<bool>(enableMentorEndpoint);
        }

        public async Task<IList<MentorViewModel>> GetAllMentorsAsync()
        {
            var getAllMentorsEndpoint = _mentorsApiEndpoints.GetAllMentorsEndpoint;
            var activeMentorEndpoint = _mentorsApiEndpoints.ActiveMentorEndpoint;

            var allMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>(getAllMentorsEndpoint);
            var activeMentors = await 
                _apiUtil.GetAsync<IList<MentorViewModel>>(activeMentorEndpoint);

            foreach (var mentor in allMentors)
            {
                mentor.IsActive = activeMentors.Any(x => x.Id == mentor.Id);
            }

            return allMentors;
        }

        public async Task<IList<MentorViewModel>> GetAllActiveMentorsAsync()
        {
            var activeMentorEndpoint = _mentorsApiEndpoints.ActiveMentorEndpoint;

            var activeMentors = await
                _apiUtil.GetAsync<IList<MentorViewModel>>(activeMentorEndpoint);

            return activeMentors;
        }

        public async Task<MentorEditViewModel> GetMentorByIdAsync(long id)
        {
            var getAllCoursesEndpoint = _coursesApiEndpoints.GetAllCoursesEndpoint;
            var getAllStudentGroupsEndpoint = _studentGroupsApiEndpoints.GetAllStudentGroupsEndpoint;
            var getMentorEndpoint = string
                .Format(_mentorsApiEndpoints.GetMentorEndpoint, id);

            var coursesTask = _apiUtil.GetAsync<IList<CourseViewModel>>(getAllCoursesEndpoint);
            var studentGroupTask = _apiUtil.GetAsync<IList<StudentGroupViewModel>>(getAllStudentGroupsEndpoint);
            var mentor = await _apiUtil.GetAsync<MentorEditViewModel>(getMentorEndpoint);

            mentor.AllGroups = await studentGroupTask;
            mentor.AllCourses = await coursesTask;

            return mentor;
        }

        public async Task<UpdateMentorDto> UpdateMentorAsync(long id, UpdateMentorDto UpdateDto)
        {
            var updateMentorEndpoint = string
               .Format(_mentorsApiEndpoints.UpdateMentorEndpoint, id);

            return await 
                _apiUtil.PutAsync(updateMentorEndpoint, UpdateDto);
        }
    }
}