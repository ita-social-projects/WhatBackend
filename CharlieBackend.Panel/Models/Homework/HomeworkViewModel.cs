using CharlieBackend.Panel.Models.Mentor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.Homework
{
    public class HomeworkViewModel
    {
        [Required]
        public long Id { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(5000)]
        public string TaskText { get; set; }

        [Required]
        public long LessonId { get; set; }

        public DateTime PublishingDate { get; set; }

        public long CreatedBy { get; set; }
        public string ThemeName { get; set; }

        public virtual IList<long> AttachmentIds { get; set; }
    }
}
