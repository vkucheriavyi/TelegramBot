using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Common;
using TelegramBot.Models.Configuration;

namespace TelegramBot.Handlers
{
    public class UpdateRouter
    {
        private readonly BotConfig _botConfig;
        public UpdateRouter(BotConfig botConfig)
        {
            _botConfig = botConfig;
        }

        public async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
        {

            if (update.Message is { Text: { } text } message)
            {
                text = text.Trim();

                if (text.StartsWith('/'))
                {
                    await CommandHandler.HandleCommand(bot, message, ct);
                    return;
                }
                else
                {
                    await bot.SendMessage(
                       chatId: message.Chat.Id,
                        text: $"❓ Unknown command `{text}`. Please, type /menu",
                        parseMode: ParseMode.Markdown,
                       cancellationToken: ct
                   );
                }
            }

            else if (update.Type == UpdateType.CallbackQuery)
            {

                await bot.AnswerCallbackQuery(
                    callbackQueryId: update.CallbackQuery!.Id,
                    cancellationToken: ct
                );

                await bot.SendMessage(
                    chatId: update.CallbackQuery.From.Id,
                    text: $"Development is still in progress..",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }

            return;
        }

        public Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken ct)
        {
            AppLogger.Error("[UpdateRouter]: An error occurred while processing an update", exception);
            return Task.CompletedTask;
        }
    }
}
