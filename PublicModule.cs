using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NET_Core
{
    public class PublicModule : ModuleBase
    {
		//         [Command("invite")]
		//         [Summary("Returns the OAuth2 Invite URL of the bot")]
		//         public async Task Invite()
		//         {
		//             var application = await Context.Client.GetApplicationInfoAsync();
		//             await ReplyAsync(
		//                 $"A user with `MANAGE_SERVER` can invite me to your server here: <https://discordapp.com/oauth2/authorize?client_id={application.Id}&scope=bot>");
		//         }
		//
		//         [Command("leave")]
		//         [Summary("Instructs the bot to leave this Guild.")]
		//         [RequireUserPermission(GuildPermission.ManageGuild)]
		//         public async Task Leave()
		//         {
		//             if (Context.Guild == null) { await ReplyAsync("This command can only be ran in a server."); return; }
		//             await ReplyAsync("Leaving~");
		//             await Context.Guild.LeaveAsync();
		//         }
		//
		//         [Command("say")]
		//         [Alias("echo")]
		//         [Summary("Echos the provided input")]
		//         public async Task Say([Remainder] string input)
		//         {
		//             await ReplyAsync(input);
		//         }

		[Command("info")]
        [Alias("정보", "information", "wjdqh")]
        public async Task Info()
        {
            var application = await Context.Client.GetApplicationInfoAsync();

            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "정보",
                Description = $"- 봇 주인: {application.Owner.Username} (ID {application.Owner.Id})\n" +
                $"- 사용한 라이브러리: Discord.Net ({DiscordConfig.Version})\n" +
                $"- 런타임: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- 돌아간 시간: {GetUptime()}\n" +
				$"- 잌잌봇 버전 : 3.0\n" +
				$"- 소스 코드 : https://github.com/BeForU/IKIK-Bot\n"
			};
            await ReplyAsync("", false, embed);

            embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "상태",
                Description = $"- 메모리(힙) 크기: {GetHeapSize()} MB\n" +
                $"- 서버: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- 채널: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- 사용자: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
            };

            await ReplyAsync("", false, embed);
        }

        [Command("help")]
        [Alias("도움", "도움말", "도움!", "ehdna")]
        [Summary("Display helps")]
        public async Task DisplayHelp()
        {
            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "도움말이에요!",
                Description = "```" +
                "!now    !지금     현재시간 출력\n" +
                "!utc    !표준시   국제 표준시 출력\n" +
                "!info   !정보     봇 정보를 보여드릴게요\n" +
                "!help   !도움     지금 이거에요!!\n" +
                "!alias  !명령어\n  같은 기능의 다른 명령어 목록을 띄워요!\n" +
				"!google !구글 \"검색어\"\n  구글에서 검색할 거에요! 최대 두 개 까지!\n" +
				"!youtube !유튜브 \"검색어\"\n  유튜브에서 검색할거에요! 최대 세 개 까지!" +
                "```" +
				"\n\"!명령어\" 라고 입력하면 좀 더 보실 수 있어요!"
            };

            await ReplyAsync("", false, embed);
        }

        [Command("alias")]
        [Alias("명령어", "다른거")]
        public async Task AliasHelp()
        {
            var application = await Context.Client.GetApplicationInfoAsync();

            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "같은 기능의 명령어들을 보여드릴게요!",
                Description = "```" +
                "!now    => !지금, !시간, !time, !wlrma, !tlrks\n" +
                "!utc    => !표준시, !유티씨, !vywnstl\n" +
                "!info   => !정보, !information, !wjdqh\n" +
				"!help   => !도움, !도움말, !도움!, !ehdna\n" +
                "!alias  => !명령어, !다른거,\n" +
                "!google => !구글, !rnrmf\n" +
                "!youtube => !유튜브, !유튭, !dbxbqm\n" +
				"```"
            };
            await ReplyAsync("", false, embed);
        }

        [Command("now")]
        [Alias("지금", "시간", "time", "wlrma", "tlrks")]
        [Summary("Display time")]
        public async Task Now()
        {
            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            sb.Append(DateTime.Now);
            sb.Append(" (");
            sb.Append(TimeZoneInfo.Local.DisplayName);
            sb.Append(")");

            await ReplyAsync(sb.ToString());
        }

        [Command("utc")]
        [Alias("표준시", "유티씨", "vywnstl")]
        [Summary("Display UTC time")]
        public async Task UTC()
        {
            await ReplyAsync(DateTime.UtcNow.ToString());
        }

        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}