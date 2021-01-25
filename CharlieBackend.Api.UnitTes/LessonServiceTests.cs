﻿using Moq;
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
        public async Task CreateLesson_ReturnLessonDTo_NotEmpty()
        {
            //Arrange
            var createLessonDTO = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z"),
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

            List<StudentOfStudentGroup> studentOfStudentGroup = new List<StudentOfStudentGroup>
            {
                new StudentOfStudentGroup(){ Id = 11 , StudentGroupId = 3 },
                new StudentOfStudentGroup(){ Id = 14, StudentGroupId = 3 }

            };
            StudentGroup studentGroup = new StudentGroup() { Id = 3, StudentsOfStudentGroups = studentOfStudentGroup };

            var lessonRepositoryMock = new Mock<ILessonRepository>();
            lessonRepositoryMock.Setup(x => x.Add(It.IsAny<Lesson>()))
                .Callback<Lesson>(x => {
                    x.Id = 7;
                    x.LessonDate = createLessonDTO.LessonDate;
                    x.MentorId = 2;
                    x.StudentGroupId = 3;
                    x.ThemeId = 5;
                    x.Mentor = new Mentor { Id = createLessonDTO.MentorId };
                    x.StudentGroup = studentGroup;
                    x.Theme = new Theme { Name = createLessonDTO.ThemeName };
                    x.Visits = new List<Visit>
                    {
                        new Visit()
                        {
                            StudentId = 11,
                            Presence = true
                        },

                        new Visit()
                        {
                            StudentId = 14,
                            Presence = false
                        }
                    };
                });

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.Add(It.IsAny<Theme>()))
                .Callback<Theme>(x =>
                {
                    x.Id = 5;
                    x.Name = "ExampleName";
                });

            var mentorReposutoryMock = new Mock<IMentorRepository>();
            mentorReposutoryMock.Setup(x => x.GetMentorByIdAsync(createLessonDTO.MentorId)).ReturnsAsync(new Mentor { Id = createLessonDTO.MentorId });

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(studentGroup.Id)).ReturnsAsync(studentGroup);
            studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(studentGroup.Id)).ReturnsAsync(new List<long?> { 11, 14 });

            var visitRepositoryMock = new Mock<IVisitRepository>();


            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorReposutoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);
            var lessonService = new LessonService(_unitOfWorkMock.Object, _mapper);

            //Act
            var result = await lessonService.CreateLessonAsync(createLessonDTO);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data.LessonVisits);
        }

        [Fact]
        public async Task CreateLessonAsync()
        {
            //Arrange
            #region DATA
            List<StudentOfStudentGroup> studentOfStudentGroup = new List<StudentOfStudentGroup>
            {
                new StudentOfStudentGroup { StudentGroupId =3 , StudentId = 11 },
                new StudentOfStudentGroup { StudentGroupId =3 , StudentId = 14 }
            };

            Theme theme = new Theme
            {
                Name = "ExampleName",
                Id = 5
            };

            Mentor mentor = new Mentor() { Id = 2 };
            Mentor mentorWrong = new Mentor() { Id = 31 };

            StudentGroup studentGroup = new StudentGroup() { Id = 3, StudentsOfStudentGroups = studentOfStudentGroup };
            StudentGroup studentGroupWrong = new StudentGroup() { Id = 100 };

            DateTime lessonDate = DateTime.Parse("2020-11-18T15:00:00.384Z");
            DateTime lessonDateWrong = DateTime.Now.AddDays(1);

            List<VisitDto> visitDto = new List<VisitDto>
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
            };

            List<VisitDto> visitDtoWrongStudent = new List<VisitDto>
            {
                new VisitDto()
                {
                    StudentId = 11,
                    Presence = true
                },

                new VisitDto()
                {
                    StudentId = 1,
                    Presence = false
                },
            };
            List<VisitDto> visitDtoWithoutStudent = new List<VisitDto>
            {
                new VisitDto()
                {
                    StudentId = 11,
                    Presence = true
                },
            };
            List<VisitDto> visitsDtoEmpty = new List<VisitDto>() { };

            var createdLesson = new LessonDto()
            {
                Id = 7,
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitDto
            };
            var createLessonDto = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitDto
            };
            var createLessonDtoWrongStudentGroup = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = studentGroupWrong.Id,
                LessonDate = lessonDate,
                LessonVisits = visitDto
            };
            var createLessonDtoWrongMentor = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = mentorWrong.Id,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitDto
            };
            var createLessonDtoWrongLessonVisitsStudent = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitDtoWrongStudent
            };
            var createLessonDtoWrongLessonVisitsWithoutStudent = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitDtoWithoutStudent
            };
            var createLessonDtoEmptyLessonVisit = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDate,
                LessonVisits = visitsDtoEmpty
            };
            var createLessonDtoWrongLessonDate = new CreateLessonDto
            {
                ThemeName = "ExampleName",
                MentorId = 2,
                StudentGroupId = 3,
                LessonDate = lessonDateWrong,
                LessonVisits = visitDto
            };
            #endregion

            #region MOCK
            var lessonRepositoryMock = new Mock<ILessonRepository>();
            lessonRepositoryMock.Setup(x => x.Add(It.IsAny<Lesson>()))
                .Callback<Lesson>(x => {
                    x.Id = 7;
                    x.LessonDate = lessonDate;
                    x.MentorId = 2;
                    x.StudentGroupId = 3;
                    x.ThemeId = 5;
                    x.Mentor = mentor;
                    x.StudentGroup = studentGroup;
                    x.Theme = theme;
                    x.Visits = new List<Visit>
                    {
                        new Visit()
                        {
                            StudentId = 11,
                            Presence = true
                        },

                        new Visit()
                        {
                            StudentId = 14,
                            Presence = false
                        }
                    };
                });

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.Add(It.IsAny<Theme>()))
                .Callback<Theme>(x =>
                {
                    x.Id = 5;
                    x.Name = "ExampleName";
                });


            var mentorReposutoryMock = new Mock<IMentorRepository>();
            var mentorReposutoryMockWrong = new Mock<IMentorRepository>();
            mentorReposutoryMock.Setup(x => x.GetMentorByIdAsync(mentor.Id)).ReturnsAsync(mentor);
            mentorReposutoryMockWrong.Setup(x => x.GetMentorByIdAsync(mentorWrong.Id)).ReturnsAsync(default(Mentor));

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(studentGroup.Id)).ReturnsAsync(studentGroup);
            studentGroupRepositoryMock.Setup(x => x.GetGroupStudentsIds(studentGroup.Id)).ReturnsAsync(new List<long?> { 11, 14 });

            var studentGroupRepositoryMockWrong = new Mock<IStudentGroupRepository>();
            studentGroupRepositoryMockWrong.Setup(x => x.GetByIdAsync(studentGroupWrong.Id)).ReturnsAsync(default(StudentGroup));
            studentGroupRepositoryMockWrong.Setup(x => x.GetGroupStudentsIds(createLessonDtoWrongLessonVisitsStudent.StudentGroupId)).ReturnsAsync(new List<long?> { 11 });

            var visitRepositoryMock = new Mock<IVisitRepository>();

            _unitOfWorkMock.Setup(x => x.LessonRepository).Returns(lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.ThemeRepository).Returns(themeRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorReposutoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.VisitRepository).Returns(visitRepositoryMock.Object);
            var lessonService = new LessonService(
                _unitOfWorkMock.Object,
                _mapper
                );


            #endregion
            //Act 
            var result = await lessonService.CreateLessonAsync(createLessonDto);
            var resultWihtWrongMentor = await lessonService.CreateLessonAsync(createLessonDtoWrongMentor);
            var resultWithWrongLessonVisit = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsStudent);
            var resultWithWrongLessonVisitStudent = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonVisitsWithoutStudent);
            var resultWithEmptyLessonVisit = await lessonService.CreateLessonAsync(createLessonDtoEmptyLessonVisit);
            var resultWithWrongLessonDate = await lessonService.CreateLessonAsync(createLessonDtoWrongLessonDate);

            //Assert
            Assert.Equal(ErrorCode.NotFound, resultWihtWrongMentor.Error.Code);
            Assert.Equal(ErrorCode.NotFound, resultWithWrongLessonVisit.Error.Code);
            Assert.Equal(ErrorCode.ValidationError, resultWithWrongLessonVisitStudent.Error.Code);
            Assert.Equal(ErrorCode.ValidationError, resultWithEmptyLessonVisit.Error.Code);
            Assert.Equal(ErrorCode.ValidationError, resultWithWrongLessonDate.Error.Code);
            Assert.NotEmpty(result.Data.LessonVisits);

            Assert.Equal(createdLesson.Id, result.Data.Id);
            Assert.Equal(createdLesson.LessonDate, result.Data.LessonDate);
            Assert.Equal(createdLesson.LessonVisits.Count, result.Data.LessonVisits.Count);


            for (int i = 0; i < result.Data.LessonVisits?.Count; i++)
            {
                Assert.Equal(createdLesson.LessonVisits[i]?.Comment, result.Data.LessonVisits[i]?.Comment);
                Assert.Equal(createdLesson.LessonVisits[i]?.Presence, result.Data.LessonVisits[i]?.Presence);
                Assert.Equal(createdLesson.LessonVisits[i]?.StudentId, result.Data.LessonVisits[i]?.StudentId);
                Assert.Equal(createdLesson.LessonVisits[i]?.StudentMark, result.Data.LessonVisits[i]?.StudentMark);
            }

            Assert.Equal(createdLesson.MentorId, result.Data.MentorId);
            Assert.Equal(createdLesson.StudentGroupId, result.Data.StudentGroupId);
            Assert.Equal(createdLesson.ThemeName, result.Data.ThemeName);

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
                LessonDate = DateTime.Parse("2021-11-18T15:30:00.384Z"),
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

            var lessonService = new LessonService(
                _unitOfWorkMock.Object,
                _mapper
                );

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

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock;
        }
    }
}
