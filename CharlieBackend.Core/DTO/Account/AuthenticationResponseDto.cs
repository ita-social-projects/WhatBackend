using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Account
{
    public class AuthenticationResponseDto
    {
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public Dictionary<string, string> RoleList { get; set; }
    }
}
