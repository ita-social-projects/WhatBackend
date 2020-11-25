using CharlieBackend.Core.DTO.Student;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class PostStudentResponse : IExamplesProvider<StudentDto>
    {
        public StudentDto GetExamples()
        {
            return new StudentDto()
            {
                Id = 12,
                Email = "student@example.com",
                FirstName = "Muhhamad",
                LastName = "Ali"
            };
        }
    }
}
