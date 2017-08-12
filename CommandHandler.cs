using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.NET_Core
{
    public class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private GoogleCrawler google;

        public async Task Install(DiscordSocketClient c)
        {
            client = c;
            commands = new CommandService();
            google = new GoogleCrawler();
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            // Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;
            if (message.Content.Length <= 1) return;
            else if (string.IsNullOrWhiteSpace(message.Content.Substring(1, 1))) return;

            // Mark where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message has a valid prefix, adjust argPos
            if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('!', ref argPos))) return;

            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the Command, store the result
            var result = await commands.ExecuteAsync(context, argPos);

            if (message.Content.StartsWith("!google", System.StringComparison.OrdinalIgnoreCase)
				|| message.Content.StartsWith("!rnrmf", System.StringComparison.OrdinalIgnoreCase)
				|| message.Content.StartsWith("!구글"))
            {
                GoogleCrawler google = new GoogleCrawler();
                await google.Google(message);
            }

            if (message.Content.StartsWith("!youtube", System.StringComparison.OrdinalIgnoreCase)
				|| message.Content.StartsWith("!dbxbqm", System.StringComparison.OrdinalIgnoreCase)
				|| message.Content.StartsWith("!유튭")
				|| message.Content.StartsWith("!유튜브"))
            {
                YoutubeSearcher youtube = new YoutubeSearcher();
                await youtube.Youtube(message);
            }

            // If the command failed, notify the user 오류 메세지 출력.
            //             else if(!result.IsSuccess)
            //                 await message.Channel.SendMessageAsync($"**흐앙!**:sob: {result.ErrorReason}");
        }
    }
}