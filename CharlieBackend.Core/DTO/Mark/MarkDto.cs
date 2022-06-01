using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Core.DTO.Mark
{
    public class MarkDto
    {
        public long Id { get; set; }

        public sbyte Value { get; set; }
        public string Comment { get; set; }

        public DateTime EvaluationDate { get; set; }

        public MarkType Type { get; set; }

        public long EvaluatedBy { get; set; }
    }
}
