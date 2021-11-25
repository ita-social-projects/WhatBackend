using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class StudentServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly StudentService _studentService;
        private readonly Mock<IBlobService> _blobServiceMock;
        private readonly Mock<IStudentService> _studentServiceMock;

        public StudentServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _blobServiceMock = new Mock<IBlobService>();
            _studentServiceMock = new Mock<IStudentService>();
            _studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object,
                _blobServiceMock.Object,
                GetCurrentUserAsExistingStudent().Object);
        }

        private void InitializeForCreateStudentAsync(long studentExpectedId = 10)
        {
            _studentRepositoryMock.Setup(x => x.Add(It.IsAny<Student>()))
                .Callback<Student>(x => x.Id = studentExpectedId);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateStudentAsync_ExistingAccountIdNotAssignedStudent_ShouldReturnStudent()
        {
            //Arrange
            long studentExpectedId = 10;

            var successExistingAccount = new Account()
            {
                Id = 1
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(1))
               .ReturnsAsync(successExistingAccount);

            InitializeForCreateStudentAsync();          

            //Act
            var successResult = await _studentService.CreateStudentAsync(1);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Id.Should().Be(studentExpectedId);
        }

        [Fact]
        public async Task CreateStudentAsync_NonExistingAccountId_ShouldReturnNotFound()
        {
            InitializeForCreateStudentAsync();

            //Act
            var nonExistingIdResult = await _studentService.CreateStudentAsync(0);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateStudentAsync_ExistingAccountIdAssignedStudent_ShouldReturnValidationError()
        {
            //Arrange
            var assignedExistingAccount = new Account()
            {
                Id = 2,
                Role = UserRole.Student
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(2))
                .ReturnsAsync(assignedExistingAccount);

            InitializeForCreateStudentAsync();

            //Act
            var alreadyAssignedResult = await _studentService.CreateStudentAsync(2);

            //Assert
            alreadyAssignedResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ExistingStudentId_ShouldReturnStudent()
        {
            //Arrange
            var existingId = 1;

            var student = new Student()
            {
                Id = 1,
                AccountId = 10,
                Account = new Account()
                {
                    Id = 10,
                    Email = "student@example.com"
                }
            };

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(existingId))
                .ReturnsAsync(student);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var successResult = await _studentService.GetStudentByIdAsync(existingId);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(_mapper.Map<StudentDto>(student));
        }

        [Fact]
        public async Task GetStudentByIdAsync_NonExistingStudentId_ShouldReturnNotFound()
        {
            //Arrange
            var existingId = 1;
            var nonExistingId = 0;

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(existingId));

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var nonExistingIdResult = await _studentService.GetStudentByIdAsync(nonExistingId);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task StudentUpdate_ExistingStudentId_ShouldReturnUpdatedStudentDto()
        {
            //Arrange
            var successUpdateStudentDto = new UpdateStudentDto()
            {
                Email = "updatedemail@example.com",
                FirstName = "FirstNameUpdated",
                LastName = "LastNameUpdated"
            };

            var expected = new StudentDto()
            {
                Id = 5, 
                Email = successUpdateStudentDto.Email,
                FirstName = successUpdateStudentDto.FirstName,
                LastName = successUpdateStudentDto.LastName
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

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(
                (long)student.AccountId, successUpdateStudentDto.Email))
                .ReturnsAsync(true);

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(5))
                .ReturnsAsync(student);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            var successResult = await _studentService
                .UpdateStudentAsync(5, successUpdateStudentDto);

            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task StudentUpdate_NonExistingId_ShouldReturnNotFound()
        {
            var emptyUpdateStudentDto = new UpdateStudentDto();

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(0));

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var nonExistingId = await _studentService
                .UpdateStudentAsync(0, emptyUpdateStudentDto);

            //Assert
            nonExistingId.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task StudentUpdate_AlreadyExistingEmailUpdateStudentDto_ShouldReturnValidationError()
        {
            //Arrange
            var existingEmail = "existingemail@example.com";

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
               (long)alreadyExistingEmailStudent.AccountId, existingEmail))
               .ReturnsAsync(false);

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(alreadyExistingEmailStudent);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            var alreadyExistingEmailResult = await _studentService
                .UpdateStudentAsync(6, alreadyExistingEmailUpdateStudentDto);

            alreadyExistingEmailResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ValidDataPassed_ShouldReturnListOfStudents()
        {
            //Arrange
            var allStudents = new List<Student>
            { 
                new Student()
                {
                    Id = 5,
                    AccountId = 6,
                    
                    Account = new Account()
                    {
                        Id = 6,
                        Email = "david.brown@example.com"
                    }
                }
            };

            _studentRepositoryMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(allStudents));

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var successResultOfStudents = await _studentService.GetAllStudentsAsync();

            //Assert
            successResultOfStudents.Data.Should().BeEquivalentTo(_mapper.Map<List<StudentDetailsDto>>(allStudents));
        }

        [Fact]
        public async Task DisableStudentAsync_AlreadyDisabledStudent_ShouldReturnNotFound()
        {
            //Arrange
            long existingStudentId = 3;
            bool isSucceed = false;

            var student = new Student()
            {
                Id = 3,
                AccountId = 2,
                Account = new Account()
                {
                    Id = 2,
                    Email = "studentmail@wxample.com"
                }
            };

            _studentServiceMock.Setup(x => x.GetAccountId(existingStudentId)).ReturnsAsync(student.AccountId);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(student.AccountId.Value)).ReturnsAsync(isSucceed);

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var notExistingResult = await _studentService.DisableStudentAsync(existingStudentId);

            //Assert
            notExistingResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task DisableStudentAsync_NotExistingStudentId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingStudentId = 0;

            _studentServiceMock.Setup(x => x.GetAccountId(notExistingStudentId));

            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var notExistingIdResult = await _studentService.DisableStudentAsync(notExistingStudentId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetSudentByAccountIdAsync_NotExistingAccountId_ShouldReturnNotFound()
        {
            //Arrange
            long expectedAccountId = 2;
            long notExistingAccountId = 1;

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(expectedAccountId));
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var notExistingResult = await _studentService.GetStudentByAccountIdAsync(notExistingAccountId);

            //Assert
            notExistingResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetSudentByAccountIdAsync_ValidDataPassed_ShouldReturnStudent()
        {
            //Arrange
            long existingAccountId = 6;

            var student = new Student()
            {
                Id = 1,
                AccountId = 6,
                Account = new Account()
                {
                    Id = 6,
                    Email = "studentmail@example.com"
                }
            };

            _studentRepositoryMock.Setup(x => x.GetStudentByAccountIdAsync(student.Account.Id)).ReturnsAsync(student);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var existingResult = await _studentService.GetStudentByAccountIdAsync(existingAccountId);

            //Assert
            existingResult.Data.Should().NotBeNull();
            existingResult.Data.Should().BeEquivalentTo(_mapper.Map<StudentDto>(student));
        }

        [Fact]
        public async Task GetStudentByEmailAsync_ValidDataPassed_ShouldReturnStudent()
        {
            //Arrange
            var existingEmail = "studentEmail@example.com";

            var student = new Student()
            {
                Id = 1,
                AccountId = 6,
                Account = new Account()
                {
                    Id = 6,
                    Email = "studentEmail@example.com"
                }
            };

            _studentRepositoryMock.Setup(x => x.GetStudentByEmailAsync(student.Account.Email)).ReturnsAsync(student);
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var existingResult = await _studentService.GetStudentByEmailAsync(existingEmail);

            //Assert
            existingResult.Data.Should().NotBeNull();
            existingResult.Data.Should().BeEquivalentTo(_mapper.Map<StudentDto>(student));
        }

        [Fact]
        public async Task GetStudentByEmailAsync_NotExistingEmail_ShouldReturnNotFound()
        {
            //Arrange
            var expectedEmail = "studentEmail@example.com";
            var notExistingEmail = "notExEmail@student.com";

            _studentRepositoryMock.Setup(x => x.GetStudentByEmailAsync(expectedEmail));
            _unitOfWorkMock.Setup(x => x.StudentRepository).Returns(_studentRepositoryMock.Object);

            //Act
            var existingResult = await _studentService.GetStudentByEmailAsync(notExistingEmail);

            //Assert
            existingResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }
    }
}
