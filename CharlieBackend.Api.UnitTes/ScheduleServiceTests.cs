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
        private readonly Mock<IScheduledEventRepository> _scheduleRepositoryMock;
        private readonly Mock<IThemeRepository> _themeRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly long existentGroupId = 3;
        private readonly long existentMentorId = 1;
        private readonly long existentThemeId = 5;
        private readonly long newGroupId = 2;
        private readonly long newThemeId = 3;
        private readonly long existingId = 1;
        private readonly long nonExistentId = -1;
        private Mock<IEventOccurrenceRepository> _eventOccuranceRepositoryMock;
        private CreateScheduleDto validScheduleDTO;
        private EventOccurrence validEventOccurrence;

        public ScheduleServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _eventOccuranceRepositoryMock = new Mock<IEventOccurrenceRepository>();
            _scheduledEventFactory = new ScheduledEventHandlerFactory();
            _scheduleRepositoryMock = new Mock<IScheduledEventRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_scheduleRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
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
                    GroupID = existentGroupId,
                    MentorID = existentMentorId,
                    ThemeID = existentThemeId
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

        private void Initialize(CreateScheduleDto createScheduleDto)
        {
            _scheduleRepositoryMock.Setup(x => x.AddRange(new List<ScheduledEvent>()
            {
                new ScheduledEvent
                {
                    ThemeId = existentThemeId,
                    StudentGroupId = existentGroupId,
                    MentorId = existentMentorId
                }
            }
            ));
            if (createScheduleDto != null)
            {
                _eventOccuranceRepositoryMock.Setup(x => x.Add(
                    new EventOccurrence
                    {
                        StudentGroupId = existentGroupId,
                        EventStart = createScheduleDto.Range.StartDate,
                        Pattern = createScheduleDto.Pattern.Type
                    }
                    ));
            }
        }

        [Fact]
        public async Task CreateScheduleAsync_ValidScheduleRequest_ShouldReturnEventOccurrenceDTO()
        {
            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(validScheduleDTO);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(validEventOccurrence));
        }

        [Fact]
        public async Task CreateScheduleAsync_PatternIntervalLessThanNull_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Daily,
                    Interval = -1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidGroupId_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Daily,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = nonExistentId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidMentorId_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Daily,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId,
                    MentorID = nonExistentId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidThemeId_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Daily,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId,
                    MentorID = existentMentorId,
                    ThemeID = nonExistentId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidDaysOfWeekInWeekly_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.Weekly,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId,
                    MentorID = existentMentorId,
                    ThemeID = existentThemeId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidDatesOfWeekInAbsoluteMonthly_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.AbsoluteMonthly,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId,
                    MentorID = existentMentorId,
                    ThemeID = existentThemeId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NotValidDaysOfWeekInRelativeMonthly_ShouldReturnValidationError()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.AbsoluteMonthly,
                    Interval = 1
                },

                Range = new OccurenceRange { },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = existentGroupId,
                    MentorID = existentMentorId,
                    ThemeID = existentThemeId
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateScheduleAsync_NullRequest_ShouldReturnValidationError()
        {
            //Arrange
            CreateScheduleDto createScheduleDto = null;

            Initialize(createScheduleDto);

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
            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.GetEventOccurrenceByIdAsync(nonExistentId);

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
                MentorID = existentMentorId,
                GroupID = existentGroupId
            };

            var expectedFilteredList = new List<ScheduledEventDTO>()
            {
            new ScheduledEventDTO
            {
                StudentGroupId = existentGroupId,
                ThemeId = existentThemeId,
                MentorId = existentMentorId,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
                EventOccuranceId = 0,
                Id = 0
            } };

            _scheduleRepositoryMock.Setup(x => x.GetEventsFilteredAsync(validRequest)).ReturnsAsync(new List<ScheduledEvent>()
            {
            new ScheduledEvent
            {
                StudentGroupId = existentGroupId,
                ThemeId = existentThemeId,
                MentorId = existentMentorId,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
            } });

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

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
                CourseID = nonExistentId,
                MentorID = nonExistentId,
                GroupID = nonExistentId
            };

            var courseRepositoryMock = new Mock<ICourseRepository>();
            courseRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            Initialize(validScheduleDTO);

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
            var startDate = DateTime.Parse("Jan 1, 2021");
            var finishDate = DateTime.Parse("Feb 1, 2021");

            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.DeleteScheduleByIdAsync(nonExistentId, startDate, finishDate);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task DeleteScheduleByIdAsync_ExistingId_ShouldReturn()
        {
            //Arrange
            validEventOccurrence.ScheduledEvents = new List<ScheduledEvent>()
                {
                    new ScheduledEvent
                    {
                        StudentGroupId = existentGroupId,
                        ThemeId = existentThemeId,
                        MentorId = existentMentorId,
                        EventStart = DateTime.Parse("Jan 1, 2021"),
                        EventFinish = DateTime.Parse("Feb 1, 2021"),
                        EventOccurrenceId = 1
                    }
                };

            var expected = new EventOccurrenceDTO
            {
                StudentGroupId = existentGroupId,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Jan 1, 2021"),
                Events = new List<ScheduledEventDTO>()
                {
                    new ScheduledEventDTO()
                {
                        StudentGroupId = existentGroupId,
                        ThemeId = existentThemeId,
                        MentorId = existentMentorId,
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
            UpdateScheduledEventDto request = new UpdateScheduledEventDto();

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(nonExistentId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdNotValidGroupId_ShouldReturnValidationError()
        {
            //Arrange
            var request = new UpdateScheduledEventDto
            {
                StudentGroupId = nonExistentId
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdNotValidMentorId_ShouldReturnValidationError()
        {
            //Arrange
            var request = new UpdateScheduledEventDto
            {
                StudentGroupId = existentGroupId,
                MentorId = nonExistentId
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdNotValidDates_ShouldReturnValidationError()
        {
            //Arrange
            var request = new UpdateScheduledEventDto
            {
                StudentGroupId = existentGroupId,
                MentorId = existentMentorId,
                ThemeId = nonExistentId,
                EventStart = DateTime.Parse("Feb 1, 2021"),
                EventEnd = DateTime.Parse("Jan 1, 2021")
            };

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateScheduledEventByID(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateScheduledEventByID_ExistingScheduledEventIdNullRequest_ShouldReturnValidationError()
        {
            //Arrange
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
            UpdateScheduledEventDto request = new UpdateScheduledEventDto
            {
                StudentGroupId = newGroupId,
                ThemeId = newThemeId
            };

            ScheduledEvent expectedEvent = new ScheduledEvent
            {
                StudentGroupId = newGroupId,
                ThemeId = newThemeId,
                MentorId = existentMentorId,
                EventStart = DateTime.Parse("Jan 1, 2021"),
                EventFinish = DateTime.Parse("Feb 1, 2021"),
                EventOccurrenceId = existingId
            };

            var expectedDTO = _mapper.Map<ScheduledEventDTO>(expectedEvent);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);
            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(newThemeId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);
            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(newGroupId)).ReturnsAsync(true);

            _scheduleRepositoryMock.Setup(x => x.GetByIdAsync(existingId)).ReturnsAsync(expectedEvent);

            Initialize(validScheduleDTO);

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
                        StudentGroupId = newGroupId,
                        ThemeId = newThemeId,
                        MentorId = existentMentorId,
                        EventStart = DateTime.Parse("Jan 1, 2021"),
                        EventFinish = DateTime.Parse("Feb 1, 2021"),
                        EventOccurrenceId = existingId
                    }
                };

            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);
            _eventOccuranceRepositoryMock.Setup(x => x.GetByIdAsync(existingId)).ReturnsAsync(validEventOccurrence);

            _scheduleRepositoryMock.Setup(x => x.IsEntityExistAsync(existingId)).ReturnsAsync(true);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);
            
            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateEventOccurrenceById(existingId, validScheduleDTO);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(validEventOccurrence));
        }

        [Fact]
        public async Task UpdateEventOccurrenceById_NonExistingIdValidRequestScheduleDTO_ShouldReturnValidationError()
        {
            _eventOccuranceRepositoryMock.Setup(x => x.IsEntityExistAsync(nonExistentId)).ReturnsAsync(false);

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(validScheduleDTO);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.UpdateEventOccurrenceById(nonExistentId, validScheduleDTO);

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
            var result = await scheduleService.UpdateEventOccurrenceById(existingId, request);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }
    }
}
