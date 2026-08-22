namespace TelegramBot.Common
{
    public static class AppLogger
    {
        public static void Info(string message)
        {
            WriteColored($"{message}", ConsoleColor.Blue);
        }

        public static void Success(string message)
        {
            WriteColored($"{message}", ConsoleColor.Green);
        }

        public static void Warning(string message)
        {
            WriteColored($"{message}", ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            WriteColored($"{message}", ConsoleColor.Red);
        }

        public static void Error(string message, Exception ex)
        {
            WriteColored($"{message}: {ex.Message}", ConsoleColor.Red);
        }

        private static void WriteColored(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
            }
            finally
            {
                Console.ForegroundColor = previousColor;
            }
        }
    }
}
