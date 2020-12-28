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

namespace CharlieBackend.Api.UnitTest.RepositoriesTests
{
    public class DashboardRepositoryTest : TestBase
    {
        private readonly Mock<ApplicationContext> _applicationContextMock;
        private readonly Mock<DbSet<StudentGroup>> _dbSetStudGroupMock;

        public DashboardRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            _applicationContextMock = new Mock<ApplicationContext>(optionsBuilder.Options);
            _dbSetStudGroupMock = new Mock<DbSet<StudentGroup>>();
        }

        [Fact]
        public async Task GetGroupIds()
        {
            // Arrange

            var inRangeStdentGroupFirst = new StudentGroup()
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
            };

            var inRangeStdentGroupSecond = new StudentGroup()
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
            };

            var outOfRangeStdentGroup = new StudentGroup()
            {
                CourseId = 1,
                StartDate = new DateTime(2003, 1, 1),
                FinishDate = new DateTime(2006, 10, 2),
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
            };

            //_applicationContextMock.Setup(x => x.);
            _applicationContextMock.Setup(x => x.StudentGroups.AddRange(inRangeStdentGroupFirst, inRangeStdentGroupSecond, outOfRangeStdentGroup));

            DashboardRepository dashboardRepository = new DashboardRepository(_applicationContextMock.Object);
            // Act

            var result = await dashboardRepository.GetGroupsIdsByCourseIdAsync(1, new DateTime(2000, 9, 1), new DateTime(2001, 12, 12));

            // Assert
            Assert.Equal(new List<long> { 1, 2 }, result);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
