using AutoFixture.Kernel;
using CharlieBackend.Core.Entities;
using System;
using System.Reflection;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    /// <summary>
    /// Prevents values generation for 
    /// <see href="https://www.entityframeworktutorial.net/basics/entity-in-entityframework.aspx">EF Core navigational properties</see>.
    /// Fixtures customized with this SpecimenBuilder leave properties that inherite <seealso cref="BaseEntity"/> unset.
    /// </summary>
    internal class NavigationalPropertiesOmmiterSpecimenBuilder : ISpecimenBuilder
    {
        private static readonly Type _typeToOmmit = typeof(BaseEntity);

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;

            if (propInfo != null && _typeToOmmit.IsAssignableFrom(propInfo.PropertyType))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
