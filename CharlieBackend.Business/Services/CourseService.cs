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

        public async Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto courseDto)
        {
            try
            {
                if (courseDto == null)
                {
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "Course Dto is null");
                }

                if (await _unitOfWork.CourseRepository.IsCourseNameTakenAsync(courseDto.Name))
                {
                    return Result<CourseDto>.GetError(ErrorCode.UnprocessableEntity, $"Course name \"{courseDto.Name}\" is already taken");
                }

                var createdCourseEntity = _mapper.Map<Course>(courseDto);

                _unitOfWork.CourseRepository.Add(createdCourseEntity);

                await _unitOfWork.CommitAsync();

                return Result<CourseDto>.GetSuccess(_mapper.Map<CourseDto>(createdCourseEntity));
            }
            catch 
            {

                _unitOfWork.Rollback();

                return Result<CourseDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public async Task<IList<CourseDto>> GetCoursesAsync(bool? isActive)
        {
            var courses = _mapper.Map<List<CourseDto>>(await _unitOfWork.CourseRepository.GetCoursesAsync(isActive));

            return courses;
        }

        public async Task<Result<CourseDto>> UpdateCourseAsync(long id, UpdateCourseDto updateCourseDto)
        {
            try
            {
                if (updateCourseDto == null)
                {
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "Course Dto is null");
                }
                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(id))
                {
                    return Result<CourseDto>.GetError(ErrorCode.NotFound, "Course Not Found");
                }

                if (await _unitOfWork.CourseRepository.IsCourseHasGroupAsync(id))
                {
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "Student group are included in this Course");
                }

                var updatedEntity = _mapper.Map<Course>(updateCourseDto);

                updatedEntity.Id = id;

                if (await _unitOfWork.CourseRepository.IsCourseNameTakenAsync(updatedEntity.Name))
                {
                    return Result<CourseDto>.GetError(ErrorCode.UnprocessableEntity, $"Сourse name \"{updatedEntity.Name}\"is already taken");
                }

                if (await _unitOfWork.CourseRepository.IsCourseActive(id))
                {
                    _unitOfWork.CourseRepository.Update(updatedEntity);
                
                    updatedEntity.IsActive = true;
                
                    await _unitOfWork.CommitAsync();

                    return Result<CourseDto>.GetSuccess(_mapper.Map<CourseDto>(updatedEntity));
                }

               return Result<CourseDto>.GetError(ErrorCode.Conflict, "Inactive course cannot be updated");
                
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<CourseDto>.GetError(ErrorCode.InternalServerError, "Internal error");
            }
        }

        public Task<bool> IsCourseNameTakenAsync(string courseName)
        {
            return _unitOfWork.CourseRepository.IsCourseNameTakenAsync(courseName);
        }

        public async Task<Result<bool>> DisableCourceAsync(long id)
        {
            if (await _unitOfWork.StudentGroupRepository.IsGroupOnCourseAsync(id))
            {
                return Result<bool>.GetError(ErrorCode.ValidationError, "Course has active student group");
            }

            Result<bool> course = await _unitOfWork.CourseRepository.DisableCourseByIdAsync(id);
            await _unitOfWork.CommitAsync();

            return course;
        }

        public async Task<bool> IsCourseActive(long id)
        {
            return await _unitOfWork.CourseRepository.IsCourseActive(id);
        }
    }
}
