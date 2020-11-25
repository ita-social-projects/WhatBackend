using CharlieBackend.Core.DTO.Course;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.CoursesController
{
    public class PostCourseResponse : IExamplesProvider<CourseDto>
    {
        public CourseDto GetExamples()
        {
            return new CourseDto()
            {
                Id = 41,
                Name = "Programming for IoT"
            };
        }
    }
}
