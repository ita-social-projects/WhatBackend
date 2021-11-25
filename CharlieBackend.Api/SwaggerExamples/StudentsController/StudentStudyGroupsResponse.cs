using CharlieBackend.Core.DTO.Student;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class StudentStudyGroupsResponse : IExamplesProvider<IList<StudentStudyGroupsDto>>
    {
        public IList<StudentStudyGroupsDto> GetExamples()
        {
            return new List<StudentStudyGroupsDto>()
            {
                new StudentStudyGroupsDto()
                {
                    Id = 2,
                    Name = "DP-18"
                },
                new StudentStudyGroupsDto()
                {
                    Id = 14,
                    Name = "DP-20"
                }
            };
        }
    }
}
