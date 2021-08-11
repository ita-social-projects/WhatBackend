using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    class StudentsClassbookResponse : IExamplesProvider<StudentsClassbookResultDto>
    {
        public StudentsClassbookResultDto GetExamples()
        {
            return new StudentsClassbookResultDto
            {
                StudentsMarks = new List<StudentMarkDto>
                {
                    new StudentMarkDto
                    {
                        Student = "John Coffee",
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 5,
                    },
                    new StudentMarkDto
                    {
                        Student = "John Coffee",
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 4,
                    },
                },
                StudentsPresences = new List<StudentVisitDto>
                {
                    new StudentVisitDto
                    {
                        Student = "John Brown",
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = true
                    },
                    new StudentVisitDto
                    {
                        Student = "John Brown",
                        Course = "Applied mathematics",
                        StudentGroup = "AM-12",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = true
                    }
                }
            };
        }
    }
}
