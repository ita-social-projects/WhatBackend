using CharlieBackend.Core.DTO.Mark;
using CharlieBackend.Core.Models.ResultModel;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IMarkService
    {
        Task<Result<MarkDto>> GetMarkByIdAsync(long id);
    }
}
