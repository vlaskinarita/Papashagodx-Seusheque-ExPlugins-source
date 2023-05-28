using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ExPlugins.BulkTraderEx.Classes;
using ExPlugins.EXtensions;

namespace ExPlugins.BulkTraderEx.Helpers;

public static class CurrencyHelper
{
	public class Currency
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private string string_1;

		public string Name
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

		public int Number
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

		public int Stack
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

		public string Id
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

		public Currency(string name, int stack)
		{
			Name = name;
			Stack = stack;
		}

		public Currency(string name, string id, int stack)
		{
			Name = name;
			Stack = stack;
			Id = id;
		}
	}

	public static HashSet<Currency> Currencies;

	public static string GetCurrencyExactNameFromInt(int num)
	{
		Currency currency = Currencies.FirstOrDefault((Currency c) => c.Number == num);
		if (currency == null)
		{
			GlobalLog.Warn($"[CurrencyExchangePlugin-GetCurrencyExactNameFromInt] Unable to find Currency info for: {num}.");
			return "ERROR";
		}
		return currency.Name;
	}

	public static int GetCurrencyMaxStackFromName(string name)
	{
		if (!name.ContainsIgnorecase("essence"))
		{
			if (!name.ContainsIgnorecase("scarab"))
			{
				Currency currency = Currencies.FirstOrDefault((Currency c) => c.Name == name);
				if (currency == null)
				{
					GlobalLog.Warn("[CurrencyExchangePlugin-GetCurrencyMaxStackFromPartialString] Unable to find Currency info for: " + name + ".");
					return 0;
				}
				return currency.Stack;
			}
			return 10;
		}
		return 9;
	}

	public static List<TradeCurrency> ParseOfficialRequest(List<WebHelper.BulkPrice> bulkList, string haveName, string wantName)
	{
		List<TradeCurrency> list = new List<TradeCurrency>();
		foreach (WebHelper.BulkPrice bulk in bulkList)
		{
			string text = bulk.Whisper.Split(' ')[0].Replace("@", "");
			if (!BulkTraderEx.BlacklistedSellers.Keys.Contains(text))
			{
				list.Add(new TradeCurrency(text, bulk.Account, bulk.Stock, wantName, bulk.MinSell, haveName, bulk.MinBuy, bulk.Whisper, bulk.WhisperToken, bulk));
			}
		}
		return list;
	}

	static CurrencyHelper()
	{
		Currencies = new HashSet<Currency>
		{
			new Currency("Ancient Orb", 20),
			new Currency("Armourer's Scrap", 40),
			new Currency("Awakened Sextant", 10),
			new Currency("Awakener's Orb", 10),
			new Currency("Blacksmith's Whetstone", 20),
			new Currency("Blessed Orb", 20),
			new Currency("Blessing of Chayula", 10),
			new Currency("Blessing of Esh", 10),
			new Currency("Blessing of Tul", 10),
			new Currency("Blessing of Uul-Netol", 10),
			new Currency("Blessing of Xoph", 10),
			new Currency("Cartographer's Chisel", 20),
			new Currency("Chaos Orb", 10),
			new Currency("Chromatic Orb", 20),
			new Currency("Crusader's Exalted Orb", 10),
			new Currency("Divine Orb", 10),
			new Currency("Eldritch Chaos Orb", 10),
			new Currency("Eldritch Exalted Orb", 10),
			new Currency("Eldritch Orb of Annulment", 20),
			new Currency("Elevated Sextant", 10),
			new Currency("Engineer's Orb", 20),
			new Currency("Enkindling Orb", 10),
			new Currency("Eternal Orb", 10),
			new Currency("Exalted Orb", 10),
			new Currency("Exalted Shard", 20),
			new Currency("Exceptional Eldritch Ember", 10),
			new Currency("Exceptional Eldritch Ichor", 10),
			new Currency("Fracturing Orb", 20),
			new Currency("Fracturing Shard", 20),
			new Currency("Gemcutter's Prism", 20),
			new Currency("Glassblower's Bauble", 20),
			new Currency("Grand Eldritch Ember", 10),
			new Currency("Grand Eldritch Ichor", 10),
			new Currency("Greater Eldritch Ember", 10),
			new Currency("Greater Eldritch Ichor", 10),
			new Currency("Harbinger's Orb", 20),
			new Currency("Hunter's Exalted Orb", 10),
			new Currency("Instilling Orb", 10),
			new Currency("Jeweller's Orb", 20),
			new Currency("Lesser Eldritch Ember", 10),
			new Currency("Lesser Eldritch Ichor", 10),
			new Currency("Mirror of Kalandra", 10),
			new Currency("Mirror Shard", 20),
			new Currency("Orb of Alchemy", 10),
			new Currency("Orb of Alteration", 20),
			new Currency("Orb of Annulment", 20),
			new Currency("Orb of Augmentation", 30),
			new Currency("Orb of Binding", 20),
			new Currency("Orb of Chance", 20),
			new Currency("Orb of Conflict", 10),
			new Currency("Orb of Dominance", 10),
			new Currency("Orb of Fusing", 20),
			new Currency("Orb of Horizons", 20),
			new Currency("Orb of Regret", 40),
			new Currency("Orb of Scouring", 30),
			new Currency("Orb of Transmutation", 40),
			new Currency("Orb of Unmaking", 40),
			new Currency("Perandus Coin", 5000),
			new Currency("Portal Scroll", 40),
			new Currency("Primal Crystallised Lifeforce", 50000),
			new Currency("Redeemer's Exalted Orb", 10),
			new Currency("Regal Orb", 10),
			new Currency("Sacred Crystallised Lifeforce", 10),
			new Currency("Sacred Orb", 10),
			new Currency("Scroll of Wisdom", 40),
			new Currency("Tainted Armourer's Scrap", 40),
			new Currency("Tainted Blacksmith's Whetstone", 20),
			new Currency("Tainted Chaos Orb", 10),
			new Currency("Tainted Chromatic Orb", 20),
			new Currency("Tainted Divine Teardrop", 10),
			new Currency("Tainted Exalted Orb", 10),
			new Currency("Tainted Jeweller's Orb", 20),
			new Currency("Tainted Mythic Orb", 10),
			new Currency("Tainted Orb of Fusing", 20),
			new Currency("Vaal Orb", 10),
			new Currency("Veiled Chaos Orb", 10),
			new Currency("Vivid Crystallised Lifeforce", 50000),
			new Currency("Warlord's Exalted Orb", 10),
			new Currency("Wild Crystallised Lifeforce", 50000),
			new Currency("Stacked Deck", 10),
			new Currency("Simulacrum", 1),
			new Currency("Rogue's Marker", 50000),
			new Currency("Birth of the Three", 3),
			new Currency("Blessing of God", 3),
			new Currency("Crest of the Elderslayers", -2),
			new Currency("Deadly End", 1),
			new Currency("Dedication to the Goddess", 1),
			new Currency("Divine Vessel", 1),
			new Currency("Facetor's Lens", 1),
			new Currency("Friendship", 3),
			new Currency("Gift to the Goddess", 3),
			new Currency("Ignominious Fate", 3),
			new Currency("Infused Engineer's Orb", 20),
			new Currency("Key to Decay", -2),
			new Currency("Key to the Crucible", -2),
			new Currency("Luck of the Vaal", 1),
			new Currency("Maddening Object", -2),
			new Currency("Mortal Set", -2),
			new Currency("Offering to the Goddess", 1),
			new Currency("Oil Extractor", 10),
			new Currency("Prime Regrading Lens", 20),
			new Currency("Ritual Vessel", 10),
			new Currency("Sacred Blossom", 1),
			new Currency("Sacrifice Set", -2),
			new Currency("Secondary Regrading Lens", 20),
			new Currency("Squandered Prosperity", 5),
			new Currency("Surveyor's Compass", 10),
			new Currency("Tailoring Orb", 20),
			new Currency("Tainted Blessing", 10),
			new Currency("Tempering Orb", 20),
			new Currency("The Devastator", 8),
			new Currency("The Iron Bard", 9),
			new Currency("The Long Watch", 3),
			new Currency("The Maven's Writ", 1),
			new Currency("The Mayor", 5),
			new Currency("The Valley of Steel Boxes", 9),
			new Currency("Tribute to the Goddess", 1),
			new Currency("Victorious Fate", 1),
			new Currency("Will of Chaos", 1)
		};
	}
}
