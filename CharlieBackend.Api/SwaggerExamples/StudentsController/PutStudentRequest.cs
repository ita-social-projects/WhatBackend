using CharlieBackend.Core.DTO.Student;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class PutStudentRequest : IExamplesProvider<UpdateStudentDto>
    {
        public UpdateStudentDto GetExamples()
        {
            return new UpdateStudentDto()
            {
                Email = "student1@example.com",
                StudentGroupIds = new List<long>() { 12, 54, 22 },
                FirstName = "Vasiliy",
                LastName = "Pupkin"
            };
        }
    }
}
