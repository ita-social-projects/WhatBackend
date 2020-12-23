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
        public async Task GetStudentsResult()
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
                CourseId = 3,
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
                (long)studentResultRequestWithOutStudent.CourseId, 
                studentResultRequestWithOutStudent.StartDate,
                studentResultRequestWithOutStudent.FinishDate))
                .ReturnsAsync(default(List<long>));

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(default(List<long>)))
                .ReturnsAsync(default(List<long>));
            
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
            long studentIdWithGroup = 7;
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

            var claimsForStudentWitoutGroup = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Denys"),
                new Claim(ClaimTypes.NameIdentifier , "1"),
                new Claim(ClaimTypes.Role, UserRole.Student.ToString()),
                new Claim("Id", "20")
            };

            var claimIdentityForStudentWitoutGroup = new ClaimsIdentity(claimsForStudentWitoutGroup, "TestAuthType");
            var claimPrincipalForStudentWitoutGroup = new ClaimsPrincipal(claimIdentityForStudentWitoutGroup);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(claimIdentity);
            mockPrincipal.Setup(x => x.Identity).Returns(claimIdentityForStudentWitoutGroup);
            mockPrincipal.Setup(x => x.IsInRole(UserRole.Student.ToString())).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(UserRole.Mentor.ToString())).Returns(false);

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
                .ReturnsAsync(new List<long>());

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

            dashbordRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(studentIdWithoutGroup, new List<long> { }))
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
           
            dashbordRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(studentIdWithoutGroup, new List<long> { }))
                .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordRepositoryMock.Object);

            var dashbordService = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var resultWithData = await dashbordService.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData, claimPrincipal); // вопрос о третьем параметре 
            var resultWithoutClassbook = await dashbordService.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook, claimPrincipal);
            var requestWithStrangerCredentials = await dashbordService.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData, claimPrincipal); // вопрос о третьем параметре 
            var resultWithoutGrop = await dashbordService.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData, claimPrincipalForStudentWitoutGroup);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, requestWithStrangerCredentials.Error.Code);
            Assert.NotNull(resultWithData);
            Assert.Equal(ErrorCode.ValidationError, resultWithoutClassbook.Error.Code);
            Assert.Empty(resultWithoutGrop.Data.StudentsMarks);
            Assert.Empty(resultWithoutGrop.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentResult()
        {
            //Arrange
            long studentIdWithGroup = 7;
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

            var claimsForStudentWitoutGroup = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Denys"),
                new Claim(ClaimTypes.NameIdentifier , "1"),
                new Claim(ClaimTypes.Role, UserRole.Student.ToString()),
                new Claim("Id", "20")
            };

            var claimIdentityForStudentWitoutGroup = new ClaimsIdentity(claimsForStudentWitoutGroup, "TestAuthType");
            var claimPrincipalForStudentWitoutGroup = new ClaimsPrincipal(claimIdentityForStudentWitoutGroup);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(claimIdentity);
            mockPrincipal.Setup(x => x.Identity).Returns(claimIdentityForStudentWitoutGroup);
            mockPrincipal.Setup(x => x.IsInRole(UserRole.Student.ToString())).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(UserRole.Mentor.ToString())).Returns(false); 

            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<StudentResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new StudentResultType[]
                {
                    StudentResultType.AverageStudentMark,
                    StudentResultType.AverageStudentVisits
                }
            };

            var dashbordAnaliticRequstWithoutClassbook = new DashboardAnalyticsRequestDto<StudentResultType>()
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
                .ReturnsAsync(new List<long>());

            dashbordRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
                new List<long> { studentIdWithGroup }, new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentMarkDto>
                {
                    new AverageStudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        StudentAverageMark = 5.1
                    }
                });

            dashbordRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long> { studentIdWithoutGroup },new List<long>()))
                .ReturnsAsync(new List<AverageStudentMarkDto>());

            dashbordRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(
                studentIdWithGroup, new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentVisitsDto>
                {
                    new AverageStudentVisitsDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        StudentAverageVisitsPercentage = 15
                    }
                });

            dashbordRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(studentIdWithoutGroup, new List<long>()))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordRepositoryMock.Object);

            var dashbordService = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var resultWithData = await dashbordService.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData, claimPrincipal); // вопрос о третьем параметре 
            var resultWithoutClassbook = await dashbordService.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook, claimPrincipal);
            var resultWithoutGroup = await dashbordService.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData, claimPrincipalForStudentWitoutGroup);
            var resultWithDataWithWrongClaim = await dashbordService.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData, claimPrincipal); // вопрос о третьем параметре 

            //Assert
            Assert.NotNull(resultWithData);
            Assert.Equal(ErrorCode.ValidationError, resultWithDataWithWrongClaim.Error.Code);
            Assert.Equal(ErrorCode.ValidationError, resultWithoutClassbook.Error.Code);
            Assert.Empty(resultWithoutGroup.Data.AverageStudentsMarks);
            Assert.Empty(resultWithoutGroup.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentGroupResult()
        {
            ////Arrange
            long courseId = 1;
            long courseIdWitoutGroup = 2;
            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<StudentGroupResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new StudentGroupResultType[]
                {
                    StudentGroupResultType.AverageStudentGroupMark,
                    StudentGroupResultType.AverageStudentGroupVisitsPercentage
                }
            };

            var dashbordAnaliticRequstWithoutData = new DashboardAnalyticsRequestDto<StudentGroupResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
            };

            var dashbortRepositoryMock = new Mock<IDashboardRepository>();

            dashbortRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            dashbortRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAsync(
                courseIdWitoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());

            dashbortRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentGroupMarkDto>
                {
                    new AverageStudentGroupMarkDto()
                    { 
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageMark = 4.5
                    }
                });
            
            dashbortRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long>()))
                .ReturnsAsync(new List<AverageStudentGroupMarkDto>());

            dashbortRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentGroupVisitDto>
                {
                    new AverageStudentGroupVisitDto()
                    {
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageVisitPercentage = 15
                    }
                });

            dashbortRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(new List<long>()))
                .ReturnsAsync(new List<AverageStudentGroupVisitDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbortRepositoryMock.Object);
            var dashbordService = new DashboardService(_unitOfWorkMock.Object);

            //Act
            var requestWithData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);
            var requestWithoutData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithoutData);
            var requestWithoutGroupOnCourse = await dashbordService.GetStudentGroupResultAsync(courseIdWitoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotNull(requestWithData);
            Assert.Equal(ErrorCode.ValidationError, requestWithoutData.Error.Code);
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsMarks);
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsVisits);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
