using CharlieBackend.Core.DTO.Course;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.CoursesController
{
    public class PostCourseRequest : IExamplesProvider<CreateCourseDto>
    {
        public CreateCourseDto GetExamples()
        {
            return new CreateCourseDto
            {
                Name = "Programming for IoT"
            };
        }
    }
}
