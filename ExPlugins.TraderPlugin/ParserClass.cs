using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExPlugins.EXtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExPlugins.TraderPlugin;

public class ParserClass
{
	public struct PostResponse
	{
		public string id;

		public int? complexity;

		public string[] result;

		public int total;
	}

	public struct Price
	{
		public double amount;

		public string currency;

		public string acc;

		public Price(double amount, string currency, string acc)
		{
			this.amount = amount;
			this.currency = currency;
			this.acc = acc;
		}
	}

	public struct BulkPrice
	{
		public string str;

		public int amt;

		public double worth;

		public int minHave;

		public int minWant;

		public string acc;
	}

	public struct Item4Search
	{
		public string name;

		public string type;

		public string category;

		public string rarity;

		public int[] sockets;

		public int[] links;

		public int links_min;

		public int map_tier;

		public string map_region;

		public bool? blighted_map;

		public int item_lvl;

		public int quality;

		public int gem_lvl;

		public bool? corrupted;

		public bool? split;

		public int scourge_tier;

		public int buyout_price_min;

		public int buyout_price_max;

		public string mods_type;

		public int mods_type_value_min;

		public int mods_type_value_max;

		public List<StatFilter> mods;
	}

	public struct StatFilter
	{
		public string name;

		public string value;

		public string type;

		public string min;

		public string max;

		public StatFilter(string name, string value, string type)
		{
			this.name = name;
			this.type = type;
			this.value = value;
			min = "0";
			max = "0";
		}

		public StatFilter(string name, string value, string type, string min, string max)
		{
			this.name = name;
			this.type = type;
			this.value = value;
			this.min = min;
			this.max = max;
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

		public bool? disabled;

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
	}

	public class Stat
	{
		public List<Filters> filters;

		public string type;

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
		public Filters filters;

		public string name;

		public List<Stat> stats;

		public Status status;

		public string type;
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

	public class Exchange
	{
		public int minimum;

		[CompilerGenerated]
		private Status status_0;

		[CompilerGenerated]
		private List<string> list_0;

		[CompilerGenerated]
		private List<string> list_1;

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
		public string engine;

		[CompilerGenerated]
		private Exchange exchange_0;

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

	private static HttpClient httpClient_0;

	private static readonly HttpClient httpClient_1;

	private static HttpClient httpClient_2;

	private static string string_0;

	private static string string_1;

	private static readonly string vuiCdoaNr5;

	private static readonly CookieContainer cookieContainer_0;

	public static Dictionary<string, int> ModType;

	private static void Main(string[] args)
	{
		Starter();
		Console.ReadKey();
	}

