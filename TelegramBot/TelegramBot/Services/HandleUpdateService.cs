using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Constants;
using TelegramBot.Models.Entities;
using TelegramBot.Services.Interfaces;

namespace TelegramBot
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly IApiUtil _apiUtil;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDataService _userDataService;
        private readonly TelegramApiEndpoints _telegramApiEndpoints;
        private readonly StudentsApiEndpoints _studentApiEndpoints;

        public HandleUpdateService(ITelegramBotClient botClient,
            ILogger<HandleUpdateService> logger,
            IApiUtil apiUtil,
            IDataProtectionProvider provider,
            IOptions<ApplicationSettings> options,
            ICurrentUserService currentUserService,
            IUserDataService userDataService,
            IHttpContextAccessor contextAccessor)
        {
            _botClient = botClient;
            _logger = logger;
            _apiUtil = apiUtil;
            _dataProtector = provider.CreateProtector(options.Value.Cookies.SecureKey);
            _contextAccessor = contextAccessor;
            _currentUserService = currentUserService;
            _userDataService = userDataService;
            _telegramApiEndpoints = options.Value.Urls.ApiEndpoints.Telegram;
            _studentApiEndpoints = options.Value.Urls.ApiEndpoints.Students;
        }

        public async Task EchoAsync(Update update)
        {
            _apiUtil.AccessToken = _userDataService.GetAccessTokenByTelegramId(update.Message.Chat.Id);

            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery!),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/start" => SendMenu(_botClient, message),
                "/inline" => SendInlineKeyboard(_botClient, message),
                "/allusers" => SendAllStudents(_botClient, message),
                "/keyboard" => SendReplyKeyboard(_botClient, message),
                "/remove" => RemoveKeyboard(_botClient, message),
                "/photo" => SendFile(_botClient, message),
                "/request" => RequestContactAndLocation(_botClient, message),
                _ => Usage(_botClient, message)
            };
            Message sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                    new[]
                    {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                    });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: inlineKeyboard);
            }

            static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new[]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    })
                {
                    ResizeKeyboard = true
                };

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: replyKeyboardMarkup);
            }

            static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Removing keyboard",
                                                      replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                return await bot.SendPhotoAsync(chatId: message.Chat.Id,
                                                photo: new InputOnlineFile(fileStream, fileName),
                                                caption: "Nice Picture");
            }

            static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup RequestReplyKeyboard = new ReplyKeyboardMarkup(
                    new[]
                    {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                    });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Who or Where are you?",
                                                      replyMarkup: RequestReplyKeyboard);
            }

            static async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: usage,
                                                      replyMarkup: new ReplyKeyboardRemove());
            }
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        #region Inline Mode

        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
        {
            _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await _botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                    results: results,
                                                    isPersonal: true,
                                                    cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
        {
            _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);
            return Task.CompletedTask;
        }

        #endregion

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task<Message> SendAllStudents(ITelegramBotClient bot, Message message)
        {
            string getAllStudentsEndpoint = _studentApiEndpoints.GetAllStudentsEndpoint;

            //ToDo: add studentViewModel
            var students = await _apiUtil.GetAsync<IList<StudentDto>>(getAllStudentsEndpoint);

            StringBuilder response = new StringBuilder();

            if (students.Any())
            {
                foreach (var student in students)
                {
                    response.Append($"{student.FirstName} {student.LastName} {student.Email}\t");
                }
            }

            return (await bot.SendTextMessageAsync(message.Chat.Id,
                response.ToString(), replyToMessageId: message.MessageId, replyMarkup: GetMainMenu()));
        }

        private async Task<Message> SendMenu(ITelegramBotClient bot, Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            string telegramSyncEndlpoint = _telegramApiEndpoints.SyncAccounts;
            var parameters = message.Text.Split(' ');
            string token;
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            AccountDto accountDto = new AccountDto();

            if (parameters.Length > 1)
            {
                token = parameters[1];
                telegramSyncEndlpoint = string.Format(telegramSyncEndlpoint, token, chatId.ToString());

                accountDto = await _apiUtil.PostAsync<AccountDto, string>(telegramSyncEndlpoint, null);
            }

            //ToDo: add authoriation after sync acc by chatid and phone number(create phone number field in db)
            await SignInAsync(message);

            string response = $"Hello {accountDto?.FirstName}! I'm a Telegram bot of WHAT. " +
              "Press the button 'MENU' below the input field to open menu and begin dealing with me:\n";

            return (await bot.SendTextMessageAsync(chatId,
                response, replyToMessageId: messageId, replyMarkup: GetMainMenu()));
        }

        private async Task SignInAsync(Message message)
        {
            long chatId = message.Chat.Id;
            var responseModel = await _apiUtil.SignInAsync(_telegramApiEndpoints.SignIn, chatId.ToString());
            string response;

            if (responseModel == null)
            {
                response = "Incorrect credential";
            }
            else
            {
                var token = responseModel.Token.Replace("Bearer ", "");

                if (token == null || !await Authenticate(token, chatId))
                {
                    response = "Incorrect credential";
                }

                Dictionary<string, string> roleList = new Dictionary<string, string>();

                foreach (var role in roleList)
                {
                    string value = _dataProtector.Protect(role.Value.Replace("Bearer", ""));
                    roleList.Add(role.Key, value);
                }

                SetResponseCookie("accessToken", _dataProtector.Protect(token));

                response = $"Hello!";
            }

            await _botClient.SendTextMessageAsync(chatId,
                response, replyToMessageId: message.MessageId, replyMarkup: GetMainMenu());
        }

        private IReplyMarkup GetMainMenu() => new ReplyKeyboardMarkup(new KeyboardButton("MENU")) { ResizeKeyboard = true };

        //ToDo: add second parameter string phone number
        private async Task<bool> Authenticate(string token, long chatId)
        {
            var handler = new JwtSecurityTokenHandler();

            _contextAccessor.HttpContext.Request.Headers.Add("Authorization", token);

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var accountId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.AccountId).Value;
            var entityId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.EntityId).Value;
            var email = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.Email).Value;
            var firstName = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.FirstName).Value;
            var lastName = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.LastName).Value;
            var localization = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.Localization).Value;

            //todo: add this data to Userdata dictionary instead of cookies
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

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(roleClaim));

            _userDataService.AddUserData(chatId, new UserData()
            {
                AccountId = Convert.ToInt64(accountId),
                CurrentRole = role,
                EntityId = Convert.ToInt64(entityId),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Localization = localization,
                AccessToken = _dataProtector.Protect(token)
            });

            return true;
        }

        private void SetResponseCookie(string key, string value)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions()
            {
                SameSite = SameSiteMode.None,
                Path = "/",
                Secure = true
            });
        }
    }
}
