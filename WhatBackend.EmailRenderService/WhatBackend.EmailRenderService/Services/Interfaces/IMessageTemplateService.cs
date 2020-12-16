using System.Threading.Tasks;
using CharlieBackend.Core.IntegrationEvents.Events;

namespace WhatBackend.EmailRenderService.Services.Interfaces
{
    public interface IMessageTemplateService
    {
        string GetEmailNotifyTemplate(string message);
    }
}
