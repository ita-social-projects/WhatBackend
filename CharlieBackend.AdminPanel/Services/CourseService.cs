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

        private readonly IMapper _mapper;

        public CourseService(IApiUtil apiUtil, IMapper mapper)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;
        }

        public async Task<IList<CourseViewModel>> GetAllCoursesAsync()
        {
            var courses =  _mapper.Map<IList<CourseViewModel>>(await _apiUtil.GetAsync<IList<CourseDto>>($"api/courses/isActive"));

            return courses;
        }

    }
}
