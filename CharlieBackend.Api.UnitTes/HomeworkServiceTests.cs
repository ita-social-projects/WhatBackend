using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Data.Exceptions;
using CharlieBackend.Core.DTO.Homework;
using System.Linq;

namespace CharlieBackend.Api.UnitTest
{
    public class HomeworkServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<HomeworkService>> _loggerMock;
        private readonly Mock<IHomeworkRepository> _homeworkRepositoryMock;
        private readonly Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private new readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly HomeworkService _homeworkService;
        private readonly CourseService _courseService;
        private readonly LessonService _lessonService;
        private HomeworkRequestDto homeworkRequestDto = new HomeworkRequestDto()
        {
            LessonId = 1
        };

        public HomeworkServiceTests()
        {
            _loggerMock = new Mock<ILogger<HomeworkService>>();
            _homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _homeworkService = new HomeworkService(_unitOfWorkMock.Object, _courseService, _lessonService, _mapper, _loggerMock.Object, _currentUserServiceMock.Object);
        }

        private static Visit CreateVisit(long id = 1, sbyte mark = 5, bool presence = true, long studentId = 1)
        {
            return new Visit()
            {
                Id = id,
                StudentMark = mark,
                Presence = presence,
                StudentId = studentId
            };
        }

        #region CreateHomeworkAsync

        [Fact]
        public async Task CreateHomeworkAsync_DefaultHomeworkRequestDto_ShouldReturnValidationError()
        {
            //Arrange
            HomeworkRequestDto homeworkRequestDto = default;

            //Act
            var result = await _homeworkService.CreateHomeworkAsync(homeworkRequestDto);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateHomeworkAsync_NotExistingLessonId_ShouldReturnValidationError()
        {
            //Arrange
            _lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            //Act
            var result = await _homeworkService.CreateHomeworkAsync(homeworkRequestDto);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);

        }

