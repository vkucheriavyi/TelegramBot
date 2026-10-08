using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Common;
using TelegramBot.Services;

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
                    await HandleStartCommand(bot, message, ct);
                    break;

                case BotConstants.Commands.Menu:
                    await HandlerMenuCommand(bot, message, ct);
                    break;

                default:
                    await HandleUnknownCommand(bot, message, ct);
                    break;
            }
        }

        private static async Task HandleStartCommand(ITelegramBotClient bot, Message message, CancellationToken ct)
        {
            await NotificationService.SendMessageAsync(bot, message.Chat.Id, "👋 Welcome to the bot! Use /menu to see available options.", ct, parseMode: ParseMode.Markdown);
        }

        private static async Task HandlerMenuCommand(ITelegramBotClient bot, Message message, CancellationToken ct)
        {
            await NotificationService.SendMessageAsync(bot, message.Chat.Id, "Please choose an action:", ct, replyMarkup: KeyboardFactory.GetMainMenuKeyboard());
        }

        private static async Task HandleUnknownCommand(ITelegramBotClient bot, Message message, CancellationToken ct)
        {
            await NotificationService.SendMessageAsync(bot, message.Chat.Id, "❓ Unknown command. Please use /menu to see available commands.", ct, parseMode: ParseMode.Markdown);
        }
    }
}