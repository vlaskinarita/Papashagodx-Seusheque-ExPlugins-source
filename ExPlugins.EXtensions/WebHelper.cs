using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.AutoLoginEx;
using ExPlugins.BulkTraderEx.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ExPlugins.EXtensions;

public class WebHelper
{
	public struct PostResponse
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private List<string> list_0;

		public string Id
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public List<string> Result
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class Status
	{
		public string option;
	}

	public class Value
	{
		public int? max;

		public int? min;

		public int? option;
	}

	public class Filters
	{
		public Category category;

		public Corrupted corrupted;

		public GemLevel gem_level;

		public string id;

		public Ilvl ilvl;

		public Links links;

		public MapBlighted map_blighted;

		public MapFilters map_filters;

		public MapRegion map_region;

		public MapTier map_tier;

		public MiscFilters misc_filters;

		public BuyoutPrice price;

		public Quality quality;

		public Rarity rarity;

		public ScourgeTier scourge_tier;

		public SocketFilters socket_filters;

		public Sockets sockets;

		public Split split;

		public TradeFilters trade_filters;

		public TypeFilters type_filters;

		public Value value;

		public bool? disabled;
	}

	public class Stat
	{
		public string type;

		public List<Filters> filters;

		public Value value;
	}

	public class Category
	{
		public string option;
	}

	public class Rarity
	{
		public string option;
	}

	public class TypeFilters
	{
		public bool? disabled;

		public Filters filters;
	}

	public class Sockets
	{
		public int? b;

		public int? g;

		public int? max;

		public int? min;

		public int? r;

		public int? w;
	}

	public class Links
	{
		public int? b;

		public int? g;

		public int? max;

		public int? min;

		public int? r;

		public int? w;
	}

	public class SocketFilters
	{
		public Filters filters;
	}

	public class TradeFilters
	{
		public Filters filters;
	}

	public class BuyoutPrice
	{
		public int max;

		public int min;
	}

	public class MapTier
	{
		public int? max;

		public int? min;
	}

	public class MapRegion
	{
		public string option;
	}

	public class MapBlighted
	{
		public string option;
	}

	public class MapFilters
	{
		public Filters filters;
	}

	public class Ilvl
	{
		public int? max;

		public int? min;
	}

	public class Quality
	{
		public int? max;

		public int? min;
	}

	public class GemLevel
	{
		public int? max;

		public int? min;
	}

	public class ScourgeTier
	{
		public int? max;

		public int? min;
	}

	public class Corrupted
	{
		public string option;
	}

	public class Split
	{
		public string option;
	}

	public class MiscFilters
	{
		public Filters filters;
	}

	public class Query
	{
		public Status status;

		public string name;

		public string type;

		public List<Stat> stats;

		public Filters filters;
	}

	public class Sort
	{
		public string price;
	}

	public class Root
	{
		public Query query;

		public Sort sort;
	}

	public class StatEntry
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private string string_2;

