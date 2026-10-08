using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Common;
using TelegramBot.Services;

namespace TelegramBot.Handlers
{
    public static class CallbackHandler
    {
        public static async Task HandleCallback(ITelegramBotClient bot, CallbackQuery? callbackQuery, CancellationToken ct)
        {
            await bot.AnswerCallbackQuery(callbackQueryId: callbackQuery!.Id, cancellationToken: ct);

            switch (callbackQuery.Data)
            {
                case BotConstants.Callbacks.MenuSystem:
                    await HandleMenuSystem(bot, callbackQuery, ct);
                    break;

                default:
                    await HandleDefaultCallback(bot, callbackQuery, ct);
                    break;
            }
        }

        static async Task HandleDefaultCallback(ITelegramBotClient bot, CallbackQuery callbackQuery, CancellationToken ct)
        {
            await NotificationService.SendMessageAsync(bot, callbackQuery.From.Id, "Please, use menu options.", ct);
        }

        static async Task HandleMenuSystem(ITelegramBotClient bot, CallbackQuery callbackQuery, CancellationToken ct)
        {
            var sb = new StringBuilder();
            var cpuTemp = GetCpuTemperature();

            var uptime = GetUptime();
            sb.AppendLine($"*System Information:*");
            sb.AppendLine($"- Uptime: {uptime}");
            sb.AppendLine($"- CPU: {cpuTemp}");

            await NotificationService.SendMessageAsync(bot, callbackQuery.From.Id, sb.ToString(), ct, parseMode: ParseMode.Markdown);
        }

        static string GetCpuTemperature()
        {
            const string path = "/sys/class/thermal/thermal_zone0/temp";
            if (!File.Exists(path)) return "N/A (not a Linux)";
            try
            {
                string raw = File.ReadAllText(path).Trim();
                if (double.TryParse(raw, CultureInfo.InvariantCulture, out double milliDegrees))
                {
                    return $"{milliDegrees / 1000.0:F1}°C";
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("[System]: Failed to get CPU temperature", ex);
                return "Failed to read CPU temperature";
            }
            return "Failed to gain CPU temperature";
        }

        static string GetUptime()
        {
            var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
        }
    }
}
