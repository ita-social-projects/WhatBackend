using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Student
{
    public class StudentModel : BaseAccountModel
    {
        [JsonIgnore]
        public virtual long? AccountId { get; set; }

        [Required]
        [JsonIgnore]
        [EmailAddress]
        [StringLength(50)]
        public override string Email { get; set; }

        [JsonIgnore]
        [StringLength(65)]
        public override string Password { get; set; }

        [JsonIgnore]
        public override int Role { get; set; }

        [Required]
        [JsonIgnore]
        public override bool IsActive { get; set; }

        [JsonIgnore]
        [JsonPropertyName("student_group_ids")]
        public virtual List<long> StudentGroupIds { get; set; }

    }
}
