using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Mentor
{
    public class CreateMentorModel : MentorModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        [JsonPropertyName("login")]
        public override string Email { get; set; }

        [Required]
        public override string FirstName { get; set; }

        [Required]
        public override string LastName { get; set; }

        [Required]
        public override List<long> Courses_id { get; set; }
    }
}
