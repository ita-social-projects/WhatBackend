using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models
{
    public class GetAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
    }
}
