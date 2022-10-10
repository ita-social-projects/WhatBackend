using CharlieBackend.Core.DTO.Account;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Utils.Interfaces;

namespace TelegramBot.Models.Commands
{
    public class StartCommand : Command
    {
        private readonly TelegramApiEndpoints _telegramApiEndpoints;
        private readonly IApiUtil _apiUtil;

        public override string Name => "start";

        public StartCommand(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;
            _telegramApiEndpoints = options.Value.Urls.ApiEndpoints.Telegram;
        }

        public override async Task<string> Execute(Message message, TelegramBotClient client)
        {
            string telegramSyncEndlpoint = _telegramApiEndpoints.SyncAccounts;
            var parameters = message.Text.Split(' ');
            string token;
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            string response = string.Empty;

            if (parameters.Length > 1)
            {
                token = parameters[1];
                telegramSyncEndlpoint = string.Format(telegramSyncEndlpoint,token, chatId.ToString());
                //ToDo: implement api call for sync with acc
                //need to add as response data class with tgToken and accToken for response
                var result = await _apiUtil.PostAsync<AccountDto, string>(telegramSyncEndlpoint, null);
            }

            response += "Hello! I'm a Telegram bot of WHAT. " +
              "Press the button 'MENU' below the input field to open menu and begin dealing with me:\n";

            return (await client.SendTextMessageAsync(chatId,
                response, replyToMessageId: messageId, replyMarkup: GetMainMenu())).Text;
        }

        private IReplyMarkup GetMainMenu()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton("MENU"))
            {
                ResizeKeyboard = true
            };
        }
    }
}
