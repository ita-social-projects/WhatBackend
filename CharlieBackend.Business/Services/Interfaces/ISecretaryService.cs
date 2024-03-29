﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ISecretaryService
    {
        Task<Result<SecretaryDto>> CreateSecretaryAsync(long accountId);

        Task<IList<SecretaryDetailsDto>> GetAllSecretariesAsync();

        Task<long?> GetAccountId(long secretaryId);

        Task<Result<SecretaryDto>> UpdateSecretaryAsync(long secretaryId, UpdateSecretaryDto secretaryDto);

        Task<Result<SecretaryDto>> GetSecretaryByAccountIdAsync(long accountId);

        Task<Result<SecretaryDto>> GetSecretaryByIdAsync(long secretaryId);
       
        Task<Result<IList<SecretaryDetailsDto>>> GetActiveSecretariesAsync();
      
        Task<Result<bool>> DisableSecretaryAsync(long secretaryId);

        Task<Result<bool>> EnableSecretaryAsync(long secretaryId);
    }
}
