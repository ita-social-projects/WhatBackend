using Moq;
using Xunit;
using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Business.Services;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.Models.ResultModel;
using FluentAssertions;
using CharlieBackend.Business.Exceptions;
using CharlieBackend.Data.Exceptions;

namespace CharlieBackend.Api.UnitTest
{
    public class LessonServiceTests : TestBase
    {
        private readonly IMapper _mapper;

        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private readonly Mock<IThemeRepository> _themeRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<IVisitRepository> _visitRepositoryMock;

        private static long visitStudentIdPresenceTrue = 11;
        private static long visitStudentIdPresenceFalse = 14;
        private static long mentorId = 2;
        private static long studentId = 1;
        private static long mentorWrongId = 31;
        private static long studentGroupId = 3;
        private static long studentGroupWr = 100;
        private static string themeName = "ExampleName";
        private static DateTime lessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z"); 

        public LessonServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _themeRepositoryMock = new Mock<IThemeRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _visitRepositoryMock = new Mock<IVisitRepository>();

            MockEntities();
        }

        private static List<VisitDto> CreateVisitDto()
        {
            return  new List<VisitDto>
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


            _currentUserServiceMock = GetCurrentUserAsExistingStudent();
        }

        [Fact]
        public async Task CreateLesson_ValidDataPassed_ShouldBeNotNull()
        {
            //Arrange
            var createLessonDTO = AddCreateLessonDto();

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(createLessonDTO.MentorId)).
                ReturnsAsync(new Mentor { Id = createLessonDTO.MentorId });

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(CreateStudentGroup().Id)).ReturnsAsync(CreateStudentGroup());
            
            _studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(CreateStudentGroup().Id)).
                ReturnsAsync(new List<long?> { visitStudentIdPresenceTrue, visitStudentIdPresenceFalse });

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object, 
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var result = await lessonService.CreateLessonAsync(createLessonDTO);

            //Assert
            result.Should().NotBeNull();
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

            var lessonService = new LessonService(
               unitOfWork: _unitOfWorkMock.Object,
               mapper: _mapper,
               currentUserService: _currentUserServiceMock.Object);

            #endregion
            //Act 
            var resultWihtWrongMentor = await lessonService.CreateLessonAsync(createLessonDtoWrongMentor);

            //Assert
            resultWihtWrongMentor.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            #endregion
            //Act 
            var resultWithWrongLessonDate = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonDate);

            //Assert
            resultWithWrongLessonDate.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);
            #endregion

            //Act 
            var resultWithWrongLessonVisitStudent = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsWithoutStudent);
            //Assert
            resultWithWrongLessonVisitStudent.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            #endregion
            //Act 
            var resultWithWrongLessonVisit = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsStudent);

            //Assert
            resultWithWrongLessonVisit.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            #endregion
            //Act 
            var resultWithEmptyLessonVisit = await lessonService.CreateLessonAsync(createLessonDtoEmptyLessonVisit);
            //Assert
            resultWithEmptyLessonVisit.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            #endregion
            //Act 
            var result = await lessonService.CreateLessonAsync(createLessonDto);

            //Assert
            createdLesson.LessonDate.Should().Equals(result.Data.LessonDate);
            createdLesson.Id.Should().Equals(result.Data.Id);
            createdLesson.LessonVisits.Count.Should().Equals(result.Data.LessonVisits.Count);
            createdLesson.MentorId.Should().Equals(result.Data.MentorId);
            createdLesson.StudentGroupId.Should().Equals(result.Data.StudentGroupId);
            createdLesson.ThemeName.Should().Equals(result.Data.ThemeName);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var nonExistingResult = await lessonService.AssignMentorToLessonAsync(nonExistentMentor);

            //Assert
            nonExistingResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var nonExistingResult = await lessonService.AssignMentorToLessonAsync(nonExistentLesson);

            //Assert
            nonExistingResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
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

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var expectedResult = await lessonService.AssignMentorToLessonAsync(expectedData);

            //Assert
            expectedResult.Data.Should().NotBeNull();
            expectedResult.Data.Should().BeEquivalentTo(expectedLesson);
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

            List<VisitDto> visitsDto = new List<VisitDto>() { };

            var foundLesson = new Lesson()
            {
                Id = 7,
                MentorId = 2,
                StudentGroupId = 3,
                ThemeId = 5,
                Mentor = mentor,
                StudentGroup = studentGroup,
                Theme = theme,
                Visits = { }
            };

            var updateLessonDto = new UpdateLessonDto
            {
                ThemeName = "new theme",
                LessonDate = DateTime.Parse("2020-11-18T15:30:00.384Z"),
                LessonVisits = new List<VisitDto> 
                {
                    new VisitDto()
                    {
                        StudentId = 11,
                        Presence = true
                    },

                    new VisitDto()
                    {
                        StudentId = 14,
                        Presence = false
                    }
                }
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
                LessonVisits = visitsDto
            };

            var themerepositoryMock = new Mock<IThemeRepository>();
            themerepositoryMock.Setup(x => x.GetThemeByNameAsync(It.IsAny<string>())).ReturnsAsync( new Theme { Name = "new theme"});

            var studentGroupRepository = new Mock<IStudentGroupRepository>();
            studentGroupRepository.Setup(x => x.GetGroupStudentsIds(It.IsAny<long>())).ReturnsAsync(new List<long?> { 11, 14 });

            var visitRepositoryMock = new Mock<IVisitRepository>();
            visitRepositoryMock.Setup(x => x.Add(It.IsAny<Visit>()))
               .Callback<Visit>(x => {
                   x.Student = new Student();
                   x.StudentId = 1;
                   x.StudentMark = 5;
                   x.LessonId = 1;
                   x.Presence = true;
                   x.Comment = "some comment";
               });
            visitRepositoryMock.Setup(x => x.DeleteWhereLessonIdAsync(It.IsAny<long>()));

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepository.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themerepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LessonRepository.GetByIdAsync(7))
                .ReturnsAsync(foundLesson);

            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var result = (await lessonService.UpdateLessonAsync(7, updateLessonDto)).Data;
            var resultWithWrongDate = await lessonService.UpdateLessonAsync(7, updateLessonDtoWithWrongDate);


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
        public async Task IsLessonDoneAsync_ValidDataPassed_ShouldReturnTrue()
        {
            //Arrange
            long lessonId = 1;
            Lesson lesson = new Lesson 
            {
                Id = lessonId,
                StudentGroup = CreateStudentGroup(),
                Mentor = new Mentor { Id = 1},
                Visits = new List<Visit> {
                    new Visit{ Id = 1, Presence = true, LessonId = lessonId },
                    new Visit{ Id = 1, Presence = false, LessonId = lessonId }
                }
            };

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);
            //Act
            var result = await lessonService.IsLessonDoneAsync(lessonId);

            //Assert
            result.Should().BeTrue();
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
                Mentor = new Mentor { Id = 1 },
                Visits = new List<Visit> {
                    new Visit{ Id = 1, Presence = false, LessonId = lessonId },
                    new Visit{ Id = 1, Presence = false, LessonId = lessonId }
                }
            };

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            _lessonRepositoryMock.Setup(x => x.GetByIdAsync(lessonId)).ReturnsAsync(lesson);
            //Act
            Func<Task> isDoneMethod = async () => { await lessonService.IsLessonDoneAsync(lessonId); };

            //Assert
            await isDoneMethod.Should().ThrowAsync<LessonNotDoneException>();
        }

        [Fact]
        public async Task IsLessonDoneAsync_NotFoundLesson_ShouldThrowException()
        {
            //Arrange
            long lessonId = 1;

            var lessonService = new LessonService(
                unitOfWork: _unitOfWorkMock.Object,
                mapper: _mapper,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            Func<Task> isDoneMethod = async () => await lessonService.IsLessonDoneAsync(lessonId);

            //Assert
            await isDoneMethod.Should().ThrowAsync<NotFoundException>();
        }
    }
}
