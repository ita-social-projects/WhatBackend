using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Course
{
    public class CreateCourseModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
