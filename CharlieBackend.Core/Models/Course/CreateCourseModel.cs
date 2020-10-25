using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Course
{
    public class CreateCourseModel : CourseModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }
    }
}
