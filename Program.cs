using System;

namespace csharpi_
{
    class Program
    {
        private static Bot bot;

        static void Main(string[] args)
        {
            bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }

        public static Bot getBot() {
            return bot;
        }
    }
}
