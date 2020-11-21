using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.ResultModel
{
    public class ErrorData
    {
        public ErrorCode Code { get; set; }

        public string Message { get; set; }
    }
}
