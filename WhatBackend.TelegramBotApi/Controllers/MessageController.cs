using CharlieBackend.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.DependencyInjection;
using CharlieBackend.Business.Services.Interfaces;
using System.Security.Claims;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Models.Commands;
using CharlieBackend.Business.Models.Commands.Attributes;
using CharlieBackend.Core.Entities;

namespace WhatBackend.TelergamBot.Controllers
{
    [ApiController]
    [Route("api/bot/message")]
    public class MessageController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<MessageHeader, Message> _messages;

        public MessageController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _messages = new Dictionary<MessageHeader, Message>();
        }
        [HttpGet]
        public string Get()
        {
            return "Bot is up and running!";
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            var client = await Bot.Get(_serviceProvider);
            var commands = Bot.Commands;
            Message message = update.Message ?? update.EditedMessage;

            var accountService = _serviceProvider
                .GetService<IAccountService>();
            var httpContextAccessor = _serviceProvider
                .GetService<IHttpContextAccessor>();
            var identity = (ClaimsIdentity)httpContextAccessor
                .HttpContext.User.Identity;
            
            if (message == null)
            {
                if (update.CallbackQuery == null)
                {
                    return BadRequest();
                }
                else
                {
                    message = new Message
                    {
                        Chat = new Chat()
                        {
                            Id = update.CallbackQuery.From.Id
                        },
                        MessageId = update.Id,
                        Text = update.CallbackQuery.Data
                    };
                }
            }

            var messageHeader = new MessageHeader()
            {
                ChatId = message.Chat.Id,
                MessageId = message.MessageId
            };

            var account = await accountService
                .GetAccountByTelegramId(message.Chat.Id);

            identity.AddClaim(new Claim(
                        ClaimConstants.AccountClaim, account.Id.ToString()));
            identity.AddClaim(new Claim(
                        ClaimConstants.EmailClaim, account.Email));

            var isNewMessage = _messages.TryAdd(messageHeader, message);

            if (isNewMessage)
            {

                foreach (var command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        if(Attribute.GetCustomAttribute(command.GetType(),
                            typeof(StudentRoleCommandAttribute)) != null)
                        {
                            identity.AddClaim(new Claim(
                                ClaimsIdentity.DefaultRoleClaimType,
                                UserRole.Student.ToString()));
                            var studentService = _serviceProvider
                                .GetService<IStudentService>();
                            var student = await studentService
                                .GetStudentByAccountIdAsync(account.Id);
                            identity.AddClaim(new Claim(
                                ClaimConstants.IdClaim,
                                student.Data.Id.ToString()));
                        }
                        else if(Attribute.GetCustomAttribute(command.GetType(),
                            typeof(MentorRoleCommandAttribute)) != null)
                        {
                            identity.AddClaim(new Claim(
                                ClaimsIdentity.DefaultRoleClaimType, UserRole.Mentor.ToString()));
                            var mentorService = _serviceProvider
                                .GetService<IMentorService>();
                            var mentor = await mentorService
                                .GetMentorByAccountIdAsync(account.Id);
                            identity.AddClaim(new Claim(
                                ClaimConstants.IdClaim,
                                mentor.Data.Id.ToString()));
                        }
                        var result = await command.Execute(message, client);
                        break;
                    }
                }

                _messages.Remove(messageHeader);
            }

            return Ok();
        }
    }
}
