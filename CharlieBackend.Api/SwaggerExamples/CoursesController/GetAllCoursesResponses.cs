using CharlieBackend.Core.DTO.Course;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.CoursesController
{
    internal class GetAllCoursesResponses : IExamplesProvider<IList<CourseDto>>
    {
        public IList<CourseDto> GetExamples()
        {
            return new List<CourseDto>
            {
                new CourseDto
                {
                    Id = 47,
                    Name = "Knitting"
                },
                new CourseDto
                {
                    Id = 48,
                    Name = "Painting with gouache"
                }
            };
        }
    }
}
