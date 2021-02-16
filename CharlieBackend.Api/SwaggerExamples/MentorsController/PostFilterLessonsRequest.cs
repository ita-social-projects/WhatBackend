using CharlieBackend.Core.DTO.Lesson;
using Swashbuckle.AspNetCore.Filters;
using System;


namespace CharlieBackend.Api.SwaggerExamples.MentorsController
{
    internal class PostFilterLessonsRequest : IExamplesProvider<FilterLessonsRequestDto>
    {
        public FilterLessonsRequestDto GetExamples()
        {
            return new FilterLessonsRequestDto()
            {
                StudentGroupId = 1,
                StartDate = new DateTime(2015, 7, 20, 18, 30, 25),
                FinishDate = new DateTime(2015, 7, 20, 18, 30, 25)
            };
        }
    }
}
