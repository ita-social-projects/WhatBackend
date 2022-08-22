using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

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
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Teresa Flores",
                        StudentAverageMark = 4
                    },
                    new AverageStudentMarkDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageMark = 5
                    }
                },
                AverageStudentVisits = new List<AverageStudentVisitsDto>
                {
                    new AverageStudentVisitsDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Teresa Flores",
                        StudentAverageVisitsPercentage = 100
                    },
                    new AverageStudentVisitsDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageVisitsPercentage = 96
                    },
                },
                AverageStudentHomeworkMarks = new List<AverageStudentMarkDto>
                {
                    new AverageStudentMarkDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Teresa Flores",
                        StudentAverageMark = 90
                    },
                    new AverageStudentMarkDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageMark = 75
                    }
                }
            };
        }
    }
}
