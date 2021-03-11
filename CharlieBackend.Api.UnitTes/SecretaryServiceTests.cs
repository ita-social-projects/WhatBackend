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

namespace CharlieBackend.Api.UnitTest
{
    public class SecretaryServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public SecretaryServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateSecretaryAsync()
        {
            //Arrange
            int secretaryExpectedId = 20;
            var successExistingAccount = new Account()
            {
                Id = 10
            };

            var assignedExistingAccount = new Account()
            {
                Id = 15,
                Role = UserRole.Mentor
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(10))
                .ReturnsAsync(successExistingAccount);

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(15))
                .ReturnsAsync(assignedExistingAccount);

            var secretaryRepositoryMock = new Mock<ISecretaryRepository>();

            secretaryRepositoryMock.Setup(x => x.Add(It.IsAny<Secretary>()))
                .Callback<Secretary>(x => x.Id = secretaryExpectedId);

            _unitOfWorkMock.Setup(x => x.SecretaryRepository).Returns(secretaryRepositoryMock.Object);

            var secretaryService = new SecretaryService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object,
                null);

            //Act
            var nonExistingIdResult = await secretaryService.CreateSecretaryAsync(0);
            var successResult = await secretaryService.CreateSecretaryAsync(10);
            var alreadyAssignedResult = await secretaryService.CreateSecretaryAsync(15);

            //Assert
            Assert.Equal(ErrorCode.NotFound, nonExistingIdResult.Error.Code);

            Assert.NotNull(successResult.Data);
            Assert.Equal(successResult.Data.Id, secretaryExpectedId);

            Assert.Equal(ErrorCode.ValidationError, alreadyAssignedResult.Error.Code);
        }
    }
}
