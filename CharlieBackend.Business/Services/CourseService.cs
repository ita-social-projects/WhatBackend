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
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "CourseDto is null");
                }

                if (await IsCourseNameTakenAsync(courseDto.Name))
                {
                    return Result<CourseDto>.GetError(ErrorCode.UnprocessableEntity, "Course already exists");
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

        public async Task<Result<IList<CourseDto>>> GetAllCoursesAsync()
        {
            return Result<IList<CourseDto>>.GetSuccess(_mapper
                    .Map<IList<CourseDto>>(await _unitOfWork.CourseRepository.GetAllAsync()));
        }

        public async Task<Result<CourseDto>> UpdateCourseAsync(long id, UpdateCourseDto updateCourseDto)
        {
            try
            {
                if (updateCourseDto == null)
                {
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "CourseDto is null");
                }

                if (!await _unitOfWork.CourseRepository.IsEntityExistAsync(id))
                {
                    return Result<CourseDto>.GetError(ErrorCode.NotFound, "Course id not found");
                }

                if (await IsCourseNameTakenAsync(updateCourseDto.Name))
                {
                    return Result<CourseDto>.GetError(ErrorCode.UnprocessableEntity, "Course already exists");
                }

                if (await _unitOfWork.CourseRepository.IsCourseEmptyAsync(id))
                {
                    return Result<CourseDto>.GetError(ErrorCode.ValidationError, "Student group are included in this Course");
                }

                var updatedEntity = _mapper.Map<Course>(updateCourseDto);

                updatedEntity.Id = id;

                _unitOfWork.CourseRepository.Update(updatedEntity);

                await _unitOfWork.CommitAsync();

                return Result<CourseDto>.GetSuccess(_mapper.Map<CourseDto>(updatedEntity));
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
            var studentGroup = await _unitOfWork.StudentGroupRepository.GetAllAsync();
            foreach (var item in studentGroup)
            {
                if (id == item.CourseId && item.FinishDate >= System.DateTime.Now)
                {
                    return Result<bool>.GetError(ErrorCode.ValidationError, "Course has active student group");
                }
            }

            Result<bool> course = await _unitOfWork.CourseRepository.DisableCourseByIdAsync(id);
            await _unitOfWork.CommitAsync();

            return course;
        }
    }
}
