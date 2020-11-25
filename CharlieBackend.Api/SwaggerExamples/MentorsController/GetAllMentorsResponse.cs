using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.MentorsController
{
    internal class GetAllMentorsResponse : IExamplesProvider<IList<MentorDto>>
    {
        public IList<MentorDto> GetExamples()
        {
            return new List<MentorDto>
            {
                new MentorDto
                {
                    Id = 43,
                    Email = "mentor@example.com",
                    FirstName = "Steve",
                    LastName = "Vozniac"
                },
                new MentorDto
                {
                    Id = 56,
                    Email = "mentor2@example.com",
                    FirstName = "Andy",
                    LastName = "Polanski"
                }
            };
        }
    }
}
