using System;
using System.Collections.Generic;
using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class CourseServiceTests : TestBase
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<ILogger<CourseService>> _loggerMock;
        private readonly IMapper _mapper;
        private CourseService _courseService;


        public CourseServiceTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _loggerMock = new Mock<ILogger<CourseService>>();
            _mapper = GetMapper(new ModelMappingProfile());
            _courseService = new CourseService(
                _unitOfWorkMock.Object,
                _mapper
            );

            _unitOfWorkMock.Setup(x => x.CourseRepository)
                .Returns(_courseRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository)
                .Returns(_studentGroupRepositoryMock.Object);
        }
        
        private List<Course> GetCourses() =>
            new List<Course>()
            {
                new Course()
                {
                    Id = 0,
                    Name = "Test_course1",
                    IsActive = true
                },
                new Course()
                {
                    Id = 1,
                    Name = "Test_course2",
                    IsActive = false
                }
            };

        [Fact]
        public async Task CreateCourseAsync_CourseDtoIsNull_ShouldReturnValidationError()
        {
            //Act
            var result = await _courseService.CreateCourseAsync(null);
            
            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateCourseAsync_SameCourseNameExists_ShouldReturnConflictError()
        {
            //Arrange
            var course = new CreateCourseDto()
            {
                Name = "Test_name"
            };
            
            _courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(course.Name))
                .ReturnsAsync(true);
            
            //Act
            var result = await _courseService.CreateCourseAsync(course);
            
            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task CreateCourseAsync_CourseNameIsUnique_ShouldReturnCreatedCourse()
        {
            //Arrange
            var course = new CreateCourseDto()
            {
                Name = "Test_name"
            };
            
            _courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(course.Name))
                .ReturnsAsync(false);
            
            //Act
            var result = await _courseService.CreateCourseAsync(course);
            
            //Assert
            result.Data.Name
                .Should()
                .BeEquivalentTo(course.Name);
        }       
        
        [Fact]
        public async Task CreateCourseAsync_ServerException_ShouldReturnInternalServerError()
        {
            //Arrange
            var course = new CreateCourseDto()
            {
                Name = "Test_name"
            };
            
            _courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(course.Name))
                .Throws<InvalidOperationException>();
            
            //Act
            var result = await _courseService.CreateCourseAsync(course);
            
            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task GetCoursesAsync_IsActiveNull_ShouldReturnAllCourses()
        {
            //Arrange
            var courses = GetCourses();
            
            _courseRepositoryMock.Setup(x => x.GetCoursesAsync(null))
                .ReturnsAsync(courses);
            
            //Act
            var result = await _courseService.GetCoursesAsync(null);
            
            //Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<CourseDto>>(courses));
        }

        [Fact]
        public async Task UpdateCourseAsync_CourseDtoIsNull_ShouldReturnValidationError()
        {
            //Act
            var result = await _courseService.UpdateCourseAsync(0, null);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateCourseAsync_InvalidId_ShouldReturnNotFoundError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(-1))
                .ReturnsAsync(false);
            
            //Act
            var result = await _courseService.UpdateCourseAsync(-1, new UpdateCourseDto());
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateCourseAsync_CourseHasGroup_ShouldReturnValidationError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseHasGroupAsync(0))
                .ReturnsAsync(true);
            
            //Act
            var result = await _courseService.UpdateCourseAsync(0, new UpdateCourseDto());
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateCourseAsync_CourseIsActive_ShouldReturnConflictError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseHasGroupAsync(0))
                .ReturnsAsync(false);
            _courseRepositoryMock.Setup(x => x.IsCourseActive(0))
                .ReturnsAsync(false);
            
            //Act
            var result = await _courseService.UpdateCourseAsync(0, new UpdateCourseDto());
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }
        
        [Fact]
        public async Task UpdateCourseAsync_SameCourseNameExists_ShouldReturnConflictError()
        {
            //Arrange
            var course = new UpdateCourseDto()
            {
                Name = "Test_name"
            };
            
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseHasGroupAsync(0))
                .ReturnsAsync(false);
            _courseRepositoryMock.Setup(x => x.IsCourseActive(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(course.Name))
                .ReturnsAsync(true);
            
            //Act
            var result = await _courseService.UpdateCourseAsync(0, course);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }
        
        [Fact]
        public async Task UpdateCourseAsync_ValidData_ShouldReturnUpdatedCourse()
        {
            //Arrange
            var course = new UpdateCourseDto()
            {
                Name = "Test_name"
            };
            
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseHasGroupAsync(0))
                .ReturnsAsync(false);
            _courseRepositoryMock.Setup(x => x.IsCourseActive(0))
                .ReturnsAsync(true);
            _courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(course.Name))
                .ReturnsAsync(false);
            
            //Act
            var result = await _courseService.UpdateCourseAsync(0, course);
            
            //Assert
            result.Data.Name.Should()
                .BeEquivalentTo(course.Name);
        }
        
        [Fact]
        public async Task UpdateCourseAsync_ServerException_ShouldReturnInternalServerError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(0))
                .Throws<InvalidOperationException>();
            
            //Act
            var result = await _courseService.UpdateCourseAsync(0, new UpdateCourseDto());
            
            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task DisableCourseAsync_CourseHasActiveStudentGroup_ShouldReturnValidationError()
        {
            //Arrange
            _studentGroupRepositoryMock.Setup(x => x.IsGroupOnCourseAsync(0))
                .ReturnsAsync(true);
            
            //Act
            var result = await _courseService.DisableCourseAsync(0);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }
        
        [Fact]
        public async Task DisableCourseAsync_ActiveCourseIsNotFound_ShouldReturnNotFoundError()
        {
            //Arrange
            _studentGroupRepositoryMock.Setup(x => x.IsGroupOnCourseAsync(0))
                .ReturnsAsync(false);
            _courseRepositoryMock.Setup(x => x.DisableCourseByIdAsync(0))
                .ReturnsAsync(new Result<Course>()
                {
                    Error = new ErrorData()
                    {
                        Code = ErrorCode.NotFound
                    }
                });
            
            //Act
            var result = await _courseService.DisableCourseAsync(0);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }
        
        [Fact]
        public async Task DisableCourseAsync_DataIsValid_ShouldReturnDisabledCourse()
        {
            //Arrange
            var courseName = "Test_name";
            _studentGroupRepositoryMock.Setup(x => x.IsGroupOnCourseAsync(0))
                .ReturnsAsync(false);
            _courseRepositoryMock.Setup(x => x.DisableCourseByIdAsync(0))
                .ReturnsAsync(new Result<Course>()
                {
                    Data = new Course()
                    {
                        Name = courseName,
                        IsActive = false
                    }
                });
            
            //Act
            var result = await _courseService.DisableCourseAsync(0);
            
            //Assert
            result.Data.Name.Should()
                .BeEquivalentTo(courseName);
            result.Data.IsActive.Should()
                .BeFalse();
        }
        
        [Fact]
        public async Task EnableCourseAsync_InactiveCourseIsNotFound_ShouldReturnNotFoundError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.EnableCourseByIdAsync(0))
                .ReturnsAsync(new Result<Course>()
                {
                    Error = new ErrorData()
                    {
                        Code = ErrorCode.NotFound
                    }
                });
            
            //Act
            var result = await _courseService.EnableCourseAsync(0);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }
        
        [Fact]
        public async Task EnableCourseAsync_CourseIsAlreadyActive_ShouldReturnConflictError()
        {
            //Arrange
            _courseRepositoryMock.Setup(x => x.IsCourseActive(0))
                .ReturnsAsync(true);
            
            //Act
            var result = await _courseService.EnableCourseAsync(0);
            
            //Assert
            result.Error.Code.Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task EnableCourseAsync_DataIsValid_ShouldReturnEnabledCourse()
        {
            //Arrange
            var courseName = "Test_name";
            _courseRepositoryMock.Setup(x => x.EnableCourseByIdAsync(0))
                .ReturnsAsync(new Result<Course>()
                {
                    Data = new Course()
                    {
                        Name = courseName,
                        IsActive = true
                    }
                });
            
            //Act
            var result = await _courseService.EnableCourseAsync(0);
            
            //Assert
            result.Data.Name.Should()
                .BeEquivalentTo(courseName);
            result.Data.IsActive.Should()
                .BeTrue();
        }
    }
}
