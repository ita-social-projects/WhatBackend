using CharlieBackend.Core.DTO.Account;
using System.Collections.Generic;

namespace CharlieBackend.Business.Helpers
{
    public interface IJwtGenerator
    {
        public Dictionary<string, string> ReturnJwtDictionary(AccountDto account);
    }
}
