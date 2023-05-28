using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.MapBotEx;
using Newtonsoft.Json;

namespace ExPlugins.EXtensions;

public class ExtensionsSettings : JsonSettings
{
	public class StashingRule
	{
		protected string _tabs;

		[CompilerGenerated]
		private readonly string string_0;

		[CompilerGenerated]
		private List<string> list_0;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public string Tabs
		{
			get
			{
				return _tabs;
			}
			set
			{
				if (!(value == _tabs))
				{
					_tabs = value;
					FillTabList();
				}
			}
		}

		[JsonIgnore]
		public List<string> TabList
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

		public StashingRule(string name, string tabs)
		{
			string_0 = name;
			_tabs = tabs;
			TabList = new List<string>();
		}

		public void FillTabList()
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			try
			{
				Parse(_tabs, TabList);
			}
			catch (Exception ex)
			{
				if (!BotManager.IsRunning)
				{
					MessageBoxes.Error("Parsing error in \"" + _tabs + "\".\n" + ex.Message);
					return;
				}
				GlobalLog.Error("Parsing error in \"" + _tabs + "\".");
				GlobalLog.Error(ex.Message);
				BotManager.Stop(new StopReasonData("parsing_error", "Parsing error in \"" + _tabs + "\".", (object)null), false);
			}
		}

		private static void Parse(string str, ICollection<string> list)
		{
			if (str == string.Empty)
			{
				throw new Exception("Stashing setting cannot be empty.");
			}
			list.Clear();
			string[] array = str.Split(',');
			string[] array2 = array;
			int num = 0;
			while (true)
			{
				if (num < array2.Length)
				{
					string text = array2[num];
					string text2 = text.Trim();
					if (text2 == string.Empty)
					{
						break;
					}
					if (!ParseRange(text2, list))
					{
						list.Add(text2);
					}
					num++;
					continue;
				}
				return;
			}
			throw new Exception("Remove double commas and/or commas from the start/end of the string.");
		}

		private static bool ParseRange(string str, ICollection<string> list)
		{
			string[] array = str.Split('-');
			if (array.Length == 2)
			{
				string text = array[0].Trim();
				string text2 = array[1].Trim();
				if (!int.TryParse(text, out var result))
				{
					throw new Exception("Invalid parameter \"" + text + "\". Only numeric values are supported with range delimiter.");
				}
				if (!int.TryParse(text2, out var result2))
				{
					throw new Exception("Invalid parameter \"" + text2 + "\". Only numeric values are supported with range delimiter.");
				}
				list.Add(text);
				for (int i = result + 1; i < result2; i++)
				{
					list.Add(i.ToString());
				}
				list.Add(text2);
				return true;
			}
			if (array.Length == 1)
			{
				return false;
			}
			throw new Exception("Invalid range string: \"" + str + "\". Supported format: \"X-Y\".");
		}

