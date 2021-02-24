using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class SecretaryDetailsDto : SecretaryDto
    {
        public string AvatarUrl { get; set; }
    }
}
