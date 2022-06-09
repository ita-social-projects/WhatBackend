using AutoFixture;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    public interface IFixtureFactory<TFixture> where TFixture : IFixture
    {
        public IFixture GetFixture();
    }
}
