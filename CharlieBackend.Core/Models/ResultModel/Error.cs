using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{

    public class Error
    {
        public ErrorCode ErrorCode { get; set; } = 0;

        public string ErrorMessage { get; set; }
    }
}
