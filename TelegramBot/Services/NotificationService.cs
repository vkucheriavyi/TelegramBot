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

            await SendSafeMessage(bot, adminChatId, message, ct: ct);
        }

        public static async Task NotifyShutdownAsync(ITelegramBotClient bot, long adminChatId, CancellationToken ct = default)
        {
            string message =
                $"🔴 *Raspberry Pi is shutting down...*\n" +
                $"🕒 *Time:* `{DateTime.Now:yyyy-MM-dd HH:mm:ss}`";

            await SendSafeMessage(bot, adminChatId, message, ct: ct);
        }

        public static async Task SendAlertAsync(ITelegramBotClient bot, long adminChatId, string alertText, CancellationToken ct = default)
        {
            string message = $"⚠️ *SYSTEM ALERT*\n\n{alertText}";
            await SendSafeMessage(bot, adminChatId, message, ct: ct);
        }

        private static async Task SendSafeMessage(ITelegramBotClient bot,
            long chatId,
            string message,
            ReplyMarkup? replyMarkup = null,
            ParseMode parseMode = ParseMode.Markdown,
            CancellationToken ct = new())
        {
            try
            {
                await bot.SendMessage(chatId, message, parseMode: parseMode, replyMarkup: replyMarkup, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                AppLogger.Error($"[NotificationService]: Failed to send message {message} to chat {chatId}", ex);
            }
        }
    }
}