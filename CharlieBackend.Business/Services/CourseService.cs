using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CourseModel> CreateCourseAsync(CourseModel courseModel)
        {
            try
            {
                var createdCourse = await _unitOfWork.CourseRepository.PostAsync(courseModel.ToCourse());
                _unitOfWork.Commit();
                return courseModel;
            }
            catch { _unitOfWork.Rollback(); return null; }
        }

        public async Task<List<CourseModel>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.CourseRepository.GetAllAsync();

            var coursesModels = new List<CourseModel>();
            foreach (var course in courses) { coursesModels.Add(course.ToCourseModel()); }

            return coursesModels;
        }
    }
}
