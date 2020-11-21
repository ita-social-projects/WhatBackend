using Moq;
using Xunit;
using System;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Entities;
using CharlieBackend.Business.Services;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Core.DTO.Account;



namespace CharlieBackend.Api.UnitTest
{
    public class AccountServiceTests : TestBase
    { 
        private readonly IMapper _mapper;

        public AccountServiceTests()
        {
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateAccountAsync()
        {
            //Arrange
            string accountExpectedEmail = "user@example.com";

            //var isEmailTaken = true;

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
                Password ="qwerty",
                ConfirmPassword = "qwerty"
            };

            var existingAccount = new Account()
            {
                Id = 1,
                Email = accountExpectedEmail,
                Role = UserRole.Mentor
            };

            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.Add(It.IsAny<Account>()))
                .Callback<Account>(x => x = existingAccount); 
            _unitOfWorkMock.Setup(x => x.AccountRepository).Returns(accountRepositoryMock.Object);


            var accountService = new AccountService(
                _unitOfWorkMock.Object,
                _mapper);

            //Act
             //var isEmailTakenResult = await accountService.CreateAccountAsync(isEmailTakenAccountModel);
            //var successResult = await accountService.CreateAccountAsync(successAccountModel);

            //Assert
            //Assert.Equal(ErrorCode.Conflict, isEmailTakenResult.Error.Code);

           // Assert.NotNull(successResult.Data);
           // Assert.Equal(successResult.Data.Id, accountExpectedId);
           // Assert.Equal(successResult.Data.Role, UserRole.NotAssigned);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
