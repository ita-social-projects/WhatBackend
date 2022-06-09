using AutoFixture.Kernel;
using System;

namespace CharlieBackend.Api.UnitTest.AutoData.SpecimenBuilders
{
    /// <summary>
    /// Provides constraint for <seealso cref="long"/> type values generation in order to set valid data to Id properties.
    /// </summary>
    public class LongIdSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Random _randomNumberGenerator = new Random();
        private static readonly Type _longType = typeof(long);

        /// <summary>
        /// Adds constraint in case of <seealso cref="long"/> type value generation 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Random integer value that is greater than zero</returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == _longType)
            {
                return _randomNumberGenerator.Next(1, int.MaxValue);
            }

            return new NoSpecimen();
        }
    }
}
