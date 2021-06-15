using Moq;
using Xunit;
using System;
using AutoMapper;
using FluentAssertions;
using System.Threading.Tasks;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Entities;
using CharlieBackend.Business.Exceptions;
using static FluentAssertions.FluentActions;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Business.Services.ScheduleServiceFolder;

namespace CharlieBackend.Api.UnitTest
{
    public class EventServiceTest : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<IScheduledEventRepository> _eventRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly Mock<IThemeRepository> _themeRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _groupRepositoryMock;
        private readonly ScheduledEvent _validEvent;
        private readonly int _existingId;
        private readonly int _nonexistingId;
        public UpdateScheduledEventDto _update;

        public EventServiceTest()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _eventRepositoryMock = new Mock<IScheduledEventRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _groupRepositoryMock = new Mock<IStudentGroupRepository>();
            _validEvent = new ScheduledEvent
            {
                StudentGroupId = 1,
                ThemeId = 1,
                MentorId = 1
            };
            _existingId = 551;
            _nonexistingId = 999;
        }

        [Fact]
        public async Task GetEvent_ExistingId_ShouldReturnScheduledEvent() 
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.GetAsync(_existingId);

            //Assert
            successResult.Should().NotBeNull();
            successResult.Should().BeEquivalentTo(_mapper.Map<ScheduledEventDTO>(_validEvent));
        }

        [Fact]
        public async Task GetEvent_NonExistingId_ShouldReturnNull()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.GetAsync(_nonexistingId);

            //Assert
            successResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateEvent_ValidData_ShouldReturnScheduledEvent()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);
            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Mentor { AccountId = 111 });
            _themeRepositoryMock.Setup(x => x.GetByIdAsync(222))
                .ReturnsAsync(new Theme { Id = 1 });
            _groupRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new StudentGroup { Id = 1 });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_groupRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);
            
            _update = new UpdateScheduledEventDto
            {
                StudentGroupId = 1,
                ThemeId = 222,
                MentorId = 1
            };
            var expectedUpdate = new ScheduledEventDTO
            {
                StudentGroupId = 1,
                ThemeId = 222,
                MentorId = 1
            };

            //Act
            var successResult = await eventService.UpdateAsync(_existingId, _update);

            //Assert
            successResult.Should().NotBeNull();
            successResult.Should().BeEquivalentTo(expectedUpdate);
        }

        [Fact]
        public async Task UpdateEvent_NotValidData_ShouldThrowError()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);
            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Mentor { AccountId = 111 });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);
            var expectedUpdate = new UpdateScheduledEventDto{};

            //Act & Assert
            Invoking(() => eventService.UpdateAsync(_existingId, expectedUpdate)).Should().Throw<EntityValidationException>();
        }

        [Fact]
        public async Task DeleteEvent_ValidData_ShouldReturnTrue()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.DeleteAsync(_existingId);

            //Assert
            successResult.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteEvent_NotValidData_ShouldReturnException()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .Throws(new Exception());

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            // Act & Assert
            Invoking(() => eventService.DeleteAsync(_existingId)).Should().Throw<Exception>();
        }
    }
}
