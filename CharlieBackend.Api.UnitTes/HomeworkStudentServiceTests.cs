using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class HomeworkStudentServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<HomeworkStudentService>> _loggerMock;
        private readonly Mock<IHomeworkRepository> _homeworkRepositoryMock;
        private readonly Mock<IHomeworkStudentRepository> _homeworkStudentRepositoryMock;
        private readonly Mock<IHomeworkStudentHistoryRepository> _homeworkStudentHistoryRepositoryMock;
        private readonly Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private new readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly HomeworkStudentService _homeworkStudentService;
        private HomeworkStudentRequestDto homeworkStudentRequestDto = new HomeworkStudentRequestDto()
        {
            HomeworkId = 1,
            HomeworkText = "Some text",
            IsSent = true
        };

        public HomeworkStudentServiceTests()
        {
            _loggerMock = new Mock<ILogger<HomeworkStudentService>>();
            _homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            _homeworkStudentRepositoryMock = new Mock<IHomeworkStudentRepository>();
            _homeworkStudentHistoryRepositoryMock = new Mock<IHomeworkStudentHistoryRepository>();
            _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _homeworkStudentService = new HomeworkStudentService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task GetHomeworkStudentHistoryByHomeworkStudentId_ValidDataWithIsSentEqualsFalse_ShouldReturnListOfHomeworkStudentExample()
        {
            //Arrange

            var homeworkStudent = new HomeworkStudent()
            {
                Id = 5,
                IsSent = false
            };

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudent.Id))
                .ReturnsAsync(homeworkStudent);

            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(It.IsAny<long>()))
                .ReturnsAsync(new List<HomeworkStudentHistory>()
                {
                    new HomeworkStudentHistory() { HomeworkStudentId = 5},
                    new HomeworkStudentHistory() { HomeworkStudentId = 5}
                });

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudent.Id);

            //Assert
            result.Should().NotBeNull();
            Assert.Equal(2, result.Count);
            foreach (var hw in result)
            {
                Assert.Equal(homeworkStudent.Id, hw.Id);
            }
        }

        [Fact]
        public async Task GetHomeworkStudentHistoryByHomeworkStudentId_ValidDataWithIsSentEqualsTrue_ShouldReturnListOfHomeworkStudentExample()
        {
            //Arrange

            var homeworkStudent = new HomeworkStudent()
            {
                Id = 5,
                IsSent = true
            };

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudent.Id))
                .ReturnsAsync(homeworkStudent);

            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(It.IsAny<long>()))
                .ReturnsAsync(new List<HomeworkStudentHistory>()
                {
                    new HomeworkStudentHistory() { HomeworkStudentId = 5},
                    new HomeworkStudentHistory() { HomeworkStudentId = 5}
                });

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudent.Id);

            //Assert
            result.Should().NotBeNull();
            Assert.Equal(3, result.Count);
            foreach (var hw in result)
            {
                Assert.Equal(homeworkStudent.Id, hw.Id);
            }
        }

        [Fact]
        public async Task UpdateMarkAsync_ValidData_ShouldReturnHomeworkStudentExample()
        {
            //Arrange

            UpdateMarkRequestDto updateMarkRequestDto = new UpdateMarkRequestDto()
            {
                StudentHomeworkId = 5,
                StudentMark = 80,
                MentorComment = "Some Comment",
                MarkType = MarkType.Homework
            };

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(updateMarkRequestDto.StudentHomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = null
                });
            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(updateMarkRequestDto.StudentHomeworkId))
                .ReturnsAsync(new List<HomeworkStudentHistory>()
                {
                    new HomeworkStudentHistory()
                });

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateMarkAsync(updateMarkRequestDto);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Mark.Should().BeEquivalentTo(new HomeworkStudentMarkDto()
            {
                Value = updateMarkRequestDto.StudentMark,
                Comment = updateMarkRequestDto.MentorComment,
                Type = updateMarkRequestDto.MarkType,
                EvaluatedBy = 0
            },
            options => options.Excluding(x => x.EvaluationDate));
        }

        [Fact]
        public async Task UpdateMarkAsync_WrongHomeworkStudentId_ShouldReturnNotFoundError()
        {
            //Arrange

            UpdateMarkRequestDto updateMarkRequestDto = new UpdateMarkRequestDto()
            {
                StudentHomeworkId = 5,
                StudentMark = 80,
                MentorComment = "Some Comment",
                MarkType = MarkType.Homework
            };

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(updateMarkRequestDto.StudentHomeworkId))
                .ReturnsAsync(default(HomeworkStudent));
            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(updateMarkRequestDto.StudentHomeworkId))
                .ReturnsAsync(new List<HomeworkStudentHistory>()
                {
                    new HomeworkStudentHistory()
                });

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateMarkAsync(updateMarkRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        #region CreateHomeworkFromStudentAsyncTests

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_ValidData_ShouldReturnHomeworkStudentExample()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(homeworkStudentRequestDto);
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_NotExistingStudent_ShouldReturnNotFoundError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(default(Student));

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
            result.Error.Message.Should().BeEquivalentTo("Student with account id 0 was not found");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_NotExistingHomework_ShouldReturnNotFoundError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(default(Homework));

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
            result.Error.Message.Should().BeEquivalentTo($"Homework with id {homeworkStudentRequestDto.HomeworkId} was not found");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_StudentAlreadyHasHomework_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(true);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"You already add homework for this Hometask {homeworkStudentRequestDto.HomeworkId}");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_RequestDataIsDefault_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto = default;

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Please provide request data");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_StudentHasNostudentGroups_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(default(IList<long?>));

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Student has no student groups");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_NotExistingLessonId_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(default(Lesson));

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo("Please be sure that lesson exists");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_LessonIsNotSuitableForStudentsGroups_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 2, 3, 4 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 1 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Student with 0 Id number not include in student group which have been lesson with 0 Id number");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_NotExistingAttachments_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(false);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Given attachment ids do not exist: 1, 2");
        }

        [Fact]
        public async Task CreateHomeworkFromStudentAsync_LateSendingOfHomework_ShouldReturnHomeworkStudentExample()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MinValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Due date already finished. Due date {DateTime.MinValue}");
        }

        #endregion

        #region UpdateHomeworkFromStudentAsyncTests

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_ValidData_ShouldReturnHomeworkStudentExample()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(homeworkStudentRequestDto);
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_NotExistingStudent_ShouldReturnNotFoundError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(default(Student));

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
            result.Error.Message.Should().BeEquivalentTo("Student with account id 0 was not found");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_NotExistingHomework_ShouldReturnNotFoundError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(default(Homework));

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
            result.Error.Message.Should().BeEquivalentTo($"Homework with id {homeworkStudentRequestDto.HomeworkId} was not found");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_StudentHasNotHaveHomework_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(false);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"There is no homework for this Hometask {homeworkStudentRequestDto.HomeworkId}");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_StudentHasNostudentGroups_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(default(List<long?>));

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Student has no student groups");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_NotExistingLessonId_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(default(Lesson));

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo("Please be sure that lesson exists");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_LessonIsNotSuitableForStudentsGroups_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 2, 3, 4 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 1 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Student with 0 Id number not include in student group which have been lesson with 0 Id number");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_NotExistingAttachments_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(false);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Given attachment ids do not exist: 1, 2");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_NoHomeworkStudentForUpdate_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(default(HomeworkStudent));

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
            result.Error.Message.Should().BeEquivalentTo($"Homework with id {homework.Id} not Found");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_HomeworkStudentIsNotCreatedByStudent_ShouldReturnValidationError()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark(),
                    StudentId = 5,
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student() { Id = 7 });

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Sorry, but homework with id 0 is not created by this student");
        }

        [Fact]
        public async Task UpdateHomeworkFromStudentAsync_LateSendingOfHomework_ShouldReturnHomeworkStudentExample()
        {
            //Arrange
            var groupList = new List<long?>() { 0 };

            var attachments = new List<Attachment>()
            {
                new Attachment { Id = 1 },
                new Attachment { Id = 2 }
            };

            var homework = new Homework()
            {
                Id = 1
            };

            homeworkStudentRequestDto.AttachmentIds = new List<long>() { 1, 2 };

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkIdAsync(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MinValue
                });

            _homeworkStudentRepositoryMock.Setup(x => x.IsStudentHasHomeworkAsync(It.IsAny<long>(), homeworkStudentRequestDto.HomeworkId)).ReturnsAsync(true);

            _homeworkStudentRepositoryMock.Setup(x => x.GetByIdAsync(homeworkStudentRequestDto.HomeworkId))
                .ReturnsAsync(new HomeworkStudent()
                {
                    HomeworkId = 1,
                    IsSent = true,
                    Mark = new Mark()
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>())).ReturnsAsync(attachments);


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(homeworkStudentRequestDto, homework.Id);

            //Assert
            result.Data.Should().BeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
            result.Error.Message.Should().BeEquivalentTo($"Due date already finished. Due date {DateTime.MinValue}");
        }

        #endregion

        #region GetHomeworkStudentForMentorTests

        [Fact]
        public async Task GetHomeworkStudentForMentor_IsSentEqualsTrue_ShouldReturnStudentsHomeworks()
        {
            //Arrange 
            long homeworkId = 5;
            var homeworksStudent = (IList<HomeworkStudent>) new List<HomeworkStudent>
            {
                new HomeworkStudent{ Id = 10, HomeworkId = homeworkId, IsSent = true }
            };

            _currentUserServiceMock.Setup(x => x.EntityId).Returns(1);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);
            _homeworkRepositoryMock.Setup(x => x.GetMentorHomeworkAsync(It.IsAny<long>(), homeworkId)).Returns(Task.FromResult(new Homework { Id = homeworkId }));

            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);
            _homeworkStudentRepositoryMock.Setup(x => x.GetHomeworkStudentForMentor(homeworkId)).Returns(Task.FromResult(homeworksStudent));

            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);
            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(((List<HomeworkStudent>)homeworksStudent).First().Id)).Returns(Task.FromResult(It.IsAny<IList<HomeworkStudentHistory>>()));

            //Act
            var response = await _homeworkStudentService.GetHomeworkStudentForMentor(homeworkId);

            //Assert
            response.Data.Should().HaveCount(1);
            response.Data[0].Id.Should().Be(10);
            response.Data[0].HomeworkId.Should().Be(5);
        }

        [Fact]
        public async Task GetHomeworkStudentForMentor_HomeworkIsNull_ShouldReturnNotFoundError()
        {
            //Arrange 
            _currentUserServiceMock.Setup(x => x.EntityId).Returns(It.IsAny<long>());
            _homeworkRepositoryMock.Setup(x => x.GetMentorHomeworkAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult<Homework>(null));
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            //Act
            var response = await _homeworkStudentService.GetHomeworkStudentForMentor(1);

            //Assert
            response.Data.Should().BeNull();
            response.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetHomeworkStudentForMentor_IsSentEqualsIsFalseAndHomeworksStudentHistoryCountIsOne_ShouldReturnNotFoundError()
        {
            //Arrange
            var homeworkStudent = (IList<HomeworkStudent>) new List<HomeworkStudent>
            {
                new HomeworkStudent { IsSent = false, Id = 2, HomeworkId = 1 }
            };

            var homeworksStudentHistory = (IList<HomeworkStudentHistory>) new List<HomeworkStudentHistory>
            {
                new HomeworkStudentHistory()
            };

            _homeworkRepositoryMock.Setup(x => x.GetMentorHomeworkAsync(It.IsAny<long>(), 1)).Returns(Task.FromResult(new Homework { Id = 1 }));
            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _homeworkStudentRepositoryMock.Setup(x => x.GetHomeworkStudentForMentor(1)).Returns(Task.FromResult(homeworkStudent));
            _unitOfWorkMock.Setup(x => x.HomeworkStudentRepository).Returns(_homeworkStudentRepositoryMock.Object);

            _homeworkStudentHistoryRepositoryMock.Setup(x => x.GetHomeworkStudentHistoryByHomeworkStudentId(2)).Returns(Task.FromResult(homeworksStudentHistory));
            _unitOfWorkMock.Setup(x => x.HomeworkStudentHistoryRepository).Returns(_homeworkStudentHistoryRepositoryMock.Object);

            //Act
            var response = await _homeworkStudentService.GetHomeworkStudentForMentor(1);

            //Assert
            response.Data.Count.Should().Be(1);
            response.Data.First().HomeworkId.Should().Be(1);
            response.Data.First().Id.Should().Be(2);
        }

        #endregion 
    }
}
