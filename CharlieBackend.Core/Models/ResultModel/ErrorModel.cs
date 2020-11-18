using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Models.ResultModel
{
    public class ErrorModel
    {
        public ErrorBody Error { get; set; }

        public class ErrorBody
        {
            public int Code { get; set; }

            public string Message { get; set; }
        }
    }
}
