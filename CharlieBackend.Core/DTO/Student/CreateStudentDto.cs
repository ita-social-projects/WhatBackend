using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class CreateStudentDto
    {
        [Required]
        [JsonIgnore]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [JsonIgnore]
        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonIgnore]
        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
