using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class UpdateStudentDto
    {
        [Required]
        [StringLength(65)]
        public string Password { get; set; }

        [StringLength(65)]
        public string? NewPassword { get; set; }

        [StringLength(65)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
        public string? ConfirmNewPassword { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? FirstName { get; set; }

        [StringLength(30)]
        public string? LastName { get; set; }

        public List<long>? StudentGroupIds { get; set; }
    }
}
