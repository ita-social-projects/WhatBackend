using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ISecretaryService
    {
        public Task<Result<SecretaryDto>> CreateSecretaryAsync(long accountId);

        public Task<Result<IList<SecretaryDto>>> GetAllSecretariesAsync();

        public Task<long?> GetAccountId(long secretaryId);

        public Task<Result<SecretaryDto>> UpdateSecretaryAsync(long secretaryId, UpdateSecretaryDto secretaryDto);

        public Task<Result<SecretaryDto>> GetSecretaryByAccountIdAsync(long accountId);

        public Task<Result<SecretaryDto>> GetSecretaryByIdAsync(long secretaryId);

    }
}
