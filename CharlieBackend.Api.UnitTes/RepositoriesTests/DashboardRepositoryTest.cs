using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data;
using CharlieBackend.Data.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest.RepositoriesTests
{
    public class DashboardRepositoryTest : TestBase
    {
        private readonly Mock<ApplicationContext> _applicationContextMock;

        public DashboardRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            _applicationContextMock = new Mock<ApplicationContext>(optionsBuilder.Options);
        }

        [Fact]
        public async Task GetGroupsIdsByCourseIdAndPeriodAsync()
        {
            // Arrange

            var mock = GetMockDbsetOfStudentGroup();

            _applicationContextMock.Setup(x => x.StudentGroups).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            // Act

            var result = await dashboardRepository.GetGroupsIdsByCourseIdAndPeriodAsync(1, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            // Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(new List<long> { 1, 4 }, result);
        }

        [Fact]
        public async Task GetGroupsIdsByStudentIdAndPeriodAsync()
        {
            //Arrange

            var mock = GetMockDbsetOfStudentGroup();

            _applicationContextMock.Setup(x => x.StudentGroups).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetGroupsIdsByStudentIdAndPeriodAsync(10, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(new List<long> { 1 }, result);
        }

        [Fact]
        public async Task GetStudentsIdsByGroupIdsAsync()
        {
            //Arrange

            var students = new List<Student>
            {
                new Student
                {
                    Id = 10,

                    StudentsOfStudentGroups = new HashSet<StudentOfStudentGroup>()
                    {
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 4
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 1
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 3
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 2
                        }
                    }
                },
                new Student
                {
                    Id = 6,

                    StudentsOfStudentGroups = new HashSet<StudentOfStudentGroup>()
                    {
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 4
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 8
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 12
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 5
                        }
                    }
                },
                new Student
                {
                    Id = 23,

                    StudentsOfStudentGroups = new HashSet<StudentOfStudentGroup>()
                    {
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 6
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 3
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 5
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentGroupId = 2
                        }
                    }
                }
            };

            var mock = students.AsQueryable().BuildMockDbSet();

            _applicationContextMock.Setup(x => x.Students).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentsIdsByGroupIdsAsync(new List<long> { 1, 4 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(new List<long> { 10, 6 }, result);
        }

        [Fact]
        public async Task GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync()
        {
            //Arrange

            var expected = new List<AverageStudentVisitsDto>
            {
                new AverageStudentVisitsDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 10",
                    StudentAverageVisitsPercentage = 100
                },
                new AverageStudentVisitsDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 11",
                    StudentAverageVisitsPercentage = 0
                },

            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.Setup(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(new List<long> { 10, 11 }, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentsPresenceListByStudentIds()
        {
            //Arrange

            var expected = new List<StudentVisitDto>
            {

                new StudentVisitDto
                {
                    LessonDate = new DateTime(),
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 11",
                    StudentId = 11,
                    Presence =false,
                },
                new StudentVisitDto
                {
                    LessonDate = new DateTime(),
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentId = 12,
                    Presence =false,
                },
                new StudentVisitDto
                {
                    LessonDate = new DateTime(),
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentId = 12,
                    Presence =true,
                },
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.Setup(X => X.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentsPresenceListByStudentIds(new List<long> { 11, 12 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentsMarksListByStudentIds()
        {
            //Arrange

            var expected = new List<StudentMarkDto>
            {
                new StudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 10",
                    StudentId = 10,
                    LessonId = 10,
                    LessonDate = new DateTime(),
                    StudentMark = 100,
                },
                new StudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentId = 12,
                    LessonId = 10,
                    LessonDate = new DateTime(),
                    StudentMark = 30,
                },
                new StudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentId = 12,
                    LessonId = 10,
                    LessonDate = new DateTime(),
                    StudentMark = 90,
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentsMarksListByStudentIds(new List<long> { 10, 12 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentAverageMarksByStudentIdsAndGropsIdsAsync()
        {
            //Arrange

            var expected = new List<AverageStudentMarkDto>
            {
                new AverageStudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 11",
                    StudentAverageMark = 78
                },
                new AverageStudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentAverageMark = 60
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.Setup(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long> { 11, 12 }, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentAverageVisitsPercentageByStudentIdsAsync()
        {
            //Arrange

            var expected = new List<AverageStudentVisitsDto>()
            {
                new AverageStudentVisitsDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Student = "Ivan 12",
                    StudentAverageVisitsPercentage = 50
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentAverageVisitsPercentageByStudentIdsAsync(12, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentPresenceListByStudentIds()
        {
            //Arrange

            var expected = new List<StudentVisitDto>
            {
                new StudentVisitDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Presence =false,
                    StudentId =12,
                    LessonId = 10
                },
                new StudentVisitDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    Presence =true,
                    StudentId =12,
                    LessonId = 10
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentPresenceListByStudentIds(12, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentMarksListByStudentIds()
        {
            //Arrange

            var expected = new List<StudentMarkDto>
            {
                new StudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    LessonId =10,
                    StudentId = 12,
                    StudentMark =0
                },
                new StudentMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    LessonId =10,
                    StudentId = 12,
                    StudentMark =90
                }

            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentMarksListByStudentIds(12, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentGroupsAverageMarks()
        {
            //Arrange

            var expected = new List<AverageStudentGroupMarkDto>
            {
                new AverageStudentGroupMarkDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    AverageMark = (decimal)47.5
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentGroupsAverageMarks(new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        [Fact]
        public async Task GetStudentGroupsAverageVisits()
        {
            //Arrange

            var expected = new List<AverageStudentGroupVisitDto>
            {
                new AverageStudentGroupVisitDto
                {
                    Course = "Course",
                    StudentGroup = "Group",
                    AverageVisitPercentage = 50
                }
            };

            var mock = GetMockDbSetOfVisits();

            _applicationContextMock.SetupGet(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);

            //Act

            var result = await dashboardRepository.GetStudentGroupsAverageVisits(new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ToString(), result[i].ToString());
            }
        }

        private Mock<DbSet<Visit>> GetMockDbSetOfVisits()
        {
            var lesson = new Lesson
            {
                StudentGroup = new StudentGroup
                {
                    StudentsOfStudentGroups = new List<StudentOfStudentGroup>
                    {
                        new StudentOfStudentGroup
                        {
                            Student = new Student
                            {
                                Id = 10,
                                Account = new Account
                                {
                                    FirstName = "Ivan",
                                    LastName = "10"
                                }
                            }
                        },
                        new StudentOfStudentGroup
                        {
                            Student = new Student
                            {
                                Id = 11,
                                Account = new Account
                                {
                                    FirstName = "Ivan",
                                    LastName = "11"
                                }
                            }
                        },
                        new StudentOfStudentGroup
                        {
                            Student = new Student
                            {
                                Id =12,
                                Account = new Account
                                {
                                    FirstName = "Ivan",
                                    LastName = "12"
                                }
                            }
                        }
                    },
                    CourseId = 2,
                    Course = new Course 
                    { 
                        Name = "Course" 
                    },
                    Name = "Group",
                },
                StudentGroupId = 1,
                Id = 10,
            };

            var visits = new List<Visit>
            {
                new Visit
                {
                    Student = new Student
                    {
                        Id = 10,
                        Account = new Account
                        {
                            FirstName = "Ivan",
                            LastName = "10"
                        }
                    },
                    Lesson = lesson,
                    Presence = true,
                    StudentId =10,
                    StudentMark = 100
                },
                new Visit
                {
                    Student = new Student
                    {
                        Id = 11,
                        Account = new Account
                        {
                            FirstName = "Ivan",
                            LastName = "11"
                        }
                    },
                    Lesson = lesson,
                    Presence = false,
                    StudentId =11,
                    StudentMark = 0
                },
                new Visit
                {
                    Student = new Student
                    {
                        Id = 12,
                        Account = new Account
                        {
                            FirstName = "Ivan",
                            LastName = "12"
                        }
                    },
                    Lesson = lesson,
                    Presence = false,
                    StudentId =12,
                    StudentMark = 0
                },
                new Visit
                {
                    Student = new Student
                    {
                        Id = 12,
                        Account = new Account
                        {
                            FirstName = "Ivan",
                            LastName = "12"
                        }
                    },
                    Lesson = lesson,
                    Presence = true,
                    StudentId =12,
                    StudentMark = 90
                }
            };

            var mock = visits.AsQueryable().BuildMockDbSet();

            return mock;
        }

        private Mock<DbSet<StudentGroup>> GetMockDbsetOfStudentGroup()
        {
            var groups = new List<StudentGroup>
            {

                 new StudentGroup()
                 {
                    CourseId = 1,
                     StartDate = new DateTime(2000, 10, 1),
                     FinishDate = new DateTime(2001, 1, 20),
                     Id = 1,
                     StudentsOfStudentGroups = new List<StudentOfStudentGroup>()
                     {
                        new StudentOfStudentGroup()
                        {
                            StudentId = 10,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 11,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 12,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 13,
                        }
                     }
                 },

                 new StudentGroup()
                 {
                    CourseId = 1,
                    StartDate = new DateTime(2000, 11, 4),
                    FinishDate = new DateTime(2001, 1, 10),
                    Id = 4,
                    StudentsOfStudentGroups = new List<StudentOfStudentGroup>()
                    {
                        new StudentOfStudentGroup()
                        {
                            StudentId = 4,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 5,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 6,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 20,
                        }
                    }
                 },

                 new StudentGroup()
                 {
                    CourseId = 1,
                    StartDate = new DateTime(2003, 1, 1),
                    FinishDate = new DateTime(2006, 10, 2),
                    Id = 5,
                    StudentsOfStudentGroups = new List<StudentOfStudentGroup>()
                    {
                        new StudentOfStudentGroup()
                        {
                            StudentId = 10,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 11,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 12,
                        },
                        new StudentOfStudentGroup()
                        {
                            StudentId = 13,
                        }
                    }
                 }
            };

            var mock = groups.AsQueryable().BuildMockDbSet();

            return mock;
        }
    }
}