using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }

        [JsonIgnore]
        [JsonPropertyName("course_id")]
        public virtual long? CourseId { get; set; }

        // [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [DataType(DataType.Date)]
        [JsonPropertyName("start_date")]
        public virtual DateTime StartDate { get; set; }

        //[RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$")]
        [DataType(DataType.Date)]
        [JsonPropertyName("finish_date")]
        public virtual DateTime FinishDate { get; set; }

        [JsonIgnore]
        [JsonPropertyName("student_ids")]
        public virtual List<long> StudentIds { get; set; }
    }
}
