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
        private readonly Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private new readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly HomeworkStudentService _homeworkStudentService;
        private HomeworkStudentRequestDto homeworkStudentRequestDto = new HomeworkStudentRequestDto()
        {
            HomeworkId = 1,
            HomeworkText = "HomeworkStudentRequestdto text",
            IsSent = true
        };

        public HomeworkStudentServiceTests()
        {
            _loggerMock = new Mock<ILogger<HomeworkStudentService>>();
            _homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            _homeworkStudentRepositoryMock = new Mock<IHomeworkStudentRepository>();
            _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _homeworkStudentService = new HomeworkStudentService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object, _currentUserServiceMock.Object);
        }
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

            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkId(It.IsAny<long>())).ReturnsAsync(new Lesson() { StudentGroupId = 0 });

            _homeworkRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Homework()
                {
                    Id = 1,
                    DueDate = DateTime.MaxValue
                });

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(It.IsAny<long>())).ReturnsAsync(new Student());

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsByStudentId(It.IsAny<long>())).ReturnsAsync(groupList);

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
        public async Task CreateHomeworkFromStudentAsync_NotExistingLessonId_ShouldReturnValidationError()
        {
            //Arrange
            _lessonRepositoryMock.Setup(x => x.GetLessonByHomeworkId(It.IsAny<long>())).ReturnsAsync(default(Lesson));
            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            //Act
            //var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            //result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }
    }
}
