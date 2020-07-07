
using System.Collections.Generic;
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
        public virtual List<long> Courses_id { get; set; }
    }
}
