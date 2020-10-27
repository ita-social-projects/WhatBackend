using System;
using System.Linq;
using System.Collections.Generic;

namespace CharlieBackend.Core.Models.ResultModel
{

    public class ErrorData
    {
        public ErrorCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
