﻿using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Models.Students;
using CharlieBackend.Panel.Models.Theme;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonCreateViewModel
    {
        public long? EventId { get; set; }

        public string ThemeName { get; set; }
        public IList<ThemeViewModel> Themes { get; set; }

        public long MentorId { get; set; }
        public IList<MentorViewModel> Mentors { get; set; }

        public long? StudentGroupId { get; set; }
        public IList<StudentGroupViewModel> StudentGroups { get; set; }

        public DateTime LessonDate { get; set; }

        public List<VisitDto> LessonVisits { get; set; }

        public IList<StudentViewModel> Students { get; set; }
    }
}
