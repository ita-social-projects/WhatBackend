using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Entities
{
    public interface IBaseEntity
    {
        public long Id { get; set; }
    }
}
