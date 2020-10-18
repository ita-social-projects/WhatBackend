using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.Course;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
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
                _unitOfWork.CourseRepository.Add(courseModel.ToCourse());

                await _unitOfWork.CommitAsync();

                return courseModel;
            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<List<CourseModel>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.CourseRepository.GetAllAsync();
            var coursesModels = new List<CourseModel>();

            foreach (var course in courses)
            {
                coursesModels.Add(course.ToCourseModel());
            }

            return coursesModels;
        }

        public async Task<CourseModel> UpdateCourseAsync(CourseModel courseModel)
        {
            try
            {
                _unitOfWork.CourseRepository.Update(courseModel.ToCourse());

                await _unitOfWork.CommitAsync();

                return courseModel;
            }
            catch
            {
                _unitOfWork.Rollback();

                return null;
            }
        }

        public Task<bool> IsCourseNameTakenAsync(string courseName)
        {
            return _unitOfWork.CourseRepository.IsCourseNameTakenAsync(courseName);
        }
    }
}
