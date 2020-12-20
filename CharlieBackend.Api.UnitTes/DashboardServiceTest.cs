using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class DashboardServiceTest : TestBase
    {
        [Fact]
        public async Task GetStudentsClassbook()
        {
            //Arrange
            var studentclassbookRequestWithData = new StudentsRequestDto<ClassbookResultType>()
            {
                CourseId = 5,
                StartDate = new DateTime(2010, 01, 01),
                FinishDate = new DateTime(2021, 01, 01),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentPresence,
                    ClassbookResultType.StudentMarks
                }
            };

            var studentclassbookWrongRequest = new StudentsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2010, 01, 01),
                FinishDate = new DateTime(2021, 01, 01),
            };

            var studentclassbookCourseWithoutStudents = new StudentsRequestDto<ClassbookResultType>()
            {
                CourseId = 10,
                StartDate = new DateTime(2010, 01, 01),
                FinishDate = new DateTime(2021, 01, 01),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentPresence,
                    ClassbookResultType.StudentMarks
                }
            };

            var dashboardRepositoryMock = new Mock<IDashboardRepository>();

            dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 1, 5 });

            dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                studentclassbookCourseWithoutStudents.CourseId.Value,
                studentclassbookCourseWithoutStudents.StartDate,
                studentclassbookCourseWithoutStudents.FinishDate)).ReturnsAsync(default(List<long>));

            dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(
                new List<long>() { 1, 5 })).ReturnsAsync(new List<long> { 5, 7, 15, 43 });

            dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(default(List<long>))).ReturnsAsync(default(List<long>));

            dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByStudentIds(
                new List<long>() { 5, 7, 15, 43 })).ReturnsAsync(new List<StudentVisitDto>() 
                {
                    new StudentVisitDto()
                    {
                        CourseId = 5,
                        LessonDate = new DateTime(2015, 01, 01),
                        StudentId = 6,
                        LessonId = 1,
                        Presence = true,
                        StudentGroupId = 1
                    }
                });

            dashboardRepositoryMock.Setup(x => x
                .GetStudentsMarksListByStudentIds(new List<long>() { 1, 5 }))
                .ReturnsAsync(new List<StudentMarkDto>
                {
                    new StudentMarkDto()
                    {
                        CourseId = 5,
                        LessonDate = new DateTime(2015, 01, 01),
                        LessonId = 1,
                        StudentGroupId = 1,
                        StudentId = 6,
                        StudentMark = 5
                    }
                });

            dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByStudentIds(default(List<long>)))
                .ReturnsAsync(new List<StudentVisitDto>());

            dashboardRepositoryMock.Setup(x => x
                .GetStudentsMarksListByStudentIds(default(List<long>)))
                .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashboardRepositoryMock.Object);

            var dashboardService = new DashboardService(
                _unitOfWorkMock.Object
                );

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);
            var requestWithWrongParameters = await dashboardService.GetStudentsClassbookAsync(studentclassbookWrongRequest);
            var requestForCourseWithoutStudents = await dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

            //Assert
            Assert.NotNull(successResult.Data);
            Assert.Equal(ErrorCode.ValidationError, requestWithWrongParameters.Error.Code);
            Assert.Empty(requestForCourseWithoutStudents.Data.StudentsMarks);
            Assert.Empty(requestForCourseWithoutStudents.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentResult()
        {
            //Arrange

            var studentResultRequestWithData = new StudentsRequestDto<StudentResultType>()
            {
                CourseId = 3,
                StartDate = new DateTime(2012, 1, 12),
                FinishDate = new DateTime(2015, 4, 21),
                IncludeAnalytics = new StudentResultType[]
                {
                    StudentResultType.AverageStudentMark,
                    StudentResultType.AverageStudentVisits
                }
            };

            var studentResultWrongRequest = new StudentsRequestDto<StudentResultType>()
            {
                StartDate = new DateTime(2012, 1, 12),
                FinishDate = new DateTime(2015, 4, 21)
            };

            var dashbordrepositoriMock = new Mock<IDashboardRepository>();

            dashbordrepositoriMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                (long)studentResultRequestWithData.CourseId, studentResultRequestWithData.StartDate, studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            dashbordrepositoriMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                (long)studentResultWrongRequest.CourseId, studentResultWrongRequest.StartDate, studentResultWrongRequest.FinishDate))
                .ReturnsAsync(new List<long>() { });

            dashbordrepositoriMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            dashbordrepositoriMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
               new List<long>() { 6, 7, 8, 9, 10 }, new List<long>() { 2 }))
                .ReturnsAsync(new List<AverageStudentMarkDto>() {
                    new AverageStudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageMark = 5.1
                    }
                });

            dashbordrepositoriMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(
                 new List<long>() { 6, 7, 8, 9, 10 }, new List<long>() { 2 }))
                .ReturnsAsync(new List<AverageStudentVisitsDto>()
                {
                    new AverageStudentVisitsDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageVisitsPercentage = 15
                    }
                });

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordrepositoriMock.Object);

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);
            var resultWithWrongData = await dashbordservice.GetStudentsResultAsync(studentResultWrongRequest);

            //Assert
            //тут пока не понял что писать 

        }
        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
