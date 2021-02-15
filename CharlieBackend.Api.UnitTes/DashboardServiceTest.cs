using System;
using AutoMapper;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;

namespace CharlieBackend.Api.UnitTest
{
    public class DashboardServiceTest : TestBase
    {
        private readonly Mock<IDashboardRepository> _dashboardRepositoryMock;
        private readonly IMapper _mapper;
        private static long studentIdWithGroup = 7;
        private static long studentIdWithoutGroup = 20;
        private static long courseId = 1;
        private static long courseIdWithoutGroup = 2;

        public DashboardServiceTest()
        {
            _dashboardRepositoryMock = new Mock<IDashboardRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        private void InitializeForDashboardRepository()
        {
            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();
        }

        [Fact]
        public async Task GetStudentsClassbook_ValidDataPassed_ShouldReturnCurrentStudentsMarks()
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

            var expectedStudentsMarks = new List<StudentMarkDto>
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
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
                .ReturnsAsync(expectedStudentsMarks);

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);

            //Assert
            successResult.Data.StudentsMarks.Should().NotBeNull();
            successResult.Data.StudentsMarks.Should().BeEquivalentTo(expectedStudentsMarks);
        }

        [Fact]
        public async Task GetStudentsClassbook_ValidDataPassed_ShouldReturnCurrentStudentsPresence()
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

            var expectedStudentsVisits = new List<StudentVisitDto>
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
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
               new List<long> { 2 }, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
               .ReturnsAsync(expectedStudentsVisits);

            InitializeForDashboardRepository();

