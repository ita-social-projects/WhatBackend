using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Homework;
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

namespace CharlieBackend.Api.UnitTest
{
    public class HomeworkServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<HomeworkService>> _loggerMock;
        private readonly Mock<IHomeworkRepository> _homeworkRepositoryMock;
        private readonly Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private readonly HomeworkService _homeworkService;
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
            _homeworkService = new HomeworkService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
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
        public async Task CreateHomeworkAsync_ValidData_ShouldReturnHomeworkExemple()
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
            result.Data.Should().BeEquivalentTo(_mapper.Map<HomeworkDto>(homeworkDto));
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

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }

        [Fact]
        public async Task UpdateMarkAsync_ValidDataPassed_ShouldReturnExpectedData()
        {
            // Arrange
            long lessonId = 1;
            sbyte mark = 5;
            sbyte updatedMark = 2;
            long visitId_one = 1;
            long visitId_two = 1;
            long studentId_one = 1;
            long studentId_two = 2;
            long homeworkId = 1;
            long homeworkStudentId_one = 1;
            long homeworkStudentId_two = 1;

            var visitTrue = CreateVisit(visitId_one, mark, true, studentId_one);
            var visitFalse = CreateVisit(visitId_two, mark, false, studentId_two);

            Lesson lesson = new Lesson()
            {
                Id = lessonId,
                Visits = new List<Visit>()
                {
                   visitTrue,
                   visitFalse
                }
            };

            var homeworkStudent_one = new HomeworkStudent
            {
                Id = homeworkStudentId_one,
                HomeworkId = homeworkId,
                StudentId = studentId_one
            };

            var homeworkStudent_two = new HomeworkStudent
            {
                Id = homeworkStudentId_two,
                HomeworkId = homeworkId,
                StudentId = studentId_two
            };

            var homework = new Homework
            {
                LessonId = lessonId,
                Id = homeworkId,
                HomeworkStudents = new List<HomeworkStudent>
                {
                    homeworkStudent_one,
                    homeworkStudent_two
                }
            };

            var lessonRepositoryMock = new Mock<ILessonRepository>();
            lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);

            var homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            homeworkRepositoryMock.Setup(x => x.GetByIdAsync(homeworkId)).ReturnsAsync(homework);

            var homeworkStudentRepositoryMock = new Mock<IHomeworkStudentRepository>();
            homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentId_one)).ReturnsAsync(homeworkStudent_one);
            homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentId_two)).ReturnsAsync(homeworkStudent_two);

            var visitRepositoryMock = new Mock<IVisitRepository>();
            visitRepositoryMock.Setup(x => x.GetByIdAsync(visitId_one)).ReturnsAsync(visitTrue);
            visitRepositoryMock.Setup(x => x.GetByIdAsync(visitId_two)).ReturnsAsync(visitFalse);

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(homeworkStudentRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);

            var homeworkService = new HomeworkService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                _loggerMock.Object);

            var request_one = new UpdateMarkRequestDto
            { 
                StudentHomeworkId = homeworkStudentId_one,
                StudentMark = updatedMark
            };

            var request_two = new UpdateMarkRequestDto
            {
                StudentHomeworkId = homeworkStudentId_two,
                StudentMark = updatedMark
            };

            //Act
            var result_one = await homeworkService.UpdateMarkAsync(request_one);
            var result_two = await homeworkService.UpdateMarkAsync(request_two);

            // Assert

            result_one.Data.StudentMark.GetValueOrDefault()
                .Should()
                .Be(updatedMark);

            result_two.Data
                .StudentMark
                .GetValueOrDefault()
                .Should()
                .Be(updatedMark);
        }

        [Fact]
        public async Task UpdateMarkAsync_WrongVisitPassed_ShouldThrowException()
        {
            // Arrange
            long lessonId = 1;
            sbyte mark = 5;
            sbyte updatedMark = 2;
            long visitId = 1;
            long studentId = 1;
            long homeworkId = 1;
            long homeworkStudentId = 1;
            long wrongHomeworkStudentId = 2;

            var visitTrue = CreateVisit(visitId, mark, true, studentId);

            Lesson lesson = new Lesson()
            {
                Id = lessonId,
                Visits = new List<Visit>()
                {
                   visitTrue,
                }
            };

            var homeworkStudent = new HomeworkStudent
            {
                Id = homeworkStudentId,
                HomeworkId = homeworkId,
                StudentId = studentId
            };

            var homework = new Homework
            {
                LessonId = lessonId,
                Id = homeworkId,
                HomeworkStudents = new List<HomeworkStudent>
                {
                    homeworkStudent,
                }
            };

            var lessonRepositoryMock = new Mock<ILessonRepository>();
            lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);

            var homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            homeworkRepositoryMock.Setup(x => x.GetByIdAsync(homeworkId)).ReturnsAsync(homework);

            var homeworkStudentRepositoryMock = new Mock<IHomeworkStudentRepository>();
            homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentId)).ReturnsAsync(homeworkStudent);

            var visitRepositoryMock = new Mock<IVisitRepository>();
            visitRepositoryMock.Setup(x => x.GetByIdAsync(visitId)).ReturnsAsync(visitTrue);

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(homeworkRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(homeworkStudentRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);

            var homeworkService = new HomeworkService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                _loggerMock.Object);

            var request = new UpdateMarkRequestDto
            {
                StudentHomeworkId = wrongHomeworkStudentId,
                StudentMark = updatedMark
            };

            //Act
            var result_one = await homeworkService.UpdateMarkAsync(request);

            // Assert

            result_one.Data.StudentMark.GetValueOrDefault()
                .Should()
                .Be(updatedMark);
        }
    }
}
