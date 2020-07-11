using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Mentor
{
    public class UpdateMentorModel : MentorModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        public override string Password { get => base.Password; set => base.Password = value; }

        [JsonPropertyName("first_name")]
        public override string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public override string LastName { get; set; }

        [JsonPropertyName("course_ids")]
        public override List<long> CourseIds { get; set; }
    }
}
