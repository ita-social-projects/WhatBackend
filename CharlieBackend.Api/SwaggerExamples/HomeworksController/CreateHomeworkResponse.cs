using CharlieBackend.Core.DTO.Homework;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    class CreateHomeworkResponse : IExamplesProvider<HomeworkDto>
    {
        public HomeworkDto GetExamples()
        {
            return new HomeworkDto
            {
                Id = 24,
                AttachmentIds = new List<long> { 3, 12 },
                DueDate = new DateTime(2021, 09, 15),
                LessonId = 7,
                TaskText = "1. Please create new HTML page <br>2. Page should contain 2 tables 2x2 in center",
                PublishingDate = new DateTime(2021, 09, 07),
                CreatedBy = 2
            };
        }
    }
}
