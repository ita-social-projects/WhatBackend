using AutoFixture;
using CharlieBackend.Api.UnitTest.AutoData.Core;
using CharlieBackend.Api.UnitTest.AutoData.SpecimenBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.Fixtures
{
    public class LessonsRepositoryFixture : Fixture
    {
        public LessonsRepositoryFixture()
        {
            Customizations.Add(new IdSpecimenBuilder());
            Customizations.Add(new NavigationalPropertiesOmmiter());
        }
    }
}
