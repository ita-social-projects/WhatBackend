﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class CreateStudentGroupModel : StudentGroupModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        public override string Name 
        { 
            get => base.Name; 
            set => base.Name = value; 
        }

        [Required]
        [JsonPropertyName("course_id")]
        public new long CourseId { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [JsonPropertyName("start_date")]
        public override string StartDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [JsonPropertyName("finish_date")]
        public override string FinishDate { get; set; }

        [Required]
        [JsonPropertyName("student_ids")]
        public new List<long> StudentIds { get; set; }

        [Required]
        [JsonPropertyName("mentor_ids")]
        public List<long> MentorIds { get; set; }
    }
}
