using CharlieBackend.Core.DTO.Account;

namespace CharlieBackend.Api.Helpers
{
    public interface IJWTGenerator
    {
        string GenerateEncodedJWT(AccountDto account);
    }
}
