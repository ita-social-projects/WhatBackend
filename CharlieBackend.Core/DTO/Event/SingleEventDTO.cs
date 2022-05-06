using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Event
{
    public class SingleEventDTO : CreateSingleEventDto
    {
        public long Id { get; set; }
    }
}
