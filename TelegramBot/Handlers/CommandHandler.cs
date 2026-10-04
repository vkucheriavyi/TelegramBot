using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Common;

namespace TelegramBot.Handlers
{
    public static class CommandHandler
    {
        public static async Task HandleCommand(ITelegramBotClient bot, Message message, CancellationToken ct)
        {
            string text = message.Text?.Trim() ?? string.Empty;

            // Separate the command itself from the arguments and bot nickname:
            // "/start 123" -> "/start"
            // "/menu@MyCoolBot" -> "/menu"
            string command = text.Split(' ')[0].Split('@')[0].ToLowerInvariant();
            switch (command)
            {
                case BotConstants.Commands.Start:
                    await bot.SendMessage(
                        chatId: message.Chat.Id,
                        text: "Welcome to the bot! Use /menu to see available options.",
                        cancellationToken: ct
                    );
                    break;
                case BotConstants.Commands.Menu:
                    var homeButtons = new InlineKeyboardMarkup(new[]
                    {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(BotConstants.ButtonLabels.System, BotConstants.Callbacks.MenuSystem),
                                InlineKeyboardButton.WithCallbackData(BotConstants.ButtonLabels.Info, BotConstants.Callbacks.MenuInfo)
                            }
                        });
                    await bot.SendMessage(
                        chatId: message.Chat.Id,
                        text: "👋 *Welcome!*\nPlease choose an action:",
                        replyMarkup: homeButtons,
                        cancellationToken: ct
                    );
                    break;
                default:
                    await bot.SendMessage(
                        chatId: message.Chat.Id,
                        text: $"❓ Unknown command `{text}`. Please use /menu to see available commands.",
                        parseMode: ParseMode.Markdown,
                        cancellationToken: ct
                    );
                    break;
            }
        }
    }
}