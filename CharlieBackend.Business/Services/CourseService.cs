using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Entities;
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

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto courseModel)
        {
            try
            {
                _unitOfWork.CourseRepository.Add(_mapper.Map<Course>(courseModel)); // add should return added Entitty?

                await _unitOfWork.CommitAsync();

                return _mapper.Map<CourseDto>(courseModel);
            }
            catch 
            {

                _unitOfWork.Rollback();

                return null;
            }
        }

        public async Task<IList<CourseDto>> GetAllCoursesAsync()
        {
            var courses = _mapper.Map<List<CourseDto>>(await _unitOfWork.CourseRepository.GetAllAsync());

            return courses;
        }

        public async Task<CourseDto> UpdateCourseAsync(long id, UpdateCourseDto courseModel)
        {
            try
            {
                var updatedEntity = _mapper.Map<Course>(courseModel);

                updatedEntity.Id = id;

                _unitOfWork.CourseRepository.Update(updatedEntity);

                await _unitOfWork.CommitAsync();

                return _mapper.Map<CourseDto>(updatedEntity);
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
