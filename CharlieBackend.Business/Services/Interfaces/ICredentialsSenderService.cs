using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface ICredentialsSenderService
    {
        public Task SendCredentialsAsync(string email, string password);
    }
}
