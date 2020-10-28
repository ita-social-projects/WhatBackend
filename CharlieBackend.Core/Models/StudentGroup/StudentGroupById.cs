using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupById
    {

        [Required]
        [JsonPropertyName("mentor_ids")]
        public virtual List<long?> MentorIds { get; set; }

        [JsonPropertyName("group_name")]
        [StringLength(100)]
        public virtual string GroupName { get; set; } //why "GroupName" if property "name" in bd???

        [Required]
        [JsonPropertyName("student_ids")]
        public virtual List<long?> StudentIds { get; set; }

        [JsonPropertyName("start_date")]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")] //TODO
        public virtual string StartDate { get; set; }

        [JsonPropertyName("finish_date")]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")] //TODO
        public virtual string FinishDate { get; set; }


    }
}
