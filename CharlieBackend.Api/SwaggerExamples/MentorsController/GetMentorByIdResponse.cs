using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetMentorByIdResponse : IExamplesProvider<MentorDto>
    {

        public MentorDto GetExamples()
        {
            return new MentorDto
            {
                FirstName = "Brad",
                LastName = "Pitt",
                Email = "student@example.com"
            };
        }
    }

}
