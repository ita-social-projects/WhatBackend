﻿using AutoMapper;
using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApiUtil _apiUtil;

        private readonly IMapper _mapper;

        public CourseService(IApiUtil apiUtil, IMapper mapper)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;
        }

        public async Task<bool> DisableCourseAsync(long id)
        {
            return await _apiUtil.DeleteAsync<bool>($"api/courses/{id}");
        }

        public async Task<bool> EnableCourseAsync(long id)
        {
            return await _apiUtil.EnableAsync<bool>($"api/courses/{id}");
        }

        public async Task UpdateCourse(long id, UpdateCourseDto UpdateDto)
        {
            await
                _apiUtil.PutAsync<UpdateCourseDto>($"api/courses/{id}", UpdateDto);
        }

        public async Task<IList<CourseViewModel>> GetAllCoursesAsync()
        {
            var courses = _mapper.Map<IList<CourseViewModel>>(await _apiUtil.GetAsync<IList<CourseDto>>($"api/courses/isActive"));

            return courses;
        }

        public async Task AddCourseAsync(CreateCourseDto courseDto)
        {
            await
                _apiUtil.CreateAsync<CreateCourseDto>($"api/courses", courseDto);
        }

    }
}