	public static void Starter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		httpClient_0 = new HttpClient();
		httpClient_2 = new HttpClient();
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Clear();
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Authority", "www.pathofexile.com");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Method", "POST");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Path", "/api/trade/search/" + vuiCdoaNr5);
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:90.0) Gecko/20100101 Firefox/90.0");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Accept", "*/*");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Accept-Language", "en-US,en;q=0.5");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("X-Requested-With", "XMLHttpRequest");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Origin", "https://www.pathofexile.com");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Connection", "keep-alive");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Referer", "https://www.pathofexile.com/trade/search/" + vuiCdoaNr5);
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Sec-Fetch-Dest", "empty");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Sec-Fetch-Mode", "cors");
		((HttpHeaders)httpClient_0.DefaultRequestHeaders).Add("Sec-Fetch-Site", "same-origin");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Clear();
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Authority", "www.pathofexile.com");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Method", "POST");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Path", "/api/trade/exchange/" + vuiCdoaNr5);
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:90.0) Gecko/20100101 Firefox/90.0");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Accept", "*/*");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Accept-Language", "en-US,en;q=0.5");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("X-Requested-With", "XMLHttpRequest");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Origin", "https://www.pathofexile.com");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Connection", "keep-alive");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Referer", "https://www.pathofexile.com/trade/exchange/" + vuiCdoaNr5);
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Sec-Fetch-Dest", "empty");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Sec-Fetch-Mode", "cors");
		((HttpHeaders)httpClient_2.DefaultRequestHeaders).Add("Sec-Fetch-Site", "same-origin");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Clear();
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("Authority", "www.pathofexile.com");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:90.0) Gecko/20100101 Firefox/90.0");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("Accept", "*/*");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("Accept-Language", "en-US,en;q=0.5");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("X-Requested-With", "XMLHttpRequest");
		((HttpHeaders)httpClient_1.DefaultRequestHeaders).Add("Connection", "keep-alive");
		string_1 = WebHelper.GetModListString();
		string_0 = WebHelper.GetStaticString();
	}

	public static async Task<List<Price>> SearchListedItems(Item4Search item)
	{
		List<Price> prices = new List<Price>();
		string json = JGen(item);
		StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
		string resultP = (await httpClient_0.PostAsync("https://www.pathofexile.com/api/trade/search/" + vuiCdoaNr5, (HttpContent)(object)data)).Content.ReadAsStringAsync().Result;
		PostResponse PostRespDecoded = JsonConvert.DeserializeObject<PostResponse>(resultP);
		if (GetUrl(PostRespDecoded) != "0")
		{
			string resultG = (await httpClient_1.GetAsync(GetUrl(PostRespDecoded))).Content.ReadAsStringAsync().Result;
			dynamic GetRespDecoded = JsonConvert.DeserializeObject(resultG);
			int limiter = GetRespDecoded.result.Count;
			if (limiter > 10)
			{
				limiter = 10;
			}
			for (int i = 0; i < limiter; i++)
			{
				Price price_buff = new Price
				{
					amount = GetRespDecoded.result[i].listing.price.amount,
					currency = GetRespDecoded.result[i].listing.price.currency,
					acc = GetRespDecoded.result[i].listing.account.name
				};
				GlobalLog.Debug($"Amount: {price_buff.amount} Currency: {price_buff.currency}");
				prices.Add(price_buff);
			}
		}
		return prices;
	}

	public static async Task<List<BulkPrice>> Bulk(string WeHave, string WeWant, int minimum = 1)
	{
		GlobalLog.Debug("[Bulk] Bulk Started");
		Console.WriteLine("Bulk Started");
		GlobalLog.Debug($"[123123]{cookieContainer_0.Count}");
		List<BulkPrice> prices = new List<BulkPrice>();
		try
		{
			GlobalLog.Debug("[Bulk] 2");
			string json = JGenBulk(WeHave, WeWant, minimum);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
			GlobalLog.Debug("[Bulk] 3");
			HttpResponseMessage responseP = await httpClient_2.PostAsync("https://www.pathofexile.com/api/trade/exchange/" + vuiCdoaNr5, (HttpContent)(object)data);
			GlobalLog.Debug("[Bulk] 4");
			string resultP = responseP.Content.ReadAsStringAsync().Result;
			GlobalLog.Debug("[Bulk] 5");
			JObject PostRespDecoded = JObject.Parse(resultP);
			GlobalLog.Debug("[Bulk] 6");
			GlobalLog.Debug($"[PostRespDecoded] {PostRespDecoded}");
			GlobalLog.Debug("[BulkJson] " + json);
			List<JToken> result = ((IEnumerable<JToken>)Extensions.Children<JToken>((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((JToken)PostRespDecoded).Children()).ToList()[2].Children())).ToList();
			BulkPrice price_buff = default(BulkPrice);
			for (int j = 0; j < result.Count; j++)
			{
				price_buff.str = ((object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[3].Children()).ToList()[0]).ToString();
				price_buff.amt = (int)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0];
				price_buff.worth = (double)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0] / (double)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0];
				price_buff.minHave = (int)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0];
				price_buff.minWant = (int)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0];
				price_buff.acc = ((object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)((IEnumerable<JToken>)(object)result[j].Children()).ToList()[0].Children()).ToList()[2].Children()).ToList()[0].Children()).ToList()[1].Children()).ToList()[0].Children()).ToList()[0].Children()).ToList()[0]).ToString();
				Console.WriteLine(price_buff.amt + " " + price_buff.worth + " " + price_buff.minHave + " " + price_buff.minWant + " " + price_buff.acc + " " + price_buff.str);
				GlobalLog.Debug("[Bulk]" + price_buff.amt + " " + price_buff.worth + " " + price_buff.minHave + " " + price_buff.minWant + " " + price_buff.acc + " " + price_buff.str);
				prices.Add(price_buff);
				price_buff = default(BulkPrice);
			}
			GlobalLog.Debug("[Bulk] 8");
			foreach (BulkPrice i in prices)
			{
				GlobalLog.Debug($"[Bulk] {i.acc}, {i.amt}");
			}
			return prices;
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			GlobalLog.Error($"[Bulk] {ex}");
			return prices;
		}
	}

	public static Item4Search GetTestItem()
	{
		return default(Item4Search);
	}

	public static string GetUrl(PostResponse resp)
	{
		string text = "https://www.pathofexile.com/api/trade/fetch/";
		int num = resp.total;
		if (num != 0)
		{
			if (num == 1)
			{
				text = text + resp.result[0] + "?query=" + resp.id;
			}
			if (num == 2)
			{
				text = text + resp.result[0] + "," + resp.result[1] + "?query=" + resp.id;
			}
			else
			{
				if (num > 10)
				{
					num = 10;
				}
				for (int i = 0; i < num; i++)
				{
					text = ((i != num - 1) ? (text + resp.result[i] + ",") : (text + resp.result[i] + "?query=" + resp.id));
				}
			}
			return text;
		}
		return "0";
	}

	public static string GetUrlBulk(PostResponse resp, int loop)
	{
		try
		{
			string text = "https://www.pathofexile.com/api/trade/fetch/";
			int num = loop * 20;
			int num2;
			if (resp.total - num >= 20)
			{
				num2 = 20;
			}
			else
			{
				num2 = resp.total - loop * 20;
				if (num2 < 0)
				{
					num2 = 0;
				}
			}
			switch (num2)
			{
			case 0:
				return "0";
			case 1:
				text = text + resp.result[num] + "?query=" + resp.id + "&exchange";
				break;
			}
			if (num2 != 2)
			{
				for (int i = 0; i < num2; i++)
				{
					text = ((i == num2 - 1) ? (text + resp.result[num + i] + "?query=" + resp.id + "&exchange") : (text + resp.result[num + i] + ","));
				}
			}
			else
			{
				text = text + resp.result[num] + "," + resp.result[num + 1] + "?query=" + resp.id + "&exchange";
			}
			return text;
		}
		catch
		{
			return "0";
		}
	}

	private static string JGen(Item4Search item)
	{
		//IL_1837: Unknown result type (might be due to invalid IL or missing references)
		//IL_183c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1848: Expected O, but got Unknown
		dynamic val = JsonConvert.DeserializeObject(string_1);
		Root root = new Root();
		Query query = new Query();
		Sort sort = new Sort();
		Status status = new Status
		{
			option = "online"
		};
		query.status = status;
		if (!string.IsNullOrEmpty(item.name))
		{
			query.name = item.name;
		}
		if (!string.IsNullOrEmpty(item.type))
		{
			query.type = item.type;
		}
		Stat stat = new Stat
		{
			type = ((!string.IsNullOrEmpty(item.mods_type)) ? item.mods_type.ToLower() : "and")
		};
		if (item.mods_type_value_min != 0 || item.mods_type_value_max != 0)
		{
			Value value = new Value
			{
				min = item.mods_type_value_min,
				max = item.mods_type_value_max
			};
			stat.value = value;
		}
		stat.filters = new List<Filters>();
		if (item.mods != null)
		{
			for (int i = 0; i < item.mods.Count; i++)
			{
				GlobalLog.Debug("[Parser] Mod: " + item.mods[i].name);
				Filters filters = new Filters();
				string key = item.mods[i].type.ToLower();
				for (int j = 0; j < val.result[ModType[key]].entries.Count; j++)
				{
					string a = val.result[ModType[key]].entries[j].text;
					if (!string.Equals(a, item.mods[i].name, StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					filters.id = val.result[ModType[key]].entries[j].id;
					if (item.mods[i].min != "0" || item.mods[i].max != "0" || !string.IsNullOrEmpty(item.mods[i].value))
					{
						Value value2 = new Value();
						if (item.mods[i].min != "0" && !string.IsNullOrEmpty(item.mods[i].min))
						{
							value2.min = int.Parse(item.mods[i].min);
						}
						if (item.mods[i].max != "0" && !string.IsNullOrEmpty(item.mods[i].max))
						{
							value2.max = int.Parse(item.mods[i].max);
						}
						if (!string.IsNullOrEmpty(item.mods[i].value))
						{
							for (int k = 0; k < val.result[ModType[key]].entries[j].option.options.Count; k++)
							{
								string text = val.result[ModType[key]].entries[j].option.options[k].text;
								if (text.ToLower() == item.mods[i].value.ToLower())
								{
									string s = val.result[ModType[key]].entries[j].option.options[k].id;
									value2.option = int.Parse(s);
									break;
								}
							}
						}
						filters.value = value2;
					}
					filters.disabled = false;
					stat.filters.Add(filters);
				}
			}
		}
		query.stats = new List<Stat> { stat };
		Console.WriteLine("\nFilters:");
		Filters filters2 = new Filters();
		if (item.category != null || item.rarity != null)
		{
			Console.WriteLine("- TypeFilters:");
			TypeFilters typeFilters = new TypeFilters();
			Filters filters3 = new Filters();
			if (item.category != null)
			{
				GlobalLog.Debug("pidor [" + item.category + "]");
				Console.WriteLine("- - Category");
				Category category = new Category
				{
					option = ConvertCategory(item.category)
				};
				filters3.category = category;
			}
			if (item.rarity != null)
			{
				Console.WriteLine("- - Rarity");
				Rarity rarity = new Rarity
				{
					option = ConvertRarity(item.rarity)
				};
				filters3.rarity = rarity;
			}
			typeFilters.filters = filters3;
			typeFilters.disabled = false;
			filters2.type_filters = typeFilters;
		}
		if ((item.sockets != null && (item.sockets[0] != 0 || item.sockets[1] != 0 || item.sockets[2] != 0 || item.sockets[3] != 0)) || (item.links != null && (item.links[0] != 0 || item.links[1] != 0 || item.links[2] != 0 || item.links[3] != 0)) || item.links_min != 0)
		{
			Console.WriteLine("- SocketFilters:");
			SocketFilters socketFilters = new SocketFilters();
			Filters filters4 = new Filters();
			if (item.sockets != null && (item.sockets[0] != 0 || item.sockets[1] != 0 || item.sockets[2] != 0 || item.sockets[3] != 0))
			{
				Console.WriteLine("- - Sockets");
				Sockets sockets = new Sockets();
				if (item.sockets[0] != 0)
				{
					sockets.r = item.sockets[0];
				}
				if (item.sockets[1] != 0)
				{
					sockets.g = item.sockets[1];
				}
				if (item.sockets[2] != 0)
				{
					sockets.b = item.sockets[2];
				}
				if (item.sockets[3] != 0)
				{
					sockets.w = item.sockets[3];
				}
				filters4.sockets = sockets;
			}
			if ((item.links != null && (item.links[0] != 0 || item.links[1] != 0 || item.links[2] != 0 || item.links[3] != 0)) || item.links_min != 0)
			{
				Console.WriteLine("- - Links");
				Links links = new Links();
				if (item.links != null)
				{
					if (item.links[0] != 0)
					{
						links.r = item.links[0];
					}
					if (item.links[1] != 0)
					{
						links.g = item.links[1];
					}
					if (item.links[2] != 0)
					{
						links.b = item.links[2];
					}
					if (item.links[3] != 0)
					{
						links.w = item.links[3];
					}
				}
				if (item.links_min != 0)
				{
					links.min = item.links_min;
				}
				filters4.links = links;
			}
			socketFilters.filters = filters4;
			filters2.socket_filters = socketFilters;
		}
		if (item.map_tier.ToString() != "0" || !string.IsNullOrEmpty(item.map_region) || item.blighted_map.HasValue)
		{
			Console.WriteLine("- MapFilters:");
			MapFilters mapFilters = new MapFilters();
			Filters filters5 = new Filters();
			if (item.map_tier.ToString() != "0")
			{
				Console.WriteLine("- - Maptier");
				MapTier map_tier = new MapTier
				{
					min = item.map_tier
				};
				filters5.map_tier = map_tier;
			}
			if (!string.IsNullOrEmpty(item.map_region))
			{
				Console.WriteLine("- - Mapregion");
				MapRegion map_region = new MapRegion
				{
					option = ConvertMapRegion(item.map_region)
				};
				filters5.map_region = map_region;
			}
			if (item.blighted_map.HasValue)
			{
				Console.WriteLine("- - Blightedmap");
				MapBlighted map_blighted = new MapBlighted
				{
					option = item.blighted_map.ToString()
				};
				filters5.map_blighted = map_blighted;
			}
			mapFilters.filters = filters5;
			filters2.map_filters = mapFilters;
		}
		Console.WriteLine("- MiscFilters:");
		MiscFilters miscFilters = new MiscFilters();
		Filters filters6 = new Filters();
		if (item.item_lvl.ToString() != "0")
		{
			Console.WriteLine("- - Ilvl");
			Ilvl ilvl = new Ilvl
			{
				min = item.item_lvl
			};
			filters6.ilvl = ilvl;
		}
		if (item.quality.ToString() != "0")
		{
			Console.WriteLine("- - Quality");
			Quality quality = new Quality
			{
				min = item.quality
			};
			filters6.quality = quality;
		}
		if (item.gem_lvl.ToString() != "0")
		{
			Console.WriteLine("- - Gemlvl");
			GemLevel gem_level = new GemLevel
			{
				min = item.gem_lvl
			};
			filters6.gem_level = gem_level;
		}
		if (item.corrupted.HasValue)
		{
			Console.WriteLine("- - Corrupted");
			Corrupted corrupted = new Corrupted
			{
				option = item.corrupted.ToString()
			};
			filters6.corrupted = corrupted;
		}
		if (item.split.HasValue)
		{
			Console.WriteLine("- - Split");
			Split split = new Split
			{
				option = item.split.ToString()
			};
			filters6.split = split;
		}
		Console.WriteLine("- - ScourgeTier");
		ScourgeTier scourge_tier = new ScourgeTier
		{
			max = item.scourge_tier
		};
		filters6.scourge_tier = scourge_tier;
		miscFilters.filters = filters6;
		filters2.misc_filters = miscFilters;
		if (item.buyout_price_min != 0 || item.buyout_price_max != 0)
		{
			Console.WriteLine("- TradeFilters:");
			TradeFilters tradeFilters = new TradeFilters();
			Filters filters7 = new Filters();
			BuyoutPrice buyoutPrice = new BuyoutPrice();
			if (item.buyout_price_min != 0)
			{
				Console.WriteLine("- - Buyout Price Min");
				buyoutPrice.min = item.buyout_price_min;
			}
			if (item.buyout_price_max != 0)
			{
				Console.WriteLine("- - Buyout Price Max");
				buyoutPrice.max = item.buyout_price_max;
			}
			filters7.price = buyoutPrice;
			tradeFilters.filters = filters7;
			filters2.trade_filters = tradeFilters;
		}
		query.filters = filters2;
		sort.price = "asc";
		root.query = query;
		root.sort = sort;
		return JsonConvert.SerializeObject((object)root, (Formatting)0, new JsonSerializerSettings
		{
			NullValueHandling = (NullValueHandling)1
		});
	}

	private static string JGenBulk(string WeHave, string WeWant, int minimum)
	{
		RootBulk rootBulk = new RootBulk();
		Exchange exchange = new Exchange();
		Status status = new Status
		{
			option = "online"
		};
		exchange.status = status;
		exchange.have = new List<string>();
		exchange.have.Add(ConvertCurrency(WeHave));
		exchange.want = new List<string>();
		exchange.want.Add(ConvertCurrency(WeWant));
		exchange.minimum = minimum;
		rootBulk.engine = "new";
		rootBulk.query = exchange;
		return JsonConvert.SerializeObject((object)rootBulk);
	}

	private static string ConvertCategory(string cat)
	{
		cat = cat.ToLower();
		return cat switch
		{
			"divination card" => "card", 
			"wand" => "weapon.wand", 
			"trinket" => "accessory.trinket", 
			"any weapon" => "weapon", 
			"bow" => "weapon.bow", 
			"two-handed melee weapon" => "weapon.twomelee", 
			"heist target" => "currency.heistobjective", 
			"amulet" => "accessory.amulet", 
			"one-handed sword" => "weapon.onesword", 
			"sceptre" => "weapon.sceptre", 
			"fishing rod" => "weapon.rod", 
			"fossil" => "currency.fossil", 
			"any expedition logbook" => "logbook", 
			"base jewel" => "jewel.base", 
			"any currency" => "currency", 
			"scarab" => "map.scarab", 
			"heist tool" => "heistequipment.heisttool", 
			"any gem" => "gem", 
			"leaguestone" => "leaguestone", 
			"heist contract" => "heistmission.contract", 
			"heist blueprint" => "heistmission.blueprint", 
			"maven's invitation" => "map.invitation", 
			"awakened support gem" => "gem.supportgemplus", 
			"body armour" => "armour.chest", 
			"two-handed axe" => "weapon.twoaxe", 
			"shield" => "armour.shield", 
			"captured beast" => "monster.beast", 
			"metamorph sample" => "monster.sample", 
			"any armour" => "armour", 
			"claw" => "weapon.claw", 
			"one-handed weapon" => "weapon.one", 
			"any jewel" => "jewel", 
			"abyss jewel" => "jewel.abyss", 
			"rune dagger" => "weapon.runedagger", 
			"belt" => "accessory.belt", 
			"helmet" => "armour.helmet", 
			"any accessory" => "accessory", 
			"watchstone" => "watchstone", 
			"warstaff" => "weapon.warstaff", 
			"heist brooch" => "heistequipment.heistreward", 
			"two-handed mace" => "weapon.twomace", 
			"two-handed sword" => "weapon.twosword", 
			"heist cloak" => "heistequipment.heistutility", 
			"one-handed melee weapon" => "weapon.onemelee", 
			"prophecy" => "prophecy", 
			"base staff" => "weapon.basestaff", 
			"quiver" => "armour.quiver", 
			"flask" => "flask", 
			"gloves" => "armour.gloves", 
			"incubator" => "currency.incubator", 
			"any heist mission" => "heistmission", 
			"one-handed mace" => "weapon.onemace", 
			"cluster jewel" => "jewel.cluster", 
			"support gem" => "gem.supportgem", 
			"any staff" => "weapon.staff", 
			"map fragment" => "map.fragment", 
			"one-handed axe" => "weapon.oneaxe", 
			"resonator" => "currency.resonator", 
			"map" => "map", 
			"skill gem" => "gem.activegem", 
			"heist gear" => "heistequipment.heistweapon", 
			"any dagger" => "weapon.dagger", 
			"any heist equipment" => "heistequipment", 
			"unique fragment" => "currency.piece", 
			"boots" => "armour.boots", 
			"ring" => "accessory.ring", 
			"base dagger" => "weapon.basedagger", 
			_ => "", 
		};
	}

	private static string ConvertCurrency(string cur)
	{
		dynamic val = JsonConvert.DeserializeObject(string_0);
		if (cur.Split(' ')[0].ToLower() == "tier")
		{
			int num = int.Parse(cur.Split(' ')[1]) + 14;
			string text = "";
			for (int num2 = 2; num2 < cur.Split(' ').Length; num2++)
			{
				text = text + " " + cur.Split(' ')[num2];
				text = text.Trim();
			}
			for (int i = 0; i < val.result[num].entries.Count; i++)
			{
				string text2 = val.result[num].entries[i].text;
				if (text2.ToLower() == text.ToLower())
				{
					return val.result[num].entries[i].id;
				}
			}
		}
		else
		{
			for (int j = 0; j < val.result.Count; j++)
			{
				for (int k = 0; k < val.result[j].entries.Count; k++)
				{
					string text3 = val.result[j].entries[k].text;
					if (text3.ToLower() == cur.ToLower())
					{
						return val.result[j].entries[k].id;
					}
				}
			}
		}
		return null;
	}

	private static string ConvertMapRegion(string mr)
	{
		mr = mr.ToLower();
		return mr switch
		{
			"valdo's rest" => "ibr", 
			"haewark hamlet" => "otl", 
			"lex proxima" => "itr", 
			"lex ejoris" => "otr", 
			"new vastir" => "obl", 
			"glennach cairns" => "ibl", 
			"lira arthain" => "obr", 
			"tirn's end" => "itl", 
			_ => "", 
		};
	}

	private static string ConvertRarity(string rar)
	{
		rar = rar.ToLower();
		string text = rar;
		string text2 = text;
		if (text2 == "unique (relic)")
		{
			return "uniquefoil";
		}
		if (text2 == "any non-unique")
		{
			return "nonunique";
		}
		return rar;
	}

	static ParserClass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		httpClient_1 = new HttpClient();
		vuiCdoaNr5 = TraderPluginSettings.Instance.LegaueName;
		cookieContainer_0 = new CookieContainer();
		ModType = new Dictionary<string, int>
		{
			["pseudo"] = 0,
			["explicit"] = 1,
			["implicit"] = 2,
			["fractured"] = 3,
			["enchant"] = 4,
			["crafted"] = 5,
			["veiled"] = 6,
			["monster"] = 7,
			["delve"] = 8,
			["ultimatum"] = 9,
			[""] = 0
		};
	}
}
