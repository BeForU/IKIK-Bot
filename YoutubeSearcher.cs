using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Threading.Tasks;

//https://developers.google.com/youtube/v3/docs/search/list?hl=ko 참고...
namespace DiscordBot.NET_Core
{
	internal class YoutubeSearcher
	{
		public async Task Youtube(SocketUserMessage message)
		{
			await message.Channel.SendMessageAsync("유튜브에서 찾아볼게요!");
			await message.Channel.TriggerTypingAsync();

			string token = System.IO.File.ReadAllText("token/google.txt");

			string keyword = message.Content;

			if (message.Content.StartsWith("!youtube", StringComparison.OrdinalIgnoreCase))
			{
				if (keyword.Length < 9)
				{
					await message.Channel.SendMessageAsync("검색어가 없네요?! \"!youtube 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(9);
			}
			else if (message.Content.StartsWith("!dbxbqm"))
			{
				if (keyword.Length < 8)
				{
					await message.Channel.SendMessageAsync("검색어가 없네요?! \"!유튜브 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(8);
			}
			else if (message.Content.StartsWith("!유튜브"))
			{
				if (keyword.Length < 5)
				{
					await message.Channel.SendMessageAsync("검색어가 없네요?! \"!유튜브 검색어\" 방식으로 써 주세요!");
					return;
				}

				keyword = message.Content.Substring(5);
			}

			//Console.WriteLine("Youtube Search");

			YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = token
			});

			SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
			listRequest.Type = "video"; // 비디오만 검색합니다.
			listRequest.MaxResults = 3; // 3개까지만 검색합니다.
			listRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.None; // 안전필터? 필요없습니다.
			listRequest.Q = keyword; //= CommandLine.RequestUserInput<string>("Search term: ");
			listRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;

			SearchListResponse searchResponse = listRequest.Execute();

			if (searchResponse.Items.Count != 0)
			{
				await message.Channel.SendMessageAsync($"http://youtu.be/{searchResponse.Items[0].Id.VideoId}\n");
			}
			else
			{
				await message.Channel.SendMessageAsync("유튜브에 검색 결과가 안나와요! 헐!");
			}

			if (searchResponse.Items.Count > 1)
			{
				for (int i = 1; i < searchResponse.Items.Count; i++)
				{
					EmbedBuilder embed = new EmbedBuilder()
					{
						//Color = new Color(79, 84, 92),
						Color = new Color(0, 150, 207),
						Description = searchResponse.Items[i].Snippet.Title +
						$"\nhttp://youtu.be/{searchResponse.Items[i].Id.VideoId}\n"
					};

					await message.Channel.SendMessageAsync("", false, embed);
				}
			}
		}
	}
}