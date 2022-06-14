using CharlieBackend.Panel.Models.Mark;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IMarkService
    {
        Task<MarkViewModel> GetMarkByIdAsync(long id);
    }
}
