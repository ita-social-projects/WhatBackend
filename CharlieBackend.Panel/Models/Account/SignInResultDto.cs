using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Account
{
    public class SignInResultDto
    {
        public Dictionary<string, string> RoleList { get; set; }
        public string Token { get; set; }
    }
}
