using AutoFixture;
using CharlieBackend.Api.UnitTest.AutoData.Core;
using CharlieBackend.Api.UnitTest.AutoData.SpecimenBuilders;

namespace CharlieBackend.Api.UnitTest.AutoData.Fixtures
{
    /// <summary>
    /// The Fixture class that is used in <seealso cref="CharlieBackend.Api.UnitTest.RepositoriesTests.LessonRepositoryTests"/>.
    /// It has constraints for values generation of Entity types.
    /// </summary>
    public class LessonsRepositoryFixture : Fixture
    {
        public LessonsRepositoryFixture()
        {
            Customizations.Add(new LongIdSpecimenBuilder());
            Customizations.Add(new NavigationalPropertiesOmmiterSpecimenBuilder());
        }
    }
}
