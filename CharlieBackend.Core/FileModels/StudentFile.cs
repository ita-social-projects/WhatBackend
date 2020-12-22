using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.FileModels
{
    public class StudentFile
    {
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
    }
}
