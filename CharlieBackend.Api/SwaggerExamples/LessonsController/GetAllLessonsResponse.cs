using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class GetAllLessonsResponse : IExamplesProvider<List<LessonDto>>
    {
        public List<LessonDto> GetExamples()
        {
            return new List<LessonDto>
            {
                new LessonDto
                {
                    Id = 34,
                    ThemeName = "Some theme",
                    MentorId = 2,
                    LessonDate = new DateTime(2015, 7, 20, 18, 30, 25),
                    StudentGroupId = 22,
                    LessonVisits = new List<VisitDto>()
                {
                    new VisitDto()
                    {
                        StudentId = 45,
                        MarkId = 5,
                        Presence = true
                    },
                    new VisitDto()
                    {
                        StudentId = 74,
                        Presence = false
                    }
                } },
                new LessonDto
                {
                    Id = 56,
                    ThemeName = "Theme 2",
                    MentorId = 2,
                    LessonDate = new DateTime(2015, 8, 20, 18, 30, 25),
                    StudentGroupId = 22,
                    LessonVisits = new List<VisitDto>()
                {
                    new VisitDto()
                    {
                        StudentId = 45,
                        MarkId = 4,
                        Presence = true
                    },
                    new VisitDto()
                    {
                        StudentId = 74,
                        MarkId = 4,
                        Presence = true
                    }
                }
            }
        };
        }
    }
}
