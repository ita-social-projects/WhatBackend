using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
    public class CreateStudentModel : StudentModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        [JsonPropertyName("first_name")]
        public override string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public override string LastName { get; set; }
    }
}
