using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class SecretaryDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
