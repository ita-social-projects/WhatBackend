using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Course
{
    public class UpdateCourseModel : CourseModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

    }
}
