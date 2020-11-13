using System.Threading.Tasks;
using CharlieBackend.Core.DTO.EmailData;

namespace WhatBackend.EmailSendingService.Services.Interfaces
{
    public interface IEmailSender
    {
        public Task SendMessageAsync(EmailData data);
    }
}
