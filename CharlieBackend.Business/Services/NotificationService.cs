﻿using EasyNetQ;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.IntegrationEvents.Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Business.Services
{
    public class NotificationService : INotificationService
    {
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

            await _bus.SendReceive.SendAsync("EmailRenderService", new AccountApprovedEvent(account.Email,
                                       account.FirstName, account.LastName, account.Role));
        }
    }
}
