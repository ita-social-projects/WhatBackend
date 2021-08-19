using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using CharlieBackend.Core.DTO.Homework;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    class GetHomeworksResponse : IExamplesProvider<List<HomeworkDto>>
    {
        public List<HomeworkDto> GetExamples()
        {
            return new List<HomeworkDto>()
            {
                new HomeworkDto
                {
                    Id = 2,
                    DueDate = new DateTime(2021, 09, 15),
                    LessonId = 7,
                    AttachmentIds = new List<long>() { 2, 4, 6 },
                    TaskText = "1. Please create new HTML page <br>2. Page should contain 2 tables 2x2 in center",
                    PublishingDate = new DateTime(2021, 09, 07),
                    CreatedBy = 2
                },
                new HomeworkDto
                {
                    Id = 4,
                    DueDate = new DateTime(2021, 09, 19),
                    LessonId = 7,
                    AttachmentIds = new List<long>() { 3, 9 },
                    TaskText = "Please delete previously created page",
                    PublishingDate = new DateTime(2021, 09, 10),
                    CreatedBy = 8
                }
            };
        }
    }
}
