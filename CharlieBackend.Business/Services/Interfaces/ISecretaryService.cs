using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ISecretaryService
    {
        public Task<Result<SecretaryDto>> CreateSecretaryAsync(CreateSecretaryDto secretaryDto);

        public Task<Result<IList<SecretaryDto>>> GetAllSecretariesAsync();

        public Task<long?> GetAccountId(long mentorId);
        public Task<Result<SecretaryDto>> UpdateSecretaryAsync(UpdateSecretaryDto secretaryDto);

        public Task<SecretaryDto> GetSecretaryByAccountIdAsync(long accountId);

    }
}
