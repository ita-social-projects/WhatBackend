using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    class StudentsResultsRequest : IExamplesProvider<StudentsResultsRequestDto>
    {
        public StudentsResultsRequestDto GetExamples()
        {
            return new StudentsResultsRequestDto
            {
                CourseId = 5,
                StartDate = new DateTime(2015, 7, 20),
                FinishtDate = new DateTime(2020, 7, 20),
                IncludeAnalytics = new StudentResultType[]
                {
                    StudentResultType.AverageStudentMark,
                    StudentResultType.AverageStudentVisits
                }
            };
        }
    }
}
