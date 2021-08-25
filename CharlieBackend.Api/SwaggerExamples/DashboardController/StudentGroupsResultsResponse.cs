using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    class StudentGroupsResultsResponse : IExamplesProvider<StudentGroupsResultsDto>
    {
        public StudentGroupsResultsDto GetExamples()
        {
            return new StudentGroupsResultsDto
            {
                AverageStudentGroupsMarks = new List<AverageStudentGroupMarkDto> 
                { 
                    new AverageStudentGroupMarkDto
                    {
                        AverageMark = 4,
                        StudentGroup = "PZ-19-1",
                        Course = "Naturalism"
                    }
                },
                AverageStudentGroupsVisits =  new List<AverageStudentGroupVisitDto> 
                { 
                    new AverageStudentGroupVisitDto
                    {
                        AverageVisitPercentage = 66,
                        StudentGroup = "PZ-19-1",
                        Course = "Naturalism"
                    }
                }
            };
        }
    }
}
