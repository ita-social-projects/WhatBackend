using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class StudentGroupServiceTest : TestBase
    {
        private readonly Mock<ILogger<StudentGroupService>> _loggerMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IScheduleService> _scheduleServiceMock;
        private readonly Mock<ILessonRepository> _lessonRepositoryMock;
        private readonly Mock<IEventOccurrenceRepository> _eventOccurrenceRepositoryMock;
        private readonly IMapper _mapper;
        private readonly StudentGroupService _studentGroupService;


        private readonly int _wrongId = -1;
        private readonly List<long> _ids = new List<long>() { 1, 2 };
        private readonly List<long> _dublicatedIds = new List<long>() { 1, 1 };
        private readonly DateTime _startDate = DateTime.Parse(_stringStartDate);
        private readonly DateTime _finishDate = DateTime.Parse(_stringFinishDate);

        private const int _id = 1;
        private const int _id2 = 2;
        private const string _emptyString = "";
        private const string _whitespaceString = "  ";
        private const string _nullString = null;
        private const string _name = "Test_name";
        private const string _name2 = "Test_name2";

        private const string _stringStartDate = "01.01.2000";
        private const string _stringFinishDate = "01.01.2020";

        #region Generate Arrange data

        private CreateStudentGroupDto GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate()
        {
            return new CreateStudentGroupDto()
            {
                Name = _name,
                CourseId = _id,
                StartDate = _startDate.Date,
                FinishDate = _finishDate.Date,
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };
        }
        
        private CreateStudentGroupDto GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds()
        {
            CreateStudentGroupDto studentGroup = GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            studentGroup.StudentIds = _ids;
            studentGroup.MentorIds = _ids;

            return studentGroup;
        }

        private UpdateStudentGroupDto GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate()
        {
            return new UpdateStudentGroupDto()
            {
                Name = _name,
                CourseId = _id,
                StartDate = _startDate.Date,
                FinishDate = _finishDate.Date,
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };
        }

        private UpdateStudentGroupDto GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds()
        {
            UpdateStudentGroupDto studentGroup = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            studentGroup.StudentIds = _ids;
            studentGroup.MentorIds = _ids;

            return studentGroup;
        }

        private StudentGroup GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds()
        {
            return new StudentGroup()
            {
                Id = _id,
                Name = _name,
                CourseId = _id,
                StartDate = _startDate,
                FinishDate = _finishDate,
                StudentsOfStudentGroups = new List<StudentOfStudentGroup>(),
                MentorsOfStudentGroups = new List<MentorOfStudentGroup>()
            };
        }

        private StudentGroup GetStudentGroupWithName2AndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds()
        {
            StudentGroup studentGroup = GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();
            studentGroup.Name = _name2;

            return studentGroup;
        }

        private List<StudentGroup> GetStudentGroupList()
        {
            return new List<StudentGroup>()
            {
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds(),
                GetStudentGroupWithName2AndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds()
            };
        }

        private static List<Student> _studentsWithIds = new List<Student>()
        {
            new Student()
            {
                Id = _id
            },
            new Student()
            {
                Id = _id2
            }
        };

        private static Course _courseWithIdAndName = new Course()
        {
            Id = _id,
            Name = _name
        };

        private MergeStudentGroupsDto GetMergeStudentGroupsDto()
        {
            return new MergeStudentGroupsDto()
            {
                ResultingStudentGroupId = _id,
                IdsOfStudentGroupsToMerge = _ids
            };
        }

        #endregion

        private void MockEntities()
        {
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository)
                .Returns(_studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentRepository)
                .Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository)
                .Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.CourseRepository)
                .Returns(_courseRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LessonRepository)
                .Returns(_lessonRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.EventOccurrenceRepository)
                .Returns(_eventOccurrenceRepositoryMock.Object);

            _studentGroupRepositoryMock.Setup(x => x.UpdateManyToMany(It.IsAny<IList<StudentOfStudentGroup>>(),
                                                                    It.IsAny<IList<StudentOfStudentGroup>>()));

            _mentorRepositoryMock.Setup(x => x.UpdateMentorGroups(It.IsAny<IList<MentorOfStudentGroup>>(),
                                                                  It.IsAny<IList<MentorOfStudentGroup>>()));
        }
        public StudentGroupServiceTest()
        {
            _loggerMock = new Mock<ILogger<StudentGroupService>>();
            _mapper = GetMapper(new ModelMappingProfile());
            _scheduleServiceMock = new Mock<IScheduleService>();

            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _lessonRepositoryMock = new Mock<ILessonRepository>();
            _eventOccurrenceRepositoryMock = new Mock<IEventOccurrenceRepository>();

            _studentGroupService = new StudentGroupService(
                _unitOfWorkMock.Object,
               _mapper,
               _loggerMock.Object,
               _scheduleServiceMock.Object);

            MockEntities();
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentIsOwnMentor_ReturnErrorCodeConflict()
        {
            //Arrange
            var newStudentGroup = 
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            var studentsWithIdsAccountIds = new List<Student>()
            {
                new Student()
                {
                    Id = _id,
                    AccountId = _id
                },
                new Student()
                {
                    Id = _id,
                    AccountId = _id
                }
            };

            var mentorsWithIdsAndAccountIds = new List<Mentor>()
            {
                new Mentor()
                {
                    Id = _id,
                    AccountId = _id
                },
                new Mentor()
                {
                    Id = _id,
                     AccountId = _id
                }
            };

            _studentRepositoryMock.Setup(x => x
                .GetStudentsByIdsAsync(newStudentGroup.StudentIds))
                .ReturnsAsync(studentsWithIdsAccountIds);

            _mentorRepositoryMock.Setup(x => x
                .GetMentorsByIdsAsync(newStudentGroup.MentorIds))
                .ReturnsAsync(mentorsWithIdsAndAccountIds);

            _studentGroupRepositoryMock.Setup(x => x
                    .IsGroupNameExistAsync(newStudentGroup.Name))
                    .ReturnsAsync(false);

            _courseRepositoryMock.Setup(x => x
                    .IsEntityExistAsync(newStudentGroup.CourseId))
                    .ReturnsAsync(true);

            //Act
            var result = await _studentGroupService
                    .CreateStudentGroupAsync(newStudentGroup);

            //Accert
            result.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroup_ShouldReturnStudentGroup()
        {
            //Arrange
            var newStudentGroup =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            var mentorsWithIds = new List<Mentor>()
            {
                new Mentor()
                {
                    Id = _id
                },
                new Mentor()
                {
                    Id = _id2
                }
            };

            _studentRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(newStudentGroup.StudentIds))
               .ReturnsAsync(_studentsWithIds);

            _mentorRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(newStudentGroup.MentorIds))
                .ReturnsAsync(mentorsWithIds);

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(newStudentGroup.Name))
                    .ReturnsAsync(false);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(newStudentGroup.CourseId))
                                .ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(newStudentGroup.StudentIds))
                                  .ReturnsAsync(new List<long>());

            //Act
            var successResult = await _studentGroupService.CreateStudentGroupAsync(newStudentGroup);

            //Assert
            successResult.Data
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(newStudentGroup);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_ExistingStudentGroup_ShouldReturnUnprocessableEntityError()
        {
            //Arrange
            var existingStudentGroup =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();
            
            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(existingStudentGroup.Name))
                   .ReturnsAsync(true);

            //Act
            var groupNameExistResult = await _studentGroupService.CreateStudentGroupAsync(existingStudentGroup);

            //Assert
            groupNameExistResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.UnprocessableEntity);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_NullStudentGroup_ShouldReturnValidationError()
        {
            //Act
            var nullGroupResult = await _studentGroupService
                .CreateStudentGroupAsync(null);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithoutCourseID_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutCourseID =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            //Act
            var withNotExistCoursIdResult = await _studentGroupService
                .CreateStudentGroupAsync(studentGroupWithoutCourseID);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithWrongDates_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithWrongDates = GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            studentGroupWithWrongDates.StartDate = DateTime.Parse(_stringFinishDate);
            studentGroupWithWrongDates.FinishDate = DateTime.Parse(_stringStartDate);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(studentGroupWithWrongDates.CourseId))
                .ReturnsAsync(true);

            //Act
            var withNotExistCoursIdResult = await _studentGroupService
                .CreateStudentGroupAsync(studentGroupWithWrongDates);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithDublicatesStudentsIds_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithWrongDates = GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            studentGroupWithWrongDates.StudentIds = _dublicatedIds;

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(studentGroupWithWrongDates.CourseId))
                .ReturnsAsync(true);

            //Act
            var withNotExistCoursIdResult = await _studentGroupService
                .CreateStudentGroupAsync(studentGroupWithWrongDates);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }
        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithDublicatesMentorsIds_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithWrongDates = GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            studentGroupWithWrongDates.MentorIds = _dublicatedIds;

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(studentGroupWithWrongDates.CourseId))
                .ReturnsAsync(true);

            //Act
            var withNotExistCoursIdResult = await _studentGroupService
                .CreateStudentGroupAsync(studentGroupWithWrongDates);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_ServerException_ShouldReturnInternalServerError()
        {
            //Arrange
            var studentGroup =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(studentGroup.Name))
                .Throws(new InvalidOperationException());

            //Act
            var withNotExistCoursIdResult = await _studentGroupService.CreateStudentGroupAsync(studentGroup);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithoutValidStudentsIds_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutValidStudentIDs =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                                      .ReturnsAsync(new List<long> { _id });

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(studentGroupWithoutValidStudentIDs.CourseId))
                .ReturnsAsync(true);

            //Act
            var withNotExistStrudentIdResult = await _studentGroupService.CreateStudentGroupAsync(studentGroupWithoutValidStudentIDs);

            //Assert
            withNotExistStrudentIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithoutValidMentorsIds_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutMentorValidIds =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _mentorRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutMentorValidIds.StudentIds))
                                      .ReturnsAsync(new List<long> { _id });

            _studentRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(studentGroupWithoutMentorValidIds.StudentIds))
                .ReturnsAsync(_studentsWithIds);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(studentGroupWithoutMentorValidIds.CourseId))
                .ReturnsAsync(true);

            //Act
            var withNotExistStrudentIdResult = await _studentGroupService.CreateStudentGroupAsync(studentGroupWithoutMentorValidIds);

            //Assert
            withNotExistStrudentIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupwithoutMentorsAndStudents_ShouldReturnrCountNull()
        {
            //Arrange
            var withoutMentorsAndStudentsStudentGroup = GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();

            _studentRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(withoutMentorsAndStudentsStudentGroup.StudentIds))
              .ReturnsAsync(new List<Student>());

            _mentorRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(withoutMentorsAndStudentsStudentGroup.MentorIds))
                .ReturnsAsync(new List<Mentor>());

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(withoutMentorsAndStudentsStudentGroup.CourseId))
                                .ReturnsAsync(true);

            //Act
            var withoutMentorsAndStudentsGroupResult = await _studentGroupService.CreateStudentGroupAsync(withoutMentorsAndStudentsStudentGroup);

            //Assert
            (withoutMentorsAndStudentsGroupResult.Data.StudentIds.Count == 0)
                .Should()
                .BeTrue();

            (withoutMentorsAndStudentsGroupResult.Data.MentorIds.Count == 0)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_ValidDataPassed_ShouldReturnNotNull()
        {
            //Arrange
            var updateStudentGroupDto = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            updateStudentGroupDto.MentorIds = _ids;

            var existingStudentGroup =
                GetStudentGroupWithName2AndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
               .ReturnsAsync(existingStudentGroup);

            var existingCourse = _courseWithIdAndName;

            _courseRepositoryMock.Setup(x => x.GetByIdAsync(updateStudentGroupDto.CourseId))
                .ReturnsAsync(existingCourse);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(updateStudentGroupDto.CourseId))
                .ReturnsAsync(true);

            //Act
            var successResult = await _studentGroupService.UpdateStudentGroupAsync(_id, updateStudentGroupDto);

            //Assert
            successResult.Data
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_NotExistingGroupId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingGroupId = -_id;
            var updateStudentGroupDto = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(notExistingGroupId))
                  .ReturnsAsync((StudentGroup)null);

            _courseRepositoryMock.Setup(x => x.GetByIdAsync(updateStudentGroupDto.CourseId))
             .ReturnsAsync(_courseWithIdAndName);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(updateStudentGroupDto.CourseId))
                                .ReturnsAsync(true);

            //Act
            var groupNotExistResult = await _studentGroupService.UpdateStudentGroupAsync(notExistingGroupId, updateStudentGroupDto);

            //Assert
            groupNotExistResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_NullGroup_ShouldReturnValidationError()
        {
            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, null);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_NonExistentCourseId_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutCourseID =
                GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            var existingStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
               .ReturnsAsync(existingStudentGroup);

            //Act
            var withNotExistCoursIdResult = await _studentGroupService
                .UpdateStudentGroupAsync(_id, studentGroupWithoutCourseID);

            //Assert
            withNotExistCoursIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_NotValidStudentIDs_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutValidStudentIDs =
                GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                                     .ReturnsAsync(_ids);

            var existingStudentGroup =
              GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
               .ReturnsAsync(existingStudentGroup);

            var newStudentGroup =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _courseRepositoryMock.Setup(x => x
                   .IsEntityExistAsync(newStudentGroup.CourseId))
                   .ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                .ReturnsAsync(_ids);

            //Act
            var withNotExistStrudentIdResult = await _studentGroupService
                .UpdateStudentGroupAsync(existingStudentGroup.Id, studentGroupWithoutValidStudentIDs);

            //Assert
            withNotExistStrudentIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_GroupNameIsNull_ShouldReturnValidationError()
        {
            //Assert
            var existingStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
                .ReturnsAsync(existingStudentGroup);

            //Act
            var groupNameIsNullResult = await _studentGroupService
                .UpdateStudentGroupAsync(_id, new UpdateStudentGroupDto() { Name = null });

            //Assert
            groupNameIsNullResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_GroupNameIsEmpty_ShouldReturnValidationError()
        {
            //Assert
            var existingStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
                .ReturnsAsync(existingStudentGroup);

            //Act
            var groupNameIsEmptyResult = await _studentGroupService
                .UpdateStudentGroupAsync(_id, new UpdateStudentGroupDto() { Name = string.Empty });

            //Assert
            groupNameIsEmptyResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_NotExistingMenttorsIs_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutValidStudentIDs =
                GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                                     .ReturnsAsync(_ids);

            var existingStudentGroup =
              GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
               .ReturnsAsync(existingStudentGroup);

            var newStudentGroup =
                GetCreateStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _courseRepositoryMock.Setup(x => x
                   .IsEntityExistAsync(newStudentGroup.CourseId))
                   .ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                .ReturnsAsync(new List<long> { });

            //_unitOfWork.MentorRepository.GetNotExistEntitiesIdsAsync
            _mentorRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.MentorIds))
                .ReturnsAsync(_ids);

            //Act
            var withNotExistStrudentIdResult = await _studentGroupService
                .UpdateStudentGroupAsync(existingStudentGroup.Id, studentGroupWithoutValidStudentIDs);

            //Assert
            withNotExistStrudentIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_ServerException_ShouldReturnInternalServerError()
        {
            //Arrange
            var anyUpdateGroup = new UpdateStudentGroupDto()
            {
                CourseId = _wrongId
            };

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                     .Throws(new InvalidOperationException());

            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, anyUpdateGroup);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }


        [Fact]
        public async Task UpdateStudentGroupAsync_DublicateGroupName_ShouldReturnUnprocessableEntityError()
        {
            //Arrange
            var validUpdateGroup = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            var studentGroup = GetStudentGroupWithName2AndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                     .ReturnsAsync(studentGroup);

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(validUpdateGroup.Name))
                .ReturnsAsync(true);


            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, validUpdateGroup);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.UnprocessableEntity);
        }


        [Fact]
        public async Task UpdateStudentGroupAsync_DublicatedStudents_ShouldReturnValidationError()
        {
            //Arrange
            var validUpdateGroup = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            validUpdateGroup.StudentIds = _dublicatedIds;

            var studentGroup = GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                     .ReturnsAsync(studentGroup);

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(validUpdateGroup.Name))
                .ReturnsAsync(false);

            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, validUpdateGroup);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_DublicatedMentors_ShouldReturnValidationError()
        {
            //Arrange
            var validUpdateGroup = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            validUpdateGroup.MentorIds = _dublicatedIds;

            var studentGroup = GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                     .ReturnsAsync(studentGroup);

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(validUpdateGroup.Name))
                .ReturnsAsync(false);

            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, validUpdateGroup);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroupAsync_StartDateMoreFinishDate_ShouldReturnValidationError()
        {
            //Arrange
            var validUpdateGroup = GetUpdateStudentGroupWithNameAndCourseIdAndStartDateAndEndDate();
            validUpdateGroup.StartDate = _finishDate;
            validUpdateGroup.FinishDate = _startDate;

            var studentGroup = GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                                     .ReturnsAsync(studentGroup);

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(validUpdateGroup.Name))
                .ReturnsAsync(false);

            //Act
            var nullGroupResult = await _studentGroupService.UpdateStudentGroupAsync(_id, validUpdateGroup);

            //Assert
            nullGroupResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Theory]
        [InlineData(_emptyString)]
        [InlineData(_whitespaceString)]
        [InlineData(_nullString)]
        public async Task IsGroupNameExistAsync_WrongNameString_ShouldReturnValidationError(string name)
        {
            //Act
            var wrongStringResult = await _studentGroupService.IsGroupNameExistAsync(name);

            //Assert
            wrongStringResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Theory]
        [InlineData(_name, true)]
        [InlineData(_name2, false)]
        public async Task IsGroupNameExistAsync_ValidData_ShouldReturnBoolValue(string name, bool expectedResult)
        {
            //Arrange
            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(_name))
                                     .ReturnsAsync(true);

            //Act
            var nullGroupResult = await _studentGroupService.IsGroupNameExistAsync(name);

            //Assert
            nullGroupResult.Data
                .Should()
                .Be(expectedResult);
        }

        [Fact]
        public async Task GetStudentGroupsByDateAsync_StartDateBiggerFinishDate_ShouldReturnValidationError()
        {
            //Act
            var wrongDatesResult = await _studentGroupService
                .GetStudentGroupsByDateAsync(_finishDate, _startDate);

            //Assert
            wrongDatesResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentGroupsByDateAsync_ValidData_ShouldReturnListStudentGroupDto()
        {
            //Arrange
            var studentGroupList = GetStudentGroupList();

            _studentGroupRepositoryMock.Setup(x => x.GetAllActiveAsync(_startDate, _finishDate))
                .ReturnsAsync(studentGroupList);

            //Act
            var validResult = await _studentGroupService
                .GetStudentGroupsByDateAsync(_startDate, _finishDate);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<StudentGroupDto>>(studentGroupList));
        }


        public static readonly object[][] CorrectData =
        {
            new object[] { null, DateTime.Parse(_stringFinishDate)},
            new object[] { DateTime.Parse(_stringStartDate), null},
            new object[] { null, null}
        };
        [Theory, MemberData(nameof(CorrectData))]
        public async Task GetAllStudentGroupsAsync_CorrectDate_ShouldReturnListStudentGroupDto(DateTime? startDate, DateTime? finishDate)
        {
            //Arrange
            var studentGroupList = GetStudentGroupList();

            _studentGroupRepositoryMock.Setup(x => x.GetAllActiveAsync(startDate, finishDate))
                .ReturnsAsync(studentGroupList);

            //Act
            var validResult = await _studentGroupService.GetAllStudentGroupsAsync(startDate, finishDate);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<StudentGroupDto>>(studentGroupList));
        }

        [Fact]
        public async Task DeleteStudentGroupAsync_ValidData_ShouldReturnFalse()
        {
            //Arrange
            _lessonRepositoryMock.Setup(x => x.GetAllLessonsForStudentGroup(_id))
                .ReturnsAsync(new List<Lesson>() { });

            //Act
            var validResult = await _studentGroupService.DeleteStudentGroupAsync(_id);

            //Assert
            validResult
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task DeleteStudentGroupAsync_ValidData_ShouldReturnTrue()
        {
            //Arrange
            _lessonRepositoryMock.Setup(x => x.GetAllLessonsForStudentGroup(_id))
                .ReturnsAsync(new List<Lesson>() { 
                    new Lesson() { 
                        Id = _id                        
                    } });


            var eventOccurrenceResult = new Result<IList<EventOccurrenceDTO>>()
            {
                Data = new List<EventOccurrenceDTO>()
                {
                    new EventOccurrenceDTO()
                    {
                        Id = _id
                    }
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.DeactivateStudentGroupAsync(_id))
                .ReturnsAsync(true);

            _scheduleServiceMock.Setup(x => x.GetEventOccurrencesByGroupIdAsync(_id))
                .Returns(Task.FromResult(eventOccurrenceResult));

            //Act
            var validResult = await _studentGroupService.DeleteStudentGroupAsync(_id);

            //Assert
            validResult
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task GetStudentGroupByIdAsync_NotExistId_ShouldReturnNotFoundError()
        {
            //Arrange
            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_wrongId))
                .ReturnsAsync(() => null);

            //Act
            var validResult = await _studentGroupService.GetStudentGroupByIdAsync(_wrongId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetStudentGroupByIdAsync_ValidData_ShouldReturnStudentGroupDto()
        {
            //Arrange
            var validStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_id))
                .ReturnsAsync(validStudentGroup);

            //Act
            var validResult = await _studentGroupService.GetStudentGroupByIdAsync(_id);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<StudentGroupDto>(validStudentGroup));
        }

        [Fact]
        public async Task MergeStudentGroupsAsync_NotExistingGroupId_ShouldReturnNotFoundError()
        {
            //Arrange
            var studentGroupWithNotExistingGroupId = GetMergeStudentGroupsDto();

            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_wrongId))
               .ReturnsAsync(() => null);

            //Act
            var validResult = await _studentGroupService.MergeStudentGroupsAsync(studentGroupWithNotExistingGroupId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task MergeStudentGroupsAsync_NotExistingGroupsToMerge_ShouldReturnNotFoundError()
        {
            //Arrange
            var validStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            var studentGroupWithNotExistingMergeGroupId = GetMergeStudentGroupsDto();

            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_id))
                .ReturnsAsync(validStudentGroup);

            //Act
            var validResult = await _studentGroupService.MergeStudentGroupsAsync(studentGroupWithNotExistingMergeGroupId);

            //Assert
            validResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task MergeStudentGroupsAsync_ValidData_ShouldReturnStudentGroupDto()
        {
            //Arrange
            var validStudentGroup =
                GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();
            var studentGroupWithMergeGroupId = GetMergeStudentGroupsDto();
            var studentGroup = GetStudentGroupWithNameAndCourseIdAndStartDateAndEndDateAndStudentsIdsAndMentorIds();

            _lessonRepositoryMock.Setup(x => x.GetAllLessonsForStudentGroup(_id))
                .ReturnsAsync(new List<Lesson>() { });

            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_id))
                .ReturnsAsync(validStudentGroup);

            _studentGroupRepositoryMock.Setup(x => x.GetActiveStudentGroupByIdAsync(_id2))
               .ReturnsAsync(validStudentGroup);

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(_id))
                .ReturnsAsync(studentGroup);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(_id))
                .ReturnsAsync(true);

            //Act
           var validResult = await _studentGroupService.MergeStudentGroupsAsync(studentGroupWithMergeGroupId);

            //Assert
            validResult.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<StudentGroupDto>(validStudentGroup));
        }
    }
}
