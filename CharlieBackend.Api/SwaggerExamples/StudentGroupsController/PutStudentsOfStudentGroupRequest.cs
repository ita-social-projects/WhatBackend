using CharlieBackend.Core.DTO.StudentGroups;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentGroupsController
{
    class PutStudentsOfStudentGroupRequest : IExamplesProvider<UpdateStudentsForStudentGroup>
    {
        public UpdateStudentsForStudentGroup GetExamples()
        {
            return new UpdateStudentsForStudentGroup
            {
                 StudentIds  = new List<long>
                 {
                     12,
                     19,
                     42,
                     92,
                     137
                 }
            };
        }
    }
}
