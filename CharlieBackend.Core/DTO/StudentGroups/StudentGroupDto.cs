using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class StudentGroupDto
    {
        public long Id { get; set; }

        [Required]

       [JsonProperty("course_id")]
        public long? CourseId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [JsonProperty("finish_date")]
        public DateTime FinishDate { get; set; }

        [JsonProperty("student_ids")]
        public IList<long> StudentIds { get; set; }

        [JsonProperty("mentor_ids")]
        public IList<long> MentorIds { get; set; }

    }
}
