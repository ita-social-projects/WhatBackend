using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Interfaces
{
    public interface ISoftDeletingModel
    {
        public bool IsDeleted { get; set; }
    }
}
