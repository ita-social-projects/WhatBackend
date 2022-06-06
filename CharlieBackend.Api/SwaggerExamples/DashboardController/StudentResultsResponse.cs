using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    /// <summary>
    /// Class for example data on SwaggerUI, that is complete copy of parental class
    /// with response data grouped in different way
    /// </summary>
    public class StudentResultsDto : StudentsResultsDto
    {

    }

    class StudentResultsResponse : IExamplesProvider<StudentResultsDto>
    {
        public StudentResultsDto GetExamples()
        {
            return new StudentResultsDto
            {
                AverageStudentsMarks = new List<AverageStudentMarkDto>
                {
                    new AverageStudentMarkDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageMark = 30
                    },
                    new AverageStudentMarkDto
                    {
                        Course = "Naturalism",
                        StudentGroup = "AquaVita-2",
                        Student = "Alice Evans",
                        StudentAverageMark = 12
                    }
                },
                AverageStudentVisits = new List<AverageStudentVisitsDto>
                {
                    new AverageStudentVisitsDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AquaVita-2",
                        Student = "Alice Evans",
                        StudentAverageVisitsPercentage = 100
                    },
                    new AverageStudentVisitsDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageVisitsPercentage = 78
                    }
                },
                AverageStudentHomeworkMarks = new List<AverageStudentMarkDto>
                {
                    new AverageStudentMarkDto
                    {
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        Student = "Alice Evans",
                        StudentAverageMark = 42
                    },
                    new AverageStudentMarkDto
                    {
                        Course = "Naturalism",
                        StudentGroup = "AquaVita-2",
                        Student = "Alice Evans",
                        StudentAverageMark = 51
                    }
                }
            };
        }
    }
}
