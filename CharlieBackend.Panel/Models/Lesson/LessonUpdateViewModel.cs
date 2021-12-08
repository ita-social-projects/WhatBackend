using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.Students;
using CharlieBackend.Panel.Models.Theme;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonUpdateViewModel
    {
        public string ThemeName { get; set; }
        public IList<ThemeViewModel> Themes { get; set; }

        public DateTime LessonDate { get; set; }

        public List<VisitDto> LessonVisits { get; set; }

        public IList<StudentViewModel> Students { get; set; }
    }
}
