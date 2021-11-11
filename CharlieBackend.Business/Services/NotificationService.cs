using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.IntegrationEvents.Events;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class NotificationService : INotificationService
    {
        private const string queueAccountApproved = "AccountApproved";
        private const string queueRegistrationSuccess = "RegistrationSuccess";
        private const string queueForgotPassword = "ForgotPassword";
        private readonly IBus _bus;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IBus bus, ILogger<NotificationService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task AccountApproved(Account account)
        {
            _logger.LogInformation($"AccountApprovedEvent has been sent for user " +
                                   $"{account.FirstName} {account.LastName}");

            await _bus.SendReceive.SendAsync(queueAccountApproved, new AccountApprovedEvent(account.Email,
                                       account.FirstName, account.LastName, account.Role));
        }

        public async Task RegistrationSuccess(Account account)
        {
            _logger.LogInformation($"RegistrationSuccessEvent has been sent for user " +
                                   $"{account.FirstName} {account.LastName}");

            await _bus.SendReceive.SendAsync(queueRegistrationSuccess, new RegistrationSuccessEvent(account.Email,
                                       account.FirstName, account.LastName));
        }

        public async Task ForgotPasswordNotify(string recepientMail, string url)
        {
            _logger.LogInformation($"ForgotPasswordNotify has been sent for email " +
                                   $"{recepientMail}");

            await _bus.SendReceive.SendAsync(queueForgotPassword, new ForgotPasswordEvent(recepientMail, url));
        }
    }
}
