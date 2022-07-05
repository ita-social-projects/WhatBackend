using CharlieBackend.Panel.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.Mentor
{
    public class MentorLocalizationViewModel
    {
        public IEnumerable<MentorViewModel> MentorViews { get; set; }

        public IStringLocalizer<MentorsController> StringLocalizer { get; set; }
    }
}
