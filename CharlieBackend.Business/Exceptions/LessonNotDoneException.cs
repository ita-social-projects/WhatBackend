using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Exceptions
{
    public class LessonNotDoneException : Exception
    {
        public LessonNotDoneException(string message) : base(message) { }
    }
}
