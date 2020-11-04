using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models
{
    public class MentorModel : BaseAccountModel
    {
        [JsonIgnore]
        public virtual long? AccountId { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public override string Password { get; set; }

        [JsonIgnore]
        public override int Role { get; set; }

        [Required]
        [JsonIgnore]
        [JsonPropertyName("course_ids")]
        public virtual List<long> CourseIds { get; set; }

        [Required]
        [JsonIgnore]
        public override bool IsActive { get; set; }
    }
}
