using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Common
{
    public static class KeyboardFactory
    {
        public static InlineKeyboardMarkup GetMainMenuKeyboard()
        {
            return new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(BotConstants.ButtonLabels.System, BotConstants.Callbacks.MenuSystem),
                    InlineKeyboardButton.WithCallbackData(BotConstants.ButtonLabels.Info, BotConstants.Callbacks.MenuInfo)
                }
            };
        }
    }
}
