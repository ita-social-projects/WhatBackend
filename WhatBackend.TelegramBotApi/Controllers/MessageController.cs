using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using CharlieBackend.Business.Models;
using System.Collections.Generic;

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
        public async Task<OkResult> Update([FromBody] Update update)
        {
            var client = await Bot.Get(_serviceProvider);
            var commands = Bot.Commands;
            Message message = update.Message ?? update.EditedMessage;

            if(message == null)
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

            var isNewMessage = _messages.TryAdd(
                    new MessageHeader()
                    {
                        ChatId = message.Chat.Id,
                        MessageId = message.MessageId
                    },
                    message);

            if (isNewMessage)
            {

                foreach (var command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        var result = await command.Execute(message, client);
                        break;
                    }
                }

                _messages.Remove(new MessageHeader()
                {
                    ChatId = message.Chat.Id,
                    MessageId = message.MessageId
                });
            }

            return Ok();
        }
    }
}
