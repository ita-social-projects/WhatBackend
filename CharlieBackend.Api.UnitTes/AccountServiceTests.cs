using Moq;
using Xunit;
using AutoMapper;
using System.Threading.Tasks;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;


namespace CharlieBackend.Api.UnitTest
{
    public class AccountServiceTests : TestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public AccountServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
            _notificationServiceMock = new Mock<INotificationService>();
        }

        [Fact]
        public async Task CreateAccountAsync()
        {
            //Arrange
            string accountExpectedEmail = "user@example.com";
            int accountExpectedId = 2;

            var successAccountModel = new CreateAccountDto()
            {
                Email = "test@example.com",
                FirstName = "test",
                LastName = "test",
                Password = "qwerty",
                ConfirmPassword = "qwerty"
            };

            var isEmailTakenAccountModel = new CreateAccountDto()
            {
                Email = accountExpectedEmail,
                FirstName = "test",
                LastName = "test",
                Password = "qwerty",
                ConfirmPassword = "qwerty"
            };

            var existingAccount = new Account()
            {
                Id = accountExpectedId,
                Email = accountExpectedEmail,
                Role = UserRole.NotAssigned
            };

            var accountRepositoryMock = new Mock<IAccountRepository>();

            accountRepositoryMock.Setup(x => x.Add(It.IsAny<Account>()))
                .Callback<Account>(x => x = existingAccount);

            accountRepositoryMock.Setup(x => x.Add(It.IsAny<Account>()))
                .Callback<Account>(x => x.Id = accountExpectedId);

            accountRepositoryMock.Setup(x => x.IsEmailTakenAsync(accountExpectedEmail))
                    .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.AccountRepository).Returns(accountRepositoryMock.Object);

            var accountService = new AccountService(
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var isEmailTakenResult = await accountService.CreateAccountAsync(isEmailTakenAccountModel);
            var successResult = await accountService.CreateAccountAsync(successAccountModel);

            //Assert
            Assert.Equal(ErrorCode.Conflict, isEmailTakenResult.Error.Code);

            Assert.NotNull(successResult.Data);
            Assert.Equal(accountExpectedId, successResult.Data.Id);
            Assert.Equal(UserRole.NotAssigned, successResult.Data.Role);
        }

        [Fact]
        public async Task ChangePasswordAsync()
        {
            //Arrange
            var salt = PasswordHelper.GenerateSalt();
            var oldPassword = "mypass";
            var newPassword = "changedPass";

            Account account = new Account
            {
                Id = 5,
                IsActive = true,
                Email = "user@exmaple.com",
                Password = PasswordHelper.HashPassword(oldPassword, salt),
                Salt = salt,
                Role = UserRole.Mentor
            };

            var changePass = new ChangeCurrentPasswordDto
            {
                Email = "user@exmaple.com",
                CurrentPassword = "mypass",
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            AccountDto updatedAccountDto = new AccountDto
            {
                Id = 5,
                Email = "user@exmaple.com",
                IsActive = true,
                Role = UserRole.Mentor
            };

            var notExistDto = new ChangeCurrentPasswordDto
            {
                Email = "notExist@exmaple.com"
            };

            var wrongPasswordDto = new ChangeCurrentPasswordDto
            {
                Email = "user@exmaple.com",
                CurrentPassword = "wrongPassword",
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            var withoutSaltDto = new ChangeCurrentPasswordDto
            {
                Email = "withoutSalt@exmaple.com"
            };

            Account accountWithoutSalt = new Account
            {
                Id = 5,
                IsActive = true,
                Email = "withoutSalt@exmaple.com",
                Password = PasswordHelper.HashPassword(oldPassword, salt),
                Salt = null,
                Role = UserRole.Mentor
            };

            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountSaltByEmail(account.Email))
                    .ReturnsAsync(salt);

            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountCredentialsByEmailAsync(changePass.Email))
                    .ReturnsAsync(account);

            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountCredentialsByEmailAsync(accountWithoutSalt.Email))
                    .ReturnsAsync(accountWithoutSalt);

            var accountService = new AccountService(
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var notExistAccount = await accountService.ChangePasswordAsync(notExistDto);
            var successResult = await accountService.ChangePasswordAsync(changePass);
            var wrongPassword = await accountService.ChangePasswordAsync(wrongPasswordDto);
            var accWithoutSalt = await accountService.ChangePasswordAsync(withoutSaltDto);

            //Assert
            Assert.NotNull(notExistDto);
            Assert.NotNull(successResult);
            Assert.NotNull(wrongPassword);
            Assert.NotNull(accWithoutSalt);

            Assert.Equal(ErrorCode.NotFound, notExistAccount.Error.Code);
            Assert.Equal(ErrorCode.Conflict, wrongPassword.Error.Code);
            Assert.Equal(ErrorCode.InternalServerError, accWithoutSalt.Error.Code);

            Assert.Equal(updatedAccountDto.Id, successResult.Data.Id);
            Assert.Equal(updatedAccountDto.Email, successResult.Data.Email);
            Assert.Equal(updatedAccountDto.IsActive, successResult.Data.IsActive);
            Assert.Equal(updatedAccountDto.Role, successResult.Data.Role);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
