﻿using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class StudentGroupServiceTest : TestBase
    {
        private readonly Mock<ILogger<StudentGroupService>> _loggerMock;
        private readonly IMapper _mapper;


        public StudentGroupServiceTest()
        {
            _loggerMock = new Mock<ILogger<StudentGroupService>>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateStudentGroup()
        {
            //Arrange

            var newStudentGroup = new CreateStudentGroupDto()
            {
                Name = "New_test_name",
                CourseId = 2,
                StartDate = DateTime.Now.Date,
                FinishDate = DateTime.Now.AddMonths(3).Date,
                StudentIds = new List<long>() { 1, 2, 3, 4 },
                MentorIds = new List<long>() { 8, 9 }

            };

            var existingStudentGroup = new CreateStudentGroupDto()
            {
                Name = "Exists_test_name",
                CourseId = 3,
                StartDate = DateTime.Now.AddMonths(3).Date,
                FinishDate = DateTime.Now.AddMonths(6).Date,
                StudentIds = new List<long>() { 5, 6, 7, 8 },
                MentorIds = new List<long>() { 10, 11 }

            };

            var withoutMentorsAndStudentsStudentGroup = new CreateStudentGroupDto()
            {
                Name = "Unique_test_name",
                CourseId = 3,
                StartDate = DateTime.Now.AddMonths(3).Date,
                FinishDate = DateTime.Now.AddMonths(6).Date,
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            var studentsRepositoryMock = new Mock<IStudentRepository>();
            var mentorsRepositoryMock = new Mock<IMentorRepository>();

            studentGroupRepositoryMock.Setup(x => x.Add(It.IsAny<StudentGroup>()));

            studentsRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(newStudentGroup.StudentIds))
                .ReturnsAsync(new List<Student>()
                {
                    new Student { Id = 1 },
                    new Student { Id = 2 },
                    new Student { Id = 3 },
                    new Student { Id = 4 }
                });

            mentorsRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(newStudentGroup.MentorIds))
                .ReturnsAsync(new List<Mentor>()
                {
                    new Mentor { Id = 18 },
                    new Mentor { Id = 19 }
                });

            studentsRepositoryMock.Setup(x => x.GetStudentsByIdsAsync(withoutMentorsAndStudentsStudentGroup.StudentIds))
               .ReturnsAsync(new List<Student>());

            mentorsRepositoryMock.Setup(x => x.GetMentorsByIdsAsync(withoutMentorsAndStudentsStudentGroup.MentorIds))
                .ReturnsAsync(new List<Mentor>());

            studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(newStudentGroup.Name))
                    .ReturnsAsync(false);

            studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(existingStudentGroup.Name))
                    .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(studentsRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorsRepositoryMock.Object);


            var studentGroupService = new StudentGroupService(
                _unitOfWorkMock.Object,
                _mapper,
                _loggerMock.Object
                );

            //Act

            var successResult = await studentGroupService.CreateStudentGroupAsync(newStudentGroup);
            var groupNameExistResult = await studentGroupService.CreateStudentGroupAsync(existingStudentGroup);
            var nullGroupResult = await studentGroupService.CreateStudentGroupAsync(null);
            var withoutMentorsAndStudentsGroupResult = await studentGroupService.CreateStudentGroupAsync(withoutMentorsAndStudentsStudentGroup);

            //Assert

            Assert.NotNull(successResult.Data);
            Assert.Equal(newStudentGroup.Name, successResult.Data.Name);

            Assert.Equal(ErrorCode.UnprocessableEntity, groupNameExistResult.Error.Code);

            Assert.Equal(ErrorCode.ValidationError, nullGroupResult.Error.Code);

            Assert.True(withoutMentorsAndStudentsGroupResult.Data.StudentIds.Count == 0);
            Assert.True(withoutMentorsAndStudentsGroupResult.Data.MentorIds.Count == 0);
        }

        [Fact]
        public async Task UpdateStudentGroup()
        {
            //Arrange

            long notExistingGroupId = -10;

            var updateStudentGroupDto = new UpdateStudentGroupDto()
            {
                Name = "new_test_name",
                CourseId = 2,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(3),
                StudentIds = new List<long>() ,
                MentorIds = new List<long>() 

            };

            var existingNameStudentGroupDto = new UpdateStudentGroupDto()
            {
                Name = "Test_name",
                CourseId = 4,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(3),
                StudentIds = new List<long>(),
                MentorIds = new List<long>()
            };

            var existingStudentGroup = new StudentGroup()
            {
                Id = 10,
                Name = "Test_name",
                CourseId = 3,
                StartDate = DateTime.Now.AddMonths(3),
                FinishDate = DateTime.Now.AddMonths(6),
                StudentsOfStudentGroups = new List<StudentOfStudentGroup>(),
                MentorsOfStudentGroups = new List<MentorOfStudentGroup>()
            };

            var existingCourse = new Course()
            {
                Id = 2,
                Name = "AAA"
            };

            var studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            var studentsRepositoryMock = new Mock<IStudentRepository>();
            var mentorsRepositoryMock = new Mock<IMentorRepository>();
            var courseRepositoryMock = new Mock<ICourseRepository>();

            studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(existingStudentGroup.Id))
                .ReturnsAsync(existingStudentGroup);

            studentGroupRepositoryMock.Setup(x => x.GetByIdAsync(notExistingGroupId))
                .ReturnsAsync((StudentGroup)null);

            studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(updateStudentGroupDto.Name))
                .ReturnsAsync(false);

            studentGroupRepositoryMock.Setup(x => x.IsGroupNameExistAsync(existingNameStudentGroupDto.Name))
               .ReturnsAsync(true);

            courseRepositoryMock.Setup(x => x.GetByIdAsync(updateStudentGroupDto.CourseId))
              .ReturnsAsync(existingCourse);

            studentGroupRepositoryMock.Setup(x => x.UpdateManyToMany(It.IsAny<IList<StudentOfStudentGroup>>(),
                                                                     It.IsAny<IList<StudentOfStudentGroup>>()));

            mentorsRepositoryMock.Setup(x => x.UpdateMentorGroups(It.IsAny<IList<MentorOfStudentGroup>>(),
                                                                  It.IsAny<IList<MentorOfStudentGroup>>()));

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(studentGroupRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(studentsRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorsRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(courseRepositoryMock.Object);

            var studentGroupService = new StudentGroupService(
                _unitOfWorkMock.Object,
                _mapper,
                _loggerMock.Object
                );

            //Act

            var successResult = await studentGroupService.UpdateStudentGroupAsync(existingStudentGroup.Id, updateStudentGroupDto);
            var groupNameExistResult = await studentGroupService.UpdateStudentGroupAsync(existingStudentGroup.Id, existingNameStudentGroupDto);
            var groupNotExistResult = await studentGroupService.UpdateStudentGroupAsync(notExistingGroupId, updateStudentGroupDto);
            var nullGroupResult = await studentGroupService.UpdateStudentGroupAsync(existingStudentGroup.Id, null);

            //Assert

            Assert.NotNull(successResult.Data);
            Assert.Equal(successResult.Data.Name, successResult.Data.Name);

            Assert.Equal(ErrorCode.UnprocessableEntity, groupNameExistResult.Error.Code);

            Assert.Equal(ErrorCode.NotFound, groupNotExistResult.Error.Code);

            Assert.Equal(ErrorCode.ValidationError, nullGroupResult.Error.Code);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock;
        }
    }
}
