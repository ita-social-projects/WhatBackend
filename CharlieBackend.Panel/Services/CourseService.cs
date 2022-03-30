using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly CoursesApiEndpoints _coursesApiEndpoints;

        public CourseService(
            IApiUtil apiUtil, 
            IMapper mapper, 
            IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;
            _coursesApiEndpoints = options.Value.Urls.ApiEndpoints.Courses;
        }

        public async Task<CourseDto> DisableCourseAsync(long id)
        {
            var disableCourseEndpoint = 
                string.Format(_coursesApiEndpoints.DisableCourseEndpoint, id);

            return await _apiUtil.DeleteAsync<CourseDto>(disableCourseEndpoint);
        }

        public async Task<CourseDto> EnableCourseAsync(long id)
        {
            var enableCourseEndpoint =
                string.Format(_coursesApiEndpoints.EnableCourseEndpoint, id);

            return await _apiUtil.EnableAsync<CourseDto>(enableCourseEndpoint);
        }

        public async Task UpdateCourse(long id, UpdateCourseDto UpdateDto)
        {
            var updateCourseEndpoint =
                string.Format(_coursesApiEndpoints.UpdateCourseEndpoint, id);

            await
                _apiUtil.PutAsync<UpdateCourseDto>(updateCourseEndpoint, UpdateDto);
        }

        public async Task<IList<CourseViewModel>> GetAllCoursesAsync()
        {
            var getAllCoursesEndpoint = _coursesApiEndpoints.GetAllCoursesEndpoint;

            var courseDtos = await _apiUtil.GetAsync<IList<CourseDto>>(getAllCoursesEndpoint);

            return _mapper.Map<IList<CourseViewModel>>(courseDtos);
        }

        public async Task AddCourseAsync(CreateCourseDto courseDto)
        {
            var addCourseEndpoint = _coursesApiEndpoints.AddCourseEndpoint;

            await _apiUtil.CreateAsync<CreateCourseDto>(addCourseEndpoint, courseDto);
        }
    }
}
