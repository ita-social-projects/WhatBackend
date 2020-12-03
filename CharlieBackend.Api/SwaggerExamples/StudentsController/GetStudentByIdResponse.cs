using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetStudentByIdResponse : IExamplesProvider<StudentMock>
    {
        public StudentMock GetExamples()
        {
            return new StudentMock
            {
                First_name = "Brad",
                Last_name = "Pitt",
                Email = "student@example.com"
            };
        }
    }

    class StudentMock
    {
        public string First_name {get; set;}

        public string Last_name { get; set; }

        public string Email { get; set; }
    }
}
