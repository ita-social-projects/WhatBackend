using AutoMapper;
using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class CourseService: ICourseService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IMapper _mapper;

        public CourseService(IApiUtil apiUtil, IOptions<ApplicationSettings> config, IMapper mapper)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IList<CourseViewModel>> GetAllCoursesAsync(string accessToken)
        {
            var courses =  _mapper.Map<IList<CourseViewModel>>(await _apiUtil.GetAsync<IList<CourseDto>>($"{_config.Value.Urls.Api.Https}/api/courses", accessToken));

            return courses;
        }

    }
}
