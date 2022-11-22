using AutoMapper;
using CharlieBackend.Business.Exceptions;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static FluentAssertions.FluentActions;

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
        private readonly int _lessonId;
        private readonly int _nonexistingId;
        public UpdateScheduledEventDto _update;
        private readonly Mock<ISchedulesEventsDbEntityVerifier> _validator;

        public EventServiceTest()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _eventRepositoryMock = new Mock<IScheduledEventRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _groupRepositoryMock = new Mock<IStudentGroupRepository>();
            _validEvent = new ScheduledEvent{  };
            _existingId = 551;
            _lessonId = 11;
            _nonexistingId = 999;
            _validator = new Mock<ISchedulesEventsDbEntityVerifier>();
        }

        [Fact]
        public async Task GetEvent_ExistingId_ShouldReturnScheduledEvent() 
        {
            //Arrange
            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

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

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            //Act
            var successResult = await eventService.GetAsync(_nonexistingId);

            //Assert
            successResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateEvent_ValidData_ShouldReturnScheduledEvent()
        {
            //Arrange
            long updatedStudentGroupId = 1;
            long updatedThemeId = 222;
            long updatedMentorId = 1;

            _eventRepositoryMock.Setup(x => x.GetByIdAsync(_existingId))
                .ReturnsAsync(_validEvent);
            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(updatedMentorId))
                .ReturnsAsync(new Mentor { });
            _themeRepositoryMock.Setup(x => x.GetByIdAsync(updatedThemeId))
                .ReturnsAsync(new Theme {  });
            _groupRepositoryMock.Setup(x => x.GetByIdAsync(updatedStudentGroupId))
                .ReturnsAsync(new StudentGroup {  });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_groupRepositoryMock.Object);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            
            _update = new UpdateScheduledEventDto
            {
                StudentGroupId = updatedStudentGroupId,
                ThemeId = updatedThemeId,
                MentorId = updatedMentorId
            };
            var expectedUpdate = new ScheduledEventDTO
            {
                StudentGroupId = updatedStudentGroupId,
                ThemeId = updatedThemeId,
                MentorId = updatedMentorId
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
                .ReturnsAsync(new Mentor { });

            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);
            var expectedUpdate = new UpdateScheduledEventDto{};
            _validator.Setup(x => x.ValidateUpdatedScheduleAsync(expectedUpdate)).ReturnsAsync(ResponseMessages.NotExist("Mentor"));


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

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

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

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            // Act & Assert
            Invoking(() => eventService.DeleteAsync(_existingId)).Should().Throw<Exception>();
        }

        [Fact]
        public async Task ConnectScheduleToLessonById_ValidData_ShouldReturnScheduledEvent() 
        {
            //Arrange
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            _eventRepositoryMock.Setup(x => x.ConnectEventToLessonByIdAsync(_existingId, _lessonId))
                .ReturnsAsync(_validEvent);

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            //Act
            var successResult = await eventService.ConnectScheduleToLessonById(_existingId, _lessonId);

            //Assert
            successResult.Data.Should().BeEquivalentTo(_mapper.Map<ScheduledEventDTO>(_validEvent));
        }

        [Fact]
        public async Task ConnectScheduleToLessonById_NonExistingEventId_ShouldReturnNotFoundExeption()
        {
            //Arrange
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            ScheduledEvent wrongScheduledEvent = new ScheduledEvent { Id = _nonexistingId };

            var wrongEventRepositoryMock = new Mock<IScheduledEventRepository>();

            wrongEventRepositoryMock.Setup(x => x.ConnectEventToLessonByIdAsync(wrongScheduledEvent.Id, _lessonId))
                .ReturnsAsync(_validEvent);

            _validator.Setup(x => x.ValidateConnectEventToLessonAsync(wrongScheduledEvent.Id, _lessonId)).ReturnsAsync(ResponseMessages.NotExist("EventID"));

            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            //Act
            var successResult = await eventService.ConnectScheduleToLessonById(wrongScheduledEvent.Id, _lessonId);

            //Assert
            successResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);

        }
        [Fact]
        public async Task ConnectScheduleToLessonById_LessonIsAlreadyConnectionToAnotherSheduledEvent_ShouldReturnConflictExeption()
        {
            //Arrange
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_eventRepositoryMock.Object);

            _eventRepositoryMock.Setup(x => x.IsLessonConnectedToSheduledEventAsync(_lessonId))
                .ReturnsAsync(true);
            _validator.Setup(x => x.ValidateConnectEventToLessonAsync(_existingId, _lessonId)).ReturnsAsync($"Lesson with id={_lessonId} is already associated with another ScheduledEvent. ");


            var eventService = new EventsService(_unitOfWorkMock.Object, _mapper, _validator.Object);

            //Act
            var successResult = await eventService.ConnectScheduleToLessonById(_existingId, _lessonId);

            //Assert
            successResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);

        }
    }
}
