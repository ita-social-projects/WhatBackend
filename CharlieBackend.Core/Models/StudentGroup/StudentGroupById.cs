using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupById
    {
        [JsonPropertyName("mentor_ids")]
        public virtual List<long?> MentorIds { get; set; }

        [JsonPropertyName("group_name")]
        public virtual string GroupName { get; set; }

        [JsonPropertyName("student_ids")]
        public virtual List<long?> StudentIds { get; set; }

        [JsonPropertyName("start_date")]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        public virtual string StartDate { get; set; }

        [JsonPropertyName("finish_date")]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        public virtual string FinishDate { get; set; }
    }
}
