using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetMentorByIdResponse : IExamplesProvider<MentorMock>
    {
        public MentorMock GetExamples()
        {
            return new MentorMock
            {
                First_name = "Brad",
                Last_name = "Pitt",
                Email = "student@example.com"
            };
        }
    }

    class MentorMock
    {
        public string First_name {get; set;}

        public string Last_name { get; set; }

        public string Email { get; set; }
    }
}
