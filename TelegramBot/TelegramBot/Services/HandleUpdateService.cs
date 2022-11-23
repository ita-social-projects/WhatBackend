using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.Entities;
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
        private readonly IUserDataService _userDataService;
        private readonly TelegramApiEndpoints _telegramApiEndpoints;
        private readonly StudentsApiEndpoints _studentApiEndpoints;
        private const int _TokenLifeTime = 720;

        public HandleUpdateService(ITelegramBotClient botClient,
            ILogger<HandleUpdateService> logger,
            IApiUtil apiUtil,
            IDataProtectionProvider provider,
            IOptions<ApplicationSettings> options,
            IUserDataService userDataService)
        {
            _botClient = botClient;
            _logger = logger;
            _apiUtil = apiUtil;
            _dataProtector = provider.CreateProtector(options.Value.Cookies.SecureKey);
            _userDataService = userDataService;
            _telegramApiEndpoints = options.Value.Urls.ApiEndpoints.Telegram;
            _studentApiEndpoints = options.Value.Urls.ApiEndpoints.Students;
        }

        public async Task EchoAsync(Update update)
        {
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
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            var currentUser = new UserData();

            if (_userDataService.UserDataByTelegramId.TryGetValue(message.Chat.Id, out currentUser)
                && currentUser.Created.Value.AddMinutes(_TokenLifeTime) >= DateTime.UtcNow)
            {
                _apiUtil.AccessToken = _userDataService.GetAccessTokenByTelegramId(message.Chat.Id);
            }
            else
            {
                if (!await SignInAsync(message))
                    return;
            }

            var action = message.Text!.Split(' ')[0] switch
            {
                "/start" => SendMenu(_botClient, message),
                "/inline" => SendInlineKeyboard(_botClient, message),
                "/students" => SendAllStudents(_botClient, message),
                "/keyboard" => SendReplyKeyboard(_botClient, message),
                "/remove" => RemoveKeyboard(_botClient, message),
                "/photo" => SendFile(_botClient, message),
                "/request" => RequestContactAndLocation(_botClient, message),
                _ => Usage(_botClient, message, currentUser?.CurrentRole)
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

            static async Task<Message> Usage(ITelegramBotClient bot, Message message, UserRole? role = UserRole.NotAssigned)
            {
                string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";

                if (role == UserRole.Student)
                {
                    usage = $"{usage}\n/classbook - send user classbook";
                }
                else if (role == UserRole.Mentor)
                {
                    usage = $"{usage}\n/setHomework  - set new homework";
                }
                else if (role == UserRole.Admin)
                {
                    usage = $"{usage}\n/students - send all students";
                }

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
            var telegramId = message.Chat.Id;
            var messageId = message.MessageId;
            AccountDto accountDto = new AccountDto();

            if (parameters.Length > 1)
            {
                token = parameters[1];
                telegramSyncEndlpoint = string.Format(telegramSyncEndlpoint, token, telegramId.ToString());

                accountDto = await _apiUtil.PostAsync<AccountDto, string>(telegramSyncEndlpoint, null);

                await SignInAsync(message);
            }

            string response = $"Hello {accountDto?.FirstName}! I'm a Telegram bot of WHAT. " +
              "Press the button 'MENU' below the input field to open menu and begin dealing with me:\n";

            return (await bot.SendTextMessageAsync(telegramId,
                response, replyToMessageId: messageId, replyMarkup: GetMainMenu()));
        }

        private async Task<bool> SignInAsync(Message message)
        {
            long chatId = message.Chat.Id;
            var responseModel = await _apiUtil.SignInAsync(_telegramApiEndpoints.SignIn, chatId.ToString());
            string response;
            bool result = true;

            if (responseModel == null)
            {
                response = "Incorrect credential";
                result = false;
            }
            else
            {
                var token = responseModel.Token.Replace("Bearer ", "");

                if (token == null || !Authenticate(token, chatId))
                {
                    response = "Incorrect credential";
                    result = false;
                }

                Dictionary<string, string> roleList = new Dictionary<string, string>();

                foreach (var role in roleList)
                {
                    string value = _dataProtector.Protect(role.Value.Replace("Bearer", ""));
                    roleList.Add(role.Key, value);
                }

                response = $"Hello!";
            }

            await _botClient.SendTextMessageAsync(chatId,
                response, replyToMessageId: message.MessageId, replyMarkup: GetMainMenu());

            return result;
        }

        private IReplyMarkup GetMainMenu() => new ReplyKeyboardMarkup(new KeyboardButton("MENU")) { ResizeKeyboard = true };

        //ToDo: add second parameter string phone number
        private bool Authenticate(string token, long chatId)
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
            UserRole currentRole;
            Enum.TryParse(role, true, out currentRole);

            _userDataService.AddUserData(chatId, new UserData()
            {
                AccountId = Convert.ToInt64(accountId),
                CurrentRole = currentRole,
                EntityId = Convert.ToInt64(entityId),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Localization = localization,
                AccessToken = _dataProtector.Protect(token),
                Created = DateTime.UtcNow
            });

            _apiUtil.AccessToken = _userDataService.GetAccessTokenByTelegramId(chatId);

            return true;
        }
    }
}
