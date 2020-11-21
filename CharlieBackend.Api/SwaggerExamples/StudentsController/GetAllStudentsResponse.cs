using CharlieBackend.Core.DTO.Student;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetAllStudentsResponse : IExamplesProvider<IList<StudentDto>>
    {
        public IList<StudentDto> GetExamples()
        {
            return new List<StudentDto>()
            {
                new StudentDto
                {
                    Id = 47,
                    Email = "student1@example.com",
                    FirstName = "David",
                    LastName = "Dadidson"
                },
                new StudentDto
                {
                    Id = 49,
                    Email = "student2@example.com",
                    FirstName = "Viktor",
                    LastName = "Dodon"
                }
            };
        }
    }
}
