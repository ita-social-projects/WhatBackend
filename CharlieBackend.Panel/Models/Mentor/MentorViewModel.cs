﻿using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Mentor
{
    public class MentorViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }
    }
}