        [Fact]
        public async Task CreateHomeworkAsync_NotExistingAttachment_ShouldReturnValidationError()
        {
            //Arrange
            homeworkRequestDto.AttachmentIds = new List<long> { 1, 2 };

            _lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(false);

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(false);

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);
            //Act
            var result = await _homeworkService.CreateHomeworkAsync(homeworkRequestDto);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);

        }

        [Fact]
        public async Task CreateHomeworkAsync_ValidData_ShouldReturnHomeworkExample()
        {
            //Arrange
            homeworkRequestDto.AttachmentIds = new List<long> { 1, 2 };
            homeworkRequestDto.DueDate = DateTime.Parse("2021-11-18T15:00:00.384Z");

            HomeworkDto homeworkDto = new HomeworkDto()
            {
                Id = 0,
                LessonId = 1,
                DueDate = DateTime.Parse("2021-11-18T15:00:00.384Z"),
                AttachmentIds = new List<long>() { 1, 2 }
            };

            _lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(true);

            _homeworkRepositoryMock.Setup(x => x.Add(It.IsAny<Homework>())).Callback<Homework>(x => new Homework());

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>()))
                .ReturnsAsync(new List<Attachment>()
                {
                    new Attachment { Id = 1 },
                    new Attachment { Id = 2 }
                });

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            //Act
            var result = await _homeworkService.CreateHomeworkAsync(homeworkRequestDto);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<HomeworkDto>(homeworkDto), x => x.Excluding(f => f.PublishingDate));
        }
        #endregion

        #region UpdateHomeworkAsync

        [Fact]
        public async Task UpdateHomeworkAsync_DefaultHomework_ShouldReturnValidationError()
        {
            //Arrange
            HomeworkRequestDto homeworkRequestDto = default;

            //Act
            var result = await _homeworkService.UpdateHomeworkAsync(1, homeworkRequestDto);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);

        }

        [Fact]
        public async Task UpdateHomeworkAsync_NotExistingHomeworkId_ShouldReturnValidationError()
        {
            //Arrange
            homeworkRequestDto.DueDate = DateTime.Parse("2021-11-18T15:00:00.384Z");

            _lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(true);
            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(default(Homework));

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);
            //Act

            var result = await _homeworkService.UpdateHomeworkAsync(1, homeworkRequestDto);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);

        }

        [Fact]
        public async Task UpdateHomeworkAsync_ValidData_ShouldReturnValidationError()
        {
            //Arrange

            homeworkRequestDto.DueDate = DateTime.Parse("2021-11-18T15:00:00.384Z");

            Homework homework = new Homework
            {
                Id = 1,
                LessonId = 1,
                DueDate = DateTime.Parse("2021-11-18T15:00:00.384Z"),
                PublishingDate = DateTime.UtcNow
            };

            _lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(true);
            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(homework);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);
            //Act

            var result = await _homeworkService.UpdateHomeworkAsync(1, homeworkRequestDto);

            //Assert
            result.Data.Should().BeEquivalentTo(_mapper.Map<HomeworkDto>(homework));

        }

        #endregion

        #region GetHomework

        [Fact]
        public async Task GetHomeworkByLessonIdAsync_DefaultLessonId_ShouldReturnValidationError()
        {
            //Arrange
            long lessonId = default;

            //Act
            var result = await _homeworkService.GetHomeworksByLessonId(lessonId);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetHomeworkByLessonIdAsync_NotExistingLessonId_ShouldReturnEmptyListOfHomework()
        {
            //Arrange
            long nonExistentLessonId = 1;

            _homeworkRepositoryMock.Setup(x => x.GetHomeworksByLessonId(It.IsAny<long>())).ReturnsAsync(new List<Homework>());

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            //Act
            var result = await _homeworkService.GetHomeworksByLessonId(nonExistentLessonId);

            //Assert
            result.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetHomeworkByIdAsync_DefaultHomeworkId_ShouldReturnValidationError()
        {
            //Arrange
            long homeworkId = default;

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(default(Homework));

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            //Act
            var result = await _homeworkService.GetHomeworkByIdAsync(homeworkId);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetHomeworkByIdAsync_NotExistingHomeworkId_ShouldReturnEmptyHomework()
        {
            //Arrange
            long nonExistentHomeworkId = 1;

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(default(Homework));

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            //Act
            var result = await _homeworkService.GetHomeworkByIdAsync(nonExistentHomeworkId);

            //Assert
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }
        #endregion

        #region GetHomeworksAsync

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsAdmin_ShouldReturnAllEntities()
        {
            //Arrange
            var homeworks = (IList<Homework>)new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Admin);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworks(It.IsAny<GetHomeworkRequestDto>())).Returns(Task.FromResult(homeworks));
                       
            //Act
            var response = _homeworkService.GetHomeworksAsync(new GetHomeworkRequestDto());

            //Assert
            response.Result.Data.Should().HaveCount(1);
            response.Result.Data.First().Id.Should().Be(1);
        }

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsMentor_AndRequestHasNoParameters_ShouldReturnAllEntities()
        {
            //Arrange
            var homeworks = (IList<Homework>) new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Mentor);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForMentor(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(new GetHomeworkRequestDto());

            //Assert
            response.Result.Data.Should().HaveCount(1);
            response.Result.Data.First().Id.Should().Be(1);
        }

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsMentor_AndGroupIdHasValue_AndMentorHasAccessToGroup_ShouldReturnAllEntities()
        {
            //Arrange
            var request = new GetHomeworkRequestDto { GroupId = 1 };
            var homeworks = (IList<Homework>)new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Mentor);
            _studentGroupRepositoryMock.Setup(x => x.DoesMentorHasAccessToGroup(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForMentor(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(request);

            //Assert
            response.Result.Data.Should().HaveCount(1);
            response.Result.Data.First().Id.Should().Be(1);
        }

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsMentor_AndCourseIdHasValue_AndMentorHasAccessToCourse_ShouldReturnAllEntities()
        {
            //Arrange
            var request = new GetHomeworkRequestDto { CourseId = 1 };
            var homeworks = (IList<Homework>)new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Mentor);
            _courseRepositoryMock.Setup(x => x.DoesMentorHasAccessToCourse(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForMentor(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(request);

            //Assert
            response.Result.Data.Should().HaveCount(1);
            response.Result.Data.First().Id.Should().Be(1);
        }

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsMentor_AndCourseIdHasValue_AndMentorHasNoAccessToCourse_ShouldReturnError()
        {
            //Arrange
            var request = new GetHomeworkRequestDto { CourseId = 1 };
            var homeworks = (IList<Homework>)new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Mentor);
            _courseRepositoryMock.Setup(x => x.DoesMentorHasAccessToCourse(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));            
            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForMentor(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(request);

            //Assert
            response.Result.Error.Message.Should().Be("Mentor can get only homeworks of his courses");
        }

        [Fact]
        public void GetHomeworksAsync_WhenRoleIsMentor_AndGroupIdHasValue_AndMentorHasNoAccessToGroup_ShouldReturnError()
        {
            //Arrange
            var request = new GetHomeworkRequestDto { GroupId = 1 };
            var homeworks = (IList<Homework>)new List<Homework> { new Homework { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Mentor);
            _studentGroupRepositoryMock.Setup(x => x.DoesMentorHasAccessToGroup(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));            
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForMentor(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(request);

            //Assert
            response.Result.Error.Message.Should().Be("Mentor can get only homeworks of groups of his courses");
        }

        [Fact]
        public void GetHomeworksAsync_WhenGroupIdHasValue_ShouldReturnFilteredEntities()
        {
            //Arrange
            var requestDto = new GetHomeworkRequestDto();
            var homeworks = (IList<Homework>)new List<Homework> { new Homework() { Id = 1 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Admin);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworks(requestDto)).Returns(Task.FromResult(homeworks));

            //Act
            var response = _homeworkService.GetHomeworksAsync(requestDto);

            //Assert
            response.Result.Data.Should().HaveCount(1);
            response.Result.Data.First().Id.Should().Be(1);
        }

        [Fact]
        public async void GetHomeworksAsync_WhenGroupIdHasValue_ShouldReturnEntity()
        {
            //Arrange
            var requestDto = new GetHomeworkRequestDto() { GroupId = 1 };
            var validResult = (IList<Homework>)new List<Homework>();

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Admin);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworks(requestDto)).Returns(Task.FromResult(validResult));

            //Act
            var result = await _homeworkService.GetHomeworksAsync(requestDto);

            //Assert
            result.Data.Should().BeEquivalentTo(validResult);
        }

        [Fact]
        public async Task GetHomeworksAsync_WhenRoleIsSecretary_AndGroupIdIsValid_ShouldReturnAllEntities()
        {
            //Arrange
            var homeworkDto = (IList<Homework>)new List<Homework>() { new Homework { LessonId = 2 } };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Secretary);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworks(It.IsAny<GetHomeworkRequestDto>())).Returns(Task.FromResult(homeworkDto));

            //Act
            var response = await _homeworkService.GetHomeworksAsync(new GetHomeworkRequestDto());

            //Assert
            response.Data.Should().HaveCount(1);
            response.Data[0].LessonId.Should().Be(2);
        }

        [Fact]
        public async Task GetHomeworksAsync_WhenRoleIsStudent_AndCourseIdIsValid_ShouldReturnEntity()
        {
            //Arange
            var homeworks = (IList<Homework>)new List<Homework>()
            {
                new Homework { Id = 1 },
                new Homework { Id = 2 }
            };

            _currentUserServiceMock.Setup(x => x.Role).Returns(UserRole.Student);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetHomeworksForStudent(It.IsAny<GetHomeworkRequestDto>(), It.IsAny<long>())).Returns(Task.FromResult(homeworks));

            //Act
            var response = await _homeworkService.GetHomeworksAsync(new GetHomeworkRequestDto());

            //Assert
            response.Data[0].Id.Should().Be(1);
            response.Data[1].Id.Should().Be(2);
            response.Data.Should().HaveCount(2);
        }

        #endregion

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }   
    }
}
