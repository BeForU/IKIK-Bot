using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.NET_Core
{
	internal class GoogleCrawler
	{
		public async Task Google(SocketUserMessage message)
		{
			await message.Channel.SendMessageAsync("구글에서 검색해볼게요!");
			await message.Channel.TriggerTypingAsync();

			string keyword = message.Content;

			if (message.Content.StartsWith("!google", StringComparison.OrdinalIgnoreCase))
			{
				if (keyword.Length < 8)
				{
					await message.Channel.SendMessageAsync("어라라라...? 검색어가 없네요?! \"!google 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(8);
			}
			else if (message.Content.StartsWith("!rnrmf"))
			{
				if (keyword.Length < 7)
				{
					await message.Channel.SendMessageAsync("어라라라...? 검색어가 없네요?! \"!구글 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(7);
			}
			else if (message.Content.StartsWith("!구글"))
			{
				if (keyword.Length < 4)
				{
					await message.Channel.SendMessageAsync("어라라라...? 검색어가 없네요?! \"!구글 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(4);
			}

			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0");

			var buffer = await httpClient.GetByteArrayAsync($"http://www.google.com/search?num=3&q={keyword}");
			var byteArray = buffer.ToArray();
			var str = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

			var regex = new Regex("<span class=\"st\">(.*?)[^ ]</span>");
			var matches = regex.Matches(str).Cast<Match>().ToList();
			string[] descs = new string[matches.Count];

			// 설명글 추출
			for (int i = 0; i < matches.Count; i++)
			{
				descs[i] = Regex.Replace(matches[i].Value, @"<[^>]*>", String.Empty);
			}

			//1차 정리
			regex = new Regex("<div class=\"g\"(.*?)</div>");
			matches = regex.Matches(str).Cast<Match>().ToList();

			str = String.Join("", matches);
			//Console.WriteLine(str);

			//제목 추출
			regex = new Regex("event(.)\">[^<](.*?)</a>");
			var titles = regex.Matches(str).Cast<Match>().ToList();

			for (int i = 0; i < titles.Count; i++)
			{
				if (titles[i].Value.Contains("저장된&nbsp;페이지") || titles[i].Value.Equals(""))
				{
					titles.Remove(titles[i]);
				}
			}

			//링크 추출
			regex = new Regex("<a href=\"(.*?)\" onmousedown=\"");
			var linkes = regex.Matches(str).Cast<Match>().ToList();

			for (int i = 0; i < linkes.Count; i++)
			{
				if (linkes[i].Value.Contains(" data-ved="))
				{
					linkes.Remove(linkes[i]);
				}
			}

			//브라우저 링크 띄움.
			EmbedBuilder embed = new EmbedBuilder()
			{
				Color = new Color(79, 84, 92),
				Title = keyword + " - Google 검색 (브라우저 열기)",
				Url = $"http://www.google.com/search?q={WebUtility.UrlEncode(keyword)}",
			};
			await message.Channel.SendMessageAsync("", false, embed);

			//Console.WriteLine("Command: Google 검색");

			// 결과 정리 및 출력
			int count = 0;
			for (int i = 0; (i < titles.Count) && count < 2; i++)
			{
				embed.Color = new Color(0, 150, 207);

				str = titles[i].Value;
				str = str.Substring(8, str.Length - 12);
				str = WebUtility.HtmlDecode(str);
				embed.Title = str;

				str = linkes[i].Value;
				str = str.Substring(9, str.Length - 24);
				str = WebUtility.HtmlDecode(str);
				embed.Url = str;

				str = descs[i];
				str = WebUtility.HtmlDecode(str);
				str = WebUtility.UrlDecode(str);
				embed.Description = str;

				count++;

				await message.Channel.SendMessageAsync("", false, embed);
			}
		}
	}
}