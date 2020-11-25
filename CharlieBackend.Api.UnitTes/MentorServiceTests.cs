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

namespace CharlieBackend.Api.UnitTest
{
    public class MentorServiceTests : TestBase
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public MentorServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _mapper = GetMapper(new ModelMappingProfile());
        }

        [Fact]
        public async Task CreateMentorAsync()
        {
            //Arrange
            int mentorExpectedId = 5;
            var successExistingAccount = new Account()
            {
                Id = 1
            };

            var assignedExistingAccount = new Account()
            {
                Id = 2,
                Role = UserRole.Mentor
            };

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(1))
                .ReturnsAsync(successExistingAccount);

            _accountServiceMock.Setup(x => x.GetAccountCredentialsByIdAsync(2))
                .ReturnsAsync(assignedExistingAccount);

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.Add(It.IsAny<Mentor>()))
                .Callback<Mentor>(x => x.Id = mentorExpectedId);
            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorRepositoryMock.Object);

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var nonExistingIdResult = await mentorService.CreateMentorAsync(0);
            var successResult = await mentorService.CreateMentorAsync(1);
            var alreadyAssignedResult = await mentorService.CreateMentorAsync(2);

            //Assert
            Assert.Equal(ErrorCode.NotFound, nonExistingIdResult.Error.Code);

            Assert.NotNull(successResult.Data);
            Assert.Equal(successResult.Data.Id, mentorExpectedId);

            Assert.Equal(ErrorCode.ValidationError, alreadyAssignedResult.Error.Code);
        }

        [Fact]
        public async Task GetAllMentorsAsync()
        {
            //Arrange
            var Mentors = new List<Mentor>() {
                new Mentor() { },
                new Mentor() { },
                new Mentor() { }
            };

            var mentorRepositoryMock = new Mock<IMentorRepository>();
            mentorRepositoryMock.Setup(x => x.GetAllAsync())
                    .ReturnsAsync(Mentors);

            _unitOfWorkMock.Setup(x => x.MentorRepository).Returns(mentorRepositoryMock.Object);

            var mentorService = new MentorService(
                _accountServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapper,
                _notificationServiceMock.Object);

            //Act
            var successResult = await mentorService.GetAllMentorsAsync();

            //Assert
            Assert.Equal(Mentors.Count, successResult.Count);
        }

        protected override Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();
            return mock;
        }
    }
}
