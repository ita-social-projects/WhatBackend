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
                        Course = "Some name",
                        StudentGroup = "Some group",
                        Student = "Ivan Ivanov",
                        StudentAverageMark = 4
                    },
                    new AverageStudentMarkDto
                    {
                        Course = "Some name",
                        StudentGroup = "Some group",
                        Student = "Ivan Ivanov",
                        StudentAverageMark = 5
                    }
                },
                AverageStudentVisits = new List<AverageStudentVisitsDto>
                {
                    new AverageStudentVisitsDto
                    {
                        Course = "Some name",
                        StudentGroup = "Some group",
                        Student = "Ivan Ivanov",
                        StudentAverageVisitsPercentage = 100
                    },
                    new AverageStudentVisitsDto
                    {
                        Course = "Some name",
                        StudentGroup = "Some group",
                        Student = "Ivan Ivanov",
                        StudentAverageVisitsPercentage = 96
                    },
                }
            };
        }
    }
}
