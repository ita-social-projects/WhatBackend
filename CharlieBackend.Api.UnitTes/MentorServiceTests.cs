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

        public MentorServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _mentorRepositoryMock = new Mock<IMentorRepository>();
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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await mentorService.CreateMentorAsync(0);

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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var successResult = await mentorService.CreateMentorAsync(1);

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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var alreadyAssignedResult = await mentorService.CreateMentorAsync(2);

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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await mentorService.UpdateMentorAsync(0, nonExistingUpdateMentorDto);

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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var alreadyExistingEmailResult = await mentorService
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

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var successResult = await mentorService
                    .UpdateMentorAsync(1, successUpdateMentorDto);

            //Assert
            successResult.Data.Should().NotBeNull();
            successResult.Data.Should().BeEquivalentTo(expectedMentorDto);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
