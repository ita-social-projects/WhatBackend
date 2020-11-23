using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentGroupsController
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
