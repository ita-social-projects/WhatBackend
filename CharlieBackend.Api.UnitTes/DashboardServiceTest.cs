using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
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
                CourseId = 1,
                StudentGroupId = 0,
                StartDate = new DateTime(2000, 01, 01),
                FinishDate = new DateTime(2030, 01, 01),
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
                CourseId = 11,
                StudentGroupId = 0,
                StartDate = new DateTime(2010, 01, 01),
                FinishDate = new DateTime(2021, 01, 01),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentPresence,
                    ClassbookResultType.StudentMarks
                }
            };

            var dashboardRepositoryMock = new Mock<IDashboardRepository>();

            dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value, 
                studentclassbookRequestWithData.StartDate, 
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 2 });

            dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookCourseWithoutStudents.CourseId.Value,
                studentclassbookCourseWithoutStudents.StartDate,
                studentclassbookCourseWithoutStudents.FinishDate)).ReturnsAsync(new List<long>());

            dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
                new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
                .ReturnsAsync(new List<StudentVisitDto>() { new StudentVisitDto() { StudentGroupId = 1 } });

            dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
                .ReturnsAsync(new List<StudentMarkDto>() { new StudentMarkDto() { StudentGroupId = 1 } });

            dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
                new List<long>(), studentclassbookCourseWithoutStudents.StartDate, studentclassbookCourseWithoutStudents.FinishDate))
                .ReturnsAsync(new List<StudentVisitDto>());

            dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                new List<long>(), studentclassbookCourseWithoutStudents.StartDate, studentclassbookCourseWithoutStudents.FinishDate))
                .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashboardRepositoryMock.Object);
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);
            var requestWithWrongParameters = await dashboardService.GetStudentsClassbookAsync(studentclassbookWrongRequest);
            var requestForCourseWithoutStudents = await dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

            //Assert
            Assert.NotEmpty(successResult.Data.StudentsMarks);
            Assert.NotEmpty(successResult.Data.StudentsPresences);
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
                CourseId = 5,
                StartDate = new DateTime(2012, 1, 12),
                FinishDate = new DateTime(2015, 4, 21),
                IncludeAnalytics = new StudentResultType[]
                {
                    StudentResultType.AverageStudentMark,
                    StudentResultType.AverageStudentVisits
                }
            };

            var dashbordrepositoryMock = new Mock<IDashboardRepository>();

            dashbordrepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithData.CourseId, studentResultRequestWithData.StartDate, studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            dashbordrepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithOutStudent.CourseId, 
                studentResultRequestWithOutStudent.StartDate,
                studentResultRequestWithOutStudent.FinishDate))
                .ReturnsAsync(new  List<long>());

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            dashbordrepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>()))
                .ReturnsAsync(new List<long>());
            
            dashbordrepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
               new List<long>() { 6, 7, 8, 9, 10 }, new List<long>() { 2 }))
                .ReturnsAsync(new List<AverageStudentMarkDto>() {
                    new AverageStudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageMark = (decimal)5.1
                    }
                });

            dashbordrepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long>(), new List<long>()))
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

            dashbordrepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(new List<long>(), new List<long>()))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(dashbordrepositoryMock.Object);
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);
            var resultWithWrongData = await dashbordservice.GetStudentsResultAsync(studentResultWrongRequest);
            var resultWithoutStudent = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithOutStudent);

            //Assert
            Assert.NotEmpty(resultWithData.Data.AverageStudentsMarks);
            Assert.NotEmpty(resultWithData.Data.AverageStudentVisits);
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
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);
            var currentUserServiceAsStrangerStudent = GetCurrentUserAsExistingStudent(entityId: long.MaxValue);

            var dashbordServiceWithStrangerCredentials = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStrangerStudent.Object);
            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);
            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);
            var resultWithoutClassbook = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook);
            var requestWithStrangerCredentials = await dashbordServiceWithStrangerCredentials.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);
            var resultWithoutGrop = await dashbordServiceWithoutGroup.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, requestWithStrangerCredentials.Error.Code);
            Assert.NotEmpty(resultWithData.Data.StudentsMarks);
            Assert.NotEmpty(resultWithData.Data.StudentsPresences);
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
                        StudentAverageMark = (decimal)5.1
                    }
                });

            dashbordRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long> { studentIdWithoutGroup }, new List<long>()))
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
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);
            var currentUserServiceAsStrangerStudent = GetCurrentUserAsExistingStudent(entityId: long.MaxValue);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);
            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);
            var dashbordServiceWithStrangerCredentials = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStrangerStudent.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);
            var resultWithoutClassbook = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook);
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);
            var resultWithDataWithWrongClaim = await dashbordServiceWithStrangerCredentials.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotEmpty(resultWithData.Data.AverageStudentsMarks);
            Assert.NotEmpty(resultWithData.Data.AverageStudentVisits);
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

            dashbortRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            dashbortRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseIdWitoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());

            dashbortRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentGroupMarkDto>
                {
                    new AverageStudentGroupMarkDto()
                    { 
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageMark = (decimal)4.5
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
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);
            var requestWithoutData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithoutData);
            var requestWithoutGroupOnCourse = await dashbordService.GetStudentGroupResultAsync(courseIdWitoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotEmpty(requestWithData.Data.AverageStudentGroupsMarks);
            Assert.NotEmpty(requestWithData.Data.AverageStudentGroupsVisits);
            Assert.Equal(ErrorCode.ValidationError, requestWithoutData.Error.Code);
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsMarks);
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsVisits);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }

        private static ClaimsPrincipal GetClaimsPrincipal(UserRole role, long id, string name)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier , "1"),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("Id", id.ToString())
            };

            var claimIdentity = new ClaimsIdentity(claims, "TestAuthType");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            return claimPrincipal;
        }

    }
}
