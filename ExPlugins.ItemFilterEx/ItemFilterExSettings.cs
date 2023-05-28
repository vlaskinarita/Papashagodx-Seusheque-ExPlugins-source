using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions;
using Newtonsoft.Json;

namespace ExPlugins.ItemFilterEx;

public class ItemFilterExSettings : JsonSettings
{
	private static ItemFilterExSettings itemFilterExSettings_0;

	[JsonIgnore]
	public Dictionary<string, int> CachedCurrency = new Dictionary<string, int>();

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private double double_1;

	[CompilerGenerated]
	private double double_2;

	[CompilerGenerated]
	private double double_3;

	[CompilerGenerated]
	private double double_4;

	[CompilerGenerated]
	private double double_5;

	[CompilerGenerated]
	private double double_6;

	[CompilerGenerated]
	private double double_7;

	[CompilerGenerated]
	private double double_8;

	[CompilerGenerated]
	private double double_9;

	[CompilerGenerated]
	private double double_10;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private int int_4;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private int int_5;

	[CompilerGenerated]
	private bool bool_4;

	[CompilerGenerated]
	private bool bool_5;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private bool bool_7;

	[CompilerGenerated]
	private bool bool_8;

	[CompilerGenerated]
	private bool bool_9;

	[CompilerGenerated]
	private bool bool_10;

	[CompilerGenerated]
	private bool bool_11;

	[CompilerGenerated]
	private bool bool_12;

	[CompilerGenerated]
	private bool bool_13;

	[CompilerGenerated]
	private bool bool_14;

	[CompilerGenerated]
	private bool bool_15;

	[CompilerGenerated]
	private bool bool_16;

	[CompilerGenerated]
	private bool reIpbqNcpQ;

	[CompilerGenerated]
	private bool bool_17;

	[CompilerGenerated]
	private bool bool_18;

	[CompilerGenerated]
	private bool bool_19;

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_0 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_1 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_2 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_3 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_4 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_5 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_6 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_7 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> llVpoOhwIO = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<MapEntry> observableCollection_8 = new ObservableCollection<MapEntry>();

	[CompilerGenerated]
	private ObservableCollection<CurrencyEntry> observableCollection_9 = new ObservableCollection<CurrencyEntry>();

	[CompilerGenerated]
	private ObservableCollection<EnchantEntry> observableCollection_10 = new ObservableCollection<EnchantEntry>();

	[CompilerGenerated]
	private ObservableCollection<OfficialPricecheckEntry> observableCollection_11 = new ObservableCollection<OfficialPricecheckEntry>();

	[CompilerGenerated]
	private bool bool_20;

	[CompilerGenerated]
	private bool bool_21;

	[CompilerGenerated]
	private int int_6;

	[CompilerGenerated]
	private int int_7;

	[CompilerGenerated]
	private int int_8;

	[CompilerGenerated]
	private int int_9;

	[CompilerGenerated]
	private int opEiiBwudA;

	[CompilerGenerated]
	private int int_10;

	[CompilerGenerated]
	private int int_11;

	[CompilerGenerated]
	private int int_12;

	[CompilerGenerated]
	private int int_13;

	[CompilerGenerated]
	private int int_14;

	[CompilerGenerated]
	private ObservableCollection<CurrencyTrackerEntry> observableCollection_12 = new ObservableCollection<CurrencyTrackerEntry>();

	[CompilerGenerated]
	private Stopwatch stopwatch_0 = new Stopwatch();

	public static List<string> FractureBasesList;

	public static ItemFilterExSettings Instance => itemFilterExSettings_0 ?? (itemFilterExSettings_0 = new ItemFilterExSettings());

	[DefaultValue(true)]
	public bool IdMaps
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

	[DefaultValue(true)]
	public bool MapOverLimit
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

	[DefaultValue(3)]
	public int OilEssenceStackCountToUpgrade
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

	[DefaultValue(100)]
	public int MaxMapAmountPickup
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

	[DefaultValue(84)]
	public int MinClusterIlvlToKeep
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

	[DefaultValue(5.0)]
	public double MinUniquePriceToKeep
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

	[DefaultValue(5.0)]
	public double MinDivCardPriceToKeep
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

