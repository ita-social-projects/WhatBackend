using Moq;
using Xunit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MockQueryable.Moq;
using CharlieBackend.Core.DTO.Dashboard;

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
        public async Task GetGroupsIdsByCourseIdAsync()
        {
            // Arrange
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

            _applicationContextMock.Setup(x => x.StudentGroups).Returns(mock.Object);

            var dashboardRepositort = new DashboardRepository(_applicationContextMock.Object);

            // Act

            var result = await dashboardRepositort.GetGroupsIdsByCourseIdAsync(1, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new List<long> { 1, 4 }, result);
        }

        [Fact]
        public async Task GetGroupsIdsByStudentIdAndPeriodAsync()
        {
            //Arrange


            var groups = new List<StudentGroup>
            {

                 new StudentGroup()
                 {
                    CourseId = 2,
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
                    CourseId = 3,
                    StartDate = new DateTime(2000, 1, 1),
                    FinishDate = new DateTime(2003, 10, 2),
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

            _applicationContextMock.Setup(x => x.StudentGroups).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);
            //Act

            var result = await dashboardRepository.GetGroupsIdsByStudentIdAndPeriodAsync(10, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            //Assert

            Assert.NotNull(result);
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
            Assert.Equal(new List<long> { 10, 6 }, result);
        }

        [Fact]
        public async Task GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync()
        {
            //Arrange

            var visits = new List<Visit>
            {
                new Visit
                {
                    Lesson = new Lesson
                    {
                        StudentGroup = new StudentGroup
                        {
                            StudentsOfStudentGroups = new List<StudentOfStudentGroup>
                            {
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 10
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 11
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 12
                                    }
                                }
                            },
                            CourseId = 2,
                        },
                        StudentGroupId = 1,
                    },
                    Presence = true,
                    StudentId =10,

                },
                new Visit
                {
                    Lesson = new Lesson
                    {
                        StudentGroup = new StudentGroup
                        {
                            StudentsOfStudentGroups = new List<StudentOfStudentGroup>
                            {
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 10
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 11
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 12
                                    }
                                }
                            },
                            CourseId = 2,
                        },
                        StudentGroupId = 1
                    },
                    Presence = false,
                    StudentId =11,
                },
                new Visit
                {
                    Lesson = new Lesson
                    {
                        StudentGroup = new StudentGroup
                        {
                            StudentsOfStudentGroups = new List<StudentOfStudentGroup>
                            {
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 10
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 11
                                    }
                                },
                                new StudentOfStudentGroup
                                {
                                    Student = new Student
                                    {
                                        Id = 12
                                    }
                                }
                            },
                            CourseId = 2,
                        },
                        StudentGroupId = 1
                    },
                    Presence = true,
                    StudentId =12,
                }
            };

            var expected = new List<AverageStudentVisitsDto>
            {
                new AverageStudentVisitsDto
                {
                    StudentId = 10,
                    CourseId = 2,
                    StudentGroupId =1,
                    StudentAverageVisitsPercentage = 100
                },
                new AverageStudentVisitsDto
                {
                     StudentId = 11,
                    CourseId = 2,
                    StudentGroupId =1,
                    StudentAverageVisitsPercentage = 0
                }
            };

            var mock = visits.AsQueryable().BuildMockDbSet();

            _applicationContextMock.Setup(x => x.Visits).Returns(mock.Object);

            var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);
            //Act

            var result = await dashboardRepository.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(new List<long> { 10, 11 }, new List<long> { 1 });

            //Assert

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
//Arrange


//var dashboardRepository = new DashboardRepository(_applicationContextMock.Object);
//Act

//Assert
