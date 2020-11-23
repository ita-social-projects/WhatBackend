using System.Threading.Tasks;
using CharlieBackend.Core.IntegrationEvents.Events;

namespace WhatBackend.EmailRenderService.Services.Interfaces
{
    public interface IMessageTemplateService
    {
        public string AccountApprovedTemplate(AccountApprovedEvent message);

        public string RegistrationSuccessTemplate(RegistrationSuccessEvent message);
    }
}
