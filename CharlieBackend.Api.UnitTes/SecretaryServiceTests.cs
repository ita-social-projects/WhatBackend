using AutoMapper;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class SecretaryServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly SecretaryService _secretaryService;
        Mock<ISecretaryRepository> _secretaryRepositoryMock;

        private readonly long _secretaryExpectedId;
        private readonly long _accountExpectedId;
        private readonly long _notExistingId;
        private readonly string _secretaryExpectedEmail;
        private readonly string _secretaryUpdatedFirstName;

        private Account GetAccountWithId()
        {
            return new Account()
            {
                Id = _accountExpectedId
            };
        }

        private Secretary GetSecretaryWithIdAndAccountId()
        {
            return new Secretary()
            {
                Id = _secretaryExpectedId,
                AccountId = _accountExpectedId
            };
        }
        private Secretary GetSecretaryWithIdAndAccountIdAndAccount()
        {
            var secretary = GetSecretaryWithIdAndAccountId();
            secretary.Account = GetAccountWithId();

            return secretary;
        }

        private UpdateSecretaryDto GetUpdateSecretaryDtoWithEmail()
        {
            return new UpdateSecretaryDto()
            {
                Email = _secretaryExpectedEmail
            };
        }

        public SecretaryServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
            _secretaryRepositoryMock = new Mock<ISecretaryRepository>();

            _secretaryService = new SecretaryService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object,
                null);

            _secretaryExpectedId = 20;
            _accountExpectedId = 10;
            _notExistingId = 0;
            _secretaryExpectedEmail = "secretary@gmail.com";
            _secretaryUpdatedFirstName = "Secretary First Name";

            _unitOfWorkMock.Setup(x => x.SecretaryRepository)
                .Returns(_secretaryRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateSecretaryAsync_ValidSecretary_ShouldReturnSecretaryDTO()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();

            var secretaryDto = new SecretaryDto()
            {
                Id = _secretaryExpectedId
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(_accountExpectedId))
                .ReturnsAsync(successExistingAccount);

            _secretaryRepositoryMock.Setup(x => x.Add(It.IsAny<Secretary>()))
                .Callback<Secretary>(x => x.Id = _secretaryExpectedId);            

            //Act
            var successResult = await _secretaryService.CreateSecretaryAsync(_accountExpectedId);

            //Assert
            successResult.Data
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(secretaryDto);
        }

        [Fact]
        public async Task CreateSecretaryAsync_NotExistingAccountId_ShouldReturnErrorCodeNotFound()
        {
            //Act
            var nonExistingIdResult = await _secretaryService.CreateSecretaryAsync(_notExistingId);

            //Assert
            nonExistingIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task CreateSecretaryAsync_AlreadyAssignedAccount_ShouldReturnErrorCodeValidationError()
        {
            //Arrange
            var assignedExistingAccount = GetAccountWithId();
            assignedExistingAccount.Role = UserRole.Mentor;

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(_accountExpectedId))
                .ReturnsAsync(assignedExistingAccount);

            //Act
            var alreadyAssignedResult = await _secretaryService.CreateSecretaryAsync(_accountExpectedId);

            //Assert
            alreadyAssignedResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.ValidationError);
        }

        [Fact]
        public async Task CreateSecretaryAsync_ExceptionDuringCreatingSecretary_ShouldReturnErrorCodeInternalServerError()
        {
            //Arrange
            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(_notExistingId))
               .Throws(new Exception());

            //Act
            var internalServerErrorResult = await _secretaryService.CreateSecretaryAsync(_notExistingId);

            //Assert
            internalServerErrorResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task UpdateSecretaryAsync_NotExistingSecretaryId_ShouldReturnErrorCodeNotFound()
        {
            //Arrange

            //Act
            var nonExistingIdResult = await _secretaryService.UpdateSecretaryAsync(_notExistingId, null);

            //Assert
            nonExistingIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task UpdateSecretaryAsync_NotChangableEmail_ShouldReturnErrorCodeConflict()
        {
            //Arrange
            var notChangableEmailSecretary = GetUpdateSecretaryDtoWithEmail();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                .GetByIdAsync(_secretaryExpectedId))
                .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(_accountExpectedId, _secretaryExpectedEmail))
                .ReturnsAsync(false);

            //Act
            var notChangableEmailResult = await _secretaryService.UpdateSecretaryAsync(_secretaryExpectedId, notChangableEmailSecretary);

            //Assert
            notChangableEmailResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task UpdateSecretaryAsync_ExceptionDuringUpdatingSecretary_ShouldReturnErrorCodeInternalServerError()
        {
            //Arrange
            _unitOfWorkMock.Setup(x => x.SecretaryRepository)
                .Throws(new Exception());

            //Act
            var internalServerErrorResult = await _secretaryService.UpdateSecretaryAsync(_notExistingId, null);

            //Assert
            internalServerErrorResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.InternalServerError);
        }

        [Fact]
        public async Task UpdateSecretaryAsync_ValidSecretary_ShouldReturnUpdatedSecretaryDTO()
        {
            //Arrange
            var updatedSecretaryDto = GetUpdateSecretaryDtoWithEmail();
            updatedSecretaryDto.FirstName = _secretaryUpdatedFirstName;

            var secretaryDto = new SecretaryDto() 
            {
                Id = _secretaryExpectedId,
                Email = _secretaryExpectedEmail,
                FirstName = _secretaryUpdatedFirstName
            };

            var successExistingSecretary = GetSecretaryWithIdAndAccountIdAndAccount();

            _secretaryRepositoryMock.Setup(x => x
                .GetByIdAsync(_secretaryExpectedId))
                .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.IsEmailChangableToAsync(_accountExpectedId, _secretaryExpectedEmail))
                .ReturnsAsync(true);

            //Act
            var successResult = await _secretaryService.UpdateSecretaryAsync(_secretaryExpectedId, updatedSecretaryDto);

            //Assert
            successResult.Data
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(secretaryDto);
        }

        [Fact]
        public async Task GetSecretaryByAccountIdAsync_ValidAccountId_ShouldReturnSecretaryDto()
        {
            //Arrange
            var secretary = GetSecretaryWithIdAndAccountIdAndAccount();

            _secretaryRepositoryMock.Setup(x => x.GetSecretaryByAccountIdAsync(secretary.Account.Id))
                .ReturnsAsync(secretary);

            //Act
            var existingResult = await _secretaryService.GetSecretaryByAccountIdAsync(_accountExpectedId);

            //Assert
            existingResult.Data
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(_mapper.Map<SecretaryDto>(secretary));
        }

        [Fact]
        public async Task GetSecretaryByIdAsync_ValidSecretaryId_ShouldReturnSecretaryDto()
        {
            //Arrange
            var secretary = GetSecretaryWithIdAndAccountIdAndAccount();

            _secretaryRepositoryMock.Setup(x => x.GetByIdAsync(secretary.Id))
                .ReturnsAsync(secretary);

            //Act
            var existingResult = await _secretaryService.GetSecretaryByIdAsync(_secretaryExpectedId);

            //Assert
            existingResult.Data
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(_mapper.Map<SecretaryDto>(secretary));
        }

        [Fact]
        public async Task GetAccountId_ValidSecretaryId_ShouldReturnAccountId()
        {
            //Arrange
            var secretary = GetSecretaryWithIdAndAccountIdAndAccount();

            _secretaryRepositoryMock.Setup(x => x.GetByIdAsync(secretary.Id))
                .ReturnsAsync(secretary);

            //Act
            var existingResult = await _secretaryService.GetAccountId(_secretaryExpectedId);

            //Assert
            existingResult
                .Should()
                .Be(_accountExpectedId);
        }

        [Fact]
        public async Task GetAllSecretariesAsync_ValidData_ShouldReturnSecretariesList()
        {
            //Arrange
            var allSecretaries = new List<Secretary>
            {
                GetSecretaryWithIdAndAccountIdAndAccount()
            };

            _secretaryRepositoryMock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(allSecretaries));

            //Act
            var successResultOfSecretaries = await _secretaryService.GetAllSecretariesAsync();

            //Assert
            successResultOfSecretaries
                .Should()
                .BeEquivalentTo(_mapper.Map<List<SecretaryDetailsDto>>(allSecretaries));
        }

        [Fact]
        public async Task GetActiveSecretariesAsync_ValidData_ShouldReturnSecretariesListWithIsActiveTrue()
        {
            //Arrange
            var activeSecretary = GetSecretaryWithIdAndAccountIdAndAccount();
            activeSecretary.Account.IsActive = true;
            var allActiveSecretaries = new List<Secretary>
            {
                activeSecretary
            };

            _secretaryRepositoryMock.Setup(x => x.GetActiveAsync())
                .Returns(Task.FromResult(allActiveSecretaries));

            //Act
            var successResultOfSecretaries = await _secretaryService.GetActiveSecretariesAsync();

            //Assert
            successResultOfSecretaries.Data
                .Should()
                .BeEquivalentTo(_mapper.Map<List<SecretaryDetailsDto>>(allActiveSecretaries));

            foreach (var secretary in successResultOfSecretaries.Data)
            {
                secretary.IsActive
                    .Should()
                    .BeTrue();
            }
        }

        [Fact]
        public async Task DisableSecretaryAsync_NotExistingSecretaryId_ShouldReturnErrorCodeNotFound()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            //Act
            var nonExistingIdResult = await _secretaryService.DisableSecretaryAsync(_notExistingId);

            //Assert
            nonExistingIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task DisableSecretaryAsync_NotChangedToDisabled_ShouldReturnErrorCodeConflict()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(_accountExpectedId))
                .ReturnsAsync(false);

            //Act
            var conflictResult = await _secretaryService.DisableSecretaryAsync(_secretaryExpectedId);

            //Assert
            conflictResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task DisableSecretaryAsyn_ValidData_ShouldReturnTrue()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.DisableAccountAsync(_accountExpectedId))
                .ReturnsAsync(true);

            //Act
            var validResult = await _secretaryService.DisableSecretaryAsync(_secretaryExpectedId);

            //Assert
            validResult.Data
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task EnableSecretaryAsync_NotExistingSecretaryId_ShouldReturnErrorCodeNotFound()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            //Act
            var nonExistingIdResult = await _secretaryService.EnableSecretaryAsync(_notExistingId);

            //Assert
            nonExistingIdResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.NotFound);
        }

        [Fact]
        public async Task EnableSecretaryAsync_NotChangedToEnabled_ShouldReturnErrorCodeConflict()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.EnableAccountAsync(_accountExpectedId))
                .ReturnsAsync(false);

            //Act
            var conflictResult = await _secretaryService.EnableSecretaryAsync(_secretaryExpectedId);

            //Assert
            conflictResult.Error.Code
                .Should()
                .BeEquivalentTo(ErrorCode.Conflict);
        }

        [Fact]
        public async Task EnableSecretaryAsync_ValidData_ShouldReturnTrue()
        {
            //Arrange
            var successExistingAccount = GetAccountWithId();
            var successExistingSecretary = GetSecretaryWithIdAndAccountId();

            _secretaryRepositoryMock.Setup(x => x
                 .GetByIdAsync(_secretaryExpectedId))
                 .ReturnsAsync(successExistingSecretary);

            _accountServiceMock.Setup(x => x.EnableAccountAsync(_accountExpectedId))
                .ReturnsAsync(true);

            //Act
            var validResult = await _secretaryService.EnableSecretaryAsync(_secretaryExpectedId);

            //Assert
            validResult.Data
                .Should()
                .BeTrue();
        }
    }
}
