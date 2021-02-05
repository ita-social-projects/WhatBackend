using AutoMapper;
using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
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

        private readonly string _accessToken;

        public CourseService(IApiUtil apiUtil, IOptions<ApplicationSettings> config, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider)
        {
            _apiUtil = apiUtil;
            _config = config;
            _mapper = mapper;

            IDataProtector protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);
            _accessToken = protector.Unprotect(httpContextAccessor.HttpContext.Request.Cookies["accessToken"]);
        }

        public async Task<IList<CourseViewModel>> GetAllCoursesAsync()
        {
            var courses =  _mapper.Map<IList<CourseViewModel>>(await _apiUtil.GetAsync<IList<CourseDto>>($"{_config.Value.Urls.Api.Https}/api/courses", _accessToken));

            return courses;
        }

    }
}
