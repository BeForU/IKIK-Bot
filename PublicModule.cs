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
        [Alias("정보", "information")]
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
				$"- 잌잌봇 버전 : 2.4\n\n"
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
        [Alias("도움", "도움말", "도움!")]
        [Summary("Display helps")]
        public async Task DisplayHelp()
        {
            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "기능 명령어 보여드릴게요!",
                Description = "```" +
                "!now    !지금     현재시간 출력\n" +
                "!utc    !표준시   국제 표준시 출력\n" +
                "!info   !정보     봇 정보를 보여드릴게요\n" +
                "!help   !도움     지금 이거에요!!\n" +
                "!alias  !명령어   같은 기능의 다른 명령어 목록을 띄워요!\n" +
                "!google !구글 \"검색어\"   구글을 검색할 거에요! 두개까지만!" +
                "```"
            };

            await ReplyAsync("", false, embed);

            embed = new EmbedBuilder()
            {
                Color = new Color(0, 122, 204),
                Title = "유튜브 링크 명령어 보여드릴게요!",
                Description = "```" +
                "!캔젤럽, !candyjellylove, ...\n" +
                "      러블리즈 - Candy Jelly Love\n" +
                "!wow, !와우\n" +
                "      러블리즈 - WoW!\n" +
                "!ndk, !nodoka, !노도카\n" +
                "      마나베 노도카 - 햇살이 비치는 Living\n" +
                "!nana, !usaming, !미미밍\n" +
                "      아베 나나 - 메르헨 데뷔!\n" +
                "!fumika, !brightblue, !문과만세\n" +
                "      사기사와 후미카 - Bright Blue" +
                "```"
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
                "!now    => !지금, !시간, !time\n" +
                "!utc    => !표준시, !유티씨\n" +
                "!info   => !정보, !information\n" +
                "!help   => !도움, !도움말, !도움!\n" +
                "!alias  => !명령어, !다른거,\n" +
                "!google => !구글, \n" +
                "-----------------------------------------------------\n" +
                "!wow    => !와우, !와우!, !wow!\n" +
                "!cjl    => !candyjellylove, !캔디젤리러브, !캔디젤리럽,\n" +
                "           !캔젤럽, !캔디 젤리 러브\n" +
                "!ndk    => !nodoka, !노도카, !마나베 노도카\n" +
                "!nana   => !usaming, !우사밍, !나나, !아베 나나,\n" +
                "           !미미밍, !미미밍!, !mimiming, !mimiming!\n" +
                "!fumika => !brightblue, !후미카, !사기사와 후미카,\n" +
                "           !문과, !문과 만세!, !문과만세, !문과 만세\n" +
                "```"
            };
            await ReplyAsync("", false, embed);
        }

        [Command("now")]
        [Alias("지금", "시간", "time")]
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
        [Alias("표준시", "유티씨")]
        [Summary("Display UTC time")]
        public async Task UTC()
        {
            await ReplyAsync(DateTime.UtcNow.ToString());
        }

        // 이하 유튜브 링크 출력. 데이터 파일로 관리하면 좋겠지만...

        [Command("wow")]
        [Alias("wow!", "와우", "와우!")]
        [Summary("WoW!")]
        public async Task WoW()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=a1ENnG-s630");
        }

        [Command("cjl")]
        [Alias("candyjellylove", "캔디젤리러브", "캔디젤리럽", "캔젤럽", "캔디 젤리 러브")]
        [Summary("Candy~ Jelly~ Love!")]
        public async Task CandyJellyLove()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=HRQEs4vOIrY");
        }

        [Command("ndk")]
        [Alias("nodoka", "노도카", "마나베 노도카")]
        [Summary("Nodoka Character Song")]
        public async Task Nodoka()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=Gf_MV1Vx08o");
        }

        [Command("nana")]
        [Alias("usaming", "우사밍", "나나", "아베 나나", "미미밍", "미미밍!","mimiming","mimiming!")]
        [Summary("Mi-Mi-Ming!x2 Usaming!")]
        public async Task Nana()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=1Rm9O_UvBWE");
        }

        [Command("fumika")]
        [Alias("brightblue", "후미카", "사기사와 후미카", "문과", "문과 만세!", "문과만세", "문과 만세")]
        [Summary("문과 만세!!!")]
        public async Task Fumika()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=acPzzC8SPL4");
        }

        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}