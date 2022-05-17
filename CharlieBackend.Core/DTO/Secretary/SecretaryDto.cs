using CharlieBackend.Core.Interfaces;

namespace CharlieBackend.Core.DTO.Secretary
{
    public class SecretaryDto : ICloneable<SecretaryDto>
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public SecretaryDto Clone()
        {
            return new SecretaryDto()
            {
                Id = this.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
        }
    }
}
