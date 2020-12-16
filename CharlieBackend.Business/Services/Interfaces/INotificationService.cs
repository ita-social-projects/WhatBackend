using System.Threading.Tasks;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface INotificationService
    {
        Task AccountApproved(Account account);

        Task RegistrationSuccess(Account account);

        Task ForgotPasswordNotify(string recepientMail, string url);
    }
}
