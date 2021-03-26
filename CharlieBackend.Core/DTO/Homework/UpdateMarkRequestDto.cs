using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Homework
{
    public class UpdateMarkRequestDto
    {
        public long? StudentHomeworkId { get; set; }
        public int? StudentMark { get; set; }
    }
}
