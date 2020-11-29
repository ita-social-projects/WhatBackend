using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    class StudentsClassbookRequestExample : IExamplesProvider<StudentsClassbookRequestDto>
    {
        public StudentsClassbookRequestDto GetExamples()
        {
            return new StudentsClassbookRequestDto
            {
                CourseId = 5,
                StartDate = new DateTime(2015, 7, 20),
                FinishDate = new DateTime(2021, 8, 20),
                IncludeAnalytics = new ClassbookResultType[]
                {
                    ClassbookResultType.StudentMarks,
                    ClassbookResultType.StudentPresence,
                }
            };
        }
    }
}
