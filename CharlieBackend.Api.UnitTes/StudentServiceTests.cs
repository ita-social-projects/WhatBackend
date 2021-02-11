using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace CharlieBackend.Api.UnitTest
{
    public class StudentServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;

        public StudentServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _studentRepositoryMock = new Mock<IStudentRepository>();
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
            
            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var successResult = await studentService.CreateStudentAsync(1);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Id.Should().Be(studentExpectedId);
        }

        [Fact]
        public async Task CreateStudentAsync_NonExistingAccountId_ShouldReturnNotFound()
        {
            InitializeForCreateStudentAsync();

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await studentService.CreateStudentAsync(0);

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

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var alreadyAssignedResult = await studentService.CreateStudentAsync(2);

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

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var successResult = await studentService.GetStudentByIdAsync(existingId);

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

            var studentService = new StudentService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await studentService.GetStudentByIdAsync(nonExistingId);

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

            var studentService = new StudentService(
            _accountServiceMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _notificationServiceMock.Object
            );

            var successResult = await studentService
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

            var studentService = new StudentService(
            _accountServiceMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _notificationServiceMock.Object
            );

            //Act
            var nonExistingId = await studentService
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

            var studentService = new StudentService(
            _accountServiceMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _notificationServiceMock.Object
            );

            var alreadyExistingEmailResult = await studentService
                .UpdateStudentAsync(6, alreadyExistingEmailUpdateStudentDto);

            alreadyExistingEmailResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
