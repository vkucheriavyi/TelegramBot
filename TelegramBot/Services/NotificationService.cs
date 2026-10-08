using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Common;

namespace TelegramBot.Services
{
    public static class NotificationService
    {
        public static async Task SendStartupNotification(ITelegramBotClient bot, long adminChatId, CancellationToken ct = new())
        {
            string message = $"🚀 *Raspberry Pi started successfully!*\n\n" +
                            $"🕒 *Start time:* `{DateTime.Now:yyyy-MM-dd HH:mm:ss}`\n" +
                            $"⚡ Bot is ready to work.";

            await SendMessageAsync(bot, adminChatId, message, ct: ct);
        }

        public static async Task NotifyShutdownAsync(ITelegramBotClient bot, long adminChatId, CancellationToken ct = default)
        {
            string message =
                $"🔴 *Raspberry Pi is shutting down...*\n" +
                $"🕒 *Time:* `{DateTime.Now:yyyy-MM-dd HH:mm:ss}`";

            await SendMessageAsync(bot, adminChatId, message, ct: ct);
        }

        public static async Task SendAlertAsync(ITelegramBotClient bot, long adminChatId, string alertText, CancellationToken ct = default)
        {
            string message = $"⚠️ *SYSTEM ALERT*\n\n{alertText}";
            await SendMessageAsync(bot, adminChatId, message, ct: ct);
        }

        public static async Task SendMessageAsync(ITelegramBotClient bot,
        long chatId,
        string message,
        CancellationToken ct = default,
        ReplyMarkup? replyMarkup = null,
        ParseMode? parseMode = ParseMode.None
            )
        {
            try
            {
                await bot.SendMessage(chatId, message, parseMode: parseMode ?? ParseMode.None, replyMarkup: replyMarkup, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                AppLogger.Error($"[NotificationService]: Failed to send message {message} to chat {chatId}", ex);
            }
        }
    }
}