using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class UpdateSecretaryDto
    {
        #nullable enable

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? FirstName { get; set; }

        [StringLength(30)]
        public string? LastName { get; set; }

        #nullable disable
    }
}
