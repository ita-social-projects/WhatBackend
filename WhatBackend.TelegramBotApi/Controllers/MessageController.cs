using CharlieBackend.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

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
            if(DateTime.Now >= new DateTime(2021,10,13,17,30, 1))
            {
                return Ok();
            }
            var client = await Bot.Get(_serviceProvider);
            var commands = Bot.Commands;
            Message message = update.Message ?? update.EditedMessage;

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

            var isNewMessage = _messages.TryAdd(messageHeader, message);

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

                _messages.Remove(messageHeader);
            }

            return Ok();
        }
    }
}
