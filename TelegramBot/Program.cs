using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Common;
using TelegramBot.Configuration;
using TelegramBot.Models.Configuration;

namespace TelegramBot;
class Program
{
    static BotConfig BotConfig { get; set; } = null!;

    static async Task Main(string[] args)
    {
        try
        {
            BotConfig = ConfigurationLoader.LoadConfig();
            AppLogger.Success("[Bot]: Configuration loaded successfully!");
        }
        catch (InvalidOperationException ex)
        {
            AppLogger.Error("[Bot]: Failed to load configuration", ex);
            Environment.Exit(1);
        }
        catch (Exception ex)
        {
            AppLogger.Error("[Bot]: Critical error occuried", ex);
            Environment.Exit(1);
        }

        var bot = new TelegramBotClient(BotConfig.BotToken);
        string message = $"🚀 *Raspberry Pi started successfully!*\n\n" +
                             $"🕒 *Start time:* `{DateTime.Now:yyyy-MM-dd HH:mm:ss}`\n" +
                             $"⚡ Bot is ready to work.";

        await bot.SendMessage(BotConfig.AdminId, message, parseMode: ParseMode.Markdown);

        await Task.Delay(-1);
    }
}
