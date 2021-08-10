using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CharlieBackend.Core.DTO.Mentor;
using FluentAssertions;

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

        public MentorServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _mentorRepositoryMock = new Mock<IMentorRepository>();
            _mentorServiceMock = new Mock<IMentorService>();
            _blobServiceMock= new Mock<IBlobService>();
            _currentUserMock = new Mock<ICurrentUserService>();

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateMentorAsync_NotExistingAccoutId_ShouldReturnNotFound()
        {
            //Arrange
            InitializeCreateMentorAsync();

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

            //Act
            var successResultOfMentors = await _mentorService.GetAllMentorsAsync();

            //Assert
            successResultOfMentors.Should().BeEquivalentTo(_mapper.Map<List<MentorDto>>(allMentors));
        }

        [Fact]
        public async Task DisableMentorAsync_NotExistingMentorId_ShouldReturnNotFound()
        {
            //Arrange
            long notExistingMentorId = 0;

            _mentorServiceMock.Setup(x => x.GetAccountId(notExistingMentorId));

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

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

            _mentorServiceMock.Setup(x => x.GetAccountId(existingMentorId))
                .ReturnsAsync(mentor.AccountId);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(mentor.AccountId.Value))
                .ReturnsAsync(isSucceed);

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(_mentorRepositoryMock.Object);

            //Act
            var notExistingIdResult = await _mentorService.DisableMentorAsync(existingMentorId);

            //Assert
            notExistingIdResult.Error.Code.Should().BeEquivalentTo(ErrorCode.NotFound);
        }
    }
}
