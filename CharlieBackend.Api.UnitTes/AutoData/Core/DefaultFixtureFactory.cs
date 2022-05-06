using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    public class DefaultFixtureFactory<TFixture> : IFixtureFactory<TFixture> where TFixture : IFixture, new()
    {
        public IFixture GetFixture()
        {
            return new TFixture();
        }

        public static implicit operator Func<IFixture>(DefaultFixtureFactory<TFixture> defaultFixtureFactory) 
            => defaultFixtureFactory.GetFixture;
    }
}
