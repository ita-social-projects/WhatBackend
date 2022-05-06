using AutoFixture.Xunit2;
using CharlieBackend.Api.UnitTest.AutoData.Core;
using CharlieBackend.Api.UnitTest.AutoData.Fixtures;

namespace CharlieBackend.Api.UnitTest.AutoData.CustomAutoDataAttribute
{
    public class LessonsRepositoryAutoDataAttribute : AutoDataAttribute
    {
        public LessonsRepositoryAutoDataAttribute() : base(new DefaultFixtureFactory<LessonsRepositoryFixture>())
        {

        }
    }
}
