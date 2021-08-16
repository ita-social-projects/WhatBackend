using CharlieBackend.Core.DTO.HomeworkStudent;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.HomeworkStudentController
{
    class CreateHomeworkStudentRequest : IExamplesProvider<HomeworkStudentRequestDto>
    {
        public HomeworkStudentRequestDto GetExamples()
        {
            return new HomeworkStudentRequestDto
            {
                HomeworkId = 1,
                HomeworkText = "Create Table",
                AttachmentIds = new List<long>{ 1, 3 },
                IsSent = true
            };
        }
    }
}
