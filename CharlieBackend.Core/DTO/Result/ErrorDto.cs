using CharlieBackend.Core.Models.ResultModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Result
{
    public class ErrorDto
    {
        [JsonProperty(PropertyName = "Errors")]
        public ErrorData Error { get; set; }
    }
}
