using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.Account
{
    public class SignInResultDto
    {
        public Dictionary<string, string> RoleList { get; set; }
        public string Token { get; set; }
    }
}
