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

namespace CharlieBackend.Api.UnitTest.RepositoriesTests
{
    public class DashboardRepositoryTest : TestBase
    {
        //короче тут мок констекста
        // ему на вход идет опшионс
        private readonly Mock<ApplicationContext> _applicationContextMock;

        public DashboardRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            _applicationContextMock = new Mock<ApplicationContext>(optionsBuilder.Options);
        }

        [Fact]
        public async Task GetGroupIds()
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

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
