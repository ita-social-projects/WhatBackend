using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
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
        private readonly Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private new readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly HomeworkStudentService _homeworkStudentService;

        public HomeworkStudentServiceTests()
        {
            _loggerMock = new Mock<ILogger<HomeworkStudentService>>();
            _homeworkRepositoryMock = new Mock<IHomeworkRepository>();
            _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _homeworkStudentService = new HomeworkStudentService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object, _currentUserServiceMock.Object);
        }
        [Fact]
        public async Task CreateHomeworkFromStudentAsync_ValidData_ShouldReturnHomeworkStudentExample()
        {

            //Arrange
            HomeworkStudentRequestDto homeworkStudentRequestDto = new HomeworkStudentRequestDto()
            {
                //HomeworkId = 1,
                HomeworkText = "HomeworkStudentRequestdto text",
                IsSent = true
            };
            homeworkStudentRequestDto.AttachmentIds = new List<long> { 1, 2 };

            HomeworkStudentDto homeworkStudentDto = new HomeworkStudentDto()
            {
                Id = 0,
                HomeworkId = 0,
                AttachmentIds = new List<long>() { 1, 2 }
            };

            //_lessonRepositoryMock.Setup(l => l.IsEntityExistAsync(1)).ReturnsAsync(true);

            _homeworkRepositoryMock.Setup(x => x.Add(It.IsAny<Homework>())).Callback<Homework>(x => new Homework());

            _attachmentRepositoryMock.Setup(x => x.IsEntityExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync()

            _attachmentRepositoryMock.Setup(x => x.GetAttachmentsByIdsAsync(It.IsAny<IList<long>>()))
                .ReturnsAsync(new List<Attachment>()
                {
                    new Attachment { Id = 1 },
                    new Attachment { Id = 2 }
                });

            //_unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.HomeworkRepository).Returns(_homeworkRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.AttachmentRepository).Returns(_attachmentRepositoryMock.Object);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var result = await _homeworkStudentService.CreateHomeworkFromStudentAsync(homeworkStudentRequestDto);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(homeworkStudentDto, x => x.Excluding(f => f.PublishingDate));
        }
    }
}
