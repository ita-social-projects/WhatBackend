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
        private readonly long nonExistentId = -1;

        public ScheduleServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _scheduledEventFactory = new ScheduledEventHandlerFactory();
            _scheduleRepositoryMock = new Mock<IScheduledEventRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_scheduleRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
        }

        private ScheduledEventFilterRequestDTO Get_ScheduledEventFilterRequestDTO(
            long? courseId = null,
            long? mentorId = null,
            long? groupId = null,
            long? themeId = null,
            long? studentAccountId = null,
            long? EventOccurenceId = null,
            DateTime? startDate = null,
            DateTime? finishDate = null)
        {
            return new ScheduledEventFilterRequestDTO
            {
                CourseID = courseId,
                MentorID = mentorId,
                GroupID = groupId,
                ThemeID = themeId,
                StudentAccountID = studentAccountId,
                EventOccurrenceID = EventOccurenceId,
                StartDate = startDate,
                FinishDate = finishDate
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

            var eventOccuranceRepositoryMock = new Mock<IEventOccurrenceRepository>();

            if (createScheduleDto != null)
            {
                eventOccuranceRepositoryMock.Setup(x => x.Add(
                    new EventOccurrence
                    {
                        StudentGroupId = existentGroupId,
                        EventStart = createScheduleDto.Range.StartDate,
                        Pattern = createScheduleDto.Pattern.Type
                    }
                    ));
            }

            _unitOfWorkMock.Setup(x => x.EventOccurrenceRepository).Returns(eventOccuranceRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateScheduleAsync_ValidScheduleRequest_ShouldReturnEventOccurrenceDTO()
        {
            //Arrange
            var createScheduleDto = new CreateScheduleDto
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

            EventOccurrence expected = new EventOccurrence
            {
                Pattern = createScheduleDto.Pattern.Type,
                StudentGroupId = createScheduleDto.Context.GroupID,
                EventStart = createScheduleDto.Range.StartDate,
                EventFinish = createScheduleDto.Range.FinishDate.Value,
                Storage = EventOccuranceStorageParser.GetPatternStorageValue(createScheduleDto.Pattern)
            };

            _themeRepositoryMock.Setup(x => x.IsEntityExistAsync(existentThemeId)).ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(existentMentorId)).ReturnsAsync(true);

            _studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(existentGroupId)).ReturnsAsync(true);

            Initialize(createScheduleDto);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(_mapper.Map<EventOccurrenceDTO>(expected));
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
        public async Task GetEventsFiltered_StartDateBiggerThanFinishDate_ShouldReturnValidationError()
        {
            // Arrange
            var finishDate =  DateTime.Now;
            var invalidStartDate = finishDate.AddDays(1);

            var scheduledEventFilterDTO = Get_ScheduledEventFilterRequestDTO(
                startDate: (DateTime?) invalidStartDate,
                finishDate: (DateTime?) finishDate);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            // Act
            var result = await scheduleService.GetEventsFiltered(scheduledEventFilterDTO);

            // Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }
    }
}
