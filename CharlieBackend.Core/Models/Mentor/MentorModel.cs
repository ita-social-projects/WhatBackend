
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models
{
    public class MentorModel : BaseAccountModel
    {
        [JsonIgnore]
        public override string Password { get; set; }

        [JsonPropertyName("login")]
        public override string Email { get; set; }

        [JsonIgnore]
        public override int Role { get; set; }
        public long[] Courses_id { get; set; }
    }
}
