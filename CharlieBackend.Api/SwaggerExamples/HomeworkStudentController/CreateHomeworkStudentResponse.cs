using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
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
                PublishingDate = new DateTime(2021, 09, 12),
                IsSent = false,
                AttachmentIds = new List<long> { 1 },
                Mark = new HomeworkStudentMarkDto
                {
                    Value = 5,
                    Comment = "Change code at line 23",
                    EvaluationDate = new DateTime(2021, 09, 10),
                    Type = MarkType.Homework
                }
            };
        }
    }
}
