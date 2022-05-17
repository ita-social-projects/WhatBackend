using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Interfaces
{
    public interface ICloneable<T>
    {
        T Clone();
    }
}
