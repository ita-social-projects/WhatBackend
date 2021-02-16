using CharlieBackend.Core.DTO.HomeworkStudent;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.HomeworkStudentController
{
    class CreateHomeworkStudentResponse : IExamplesProvider<HomeworkStudentDto>
    {
        public HomeworkStudentDto GetExamples()
        {
            return new HomeworkStudentDto
            { 
                Id = 1,
                StudentId = 2,
                HomeworkId = 1,
                HomeworkText = "Homework done.... ",
                AttachmentIds =  new List<long>{ 1 }
            };
        }
    }
}
