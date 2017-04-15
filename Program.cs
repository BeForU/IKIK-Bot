using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.NET_Core
{
    class Program
    {
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        // Create a DiscordClient with WebSocket support
        private DiscordSocketClient client;
        private CommandHandler handler;

        public async Task Start()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,              // Specify console verbose information level.
                AlwaysDownloadUsers = true,                  // Start the cache off with updated information.
                MessageCacheSize = 1000                      // Tell discord.net how long to store messages (per channel).
            });

            client.Log += (l)                               // Register the console log event.
             => Task.Run(()
             => Console.WriteLine($"[{l.Severity}] {l.Source}: {l.Exception?.ToString() ?? l.Message}"));

            // Place the token of your bot account here
            string token = "TOKEN HERE";

            handler = new CommandHandler();
            await handler.Install(client);

            // Configure the client to use a Bot token, and use our token
            await client.LoginAsync(TokenType.Bot, token);
            // Connect the client to Discord's gateway
            await client.StartAsync();

            // Block this task until the program is exited.
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}