using CharlieBackend.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Mark
{
    public class MarkViewModel
    {
        [Required]
        public long Id { get; set; }

        public sbyte Value { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }

        public DateTime EvaluationDate { get; set; }

        public MarkType Type { get; set; }

        [Required]
        public long EvaluatedBy { get; set; }
    }
}
