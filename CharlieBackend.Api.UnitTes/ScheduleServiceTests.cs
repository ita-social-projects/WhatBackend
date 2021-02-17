using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using FluentAssertions;
using CharlieBackend.Business.Services.Interfaces;
using System.Linq;

namespace CharlieBackend.Api.UnitTest
{
    public class ScheduleServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly IScheduledEventHandlerFactory _scheduledEventFactory;
        private Mock<IScheduledEventRepository> _scheduleRepositoryMock;
        private Mock<IEventOccurrenceRepository> _eventOccuranceRepositoryMock;
        private CreateScheduleDto validScheduleDTO;
        private EventOccurrence validEventOccurrence;

        public ScheduleServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _scheduledEventFactory = new ScheduledEventHandlerFactory();
            _scheduleRepositoryMock = new Mock<IScheduledEventRepository>();
            _eventOccuranceRepositoryMock = new Mock<IEventOccurrenceRepository>();
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_scheduleRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.EventOccurrenceRepository).Returns(_eventOccuranceRepositoryMock.Object);
            validScheduleDTO = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Daily,
                    Interval = 1,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                },

                Range = new OccurenceRange
                {
                    StartDate = DateTime.Parse("Jan 1, 2021"),
                    FinishDate = DateTime.Parse("Feb 1, 2021")
                },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = 3,
                    MentorID = 1,
                    ThemeID = 5
                }
            };

            validEventOccurrence = new EventOccurrence
            {
                Pattern = validScheduleDTO.Pattern.Type,
                StudentGroupId = validScheduleDTO.Context.GroupID,
                EventStart = validScheduleDTO.Range.StartDate,
                EventFinish = validScheduleDTO.Range.FinishDate.Value,
                Storage = EventOccuranceStorageParser.GetPatternStorageValue(validScheduleDTO.Pattern)
            };
        }

        private void Initialize(CreateScheduleDto createScheduleDto, Mock<IThemeRepository> themeRepositoryMock = null,
            Mock<IMentorRepository> mentorRepositoryMock = null, Mock<IStudentGroupRepository> studentGroupRepositoryMock = null)
        {
            _scheduleRepositoryMock.Setup(x => x.AddRange(new List<ScheduledEvent>()
            {
                new ScheduledEvent
                {
                    ThemeId = 5,
                    StudentGroupId = 3,
                    MentorId = 1
                }
            }
            ));

            _eventOccuranceRepositoryMock.Setup(x => x.Add(
                new EventOccurrence
                {
                    StudentGroupId = 3,
                    EventStart = createScheduleDto.Range.StartDate,
                    Pattern = createScheduleDto.Pattern.Type
                }
                ));

            if (themeRepositoryMock != null)
            {
                _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);
            }

            if (mentorRepositoryMock != null)
            {
                _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorRepositoryMock.Object);
            }

            if (studentGroupRepositoryMock != null)
            {
                _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepositoryMock.Object);
            }
        }

        [Fact]
        public async Task CreateScheduleAsync_ValidScheduleRequest_ShouldReturnEventOccurrenceDTO()
        {
            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(true);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(validScheduleDTO, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(validScheduleDTO);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(validEventOccurrence));
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidScheduleRequest_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Weekly,
                    Interval = -1,
                    DaysOfWeek = new List<DayOfWeek> {},
                },

                Range = new OccurenceRange
                {
                    StartDate = DateTime.Parse("Jan 1, 2021")
                },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = 1
                }
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(false);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(false);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(createScheduleDto, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetEventOccurrenceByIdAsync_ExistingId_ShouldReturnExpectedEventOccurrenceDTO()
        {
            //Arrange
            var existingId = 0;

            Initialize(validScheduleDTO);

            _eventOccuranceRepositoryMock.Setup(x => x.GetByIdAsync(existingId)).ReturnsAsync(validEventOccurrence);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.GetEventOccurrenceByIdAsync(existingId);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(validEventOccurrence));
        }

        [Fact]
        public async Task GetEventOccurrenceByIdAsync_NonExistingId_ShouldReturnNotFound()
        {
            //Arrange
            var nonExistingId = -1;

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.GetEventOccurrenceByIdAsync(nonExistingId);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetEventsFiltered_ValidScheduledEventFilterRequest_ShouldReturnFilteredListOfScheduleEventDTO()
        {
            //Arrange
            var validRequest = new ScheduledEventFilterRequestDTO
            {
                MentorID = 1,
                GroupID = 3
            };

            var expectedFilteredList = new List<ScheduledEventDTO>()
            {
            new ScheduledEventDTO
            {
                StudentGroupId = 3,
                ThemeId = 5,
                MentorId = 1,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
                EventOccuranceId = 0,
                Id = 0
            } };

            _scheduleRepositoryMock.Setup(x => x.GetEventsFilteredAsync(validRequest)).ReturnsAsync(new List<ScheduledEvent>()
            {
            new ScheduledEvent
            {
                StudentGroupId = 3,
                ThemeId = 5,
                MentorId = 1,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
            } });

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(validScheduleDTO, mentorRepositoryMock: mentorRepositoryMock, studentGroupRepositoryMock: studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.GetEventsFiltered(validRequest);

            //Assert
            result.Should().NotBeNull();
            result.Data.ElementAt(0).Should().BeEquivalentTo(expectedFilteredList.ElementAt(0));
        }

        [Fact]
        public async Task GetEventsFiltered_NonValidScheduledEventFilterRequest_ShouldReturnValidationError()
        {
            //Arrange
            var nonValidRequest = new ScheduledEventFilterRequestDTO
            { 
                CourseID = -1,
                MentorID = -1,
                GroupID = -1
            };

            var courseRepositoryMock = new Mock<ICourseRepository>();
            courseRepositoryMock.Setup(x => x.IsEntityExistAsync(-1)).ReturnsAsync(false);

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(false);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(-1)).ReturnsAsync(false);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(-1)).ReturnsAsync(false);

            Initialize(validScheduleDTO, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(courseRepositoryMock.Object);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.GetEventsFiltered(nonValidRequest);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task DeleteScheduleByIdAsync_NonExistingId_ShouldReturnValidationError()
        {
            //Arrange
            var nonExistingId = -1;
            var startDate = DateTime.Parse("Jan 1, 2021");
            var finishDate = DateTime.Parse("Feb 1, 2021");

            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistingId)).ReturnsAsync(false);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.DeleteScheduleByIdAsync(nonExistingId, startDate, finishDate);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task DeleteScheduleByIdAsync_ExistingId_ShouldReturn()
        {
            //Arrange
            var existingId = 1;

            validEventOccurrence.ScheduledEvents = new List<ScheduledEvent>()
                {
                    new ScheduledEvent
                    {
                        StudentGroupId = 3,
                        ThemeId = 5,
                        MentorId = 1,
                        EventStart = DateTime.Parse("Jan 1, 2021"),
                        EventFinish = DateTime.Parse("Feb 1, 2021"),
                        EventOccurrenceId = 1
                    }
                };

            var expected = new EventOccurrenceDTO
            {
                StudentGroupId = 3,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Jan 1, 2021"),
                Events = new List<ScheduledEventDTO>()
                {
                    new ScheduledEventDTO()
                {
                        StudentGroupId = 3,
                        ThemeId = 5,
                        MentorId = 1,
                        EventStart = DateTime.Parse("Jan 1, 2021"),
                        EventFinish = DateTime.Parse("Feb 1, 2021"),
                        EventOccuranceId = 1
                } },
                Id = 0
            };
            var startDate = DateTime.Parse("Jan 1, 2021");
            var finishDate = DateTime.Parse("Feb 1, 2021");

            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);
            _eventOccuranceRepositoryMock.Setup(x => x.GetByIdAsync(existingId)).ReturnsAsync(validEventOccurrence);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.DeleteScheduleByIdAsync(existingId, startDate, finishDate);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_NonExistingScheduledEventId_ShouldReturnValidationError()
        {
            //Arrange
            var nonExistingId = -1;
            UpdateScheduledEventDto request = new UpdateScheduledEventDto();

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistingId)).ReturnsAsync(false);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(nonExistingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdNullRequest_ShouldReturnValidationError()
        {
            //Arrange
            var existingId = 1;
            UpdateScheduledEventDto request = null;

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdValidRequest_ShouldReturnUpdatedScheduleDTO()
        {
            //Arrange
            var existingId = 1;

            UpdateScheduledEventDto request = new UpdateScheduledEventDto
            {
                StudentGroupId = 2,
                ThemeId = 3
            };

            ScheduledEvent expectedEvent = new ScheduledEvent
            {
                StudentGroupId = 2,
                ThemeId = 3,
                MentorId = 1,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
                EventOccurrenceId = 1
            };

            var expectedDTO = _mapper.Map<ScheduledEventDTO>(expectedEvent);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(true);
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(2)).ReturnsAsync(true);

            _scheduleRepositoryMock.Setup(x => x.GetByIdAsync(existingId)).ReturnsAsync(expectedEvent);

            Initialize(validScheduleDTO, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expectedDTO);
        }

        [Fact]
        public async Task UpdateEventOccurrenceById_ExistingIdValidScheduleDTO_ShouldReturnExpectedEventOccurrenceDTO()
        {
            validEventOccurrence.ScheduledEvents = new List<ScheduledEvent>()
                {
                    new ScheduledEvent
                    {
                        StudentGroupId = 2,
                        ThemeId = 3,
                        MentorId = 1,
                        EventStart = DateTime.Parse("Jan 1, 2021"),
                        EventFinish = DateTime.Parse("Feb 1, 2021"),
                        EventOccurrenceId = 1
                    }
                };

            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);
            _eventOccuranceRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(validEventOccurrence);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(true);
            
            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(validScheduleDTO, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateEventOccurrenceById(1, validScheduleDTO);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(validEventOccurrence));
        }

        [Fact]
        public async Task UpdateEventOccurrenceById_NonExistingIdValidRequestScheduleDTO_ShouldReturnValidationError()
        {
            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(-1)).ReturnsAsync(false);

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(true);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(validScheduleDTO,themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateEventOccurrenceById(-1, validScheduleDTO);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateEventOccurrenceById_ExistingIdNullRequestScheduleDTO_ShouldReturnValidationError()
        {
            CreateScheduleDto request = null;

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateEventOccurrenceById(1, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
