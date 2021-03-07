using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.EventOccurence
{
    public class EventOccurenceViewModel
    {
        public long Id { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }

        public PatternType Pattern { get; set; }

        public long Storage { get; set; }
    }
}
