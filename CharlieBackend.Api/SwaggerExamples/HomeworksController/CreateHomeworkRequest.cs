using CharlieBackend.Core.DTO.Homework;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    class CreateHomeworkRequest : IExamplesProvider<HomeworkRequestDto>
    {
        public HomeworkRequestDto GetExamples()
        {
            return new HomeworkRequestDto
            {
                AttachmentIds = new List<long> { 3, 12 },
                DueDate = new DateTime(2021, 09, 15),
                LessonId = 7,
                TaskText = "1. Please create new HTML page <br>2. Page should contain 2 tables 2x2 in center"
            };
        }
    }
}
