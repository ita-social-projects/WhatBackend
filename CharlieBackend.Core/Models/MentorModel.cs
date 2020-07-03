using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.Models
{
    public class MentorModel : UserModel
    {
        public long[] courses_id { get; set; }
    }
}
