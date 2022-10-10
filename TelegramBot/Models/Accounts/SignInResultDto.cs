using System.Collections.Generic;

namespace TelegramBot.Models.Accounts
{
    public class SignInResultDto
    {
        public Dictionary<string, string> RoleList { get; set; }
        public string Token { get; set; }
    }
}
