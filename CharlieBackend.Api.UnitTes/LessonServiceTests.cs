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

namespace CharlieBackend.Api.UnitTest
{
    public class LessonServiceTests : TestBase
    {
        private readonly IMapper _mapper;

        public LessonServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CeateLessonAsync()
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
            List<Visit> visits = new List<Visit>() { };

            var createdLesson = new LessonDto()
            {
                Id = 7,
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z"),
                LessonVisits = visitsDto
            };

            var createLessonDto = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z"),
                LessonVisits = visitsDto
            };

            var lessonRepositoryMock = new Mock<ILessonRepository>();
            lessonRepositoryMock.Setup(x => x.Add(It.IsAny<Lesson>()))
                .Callback<Lesson>(x => {
                    x.Id = 7;
                    x.LessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z");
                    x.MentorId = 2;
                    x.StudentGroupId = 3;
                    x.ThemeId = 5;
                    x.Mentor = mentor;
                    x.StudentGroup = studentGroup;
                    x.Theme = theme;
                    x.Visits = visits;
                });

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.Add(It.IsAny<Theme>()))
                .Callback<Theme>(x =>
                {
                    x.Id = 5;
                    x.Name = "ExampleName";
                });

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);

            var lessonService = new LessonService(
                _unitOfWorkMock.Object,
                _mapper
                );

            //Act 
            var result = (await lessonService.CreateLessonAsync(createLessonDto)).Data;

            //Assert
            Assert.NotNull(result);

            Assert.Equal(createdLesson.Id, result.Id);
            Assert.Equal(createdLesson.LessonDate, result.LessonDate);
            Assert.Equal(createdLesson.LessonVisits.Count, result.LessonVisits.Count);

            for (int i = 0; i < result.LessonVisits?.Count; i++)
            {
                Assert.Equal(createdLesson.LessonVisits[i]?.Comment, result.LessonVisits[i]?.Comment);
                Assert.Equal(createdLesson.LessonVisits[i]?.Presence, result.LessonVisits[i]?.Presence);
                Assert.Equal(createdLesson.LessonVisits[i]?.StudentId, result.LessonVisits[i]?.StudentId);
                Assert.Equal(createdLesson.LessonVisits[i]?.StudentMark, result.LessonVisits[i]?.StudentMark);
            }

            Assert.Equal(createdLesson.MentorId, result.MentorId);
            Assert.Equal(createdLesson.StudentGroupId, result.StudentGroupId);
            Assert.Equal(createdLesson.ThemeName, result.ThemeName);
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
                ThemeName = null,
                LessonDate = DateTime.Parse("2020-11-18T15:30:00.384Z"),
                LessonVisits = null
            };

            var foundLessonDto = new LessonDto()
            {
                Id = 7,
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z"),
                LessonVisits = null
            };

            var updatedLesson = new LessonDto()
            {
                Id = 7,
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:30:00.384Z"),
                LessonVisits = visitsDto
            };

            _unitOfWorkMock.Setup(x => x.LessonRepository.GetByIdAsync(7))
                .ReturnsAsync(foundLesson);

            var lessonService = new LessonService(
                _unitOfWorkMock.Object,
                _mapper
                );

            //Act
            var result = (await lessonService.UpdateLessonAsync(7, updateLessonDto)).Data;

            //Assert
            Assert.NotNull(result);

            Assert.Equal(updatedLesson.Id, result.Id);
            Assert.Equal(updatedLesson.LessonDate, result.LessonDate);
            Assert.Equal(updatedLesson.LessonVisits, result.LessonVisits);
            Assert.Equal(updatedLesson.MentorId, result.MentorId);
            Assert.Equal(updatedLesson.StudentGroupId, result.StudentGroupId);
            Assert.Equal(updatedLesson.ThemeName, result.ThemeName);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock;
        }
    }
}
