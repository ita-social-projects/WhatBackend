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
    public class CourseServiceTest : TestBase
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<ILogger<CourseService>> _loggerMock;
        private readonly IMapper _mapper;
        private CourseService _courseService;


        public CourseServiceTest()
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

        [Fact]
        public async Task CreateCourseAsync_CourseDtoIsNull_ReturnValidationError()
        {
            //Act
            var result = await _courseService.CreateCourseAsync(null);
            
            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateCourse()
        {
            //Arrange

            var newCourse = new CreateCourseDto()
            {
                Name = "New_test_name"
            };

            var existingCourse = new CreateCourseDto()
            {
                Name = "Exists_test_name"
            };


            var courseRepositoryMock = new Mock<ICourseRepository>();

            courseRepositoryMock.Setup(x => x.Add(It.IsAny<Course>()));

            courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(newCourse.Name))
                    .ReturnsAsync(false);

            courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(existingCourse.Name))
                    .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(courseRepositoryMock.Object);

            _courseService = new CourseService(
                _unitOfWorkMock.Object,
                _mapper
            );

            //Act

            var successResult = await _courseService.CreateCourseAsync(newCourse);
            var courseNameExistResult = await _courseService.CreateCourseAsync(existingCourse);
            var nullCourseResult = await _courseService.CreateCourseAsync(null);

            //Assert

            Assert.NotNull(successResult.Data);
            Assert.Equal(newCourse.Name, successResult.Data.Name);

            Assert.Equal(ErrorCode.Conflict, courseNameExistResult.Error.Code);

            Assert.Equal(ErrorCode.ValidationError, nullCourseResult.Error.Code);
        }

        [Fact]
        public async Task UpdateCourse()
        {
            //Arrange

            long notExistingCourseId = -10;

            var updateCourseDto = new UpdateCourseDto()
            {
                Name = "new_test_name"
            };

            var existingCourseDto = new UpdateCourseDto()
            {
                Name = "Test_name"
            };

            var existingCourse = new Course()
            {
                Id = 10,
                Name = "Test_name"
            };

            var courseRepositoryMock = new Mock<ICourseRepository>();


            courseRepositoryMock.Setup(x => x.IsEntityExistAsync(notExistingCourseId))
                .ReturnsAsync(false);

            courseRepositoryMock.Setup(x => x.IsEntityExistAsync(existingCourse.Id))
               .ReturnsAsync(true);

            courseRepositoryMock.Setup(x => x.IsCourseActive(existingCourse.Id))
              .ReturnsAsync(true);

            courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(updateCourseDto.Name))
                  .ReturnsAsync(false);

            courseRepositoryMock.Setup(x => x.IsCourseNameTakenAsync(existingCourse.Name))
                    .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(courseRepositoryMock.Object);

            var courseService = new CourseService(
                _unitOfWorkMock.Object,
                _mapper
                );

            //Act

            var successResult = await courseService.UpdateCourseAsync(existingCourse.Id, updateCourseDto);
            var courseNameExistResult = await courseService.UpdateCourseAsync(existingCourse.Id, existingCourseDto);
            var courseNotExistResult = await courseService.UpdateCourseAsync(notExistingCourseId, updateCourseDto);
            var nullCourseResult = await courseService.UpdateCourseAsync(existingCourse.Id, null);

            //Assert

            Assert.NotNull(successResult.Data);
            Assert.Equal(successResult.Data.Name, successResult.Data.Name);

            Assert.Equal(ErrorCode.UnprocessableEntity, courseNameExistResult.Error.Code);

            Assert.Equal(ErrorCode.NotFound, courseNotExistResult.Error.Code);

            Assert.Equal(ErrorCode.ValidationError, nullCourseResult.Error.Code);
        }
    }
}
