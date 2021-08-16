using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Core.DTO.HomeworkStudent
{
    public class HomeworkStudentMarkDto
    {
        public int Value { get; set; }
        public string Comment { get; set; }
        public DateTime EvaluationDate { get; set; }
        public MarkType Type { get; set; }
    }
}
