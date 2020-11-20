using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.MentorsController
{
    public class PutMentorResponse : IExamplesProvider<MentorDto>
    {
        public MentorDto GetExamples()
        {
            return new MentorDto
            {
                Id = 47,
                Email = "mentor@example.com",
                FirstName = "Steve",
                LastName = "Donaldson"
            };
        }
    }
}
