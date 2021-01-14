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
                    MentorId = 7,
                    AttachmentIds = new List<long>() { 2, 4, 6 },
                    StudentGroupId = 3,
                    TaskText = "1. Please create new HTML page <br>2. Page should contain 2 tables 2x2 in center"
                },
                new HomeworkDto
                {
                    Id = 4,
                    DueDate = new DateTime(2021, 09, 19),
                    MentorId = 7,
                    AttachmentIds = new List<long>() { 3, 9 },
                    StudentGroupId = 3,
                    TaskText = "Please delete previously created page"
                }
            };
        }
    }
}
