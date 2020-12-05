using Moq;
using Xunit;
using System;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Services;
using CharlieBackend.Data.Repositories.Impl;
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

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
