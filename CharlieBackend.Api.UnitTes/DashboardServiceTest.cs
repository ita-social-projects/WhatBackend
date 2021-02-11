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
        private readonly Mock<IDashboardRepository> _dashboardRepositoryMock;
        private static long studentIdWithGroup = 7;
        private static long studentIdWithoutGroup = 20;
        private static long courseId = 1;
        private static long courseIdWitoutGroup = 2;

        public DashboardServiceTest()
        {
            _dashboardRepositoryMock = new Mock<IDashboardRepository>();
        }

        private void InitializeForDashboardRepository()
        {
            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();
        }

        [Fact]
        public async Task GetStudentsClassbook_NotEmptyStudentsMarks_ShouldReturnCurrentStudentsMarks()
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
                .ReturnsAsync(new List<StudentMarkDto>() { new StudentMarkDto() { StudentGroupId = 1 } });

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);

            //Assert
            Assert.NotEmpty(successResult.Data.StudentsMarks);
        }

        [Fact]
        public async Task GetStudentsClassbook_NotEmptyStudentsPresence_ShouldReturnCurrentStudentsPresence()
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
               new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
               .ReturnsAsync(new List<StudentVisitDto>() { new StudentVisitDto() { StudentGroupId = 1 } });

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);

            //Assert
            Assert.NotEmpty(successResult.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentsClassbook_WrongDataWithoutCourseId_ShouldReturnValidationError()
        {
            //Arrange
            var studentClassbookWrongRequest = new StudentsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2010, 01, 01),
                FinishDate = new DateTime(2021, 01, 01),
            };

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithWrongParameters = await dashboardService.GetStudentsClassbookAsync(studentClassbookWrongRequest);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, requestWithWrongParameters.Error.Code);
        }

        [Fact]
        public async Task GetStudentsClassbook_RequestForCourseWithoutStudents_ShouldReturnEmptyStudentPresences()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookCourseWithoutStudents.CourseId.Value,
                studentclassbookCourseWithoutStudents.StartDate,
                studentclassbookCourseWithoutStudents.FinishDate)).ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
                new List<long>(), studentclassbookCourseWithoutStudents.StartDate, studentclassbookCourseWithoutStudents.FinishDate))
                .ReturnsAsync(new List<StudentVisitDto>());

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestForCourseWithoutStudents = await dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

            //Assert
            Assert.Empty(requestForCourseWithoutStudents.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentsClassbook_RequestForCourseWithoutStudents_ShouldReturnEmptyStudentMarks()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookCourseWithoutStudents.CourseId.Value,
                studentclassbookCourseWithoutStudents.StartDate,
                studentclassbookCourseWithoutStudents.FinishDate)).ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                 new List<long>(), studentclassbookCourseWithoutStudents.StartDate, studentclassbookCourseWithoutStudents.FinishDate))
                 .ReturnsAsync(new List<StudentMarkDto>());

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestForCourseWithoutStudents = await dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

            //Assert
            Assert.Empty(requestForCourseWithoutStudents.Data.StudentsMarks);
        }

        [Fact]
        public async Task GetStudentsResult_NotEmptyResultsForStudents_ShouldReturnStudentsAverageMarks()
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithData.CourseId, 
                studentResultRequestWithData.StartDate, 
                studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
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

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);

            //Assert
            Assert.NotEmpty(resultWithData.Data.AverageStudentsMarks);
        }

        [Fact]
        public async Task GetStudentsResult_NotEmptyResultsForStudents_ShouldReturnStudentsAverageVisits()
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithData.CourseId, studentResultRequestWithData.StartDate, studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(
                 new List<long>() { 6, 7, 8, 9, 10 }, new List<long>() { 2 }))
                .ReturnsAsync(new List<AverageStudentVisitsDto>()
                {
                    new AverageStudentVisitsDto()
                    {
                        CourseId = 5,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageVisitsPercentage = 15
                    }
                });

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);

            //Assert
            Assert.NotEmpty(resultWithData.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentsResult_WrongDataOfStudents_ShouldReturnValidationError()
        {
            //Arrange
            var studentResultWrongRequest = new StudentsRequestDto<StudentResultType>()
            {
                StartDate = new DateTime(2012, 1, 12),
                FinishDate = new DateTime(2015, 4, 21)
            };

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>()))
                .ReturnsAsync(new List<long>());

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithWrongData = await dashbordservice.GetStudentsResultAsync(studentResultWrongRequest);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, resultWithWrongData.Error.Code);
        }

        [Fact]
        public async Task GetStudentsResult_EmptyResultsForStudents_ShouldReturnEmptyStudentsAverageMarks()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithOutStudent.CourseId,
                studentResultRequestWithOutStudent.StartDate,
                studentResultRequestWithOutStudent.FinishDate))
                .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>()))
               .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long>(), new List<long>()))
                .ReturnsAsync(new List<AverageStudentMarkDto>());

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithoutStudent = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithOutStudent);

            //Assert
            Assert.Empty(resultWithoutStudent.Data.AverageStudentsMarks);
        }

        [Fact]
        public async Task GetStudentsResult_EmptyResultsForStudents_ShouldReturnEmptyStudentsAverageVisits()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithOutStudent.CourseId,
                studentResultRequestWithOutStudent.StartDate,
                studentResultRequestWithOutStudent.FinishDate))
                .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>()))
               .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(new List<long>(), new List<long>()))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithoutStudent = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithOutStudent);

            //Assert
            Assert.Empty(resultWithoutStudent.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentClassbook_StudentIdWithExistingGroup_ShouldReturnCurrentStudentResult()
        {
            //Arrange
            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentMarks,
                    ClassbookResultType.StudentPresence
                }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(
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
                });

            _dashboardRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(
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

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotEmpty(resultWithData.Data.StudentsMarks);
            Assert.NotEmpty(resultWithData.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentClassbook_StudentIdNotExistingGroup_ShouldReturnEmptyStudentResult()
        {
            //Arrange
            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentMarks,
                    ClassbookResultType.StudentPresence
                }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(studentIdWithoutGroup, new List<long> { }))
                .ReturnsAsync(new List<StudentVisitDto>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(studentIdWithoutGroup, new List<long> { }))
               .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Empty(resultWithoutGroup.Data.StudentsMarks);
            Assert.Empty(resultWithoutGroup.Data.StudentsPresences);
        }

        [Fact]
        public async Task GetStudentClassbook_NotExistingStudentId_ShouldReturnValidationError()
        {
            //Arrange
            var dashbordAnaliticRequstWithData = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentMarks,
                    ClassbookResultType.StudentPresence
                }
            };

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStrangerStudent = GetCurrentUserAsExistingStudent(entityId: long.MaxValue);

            var dashbordServiceWithStrangerCredentials = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStrangerStudent.Object);

            //Act
            var requestWithStrangerCredentials = await dashbordServiceWithStrangerCredentials.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, requestWithStrangerCredentials.Error.Code);
        }

        [Fact]
        public async Task GetStudentClassbook_ExistingStudentIdNotExistingStudentClassbook_ShouldReturnValidationError()
        {
            //Arrange
            var dashbordAnaliticRequstWithoutClassbook = new DashboardAnalyticsRequestDto<ClassbookResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
            };

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithoutClassbook = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, resultWithoutClassbook.Error.Code);
        }

        [Fact]
        public async Task GetStudentResult_ExistingStudentIdExistingStudentResult_ShouldReturnCurrentStudentResult()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
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

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(
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

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotEmpty(resultWithData.Data.AverageStudentsMarks);
            Assert.NotEmpty(resultWithData.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentResult_ExistingStudentIdNotExistingGroup_ShouldReturnEmptyStudentResult()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(new List<long> { studentIdWithoutGroup }, new List<long>()))
                .ReturnsAsync(new List<AverageStudentMarkDto>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(studentIdWithoutGroup, new List<long>()))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Empty(resultWithoutGroup.Data.AverageStudentsMarks);
            Assert.Empty(resultWithoutGroup.Data.AverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentResult_NotExistingStudentId_ShouldReturnValidationError()
        {
            //Arrange
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

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStrangerStudent = GetCurrentUserAsExistingStudent(entityId: long.MaxValue);

            var dashbordServiceWithStrangerCredentials = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStrangerStudent.Object);

            //Act
            var resultWithDataWithWrongClaim = await dashbordServiceWithStrangerCredentials.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, resultWithDataWithWrongClaim.Error.Code);
        }

        [Fact]
        public async Task GetStudentResult_ExistingStudentIdNotExistingStudentResult_ShouldReturnValidationError()
        {
            //Arrange
            var dashbordAnaliticRequstWithoutClassbook = new DashboardAnalyticsRequestDto<StudentResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
            };

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithoutClassbook = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, resultWithoutClassbook.Error.Code);
        }

        [Fact]
        public async Task GetStudentGroupResult_ExistingGroupIdExistingStudentIds_ShouldReturnCurrentStudentsGroupResults()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentGroupMarkDto>
                {
                    new AverageStudentGroupMarkDto()
                    {
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageMark = (decimal)4.5
                    }
                });

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(new List<long> { 1 }))
                .ReturnsAsync(new List<AverageStudentGroupVisitDto>
                {
                    new AverageStudentGroupVisitDto()
                    {
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageVisitPercentage = 15
                    }
                });

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            Assert.NotEmpty(requestWithData.Data.AverageStudentGroupsMarks);
            Assert.NotEmpty(requestWithData.Data.AverageStudentGroupsVisits);
        }

        [Fact]
        public async Task GetStudentGroupResult_ExistingStudentIdsNotExistingGroup_ShouldReturnEmptyStudentsResults()
        {
            //Arrange
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

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseIdWitoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());


            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long>()))
                .ReturnsAsync(new List<AverageStudentGroupMarkDto>());

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(new List<long>()))
                .ReturnsAsync(new List<AverageStudentGroupVisitDto>());

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithoutGroupOnCourse = await dashbordService.GetStudentGroupResultAsync(courseIdWitoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsMarks);
            Assert.Empty(requestWithoutGroupOnCourse.Data.AverageStudentGroupsVisits);
        }

        [Fact]
        public async Task GetStudentGroupResult()
        {
            //Arrang
            var dashbordAnaliticRequstWithoutData = new DashboardAnalyticsRequestDto<StudentGroupResultType>()
            {
                StartDate = new DateTime(2011, 1, 1),
                FinishDate = new DateTime(2017, 5, 20),
            };

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithoutData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithoutData);

            //Assert
            Assert.Equal(ErrorCode.ValidationError, requestWithoutData.Error.Code);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }

    }
}
