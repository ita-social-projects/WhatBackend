using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Course
{
    public class CreateCourseModel : CourseModel
    {
        [JsonIgnore]
        public override long Id { get; set; }
    }
}
