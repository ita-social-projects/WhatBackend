using CharlieBackend.Core.DTO.Dashboard;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

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
                        StudentId = 14,
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        MarkId = 5,
                        Mark = 95
                    },
                    new StudentMarkDto
                    {
                        Student = "Erik Brown",
                        StudentId = 15,
                        Course = "Naturalism",
                        StudentGroup = "PZ-19-1",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Mark = 0,
                    },
                },
                StudentsPresences = new List<StudentVisitDto>
                {
                    new StudentVisitDto
                    {
                        Student = "John Coffee",
                        StudentId = 14,
                        Course = "Naturalism",
                        StudentGroup = "AM-12",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = true
                    },
                    new StudentVisitDto
                    {
                        Student = "Erik Brown",
                        StudentId = 15,
                        Course = "Naturalism",
                        StudentGroup = "AM-12",
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = false
                    }
                }
            };
        }
    }
}
