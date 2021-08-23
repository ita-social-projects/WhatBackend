using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.DashboardController
{
    /// <summary>
    /// Class for example data on SwaggerUI, that is complete copy of parental class
    /// with response data grouped in different way
    /// </summary>
    public class StudentClassbookResultDto : StudentsClassbookResultDto
    {

    }

    class StudentClassbookResponse : IExamplesProvider<StudentClassbookResultDto>
    {
        public StudentClassbookResultDto GetExamples()
        {
            return new StudentClassbookResultDto
            {
                StudentsMarks = new List<StudentMarkDto>
                {
                    new StudentMarkDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 5,
                    },
                    new StudentMarkDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Naturalism",
                        StudentGroup = "PZ-12-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 11,
                    },
                    new StudentMarkDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Capitalism",
                        StudentGroup = "PZ-33-4",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 8,
                    },
                },
                StudentsPresences = new List<StudentVisitDto>
                {
                    new StudentVisitDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = false
                    },
                    new StudentVisitDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Naturalism",
                        StudentGroup = "PZ-12-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = false
                    },
                    new StudentVisitDto
                    {
                        Student = "Erik Brown",
                        StudentId = 155,
                        Course = "Capitalism",
                        StudentGroup = "PZ-33-4",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = false
                    }
                }
            };
        }
    }
}

