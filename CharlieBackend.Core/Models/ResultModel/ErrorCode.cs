﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{
    public enum ErrorCode
    {
        None = 0,
        ValidationError,
        Unauthorized,
    }
}
