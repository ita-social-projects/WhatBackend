using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Exceptions;
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
    public class LessonServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<LessonService>> _loggerMock;

        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private readonly Mock<IThemeRepository> _themeRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<IVisitRepository> _visitRepositoryMock;
        private readonly Mock<IMarkRepository> _markRepositoryMock;
        private readonly LessonService _lessonService;

        private static long visitStudentIdPresenceTrue = 11;
        private static long visitStudentIdPresenceFalse = 14;
        private static long mentorId = 2;
        private static long studentId = 1;
        private static long mentorWrongId = 31;
        private static long studentGroupId = 3;
        private static long studentGroupWr = 100;
        private static string themeName = "ExampleName";
        private static DateTime lessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z");

        private readonly DateTime _startDate = DateTime.Parse(_stringStartDate);
        private readonly DateTime _finishDate = DateTime.Parse(_stringFinishDate);

        private const long _id = 1;
        private const long _wrongId = -1;
        private const long defaultId = default;
        private const string _stringStartDate = "01.01.2000";
        private const string _stringFinishDate = "01.01.2023";

        public LessonServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _loggerMock = new Mock<ILogger<LessonService>>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _visitRepositoryMock = new Mock<IVisitRepository>();
            _markRepositoryMock = new Mock<IMarkRepository>();

            MockEntities();


            _lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object,
                logger: _loggerMock.Object
                );
        }

        private static List<VisitDto> CreateVisitDto()
        {
            return new List<VisitDto>
            {
                new VisitDto()
                {
                    StudentId = visitStudentIdPresenceTrue,
                    Presence = true
                },

                new VisitDto()
                {
                    StudentId = visitStudentIdPresenceFalse,
                    Presence = false
                }
            };
        }

        private static LessonDto AddLessonDto()
        {
            return new LessonDto()
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonDate = lessonDate,
                LessonVisits = CreateVisitDto()
            };
        }

        private static CreateLessonDto AddCreateLessonDto()
        {
            return new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonDate = lessonDate,
                LessonVisits = CreateVisitDto()
            };
        }

        private static StudentGroup CreateStudentGroup()
        {
            List<StudentOfStudentGroup> studentOfStudentGroup = new List<StudentOfStudentGroup>
            {
                new StudentOfStudentGroup { StudentGroupId = studentGroupId, StudentId = visitStudentIdPresenceTrue },
                new StudentOfStudentGroup { StudentGroupId = studentGroupId, StudentId = visitStudentIdPresenceFalse }
            };
            return new StudentGroup() { Id = 3, StudentsOfStudentGroups = studentOfStudentGroup };
        }

        private List<Lesson> GetLessonsList()
        {
            return new List<Lesson>()
            {
                _mapper.Map<Lesson>(AddLessonDto()),
                _mapper.Map<Lesson>(AddLessonDto())
            };
        }

        private void MockEntities()
        {
            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(CreateStudentGroup().Id)).ReturnsAsync(CreateStudentGroup());

            _studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(CreateStudentGroup().Id))
                .ReturnsAsync(new List<long?> { visitStudentIdPresenceTrue, visitStudentIdPresenceFalse });

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(_lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(_themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(_visitRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MarkRepository).Returns(_markRepositoryMock.Object);


            _currentUserServiceMock = GetCurrentUserAsExistingStudent();
        }

        [Fact]
        public async Task CreateLesson_ThrowException_ShouldReturnNull()
        {
            //Arrange
            CreateLessonDto createLessonDto = AddCreateLessonDto();

            _themeRepositoryMock.Setup(x => x.GetThemeByNameAsync(themeName)).Throws(new Exception());

            //Act
            var result = await _lessonService.CreateLessonAsync(createLessonDto);

            //Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task CreateLesson_ValidDataPassed_ShouldBeNotNull()
        {
            //Arrange
            var createLessonDTO = AddCreateLessonDto();
            
            foreach(var visit in createLessonDTO.LessonVisits)
            {
                visit.StudentMark = 10;
            }

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(createLessonDTO.MentorId)).
                ReturnsAsync(new Mentor { Id = createLessonDTO.MentorId, AccountId = _id });

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(CreateStudentGroup().Id)).ReturnsAsync(CreateStudentGroup());

            _studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(CreateStudentGroup().Id)).
                ReturnsAsync(new List<long?> { visitStudentIdPresenceTrue, visitStudentIdPresenceFalse });

            //Act
            var result = await _lessonService.CreateLessonAsync(createLessonDTO);

            //Assert
            result
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task CreateLesson_ValidDataWithMark_ShouldReturnLessonDto()
        {
            //Arrange
            var createLessonDTO = AddCreateLessonDto();

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(createLessonDTO.MentorId)).
                ReturnsAsync(new Mentor { Id = createLessonDTO.MentorId });

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(CreateStudentGroup().Id)).ReturnsAsync(CreateStudentGroup());

            _studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(CreateStudentGroup().Id)).
                ReturnsAsync(new List<long?> { visitStudentIdPresenceTrue, visitStudentIdPresenceFalse });

            //Act
            var result = await _lessonService.CreateLessonAsync(createLessonDTO);

            //Assert
            result
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task CreateLessonAsync_NonExistingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            #region DATA
            LessonDto createdLesson = AddLessonDto();
            CreateLessonDto createLessonDto = AddCreateLessonDto();

            Mentor mentor = new Mentor() { Id = mentorId };
            Mentor mentorWrong = new Mentor() { Id = mentorWrongId };
            var createLessonDtoWrongMentor = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorWrong.Id,
                StudentGroupId = studentGroupId,
                LessonVisits = CreateVisitDto()
            };
            #endregion

            #region MOCK

            var mentorReposutoryMockWrong = new Mock<IMentorRepository>();
            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);
            mentorReposutoryMockWrong.Setup(x => x.GetMentorByIdAsync(mentorWrong.Id)).ReturnsAsync(default(Mentor));

            #endregion
            //Act 
            var resultWihtWrongMentor = await _lessonService.CreateLessonAsync(createLessonDtoWrongMentor);

            //Assert
            resultWihtWrongMentor.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateLessonAsync_NonExistingStudentGroupId_ShouldReturnNotFound()
        {
            //Arrange
            #region DATA

            var createLessonDtoWrongStudentGroup = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = _wrongId,
                LessonVisits = CreateVisitDto()
            };

            var mentor = new Mentor
            {
                Id = mentorId
            };

            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentorId)).ReturnsAsync(mentor);
            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_wrongId)).ReturnsAsync(default(StudentGroup));

            #endregion
            //Act 
            var resultWihtWrongStudentGroup = await _lessonService.CreateLessonAsync(createLessonDtoWrongStudentGroup);

            //Assert
            resultWihtWrongStudentGroup.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateLessonAsync_ExistingTheme_ShouldReturnLessonDto()
        {
            //Arrange
            #region DATA
            LessonDto createdLesson = AddLessonDto();
            CreateLessonDto createLessonDto = AddCreateLessonDto();

            Mentor mentor = new Mentor() { Id = mentorId };

            var theme = new Theme()
            {
                Id = _id,
                Name = themeName
            };
            #endregion

            #region MOCK
            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);
            _themeRepositoryMock.Setup(x => x.GetThemeByNameAsync(themeName)).ReturnsAsync(theme);
            #endregion

            //Act 
            var result = await _lessonService.CreateLessonAsync(createLessonDto);

            //Assert
            createdLesson.LessonDate
                .Should()
                .Equals(result.Data.LessonDate);
            createdLesson.Id
                .Should()
                .Equals(result.Data.Id);
            createdLesson.LessonVisits.Count
                .Should()
                .Equals(result.Data.LessonVisits.Count);
            createdLesson.MentorId
                .Should()
                .Equals(result.Data.MentorId);
            createdLesson.StudentGroupId
                .Should()
                .Equals(result.Data.StudentGroupId);
            createdLesson.ThemeName
                .Should()
                .Equals(result.Data.ThemeName);
        }

        [Fact]
        public async Task CreateLessonAsync_WrongLessonDate_ShouldReturnValidationError()
        {
            //Arrange
            #region DATA
            LessonDto createdLesson = AddLessonDto();
            CreateLessonDto createLessonDto = AddCreateLessonDto();

            Mentor mentor = new Mentor() { Id = mentorId };
            DateTime lessonDateWrong = DateTime.Now.AddDays(1);

            var createLessonDtoWrongLessonDate = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonDate = lessonDateWrong,
                LessonVisits = CreateVisitDto()
            };
            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);

            #endregion
            //Act 
            var resultWithWrongLessonDate = await _lessonService.CreateLessonAsync(createLessonDtoWrongLessonDate);

            //Assert
            resultWithWrongLessonDate.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateLessonAsync_WrongLessonVisitsWithoutStudent_ShouldReturnValidationError()
        {
            //Arrange
            #region DATA

            Mentor mentor = new Mentor() { Id = mentorId };

            StudentGroup studentGroupWrong = new StudentGroup() { Id = studentGroupWr };

            List<VisitDto> visitDtoWithoutStudent = new List<VisitDto>
            {
                new VisitDto()
                {
                    StudentId = visitStudentIdPresenceTrue,
                    Presence = true
                },
            };

            var createLessonDtoWrongLessonVisitsWithoutStudent = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonVisits = visitDtoWithoutStudent
            };
            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);

            #endregion

            //Act 
            var resultWithWrongLessonVisitStudent = await _lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsWithoutStudent);
            //Assert
            resultWithWrongLessonVisitStudent.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateLessonAsync_WrongLessonVisitsStudent_ShouldReturnNotFound()
        {
            //Arrange
            #region DATA
            Mentor mentor = new Mentor() { Id = mentorId };

            List<VisitDto> visitDtoWrongStudent = new List<VisitDto>
            {
                new VisitDto()
                {
                    StudentId = visitStudentIdPresenceTrue,
                    Presence = true
                },

                new VisitDto()
                {
                    StudentId = studentId,
                    Presence = false
                },
            };

            var createLessonDtoWrongLessonVisitsStudent = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonVisits = visitDtoWrongStudent
            };

            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);

            var studentGroupRepositoryMockWrong = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMockWrong.Setup(x => x.GetGroupStudentsIds(createLessonDtoWrongLessonVisitsStudent.StudentGroupId)).ReturnsAsync(new List<long?> { studentGroupId });

            #endregion
            //Act 
            var resultWithWrongLessonVisit = await _lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsStudent);

            //Assert
            resultWithWrongLessonVisit.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateLessonAsync_EmptyLessonVisit_ShouldReturnValidationError()
        {
            //Arrange
            #region DATA
            Mentor mentor = new Mentor() { Id = mentorId };

            List<VisitDto> visitsDtoEmpty = new List<VisitDto>() { };

            var createLessonDtoEmptyLessonVisit = new CreateLessonDto
            {
                ThemeName = themeName,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                LessonVisits = visitsDtoEmpty
            };
            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);

            #endregion
            //Act 
            var resultWithEmptyLessonVisit = await _lessonService.CreateLessonAsync(createLessonDtoEmptyLessonVisit);
            //Assert
            resultWithEmptyLessonVisit.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateLessonAsync_ValidDataPassed_ShouldReturnSameData()
        {
            //Arrange
            #region DATA
            LessonDto createdLesson = AddLessonDto();
            CreateLessonDto createLessonDto = AddCreateLessonDto();

            Mentor mentor = new Mentor() { Id = mentorId };
            #endregion

            #region MOCK

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);

            #endregion
            //Act 
            var result = await _lessonService.CreateLessonAsync(createLessonDto);

            //Assert
            createdLesson.LessonDate
                .Should()
                .Equals(result.Data.LessonDate);
            createdLesson.Id
                .Should()
                .Equals(result.Data.Id);
            createdLesson.LessonVisits.Count
                .Should()
                .Equals(result.Data.LessonVisits.Count);
            createdLesson.MentorId
                .Should()
                .Equals(result.Data.MentorId);
            createdLesson.StudentGroupId
                .Should()
                .Equals(result.Data.StudentGroupId);
            createdLesson.ThemeName
                .Should()
                .Equals(result.Data.ThemeName);
        }

        [Fact]
        public async Task AssignMentorToLessonAsync_NonExistentMentorId_ShouldReturnNotFound()
        {
            //Arrange
            var nonExistentMentor = new AssignMentorToLessonDto()
            {
                MentorId = 10,
                LessonId = 2
            };

            long mentorId = 10;
            Mentor mentor = new Mentor() { Id = mentorId };

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(nonExistentMentor.MentorId)).ReturnsAsync(mentor);

            //Act
            var nonExistingResult = await _lessonService.AssignMentorToLessonAsync(nonExistentMentor);

            //Assert
            nonExistingResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task AssignMentorToLessonAsync_NonExistentLessonId_ShouldReturnNotFound()
        {
            //Arrange
            var nonExistentLesson = new AssignMentorToLessonDto()
            {
                MentorId = 3,
                LessonId = 20
            };

            long lessonId = 20;
            Lesson lesson = new Lesson { Id = lessonId };

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(nonExistentLesson.LessonId)).ReturnsAsync(lesson);

            //Act
            var nonExistingResult = await _lessonService.AssignMentorToLessonAsync(nonExistentLesson);

            //Assert
            nonExistingResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task AssignMentorToLessonAsync_ValidDataPassed_ShouldReturnExpectedData()
        {
            //Arrange
            long lessonId = 2;

            var expectedData = new AssignMentorToLessonDto()
            {
                MentorId = mentorId,
                LessonId = 2
            };

            var expectedLesson = new Lesson()
            {
                Id = lessonId,
                MentorId = mentorId,
                StudentGroupId = studentGroupId,
                ThemeId = 1,
                LessonDate = lessonDate
            };

            Mentor mentor = new Mentor() { Id = mentorId };


            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(expectedData.MentorId)).ReturnsAsync(mentor);

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(expectedData.LessonId)).ReturnsAsync(expectedLesson);

            //Act
            var expectedResult = await _lessonService.AssignMentorToLessonAsync(expectedData);

            //Assert
            expectedResult.Data
                .Should()
                .NotBeNull();
            expectedResult.Data
                .Should()
                .BeEquivalentTo(expectedLesson);
        }

        [Fact]
        public async Task UpdateLessonAsync()
        {
            //Arrange
            Theme theme = new Theme
            {
                Name = "ExampleName",
                Id = 5
            };

            Mentor mentor = new Mentor
            {
                Id = 2
            };

            StudentGroup studentGroup = new StudentGroup
            {
                Id = 3
            };

            List<VisitDto> visitsDtoList = new List<VisitDto>()
            {
                new VisitDto()
                {
                    StudentId = 11,
                    Presence = true,
                    StudentMark = 99,
                    Comment = "comm"
                },

                new VisitDto()
                {
                    StudentId = 14,
                    Presence = false,
                    StudentMark = 99,
                    Comment = "comm"
                }
            };

            var foundLesson = new Lesson()
            {
                Id = 7,
                MentorId = 2,
                StudentGroupId = 3,
                ThemeId = 5,
                Mentor = mentor,
                StudentGroup = studentGroup,
                Theme = theme,
                Visits = new List<Visit>
                {
                    new Visit()
                    {
                        Id = 4,
                        StudentId = 11,
                        Presence = true,
                        MarkId = 5,
                        Mark = new Mark()
                        {
                            Id = 5,
                            Value = 99,
                            Comment = "comm"
                        }
                    },

                    new Visit()
                    {
                        Id = 3,
                        StudentId = 14,
                        Presence = false,
                        MarkId = 6,
                        Mark = new Mark()
                        {
                            Id = 6,
                            Value = 99,
                            Comment = "comm"
                        }
                    }
                }
            };

            var updateLessonDto = new UpdateLessonDto
            {
                ThemeName = "new theme",
                LessonDate = DateTime.Parse("2020-11-18T15:30:00.384Z"),
                LessonVisits = visitsDtoList
            };

            var updateLessonDtoWithWrongDate = new UpdateLessonDto
            {
                ThemeName = null,
                LessonDate = DateTime.Now.AddDays(1),
                LessonVisits = null
            };

            var foundLessonDto = new LessonDto()
            {
                Id = 7,
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-19T15:00:00.384Z"),
                LessonVisits = null
            };

            var updatedLesson = new LessonDto()
            {
                Id = 7,
                ThemeName = "new theme",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:30:00.384Z"),
                LessonVisits = visitsDtoList
            };

            var themerepositoryMock = new Mock<IThemeRepository>();
            themerepositoryMock.Setup(x => x.GetThemeByNameAsync(It.IsAny<string>())).ReturnsAsync(new Theme { Name = "new theme" });

            var studentGroupRepository = new Mock<IStudentGroupRepository>();
            studentGroupRepository.Setup(x => x.GetGroupStudentsIds(It.IsAny<long>())).ReturnsAsync(new List<long?> { 11, 14 });

            var visitRepositoryMock = new Mock<IVisitRepository>();
            visitRepositoryMock.Setup(x => x.Add(It.IsAny<Visit>()))
               .Callback<Visit>(x =>
               {
                   x.Student = new Student();
                   x.StudentId = 1;
                   x.MarkId = 5;
                   x.LessonId = 1;
                   x.Presence = true;
               });
            visitRepositoryMock.Setup(x => x.DeleteWhereLessonIdAsync(It.IsAny<long>()));

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepository.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themerepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LessonRepository.GetByIdAsync(7))
                .ReturnsAsync(foundLesson);

            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            //Act
            var result = (await _lessonService.UpdateLessonAsync(7, updateLessonDto)).Data;
            var resultWithWrongDate = await _lessonService.UpdateLessonAsync(7, updateLessonDtoWithWrongDate);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, resultWithWrongDate.Error.Code);

            Assert.NotNull(result);
            Assert.Equal(updatedLesson.Id, result.Id);
            Assert.Equal(updatedLesson.LessonDate, result.LessonDate);
            Assert.Equal(updatedLesson.LessonVisits, result.LessonVisits);
            Assert.Equal(updatedLesson.MentorId, result.MentorId);
            Assert.Equal(updatedLesson.StudentGroupId, result.StudentGroupId);
            Assert.Equal(updatedLesson.ThemeName, result.ThemeName);
        }

        [Fact]
        public async Task UpdateLessonAsync_ServerException_ShouldReturnNull()
        {
            //Arrange
            var anyUpdateLesson = new UpdateLessonDto { ThemeName = "new theme name" };

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                        .Throws(new InvalidOperationException());

            //Act
            var result = await _lessonService.UpdateLessonAsync(_id, anyUpdateLesson);

            //Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task UpdateLessonAsync_LessonNotFound_ShouldReturnNull()
        {
            //Arrange
            var anyUpdateLesson = new UpdateLessonDto { ThemeName = "new theme name" };

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(_id)).ReturnsAsync(() => null);

            //Act
            var result = await _lessonService.UpdateLessonAsync(_id, anyUpdateLesson);

            //Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task UpdateLessonAsync_ThemeNotFound_ShouldReturnLessonDto()
        {
            //Arrange
            var anyUpdateLesson = new UpdateLessonDto { ThemeName = "new theme name" };
            var lesson = _mapper.Map<Lesson>(AddLessonDto());
            var updatedLessonDto = _mapper.Map<LessonDto>(lesson);
            updatedLessonDto.ThemeName = anyUpdateLesson.ThemeName;

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(_id)).ReturnsAsync(lesson);
            _themeRepositoryMock.Setup(x => x.GetThemeByNameAsync(anyUpdateLesson.ThemeName)).ReturnsAsync(() => null);

            //Act
            var result = await _lessonService.UpdateLessonAsync(_id, anyUpdateLesson);

            //Assert
            result.Data
                .Should()
                .BeEquivalentTo(updatedLessonDto);
        }

        [Fact]
        public async Task UpdateLessonAsync_StudentIDNotIncludeInGroup_ShouldReturnNotFoundError()
        {
            //Arrange
            var anyUpdateLesson = new UpdateLessonDto
            {
                ThemeName = "new theme name",
                LessonVisits = CreateVisitDto()
            };
            var foundLesson = _mapper.Map<Lesson>(AddLessonDto());
            var studentNotIncludeGroupIds = new List<long?>();

            for (long i = 0; i < 10; i++)
            {
                if (i != foundLesson.StudentGroupId)
                {
                    studentNotIncludeGroupIds.Add(i);
                }
            }

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(_id)).ReturnsAsync(foundLesson);
            _studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds((long)foundLesson.StudentGroupId)).ReturnsAsync(studentNotIncludeGroupIds);

            //Act
            var result = await _lessonService.UpdateLessonAsync(_id, anyUpdateLesson);

            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task IsLessonDoneAsync_ValidDataPassed_ShouldReturnTrue()
        {
            //Arrange
            long lessonId = 1;
            Lesson lesson = new Lesson
            {
                Id = lessonId,
                StudentGroup = CreateStudentGroup(),
                Mentor = new Mentor { Id = 1 },
                Visits = new List<Visit> {
                    new Visit{ Id = 1, Presence = true, LessonId = lessonId },
                    new Visit{ Id = 1, Presence = false, LessonId = lessonId }
                }
            };

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);
            //Act
            var result = await _lessonService.IsLessonDoneAsync(lessonId);

            //Assert
            result.Data
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task IsLessonDoneAsync_WithoutVisits_ShouldThrowException()
        {
            //Arrange
            long lessonId = 1;
            Lesson lesson = new Lesson
            {
                Id = lessonId,
                StudentGroup = CreateStudentGroup(),
                Mentor = new Mentor { Id = 1 }
            };

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);
            //Act
            Func<Task> isDoneMethod = async () => await _lessonService.IsLessonDoneAsync(lessonId);

            //Assert
            await isDoneMethod
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task IsLessonDoneAsync_NotFoundLesson_ShouldThrowException()
        {
            //Arrange
            long lessonId = 1;

            //Act
            Func<Task> isDoneMethod = async () => await _lessonService.IsLessonDoneAsync(lessonId);

            //Assert
            await isDoneMethod
                .Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetLessonsByDateAsync_StartDateBiggerFinishDate_ShouldReturnValidationError()
        {
            //Act
            var wrongDatesResult = await _lessonService
                .GetLessonsByDate(_finishDate, _startDate);

            //Assert
            wrongDatesResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        public static readonly object[][] CorrectData =
        {
            new object[] { null, DateTime.Parse(_stringFinishDate)},
            new object[] { DateTime.Parse(_stringStartDate), null},
            new object[] { null, null}
        };
        [Theory, MemberData(nameof(CorrectData))]
        public async Task GetLessonsByDateAsync_CorrectDate_ShouldReturnListLessonDto(DateTime? startDate, DateTime? finishDate)
        {
            //Arrange
            List<Lesson> lessonsList = GetLessonsList();
            var lastLesson = lessonsList.Last();

            if (startDate == null && finishDate == null)
            {
                _lessonRepositoryMock.Setup(x => x.GetLastLessonAsync()).ReturnsAsync(lastLesson);
                _lessonRepositoryMock.Setup(x => x.GetLessonsByDateAsync(lastLesson.LessonDate.AddDays(-30), lastLesson.LessonDate)).ReturnsAsync(lessonsList);
            }

            _lessonRepositoryMock.Setup(x => x.GetLessonsByDateAsync(startDate, finishDate)).ReturnsAsync(lessonsList);

            //Act
            var validResult = await _lessonService.GetLessonsByDate(startDate, finishDate);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<LessonDto>>(lessonsList));
        }

        [Fact]
        public async Task GetLessonsByDateAsync_ValidData_ShouldReturnListLessonDto()
        {
            //Arrange
            var lessonsList = GetLessonsList();

            _lessonRepositoryMock.Setup(x => x.GetLessonsByDateAsync(_startDate, _finishDate)).ReturnsAsync(lessonsList);

            //Act
            var validResult = await _lessonService.GetLessonsByDate(_startDate, _finishDate);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<LessonDto>>(lessonsList));
        }

        [Fact]
        public async Task GetLessonByIdAsync_NotExistId_ShouldReturnNotFoundError()
        {
            //Act
            var validResult = await _lessonService.GetLessonByIdAsync(_wrongId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetLessonByIdAsync_ValidData_ShouldReturnLessonDto()
        {
            //Arrange
            var validLesson = _mapper.Map<Lesson>(AddLessonDto());

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(_id)).ReturnsAsync(validLesson);

            //Act
            var validResult = await _lessonService.GetLessonByIdAsync(_id);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<LessonDto>(validLesson));
        }

        [Theory]
        [InlineData(_wrongId)]
        [InlineData(defaultId)]
        public async Task GetAllLessonsForStudentGroup_WrongId_ShouldReturnValidationError(long wrongId)
        {
            //Arrange

            //Act
            var validResult = await _lessonService.GetAllLessonsForStudentGroup(wrongId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetAllLessonsForStudentGroup_ValidData_ShouldReturnListLessonDto()
        {
            //Arrange
            var lessonsList = GetLessonsList();

            _lessonRepositoryMock.Setup(x => x.GetAllLessonsForStudentGroupAsync(studentGroupId)).ReturnsAsync(lessonsList);

            //Act
            var validResult = await _lessonService.GetAllLessonsForStudentGroup(studentGroupId);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<IList<LessonDto>>(lessonsList));
        }

        [Fact]
        public async Task GetAllLessonsForMentor_ValidData_ShouldReturnListLessonDto()
        {
            //Arrange
            var lessonsList = GetLessonsList();

            Mentor mentor = new Mentor()
            {
                Id = mentorId,
                Lesson = lessonsList
            };

            _lessonRepositoryMock.Setup(x => x.GetAllLessonsForMentorAsync(mentorId)).ReturnsAsync(lessonsList);

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(mentorId)).ReturnsAsync(mentor);

            //Act
            var validResult = await _lessonService.GetAllLessonsForMentor(mentorId);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<IList<LessonDto>>(lessonsList));
        }

        [Theory]
        [InlineData(_wrongId)]
        [InlineData(defaultId)]
        public async Task GetAllLessonsForMentor_NotExistId_ShouldReturnValidationError(long wrongId)
        {
            //Act
            var validResult = await _lessonService.GetAllLessonsForMentor(wrongId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetAllLessonsForMentor_NullMentor_ShouldReturnValidationError()
        {
            //Arrange
            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentorId)).ReturnsAsync(() => null);

            //Act
            var validResult = await _lessonService.GetAllLessonsForMentor(mentorId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetLessonsForMentorAsync_ValidData_ShouldReturnListLessonDto()
        {
            //Arrange
            var filter = new FilterLessonsRequestDto() { StudentGroupId = studentGroupId };

            var mentor = new Mentor() { Id = mentorId };

            List<Lesson> lessons = GetLessonsList();

            _mentorRepositoryMock.Setup(x => x.GetMentorByAccountIdAsync(_currentUserServiceMock.Object.AccountId)).ReturnsAsync(mentor);
            _lessonRepositoryMock.Setup(x => x.GetLessonsForMentorAsync(studentGroupId, filter.StartDate, filter.FinishDate, mentor.Id)).ReturnsAsync(lessons);

            //Act
            var result = await _lessonService.GetLessonsForMentorAsync(filter);

            //Assert
            result
                .Should()
                .BeEquivalentTo(_mapper.Map<List<LessonDto>>(lessons));
        }

        [Fact]
        public async Task GetLessonsForMentorAsync_NoFilter_ShouldReturnEmtyListLessonDto()
        {
            //Arrange
            var mentor = new Mentor() { Id = mentorId };

            _mentorRepositoryMock.Setup(x => x.GetMentorByAccountIdAsync(_currentUserServiceMock.Object.AccountId)).ReturnsAsync(mentor);

            //Act
            var result = await _lessonService.GetLessonsForMentorAsync(null);

            //Assert
            result
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task GetLessonsForStudentAsync_ValidData_ShouldReturnListLessonDto()
        {
            //Arrange
            var filter = new FilterLessonsRequestDto() { StudentGroupId = studentGroupId };
            var studentGroupsIds = new List<long?>() { studentGroupId };
            var _currentUserId = _currentUserServiceMock.Object.EntityId;
            List<Lesson> lessons = GetLessonsList();

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(_currentUserId)).ReturnsAsync(studentGroupsIds);
            _lessonRepositoryMock.Setup(x => x.GetLessonsForStudentAsync(filter.StudentGroupId, filter.StartDate, filter.FinishDate, _currentUserId)).ReturnsAsync(lessons);

            //Act
            var result = await _lessonService.GetLessonsForStudentAsync(filter);

            //Assert
            result.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<LessonDto>>(lessons));
        }

        [Fact]
        public async Task GetLessonsForStudentAsync_StudentNotExistInGroup_ShouldReturnListLessonDto()
        {
            //Arrange
            var filter = new FilterLessonsRequestDto() { StudentGroupId = studentGroupId };
            var studentGroupsIds = new List<long?>();
            var _currentUserId = _currentUserServiceMock.Object.EntityId;

            for (long i = 0; i < 10; i++)
            {
                if (i != studentGroupId)
                {
                    studentGroupsIds.Add(i);
                }
            }

            _studentGroupRepositoryMock.Setup(x => x.GetStudentGroupsIdsByStudentId(_currentUserId)).ReturnsAsync(studentGroupsIds);

            //Act
            var result = await _lessonService.GetLessonsForStudentAsync(filter);

            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.AccessDenied); 
        }

        [Fact]
        public async Task GetStudentLessonsAsync_ValidData_ShouldReturnListStudentLessonDto()
        {
            //Arrange
            var currentUserId = _currentUserServiceMock.Object.EntityId;
            List<StudentLessonDto> studentLessonModels = new List<StudentLessonDto>()
            {
                new StudentLessonDto { StudentGroupId = studentGroupId, Presence = false, Id = _id },
                new StudentLessonDto { StudentGroupId = studentGroupId, Presence = true, Id = _id }
            };

            _lessonRepositoryMock.Setup(x => x.GetStudentInfoAsync(currentUserId)).ReturnsAsync(studentLessonModels);

            //Act
            var result = await _lessonService.GetStudentLessonsAsync(currentUserId);

            //Assert
            result.Data
                .Should()
                .BeEquivalentTo(studentLessonModels);
        }

        [Fact]
        public async Task GetStudentLessonsAsync_NotCurrentStudentId_ShouldReturnUnauthorizedError()
        {
            //Arrange
            var notCurrentUserId = _currentUserServiceMock.Object.EntityId + 1;

            //Act
            var result = await _lessonService.GetStudentLessonsAsync(notCurrentUserId);

            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Unauthorized);
        }

        [Fact]
        public async Task GetStudentLessonsAsync_NoStudentLessonModels_ShouldReturnNotFoundError()
        {
            //Arrange
            var currentUserId = _currentUserServiceMock.Object.EntityId;
            List<StudentLessonDto> studentLessonModels = null;

            _lessonRepositoryMock.Setup(x => x.GetStudentInfoAsync(currentUserId)).ReturnsAsync(studentLessonModels);

            //Act
            var result = await _lessonService.GetStudentLessonsAsync(currentUserId);

            //Assert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }
    }
}
