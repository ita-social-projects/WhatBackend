using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Constants;
using TelegramBot.Models;
using TelegramBot.Utils.Interfaces;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("bot/message")]
    public class MessageController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataProtector _protector;
        private readonly Dictionary<MessageHeader, Message> _messages;
        private readonly IApiUtil _apiUtil;
        private readonly TelegramApiEndpoints _telegramApiEndpoints;

        public MessageController(IServiceProvider serviceProvider, 
            IApiUtil apiUtil,
            TelegramApiEndpoints telegramApiEndpoints,
            IDataProtector protector)
        {
            _serviceProvider = serviceProvider;
            _apiUtil = apiUtil;
            _messages = new Dictionary<MessageHeader, Message>();
            _telegramApiEndpoints = telegramApiEndpoints;
            _protector = protector;
        }

        [HttpGet]
        public string Get()
        {
            return "Bot is up and running!";
        }

        [HttpGet("auth")]
        public async Task<IActionResult> Login(string telegramId)
        {
            var responseModel = await _apiUtil.SignInAsync(_telegramApiEndpoints.SignIn, telegramId);

            if (responseModel == null)
            {
                return Unauthorized();
            }

            var token = responseModel.Token.Replace("Bearer ", "");

            if (token == null || !await Authenticate(token))
            {
                return Unauthorized();
            }

            Dictionary<string, string> roleList = new Dictionary<string, string>();

            foreach (var item in responseModel.RoleList)
            {
                string value = _protector.Protect(item.Value.Replace("Bearer ", ""));
                roleList.Add(item.Key, value);
            }

            SetResponseCookie("accessToken", _protector.Protect(token));

            return RedirectToAction("Index", "Home");
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
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

            //GetAccountByTelegramId

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

        private void SetResponseCookie(string key, string value)
        {
            Response.Cookies.Append(key, value, new CookieOptions()
            {
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = true
            });
        }

        private async Task<bool> Authenticate(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var accountId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.AccountId).Value;
            var entityId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.EntityId).Value;
            var email = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.Email).Value;
            var firstName = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.FirstName).Value;
            var lastName = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.LastName).Value;
            var localization = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.Localization).Value;

            SetResponseCookie("currentRole", role);
            SetResponseCookie("accountId", accountId);
            SetResponseCookie("entityId", entityId);
            SetResponseCookie("email", email);
            SetResponseCookie("firstName", firstName);
            SetResponseCookie("lastName", lastName);
            SetResponseCookie("localization", localization);

            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                 new Claim(ClaimsConstants.AccountId, accountId),
                 new Claim(ClaimsConstants.EntityId, entityId),
                 new Claim(ClaimsConstants.Email, email),
                 new Claim(ClaimsConstants.FirstName, firstName),
                 new Claim(ClaimsConstants.LastName, lastName),
                 new Claim(ClaimsConstants.Localization, localization)
            };

            ClaimsIdentity roleClaim = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // set authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(roleClaim));

            return true;
        }
    }
}