		public string id
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string text
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public string type
		{
			[CompilerGenerated]
			get
			{
				return string_2;
			}
			[CompilerGenerated]
			set
			{
				string_2 = value;
			}
		}
	}

	public class StatListResult
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private List<StatEntry> list_0;

		public string label
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public List<StatEntry> entries
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class StatListRoot
	{
		[CompilerGenerated]
		private List<StatListResult> list_0;

		public List<StatListResult> result
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class DirectWhisperBody
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private List<int> list_0;

		public string token
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public List<int> values
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class StaticJsonEntry
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private string string_2;

		public string id
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string text
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public string image
		{
			[CompilerGenerated]
			get
			{
				return string_2;
			}
			[CompilerGenerated]
			set
			{
				string_2 = value;
			}
		}
	}

	public class StaticJsonResult
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private List<StaticJsonEntry> list_0;

		public string id
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string label
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public List<StaticJsonEntry> entries
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class StaticJsonRoot
	{
		[CompilerGenerated]
		private List<StaticJsonResult> list_0;

		public List<StaticJsonResult> result
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}
	}

	public class BulkPrice
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private int int_2;

		[CompilerGenerated]
		private string string_2;

		[CompilerGenerated]
		private bool bool_0;

		public string Whisper
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string WhisperToken
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public int Stock
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		public double Price
		{
			[CompilerGenerated]
			get
			{
				return double_0;
			}
			[CompilerGenerated]
			set
			{
				double_0 = value;
			}
		}

		public int MinSell
		{
			[CompilerGenerated]
			get
			{
				return int_1;
			}
			[CompilerGenerated]
			set
			{
				int_1 = value;
			}
		}

		public int MinBuy
		{
			[CompilerGenerated]
			get
			{
				return int_2;
			}
			[CompilerGenerated]
			set
			{
				int_2 = value;
			}
		}

		public string Account
		{
			[CompilerGenerated]
			get
			{
				return string_2;
			}
			[CompilerGenerated]
			set
			{
				string_2 = value;
			}
		}

		public bool Afk
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}
	}

	public class Exchange
	{
		[JsonProperty(Order = 4)]
		public int minimum;

		[CompilerGenerated]
		private Status status_0;

		[CompilerGenerated]
		private List<string> list_0;

		[CompilerGenerated]
		private List<string> list_1;

		[JsonProperty(Order = 1)]
		public Status status
		{
			[CompilerGenerated]
			get
			{
				return status_0;
			}
			[CompilerGenerated]
			set
			{
				status_0 = value;
			}
		}

		[JsonProperty(Order = 2)]
		public List<string> have
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}

		[JsonProperty(Order = 3)]
		public List<string> want
		{
			[CompilerGenerated]
			get
			{
				return list_1;
			}
			[CompilerGenerated]
			set
			{
				list_1 = value;
			}
		}
	}

	public class RootBulk
	{
		[JsonProperty(Order = 2)]
		public string engine;

		[CompilerGenerated]
		private Exchange exchange_0;

		[JsonProperty(Order = 1)]
		public Exchange query
		{
			[CompilerGenerated]
			get
			{
				return exchange_0;
			}
			[CompilerGenerated]
			set
			{
				exchange_0 = value;
			}
		}
	}

	private static DateTime dateTime_0;

	public static StaticJsonRoot StaticJson;

	public static StatListRoot ModListJson;

	private static int int_0;

	private static int int_1;

	private static int int_2;

	public static CookieContainer GetCookie()
	{
		CookieContainer cookieContainer = new CookieContainer();
		cookieContainer.Add(new Cookie("POESESSID", AutoLoginSettings.Instance.Poesessid, "/", ".pathofexile.com"));
		return cookieContainer;
	}

	public static async Task<bool> DirectWhisper(int WeWant, string token)
	{
		Uri url = new Uri("https://www.pathofexile.com/api/trade/whisper");
		DirectWhisperBody body = new DirectWhisperBody
		{
			token = token,
			values = new List<int> { WeWant }
		};
		string json = JsonConvert.SerializeObject((object)body, (Formatting)0);
		RestClient client = new RestClient(url)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
			CookieContainer = GetCookie()
		};
		client.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
		client.AddDefaultHeader("x-requested-with", "XMLHttpRequest");
		RestRequest request = new RestRequest(Method.POST)
		{
			RequestFormat = DataFormat.Json
		};
		request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
		Task<IRestResponse> postTask = client.ExecutePostTaskAsync(request);
		if (Coroutine.Current != null)
		{
			postTask = Coroutine.ExternalTask<IRestResponse>(postTask);
		}
		IRestResponse response = await postTask;
		HttpStatusCode statusCode = response.StatusCode;
		string resultP = response.Content;
		if (statusCode != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[DirectWhisper][{statusCode}][{resultP}]");
			return false;
		}
		ChatEntry msg = ChatPanel.Messages.LastOrDefault((ChatEntry m) => m.Message.Contains("@To") && (int)m.MessageType == 3);
		if (msg != null)
		{
			string msgString = msg.Message;
			GlobalLog.Debug(string.Format(arg1: Regex.Replace(msgString, "[^\\u0000-\\u007F]+", string.Empty), format: "[DirectWhisper][{0}][{1}]", arg0: statusCode));
		}
		return true;
	}

	public static async Task<string> GetAffixTradeId(ModAffix affix, bool Implicit)
	{
		await GetModListJson();
		string description = affix.Stats.First().Description;
		string formatted = description.Replace(":+d", "").Replace("\\n", " ");
		formatted = Regex.Replace(formatted, "{[0-9]}", "#");
		bool fractured = affix.IsFractured;
		bool enchant = affix.Category.Equals("SkillEnchantment");
		List<StatListResult> modList = ModListJson.result;
		modList = (fractured ? Enumerable.Where(modList, (StatListResult e) => e.label.Equals("Fractured")).ToList() : (enchant ? Enumerable.Where(modList, (StatListResult e) => e.label.Equals("Enchant")).ToList() : (Implicit ? Enumerable.Where(modList, (StatListResult e) => e.label.Equals("Implicit")).ToList() : Enumerable.Where(modList, (StatListResult e) => e.label.Equals("Explicit")).ToList())));
		foreach (StatEntry entry in modList.SelectMany((StatListResult s) => s.entries))
		{
			string text = Regex.Replace(entry.text, "\\s+", " ");
			if (text.Contains(formatted))
			{
				GlobalLog.Warn("[" + affix.DisplayName + "][" + formatted + "] trade mod id: " + entry.id + " type: " + entry.type);
				return entry.id;
			}
		}
		return "";
	}

	private static async Task HandleRateLimit()
	{
		dateTime_0 = DateTime.Now;
		GlobalLog.Debug($"[WebHelper] Rate limit {int_0}/{int_1} Wait: {Math.Max(int_0 * int_0, int_2)}");
		while (int_0 >= 2 && dateTime_0.AddSeconds(int_0 * int_0) > DateTime.Now)
		{
			GlobalLog.Debug($"[WebHelper] Waiting: {dateTime_0.AddSeconds(int_0 * int_0) - DateTime.Now:ss} sec before next request");
			if (Coroutine.Current != null)
			{
				await Wait.SleepSafe(750);
			}
			else
			{
				Thread.Sleep(750);
			}
		}
		while (int_2 != 0 && dateTime_0.AddSeconds(int_2) > DateTime.Now)
		{
			GlobalLog.Debug($"[WebHelper] Waiting: {dateTime_0.AddSeconds(int_2) - DateTime.Now:ss} sec before next request");
			if (Coroutine.Current == null)
			{
				Thread.Sleep(750);
			}
			else
			{
				await Wait.SleepSafe(750);
			}
		}
	}

	public static async Task GetModListJson()
	{
		if (ModListJson == null)
		{
			Uri url = new Uri("https://www.pathofexile.com/api/trade/data/stats");
			RestClient client = new RestClient(url)
			{
				UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
				BaseUrl = url
			};
			if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
			{
				client.CookieContainer = GetCookie();
			}
			Task<IRestResponse> downloadTask = client.ExecuteGetTaskAsync(new RestRequest());
			if (Coroutine.Current != null)
			{
				downloadTask = Coroutine.ExternalTask<IRestResponse>(downloadTask);
			}
			IRestResponse resonse = await downloadTask;
			HttpStatusCode statusCode = resonse.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				GlobalLog.Error($"[WebHelper:GetStatlistJson][{statusCode}]");
				return;
			}
			GlobalLog.Debug($"[WebHelper:GetStatlistJson][{statusCode}]");
			StatListRoot json = JsonConvert.DeserializeObject<StatListRoot>(resonse.Content);
			ModListJson = json;
		}
	}

	public static async Task GetStaticJson()
	{
		Uri url = new Uri("https://www.pathofexile.com/api/trade/data/static");
		RestClient client = new RestClient(url)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
			BaseUrl = url
		};
		if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
		{
			client.CookieContainer = GetCookie();
		}
		Task<IRestResponse> downloadTask = client.ExecuteGetTaskAsync(new RestRequest());
		if (Coroutine.Current != null)
		{
			downloadTask = Coroutine.ExternalTask<IRestResponse>(downloadTask);
		}
		IRestResponse resonse = await downloadTask;
		HttpStatusCode statusCode = resonse.StatusCode;
		if (statusCode != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[WebHelper:GetStaticJson][{statusCode}]");
			return;
		}
		GlobalLog.Debug($"[WebHelper:GetStaticJson][{statusCode}]");
		StaticJson = JsonConvert.DeserializeObject<StaticJsonRoot>(resonse.Content);
	}

	public static string GetStaticString()
	{
		Uri baseUrl = new Uri("https://www.pathofexile.com/api/trade/data/static");
		RestClient restClient = new RestClient(baseUrl)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
			BaseUrl = baseUrl
		};
		if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
		{
			restClient.CookieContainer = GetCookie();
		}
		IRestResponse restResponse = restClient.Get(new RestRequest());
		HttpStatusCode statusCode = restResponse.StatusCode;
		if (statusCode == HttpStatusCode.OK)
		{
			GlobalLog.Debug($"[WebHelper:GetStaticJson][{statusCode}]");
			return restResponse.Content;
		}
		GlobalLog.Error($"[WebHelper:GetStaticJson][{statusCode}]");
		return null;
	}

	public static string GetModListString()
	{
		Uri baseUrl = new Uri("https://www.pathofexile.com/api/trade/data/stats");
		RestClient restClient = new RestClient(baseUrl)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
			BaseUrl = baseUrl
		};
		if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
		{
			restClient.CookieContainer = GetCookie();
		}
		IRestResponse restResponse = restClient.Get(new RestRequest());
		HttpStatusCode statusCode = restResponse.StatusCode;
		if (statusCode != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[WebHelper:GetStatlistJson][{statusCode}]");
			return null;
		}
		GlobalLog.Debug($"[WebHelper:GetStatlistJson][{statusCode}]");
		return restResponse.Content;
	}

	public static async Task<double> GetItemPrice(Item item, List<ModAffix> statsToCheck, string league, bool checkIlvl = false, bool checkCorrupted = false)
	{
		await HandleRateLimit();
		List<double> prices = new List<double>();
		string json = await JGen(item, statsToCheck, checkIlvl, checkCorrupted);
		Uri url = new Uri("https://www.pathofexile.com/api/trade/search/" + league);
		RestClient client = new RestClient(url)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36"
		};
		if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
		{
			client.CookieContainer = GetCookie();
		}
		client.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
		RestRequest request = new RestRequest(Method.POST)
		{
			RequestFormat = DataFormat.Json
		};
		request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
		Task<IRestResponse> postTask = client.ExecutePostTaskAsync(request);
		if (Coroutine.Current != null)
		{
			postTask = Coroutine.ExternalTask<IRestResponse>(postTask);
		}
		IRestResponse responseP = await postTask;
		HttpStatusCode statusCode2 = responseP.StatusCode;
		string resultP = responseP.Content;
		object obj = Enumerable.FirstOrDefault(responseP.Headers, (Parameter h) => h.Name == "X-Rate-Limit-Account-State")?.Value;
		if (obj is string headerP)
		{
			int_2 = int.Parse(headerP.Split(':')[2]);
			int_1 = int.Parse(headerP.Split(':')[1]);
			int_0 = int.Parse(headerP.Split(':')[0]);
		}
		if (statusCode2 != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[WebHelper:GetItemPrice:POST][{statusCode2}]");
			return -1.0;
		}
		GlobalLog.Debug($"[WebHelper:GetItemPrice:POST][{statusCode2}]");
		PostResponse content = JsonConvert.DeserializeObject<PostResponse>(resultP);
		string id = content.Id;
		List<string> results = content.Result.Take(10).ToList();
		await HandleRateLimit();
		string fetchString = "https://www.pathofexile.com/api/trade/fetch/" + string.Join(",", results) + "?query=" + id;
		client.BaseUrl = new Uri(fetchString);
		Task<IRestResponse> getTask = client.ExecuteGetTaskAsync(new RestRequest());
		if (Coroutine.Current != null)
		{
			getTask = Coroutine.ExternalTask<IRestResponse>(getTask);
		}
		IRestResponse responseG = await getTask;
		statusCode2 = responseG.StatusCode;
		string resultG = responseG.Content;
		obj = Enumerable.FirstOrDefault(responseG.Headers, (Parameter h) => h.Name == "X-Rate-Limit-Account-State")?.Value;
		if (obj is string headerG)
		{
			int_2 = int.Parse(headerG.Split(':')[2]);
			int_1 = int.Parse(headerG.Split(':')[1]);
			int_0 = int.Parse(headerG.Split(':')[0]);
		}
		if (statusCode2 != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[WebHelper:GetItemPrice:GET][{statusCode2}]");
			GlobalLog.Info(json);
			return -1.0;
		}
		GlobalLog.Debug($"[WebHelper:GetItemPrice:GET][{statusCode2}]");
		JObject result = JObject.Parse(resultG);
		foreach (dynamic listing in ((dynamic)result).result)
		{
			string currency = listing.listing.price.currency;
			double price = listing.listing.price.amount;
			if (currency.ContainsIgnorecase("divine"))
			{
				price *= PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb");
			}
			prices.Add(price);
		}
		return Math.Floor(prices.Sum() / (double)prices.Count);
	}

	public static async Task<List<BulkPrice>> Bulk(string WeHave, string WeWant, string league, int minimum = 1)
	{
		await HandleRateLimit();
		List<BulkPrice> prices = new List<BulkPrice>();
		string json = JGenBulk(WeHave, WeWant, minimum);
		Uri url = new Uri("https://www.pathofexile.com/api/trade/exchange/" + league);
		RestClient client = new RestClient(url)
		{
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36"
		};
		if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.Poesessid))
		{
			client.CookieContainer = GetCookie();
		}
		client.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
		RestRequest request = new RestRequest(Method.POST)
		{
			RequestFormat = DataFormat.Json
		};
		request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
		Task<IRestResponse> postTask = client.ExecutePostTaskAsync(request);
		if (Coroutine.Current != null)
		{
			postTask = Coroutine.ExternalTask<IRestResponse>(postTask);
		}
		IRestResponse response = await postTask;
		HttpStatusCode statusCode = response.StatusCode;
		string resultP = response.Content;
		if (statusCode != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[WebHelper:BulkPOST] {WeHave} => {WeWant} [{statusCode}]");
			GlobalLog.Info(json);
			return prices;
		}
		GlobalLog.Debug($"[WebHelper:BulkPOST]{WeHave} => {WeWant} [{statusCode}]");
		JObject postRespDecoded = JObject.Parse(resultP);
		dynamic result = ((IEnumerable<JToken>)Extensions.Children<JToken>((IEnumerable<JToken>)((IEnumerable<JToken>)Extensions.Children<JToken>((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((JToken)postRespDecoded).Children()).ToList()[2].Children())).ToList())).ToList();
		foreach (dynamic t in result)
		{
			BulkPrice deal = new BulkPrice
			{
				Whisper = t.listing.whisper,
				WhisperToken = t.listing.whisper_token,
				Stock = t.listing.offers.First.item.stock,
				MinSell = (int)t.listing.offers[0].item.amount,
				MinBuy = (int)t.listing.offers[0].exchange.amount,
				Price = (double)t.listing.offers[0].exchange.amount / (double)t.listing.offers[0].item.amount,
				Account = t.listing.account.name,
				Afk = (t.listing.account.online.status != null && t.listing.account.online.status == "afk")
			};
			prices.Add(deal);
		}
		object obj = Enumerable.FirstOrDefault(response.Headers, (Parameter h) => h.Name == "X-Rate-Limit-Account-State")?.Value;
		if (obj is string header)
		{
			int_2 = int.Parse(header.Split(':')[2]);
			int_1 = int.Parse(header.Split(':')[1]);
			int_0 = int.Parse(header.Split(':')[0]);
		}
		return (from p in Enumerable.Where(prices, (BulkPrice p) => !p.Afk)
			orderby p.Price
			select p).ToList();
	}

	private static string JGenBulk(string WeHave, string WeWant, int minimum)
	{
		Exchange query = new Exchange
		{
			status = new Status
			{
				option = "onlineleague"
			},
			have = new List<string> { CurrencyHelper.Currencies.First((CurrencyHelper.Currency c) => c.Name.EqualsIgnorecase(WeHave)).Id },
			want = new List<string> { CurrencyHelper.Currencies.First((CurrencyHelper.Currency c) => c.Name.EqualsIgnorecase(WeWant)).Id },
			minimum = minimum
		};
		RootBulk rootBulk = new RootBulk
		{
			engine = "new",
			query = query
		};
		return JsonConvert.SerializeObject((object)rootBulk, (Formatting)0);
	}

	public static async Task<string> JGen(Item item, List<ModAffix> statsToCheck, bool checkIlvl, bool checkCorrupted)
	{
		List<Filters> statFilters = new List<Filters>();
		foreach (ModAffix affix in item.ExplicitAffixes)
		{
			if (!statsToCheck.Any() || statsToCheck.Select((ModAffix a) => a.DisplayName).Contains(affix.DisplayName))
			{
				string id = await GetAffixTradeId(affix, Implicit: false);
				if (!string.IsNullOrWhiteSpace(id))
				{
					Filters filter = new Filters
					{
						id = id,
						value = new Value
						{
							min = (int)Math.Ceiling((double)affix.Values.First() * 0.95)
						},
						disabled = false
					};
					statFilters.Add(filter);
				}
			}
		}
		foreach (ModAffix affix2 in item.ImplicitAffixes)
		{
			if (!statsToCheck.Any() || statsToCheck.Select((ModAffix a) => a.DisplayName).Contains(affix2.DisplayName))
			{
				string id2 = await GetAffixTradeId(affix2, Implicit: true);
				if (!string.IsNullOrWhiteSpace(id2))
				{
					Filters filter2 = new Filters
					{
						id = id2,
						value = new Value
						{
							min = (int)Math.Ceiling((double)affix2.Values.First() * 0.95)
						},
						disabled = false
					};
					statFilters.Add(filter2);
				}
			}
		}
		Root root = new Root();
		Filters filters = new Filters();
		Stat stat = new Stat
		{
			type = "and",
			filters = statFilters
		};
		Query query = new Query
		{
			status = new Status
			{
				option = "onlineleague"
			},
			type = item.Name,
			stats = new List<Stat> { stat }
		};
		if ((int)item.Rarity == 3)
		{
			query.name = item.FullName;
		}
		TypeFilters typeFilters = new TypeFilters();
		Filters typefil = new Filters();
		Rarity rarity2 = new Rarity();
		Rarity val;
		string option;
		if (item.IsFractured)
		{
			val = (Rarity)1;
			option = ((object)(Rarity)(ref val)).ToString().ToLower();
		}
		else
		{
			val = item.Rarity;
			option = ((object)(Rarity)(ref val)).ToString().ToLower();
		}
		rarity2.option = option;
		Rarity rarity = rarity2;
		typefil.rarity = rarity;
		typeFilters.filters = typefil;
		typeFilters.disabled = false;
		filters.type_filters = typeFilters;
		if (item.MaxLinkCount == 6)
		{
			filters.socket_filters = new SocketFilters
			{
				filters = new Filters
				{
					links = new Links
					{
						min = 6
					}
				}
			};
		}
		MiscFilters miscFilters = new MiscFilters();
		Filters miscfil = new Filters();
		if (checkIlvl)
		{
			Ilvl ilvl = new Ilvl
			{
				min = item.ItemLevel
			};
			miscfil.ilvl = ilvl;
		}
		if (checkCorrupted && item.IsCorrupted)
		{
			Corrupted corrupted = new Corrupted
			{
				option = "true"
			};
			miscfil.corrupted = corrupted;
		}
		miscFilters.filters = miscfil;
		filters.misc_filters = miscFilters;
		query.filters = filters;
		root.query = query;
		root.sort = new Sort
		{
			price = "asc"
		};
		return JsonConvert.SerializeObject((object)root, (Formatting)0, new JsonSerializerSettings
		{
			NullValueHandling = (NullValueHandling)1
		});
	}

	static WebHelper()
	{
		dateTime_0 = DateTime.MinValue;
	}
}
