using CharlieBackend.Core.DTO.Account;

namespace CharlieBackend.Business.Helpers
{
    public interface IJwtGenerator
    {
        string GenerateEncodedJwt(AccountDto account);
    }
}
