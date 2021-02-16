using Moq;
using Xunit;
using System;
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
                Password = "Qqwerty3_",
                ConfirmPassword = "Qqwerty3_"
            };

            var isEmailTakenAccountModel = new CreateAccountDto()
            {
                Email = accountExpectedEmail,
                FirstName = "test",
                LastName = "test",
                Password = "Qqwerty3_",
                ConfirmPassword = "Qqwerty3_"
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

        [Fact]
        public async Task GenerateForgotPasswordToken()
        {
            //Arrange
            var successForgotPasswordDto = new ForgotPasswordDto
            {
                Email = "example@example.com",
                FormUrl = "https://frontenddomain/account/resetPassword"
            };

            var successUserAccount = new Account
            {
                Email = "example@example.com"
            };

            var userDoesntExistDto = new ForgotPasswordDto
            {
                Email = "doesntexist@example.com",
                FormUrl = "https://frontenddomain/account/resetPassword"
            };

            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountCredentialsByEmailAsync(successForgotPasswordDto.Email))
                    .ReturnsAsync(successUserAccount);

            var accountService = new AccountService(
                    _unitOfWorkMock.Object,
                    _mapper,
                    _notificationServiceMock.Object);

            //Act
            var successResult = await accountService.GenerateForgotPasswordToken(successForgotPasswordDto);
            var userDoesntExistResult = await accountService.GenerateForgotPasswordToken(userDoesntExistDto);

            //Assert
            Assert.NotNull(successResult);
            Assert.NotNull(userDoesntExistResult);

            Assert.Equal(ErrorCode.NotFound, userDoesntExistResult.Error.Code);

            Assert.Equal(successForgotPasswordDto.Email, successResult.Data.Email);
            Assert.Equal(successForgotPasswordDto.FormUrl, successResult.Data.FormUrl);
        }

        [Fact]
        public async Task ResetPasswordAsync()
        {
            //Arrange
            var userGuid = Guid.NewGuid().ToString();
            var forgotPasswordGenDate = DateTime.Now;
            var userWithoutTokenDateGuid = Guid.NewGuid().ToString();
            var expiredDate = DateTime.Now.AddDays(-1);

            var validUser = new Account
            {
                Id = 5,
                Email = "example@example.com",
                IsActive = true,
                ForgotPasswordToken = userGuid,
                ForgotTokenGenDate = forgotPasswordGenDate,
                Role = UserRole.Mentor
            };

            var successResetPasswordDto = new ResetPasswordDto
            {
                Email = "example@example.com",
                NewPassword = "bob228",
                ConfirmNewPassword = "bob228"
            };

            var successAccountDto = new AccountDto
            {
                Id = 5,
                Email = "example@example.com",
                IsActive = true,
                Role = UserRole.Mentor
            };

            var userDoesntExist = new ResetPasswordDto
            {
                Email = "dontexist@example.com"
            };

            var userWithTokenDateExpired = new Account
            {
                Email = "expiredDate@example.com",
                ForgotPasswordToken = userGuid,
                ForgotTokenGenDate = expiredDate
            };

            var userWithTokenDateExpiredDto = new ResetPasswordDto
            {
                Email = "expiredDate@example.com"
            };

            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountCredentialsByEmailAsync(successResetPasswordDto.Email))
                    .ReturnsAsync(validUser);
            _unitOfWorkMock.Setup(x => x.AccountRepository.GetAccountCredentialsByEmailAsync(userWithTokenDateExpiredDto.Email))
                    .ReturnsAsync(userWithTokenDateExpired);

            var accountService = new AccountService(
                    _unitOfWorkMock.Object,
                    _mapper,
                    _notificationServiceMock.Object);

            //Act
            var userDoesntExistResult = await accountService.ResetPasswordAsync(userGuid, userDoesntExist);
            var invalidFormToken = await accountService.ResetPasswordAsync(Guid.NewGuid().ToString(), successResetPasswordDto);
            var expiredTokenDate = await accountService.ResetPasswordAsync(userGuid, userWithTokenDateExpiredDto);
            var successResult = await accountService.ResetPasswordAsync(userGuid.ToString(), successResetPasswordDto);

            //Assert
            Assert.NotNull(userDoesntExistResult);
            Assert.NotNull(invalidFormToken);
            Assert.NotNull(successResult);
            Assert.NotNull(expiredTokenDate);

            Assert.Equal(ErrorCode.NotFound, userDoesntExistResult.Error.Code);
            Assert.Equal(ErrorCode.ValidationError, invalidFormToken.Error.Code);
            Assert.Equal(ErrorCode.ForgotPasswordExpired, expiredTokenDate.Error.Code);

            Assert.Equal(successAccountDto.Id, successResult.Data.Id);
            Assert.Equal(successAccountDto.Email, successResult.Data.Email);
            Assert.Equal(successAccountDto.IsActive, successResult.Data.IsActive);
            Assert.Equal(successAccountDto.Role, successResult.Data.Role);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
