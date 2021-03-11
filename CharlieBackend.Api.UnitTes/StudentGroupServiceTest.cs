using AutoMapper;
using CharlieBackend.Business.Services;
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
        private readonly IMapper _mapper;

        private readonly int id = 1;
        private readonly string name = "Test_name";
        private readonly List<long> ids = new List<long>() { 1, 2 };
        private readonly int date = 1;

        private StudentGroupService StudentGroupServiceMock()
        {
            return new StudentGroupService(
               _unitOfWorkMock.Object,
               _mapper,
               _loggerMock.Object
               );
        }
        private StudentGroup ExistingStudentGroup()
        {
            var existingStudentGroup = new StudentGroup()
            {
                Id = id,
                Name = name,
                CourseId = id,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(date),
                StudentsOfStudentGroups = new List<StudentOfStudentGroup>(),
                MentorsOfStudentGroups = new List<MentorOfStudentGroup>()
            };
            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
                .ReturnsAsync(existingStudentGroup);
            return existingStudentGroup;
        }
        private void MockEntities()
        {
            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);

            _studentGroupRepositoryMock.Setup(x => x.UpdateManyToMany(It.IsAny<IList<StudentOfStudentGroup>>(),
                                                                    It.IsAny<IList<StudentOfStudentGroup>>()));

            _mentorRepositoryMock.Setup(x => x.UpdateMentorGroups(It.IsAny<IList<MentorOfStudentGroup>>(),
                                                                  It.IsAny<IList<MentorOfStudentGroup>>()));
        }
        public StudentGroupServiceTest()
        {
            _loggerMock = new Mock<ILogger<StudentGroupService>>();
            _mapper = GetMapper(new ModelMappingProfile());

            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();

            MockEntities();
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroup_ShouldReturnStudentGroup()
        {
            //Arrange
            var newStudentGroup = new CreateStudentGroupDto()
            {
                Name = name,
                CourseId = id,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };

            _studentRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(newStudentGroup.StudentIds))
               .ReturnsAsync(new List<Student>()
               {
                    new Student { Id = 1 },
                    new Student { Id = 2 }
               });

            _mentorRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(newStudentGroup.MentorIds))
                .ReturnsAsync(new List<Mentor>()
                {
                    new Mentor { Id = 1 },
                    new Mentor { Id = 2 }
                });

            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(newStudentGroup.Name))
                    .ReturnsAsync(false);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(newStudentGroup.CourseId))
                                .ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(newStudentGroup.StudentIds))
                                  .ReturnsAsync(new List<long>());

            //Act
            var successResult = await StudentGroupServiceMock().CreateStudentGroupAsync(newStudentGroup);
            
            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(newStudentGroup);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_ExistingStudentGroup_ShouldReturnUnprocessableEntity()
        {
            //Arrange
            var existingStudentGroup = new CreateStudentGroupDto()
            {
                Name = "Exists_test_name",
                CourseId = id,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };
            _studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(existingStudentGroup.Name))
                   .ReturnsAsync(true);

            //Act
            var groupNameExistResult = await StudentGroupServiceMock().CreateStudentGroupAsync(existingStudentGroup);
            
            //Assert
            groupNameExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.UnprocessableEntity);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_NullStudentGroup_ShouldReturnValidationError()
        {
            //Act
            var nullGroupResult = await StudentGroupServiceMock().CreateStudentGroupAsync(null);

            //Assert
            nullGroupResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithoutCourseID_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutCourseID = new CreateStudentGroupDto()
            {
                Name = "New_test_name",
                CourseId = -id,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };

            //Act
            var withNotExistCoursIdResult = await StudentGroupServiceMock().CreateStudentGroupAsync(studentGroupWithoutCourseID);

            //Assert
            withNotExistCoursIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupWithoutValidStudentIDs_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutValidStudentIDs = new CreateStudentGroupDto()
            {
                Name = "New_test_name",
                CourseId = id,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };

            _studentGroupRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                                      .ReturnsAsync(new List<long> { id });

            //Act
            var withNotExistStrudentIdResult = await StudentGroupServiceMock().CreateStudentGroupAsync(studentGroupWithoutValidStudentIDs);

            //Assert
            withNotExistStrudentIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_StudentGroupwithoutMentorsAndStudents_ShouldReturnrCountNull()
        {
            //Arrange
            var withoutMentorsAndStudentsStudentGroup = new CreateStudentGroupDto()
            {
                Name = "Unique_test_name",
                CourseId = id,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };
             _studentRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(withoutMentorsAndStudentsStudentGroup.StudentIds))
               .ReturnsAsync(new List<Student>());

            _mentorRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(withoutMentorsAndStudentsStudentGroup.MentorIds))
                .ReturnsAsync(new List<Mentor>());

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(withoutMentorsAndStudentsStudentGroup.CourseId))
                                .ReturnsAsync(true);

            //Act
            var withoutMentorsAndStudentsGroupResult = await StudentGroupServiceMock().CreateStudentGroupAsync(withoutMentorsAndStudentsStudentGroup);

            //Assert
            (withoutMentorsAndStudentsGroupResult.Data.StudentIds.Count == 0).Should().BeTrue();
            (withoutMentorsAndStudentsGroupResult.Data.MentorIds.Count == 0).Should().BeTrue();
        }

        [Fact]
        public async Task UpdateStudentGroup_ValidDataPassed_ShouldReturnNotNull()
        {
            //Arrange
            var updateStudentGroupDto = new UpdateStudentGroupDto()
            {
                Name = name,
                CourseId = id,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(date),
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };
            var existingCourse = new Course()
            {
                Id = id,
                Name = name
            };

            _courseRepositoryMock.Setup(x => x.GetByIdAsync(updateStudentGroupDto.CourseId))
              .ReturnsAsync(existingCourse);

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(updateStudentGroupDto.CourseId))
                                .ReturnsAsync(true);

            //Act
            var successResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, updateStudentGroupDto);

            //Assert
            successResult.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateStudentGroup_NotExistingValidDataPassed_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingGroupId = -id;
            var updateStudentGroupDto = new UpdateStudentGroupDto()
            {
                Name = "new_test_name",
                CourseId = id,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(date),
                StudentIds = new List<long>(),
                MentorIds = new List<long>()

            };

            _studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(notExistingGroupId))
                  .ReturnsAsync((StudentGroup)null);

            _courseRepositoryMock.Setup(x => x.GetByIdAsync(updateStudentGroupDto.CourseId))
             .ReturnsAsync(new Course()
             {
                 Id = id,
                 Name = name
             });

            _courseRepositoryMock.Setup(x => x.IsEntityExistAsync(updateStudentGroupDto.CourseId))
                                .ReturnsAsync(true);

            //Act
            var groupNotExistResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(notExistingGroupId, updateStudentGroupDto);

            //Assert
            groupNotExistResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateStudentGroup_NullGroup_ShouldReturnValidationError()
        {
            //Act
            var nullGroupResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, null);

            //Assert
            nullGroupResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroup_NonExistentCourseId_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutCourseID = new UpdateStudentGroupDto()
            {
                Name = "New_test_name",
                CourseId = -id,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };

            //Act
            var withNotExistCoursIdResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, studentGroupWithoutCourseID);

            //Assert
            withNotExistCoursIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroup_NotValidStudentIDs_ShouldReturnValidationError()
        {
            //Arrange
            var studentGroupWithoutValidStudentIDs = new UpdateStudentGroupDto()
            {
                Name = "New_test_name",
                CourseId = id,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(date).Date,
                StudentIds = ids,
                MentorIds = ids
            };
            _studentGroupRepositoryMock.Setup(x => x.GetNotExistEntitiesIdsAsync(studentGroupWithoutValidStudentIDs.StudentIds))
                                     .ReturnsAsync(new List<long> { id });

            //Act
            var withNotExistStrudentIdResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, studentGroupWithoutValidStudentIDs);

            //Assert
            withNotExistStrudentIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroup_GroupNameIsNull_ShouldReturnValidationError()
        {
            //Act
            var groupNameIsNullResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, new UpdateStudentGroupDto() { Name = null });

            //Assert
            groupNameIsNullResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateStudentGroup_GroupNameIsEmpty_ShouldReturnValidationError()
        {
            //Act
            var groupNameIsEmptyResult = await StudentGroupServiceMock().UpdateStudentGroupAsync(ExistingStudentGroup().Id, new UpdateStudentGroupDto() { Name = string.Empty });

            //Assert
            groupNameIsEmptyResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }
    }
}
