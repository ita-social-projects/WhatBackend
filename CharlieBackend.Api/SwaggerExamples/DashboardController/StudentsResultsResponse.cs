using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    class StudentsResultsResponse : IExamplesProvider<StudentsResultsDto>
    {
        public StudentsResultsDto GetExamples()
        {
            return new StudentsResultsDto
            {
                AverageStudentsMarks = new List<AverageStudentMarkDto>
                {
                    new AverageStudentMarkDto
                    {
                        CourseId = 5,
                        StudentGroupId = 7,
                        StudentId = 12,
                        StudentAverageMark = 4
                    },
                    new AverageStudentMarkDto
                    {
                        CourseId = 5,
                        StudentGroupId = 7,
                        StudentId = 13,
                        StudentAverageMark = 5
                    }
                },
                AverageStudentVisits = new List<AverageStudentVisitsDto>
                {
                    new AverageStudentVisitsDto
                    {
                        CourseId = 5,
                        StudentGroupId = 7,
                        StudentId = 12,
                        StudentAverageVisitsPercentage = 100
                    },
                    new AverageStudentVisitsDto
                    {
                        CourseId = 5,
                        StudentGroupId = 7,
                        StudentId = 12,
                        StudentAverageVisitsPercentage = 96
                    },
                }
            };
        }
    }
}
