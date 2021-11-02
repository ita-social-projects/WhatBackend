using System;

namespace CharlieBackend.Business.Exceptions
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException(string message) : base(message) { }
    }
}
