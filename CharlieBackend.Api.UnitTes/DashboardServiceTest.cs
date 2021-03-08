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
        private readonly DashboardService _dashboardService;
        private readonly IMapper _mapper;
        private static long studentIdWithGroup = 7;
        private static long studentIdWithoutGroup = 20;
        private static long courseId = 1;
        private static long courseIdWithoutGroup = 2;

        public DashboardServiceTest()
        {
            _dashboardRepositoryMock = new Mock<IDashboardRepository>();
            _mapper = GetMapper(new ModelMappingProfile());
            _unitOfWorkMock.Setup(x => x.DashboardRepository).Returns(_dashboardRepositoryMock.Object);
            _currentUserServiceMock = GetCurrentUserAsExistingStudent();

            _dashboardService = new DashboardService(_unitOfWorkMock.Object,
                                                     _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task GetStudentsClassbook_ValidDataPassed_ShouldReturnExpectedData()
        {
            //Arrange
            long expectedCourseId = 2;
            long expectedStudentGroupId = 1;
            long expectedStudentId = 5;
            long expectedLessonId = 2;

            var existentGroupIds = new List<long> { 2 };

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
                new StudentMarkDto
                        {
                            CourseId = expectedCourseId,
                            StudentGroupId = expectedStudentGroupId,
                            StudentId = expectedStudentId,
                            LessonId = expectedLessonId,
                            LessonDate = new DateTime(2015,4,12),
                            StudentMark = 5
                        }
            };

            var expectedStudentsVisits = new List<StudentVisitDto>
            {
                new StudentVisitDto()
                    {
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        LessonId = expectedLessonId,
                        LessonDate = new DateTime(2015,4,12),
                        Presence = true
                    }
            };

            var expectedStudentsClassbookResult = new StudentsClassbookResultDto()
            {
                StudentsMarks = expectedStudentsMarks,
                StudentsPresences = expectedStudentsVisits
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                studentclassbookRequestWithData.CourseId.Value,
                studentclassbookRequestWithData.StartDate,
                studentclassbookRequestWithData.FinishDate)).ReturnsAsync(existentGroupIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentsMarksListByGroupIdsAndDate(
                existentGroupIds, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
                .ReturnsAsync(expectedStudentsMarks);

            _dashboardRepositoryMock.Setup(x => x.GetStudentsPresenceListByGroupIdsAndDate(
               existentGroupIds, studentclassbookRequestWithData.StartDate, studentclassbookRequestWithData.FinishDate))
               .ReturnsAsync(expectedStudentsVisits);

            //Act
            var successResult = await _dashboardService.GetStudentsClassbookAsync(studentclassbookRequestWithData);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(expectedStudentsClassbookResult);
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

            //Act
            var requestWithWrongParameters = await _dashboardService.GetStudentsClassbookAsync(studentClassbookWrongRequest);

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

            //Act
            var requestForCourseWithoutStudents = await _dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

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

            //Act
            var requestForCourseWithoutStudents = await _dashboardService.GetStudentsClassbookAsync(studentclassbookCourseWithoutStudents);

            //Assert
            requestForCourseWithoutStudents.Data.StudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentsResult_ValidDataPassed_ShouldReturnExpectedData()
        {
            //Arrange
            long expectedCourseId = 2;
            long expectedStudentGroupId = 1;
            long expectedStudentId = 5;

            var existentStudentIds = new List<long>() { 6, 7, 8, 9, 10 };
            var existentGroupIds = new List<long>() { 2 };

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
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        StudentAverageMark = (decimal)5.1
                    }
            };

            var expectedAverageStudentVisits = new List<AverageStudentVisitsDto>
            {
                new AverageStudentVisitsDto()
                    {
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        StudentAverageVisitsPercentage = 15
                    }
            };


            var expectedStudentsResult = new StudentsResultsDto()
            {
                AverageStudentVisits = expectedAverageStudentVisits,
                AverageStudentsMarks = expectedStudentsMarks
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                (long)studentResultRequestWithData.CourseId,
                studentResultRequestWithData.StartDate,
                studentResultRequestWithData.FinishDate))
                .ReturnsAsync(existentGroupIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentsIdsByGroupIdsAsync(existentGroupIds))
                .ReturnsAsync(existentStudentIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
              existentStudentIds, existentGroupIds))
               .ReturnsAsync(expectedStudentsMarks);

            _dashboardRepositoryMock.Setup(x => x.GetStudentsAverageVisitsByStudentIdsAndGroupsIdsAsync(
                 existentStudentIds, existentGroupIds))
                .ReturnsAsync(expectedAverageStudentVisits);

            //Act
            var resultWithData = await _dashboardService.GetStudentsResultAsync(studentResultRequestWithData);

            //Assert
            resultWithData.Data.Should().NotBeNull();
            resultWithData.Data.Should().BeEquivalentTo(expectedStudentsResult);
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

            //Act
            var resultWithWrongData = await _dashboardService.GetStudentsResultAsync(studentResultWrongRequest);

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

            //Act
            var resultWithoutStudent = await _dashboardService.GetStudentsResultAsync(studentResultRequestWithOutStudent);

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

            //Act
            var resultWithoutStudent = await _dashboardService.GetStudentsResultAsync(studentResultRequestWithOutStudent);

            //Assert
            resultWithoutStudent.Data.AverageStudentVisits.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentClassbook_ValidDataPassed_ShouldReturnExpectedData()
        {
            //Arrange
            long expectedCourseId = 2;
            long expectedStudentGroupId = 1;
            long expectedStudentId = 5;
            long expectedLessonId = 2;

            var existentGroupIds = new List<long>() { 1 };

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
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        LessonId = expectedLessonId,
                        LessonDate = new DateTime(2015,4,12),
                        StudentMark = 5
                    }
            };

            var expectedStudentVisits = new List<StudentVisitDto>
            {
                new StudentVisitDto()
                    {
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        LessonId = expectedLessonId,
                        LessonDate = new DateTime(2015,4,12),
                        Presence = true
                    }
            };

            var expectedStudentClassbook = new StudentsClassbookResultDto()
            {
                StudentsMarks = expectedStudentMarks,
                StudentsPresences = expectedStudentVisits
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(existentGroupIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentMarksListByStudentIds(
                studentIdWithGroup, existentGroupIds))
                .ReturnsAsync(expectedStudentMarks);

            _dashboardRepositoryMock.Setup(x => x.GetStudentPresenceListByStudentIds(
                studentIdWithGroup, existentGroupIds))
                .ReturnsAsync(expectedStudentVisits);

            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);
            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.Should().NotBeNull();
            resultWithData.Data.Should().BeEquivalentTo(expectedStudentClassbook);
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

            var currentUserServiceAsStudentWithoutGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithoutGroup);
            var dashbordServiceWithoutGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithoutGroup.Object);

            //Act
            var resultWithoutGroup = await dashbordServiceWithoutGroup.GetStudentClassbookAsync(studentIdWithoutGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithoutGroup.Data.StudentsMarks.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetStudentClassbook_NotExistingGroup_ShouldReturnEmptyStudentPresence()
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

            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);
            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithoutClassbook = await dashbordServiceWithGroup.GetStudentClassbookAsync(studentIdWithGroup, dashbordAnaliticRequstWithoutClassbook);

            //Assert
            resultWithoutClassbook.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentResult_ValidDataPassed_ShouldReturnExpectedData()
        {
            //Arrange
            long expectedCourseId = 1;
            long expectedStudentGroupId = 1;
            long expectedStudentId = 6;

            var existentGroupIds = new List<long>() { 1 };

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
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        StudentAverageMark = (decimal)5.1
                    }
            };

            var expectedAverageStudentVisits = new List<AverageStudentVisitsDto>
            {
                 new AverageStudentVisitsDto()
                    {
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        StudentId = expectedStudentId,
                        StudentAverageVisitsPercentage = 15
                    }
            };

            var expectedStudentResult = new StudentsResultsDto()
            {
                AverageStudentsMarks = expectedAverageStudentMark,
                AverageStudentVisits = expectedAverageStudentVisits
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByStudentIdAndPeriodAsync(
                studentIdWithGroup, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(existentGroupIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageMarksByStudentIdsAndGropsIdsAsync(
                new List<long> { studentIdWithGroup }, existentGroupIds))
                .ReturnsAsync(expectedAverageStudentMark);

            _dashboardRepositoryMock.Setup(x => x.GetStudentAverageVisitsPercentageByStudentIdsAsync(
                studentIdWithGroup, existentGroupIds))
                .ReturnsAsync(expectedAverageStudentVisits);

            var currentUserServiceAsStudentWithGroup = GetCurrentUserAsExistingStudent(entityId: studentIdWithGroup);
            var dashbordServiceWithGroup = new DashboardService(_unitOfWorkMock.Object, currentUserServiceAsStudentWithGroup.Object);

            //Act
            var resultWithData = await dashbordServiceWithGroup.GetStudentResultAsync(studentIdWithGroup, dashbordAnaliticRequstWithData);

            //Assert
            resultWithData.Data.Should().NotBeNull();
            resultWithData.Data.Should().BeEquivalentTo(expectedStudentResult);
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
            long expectedCourseId = 1;
            long expectedStudentGroupId = 1;

            var existentGroupIds = new List<long>() { 1 };

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
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        AverageMark = (decimal)4.5
                    }
            };

            var expectedAverageGroupVisits = new List<AverageStudentGroupVisitDto>
            {
                new AverageStudentGroupVisitDto()
                    {
                        CourseId = expectedCourseId,
                        StudentGroupId = expectedStudentGroupId,
                        AverageVisitPercentage = 15
                    }
            };

            var expectedStudentGroupResult = new StudentGroupsResultsDto()
            {
                AverageStudentGroupsMarks = expectedAverageStudentGroupMark,
                AverageStudentGroupsVisits = expectedAverageGroupVisits
            };

            _dashboardRepositoryMock.Setup(x => x.GetGroupsIdsByCourseIdAndPeriodAsync(
                courseId, dashbordAnaliticRequstWithData.StartDate, dashbordAnaliticRequstWithData.FinishDate))
                .ReturnsAsync(existentGroupIds);

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageMarks(existentGroupIds))
                .ReturnsAsync(expectedAverageStudentGroupMark);

            _dashboardRepositoryMock.Setup(x => x.GetStudentGroupsAverageVisits(existentGroupIds))
                .ReturnsAsync(expectedAverageGroupVisits);

            //Act
            var requestWithData = await _dashboardService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            requestWithData.Data.Should().NotBeNull();
            requestWithData.Data.Should().BeEquivalentTo(expectedStudentGroupResult);
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

            //Act
            var requestWithoutGroupOnCourse = await _dashboardService.GetStudentGroupResultAsync(courseIdWithoutGroup, dashbordAnaliticRequstWithData);

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

            //Act
            var requestWithoutData = await _dashboardService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithoutData);

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

            //Act
            var requesWithoutStudentGroupId = await _dashboardService.GetStudentGroupResultAsync(courseId, dashbordAnaliticRequstWithData);

            //Assert
            requesWithoutStudentGroupId.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }
    }
}
