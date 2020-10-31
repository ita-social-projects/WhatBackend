using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class UpdateStudentDto
    {
        [Required]
        [JsonIgnore]
        public long Id { get; set; }

        //public new string Email
        //{
        //    get => base.Email;
        //    set => base.Email = value;
        //}

        //public string Password
        //{
        //    get => base.Password;
        //    set => base.Password = value;
        //}

        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [JsonPropertyName("student_group_ids")]
        public List<long> StudentGroupIds { get; set; }
    }
}
