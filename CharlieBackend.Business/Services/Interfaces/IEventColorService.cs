using CharlieBackend.Core.DTO;
using CharlieBackend.Core.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface  IEventColorService
    {
        public Task<Result<IList<EventColorDTO>>> GetAllEventColorsAsync();

        public Task<Result<EventColorDTO>> GetEventColorByIdAsync(long id);
    }
}
