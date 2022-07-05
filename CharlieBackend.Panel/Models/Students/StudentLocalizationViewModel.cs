using CharlieBackend.Panel.Controllers;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Students
{
    public class StudentLocalizationViewModel
    {
        public IEnumerable<StudentViewModel> StudentViews { get; set; }
        public IStringLocalizer<StudentsController> StringLocalizer { get; set; }
    }
}
