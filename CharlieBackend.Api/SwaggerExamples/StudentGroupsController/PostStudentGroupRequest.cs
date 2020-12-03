using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentGroupsController
{
    class PostStudentGroupRequest : IExamplesProvider<CreateStudentGroupDto>
    {
        public CreateStudentGroupDto GetExamples()
        {
            return new CreateStudentGroupDto()
            {
                CourseId = 17,
                Name = "SX-13",
                StartDate = new DateTime(2015, 7, 20),
                FinishDate = new DateTime(2015, 9, 20),
                MentorIds = new List<long> { 12, 43 },
                StudentIds = new List<long> { 33, 42, 43 }
            };
        }
    }
}
