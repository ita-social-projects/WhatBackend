using CharlieBackend.Core.DTO.Homework;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace CharlieBackend.Api.SwaggerExamples.HomeworksController
{
    class GetHomeworkRequest : IExamplesProvider<GetHomeworkRequestDto>
    {
        public GetHomeworkRequestDto GetExamples()
        {
            return new GetHomeworkRequestDto 
            { 
                CourseId = null,
                GroupId = null,
                ThemeId = null,
                StartDate = new DateTime(2011, 09, 15),
                FinishDate = new DateTime(2021, 09, 15)
            };
        }
    }
}
