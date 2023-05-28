using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;
using RestSharp;

namespace ExPlugins.EXtensions;

public class PoeNinjaTracker
{
	internal class Class40<T>
	{
		[CompilerGenerated]
		private List<T> list_0;

		private static object object_0;

		public List<T> Lines
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

		internal static bool smethod_0()
		{
			return object_0 == null;
		}

		internal static object smethod_1()
		{
			return object_0;
		}
	}

	public class CurrencyInformation
	{
		public class ValueInfo
		{
			[CompilerGenerated]
			private int int_0;

			[CompilerGenerated]
			private int int_1;

			[CompilerGenerated]
			private double double_0;

			public int Id
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

			public int Count
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

			public double Value
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
		}

		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private double double_0;

		public string CurrencyTypeName
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

		public double ChaosEquivalent
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

		public override string ToString()
		{
			return $"[{CurrencyTypeName}]:{ChaosEquivalent}c";
		}
	}

	public class GemInformation
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private double double_1;

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

		public string Corrupted
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

		public int GemLevel
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

		public int GemQuality
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

		public double ChaosValue
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

		public double DivineValue
		{
			[CompilerGenerated]
			get
			{
				return double_1;
			}
			[CompilerGenerated]
			set
			{
				double_1 = value;
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(Corrupted))
			{
				Corrupted = null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[" + Name + "]");
			stringBuilder.Append((!(Corrupted == "true")) ? $"[{GemLevel}/{GemQuality}]" : $"[{GemLevel}/{GemQuality}, Corrupted]");
			stringBuilder.Append($":{ChaosValue}c/{DivineValue}div");
			return stringBuilder.ToString();
		}
	}

	public class ClusterInformation
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private string string_2;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private double double_1;

		public string BaseType
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

		public string Name
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

		public string Variant
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

		public int LevelRequired
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

		public double ChaosValue
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

		public double DivineValue
		{
			[CompilerGenerated]
			get
			{
				return double_1;
			}
			[CompilerGenerated]
			set
			{
				double_1 = value;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[" + BaseType.Replace(" Cluster Jewel", "") + " " + Variant + "]:");
			stringBuilder.Append($"[{Name} ({LevelRequired})]");
			stringBuilder.Append($":{ChaosValue}c/{DivineValue}div");
			return stringBuilder.ToString();
		}
	}

	public class ItemInformation
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private double double_1;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private int int_2;

		[CompilerGenerated]
		private int int_3 = 1;

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

		public double ChaosValue
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

		public double DivineValue
		{
			[CompilerGenerated]
			get
			{
				return double_1;
			}
			[CompilerGenerated]
			set
			{
				double_1 = value;
			}
		}

		public int MapTier
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

		public int Links
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

		public int ItemClass
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

