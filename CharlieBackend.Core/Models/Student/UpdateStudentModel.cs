using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Student
{
    public class UpdateStudentModel : StudentModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }

        public new string Email 
        { 
            get => base.Email; 
            set => base.Email = value; 
        }

        public override string Password 
        { 
            get => base.Password; 
            set => base.Password = value; 
        }

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public override string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public override string LastName { get; set; }

        [JsonPropertyName("student_group_ids")]
        public new List<long> StudentGroupIds { get; set; }
    }
}
