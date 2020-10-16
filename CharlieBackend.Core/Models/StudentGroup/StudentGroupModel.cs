using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }

        [JsonIgnore]
        [JsonPropertyName("course_id")]
        public virtual long? CourseId { get; set; }

        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [JsonPropertyName("start_date")]
        public virtual string StartDate { get; set; }

        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [JsonPropertyName("finish_date")]
        public virtual string FinishDate { get; set; }

        [JsonIgnore]
        [JsonPropertyName("student_ids")]
        public virtual List<long> StudentIds { get; set; }
    }
}
