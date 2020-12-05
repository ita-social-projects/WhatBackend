using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto courseModel)
        {
            try
            {
                var createdCourseEntity = _mapper.Map<Course>(courseModel);

                _unitOfWork.CourseRepository.Add(createdCourseEntity);

                await _unitOfWork.CommitAsync();
                var createdCourseDto = _mapper.Map<CourseDto>(createdCourseEntity);

                return Result<CourseDto>.GetSuccess(createdCourseDto);
            }
            catch 
            {

                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<Result<IList<CourseDto>>> GetAllCoursesAsync()
        {
            var courses = _mapper.Map<List<CourseDto>>(await _unitOfWork.CourseRepository.GetAllAsync());

            return Result<IList<CourseDto>>.GetSuccess(courses);
        }

        public async Task<Result<CourseDto>> UpdateCourseAsync(long id, UpdateCourseDto courseModel)
        {
            try
            {
                var updatedEntity = _mapper.Map<Course>(courseModel);

                updatedEntity.Id = id;

                _unitOfWork.CourseRepository.Update(updatedEntity);

                await _unitOfWork.CommitAsync();
                var updatedCourse = _mapper.Map<CourseDto>(updatedEntity);

                return Result<CourseDto>.GetSuccess(updatedCourse);
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
