using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    public interface IFixtureFactory<TFixture> where TFixture : IFixture
    {
        public IFixture GetFixture();

        public static implicit operator Func<IFixture>(IFixtureFactory<IFixture> defaultFixtureFactory);
    }
}
