using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Common;
using TelegramBot.Configuration;
using TelegramBot.Handlers;
using TelegramBot.Models.Configuration;
using TelegramBot.Services;

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
        var cancellationToken = new CancellationTokenSource().Token;

        var updateRouter = new UpdateRouter(BotConfig);

        bot.StartReceiving(
            updateHandler: updateRouter.HandleUpdate,
                errorHandler: updateRouter.HandleError,
                cancellationToken: cancellationToken,
                receiverOptions: new Telegram.Bot.Polling.ReceiverOptions
                {
                    AllowedUpdates = new[] { UpdateType.CallbackQuery, UpdateType.Message }, // receive only messages and callback queries
                    DropPendingUpdates = true // ignore any pending updates that were sent while the bot was offline
                }
            );

        await NotificationService.SendStartupNotification(bot, BotConfig.AdminId);

        await Task.Delay(-1);
        await NotificationService.NotifyShutdownAsync(bot, BotConfig.AdminId);
    }
}