	[DefaultValue(5.0)]
	public double MinScoutingReportPriceToKeep
	{
		[CompilerGenerated]
		get
		{
			return double_2;
		}
		[CompilerGenerated]
		set
		{
			double_2 = value;
		}
	}

	[DefaultValue(1.0)]
	public double MinFossilPriceToKeep
	{
		[CompilerGenerated]
		get
		{
			return double_3;
		}
		[CompilerGenerated]
		set
		{
			double_3 = value;
		}
	}

	[DefaultValue(1.0)]
	public double MinScarabPriceToKeep
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

	[DefaultValue(1.0)]
	public double MinOilPriceToKeep
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

	[DefaultValue(1.0)]
	public double MinBreachstonePriceToKeep
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

	[DefaultValue(7.0)]
	public double MinGemPriceToKeep
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

	[DefaultValue(50.0)]
	public double MinClusterPriceToKeep
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

	[DefaultValue(50.0)]
	public double MinFracturedPriceToKeep
	{
		[CompilerGenerated]
		get
		{
			return double_9;
		}
		[CompilerGenerated]
		set
		{
			double_9 = value;
		}
	}

	[DefaultValue(2.5)]
	public double MaxOilEssencePriceToSellInStack
	{
		[CompilerGenerated]
		get
		{
			return double_10;
		}
		[CompilerGenerated]
		set
		{
			double_10 = value;
		}
	}

	[DefaultValue(1)]
	public int MinQualityForGemToPickup
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

