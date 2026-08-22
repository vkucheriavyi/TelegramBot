using Microsoft.Extensions.Configuration;
using TelegramBot.Models.Configuration;

namespace TelegramBot.Configuration;

public static class ConfigurationLoader
{
    public static BotConfig LoadConfig()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var section = config.GetSection("TelegramBot");
        if (!section.Exists()) throw new Exception("Configuration section 'TelegramBot' is missing in appsettings.json or user secrets.");

        var botConfig = section.Get<BotConfig>() ?? throw new InvalidOperationException("Section 'TelegramBot' is invalid.");

        if (string.IsNullOrWhiteSpace(botConfig.BotToken)) throw new Exception("BotToken is missing in configuration.");

        if (botConfig.AdminId == 0) throw new Exception("AdminId is missing or invalid in configuration.");

        return botConfig;
    }
}
