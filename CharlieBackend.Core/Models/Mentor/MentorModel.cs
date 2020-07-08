
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models
{
    public class MentorModel : BaseAccountModel
    {
        [JsonIgnore]
        public override string Password { get; set; }

        [JsonIgnore]
        public override int Role { get; set; }

        [JsonPropertyName("course_ids")]
        public virtual List<long> CourseIds { get; set; }
    }
}
