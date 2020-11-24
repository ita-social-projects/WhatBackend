using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface INotificationService
    {
        public Task AccountApproved(Account account);
        public Task RegistrationSuccess(Account account);
    }
}
