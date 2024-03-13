using Telegram.Bot;
using Telegram.Bot.Types;

namespace Tracker.TelegramBot.Services
{
    public class BotService
    {

        private readonly ITelegramBotClient botClient;

        public BotService(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }


        public async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Answer Message",
                cancellationToken: cancellationToken);
        }

        public async Task OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Answer CallbackQuery",
                cancellationToken: cancellationToken);
        }
    }
}
