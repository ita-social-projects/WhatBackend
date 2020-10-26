using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{

    public class ErrorData
    {
        public ErrorCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