            var dashboardService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var successResult = await dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);

            //Assert
            successResult.Data.StudentsPresences.Should().NotBeNull();
            successResult.Data.StudentsPresences.Should().BeEquivalentTo(expectedStudentsVisits);
        }

        [Fact]
        public async Task GetStudentsClassbook_NotExistingCourseId_ShouldReturnValidationError()
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
            requestWithWrongParameters.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentsClassbook_NotExistingGroupId_ShouldReturnEmptyStudentPresences()
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
            requestForCourseWithoutStudents.Data.StudentsPresences.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentsClassbook_NotExistingGroupId__ShouldReturnEmptyStudentMarks()
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
            requestForCourseWithoutStudents.Data.StudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentsResult_ExistingResultsForStudents_ShouldReturnStudentsAverageMarks()
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

            var expectedStudentsMarks = new List<AverageStudentMarkDto>
            {
                new AverageStudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageMark = (decimal)5.1
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
               .ReturnsAsync(expectedStudentsMarks);

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);

            //Assert
            resultWithData.Data.AverageStudentsMarks.Should().NotBeNull();
            resultWithData.Data.AverageStudentsMarks.Should().Equal(expectedStudentsMarks);
        }

        [Fact]
        public async Task GetStudentsResult_ExisitingResultsForStudents_ShouldReturnStudentsAverageVisits()
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

            var expectedAverageStudentVisits = new List<AverageStudentVisitsDto>
            {
                new AverageStudentVisitsDto()
                    {
                        CourseId = 5,
                        StudentGroupId = 2,
                        StudentId = 6,
                        StudentAverageVisitsPercentage = 15
                    }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithData.CourseId, studentResultRequestWithData.StartDate, studentResultRequestWithData.FinishDate))
                .ReturnsAsync(new List<long>() { 2 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(new List<long>() { 2 }))
                .ReturnsAsync(new List<long>() { 6, 7, 8, 9, 10 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(
                 new List<long>() { 6, 7, 8, 9, 10 }, new List<long>() { 2 }))
                .ReturnsAsync(expectedAverageStudentVisits);

            InitializeForDashboardRepository();

            var dashbordservice = new DashboardService(_unitOfWorkMock.Object, _currentUserServiceMock.Object);

            //Act
            var resultWithData = await dashbordservice.GetStudentsResultAsync(studentResultRequestWithData);

            //Assert
            resultWithData.Data.AverageStudentVisits.Should().NotBeNull();
            resultWithData.Data.AverageStudentVisits.Should().Equal(expectedAverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentsResult_NotExistingGroupIdNotExistingStudents_ShouldReturnValidationError()
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
            resultWithWrongData.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentsResult_NotExistingCourseId_ShouldReturnEmptyStudentsAverageMarks()
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
            resultWithoutStudent.Data.AverageStudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentsResult_NotExistingCourseId_ShouldReturnEmptyStudentsAverageVisits()
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
            resultWithoutStudent.Data.AverageStudentVisits.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentClassbook_ValidDataPassed_ShouldReturnCurrentStudentMarks()
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

            var expectedStudentMarks = new List<StudentMarkDto>
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
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(
                studentIdWithGroup, new List<long> { 1 }))
                .ReturnsAsync(expectedStudentMarks);

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.StudentsMarks.Should().NotBeNull();
            resultWithData.Data.StudentsMarks.Should().Equal(expectedStudentMarks);
        }

        [Fact]
        public async Task GetStudentClassbook_ValidDataPassed_ShouldReturnCurrentStudentVisits()
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

            var expectedStudentVisits = new List<StudentVisitDto>
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
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(
                studentIdWithGroup, new List<long>() { 1 }))
                .ReturnsAsync(expectedStudentVisits);

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.StudentsPresences.Should().NotBeNull();
            resultWithData.Data.StudentsPresences.Should().Equal(expectedStudentVisits);
        }

        [Fact]
        public async Task GetStudentClassbook_NotExistingGroup_ShouldReturnEmptyStudentMarks()
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

            _dashboardRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(studentIdWithoutGroup, new List<long> { }))
               .ReturnsAsync(new List<StudentMarkDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithoutGroup.Data.StudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentClassbook_NotExistingGroup_ShouldReturnEmptyStudentResult()
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


            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithoutGroup.Data.StudentsPresences.Should().BeNullOrEmpty();
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
            requestWithStrangerCredentials.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentClassbook_NotExistingStudentClassbook_ShouldReturnValidationError()
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
            resultWithoutClassbook.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentResult_ValidDataPassed_ShouldReturnCurrentStudentAverageMarks()
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

            var expectedAverageStudentMark = new List<AverageStudentMarkDto>
            {
                new AverageStudentMarkDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        StudentAverageMark = (decimal)5.1
                    }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
                new List<long> { studentIdWithGroup }, new List<long> { 1 }))
                .ReturnsAsync(expectedAverageStudentMark);

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.AverageStudentsMarks.Should().NotBeNull();
            resultWithData.Data.AverageStudentsMarks.Should().BeEquivalentTo(expectedAverageStudentMark);
        }

        [Fact]
        public async Task GetStudentResult_ValidDataPassed_ShouldReturnCurrentStudentAverageVisits()
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

            var expectedAverageStudentVisits = new List<AverageStudentVisitsDto>
            {
                 new AverageStudentVisitsDto()
                    {
                        CourseId = 2,
                        StudentGroupId = 1,
                        StudentId = 5,
                        StudentAverageVisitsPercentage = 15
                    }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(
                studentIdWithGroup, new List<long> { 1 }))
                .ReturnsAsync(expectedAverageStudentVisits);

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);

            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.AverageStudentVisits.Should().NotBeNull();
            resultWithData.Data.AverageStudentVisits.Should().BeEquivalentTo(expectedAverageStudentVisits);
        }

        [Fact]
        public async Task GetStudentResult_NotExistingGroup_ShouldReturnEmptyStudentMarks()
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

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithoutGroup.Data.AverageStudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentResult_NotExistingGroup_ShouldReturnEmptyStudentVisits()
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

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(studentIdWithoutGroup, new List<long>()))
                .ReturnsAsync(new List<AverageStudentVisitsDto>());

            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);

            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentResultAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithoutGroup.Data.AverageStudentVisits.Should().BeNullOrEmpty();
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
            resultWithDataWithWrongClaim.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentResult_NotExistingStudentResult_ShouldReturnValidationError()
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
            resultWithoutClassbook.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentGroupResult_ValidDataPassed_ShouldReturnCurrentStudentsAverageGroupMark()
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

            var expectedAverageStudentGroupMark = new List<AverageStudentGroupMarkDto>
            {
                new AverageStudentGroupMarkDto()
                    {
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageMark = (decimal)4.5
                    }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(new List<long> { 1 }))
                .ReturnsAsync(expectedAverageStudentGroupMark);

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            requestWithData.Data.AverageStudentGroupsMarks.Should().NotBeNull();
            requestWithData.Data.AverageStudentGroupsMarks.Should().BeEquivalentTo(expectedAverageStudentGroupMark);
        }

        [Fact]
        public async Task GetStudentGroupResult_ValidDataPassed_ShouldReturnCurrentStudentsGroupAverageVisits()
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

            var expectedAverageGroupVisits = new List<AverageStudentGroupVisitDto>
            {
                new AverageStudentGroupVisitDto()
                    {
                        CourseId = 1,
                        StudentGroupId = 1,
                        AverageVisitPercentage = 15
                    }
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long> { 1 });

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(new List<long> { 1 }))
                .ReturnsAsync(expectedAverageGroupVisits);

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requestWithData = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            requestWithData.Data.AverageStudentGroupsVisits.Should().NotBeNull();
            requestWithData.Data.AverageStudentGroupsVisits.Should().BeEquivalentTo(expectedAverageGroupVisits);
        }

        [Fact]
        public async Task GetStudentGroupResult_NotExistingGroup_ShouldReturnEmptyStudentsResults()
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
                courseIdWithoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
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
            var requestWithoutGroupOnCourse = await dashbordService.GetStudentGroupResultAsync(courseIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            requestWithoutGroupOnCourse.Data.AverageStudentGroupsMarks.Should().BeNullOrEmpty();
            requestWithoutGroupOnCourse.Data.AverageStudentGroupsVisits.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentGroupResult_NotExistingAnalitics_ShouldReturnValidationError()
        {
            //Arrange
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
            requestWithoutData.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentGroupResult_NullStudentGroupId_ShouldReturnNotFound()
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
                courseIdWithoutGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(new List<long>());

            InitializeForDashboardRepository();

            var dashbordService = new DashboardService(
                unitOfWork: _unitOfWorkMock.Object,
                currentUserService: _currentUserServiceMock.Object);

            //Act
            var requesWithoutStudentGroupId = await dashbordService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            requesWithoutStudentGroupId.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }

    }
}
