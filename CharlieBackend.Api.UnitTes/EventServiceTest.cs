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
        private readonly ScheduledEvent validEvent;
        private readonly int existingId;
        private readonly int nonexistingId;
        public UpdateScheduledEventDto update;
        public EventServiceTest()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _eventRepositoryMock = new Mock<IScheduledEventRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _groupRepositoryMock = new Mock<IStudentGroupRepository>();
            validEvent = new ScheduledEvent
            {
                StudentGroupId = 1,
                ThemeId = 1,
                MentorId = 1
            };
            existingId = 551;
            nonexistingId = 999;
        }

        [Fact]
        public async Task GetEvent_ExistingId_ShouldReturnScheduledEvent() 
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.GetAsync(existingId);

            //Assert
            successResult.Should().NotBeNull();
            successResult.Should().BeEquivalentTo(_mapper.Map<ScheduledEventDTO>(validEvent));
        }

        [Fact]
        public async Task GetEvent_NonExistingId_ShouldReturnNull()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.GetAsync(nonexistingId);

            //Assert
            successResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateEvent_ValidData_ShouldReturnScheduledEvent()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(validEvent);
            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Mentor { AccountId = 111 });
            _themeRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Theme { Id = 1 });
            _groupRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new StudentGroup { Id = 1 });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_groupRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            update = new UpdateScheduledEventDto
            {
                StudentGroupId = 1,
                ThemeId = 1,
                MentorId = 1
            };
            var expectedUpdate = new ScheduledEventDTO
            {
                StudentGroupId = 1,
                ThemeId = 1,
                MentorId = 1
            };

            //Act
            var successResult = await eventService.UpdateAsync(existingId, update);

            //Assert
            successResult.Should().NotBeNull();
            successResult.Should().BeEquivalentTo(expectedUpdate);
        }

        [Fact]
        public async Task UpdateEvent_NotValidData_ShouldThrowError()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(validEvent);
            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Mentor { AccountId = 111 });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);
            var expectedUpdate = new UpdateScheduledEventDto{};

            //Act & Assert
            Invoking(() => eventService.UpdateAsync(existingId, expectedUpdate)).Should().Throw<EntityValidationException>();
        }

        [Fact]
        public async Task DeleteEvent_ValidData_ShouldReturnTrue()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            //Act
            var successResult = await eventService.DeleteAsync(existingId);

            //Assert
            successResult.Data.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteEvent_NotValidData_ShouldReturnException()
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .Throws(new Exception());

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper);

            // Act & Assert
            Invoking(() => eventService.DeleteAsync(existingId)).Should().Throw<Exception>();
        }
    }
}
