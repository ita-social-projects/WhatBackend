using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.AutoData.SpecimenBuilders
{
    public class IdSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Random _randomNumberGenerator = new Random();

        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(long))
            {
                return _randomNumberGenerator.Next(0, int.MaxValue);
            }

            return new NoSpecimen();
        }
    }
}
