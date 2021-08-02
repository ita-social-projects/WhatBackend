using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using System.Collections.Generic;

namespace CharlieBackend.Business.Helpers
{
    public interface IJwtGenerator
    {
        public Dictionary<string, string> GetRoleJwtDictionary(AccountDto account, Dictionary<UserRole, long> roleIds);
        public string GenerateEncodedJwt(AccountDto account, UserRole role, long roleId);
    }
}
