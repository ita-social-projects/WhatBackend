using AutoMapper;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Moq;

namespace CharlieBackend.Api.UnitTest
{
    public abstract class TestBase
    {
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public TestBase()
        {
            _unitOfWorkMock = GetUnitOfWorkMock();
        }

        protected IMapper GetMapper(params Profile[] profiles)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            return new Mapper(configuration);
        }

        protected abstract Mock<IUnitOfWork> GetUnitOfWorkMock();
    }
}
