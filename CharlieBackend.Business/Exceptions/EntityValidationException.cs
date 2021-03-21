using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Exceptions
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException(string message) : base(message) { }
    }
}
