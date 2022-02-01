using CharlieBackend.Panel.Models.Secretary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface ISecretaryService
    {
        Task<IList<SecretaryViewModel>> GetAllSecretariesAsync();

        Task<bool> DisableSecretaryAsync(long id);

        Task<bool> EnableSecretaryAsync(long id);
    }
}
