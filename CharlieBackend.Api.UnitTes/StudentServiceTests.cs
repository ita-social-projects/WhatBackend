using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class StudentServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public StudentServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateStudentAsync()
        {
            int studentExpectedId = 10;
            var successExistingAccount = new Account()
            {
                Id = 1
            };

            var assignedExistingAccount = new Account()
            {
                Id = 2,
                Role = UserRole.Student
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(1))
               .ReturnsAsync(successExistingAccount);

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(2))
                .ReturnsAsync(assignedExistingAccount);

            var studentRepositoryMock = new Mock<IStudentRepository>();
            studentRepositoryMock.Setup(x => x.Add(It.IsAny<Student>()))
                .Callback<Student>(x => x.Id = studentExpectedId);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(studentRepositoryMock.Object);

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await studentService.CreateStudentAsync(0);
            var successResult = await studentService.CreateStudentAsync(1);
            var alreadyAssignedResult = await studentService.CreateStudentAsync(2);

            //Assert
            Assert.Equal(ErrorCode.NotFound, nonExistingIdResult.Error.Code);

            Assert.NotNull(successResult.Data);
            Assert.Equal(studentExpectedId, successResult.Data.Id);

            Assert.Equal(ErrorCode.ValidationError, alreadyAssignedResult.Error.Code);
        }

        [Fact]
        public async Task StudentUpdate()
        {
            //Arrange
            var existingEmail = "existingemail@example.com";

            var emptyUpdateStudentDto = new UpdateStudentDto();

            var successUpdateStudentDto = new UpdateStudentDto()
            {
                Email = "updatedemail@example.com",
                FirstName = "FirstNameUpdated",
                LastName = "LastNameUpdated"
            };

            var student = new Student()
            {
                Id = 5,
                AccountId = 10,
                Account = new Account()
                {
                    Id = 10,
                    Email = "student@example.com"
                }
            };

            var alreadyExistingEmailUpdateStudentDto = new UpdateStudentDto()
            {
                Email = existingEmail,
                FirstName = "updateTest",
                LastName = "updateTest"
            };

            var alreadyExistingEmailStudent = new Student()
            {
                Id = 6,
                AccountId = 11,
                Account = new Account()
                {
                    Id = 11,
                    Email = existingEmail
                }
            };

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(
                (long)student.AccountId, successUpdateStudentDto.Email))
                .ReturnsAsync(true);

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(
                (long)alreadyExistingEmailStudent.AccountId, existingEmail))
                .ReturnsAsync(false);

            var studentRepositoryMock = new Mock<IStudentRepository>();

            studentRepositoryMock.Setup(x => x.GetByIdAsync(5))
                .ReturnsAsync(student);

            studentRepositoryMock.Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(alreadyExistingEmailStudent);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(studentRepositoryMock.Object);

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object
                );

            //Act
            var nonExistingId = await studentService
                .UpdateStudentAsync(0, emptyUpdateStudentDto);
            var successResult = await studentService
                .UpdateStudentAsync(5, successUpdateStudentDto);
            var alreadyExistingEmailResult = await studentService
                .UpdateStudentAsync(6, alreadyExistingEmailUpdateStudentDto);

            //Assert
            Assert.Equal(ErrorCode.NotFound, nonExistingId.Error.Code);

            Assert.NotNull(successResult.Data);
            Assert.Equal(successUpdateStudentDto.Email, successResult.Data.Email);
            Assert.Equal(successUpdateStudentDto.FirstName, successResult.Data.FirstName);
            Assert.Equal(successUpdateStudentDto.LastName, successResult.Data.LastName);

            Assert.Equal(ErrorCode.ValidationError, alreadyExistingEmailResult.Error.Code);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
