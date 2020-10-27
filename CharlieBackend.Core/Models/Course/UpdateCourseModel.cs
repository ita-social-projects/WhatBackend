using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Course
{
    public class UpdateCourseModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

    }
}
