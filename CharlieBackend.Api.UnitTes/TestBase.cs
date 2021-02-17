using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;

namespace CharlieBackend.Api.UnitTest
{
    public abstract class TestBase
    {
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected Mock<ICurrentUserService> _currentUserServiceMock;

        public TestBase()
        {
            _unitOfWorkMock = GetUnitOfWorkMock();
        }

        protected IMapper GetMapper(params Profile[] profiles)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            return new Mapper(configuration);
        }

        protected virtual Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock;
        }

        protected Mock<ICurrentUserService> GetCurrentUserAsExistingStudent(
            long entityId = 1,
            long accountId = 7,
            string email = "serg.Mor@gmail.com",
            UserRole role = UserRole.Student)
        {
            var studentMock = new Mock<ICurrentUserService>();

            studentMock.Setup(user => user.AccountId).Returns(accountId);
            studentMock.Setup(user => user.EntityId).Returns(entityId);
            studentMock.Setup(user => user.Email).Returns(email);
            studentMock.Setup(user => user.Role).Returns(role);

            return studentMock;
        }
    }
}
