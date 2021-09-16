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
        private readonly ICurrentUserService _currentUserService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
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
        public async Task<bool> CheckDoesMentorCanSeeCourseAsync(long mentorId, long? courseId)
        {
            if (await _unitOfWork.CourseRepository.GetMentorCourseAsync(mentorId, courseId) == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckDoesMentorCanSeeGroupAsync(long mentorId, long? groupId)
        {
            var courseOfGroup = await _unitOfWork.CourseRepository.GetCourseOfGroupAsync(mentorId, groupId);
            if (await _unitOfWork.CourseRepository.GetMentorCourseAsync(mentorId, courseOfGroup) == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<IList<long?>> GetMentorCoursesAsync(long mentorId)
        {
            return await _unitOfWork.CourseRepository.GetMentorCoursesById(mentorId);
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

                if (!await _unitOfWork.CourseRepository.IsCourseActive(id))
                {
                    return Result<CourseDto>.GetError(ErrorCode.Conflict, "Inactive course cannot be updated");
                }

                var updatedEntity = _mapper.Map<Course>(updateCourseDto);

                updatedEntity.Id = id;

                if (await _unitOfWork.CourseRepository.IsCourseNameTakenAsync(updatedEntity.Name))
                {
                    return Result<CourseDto>.GetError(ErrorCode.UnprocessableEntity, $"Сourse name \"{updatedEntity.Name}\"is already taken");
                }

                updatedEntity.IsActive = true;

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

        public async Task<Result<CourseDto>> DisableCourseAsync(long id)
        {
            if (await _unitOfWork.StudentGroupRepository.IsGroupOnCourseAsync(id))
            {
                return Result<CourseDto>.GetError(ErrorCode.ValidationError, "Course has active student group");
            }

            var courseDisabled = await _unitOfWork.CourseRepository.DisableCourseByIdAsync(id);

            if (courseDisabled.Data == null && courseDisabled.Error != null)
            {
                return Result<CourseDto>.GetError(courseDisabled.Error.Code, courseDisabled.Error.Message);
            }

            await _unitOfWork.CommitAsync();      
            var courseDto = _mapper.Map<Course, CourseDto>(courseDisabled.Data);            
            return  Result<CourseDto>.GetSuccess(courseDto);
        }

        public async Task<Result<CourseDto>> EnableCourseAsync(long id)
        {
            if (await _unitOfWork.CourseRepository.IsCourseActive(id))
            {
                return Result<CourseDto>.GetError(ErrorCode.Conflict, "Course is already active.");
            }

            var course= await _unitOfWork.CourseRepository.EnableCourseByIdAsync(id);  
            
            if (course.Data == null && course.Error != null)
            {
                return Result<CourseDto>.GetError(course.Error.Code, course.Error.Message);
            }

            await _unitOfWork.CommitAsync();          
            var courseDto = _mapper.Map<Course,CourseDto>(course.Data);               
            return Result<CourseDto>.GetSuccess(courseDto); 
        }

        public async Task<bool> IsCourseActive(long id)
        {
            return await _unitOfWork.CourseRepository.IsCourseActive(id);
        }
    }
}
