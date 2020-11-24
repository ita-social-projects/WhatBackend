using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

            var lesson = new Lesson()
            {
                MentorId = 2,
                StudentGroupId = 3,
                ThemeId = 5,
                Mentor = mentor,
                StudentGroup = studentGroup,
                Theme = theme,
                Visits = { }
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

            //var visitRepositoryMock = new Mock<IVisitRepository>();
            //visitRepositoryMock.Setup(x => x.Add(It.IsAny<Visit>()))
            //    .Callback<Visit>(x =>
            //    {
                    
            //    });

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);
            //_unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);

            var lessonService = new LessonService(
                _unitOfWorkMock.Object,
                _mapper
                );

            //Act
            var succesResult = await lessonService.CreateLessonAsync(createLessonDto);

            //Assert
            Assert.Equal(createdLesson.Id, succesResult.Id);
            Assert.Equal(createdLesson.LessonDate, succesResult.LessonDate);
            Assert.Equal(createdLesson.LessonVisits, succesResult.LessonVisits);
            Assert.Equal(createdLesson.MentorId, succesResult.MentorId);
            Assert.Equal(createdLesson.StudentGroupId, succesResult.StudentGroupId);
            Assert.Equal(createdLesson.ThemeName, succesResult.ThemeName);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {

            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
