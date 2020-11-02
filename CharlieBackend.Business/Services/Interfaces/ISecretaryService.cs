using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ISecretaryService
    {
        public Task<SecretaryDto> CreateSecretaryAsync(CreateSecretaryDto secretaryDto);

        public Task<IList<SecretaryDto>> GetAllSecretariesAsync();

        public Task<long?> GetAccountId(long mentorId);

        public Task<SecretaryDto> UpdateSecretaryAsync(UpdateSecretaryDto secretaryDto);

        public Task<SecretaryDto> GetSecretaryByAccountIdAsync(long accountId);

    }
}
