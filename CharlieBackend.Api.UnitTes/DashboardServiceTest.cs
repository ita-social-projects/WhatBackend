using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.AspNetCore.Http;
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

            var studentResultRequestWithOutStudent = new StudentsRequestDto<StudentResultType>()
            {
                CourseId = 9,
                StartDate = new DateTime(2012, 1, 12),
                FinishDate = new DateTime(2015, 4, 21),
                IncludeAnalytics = new StudentResultType[]
                {
                    StudentResultType.AverageStudentMark,
                    StudentResultType.AverageStudentVisits
                }
            };

            var dashbordrepositoryMock = new Mock<IDashboardRepository>();

            dashbordrepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                (long)studentResultRequestWithData.CourseId, studentResultRequestWithData.StartDate, studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            dashbordrepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                (long)studentResultRequestWithOutStudent.CourseId, studentResultRequestWithOutStudent.StartDate, studentResultRequestWithOutStudent.FinishDate))
                .ReturnsAsync(new List<long>() { });

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(default(List<long>))).ReturnsAsync(default(List<long>));
            
            dashbordrepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
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

            dashbordrepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(default(List<long>), default(List<long>)))
                .ReturnsAsync(new List<AverageStudentMarkDto>());

            dashbordrepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(
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

            dashbordrepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(default(List<long>), default(List<long>)))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordrepositoryMock.Object);

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);
            var resultWithWrongData = await dashbordservice.GetStudentsResultAsync(studentResultWrongRequest);
            var resultWithoutStudent = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithOutStudent);

            //Assert
            Assert.NotNull(resultWithData.Data);
            Assert.Equal(ErrorCode.ValidationError, resultWithWrongData.Error.Code);
            Assert.Empty(resultWithoutStudent.Data.AverageStudentsMarks);
            Assert.Empty(resultWithoutStudent.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentClassbook()
        {
            //Arrange
            long studentIdWithGroup = 5;
            long studentIdWithoutGroup = 20;
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Denys"),
                new Claim(ClaimTypes.NameIdentifier , "1"),
                new Claim(ClaimTypes.Role, UserRole.Student.ToString()),
                new Claim("Id", "7")
            };
            var claimIdentity = new ClaimsIdentity(claims, "TestAuthType");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(claimIdentity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new ClassbookResultType []
                {
                    ClassbookResultType.StudentMarks,
                    ClassbookResultType.StudentPresence
                }
            };

            var dashbordAnaliticRequstWithoutClassbook = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
            };

            var dashbordRepositoryMock = new Mock<IDashboardRepository>();

            dashbordRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });
            
            dashbordRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(default(List<long>));

            dashbordRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(
                studentIdWithGroup, new List<long>() { 1 }))
                .ReturnsAsync(new List<StudentVisitDto>
                {
                    new StudentVisitDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        LessonId = 2,
                        LessonDate = new DateTime(2015,4,12),
                        Presence = true
                    }
                }) ;

            dashbordRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(studentIdWithoutGroup, default(List<long>)))
                .ReturnsAsync(new List<StudentVisitDto>());

            dashbordRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(
                studentIdWithGroup, new List<long> { 1 }))
                .ReturnsAsync(new List<StudentMarkDto>
                {
                    new StudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        LessonId = 2,
                        LessonDate = new DateTime(2015,4,12),
                        StudentMark = 5
                    }
                });
           
            dashbordRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(studentIdWithoutGroup, default(List<long>)))
                .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordRepositoryMock.Object);

            var dashbordService = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var resultWithData = await dashbordService.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData, claimPrincipal); // вопрос о третьем параметре 
            var resultWithoutClassbook = await dashbordService.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook, claimPrincipal);
            var resultWithoutGrop = await dashbordService.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData, claimPrincipal);

            //Assert
            Assert.NotNull(resultWithData);
            Assert.Equal(ErrorCode.ValidationError, resultWithoutClassbook.Error.Code);
            Assert.Empty(resultWithoutGrop.Data.StudentsMarks);
            Assert.Empty(resultWithoutGrop.Data.StudentsPresences);
        }
        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
