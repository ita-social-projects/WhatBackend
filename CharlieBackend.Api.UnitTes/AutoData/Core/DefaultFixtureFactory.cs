using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    /// <summary>
    /// Default factory for creating custom Fixture classes. 
    /// The factory is used to pass <seealso cref="GetFixture"/> delegate to <seealso cref="AutoFixture.Xunit2.AutoDataAttribute"/> constructor
    /// in <seealso cref="CustomAutoDataAttributes"/> namespace classes.
    /// </summary>
    /// <typeparam name="TFixture">The type of Fixture class that factory is intended to create</typeparam>
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
