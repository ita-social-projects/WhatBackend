using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
    public class UpdateStudentModel : StudentModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        public override string Password { get => base.Password; set => base.Password = value; }

        [JsonPropertyName("first_name")]
        public override string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public override string LastName { get; set; }

        [JsonPropertyName("student_group_ids")]
        public override List<long> StudentGroupIds { get; set; }
    }
}
