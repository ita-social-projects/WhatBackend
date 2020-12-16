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
                        StudentId = 33,
                        CourseId = 4,
                        StudentGroupId = 31,
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 5,
                    },
                    new StudentMarkDto
                    {
                        StudentId = 34,
                        CourseId = 4,
                        StudentGroupId = 31,
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        StudentMark = 4,
                    },
                },
                StudentsPresences = new List<StudentVisitDto>
                {
                    new StudentVisitDto
                    {
                        StudentId = 33,
                        CourseId = 4,
                        StudentGroupId = 31,
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = true
                    },
                    new StudentVisitDto
                    {
                        StudentId = 34,
                        CourseId = 4,
                        StudentGroupId = 31,
                        LessonId = 345,
                        LessonDate = new DateTime(2017, 5, 21),
                        Presence = true
                    }
                }
            };
        }
    }
}
