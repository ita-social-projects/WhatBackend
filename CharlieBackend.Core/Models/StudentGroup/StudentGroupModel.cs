using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.StudentGroup
{
    public class StudentGroupModel
    {
        public string Name { get; set; }

        [JsonPropertyName("course_id")]
        public int CourseId { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("finish_date")]
        public string FinishDate { get; set; }

        [JsonPropertyName("student_ids")]
        public IEnumerable<int> StudentIds { get; set; }
    }
}
