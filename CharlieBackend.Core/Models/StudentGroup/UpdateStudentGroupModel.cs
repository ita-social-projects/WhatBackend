using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class UpdateStudentGroupModel : StudentGroupModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [JsonPropertyName("student_ids")]
        public new List<long> StudentIds { get; set; }

        [JsonPropertyName("course_id")]
        public new long CourseId { get; set; }
    }
}
