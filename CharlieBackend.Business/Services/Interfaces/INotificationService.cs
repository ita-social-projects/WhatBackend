using CharlieBackend.Core.Entities;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface INotificationService
    {
        Task AccountApproved(Account account);

        Task RegistrationSuccess(Account account);

        Task ForgotPasswordNotify(string recepientMail, string url);
    }
}
