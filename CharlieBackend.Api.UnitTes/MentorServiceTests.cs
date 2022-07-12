using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Mentor;
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
    public class MentorServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IMentorRepository> _mentorRepositoryMock;
        private readonly MentorService _mentorService;
        private readonly Mock<IMentorService> _mentorServiceMock;
        private readonly Mock<IBlobService> _blobServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<IStudentGroupRepository> _studentGroupRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;

        public MentorServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _mentorServiceMock = new Mock<IMentorService>();
            _blobServiceMock = new Mock<IBlobService>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _studentGroupRepositoryMock = new Mock<IStudentGroupRepository>();
            _courseRepositoryMock = new Mock <ICourseRepository>();  

            _mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object,
                _blobServiceMock.Object, _currentUserMock.Object);
        }

        private void InitializeCreateMentorAsync(long mentorExpectedId = 5)
        {
            _mentorRepositoryMock.Setup(x => x.Add(It.IsAny<Mentor>()))
                .Callback<Mentor>(x => x.Id = mentorExpectedId);

            
        }

        private void InitializeMentorRepository()
        {
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
        }

        [Fact]
        public void CheckRoleAndIdMentor_CheckOtherMentorId_ReturnUnauthorized()
        {
            //Arrange
            long currentId = 1;
            long otherId = 2;
            Result<object> expacted = new Result<object>()
            {
                Error = new ErrorData()
                {
                    Code = ErrorCode.Unauthorized,
                    Message = ""
                }
            };

            _currentUserMock.Setup(u => u.Role).Returns(UserRole.Mentor);
            _currentUserMock.Setup(u => u.EntityId).Returns(otherId);

            //Act
            var result = _mentorService.CheckRoleAndIdMentor<object>(currentId);

            //Assert
            Assert.NotNull(result.Error);
            Assert.Equal(expacted.Error.Code, result.Error.Code);
        }

        [Fact]
        public void CheckRoleAndIdMentor_CheckCurentMentorId_ReturnUnauthorized()
        {
            //Arrange
            long currentId = 1;
            long otherId = 1;

            _currentUserMock.Setup(u => u.Role).Returns(UserRole.Mentor);
            _currentUserMock.Setup(u => u.EntityId).Returns(otherId);

            //Act
            var result = _mentorService.CheckRoleAndIdMentor<object>(currentId);

            //Assert
            Assert.Null(result.Error);
        }

        [Fact]
        public void CheckRoleAndIdMentor_CheckNotMentorId_ReturnUnauthorized()
        {
            //Arrange
            long currentId = 1;
            long otherId = 1;

            _currentUserMock.Setup(u => u.Role).Returns(UserRole.Student);
            _currentUserMock.Setup(u => u.EntityId).Returns(otherId);

            //Act
            var result = _mentorService.CheckRoleAndIdMentor<object>(currentId);

            //Assert
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task CreateMentorAsync_NotExistingAccoutId_ShouldReturnNotFound()
        {
            //Arrange
            InitializeCreateMentorAsync();
            InitializeMentorRepository();

            //Act
            var nonExistingIdResult = await _mentorService.CreateMentorAsync(0);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateMentorAsync_ValidDataPassed_ShouldReturnMentor()
        {
            //Arrange
            long mentorExpectedId = 5;

            var successExistingAccount = new Account()
            {
                Id = 1
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(1))
                .ReturnsAsync(successExistingAccount);

            InitializeCreateMentorAsync();
            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.CreateMentorAsync(1);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Id.Should().Be(mentorExpectedId);
        }

        [Fact]
        public async Task CreateMentorAsync_AlreadyAssignedMentor_ShouldReturnValidationError()
        {
            //Arrange
            var assignedExistingAccount = new Account()
            {
                Id = 2,
                Role = UserRole.Mentor
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(2))
                .ReturnsAsync(assignedExistingAccount);

            InitializeCreateMentorAsync();
            InitializeMentorRepository();

            //Act
            var alreadyAssignedResult = await _mentorService.CreateMentorAsync(2);

            //Assert
            alreadyAssignedResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateMentorAsync_NotExistingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            var nonExistingUpdateMentorDto = new UpdateMentorDto();

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(0));

            InitializeMentorRepository();

            //Act
            var nonExistingIdResult = await _mentorService.UpdateMentorAsync(0, nonExistingUpdateMentorDto);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateMentorAsync_AlreadyExistingEmailUpdateStudentDto_ShouldReturnValidationError()
        {
            //Arrange
            var existingEmail = "existingemail@example.com";

            var alreadyExistingEmailUpdateMentorDto = new UpdateMentorDto()
            {
                Email = existingEmail,
                FirstName = "updateTest",
                LastName = "updateTest"
            };

            var alreadyExistingEmailMentor = new Mentor()
            {
                Id = 2,
                AccountId = 2,
                Account = new Account()
                {
                    Id = 2,
                    Email = existingEmail
                }
            };

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(
                    (long)alreadyExistingEmailMentor.AccountId,
                    existingEmail))
                    .ReturnsAsync(false);

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(2))
                    .ReturnsAsync(alreadyExistingEmailMentor);

            InitializeMentorRepository();

            //Act
            var alreadyExistingEmailResult = await _mentorService
                    .UpdateMentorAsync(2, alreadyExistingEmailUpdateMentorDto);

            //Assert
            alreadyExistingEmailResult.Error.Code.Should().BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task UpdateMentorAsync_ValidDataPassed_ShouldReturnUpdatedMentorDto()
        {
            //Arrange
            var successUpdateMentorDto = new UpdateMentorDto()
            {
                Email = "updateTest@gmail.com",
                FirstName = "updateTest",
                LastName = "updateTest"
            };

            var successMentor = new Mentor()
            {
                Id = 1,
                AccountId = 1,
                Account = new Account()
                {
                    Id = 1,
                    Email = "test@gmail.com"
                }
            };

            var expectedMentorDto = new MentorDto()
            {
                Id = 1,
                Email = successUpdateMentorDto.Email,
                FirstName = successUpdateMentorDto.FirstName,
                LastName = successUpdateMentorDto.LastName
            };

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(
                    (long)successMentor.AccountId, successUpdateMentorDto.Email))
                    .ReturnsAsync(true);

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                    .ReturnsAsync(successMentor);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService
                    .UpdateMentorAsync(1, successUpdateMentorDto);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(expectedMentorDto);
        }

        [Fact]
        public async Task GetMentorByAccountIdAsync_ValidDataPassed_ShouldReturnMentor()
        {
            //Arrange
            var accountExpectedId = 2;

            var mentor = new Mentor()
            {
                Id = 3,
                AccountId = 2,
                Account = new Account()
                {
                    Id = 2,
                    Email = "existingemail@example.com"
                }
            };

            _mentorRepositoryMock.Setup(x => x.GetMentorByAccountIdAsync(accountExpectedId))
                        .ReturnsAsync(mentor);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.GetMentorByAccountIdAsync(accountExpectedId);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(_mapper.Map<MentorDto>(mentor));
        }

        [Fact]
        public async Task GetMentorByAccountIdAsync_NotExistingAccountId_ShouldReturnNotFound()
        {
            //Arrange
            var accountExpectedId = 2;
            var notExisingId = 0;

            _mentorRepositoryMock.Setup(x => x.GetMentorByAccountIdAsync(accountExpectedId));

            InitializeMentorRepository();

            //Act
            var nonExistingIdResult = await _mentorService.GetMentorByAccountIdAsync(notExisingId);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetMentorByIdAsync_ValidDataPassed_ShouldReturnMentorById()
        {
            //Arrange
            var mentorExpectedId = 2;

            var mentor = new Mentor()
            {
                Id = 2,
                AccountId = 2,
            };

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(mentorExpectedId))
                        .ReturnsAsync(mentor);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.GetMentorByIdAsync(mentorExpectedId);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(_mapper.Map<MentorDto>(mentor));
        }

        [Fact]
        public async Task GetMentorByIdAsync_NotExisingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            var mentorExpectedId = 2;
            var notExisingId = 0;

            _mentorRepositoryMock.Setup(x => x.GetMentorByIdAsync(mentorExpectedId));

            InitializeMentorRepository();

            //Act
            var nonExistingIdResult = await _mentorService.GetMentorByAccountIdAsync(notExisingId);

            //Assert
            nonExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task GetAllMentorsAsync_ValidDataPassed_ShouldReturnListOfMentors()
        {
            //Arrange
            var allMentors = new List<Mentor>
            {
                new Mentor()
                {
                    Id = 3,
                    AccountId = 2,
                    Account = new Account()
                {
                    Id = 2,
                    Email = "existingemail@example.com"
                }
                }
            };

            _mentorRepositoryMock.Setup(x => x.GetAllAsync())
                        .ReturnsAsync(allMentors);

            InitializeMentorRepository();

            //Act
            var successResultOfMentors = await _mentorService.GetAllMentorsAsync();

            //Assert
            successResultOfMentors.Should().BeEquivalentTo(_mapper.Map<List<MentorDto>>(allMentors));
        }

        [Fact]
        public async Task GetAllMentorsAsync_NoDataPassed_ShouldReturnEmptyListOfMentors()
        {
            //Arrange
            var mentors = new List<Mentor>();

            _mentorRepositoryMock.Setup(x => x.GetAllAsync())
                                 .ReturnsAsync(mentors);

            InitializeMentorRepository();

            //Act
            var emptyListOfMentors = await _mentorService.GetAllMentorsAsync();

            //Assert
            emptyListOfMentors.Should().BeEquivalentTo(_mapper.Map<List<MentorDto>>(mentors));
            emptyListOfMentors.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllActiveMentorsAsync_ValidDataPassed_ShouldReturnAllActiveMentors()
        {
            //Arrange
            var allActiveMentors = new List<Mentor>()
            {
                new Mentor()
                {
                    Id = 1,
                    AccountId = 2,
                    Account = new Account()
                    {
                        Id = 2,
                        Email = "existingemail@example.com"
                    }
                }
            };

            _mentorRepositoryMock.Setup(x => x.GetAllActiveAsync())
                                 .ReturnsAsync(allActiveMentors);

            InitializeMentorRepository();

            //Act
            var emptyListOfMentors = await _mentorService.GetAllActiveMentorsAsync();

            //Assert
            emptyListOfMentors.Should().BeEquivalentTo(_mapper.Map<List<MentorDto>>(allActiveMentors));
        }

        [Fact]
        public async Task GetAccountIdAsync_ValidDataPassed_ShouldReturnMentorAccountById()
        {
            //Arrange
            long existingMentorId = 1;
            var mentor = new Mentor()
            {
                Id = 1,
                AccountId = 2
            };

            _mentorRepositoryMock.Setup(x => x.GetByIdAsync(existingMentorId))
                                 .ReturnsAsync(mentor);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.GetAccountIdAsync(existingMentorId);

            //Assert
            successResult.Should().NotBeNull();
            successResult.Value.Should().Be(mentor.AccountId);
        }

        [Fact]
        public async Task DisableMentorAsync_NotExistingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingMentorId = 0;

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(notExistingMentorId));

            InitializeMentorRepository();

            //Act
            var notExistingIdResult = await _mentorService.DisableMentorAsync(notExistingMentorId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task DisableMentorAsync_AlreadyDisabledMentor_ShouldReturnNotFound()
        {
            //Arrange
            long existingMentorId = 3;
            bool isSucceed = false;

            var mentor = new Mentor()
            {
                Id = 3,
                AccountId = 2,
                Account = new Account()
                {
                    Id = 2,
                    Email = "mentoremail@example.com"
                }
            };

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(existingMentorId))
                .ReturnsAsync(mentor.AccountId);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(mentor.AccountId.Value))
                .ReturnsAsync(isSucceed);

            InitializeMentorRepository();

            //Act
            var notExistingIdResult = await _mentorService.DisableMentorAsync(existingMentorId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task DisableMentorAsync_ValidDataPassed_ShouldReturnSuccess()
        {
            //Arrange
            long existingMentorId = 5;
            bool isSucceed = true;

            var mentor = new Mentor()
            {
                Id = 5,
                AccountId = 1,
                Account = new Account()
                {
                    Id = 1,
                    Email = "mentoremail@example.com"
                }
            };

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(existingMentorId))
                .ReturnsAsync(mentor.AccountId);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(mentor.AccountId.Value))
                .ReturnsAsync(isSucceed);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.DisableMentorAsync(existingMentorId);

            //Assert
            successResult.Data.Should().Equals(true);
        }

        [Fact]
        public async Task EnableMentorAsync_AlreadyEnabledMentor_ShouldReturnNotFound()
        {
            //Arrange
            long existingMentorId = 3;
            bool isSucceed = false;

            var mentor = new Mentor()
            {
                Id = 3,
                AccountId = 2,
                Account = new Account()
                {
                    Id = 2,
                    Email = "mentoremail@example.com"
                }
            };

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(existingMentorId))
                .ReturnsAsync(mentor.AccountId);

            _accountServiceMock.Setup(x => x.EnableAccountAsync(mentor.AccountId.Value))
                .ReturnsAsync(isSucceed);

            InitializeMentorRepository();

            //Act
            var notExistingIdResult = await _mentorService.EnableMentorAsync(existingMentorId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task EnableMentorAsync_NotExistingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingMentorId = 20;

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(notExistingMentorId));

            InitializeMentorRepository();

            //Act
            var notExistingIdResult = await _mentorService.EnableMentorAsync(notExistingMentorId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task EnableMentorAsync_ValidDataPassed_ShouldReturnSuccess()
        {
            //Arrange
            long existingMentorId = 5;
            bool isSucceed = true;

            var mentor = new Mentor()
            {
                Id = 5,
                AccountId = 1,
                Account = new Account()
                {
                    Id = 1,
                    Email = "mentoremail@example.com"
                }
            };

            _mentorServiceMock.Setup(x => x.GetAccountIdAsync(existingMentorId))
                .ReturnsAsync(mentor.AccountId);

            _accountServiceMock.Setup(x => x.EnableAccountAsync(mentor.AccountId.Value))
                .ReturnsAsync(isSucceed);

            InitializeMentorRepository();

            //Act
            var successResult = await _mentorService.EnableMentorAsync(existingMentorId);

            //Assert
            successResult.Data.Should().Equals(true);
        }

        [Fact]
        public async Task GetMentorStudyGroupsByMentorIdAsync_NotExistingMentorId_ShouldReturnEmptyListOfGroups()
        {
            //Arrange
            var notExistingMentorId = 0;

            _studentGroupRepositoryMock.Setup(x => x.GetMentorStudyGroups(notExistingMentorId));

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var notExistingIdResult = await _mentorService.GetMentorStudyGroupsByMentorIdAsync(notExistingMentorId);

            //Assert
            notExistingIdResult.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMentorStudyGroupsByMentorIdAsync_ValidDataPassed_ShouldReturnMentorsStudentGroups()
        {
            //Arrange
            var existingMentorId = 2;

            var mentorStudyGroups = new List<MentorStudyGroupsDto>()
            {
                new MentorStudyGroupsDto()
                {
                    Id = 2,
                    Name = "John"
                }
            };

            _studentGroupRepositoryMock.Setup(x => x.GetMentorStudyGroups(existingMentorId)).Returns(Task.FromResult(mentorStudyGroups));

            _unitOfWorkMock.Setup(x => x.StudentGroupRepository).Returns(_studentGroupRepositoryMock.Object);

            //Act
            var successResult = await _mentorService.GetMentorStudyGroupsByMentorIdAsync(existingMentorId);

            //Assert
            successResult.Should().BeEquivalentTo(mentorStudyGroups);
        }

        [Fact]
        public async Task GetMentorCoursesByMentorIdAsync_NotExistingMentorId_ShouldReturnEmptyListOfCourses()
        {
            //Arrange
            var notExistingMentorId = 0;

            _courseRepositoryMock.Setup(x => x.GetMentorCoursesAsync(notExistingMentorId));

            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);

            //Act
            var notExistingIdResult = await _mentorService.GetMentorCoursesByMentorIdAsync(notExistingMentorId);

            //Assert
            notExistingIdResult.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMentorCoursesByMentorIdAsync_ValidDataPassed_ShouldReturnMentorCourses()
        {
            //Arrange
            var existingMentorId = 2;

            var mentorCourses = new List<MentorCoursesDto>()
            {
                new MentorCoursesDto()
                {
                    Id = 2,
                    Name = "Mentor name"
                }
            };

            _courseRepositoryMock.Setup(x => x.GetMentorCoursesAsync(existingMentorId)).Returns(Task.FromResult(mentorCourses));

            _unitOfWorkMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);

            //Act
            var successResult = await _mentorService.GetMentorCoursesByMentorIdAsync(existingMentorId);

            //Assert
            successResult.Should().BeEquivalentTo(mentorCourses);
        }
    }
}