		internal void CopyContents(StashingRule other)
		{
			_tabs = other._tabs;
		}
	}

	public class TogglableStashingRule : StashingRule
	{
		[CompilerGenerated]
		private bool bool_0;

		public bool Enabled
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

		public TogglableStashingRule(string name, string tabs, bool enabled = false)
			: base(name, tabs)
		{
			Enabled = enabled;
		}

		internal void CopyContents(TogglableStashingRule other)
		{
			_tabs = other._tabs;
			Enabled = other.Enabled;
		}
	}

	public class FullTabInfo
	{
		public readonly List<string> ControlsMetadata = new List<string>();

		public readonly string Name;

		public FullTabInfo(string name, string metadata)
		{
			Name = name;
			if (metadata != null)
			{
				ControlsMetadata.Add(metadata);
			}
		}
	}

	public class ChestEntry
	{
		[CompilerGenerated]
		private readonly string string_0;

		[CompilerGenerated]
		private bool bool_0;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public bool Enabled
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

		public ChestEntry(string name, bool enabled = true)
		{
			string_0 = name;
			Enabled = enabled;
		}
	}

	public static class StashingCategory
	{
		public const string Ignored = "Tabs to ignore";

		public const string Currency = "Currency";

		public const string AltCurrency = "AltCurrency";

		public const string Rare = "Rares";

		public const string Unique = "Uniques";

		public const string Gem = "Gems";

		public const string Card = "Cards";

		public const string Essence = "Essences";

		public const string Oil = "Oils";

		public const string Splinters = "Splinters";

		public const string Delve = "Delve";

		public const string Jewel = "Jewels";

		public const string ExpensiveJewel = "Expensive Jewels [>50c worth]";

		public const string Map = "Maps";

		public const string Simulacrum = "Simulacrums";

		public const string BlightedMaps = "Blighted Maps";

		public const string InfluencedMaps = "Influenced Maps";

		public const string ChargedCompass = "Charged Compass";

		public const string Fragment = "Fragments";

		public const string Other = "Other";
	}

	public static class CurrencyGroup
	{
		public const string Sextant = "Sextants";

		public const string Breach = "Breach";
	}

	public class InventoryCurrency
	{
		public const string DefaultName = "CurrencyName";

		private int int_0;

		private string string_0;

		private int int_1;

		[CompilerGenerated]
		private int int_2;

		public string Name
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = (string.IsNullOrWhiteSpace(value) ? "CurrencyName" : value.Trim());
			}
		}

		public int Row
		{
			get
			{
				return int_1;
			}
			set
			{
				if (value == 0)
				{
					int_1 = ((int_1 != 1) ? 1 : (-1));
				}
				else
				{
					int_1 = value;
				}
			}
		}

		public int Column
		{
			get
			{
				return int_0;
			}
			set
			{
				if (value != 0)
				{
					int_0 = value;
				}
				else
				{
					int_0 = ((int_0 != 1) ? 1 : (-1));
				}
			}
		}

		public int Restock
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

		public InventoryCurrency()
		{
			Name = "CurrencyName";
			Row = -1;
			Column = -1;
			Restock = -1;
		}

		public InventoryCurrency(string name, int row, int column, int restock = -1)
		{
			Name = name;
			Row = row;
			Column = column;
			Restock = restock;
		}
	}

	public class ExchangeSettings
	{
		[CompilerGenerated]
		private bool bool_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		public bool Enabled
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

		public int Min
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

		public int Save
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

		public ExchangeSettings(int min, int save)
		{
			Min = min;
			Save = save;
		}
	}

	private static ExtensionsSettings extensionsSettings_0;

	[JsonIgnore]
	public static readonly Rarity[] Rarities;

	[JsonIgnore]
	public static readonly string[] CurrencyExchangeActs;

	[JsonIgnore]
	private static readonly HashSet<string> hashSet_0;

	[JsonIgnore]
	public static readonly PantheonGod[] PantheonMajorGods;

	[JsonIgnore]
	public static readonly PantheonGod[] PantheonMinorGods;

	[JsonIgnore]
	public static readonly NotificationType[] NotificationTypes;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private int int_0 = 5;

	[CompilerGenerated]
	private int int_1 = 15;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private int int_2 = 2;

	[CompilerGenerated]
	private string string_0 = "5";

	[CompilerGenerated]
	private int int_3 = 50;

	[CompilerGenerated]
	private int int_4 = -1;

	[CompilerGenerated]
	private int int_5 = -1;

	[CompilerGenerated]
	private Rarity rarity_0 = (Rarity)3;

	private bool bool_2;

	private bool bool_3;

	private string string_1;

	private string string_2;

	private bool bool_4;

	[CompilerGenerated]
	private bool bool_5;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private int int_6;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private double double_1;

	[CompilerGenerated]
	private bool bool_7;

	[CompilerGenerated]
	private bool bool_8;

	[CompilerGenerated]
	private bool bool_9;

	[CompilerGenerated]
	private bool bool_10;

	[CompilerGenerated]
	private int int_7;

	[CompilerGenerated]
	private bool bool_11;

	[CompilerGenerated]
	private bool bool_12;

	[CompilerGenerated]
	private string string_3;

	[CompilerGenerated]
	private int int_8;

	[CompilerGenerated]
	private bool bool_13;

	[CompilerGenerated]
	private bool bool_14;

	[CompilerGenerated]
	private bool bool_15;

	[CompilerGenerated]
	private bool bool_16 = true;

	[CompilerGenerated]
	private int int_9 = 3;

	[CompilerGenerated]
	private int int_10 = 8;

	[CompilerGenerated]
	private int Nvkygiylid = 15;

	[CompilerGenerated]
	private int int_11 = 30;

	private bool bool_17;

	private int int_12;

	private int int_13;

	private int int_14;

	private double double_2 = 0.01;

	private double double_3 = 0.01;

	[CompilerGenerated]
	private bool bool_18 = false;

	[CompilerGenerated]
	private double double_4 = 1.0;

	[CompilerGenerated]
	private double double_5 = 1.0;

	[CompilerGenerated]
	private double double_6 = 1.0;

	[CompilerGenerated]
	private double double_7 = 1.0;

	[CompilerGenerated]
	private double MpnyqSyqp3 = 1.0;

	[CompilerGenerated]
	private double double_8 = 1.0;

	private Dictionary<PauseTypeEnum, List<PauseData>> dictionary_0 = new Dictionary<PauseTypeEnum, List<PauseData>>
	{
		{
			PauseTypeEnum.IdPause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.StashPause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.SellPause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.StashFastMovePause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.VendorFastMovePause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.TownMovePause,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.NpcTalkPauseProbability,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.StashPauseProbability,
			new List<PauseData>()
		},
		{
			PauseTypeEnum.WaypointPauseProbability,
			new List<PauseData>()
		}
	};

	private Stopwatch stopwatch_0 = Stopwatch.StartNew();

	[CompilerGenerated]
	private PantheonGod pantheonGod_0;

	[CompilerGenerated]
	private PantheonGod yFayOpmeoB;

	[JsonIgnore]
	public readonly List<FullTabInfo> FullTabsList = new List<FullTabInfo>();

	[CompilerGenerated]
	private ObservableCollection<NotificationEntry> observableCollection_0 = new ObservableCollection<NotificationEntry>();

	[CompilerGenerated]
	private ObservableCollection<FollowerEntry> observableCollection_1 = new ObservableCollection<FollowerEntry>();

	[CompilerGenerated]
	private ObservableCollection<SpecialStashingRule> observableCollection_2 = new ObservableCollection<SpecialStashingRule>();

	[CompilerGenerated]
	private List<StashingRule> list_0 = new List<StashingRule>();

	[CompilerGenerated]
	private List<TogglableStashingRule> list_1 = new List<TogglableStashingRule>();

	[CompilerGenerated]
	private ObservableCollection<InventoryCurrency> observableCollection_3 = new ObservableCollection<InventoryCurrency>();

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_0 = new ExchangeSettings(160, 40);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_1 = new ExchangeSettings(120, 30);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_2 = new ExchangeSettings(100, 20);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_3 = new ExchangeSettings(100, 0);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_4 = new ExchangeSettings(100, 20);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_5 = new ExchangeSettings(60, 30);

	[CompilerGenerated]
	private ExchangeSettings exchangeSettings_6 = new ExchangeSettings(20, 0);

	private readonly List<ChestEntry> list_2 = new List<ChestEntry>();

	private readonly List<ChestEntry> list_3 = new List<ChestEntry>();

	private readonly List<ChestEntry> list_4 = new List<ChestEntry>();

	private readonly List<ChestEntry> list_5 = new List<ChestEntry>();

	public static ExtensionsSettings Instance => extensionsSettings_0 ?? (extensionsSettings_0 = new ExtensionsSettings());

	public bool CardsEnabled
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

	public int MinCardSets
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

	public int MaxCardSets
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

	public bool GcpEnabled
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
		[CompilerGenerated]
		set
		{
			bool_1 = value;
		}
	}

	public int GcpMaxPrice
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

	public string CurrencyExchangeAct
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

	public int ChestOpenRange
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

	public int StrongboxOpenRange
	{
		[CompilerGenerated]
		get
		{
			return int_4;
		}
		[CompilerGenerated]
		set
		{
			int_4 = value;
		}
	}

	public int ShrineOpenRange
	{
		[CompilerGenerated]
		get
		{
			return int_5;
		}
		[CompilerGenerated]
		set
		{
			int_5 = value;
		}
	}

	public Rarity MaxStrongboxRarity
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			rarity_0 = value;
		}
	}

	[DefaultValue(false)]
	public bool PromtMessagesToLog
	{
		get
		{
			return bool_4;
		}
		set
		{
			if (!value.Equals(bool_4))
			{
				bool_4 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => PromtMessagesToLog));
			}
		}
	}

	[DefaultValue(false)]
	public bool UseFollower
	{
		get
		{
			return bool_2;
		}
		set
		{
			if (!value.Equals(bool_2))
			{
				bool_2 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => UseFollower));
			}
		}
	}

	[DefaultValue(false)]
	public bool DiscordNotificationsEnabled
	{
		get
		{
			return bool_3;
		}
		set
		{
			if (!value.Equals(bool_3))
			{
				bool_3 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DiscordNotificationsEnabled));
			}
		}
	}

	[DefaultValue("")]
	public string DiscordWebHookUrl
	{
		get
		{
			return string_1;
		}
		set
		{
			if (!value.Equals(string_1))
			{
				string_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => DiscordWebHookUrl));
			}
		}
	}

	[DefaultValue("")]
	public string DiscordNotificationAddition
	{
		get
		{
			return string_2;
		}
		set
		{
			if (!value.Equals(string_2))
			{
				string_2 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => DiscordNotificationAddition));
			}
		}
	}

	[DefaultValue(false)]
	public bool SkipLootBasedOnValueAndRange
	{
		[CompilerGenerated]
		get
		{
			return bool_5;
		}
		[CompilerGenerated]
		set
		{
			bool_5 = value;
		}
	}

	[DefaultValue(0.3)]
	public double LootSkipPrice
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

	[DefaultValue(45)]
	public int LootSkipRange
	{
		[CompilerGenerated]
		get
		{
			return int_6;
		}
		[CompilerGenerated]
		set
		{
			int_6 = value;
		}
	}

	[DefaultValue(true)]
	public bool StopBotOnVendoringValuableItem
	{
		[CompilerGenerated]
		get
		{
			return bool_6;
		}
		[CompilerGenerated]
		set
		{
			bool_6 = value;
		}
	}

	[DefaultValue(25.0)]
	public double ValuableTreshold
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

	[DefaultValue(true)]
	public bool SkipItemsFilteredIngame
	{
		[CompilerGenerated]
		get
		{
			return bool_7;
		}
		[CompilerGenerated]
		set
		{
			bool_7 = value;
		}
	}

	[DefaultValue(true)]
	public bool UseChatForHideout
	{
		[CompilerGenerated]
		get
		{
			return bool_8;
		}
		[CompilerGenerated]
		set
		{
			bool_8 = value;
		}
	}

	[DefaultValue(true)]
	public bool UnpackStackedDeck
	{
		[CompilerGenerated]
		get
		{
			return bool_9;
		}
		[CompilerGenerated]
		set
		{
			bool_9 = value;
		}
	}

	[DefaultValue(true)]
	public bool ApplyIncubators
	{
		[CompilerGenerated]
		get
		{
			return bool_10;
		}
		[CompilerGenerated]
		set
		{
			bool_10 = value;
		}
	}

	[DefaultValue(4)]
	public int MinInventorySquares
	{
		[CompilerGenerated]
		get
		{
			return int_7;
		}
		[CompilerGenerated]
		set
		{
			int_7 = value;
		}
	}

	[DefaultValue(true)]
	public bool KillPoeOnRareException
	{
		[CompilerGenerated]
		get
		{
			return bool_11;
		}
		[CompilerGenerated]
		set
		{
			bool_11 = value;
		}
	}

	[DefaultValue(false)]
	public bool SwitchBotOnNoMaps
	{
		[CompilerGenerated]
		get
		{
			return bool_12;
		}
		[CompilerGenerated]
		set
		{
			bool_12 = value;
		}
	}

	[DefaultValue("HideoutLayouts")]
	public string HideoutTemplateDir
	{
		[CompilerGenerated]
		get
		{
			return string_3;
		}
		[CompilerGenerated]
		set
		{
			string_3 = value;
		}
	}

	[DefaultValue(0)]
	public int HideoutTemplateAmount
	{
		[CompilerGenerated]
		get
		{
			return int_8;
		}
		[CompilerGenerated]
		set
		{
			int_8 = value;
		}
	}

	[DefaultValue(true)]
	public bool IgnoreHidoutAreaTransitions
	{
		[CompilerGenerated]
		get
		{
			return bool_13;
		}
		[CompilerGenerated]
		set
		{
			bool_13 = value;
		}
	}

	[DefaultValue(false)]
	[JsonIgnore]
	public bool ForceKeyboardSwitch
	{
		[CompilerGenerated]
		get
		{
			return bool_14;
		}
		[CompilerGenerated]
		set
		{
			bool_14 = value;
		}
	}

	[JsonIgnore]
	[DefaultValue(false)]
	public bool ForceWaypointHideout
	{
		[CompilerGenerated]
		get
		{
			return bool_15;
		}
		[CompilerGenerated]
		set
		{
			bool_15 = value;
		}
	}

	public bool StuckDetectionEnabled
	{
		[CompilerGenerated]
		get
		{
			return bool_16;
		}
		[CompilerGenerated]
		set
		{
			bool_16 = value;
		}
	}

	public int MaxStucksPerInstance
	{
		[CompilerGenerated]
		get
		{
			return int_9;
		}
		[CompilerGenerated]
		set
		{
			int_9 = value;
		}
	}

	public int MaxStuckCountSmall
	{
		[CompilerGenerated]
		get
		{
			return int_10;
		}
		[CompilerGenerated]
		set
		{
			int_10 = value;
		}
	}

	public int MaxStuckCountMedium
	{
		[CompilerGenerated]
		get
		{
			return Nvkygiylid;
		}
		[CompilerGenerated]
		set
		{
			Nvkygiylid = value;
		}
	}

	public int MaxStuckCountLong
	{
		[CompilerGenerated]
		get
		{
			return int_11;
		}
		[CompilerGenerated]
		set
		{
			int_11 = value;
		}
	}

	[DefaultValue(false)]
	public bool BreaksEnabled
	{
		get
		{
			return bool_17;
		}
		set
		{
			bool_17 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => BreaksEnabled));
		}
	}

	[DefaultValue(60)]
	public int BreakEveryMinutes
	{
		get
		{
			return int_12;
		}
		set
		{
			int_12 = value;
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => BreakEveryMinutes));
		}
	}

	[DefaultValue(10)]
	public int MinBreak
	{
		get
		{
			return int_13;
		}
		set
		{
			int_13 = value;
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => MinBreak));
		}
	}

	[DefaultValue(60)]
	public int MaxBreak
	{
		get
		{
			return int_14;
		}
		set
		{
			int_14 = value;
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => MaxBreak));
		}
	}

	[JsonIgnore]
	public double GenericPauseDynamicFactor
	{
		get
		{
			return double_2;
		}
		set
		{
			double_2 = value;
			((NotificationObject)this).NotifyPropertyChanged<double>((Expression<Func<double>>)(() => GenericPauseDynamicFactor));
		}
	}

	[JsonIgnore]
	public double TownPauseDynamicFactor
	{
		get
		{
			return double_3;
		}
		set
		{
			double_3 = value;
			((NotificationObject)this).NotifyPropertyChanged<double>((Expression<Func<double>>)(() => TownPauseDynamicFactor));
		}
	}

	public bool LoginPauseAfterError
	{
		[CompilerGenerated]
		get
		{
			return bool_18;
		}
		[CompilerGenerated]
		set
		{
			bool_18 = value;
		}
	}

	[JsonIgnore]
	public bool TownTalk => false;

	[JsonIgnore]
	public bool HumanizerNew => true;

	public double IdPauseFactor
	{
		[CompilerGenerated]
		get
		{
			return double_4;
		}
		[CompilerGenerated]
		set
		{
			double_4 = value;
		}
	}

	public double StashPauseFactor
	{
		[CompilerGenerated]
		get
		{
			return double_5;
		}
		[CompilerGenerated]
		set
		{
			double_5 = value;
		}
	}

	public double SellPauseFactor
	{
		[CompilerGenerated]
		get
		{
			return double_6;
		}
		[CompilerGenerated]
		set
		{
			double_6 = value;
		}
	}

	public double StashFastMovePauseFactor
	{
		[CompilerGenerated]
		get
		{
			return double_7;
		}
		[CompilerGenerated]
		set
		{
			double_7 = value;
		}
	}

	public double VendorFastMovePauseFactor
	{
		[CompilerGenerated]
		get
		{
			return MpnyqSyqp3;
		}
		[CompilerGenerated]
		set
		{
			MpnyqSyqp3 = value;
		}
	}

	public double TownMovePauseFactor
	{
		[CompilerGenerated]
		get
		{
			return double_8;
		}
		[CompilerGenerated]
		set
		{
			double_8 = value;
		}
	}

	[JsonIgnore]
	public Dictionary<PauseTypeEnum, List<PauseData>> PauseDataCollection
	{
		get
		{
			return dictionary_0;
		}
		set
		{
			dictionary_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<Dictionary<PauseTypeEnum, List<PauseData>>>((Expression<Func<Dictionary<PauseTypeEnum, List<PauseData>>>>)(() => PauseDataCollection));
		}
	}

	[JsonIgnore]
	public Stopwatch TotalRuntime
	{
		get
		{
			return stopwatch_0;
		}
		set
		{
			stopwatch_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<Stopwatch>((Expression<Func<Stopwatch>>)(() => TotalRuntime));
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public PantheonGod PantheonMajorGod
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return pantheonGod_0;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			pantheonGod_0 = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public PantheonGod PantheonMinorGod
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return yFayOpmeoB;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			yFayOpmeoB = value;
		}
	}

	public ObservableCollection<NotificationEntry> NotificationsList
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_0;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_0 = value;
		}
	}

	public ObservableCollection<FollowerEntry> Followers
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_1;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_1 = value;
		}
	}

	public ObservableCollection<SpecialStashingRule> SpecificStashingByName
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_2;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_2 = value;
		}
	}

	public List<StashingRule> GeneralStashingRules
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

	public List<TogglableStashingRule> CurrencyStashingRules
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

	public ObservableCollection<InventoryCurrency> InventoryCurrencies
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_3;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_3 = value;
		}
	}

	public ExchangeSettings TransExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_0;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_0 = value;
		}
	}

	public ExchangeSettings AugsExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_1;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_1 = value;
		}
	}

	public ExchangeSettings AltsExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_2;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_2 = value;
		}
	}

	public ExchangeSettings JewsExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_3;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_3 = value;
		}
	}

	public ExchangeSettings ChancesExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_4;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_4 = value;
		}
	}

	public ExchangeSettings ScoursExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_5;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_5 = value;
		}
	}

	public ExchangeSettings RegretExchange
	{
		[CompilerGenerated]
		get
		{
			return exchangeSettings_6;
		}
		[CompilerGenerated]
		set
		{
			exchangeSettings_6 = value;
		}
	}

	public List<ChestEntry> Chests => list_2;

	public List<ChestEntry> Strongboxes => list_4;

	public List<ChestEntry> Shrines => list_5;

	private ExtensionsSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"EXtensions.json"
		}))
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		NotificationsList = new ObservableCollection<NotificationEntry>(Enumerable.Where(NotificationsList, (NotificationEntry s) => s.NotifType != NotificationType.Custom || !string.IsNullOrWhiteSpace(s.Content)));
		Followers = new ObservableCollection<FollowerEntry>(from e in Enumerable.Where(Followers, (FollowerEntry s) => !string.IsNullOrWhiteSpace(s.Name))
			group e by e.Name into g
			select g.First());
		SpecificStashingByName = new ObservableCollection<SpecialStashingRule>(from e in Enumerable.Where(SpecificStashingByName, (SpecialStashingRule s) => !string.IsNullOrWhiteSpace(s.TabName) && !string.IsNullOrWhiteSpace(s.Name))
			group e by e.Name into g
			select g.First());
		InitGeneralStashingRules();
		InitCurrencyStashingRules();
		InitChestEntries(ref list_2, GetDefaultChestList);
		InitChestEntries(ref list_3, GetDefaultMonolithList);
		InitChestEntries(ref list_4, GetDefaultStrongboxList);
		InitChestEntries(ref list_5, GetDefaultShrineList);
		if (InventoryCurrencies == null || InventoryCurrencies.Count < 1)
		{
			InventoryCurrencies = new ObservableCollection<InventoryCurrency>
			{
				new InventoryCurrency(CurrencyNames.Wisdom, 1, 1),
				new InventoryCurrency(CurrencyNames.Portal, 2, 1)
			};
		}
		int num = 1;
		int num2 = 0;
		foreach (InventoryCurrency item in InventoryCurrencies.OrderByDescending((InventoryCurrency r) => r.Name.Equals(CurrencyNames.Wisdom)).ToList())
		{
			num2++;
			if (num2 > 5)
			{
				num2 = 1;
				num++;
			}
			item.Row = num2;
			item.Column = num;
			if (item.Restock == -1)
			{
				item.Restock = PoeNinjaTracker.GetStackSize(item.Name) - 3;
			}
		}
		if (!NotificationsList.Any())
		{
			NotificationsList.Add(new NotificationEntry(NotificationType.AutoLoginBanned, "", useAddition: true));
			NotificationsList.Add(new NotificationEntry(NotificationType.MirrorFailedToPickup, "", useAddition: true));
			NotificationsList.Add(new NotificationEntry(NotificationType.ItemStashed, "Headhunter", useAddition: true));
		}
	}

	public List<string> GetTabsForCategory(string categoryName)
	{
		StashingRule stashingRule = Enumerable.FirstOrDefault(GeneralStashingRules, (StashingRule r) => r.Name == categoryName);
		if (stashingRule == null)
		{
			GlobalLog.Error("[EXtensions] Stashing rule requested for unknown name: \"" + categoryName + "\".");
			return GeneralStashingRules.First((StashingRule r) => r.Name == "Other").TabList;
		}
		return stashingRule.TabList;
	}

	public void SetGeneralStashingRule(string categoryName, List<string> tabs)
	{
		StashingRule stashingRule = GeneralStashingRules.Find((StashingRule r) => r.Name == categoryName);
		if (stashingRule == null)
		{
			GlobalLog.Error("[EXtensions] Stashing rule requested for unknown name: \"" + categoryName + "\".");
			return;
		}
		stashingRule.Tabs = string.Join(",", tabs);
		stashingRule.FillTabList();
	}

	public List<string> GetTabsForCurrency(string currencyName)
	{
		if (currencyName.EndsWith("Sextant"))
		{
			return GetIndividualOrDefault("Sextants");
		}
		if (hashSet_0.Contains(currencyName))
		{
			return GetIndividualOrDefault("Breach");
		}
		if (CurrencyNames.dictionary_0.TryGetValue(currencyName, out var value))
		{
			currencyName = value;
		}
		return GetIndividualOrDefault(currencyName);
	}

	private List<string> GetIndividualOrDefault(string currencyName)
	{
		SpecialStashingRule specialStashingRule = Enumerable.FirstOrDefault(SpecificStashingByName, (SpecialStashingRule r) => r.Name.Equals(currencyName));
		if (specialStashingRule != null)
		{
			return new List<string> { specialStashingRule.TabName };
		}
		TogglableStashingRule togglableStashingRule = CurrencyStashingRules.Find((TogglableStashingRule r) => r.Enabled && r.Name == currencyName);
		if (togglableStashingRule != null)
		{
			return togglableStashingRule.TabList;
		}
		if (Enumerable.Any(GeneralSettings.Instance.AnointOils, (OilEntry o) => o.Name == currencyName))
		{
			return GeneralStashingRules.Find((StashingRule r) => r.Name == "Currency").TabList;
		}
		if (currencyName.EndsWith("Oil"))
		{
			return GeneralStashingRules.Find((StashingRule r) => r.Name == "Oils").TabList;
		}
		if (currencyName.Equals("Remnant of Corruption") || currencyName.Contains("Essence"))
		{
			return GeneralStashingRules.Find((StashingRule r) => r.Name == "Essences").TabList;
		}
		return GeneralStashingRules.Find((StashingRule r) => r.Name == "Currency").TabList;
	}

	public bool IsTabFull(string tabName, string itemMetadata)
	{
		foreach (FullTabInfo fullTabs in FullTabsList)
		{
			if (!(fullTabs.Name != tabName))
			{
				List<string> controlsMetadata = fullTabs.ControlsMetadata;
				return controlsMetadata.Count == 0 || controlsMetadata.Contains(itemMetadata);
			}
		}
		return false;
	}

	public void MarkTabAsFull(string tabName, string itemMetadata)
	{
		FullTabInfo fullTabInfo = FullTabsList.Find((FullTabInfo t) => t.Name == tabName);
		if (fullTabInfo != null)
		{
			if (itemMetadata != null)
			{
				fullTabInfo.ControlsMetadata.Add(itemMetadata);
				GlobalLog.Debug("[MarkTabAsFull] Existing tab updated. Name: \"" + tabName + "\". Metadata: \"" + itemMetadata + "\".");
			}
			else
			{
				GlobalLog.Debug("[MarkTabAsFull] \"" + tabName + "\" is already marked as full.");
			}
		}
		else
		{
			FullTabsList.Add(new FullTabInfo(tabName, itemMetadata));
			GlobalLog.Debug("[MarkTabAsFull] New tab added. Name: \"" + tabName + "\". Metadata: \"" + (itemMetadata ?? "null") + "\".");
		}
	}

	private static List<StashingRule> GetDefaultGeneralStashingRules()
	{
		return new List<StashingRule>
		{
			new StashingRule("Tabs to ignore", "duck"),
			new StashingRule("Currency", "1"),
			new StashingRule("AltCurrency", "1"),
			new StashingRule("Rares", "2"),
			new StashingRule("Uniques", "2"),
			new StashingRule("Gems", "3"),
			new StashingRule("Cards", "3"),
			new StashingRule("Essences", "3"),
			new StashingRule("Oils", "3"),
			new StashingRule("Delve", "3"),
			new StashingRule("Splinters", "4"),
			new StashingRule("Jewels", "3"),
			new StashingRule("Maps", "4"),
			new StashingRule("Simulacrums", "3"),
			new StashingRule("Blighted Maps", "3"),
			new StashingRule("Influenced Maps", "4"),
			new StashingRule("Expensive Jewels [>50c worth]", "2"),
			new StashingRule("Charged Compass", "1"),
			new StashingRule("Fragments", "4"),
			new StashingRule("Other", "4")
		};
	}

	private static List<TogglableStashingRule> GetDefaultCurrencyStashingRules()
	{
		return new List<TogglableStashingRule>
		{
			new TogglableStashingRule("Breach", "1"),
			new TogglableStashingRule("Sextants", "1"),
			new TogglableStashingRule(CurrencyNames.PerandusCoin, "1"),
			new TogglableStashingRule(CurrencyNames.Wisdom, "1"),
			new TogglableStashingRule(CurrencyNames.Portal, "1"),
			new TogglableStashingRule(CurrencyNames.Transmutation, "1"),
			new TogglableStashingRule(CurrencyNames.Augmentation, "1"),
			new TogglableStashingRule(CurrencyNames.Alteration, "1"),
			new TogglableStashingRule(CurrencyNames.Scrap, "1"),
			new TogglableStashingRule(CurrencyNames.Whetstone, "1"),
			new TogglableStashingRule(CurrencyNames.Glassblower, "1"),
			new TogglableStashingRule(CurrencyNames.Chisel, "1"),
			new TogglableStashingRule(CurrencyNames.Chromatic, "1"),
			new TogglableStashingRule(CurrencyNames.Chance, "1"),
			new TogglableStashingRule(CurrencyNames.Alchemy, "1"),
			new TogglableStashingRule(CurrencyNames.Jeweller, "1"),
			new TogglableStashingRule(CurrencyNames.Scouring, "1"),
			new TogglableStashingRule(CurrencyNames.Fusing, "1"),
			new TogglableStashingRule(CurrencyNames.Blessed, "1"),
			new TogglableStashingRule(CurrencyNames.Regal, "1"),
			new TogglableStashingRule(CurrencyNames.Chaos, "1"),
			new TogglableStashingRule(CurrencyNames.Vaal, "1"),
			new TogglableStashingRule(CurrencyNames.Regret, "1"),
			new TogglableStashingRule(CurrencyNames.Gemcutter, "1"),
			new TogglableStashingRule(CurrencyNames.Divine, "1"),
			new TogglableStashingRule(CurrencyNames.Exalted, "1"),
			new TogglableStashingRule(CurrencyNames.Eternal, "1"),
			new TogglableStashingRule(CurrencyNames.Mirror, "1"),
			new TogglableStashingRule(CurrencyNames.Annulment, "1"),
			new TogglableStashingRule(CurrencyNames.Binding, "1"),
			new TogglableStashingRule(CurrencyNames.Horizon, "1"),
			new TogglableStashingRule(CurrencyNames.Harbinger, "1"),
			new TogglableStashingRule(CurrencyNames.Engineer, "1"),
			new TogglableStashingRule(CurrencyNames.Ancient, "1")
		};
	}

	private void InitGeneralStashingRules()
	{
		if (GeneralStashingRules.Count != 0)
		{
			List<StashingRule> defaultGeneralStashingRules = GetDefaultGeneralStashingRules();
			foreach (StashingRule stashingRule_0 in defaultGeneralStashingRules)
			{
				StashingRule stashingRule = GeneralStashingRules.Find((StashingRule c) => c.Name == stashingRule_0.Name);
				if (stashingRule != null)
				{
					stashingRule_0.CopyContents(stashingRule);
				}
			}
			GeneralStashingRules = defaultGeneralStashingRules;
		}
		else
		{
			GeneralStashingRules = GetDefaultGeneralStashingRules();
		}
		foreach (StashingRule generalStashingRule in GeneralStashingRules)
		{
			generalStashingRule.FillTabList();
		}
	}

	private void InitCurrencyStashingRules()
	{
		if (CurrencyStashingRules.Count != 0)
		{
			List<TogglableStashingRule> defaultCurrencyStashingRules = GetDefaultCurrencyStashingRules();
			foreach (TogglableStashingRule togglableStashingRule_0 in defaultCurrencyStashingRules)
			{
				TogglableStashingRule togglableStashingRule = CurrencyStashingRules.Find((TogglableStashingRule c) => c.Name == togglableStashingRule_0.Name);
				if (togglableStashingRule != null)
				{
					togglableStashingRule_0.CopyContents(togglableStashingRule);
				}
			}
			CurrencyStashingRules = defaultCurrencyStashingRules;
		}
		else
		{
			CurrencyStashingRules = GetDefaultCurrencyStashingRules();
		}
		foreach (TogglableStashingRule currencyStashingRule in CurrencyStashingRules)
		{
			currencyStashingRule.FillTabList();
		}
	}

	private static List<ChestEntry> GetDefaultChestList()
	{
		return new List<ChestEntry>
		{
			new ChestEntry("Chest"),
			new ChestEntry("Bone Chest"),
			new ChestEntry("Golden Chest"),
			new ChestEntry("Tribal Chest"),
			new ChestEntry("Sarcophagus"),
			new ChestEntry("Boulder"),
			new ChestEntry("Trunk"),
			new ChestEntry("Cocoon"),
			new ChestEntry("Corpse"),
			new ChestEntry("Bound Corpse"),
			new ChestEntry("Crucified Corpse"),
			new ChestEntry("Impaled Corpse"),
			new ChestEntry("Armour Rack"),
			new ChestEntry("Weapon Rack"),
			new ChestEntry("Scribe's Rack"),
			new ChestEntry("Bindle"),
			new ChestEntry("Cocoon"),
			new ChestEntry("Putrid Cocoon"),
			new ChestEntry("Decayed Cocoon"),
			new ChestEntry("LegionChests")
		};
	}

	private static List<ChestEntry> GetDefaultMonolithList()
	{
		return new List<ChestEntry>
		{
			new ChestEntry("Metadata/Terrain/Leagues/Legion/Objects/LegionInitiator")
		};
	}

	private static List<ChestEntry> GetDefaultStrongboxList()
	{
		return new List<ChestEntry>
		{
			new ChestEntry("Arcanist's Strongbox"),
			new ChestEntry("Armourer's Strongbox"),
			new ChestEntry("Artisan's Strongbox"),
			new ChestEntry("Blacksmith's Strongbox"),
			new ChestEntry("Cartographer's Strongbox"),
			new ChestEntry("Diviner's Strongbox"),
			new ChestEntry("Gemcutter's Strongbox"),
			new ChestEntry("Jeweller's Strongbox"),
			new ChestEntry("Large Strongbox"),
			new ChestEntry("Ornate Strongbox"),
			new ChestEntry("Strongbox")
		};
	}

	private static List<ChestEntry> GetDefaultShrineList()
	{
		return new List<ChestEntry>
		{
			new ChestEntry("Acceleration Shrine"),
			new ChestEntry("Brutal Shrine"),
			new ChestEntry("Diamond Shrine"),
			new ChestEntry("Divine Shrine", enabled: false),
			new ChestEntry("Echoing Shrine"),
			new ChestEntry("Freezing Shrine"),
			new ChestEntry("Impenetrable Shrine"),
			new ChestEntry("Lightning Shrine"),
			new ChestEntry("Massive Shrine"),
			new ChestEntry("Replenishing Shrine"),
			new ChestEntry("Resistance Shrine"),
			new ChestEntry("Shrouded Shrine"),
			new ChestEntry("Skeletal Shrine")
		};
	}

	private static void InitChestEntries(ref List<ChestEntry> jsonList, Func<List<ChestEntry>> getDefaulList)
	{
		if (jsonList.Count == 0)
		{
			jsonList = getDefaulList();
			return;
		}
		List<ChestEntry> list = getDefaulList();
		foreach (ChestEntry chestEntry_0 in list)
		{
			ChestEntry chestEntry = jsonList.Find((ChestEntry c) => c.Name == chestEntry_0.Name);
			if (chestEntry != null)
			{
				chestEntry_0.Enabled = chestEntry.Enabled;
			}
		}
		jsonList = list;
	}

	public string PauseReport()
	{
		string text = "";
		text += RuntimeReport();
		text += $"[IdRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.IdPause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.IdPause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[StashRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.StashPause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.StashPause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[SellRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.SellPause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.SellPause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[FastMoveFromStashRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.StashFastMovePause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.StashFastMovePause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[FastMoveToVendorRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.VendorFastMovePause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.VendorFastMovePause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[TownMoveRandomDelay] Triggered {PauseDataCollection[PauseTypeEnum.TownMovePause].Count} times, for a total of {PauseDataCollection[PauseTypeEnum.TownMovePause].Sum((PauseData x) => x.Pause) / 1000} seconds.\n";
		text += $"[NpcTalkPauseProbability] Triggered {PauseDataCollection[PauseTypeEnum.NpcTalkPauseProbability].Count} times.\n";
		text += $"[StashPauseProbability] Triggered {PauseDataCollection[PauseTypeEnum.StashPauseProbability].Count} times.\n";
		text += $"[WaypointPauseProbability] Triggered {PauseDataCollection[PauseTypeEnum.WaypointPauseProbability].Count} times.\n";
		text += TotalPauseReport();
		return text + RuntimeDifferenceReport();
	}

	public string RuntimeReport()
	{
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(TotalRuntime.ElapsedMilliseconds);
		return $"Total Runtime = Days:{timeSpan.Days}, Hours:{timeSpan.Hours}, Minutes:{timeSpan.Minutes}, Seconds:{timeSpan.Seconds}\n";
	}

	public string TotalPauseReport()
	{
		int num = PauseDataCollection.Sum((KeyValuePair<PauseTypeEnum, List<PauseData>> p) => p.Value.Sum((PauseData x) => x.Pause));
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(num);
		return $"Total Pause = Days:{timeSpan.Days}, Hours:{timeSpan.Hours}, Minutes:{timeSpan.Minutes}, Seconds:{timeSpan.Seconds}\n";
	}

	public string RuntimeDifferenceReport()
	{
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(TotalRuntime.ElapsedMilliseconds);
		int num = PauseDataCollection.Sum((KeyValuePair<PauseTypeEnum, List<PauseData>> p) => p.Value.Sum((PauseData x) => x.Pause));
		TimeSpan timeSpan2 = timeSpan.Subtract(TimeSpan.FromMilliseconds(num));
		return $"Production Runtime = Days:{timeSpan2.Days}, Hours:{timeSpan2.Hours}, Minutes:{timeSpan2.Minutes}, Seconds:{timeSpan2.Seconds}\n";
	}

	static ExtensionsSettings()
	{
		Rarity[] array = new Rarity[4];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		Rarities = (Rarity[])(object)array;
		CurrencyExchangeActs = new string[10] { "3", "4", "5", "6", "7", "8", "9", "10", "11", "Random" };
		hashSet_0 = new HashSet<string> { "Splinter of Xoph", "Splinter of Tul", "Splinter of Esh", "Splinter of Uul-Netol", "Splinter of Chayula", "Blessing of Xoph", "Blessing of Tul", "Blessing of Esh", "Blessing of Uul-Netol", "Blessing of Chayula" };
		PantheonGod[] array2 = new PantheonGod[4];
		RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		PantheonMajorGods = (PantheonGod[])(object)array2;
		PantheonGod[] array3 = new PantheonGod[8];
		RuntimeHelpers.InitializeArray(array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		PantheonMinorGods = (PantheonGod[])(object)array3;
		NotificationTypes = new NotificationType[16]
		{
			NotificationType.LevelUp,
			NotificationType.Death,
			NotificationType.BotStop,
			NotificationType.MirrorFailedToPickup,
			NotificationType.AutoLoginBanned,
			NotificationType.AutoLoginMaintenance,
			NotificationType.AutoLoginPatch,
			NotificationType.AutoLoginUnlockCode,
			NotificationType.ItemStashed,
			NotificationType.BlightFinished,
			NotificationType.BlightStarted,
			NotificationType.StrongboxOpened,
			NotificationType.MapFinished,
			NotificationType.SimulacrumWaveStart,
			NotificationType.CrucibleFound,
			NotificationType.Custom
		};
	}
}
