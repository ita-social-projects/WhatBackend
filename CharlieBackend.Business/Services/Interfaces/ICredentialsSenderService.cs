using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICredentialsSenderService
    {
        public Task<bool> SendCredentialsAsync(string email, string password);
    }
}
