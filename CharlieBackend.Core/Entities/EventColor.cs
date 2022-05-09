using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public class EventColor : BaseEntity
    {
        public string Color { get; set; }

        public virtual IList<EventOccurrence> EventOccurances { get; set; }
    }
}
