using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace CharlieBackend.Core.DTO.Account
{
	public class CreateAccountDto
	{
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(65)]
        public string Password { get; set; }

        [Required]
        [StringLength(65)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }

    }
}
