using CharlieBackend.Core.DTO.Homework;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    class GetHomeworksByCourseIdResponse : IExamplesProvider<List<HomeworkDto>>
    {
        public List<HomeworkDto> GetExamples()
        {
            return new List<HomeworkDto>
            {
                new HomeworkDto()
                {
                    Id = 24,
                    IsCommon = true,
                    AttachmentIds = new List<long> { 3, 12 },
                    DeadlineDays = 5,
                    MentorId = 7,
                    ThemeId = 3,
                    TaskText = "1. Please create new HTML page \n2. Page should contain 2 tables 2x2 in center",
                },
                new HomeworkDto()
                {
                    Id = 32,
                    IsCommon = true,
                    AttachmentIds = new List<long> { 4, 7 },
                    DeadlineDays = 5,
                    MentorId = 7,
                    ThemeId = 4,
                    TaskText = "1. Please create new HTML page \n2. Page should Japaneese flag 300x200 pixels. And round diameter is 120 pixels. Use only CSS",
                }
            };
        }
    }
}
