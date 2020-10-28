using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CharlieBackend.Core.Models.ResultModel
{
    public enum ErrorCode
    {
        None,
        ValidationError,
        Unauthorized,
        InternalServerError,
        NotFound
    }
}
