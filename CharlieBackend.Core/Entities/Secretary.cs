using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public class Secretary : BaseEntity
    {
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
