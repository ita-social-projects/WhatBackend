using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
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
                var createdCourseEntity = _mapper.Map<Course>(courseModel);

                _unitOfWork.CourseRepository.Add(createdCourseEntity);

                await _unitOfWork.CommitAsync();

                return _mapper.Map<CourseDto>(createdCourseEntity);
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
                if (await _unitOfWork.CourseRepository.IsCourseEmptyAsync(id))
                {
                    return null;
                }
                var updatedEntity = _mapper.Map<Course>(courseModel);

                updatedEntity.Id = id;

                _unitOfWork.CourseRepository.Update(updatedEntity);

                await _unitOfWork.CommitAsync();

                return _mapper.Map<CourseDto>(updatedEntity);
            }
            catch 
            {

                return null;
            }

        }

        public Task<bool> IsCourseNameTakenAsync(string courseName)
        {
            return _unitOfWork.CourseRepository.IsCourseNameTakenAsync(courseName);
        }

        public async Task<bool> DisableCourceAsync(long id)
        {
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();
            foreach (var item in studentGroup)
            {
                if (id == item.CourseId && item.FinishDate >= DateTime.Now)
                {
                    return false;
                }
            }

            var course = await _unitOfWork.CourseRepository.DisableCourseByIdAsync(id);
            await _unitOfWork.CommitAsync();

            return course;
        }

        public Task<bool> IsCourseEmptyAsync(long id)
        {
            return _unitOfWork.CourseRepository.IsCourseEmptyAsync(id);
        }
    }
}
