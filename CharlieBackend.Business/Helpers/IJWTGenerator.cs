using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Helpers
{
    public interface IJwtGenerator
    {
        string GenerateEncodedJwt(AccountDto account, UserRole role);
    }
}
