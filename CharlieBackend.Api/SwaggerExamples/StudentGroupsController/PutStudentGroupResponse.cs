using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentGroupsController
{
    class PutStudentGroupResponse : IExamplesProvider<UpdateStudentGroupDto>
    {
        public UpdateStudentGroupDto GetExamples()
        {
            return new UpdateStudentGroupDto
            {
                CourseId = 13,
                Name = "LVX-18",
                StartDate = new DateTime(2015, 7, 20),
                FinishDate = new DateTime(2015, 7, 20)
            };
        }
    }
}
