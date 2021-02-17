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
        private Mock<IScheduledEventRepository> _scheduleRepositoryMock;

        public ScheduleServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _scheduledEventFactory = new ScheduledEventHandlerFactory();
            _scheduleRepositoryMock = new Mock<IScheduledEventRepository>();
            _unitOfWorkMock.Setup(x => x.ScheduledEventRepository).Returns(_scheduleRepositoryMock.Object);
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

            var eventOccuranceRepositoryMock = new Mock<IEventOccurrenceRepository>();

            eventOccuranceRepositoryMock.Setup(x => x.Add(
                new EventOccurrence
                {
                    StudentGroupId = 3,
                    EventStart = createScheduleDto.Range.StartDate,
                    Pattern = createScheduleDto.Pattern.Type
                }
                ));

            _unitOfWorkMock.Setup(x => x.EventOccurrenceRepository).Returns(eventOccuranceRepositoryMock.Object);

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
                    GroupID = 3,
                    MentorID = 1,
                    ThemeID = 5
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

            var themeRepositoryMock = new Mock<IThemeRepository>();

            themeRepositoryMock.Setup(x => x.IsEntityExistAsync(5)).ReturnsAsync(true);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(3)).ReturnsAsync(true);

            Initialize(createScheduleDto, themeRepositoryMock, mentorRepositoryMock, studentGroupRepositoryMock);

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
                    GroupID = 1
                }
            };

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            Initialize(createScheduleDto, studentGroupRepositoryMock: studentGroupRepositoryMock);

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
                    GroupID = 1
                }
            };

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(false);

            Initialize(createScheduleDto, studentGroupRepositoryMock: studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
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
                    GroupID = 1
                }
            };

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.IsEntityExistAsync(1)).ReturnsAsync(true);

            Initialize(createScheduleDto, studentGroupRepositoryMock: studentGroupRepositoryMock);

            var scheduleService = new ScheduleService(_unitOfWorkMock.Object, _mapper, _scheduledEventFactory);

            //Act
            var result = await scheduleService.CreateScheduleAsync(createScheduleDto);

            //Assert
            result.Should().NotBeNull();
            result.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        /*[Fact]
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
        }*/



        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