		public int StackSize
		{
			[CompilerGenerated]
			get
			{
				return int_3;
			}
			[CompilerGenerated]
			set
			{
				int_3 = value;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Name ?? "");
			if (MapTier != 0)
			{
				stringBuilder.Append($"(T{MapTier})");
			}
			if (Links != 0)
			{
				stringBuilder.Append($"[{Links}L]");
			}
			if (StackSize != 0)
			{
				stringBuilder.Append($" [stackSize:{StackSize}]");
			}
			stringBuilder.Append($":{ChaosValue}c/{DivineValue}div");
			return stringBuilder.ToString();
		}
	}

	public class UniqueItemInformation
	{
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private string string_2;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private double double_1;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private int int_2;

		public string Name
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

		public string BaseType
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

		public string Icon
		{
			get
			{
				return string_0;
			}
			set
			{
				int num = value.LastIndexOf('/');
				string encodedValue = ((num != -1) ? value.Substring(value.LastIndexOf('/')).Replace("/", "").Replace(".png", "") : value);
				encodedValue = WebUtility.UrlDecode(encodedValue);
				string_0 = encodedValue;
			}
		}

		public double ChaosValue
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

		public double DivineValue
		{
			[CompilerGenerated]
			get
			{
				return double_1;
			}
			[CompilerGenerated]
			set
			{
				double_1 = value;
			}
		}

		public int MapTier
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

		public int Links
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

		public int ItemClass
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

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[" + Name + "]");
			if (MapTier != 0)
			{
				stringBuilder.Append($"(T{MapTier})");
			}
			if (Links != 0)
			{
				stringBuilder.Append($"[{Links}L]");
			}
			stringBuilder.Append($":{ChaosValue}c/{DivineValue}div");
			stringBuilder.Append("  (Artwork:" + Icon + ")");
			return stringBuilder.ToString();
		}
	}

	public class PriceBackup
	{
		[CompilerGenerated]
		private DateTime dateTime_0;

		[CompilerGenerated]
		private List<CurrencyInformation> list_0;

		[CompilerGenerated]
		private List<CurrencyInformation> list_1;

		[CompilerGenerated]
		private List<UniqueItemInformation> list_2;

		[CompilerGenerated]
		private List<ItemInformation> list_3;

		[CompilerGenerated]
		private List<ClusterInformation> list_4;

		[CompilerGenerated]
		private List<GemInformation> list_5;

		public DateTime Date
		{
			[CompilerGenerated]
			get
			{
				return dateTime_0;
			}
			[CompilerGenerated]
			set
			{
				dateTime_0 = value;
			}
		}

		public List<CurrencyInformation> Currency
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

		public List<CurrencyInformation> Fragments
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

		public List<UniqueItemInformation> Uniques
		{
			[CompilerGenerated]
			get
			{
				return list_2;
			}
			[CompilerGenerated]
			set
			{
				list_2 = value;
			}
		}

		public List<ItemInformation> Items
		{
			[CompilerGenerated]
			get
			{
				return list_3;
			}
			[CompilerGenerated]
			set
			{
				list_3 = value;
			}
		}

		public List<ClusterInformation> Clusters
		{
			[CompilerGenerated]
			get
			{
				return list_4;
			}
			[CompilerGenerated]
			set
			{
				list_4 = value;
			}
		}

		public List<GemInformation> Gems
		{
			[CompilerGenerated]
			get
			{
				return list_5;
			}
			[CompilerGenerated]
			set
			{
				list_5 = value;
			}
		}
	}

	private static readonly Dictionary<string, double> dictionary_0;

	[CompilerGenerated]
	private static List<ItemInformation> list_0;

	[CompilerGenerated]
	private static List<CurrencyInformation> list_1;

	[CompilerGenerated]
	private static List<CurrencyInformation> list_2;

	[CompilerGenerated]
	private static List<GemInformation> list_3;

	[CompilerGenerated]
	private static List<ClusterInformation> list_4;

	[CompilerGenerated]
	private static List<UniqueItemInformation> list_5;

	public static bool Dumped;

	private static int int_0;

	private static readonly Dictionary<int, string> dictionary_1;

	public static List<ItemInformation> Items
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
		[CompilerGenerated]
		private set
		{
			list_0 = value;
		}
	}

	public static List<CurrencyInformation> Currency
	{
		[CompilerGenerated]
		get
		{
			return list_1;
		}
		[CompilerGenerated]
		private set
		{
			list_1 = value;
		}
	}

	public static List<CurrencyInformation> Fragments
	{
		[CompilerGenerated]
		get
		{
			return list_2;
		}
		[CompilerGenerated]
		private set
		{
			list_2 = value;
		}
	}

	public static List<GemInformation> Gems
	{
		[CompilerGenerated]
		get
		{
			return list_3;
		}
		[CompilerGenerated]
		private set
		{
			list_3 = value;
		}
	}

	public static List<ClusterInformation> Clusters
	{
		[CompilerGenerated]
		get
		{
			return list_4;
		}
		[CompilerGenerated]
		private set
		{
			list_4 = value;
		}
	}

	public static List<UniqueItemInformation> Uniques
	{
		[CompilerGenerated]
		get
		{
			return list_5;
		}
		[CompilerGenerated]
		private set
		{
			list_5 = value;
		}
	}

	public string Name => "PoeNinjaTracker";

	public string Author => "Apoc //Seusheque mod //Alcor75 mod";

	public string Description => "Uses poe.ninja to track the cost of items in chaos and/or exalted orbs.";

	public string Version => "2.0.0.0";

	public static async Task Init(string league = "Standard")
	{
		if (LokiPoe.IsInGame)
		{
			league = LokiPoe.Me.League;
			league = league.Replace("Solo Self-Found", "").Replace("SSF", "").Replace("Hardcore", "")
				.Replace("HC", "")
				.Replace("Ruthless", "")
				.Replace("R ", "")
				.Trim();
			if (string.IsNullOrWhiteSpace(league) || league.Equals("Hardcore"))
			{
				league = "Standard";
			}
		}
		string filename = $"{DateTime.Now:dd-MM-yyyy}_PriceBackup_{league}.json";
		string path = Path.Combine("State/ItemDumps/", filename);
		Directory.CreateDirectory("State/ItemDumps/");
		FileInfo file = new FileInfo(path);
		if (File.Exists(path))
		{
			if (file.LastWriteTime.AddHours(6.0) < DateTime.Now)
			{
				GlobalLog.Error($"[PoeNinjaTracker] Dump {path} is too old {DateTime.Now - file.LastWriteTime:hh\\:mm\\:ss} Time to make a new one.");
				File.Delete(path);
				Dumped = false;
			}
		}
		else
		{
			Dumped = false;
		}
		if (Dumped)
		{
			return;
		}
		GlobalLog.Debug("[PoeNinjaTracker] Loading Prices.");
		if (!LoadFromFile(league))
		{
			await Update(league);
			GlobalLog.Debug("[PoeNinjaTracker] Loading item prices failed.");
			WriteToFile("Uniques", league);
			WriteToFile("Items", league);
			WriteToFile("Currency", league);
			WriteToFile("Gems", league);
			WriteToFile("Clusters", league);
			WriteToFile("Fragments", league);
			WriteToFile("Cards", league);
			WriteToFile("Scarabs", league);
			WriteToFile("FossilsAndResonators", league);
			if (league == "Standard" || league == "Hardcore")
			{
				WriteToFile("Relics", league);
			}
		}
		else
		{
			GlobalLog.Debug("[PoeNinjaTracker] Loading item prices success.");
		}
		Dumped = true;
	}

	public static double LookupCurrencyChaosValueByFullName(string fullName)
	{
		if (!(fullName == "Chaos Orb"))
		{
			if (list_1 != null && list_2 != null && list_0 != null)
			{
				CurrencyInformation currencyInformation = Enumerable.FirstOrDefault(list_1, (CurrencyInformation c) => c.CurrencyTypeName == fullName);
				CurrencyInformation currencyInformation2 = Enumerable.FirstOrDefault(list_2, (CurrencyInformation f) => f.CurrencyTypeName == fullName);
				ItemInformation itemInformation = Enumerable.FirstOrDefault(list_0, (ItemInformation i) => i.Name == fullName);
				double? num = null;
				if (currencyInformation != null)
				{
					num = currencyInformation.ChaosEquivalent;
				}
				else if (currencyInformation2 != null)
				{
					num = currencyInformation2.ChaosEquivalent;
				}
				else if (itemInformation != null)
				{
					num = itemInformation.ChaosValue;
				}
				return num ?? (-1.0);
			}
			throw new InvalidOperationException();
		}
		return 1.0;
	}

	public static async Task<double> PriceCheckOfficial(Item item, List<ModAffix> statsToCheck, bool checkIlvl, bool checkCorrupted)
	{
		List<string> affixList = item.Affixes.Select((ModAffix a) => $"{a.InternalName}[{a.Values.First()}]").ToList();
		string affixString = string.Join("|", affixList).Trim();
		if (dictionary_0.TryGetValue(affixString, out var parsedPrice))
		{
			return (parsedPrice <= -1.0) ? LookupChaosValue(item) : parsedPrice;
		}
		double price = await WebHelper.GetItemPrice(item, statsToCheck, LokiPoe.Me.League, checkIlvl, checkCorrupted);
		dictionary_0.Add(affixString, price);
		return (price > -1.0) ? price : LookupChaosValue(item);
	}

	public static double LookupChaosValue(Item item, bool useMaxLinkEvaluation = false)
	{
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Invalid comparison between Unknown and I4
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Invalid comparison between Unknown and I4
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		List<string> values = item.Affixes.Select((ModAffix a) => $"{a.InternalName}[{a.Values.First()}]").ToList();
		if (dictionary_0.TryGetValue(string.Join("|", values).Trim(), out var value) && value > -1.0)
		{
			return value;
		}
		if (list_0 != null)
		{
			string string_0 = item.FullName;
			if (item.Class == "ExpeditionLogbook")
			{
				int num = 20;
				if (item.IsCorrupted)
				{
					num -= 10;
				}
				num -= 77 - item.ItemLevel;
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => exp.ExpeditionFaction == "Black Scythe Mercenaries"))
				{
					num += 5;
				}
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => exp.ExpeditionFaction == "Knights of the Sun"))
				{
					num += 15;
				}
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => Enumerable.Any(exp.ExpeditionAreaSpecificModsString, (string s) => s.Contains("Medved"))))
				{
					num += 45;
				}
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => Enumerable.Any(exp.ExpeditionAreaSpecificModsString, (string s) => s.Contains("Vorana"))))
				{
					num += 60;
				}
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => Enumerable.Any(exp.ExpeditionAreaSpecificModsString, (string s) => s.Contains("Uhtred"))))
				{
					num += 25;
				}
				if (Enumerable.Any(item.Components.ExpeditionSagaComponent.Expeditions, (ExpeditionWrapper exp) => Enumerable.Any(exp.ExpeditionAreaSpecificModsString, (string s) => s.Contains("Olroth"))))
				{
					num += 400;
				}
				return (num > 0) ? num : 20;
			}
			Dictionary<StatTypeGGG, int> stats = item.Stats;
			string renderArt = item.RenderArt;
			if (string_0 == "Chaos Orb")
			{
				return 1.0;
			}
			float num2 = 1f;
			switch (string_0)
			{
			case "Exalted Shard":
				string_0 = "Exalted Orb";
				num2 = 0.05f;
				break;
			case "Transmutation Shard":
				string_0 = "Orb of Transmutation";
				num2 = 0.05f;
				break;
			case "Regal Shard":
				string_0 = "Regal Orb";
				num2 = 0.05f;
				break;
			case "Indigo Oil":
				string_0 = "Crimson Oil";
				num2 = 1f / 9f;
				break;
			case "Alteration Shard":
				string_0 = "Orb of Alteration";
				num2 = 0.05f;
				break;
			case "Ancient Shard":
				string_0 = "Ancient Orb";
				num2 = 0.05f;
				break;
			case "Chaos Shard":
				string_0 = "Chaos Orb";
				num2 = 0.05f;
				break;
			case "Alchemy Shard":
				string_0 = "Orb of Alchemy";
				num2 = 0.05f;
				break;
			case "Violet Oil":
				string_0 = "Crimson Oil";
				num2 = 1f / 3f;
				break;
			case "Mirror Shard":
				string_0 = "Mirror of Kalandra";
				num2 = 0.05f;
				break;
			case "Amber Oil":
				string_0 = "Crimson Oil";
				num2 = 0.0013717421f;
				break;
			case "Annulment Shard":
				string_0 = "Orb of Annulment";
				num2 = 0.05f;
				break;
			case "Black Oil":
				string_0 = "Opalescent Oil";
				num2 = 1f / 3f;
				break;
			case "Ritual Splinter":
				string_0 = "Ritual Vessel";
				num2 = 0.01f;
				break;
			case "Clear Oil":
				string_0 = "Crimson Oil";
				num2 = 0.00015241579f;
				break;
			case "Teal Oil":
				string_0 = "Crimson Oil";
				num2 = 1f / 81f;
				break;
			case "Engineer's Shard":
				string_0 = "Engineer's Orb";
				num2 = 0.05f;
				break;
			case "Horizon Shard":
				string_0 = "Orb of Horizons";
				num2 = 0.05f;
				break;
			case "Harbinger's Shard":
				string_0 = "Harbinger's Orb";
				num2 = 0.05f;
				break;
			case "Sepia Oil":
				string_0 = "Crimson Oil";
				num2 = 0.00045724737f;
				break;
			case "Azure Oil":
				string_0 = "Crimson Oil";
				num2 = 1f / 27f;
				break;
			case "Binding Shard":
				string_0 = "Orb of Binding";
				num2 = 0.05f;
				break;
			case "Verdant Oil":
				string_0 = "Crimson Oil";
				num2 = 0.004115226f;
				break;
			}
			double? num3 = Enumerable.FirstOrDefault(list_1, (CurrencyInformation c) => c.CurrencyTypeName == string_0)?.ChaosEquivalent;
			if (!num3.HasValue)
			{
				num3 = Enumerable.FirstOrDefault(list_2, (CurrencyInformation f) => f.CurrencyTypeName == string_0)?.ChaosEquivalent;
				if (num3.HasValue)
				{
					return num3.Value;
				}
				int quality = ((item.Quality >= 20) ? item.Quality : 0);
				if ((int)item.Rarity != 4)
				{
					if (!item.Metadata.StartsWith("Metadata/Items/Jewels/JewelPassiveTreeExpansion") || (int)item.Rarity == 3)
					{
						return LookupItemChaosValue(item, useMaxLinkEvaluation, stats, renderArt);
					}
					return LookupClusterBaseChaosValue(item, stats);
				}
				return LookupGemChaosValue(item.FullName, item.SkillGemLevel, quality, item.IsCorrupted, item.SkillGemQualityType);
			}
			return num3.Value * (double)num2;
		}
		throw new InvalidOperationException();
	}

	public static string GetUnidItemFullName(string renderArt)
	{
		int num = renderArt.LastIndexOf('/');
		renderArt = ((num != -1) ? renderArt.Substring(renderArt.LastIndexOf('/')).Replace("/", "").Replace(".dds", "") : renderArt.Replace(".dds", ""));
		UniqueItemInformation uniqueItemInformation = Enumerable.FirstOrDefault(list_5.OrderByDescending((UniqueItemInformation i) => i.ChaosValue), (UniqueItemInformation i) => i.Icon == renderArt);
		return (uniqueItemInformation == null) ? "UNKNOWN" : (uniqueItemInformation.Name.Replace("Replica ", "") ?? "");
	}

	public static int GetStackSize(string itemName)
	{
		if (!itemName.Contains("Set"))
		{
			if (!itemName.Contains("Splinter"))
			{
				if (itemName.Contains("Scroll") && !itemName.ContainsIgnorecase("fragment"))
				{
					return 40;
				}
				if (!itemName.Contains("Recombinator"))
				{
					if (itemName.Contains("Scouting Report"))
					{
						return 20;
					}
					if (!itemName.Contains(" Shard"))
					{
						if (!itemName.Contains("Sacrifice at "))
						{
							if (!itemName.Contains("Mortal "))
							{
								if (itemName.Contains("Incubator"))
								{
									return 10;
								}
								if (itemName.Contains("Catalyst"))
								{
									return 10;
								}
								if (!itemName.Contains("'s Crest"))
								{
									if (!itemName.Contains("Delirium Orb"))
									{
										if (!itemName.Contains("Fragment of "))
										{
											if (!itemName.Contains("Reliquary Key"))
											{
												if (itemName.Contains("Breachstone"))
												{
													return 1;
												}
												if (!itemName.Contains("'s Memory of "))
												{
													if (!itemName.Contains(" Invitation"))
													{
														if (!itemName.Contains(" Lure"))
														{
															if (!itemName.Contains("Timeless") || !itemName.Contains("Emblem"))
															{
																if (itemName.Contains("Map") || itemName.Contains("(Tier"))
																{
																	return 1;
																}
																return Enumerable.FirstOrDefault(list_0, (ItemInformation i) => i.Name == itemName)?.StackSize ?? (-1);
															}
															return 1;
														}
														return 1;
													}
													return 1;
												}
												return 1;
											}
											return 1;
										}
										return 10;
									}
									return 10;
								}
								return 10;
							}
							return 10;
						}
						return 10;
					}
					return 20;
				}
				return 20;
			}
			return 100;
		}
		return -1;
	}

	private static double LookupItemChaosValue(Item item, bool useMaxLinkEvaluation, IReadOnlyDictionary<StatTypeGGG, int> itemStats, string renderArt)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Invalid comparison between Unknown and I4
		string string_0 = item.FullName;
		string string_1 = item.Name;
		Rarity rarity = item.Rarity;
		int int_1 = item.MapTier;
		int num = renderArt.LastIndexOf('/');
		renderArt = ((num != -1) ? renderArt.Substring(item.RenderArt.LastIndexOf('/')).Replace("/", "").Replace(".dds", "") : renderArt.Replace(".dds", ""));
		if (useMaxLinkEvaluation)
		{
			return (from i in Enumerable.Where(list_0, (ItemInformation i) => i.Name == string_0 && i.ItemClass != 9)
				orderby i.Links descending
				select i).FirstOrDefault()?.ChaosValue ?? (-1.0);
		}
		int int_0 = item.MaxLinkCount;
		if (item.MaxLinkCount <= 4)
		{
			int_0 = 0;
		}
		if (item.Class == "Map" && (int)rarity != 3)
		{
			if (itemStats.ContainsKey((StatTypeGGG)13845) && TraderPluginSettings.Instance.AwakenerGuardianMapsPrice > 0)
			{
				return TraderPluginSettings.Instance.AwakenerGuardianMapsPrice;
			}
			if (itemStats.ContainsKey((StatTypeGGG)6548) && TraderPluginSettings.Instance.ElderGuardianMapsPrice > 0)
			{
				return TraderPluginSettings.Instance.ElderGuardianMapsPrice;
			}
			if (itemStats.ContainsKey((StatTypeGGG)6827) && itemStats[(StatTypeGGG)6827] == 1 && TraderPluginSettings.Instance.ShaperGuardianMapsPrice > 0)
			{
				return TraderPluginSettings.Instance.ShaperGuardianMapsPrice;
			}
			if (itemStats.ContainsKey((StatTypeGGG)10342))
			{
				string_0 = (itemStats.ContainsKey((StatTypeGGG)14763) ? ("Blight-ravaged " + item.Name) : ("Blighted " + item.Name));
			}
			else
			{
				string_0 = item.Name;
			}
		}
		if (item.Class == "MiscMapItem")
		{
			string_0 = item.Name;
		}
		double result = -1.0;
		if (string_0.Equals("Voices"))
		{
			List<UniqueItemInformation> list = (from i in Enumerable.Where(list_5, (UniqueItemInformation i) => i.Name == string_0)
				orderby i.ChaosValue descending
				select i).ToList();
			itemStats.TryGetValue((StatTypeGGG)10923, out var value);
			if (list.Any())
			{
				switch (value)
				{
				case 1:
				{
					UniqueItemInformation uniqueItemInformation = list[0];
					if (uniqueItemInformation != null)
					{
						return uniqueItemInformation.ChaosValue;
					}
					break;
				}
				case 3:
				{
					UniqueItemInformation uniqueItemInformation = list[1];
					if (uniqueItemInformation != null)
					{
						return uniqueItemInformation.ChaosValue;
					}
					break;
				}
				case 5:
				{
					UniqueItemInformation uniqueItemInformation = list[2];
					if (uniqueItemInformation != null)
					{
						return uniqueItemInformation.ChaosValue;
					}
					break;
				}
				case 7:
				{
					UniqueItemInformation uniqueItemInformation = list[3];
					if (uniqueItemInformation != null)
					{
						return uniqueItemInformation.ChaosValue;
					}
					break;
				}
				}
				return -1.0;
			}
		}
		if ((int)rarity != 3)
		{
			List<ItemInformation> list2 = Enumerable.Where(list_0, (ItemInformation i) => i.Name == string_0 && i.Links == int_0 && i.MapTier == int_1 && i.ItemClass != 9).ToList();
			if (string_0.Contains("Essence"))
			{
				list2 = Enumerable.Where(list_0, (ItemInformation i) => i.Name == string_0).ToList();
			}
			ItemInformation itemInformation = ((list2.Count <= 1 || !itemStats.ContainsKey((StatTypeGGG)14763)) ? list2.FirstOrDefault() : list2.OrderBy((ItemInformation i) => i.ChaosValue).FirstOrDefault());
			if (itemInformation != null)
			{
				result = itemInformation.ChaosValue;
			}
			return result;
		}
		List<UniqueItemInformation> source = Enumerable.Where(list_5, (UniqueItemInformation i) => i.Links == int_0 && string_1 == i.BaseType).ToList();
		UniqueItemInformation uniqueItemInformation2 = (item.IsIdentified ? Enumerable.FirstOrDefault(source, (UniqueItemInformation i) => i.Name == string_0 && i.ItemClass != 9) : Enumerable.FirstOrDefault(from i in Enumerable.Where(source, (UniqueItemInformation i) => !i.Name.ContainsIgnorecase("replica") && i.ItemClass != 9)
			orderby i.ChaosValue descending
			select i, (UniqueItemInformation i) => i.Icon == renderArt));
		if (uniqueItemInformation2 != null)
		{
			result = uniqueItemInformation2.ChaosValue;
		}
		return result;
	}

	private static double LookupGemChaosValue(string fullname, int level, int quality, bool corrupted, GemQualityType qualityType)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected I4, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		if (list_3 != null)
		{
			string string_1 = (corrupted ? "true" : null);
			string string_0 = fullname;
			int int_1 = 1;
			int int_0 = ((quality >= 20) ? quality : 0);
			if (level >= 20 || string_0.Contains("Enlighten") || string_0.Contains("Empower") || string_0.Contains("Enhance") || string_0.Contains("Awakened"))
			{
				int_1 = level;
			}
			switch ((int)qualityType)
			{
			default:
				throw new ArgumentOutOfRangeException("qualityType", qualityType, "unknown gem quality");
			case 1:
				string_0 = "Anomalous " + fullname;
				break;
			case 2:
				string_0 = "Divergent " + fullname;
				break;
			case 3:
				string_0 = "Phantasmal " + fullname;
				break;
			case 0:
				break;
			}
			GemInformation gemInformation = Enumerable.FirstOrDefault(from i in Enumerable.Where(list_3, (GemInformation i) => int_0 - i.GemQuality >= 0)
				orderby int_0 - i.GemQuality
				select i, (GemInformation i) => i.Name == string_0 && i.GemLevel == int_1 && i.Corrupted == string_1);
			if (gemInformation != null || (int)qualityType != 0 || string_0.Contains("Enlighten") || string_0.Contains("Empower") || string_0.Contains("Enhance") || string_0.Contains("Awakened"))
			{
				return (from i in Enumerable.Where(list_3, (GemInformation i) => i.Name == string_0)
					orderby Math.Abs(int_1 - i.GemLevel), Math.Abs(int_0 - i.GemQuality), i.Corrupted == corrupted.ToString() descending
					select i).FirstOrDefault()?.ChaosValue ?? (-1.0);
			}
			return -1.0;
		}
		throw new InvalidOperationException();
	}

	private static double LookupClusterBaseChaosValue(Item item, IReadOnlyDictionary<StatTypeGGG, int> itemStats)
	{
		if (list_4 == null)
		{
			throw new InvalidOperationException();
		}
		int key = itemStats[(StatTypeGGG)10919];
		string string_0 = dictionary_1[key];
		return Enumerable.FirstOrDefault(list_4, (ClusterInformation c) => c.Name.EqualsIgnorecase(string_0) && c.Variant.EqualsIgnorecase($"{itemStats[(StatTypeGGG)10920]} passives") && c.LevelRequired == item.ItemLevel)?.ChaosValue ?? (from c in Enumerable.Where(list_4, (ClusterInformation c) => c.LevelRequired != 84 && c.Name.EqualsIgnorecase(string_0) && c.Variant.EqualsIgnorecase($"{itemStats[(StatTypeGGG)10920]} passives"))
			orderby Math.Abs(c.LevelRequired - item.ItemLevel)
			select c).FirstOrDefault()?.ChaosValue ?? (-1.0);
	}

	private static async Task Update(string league)
	{
		List<CurrencyInformation> currency;
		List<CurrencyInformation> fragments;
		List<GemInformation> gems;
		List<ClusterInformation> clusters;
		List<ItemInformation> items;
		List<UniqueItemInformation> uniqs;
		while (true)
		{
			string[] urls = new string[19]
			{
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Essence&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=DivinationCard&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueMap&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueJewel&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueFlask&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueWeapon&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueArmour&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=UniqueAccessory&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Map&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=BlightedMap&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=BlightRavagedMap&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Scarab&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=DeliriumOrb&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Incubator&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Fossil&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Resonator&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Oil&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Invitation&language=en",
				"https://poe.ninja/api/data/itemoverview?league=" + league + "&type=Artifact&language=en"
			};
			try
			{
				RestClient client = new RestClient
				{
					UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:90.0) Gecko/20100101 Firefox/90.0",
					Encoding = Encoding.UTF8
				};
				currency = await GetAndDeserialize<CurrencyInformation>(client, "https://poe.ninja/api/data/currencyoverview?league=" + league + "&type=Currency&language=en");
				fragments = await GetAndDeserialize<CurrencyInformation>(client, "https://poe.ninja/api/data/currencyoverview?league=" + league + "&type=Fragment&language=en");
				gems = await GetAndDeserialize<GemInformation>(client, "https://poe.ninja/api/data/itemoverview?league=" + league + "&type=SkillGem&language=en");
				clusters = await GetAndDeserialize<ClusterInformation>(client, "https://poe.ninja/api/data/itemoverview?league=" + league + "&type=ClusterJewel&language=en");
				List<ItemInformation> itemslist = new List<ItemInformation>();
				List<UniqueItemInformation> uniqList = new List<UniqueItemInformation>();
				foreach (string url in Enumerable.Where(urls, (string s) => !s.Contains("Unique")))
				{
					itemslist.AddRange(await GetAndDeserialize<ItemInformation>(client, url));
				}
				items = itemslist;
				foreach (string url2 in Enumerable.Where(urls, (string s) => s.Contains("Unique")))
				{
					uniqList.AddRange(await GetAndDeserialize<UniqueItemInformation>(client, url2));
				}
				uniqs = uniqList;
			}
			catch (Exception)
			{
				currency = new List<CurrencyInformation>();
				fragments = new List<CurrencyInformation>();
				gems = new List<GemInformation>();
				clusters = new List<ClusterInformation>();
				items = new List<ItemInformation>();
				uniqs = new List<UniqueItemInformation>();
			}
			if (currency.Count > 0 && fragments.Count > 0 && gems.Count > 0 && items.Count > 0 && clusters.Count > 0 && uniqs.Count > 0)
			{
				break;
			}
			GlobalLog.Error("[PoeNinjaTracker] Unable to get data from PoeNinjaTracker, this usually indicate the site is down, trying to load last price file ignoring creation date.");
			if (!LoadFromFile(league, ignoreDate: true))
			{
				GlobalLog.Warn("[PoeNinjaTracker] now retrying to dump prices from Poe Ninja.");
				if (int_0 < 10)
				{
					int_0++;
					continue;
				}
				string stopstring = "Unable to get prices from poe.ninja. Backup file is not present. Please copy State\\ItemDumps folder from another bot.";
				GlobalLog.Error("[PoeNinjaTracker] " + stopstring);
				GlobalLog.Error("[PoeNinjaTracker] Stopping the bot now.");
				StopReasonData stopReason = new StopReasonData("poeninja_fail_to_get_prices", stopstring, (object)null);
				BotManager.Stop(stopReason, false);
				await Wait.SleepSafe(1000);
			}
			GlobalLog.Debug("[PoeNinjaTracker] Successfully loaded backup prices.");
			return;
		}
		Currency = currency;
		Items = items;
		Uniques = uniqs;
		Fragments = fragments;
		Clusters = clusters;
		Gems = gems;
		PriceBackup priceBackup = new PriceBackup
		{
			Currency = Currency.OrderByDescending((CurrencyInformation i) => i.ChaosEquivalent).ToList(),
			Uniques = Uniques.OrderByDescending((UniqueItemInformation i) => i.ChaosValue).ToList(),
			Items = Items.OrderByDescending((ItemInformation i) => i.ChaosValue).ToList(),
			Fragments = Fragments.OrderByDescending((CurrencyInformation i) => i.ChaosEquivalent).ToList(),
			Clusters = Clusters.OrderBy((ClusterInformation i) => i.Name).ToList(),
			Gems = Gems.OrderByDescending((GemInformation i) => i.ChaosValue).ToList(),
			Date = DateTime.Now
		};
		uniqs.RemoveAll((UniqueItemInformation i) => i.Name == "Pillar of the Caged God" && i.BaseType == "Long Staff");
		uniqs.RemoveAll((UniqueItemInformation i) => i.Name == "Dusktoe" && i.BaseType == "Leatherscale Boots");
		uniqs.RemoveAll((UniqueItemInformation i) => i.Name == "Infernal Mantle" && i.BaseType == "Occultist's Vestment");
		uniqs.RemoveAll((UniqueItemInformation i) => i.Name == "The Searing Touch" && i.BaseType == "Long Staff");
		GlobalLog.Debug($"[PoeNinjaTracker] CurCount: {currency.Count}");
		GlobalLog.Debug($"[PoeNinjaTracker] FragCount: {fragments.Count}");
		GlobalLog.Debug($"[PoeNinjaTracker] GemCount: {gems.Count}");
		GlobalLog.Debug($"[PoeNinjaTracker] UniquesCount: {uniqs.Count}");
		GlobalLog.Debug($"[PoeNinjaTracker] ItemsCount: {items.Count}");
		WritePriceBackup(priceBackup, league);
	}

	private static async Task<List<T>> GetAndDeserialize<T>(IRestClient c, string urlString)
	{
		Uri url = new Uri(urlString);
		List<T> tempList = new List<T>();
		c.BaseUrl = url;
		Task<IRestResponse> downloadTask = c.ExecuteGetTaskAsync(new RestRequest());
		if (Coroutine.Current != null)
		{
			downloadTask = Coroutine.ExternalTask<IRestResponse>(downloadTask);
		}
		IRestResponse resonse = await downloadTask;
		HttpStatusCode statusCode = resonse.StatusCode;
		if (statusCode != HttpStatusCode.OK)
		{
			GlobalLog.Error($"[PoeNinjaTracker:GetAndDeserialize] {urlString}:[{statusCode}]");
			return tempList;
		}
		GlobalLog.Debug($"[PoeNinjaTracker:GetAndDeserialize] {urlString}:[{statusCode}]");
		Class40<T> jsonParsed = JsonConvert.DeserializeObject<Class40<T>>(resonse.Content);
		if (jsonParsed?.Lines != null)
		{
			tempList.AddRange(jsonParsed.Lines);
			return tempList;
		}
		return tempList;
	}

	public static void WritePriceBackup(PriceBackup prices, string league)
	{
		string contents = JsonConvert.SerializeObject((object)prices, (Formatting)1);
		string path = $"{DateTime.Now:dd-MM-yyyy}_PriceBackup_{league}.json";
		string path2 = Path.Combine("State/ItemDumps/", path);
		File.WriteAllText(path2, contents);
	}

	public static void WriteToFile(string name, string league)
	{
		if (Dumped)
		{
			return;
		}
		string path = $"{DateTime.Now:dd-MM-yyyy}_{name}_{league}.txt";
		string text = Path.Combine("State/ItemDumps/", path);
		Directory.CreateDirectory("State/ItemDumps/");
		using FileStream stream = new FileStream(text, FileMode.Create);
		GlobalLog.Warn("[PoeNinjaTracker] Dumping " + name + " prices to " + text);
		using StreamWriter streamWriter = new StreamWriter(stream);
		switch (name)
		{
		case "Currency":
			if (list_1 == null)
			{
				break;
			}
			{
				foreach (CurrencyInformation item in list_1.OrderByDescending((CurrencyInformation i) => i.ChaosEquivalent))
				{
					streamWriter.WriteLine(item);
					streamWriter.Flush();
				}
				break;
			}
		case "Gems":
			if (list_3 == null)
			{
				break;
			}
			{
				foreach (GemInformation item2 in list_3.OrderByDescending((GemInformation i) => i.ChaosValue))
				{
					streamWriter.WriteLine(item2);
					streamWriter.Flush();
				}
				break;
			}
		case "Relics":
			if (list_0 == null)
			{
				break;
			}
			{
				foreach (UniqueItemInformation item3 in from i in Enumerable.Where(list_5, (UniqueItemInformation i) => i.ItemClass == 9)
					orderby i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine($"[RELIC] {item3}");
					streamWriter.Flush();
				}
				break;
			}
		case "FossilsAndResonators":
			if (list_0 == null)
			{
				break;
			}
			{
				foreach (ItemInformation item4 in from i in Enumerable.Where(list_0, (ItemInformation i) => i.Name.Contains("Fossil") || i.Name.Contains("Resonator"))
					orderby i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine(item4);
					streamWriter.Flush();
				}
				break;
			}
		case "Clusters":
			if (list_4 == null)
			{
				break;
			}
			{
				foreach (ClusterInformation item5 in list_4.OrderByDescending((ClusterInformation i) => i.ChaosValue))
				{
					streamWriter.WriteLine(item5);
					streamWriter.Flush();
				}
				break;
			}
		case "Fragments":
			if (list_2 == null)
			{
				break;
			}
			{
				foreach (CurrencyInformation item6 in list_2.OrderByDescending((CurrencyInformation i) => i.ChaosEquivalent))
				{
					streamWriter.WriteLine(item6);
					streamWriter.Flush();
				}
				break;
			}
		case "Cards":
			if (list_0 == null)
			{
				break;
			}
			{
				foreach (ItemInformation item7 in from i in Enumerable.Where(list_0, (ItemInformation i) => i.ItemClass == 6)
					orderby i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine(item7);
					streamWriter.Flush();
				}
				break;
			}
		case "Items":
			if (list_0 == null)
			{
				break;
			}
			{
				foreach (ItemInformation item8 in from i in Enumerable.Where(list_0, (ItemInformation i) => i.ItemClass != 9 && i.ItemClass != 6 && !i.Name.Contains("Scarab") && !i.Name.Contains("Fossil") && !i.Name.Contains("Resonator"))
					orderby i.Name, i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine(item8);
					streamWriter.Flush();
				}
				break;
			}
		case "Uniques":
			if (list_1 == null)
			{
				break;
			}
			{
				foreach (UniqueItemInformation item9 in from i in Enumerable.Where(list_5, (UniqueItemInformation i) => i.ItemClass != 9)
					orderby i.Name, i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine(item9);
					streamWriter.Flush();
				}
				break;
			}
		case "Scarabs":
			if (list_0 == null)
			{
				break;
			}
			{
				foreach (ItemInformation item10 in from i in Enumerable.Where(list_0, (ItemInformation i) => i.Name.Contains("Scarab"))
					orderby i.ChaosValue descending
					select i)
				{
					streamWriter.WriteLine(item10);
					streamWriter.Flush();
				}
				break;
			}
		}
	}

	private static bool LoadFromFile(string league, bool ignoreDate = false)
	{
		if (!Dumped)
		{
			GlobalLog.Debug("[PoeNinjaTracker] Loading item prices for " + league);
			string text = "State/ItemDumps/";
			string path = $"{DateTime.Now:dd-MM-yyyy}_PriceBackup_{league}.json";
			string path2 = Path.Combine(text, path);
			Directory.CreateDirectory(text);
			if (!File.Exists(path2))
			{
				return false;
			}
			string text2 = File.ReadAllText(path2);
			PriceBackup priceBackup = JsonConvert.DeserializeObject<PriceBackup>(text2);
			if (priceBackup != null)
			{
				if (ignoreDate || !(priceBackup.Date.AddHours(8.0) < DateTime.Now))
				{
					if (priceBackup.Currency.Count > 0)
					{
						if (priceBackup.Fragments.Count <= 0)
						{
							return false;
						}
						if (priceBackup.Gems.Count > 0)
						{
							if (priceBackup.Clusters.Count > 0)
							{
								if (priceBackup.Items.Count <= 0)
								{
									return false;
								}
								if (priceBackup.Uniques.Count <= 0)
								{
									return false;
								}
								Currency = priceBackup.Currency;
								Fragments = priceBackup.Fragments;
								Gems = priceBackup.Gems;
								Clusters = priceBackup.Clusters;
								Items = priceBackup.Items;
								Uniques = priceBackup.Uniques;
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return true;
	}

	static PoeNinjaTracker()
	{
		dictionary_0 = new Dictionary<string, double>();
		dictionary_1 = new Dictionary<int, string>
		{
			{ 1, "Axe Attacks deal 12% increased Damage with Hits and Ailments, Sword Attacks deal 12% increased Damage with Hits and Ailments" },
			{ 2, "Staff Attacks deal 12% increased Damage with Hits and Ailments, Mace or Sceptre Attacks deal 12% increased Damage with Hits and Ailments" },
			{ 3, "Claw Attacks deal 12% increased Damage with Hits and Ailments, Dagger Attacks deal 12% increased Damage with Hits and Ailments" },
			{ 4, "12% increased Damage with Bows, 12% increased Damage Over Time with Bow Skills" },
			{ 5, "Wand Attacks deal 12% increased Damage with Hits and Ailments" },
			{ 6, "12% increased Damage with Two Handed Weapons" },
			{ 7, "12% increased Attack Damage while Dual Wielding" },
			{ 8, "12% increased Attack Damage while holding a Shield" },
			{ 9, "10% increased Attack Damage" },
			{ 10, "10% increased Spell Damage" },
			{ 11, "10% increased Elemental Damage" },
			{ 12, "12% increased Physical Damage" },
			{ 13, "12% increased Fire Damage" },
			{ 14, "12% increased Lightning Damage" },
			{ 15, "12% increased Cold Damage" },
			{ 16, "12% increased Chaos Damage" },
			{ 17, "Minions deal 10% increased Damage" },
			{ 18, "12% increased Burning Damage" },
			{ 19, "12% increased Chaos Damage over Time" },
			{ 20, "12% increased Physical Damage over Time" },
			{ 21, "12% increased Cold Damage over Time" },
			{ 22, "10% increased Damage over Time" },
			{ 23, "10% increased Effect of Non-Damaging Ailments" },
			{ 26, "10% increased Damage while affected by a Herald" },
			{ 27, "Minions deal 10% increased Damage while you are affected by a Herald" },
			{ 28, "Exerted Attacks deal 20% increased Damage" },
			{ 29, "15% increased Critical Strike Chance" },
			{ 30, "Minions have 12% increased maximum Life" },
			{ 31, "10% increased Area Damage" },
			{ 32, "10% increased Projectile Damage" },
			{ 33, "12% increased Trap Damage, 12% increased Mine Damage" },
			{ 34, "12% increased Totem Damage" },
			{ 35, "12% increased Brand Damage" },
			{ 36, "Channelling Skills deal 12% increased Damage" },
			{ 37, "6% increased Flask Effect Duration" },
			{ 38, "10% increased Life Recovery from Flasks, 10% increased Mana Recovery from Flasks" },
			{ 39, "4% increased maximum Life" },
			{ 40, "6% increased maximum Energy Shield" },
			{ 41, "6% increased maximum Mana" },
			{ 42, "15% increased Armour" },
			{ 43, "15% increased Evasion Rating" },
			{ 44, "+1% Chance to Block Attack Damage" },
			{ 45, "1% Chance to Block Spell Damage" },
			{ 46, "+15% to Fire Resistance" },
			{ 47, "+15% to Cold Resistance" },
			{ 48, "+15% to Lightning Resistance" },
			{ 49, "+12% to Chaos Resistance" },
			{ 50, "+2% chance to Suppress Spell Damage" },
			{ 54, "6% increased Mana Reservation Efficiency of Skills" },
			{ 55, "2% increased Effect of your Curses" }
		};
	}
}
