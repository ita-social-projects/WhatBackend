using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
    public class CreateStudentModel : StudentModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        public new string Email { get; set; }

        [JsonIgnore]
        [JsonPropertyName("first_name")]
        public override string FirstName { get; set; }

        [JsonIgnore]
        [JsonPropertyName("last_name")]
        public override string LastName { get; set; }
    }
}
