using CharlieBackend.Core.DTO.Lesson;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
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
