using AutoFixture.Kernel;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.Core
{
    internal class NavigationalPropertiesOmmiter : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;

            if (propInfo != null && typeof(BaseEntity).IsAssignableFrom(propInfo.PropertyType))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
