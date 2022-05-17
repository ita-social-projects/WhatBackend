using CharlieBackend.Core.Interfaces;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class UpdateSecretaryDto : ICloneable<UpdateSecretaryDto>
    {
        #nullable enable
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        #nullable disable

        public UpdateSecretaryDto Clone()
        {
            return new UpdateSecretaryDto()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
        }
    }
}