	[DefaultValue(true)]
	public bool ForceLimitCurrencyPickup
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
		[CompilerGenerated]
		set
		{
			bool_2 = value;
		}
	}

	[DefaultValue(5)]
	public int MinScrollStackToPickup
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

	[DefaultValue(false)]
	public bool CustomScrollHandler
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
		[CompilerGenerated]
		set
		{
			bool_3 = value;
		}
	}

	[DefaultValue(300)]
	public int CustomScrollAmount
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

	[DefaultValue(false)]
	public bool PickupVeiledItems
	{
		[CompilerGenerated]
		get
		{
			return bool_4;
		}
		[CompilerGenerated]
		set
		{
			bool_4 = value;
		}
	}

	[DefaultValue(false)]
	public bool PickupFracturedItems
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

	[DefaultValue(false)]
	public bool PriceCheckFracturedItems
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

	[DefaultValue(false)]
	public bool PickupSynthItems
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

	[DefaultValue(false)]
	public bool PickupAllRares
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

	[DefaultValue(false)]
	public bool SellAllRares
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
	public bool PickupBlightedMaps
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

	[DefaultValue(false)]
	public bool PickupSmallRgb
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

	[DefaultValue(true)]
	public bool PickupEssences
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

	[DefaultValue(false)]
	public bool PickupLowTierEssences
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
	public bool Pickup6Socket
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

	[DefaultValue(false)]
	public bool PickupRareAmulets
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

	[DefaultValue(false)]
	public bool PickupRareRings
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

	[DefaultValue(false)]
	public bool PickupRareBelts
	{
		[CompilerGenerated]
		get
		{
			return reIpbqNcpQ;
		}
		[CompilerGenerated]
		set
		{
			reIpbqNcpQ = value;
		}
	}

	[DefaultValue(false)]
	public bool PickupRareJewels
	{
		[CompilerGenerated]
		get
		{
			return bool_17;
		}
		[CompilerGenerated]
		set
		{
			bool_17 = value;
		}
	}

	[DefaultValue(true)]
	public bool PickupLogBooks
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

	[DefaultValue(true)]
	public bool UpgradeOilEssences
	{
		[CompilerGenerated]
		get
		{
			return bool_19;
		}
		[CompilerGenerated]
		set
		{
			bool_19 = value;
		}
	}

	public ObservableCollection<NameEntry> AlwaysPickupGems
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

	public ObservableCollection<NameEntry> DivCardsToKeep
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

	public ObservableCollection<NameEntry> MapsToKeep
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

	public ObservableCollection<NameEntry> UniquesToKeep
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

	public ObservableCollection<NameEntry> UniquesToAvoid
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_4;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_4 = value;
		}
	}

	public ObservableCollection<NameEntry> AlwaysSellItemNames
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_5;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_5 = value;
		}
	}

	public ObservableCollection<NameEntry> AlwaysSellUniques
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_6;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_6 = value;
		}
	}

	public ObservableCollection<NameEntry> AlwaysPickupItemNames
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_7;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_7 = value;
		}
	}

	public ObservableCollection<NameEntry> ContractJobsToSave
	{
		[CompilerGenerated]
		get
		{
			return llVpoOhwIO;
		}
		[CompilerGenerated]
		set
		{
			llVpoOhwIO = value;
		}
	}

	public ObservableCollection<MapEntry> MapLimits
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_8;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_8 = value;
		}
	}

	public ObservableCollection<CurrencyEntry> CurrencyLimits
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_9;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_9 = value;
		}
	}

	public ObservableCollection<EnchantEntry> EnchantsToSave
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_10;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_10 = value;
		}
	}

	public ObservableCollection<OfficialPricecheckEntry> OfficialPricecheck
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_11;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_11 = value;
		}
	}

	[JsonIgnore]
	public bool IsStashCached
	{
		[CompilerGenerated]
		get
		{
			return bool_20;
		}
		[CompilerGenerated]
		set
		{
			bool_20 = value;
		}
	}

	[JsonIgnore]
	public bool IsCurrencyCachedOnStart
	{
		[CompilerGenerated]
		get
		{
			return bool_21;
		}
		[CompilerGenerated]
		set
		{
			bool_21 = value;
		}
	}

	[DefaultValue(100)]
	public int MapAmount
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

	public int WhiteMapAmount
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

	public int YellowMapAmount
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

	public int RedMapAmount
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

	public int InfluencedCount
	{
		[CompilerGenerated]
		get
		{
			return opEiiBwudA;
		}
		[CompilerGenerated]
		set
		{
			opEiiBwudA = value;
		}
	}

	public int ChaosAmount
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

	public int DivAmount
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

	public int AlchAmount
	{
		[CompilerGenerated]
		get
		{
			return int_12;
		}
		[CompilerGenerated]
		set
		{
			int_12 = value;
		}
	}

	[JsonIgnore]
	public int RecipeCount
	{
		[CompilerGenerated]
		get
		{
			return int_13;
		}
		[CompilerGenerated]
		set
		{
			int_13 = value;
		}
	}

	[JsonIgnore]
	public int DeathCount
	{
		[CompilerGenerated]
		get
		{
			return int_14;
		}
		[CompilerGenerated]
		set
		{
			int_14 = value;
		}
	}

	[JsonIgnore]
	public ObservableCollection<CurrencyTrackerEntry> ShownEntries
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_12;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_12 = value;
		}
	}

	[JsonIgnore]
	public Stopwatch Runtime
	{
		[CompilerGenerated]
		get
		{
			return stopwatch_0;
		}
		[CompilerGenerated]
		set
		{
			stopwatch_0 = value;
		}
	}

	private ItemFilterExSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"ItemFilterEx.json"
		}))
	{
		AlwaysPickupGems = new ObservableCollection<NameEntry>(from s in AlwaysPickupGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		OfficialPricecheck = new ObservableCollection<OfficialPricecheckEntry>(from s in OfficialPricecheck
			where !string.IsNullOrWhiteSpace(s.FullName)
			select s into e
			group e by e.FullName into g
			select g.First());
		DivCardsToKeep = new ObservableCollection<NameEntry>(from s in DivCardsToKeep
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		MapsToKeep = new ObservableCollection<NameEntry>(from s in MapsToKeep
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		UniquesToKeep = new ObservableCollection<NameEntry>(from s in UniquesToKeep
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		UniquesToAvoid = new ObservableCollection<NameEntry>(from s in UniquesToAvoid
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		AlwaysSellItemNames = new ObservableCollection<NameEntry>(from s in AlwaysSellItemNames
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		AlwaysPickupItemNames = new ObservableCollection<NameEntry>(from s in AlwaysPickupItemNames
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		AlwaysSellUniques = new ObservableCollection<NameEntry>(from s in AlwaysSellUniques
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ContractJobsToSave = new ObservableCollection<NameEntry>(from s in ContractJobsToSave
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		CurrencyLimits = new ObservableCollection<CurrencyEntry>(from s in CurrencyLimits
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		EnchantsToSave = new ObservableCollection<EnchantEntry>(from s in EnchantsToSave
			where !string.IsNullOrWhiteSpace(s.InternalName)
			select s into e
			group e by e.InternalName into g
			select g.First());
		if (CurrencyLimits == null || CurrencyLimits.Count < 1)
		{
			CurrencyLimits = new ObservableCollection<CurrencyEntry>
			{
				new CurrencyEntry(CurrencyNames.Transmutation, 40),
				new CurrencyEntry(CurrencyNames.Augmentation, 30),
				new CurrencyEntry(CurrencyNames.Scrap, 40),
				new CurrencyEntry(CurrencyNames.Whetstone, 20),
				new CurrencyEntry(CurrencyNames.Glassblower, 80),
				new CurrencyEntry(CurrencyNames.SplinterTul, 0),
				new CurrencyEntry(CurrencyNames.SplinterEsh, 0),
				new CurrencyEntry(CurrencyNames.SplinterXoph, 0),
				new CurrencyEntry(CurrencyNames.SplinterUulNetol, 0),
				new CurrencyEntry(CurrencyNames.EngineerShard, 0),
				new CurrencyEntry(CurrencyNames.HorizonShard, 0)
			};
		}
		if (AlwaysPickupGems == null || AlwaysPickupGems.Count < 1)
		{
			AlwaysPickupGems = new ObservableCollection<NameEntry>
			{
				new NameEntry("Empower Support"),
				new NameEntry("Enlighten Support"),
				new NameEntry("Enhance Support")
			};
		}
		if (MapsToKeep == null)
		{
			MapsToKeep = new ObservableCollection<NameEntry>
			{
				new NameEntry("Arcade Map"),
				new NameEntry("Burial Chambers Map")
			};
		}
		if (MapLimits == null || MapLimits.Count < 1)
		{
			MapLimits = new ObservableCollection<MapEntry>
			{
				new MapEntry(1, 3),
				new MapEntry(2, 3),
				new MapEntry(3, 3),
				new MapEntry(4, 3),
				new MapEntry(5, 3),
				new MapEntry(6, 5),
				new MapEntry(7, 5),
				new MapEntry(8, 5),
				new MapEntry(9, 5),
				new MapEntry(10, 15),
				new MapEntry(11, 15),
				new MapEntry(12, 15),
				new MapEntry(13, 15),
				new MapEntry(14, 15),
				new MapEntry(15, 15),
				new MapEntry(16, 15)
			};
		}
		if (DivCardsToKeep == null || DivCardsToKeep.Count < 1)
		{
			DivCardsToKeep = new ObservableCollection<NameEntry>
			{
				new NameEntry("House of Mirrors"),
				new NameEntry("The Doctor"),
				new NameEntry("The Fiend")
			};
		}
		if (UniquesToKeep == null || UniquesToKeep.Count < 1)
		{
			UniquesToKeep = new ObservableCollection<NameEntry>
			{
				new NameEntry("Headhunter"),
				new NameEntry("Mageblood")
			};
		}
		if (AlwaysPickupItemNames == null || AlwaysPickupItemNames.Count < 1)
		{
			AlwaysPickupItemNames = new ObservableCollection<NameEntry>
			{
				new NameEntry("Incandescent Invitation"),
				new NameEntry("Screaming Invitation")
			};
		}
		if (AlwaysSellUniques == null || AlwaysSellUniques.Count < 1)
		{
			AlwaysSellUniques = new ObservableCollection<NameEntry>();
		}
		if (ContractJobsToSave == null || ContractJobsToSave.Count < 1)
		{
			ContractJobsToSave = new ObservableCollection<NameEntry>
			{
				new NameEntry("Deception")
			};
		}
		if (AlwaysSellItemNames == null || AlwaysSellItemNames.Count < 1)
		{
			AlwaysSellItemNames = new ObservableCollection<NameEntry>
			{
				new NameEntry("Divine Vessel"),
				new NameEntry("Shadow Sceptre")
			};
		}
		if (OfficialPricecheck == null || OfficialPricecheck.Count < 1)
		{
			OfficialPricecheck = new ObservableCollection<OfficialPricecheckEntry>
			{
				new OfficialPricecheckEntry(enabled: true, "Split Personality", checkStats: true, new ObservableCollection<string>(), checkIlvl: false, checkCorrupted: false)
			};
		}
	}

	static ItemFilterExSettings()
	{
		FractureBasesList = new List<string>
		{
			"Agate Amulet", "Amber Amulet", "Amethyst Ring", "Apothecary's Gloves", "Blue Pearl Amulet", "Bone Helmet", "Carnal Sceptre", "Cerulean Ring", "Citadel Bow", "Citrine Amulet",
			"Convening Wand", "Convoking Wand", "Crystal Wand", "Demon's Horn", "Eternal Burgonet", "Fingerless Silk Gloves", "Fossilised Spirit Shield", "Fugitive Boots", "Gemini Claw", "Gripped Gloves",
			"Harbinger Bow", "Harmonic Spirit Shield", "Heathen Wand", "Heavy Arrow Quiver", "Hubris Circlet", "Imbued Wand", "Imperial Bow", "Imperial Claw", "Iolite Ring", "Jade Amulet",
			"Lapis Amulet", "Leather Belt", "Maraketh Bow", "Marble Amulet", "Omen Wand", "Onyx Amulet", "Opal Ring", "Opal Sceptre", "Opal Wand", "Pagan Wand",
			"Penetrating Arrow Quiver", "Primal Arrow Quiver", "Profane Wand", "Prophecy Wand", "Royal Burgonet", "Ruby Ring", "Sambar Sceptre", "Sapphire Ring", "Short Bow", "Sorcerer Boots",
			"Sorcerer Gloves", "Spike-Point Arrow Quiver", "Spiked Gloves", "Spine Bow", "Steel Ring", "Thicket Bow", "Titan Gauntlets", "Titan Greaves", "Titanium Spirit Shield", "Topaz Ring",
			"Tornado Wand", "Turquoise Amulet", "Two-Stone Ring", "Two-Toned Boots", "Unset Ring", "Vaal Greaves", "Vaal Sceptre", "Vaal Spirit Shield", "Vermillion Ring", "Void Sceptre",
			"Ivory Spirit Shield", "Bone Spirit Shield", "Lacewood Spirit Shield", "Thorium Spirit Shield", "Chiming Spirit Shield", "Ancient Spirit Shield", "Walnut Spirit Shield", "Brass Spirit Shield", "Jingling Spirit Shield", "Tarnished Spirit Shield",
			"Twig Spirit Shield", "Yew Spirit Shield", "Vaal Gauntlets", "Goliath Greaves", "Terror Claw", "Eye Gouger", "Hellion's Paw", "Throat Stabber", "Twin Claw", "Eagle Claw",
			"Noble Claw", "Gut Ripper", "Tiger's Paw", "Gouger", "Double Claw", "Fright Claw", "Sparkling Claw", "Blinder", "Cat's Paw", "Awl",
			"Sharktooth Claw", "Nailed Fist", "Highborn Bow", "Crude Bow", "Bone Bow", "Royal Bow", "Grove Bow", "Reflex Bow", "Ivory Bow", "Steelwood Bow",
			"Platinum Sceptre", "Tyrant's Sekhem", "Karui Sceptre", "Stag Sceptre", "Abyssal Sceptre", "Royal Sceptre", "Blood Sceptre", "Lead Sceptre", "Crystal Sceptre", "Sekhem",
			"Horned Sceptre", "Grinning Fetish", "Shadow Sceptre", "Ritual Sceptre", "Ochre Sceptre", "Iron Sceptre", "Quartz Sceptre", "Bronze Sceptre", "Darkwood Sceptre", "Driftwood Sceptre",
			"Coiled Wand", "Engraved Wand", "Faun's Horn", "Sage Wand", "Spiraled Wand", "Calling Wand", "Quartz Wand", "Carved Wand", "Goat's Horn", "Driftwood Wand"
		};
	}
}
