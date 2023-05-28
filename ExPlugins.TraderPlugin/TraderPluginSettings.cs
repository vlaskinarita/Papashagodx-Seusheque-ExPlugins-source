using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using JetBrains.Annotations;

namespace ExPlugins.TraderPlugin;

public class TraderPluginSettings : JsonSettings
{
	public class NameEntry : INotifyPropertyChanged
	{
		private string string_0;

		public string Name
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				OnPropertyChanged("Name");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public NameEntry()
		{
		}

		public NameEntry(string name)
		{
			Name = name;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static TraderPluginSettings traderPluginSettings_0;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private bool bool_4;

	[CompilerGenerated]
	private bool bool_5;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private bool bool_7;

	[CompilerGenerated]
	private int int_4;

	[CompilerGenerated]
	private int int_5;

	[CompilerGenerated]
	private int MmcbFftiT;

	[CompilerGenerated]
	private int int_6;

	[CompilerGenerated]
	private int int_7;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private int int_8;

	[CompilerGenerated]
	private int int_9;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private double double_1;

	[CompilerGenerated]
	private bool bool_8;

	[CompilerGenerated]
	private int int_10;

	[CompilerGenerated]
	private int int_11;

	[CompilerGenerated]
	private int int_12;

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
	private bool bool_17;

	[CompilerGenerated]
	private bool bool_18;

	[CompilerGenerated]
	private bool bool_19;

	[CompilerGenerated]
	private bool bool_20;

	[CompilerGenerated]
	private bool bool_21;

	[CompilerGenerated]
	private bool bool_22;

	[CompilerGenerated]
	private bool bool_23;

	[CompilerGenerated]
	private bool bool_24;

	[CompilerGenerated]
	private bool bool_25;

	[CompilerGenerated]
	private bool bool_26;

	[CompilerGenerated]
	private bool bool_27;

	[CompilerGenerated]
	private bool bool_28;

	[CompilerGenerated]
	private double double_2;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private bool bool_29;

	[CompilerGenerated]
	private bool bool_30;

	[CompilerGenerated]
	private bool bool_31;

	[CompilerGenerated]
	private double double_3;

	[CompilerGenerated]
	private int int_13;

	[CompilerGenerated]
	private bool bool_32;

	[CompilerGenerated]
	private string string_3;

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_0 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_1 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_2 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_3 = new ObservableCollection<NameEntry>();

	public static TraderPluginSettings Instance => traderPluginSettings_0 ?? (traderPluginSettings_0 = new TraderPluginSettings());

	[DefaultValue(false)]
	public bool DebugMode
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
	public bool RepriceTradeTab
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

	[DefaultValue(false)]
	public bool EnableConversationAI
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

	[DefaultValue(0)]
	public int EaterExarchInvitationPriceChaos
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

	[DefaultValue(true)]
	public bool ShouldListClusters
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

	[DefaultValue(true)]
	public bool ShouldListElderGuardianMaps
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

	[DefaultValue(true)]
	public bool ShouldListShaperGuardianMaps
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

	[DefaultValue(true)]
	public bool ShouldListAwakenerGuardianMaps
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

	[DefaultValue(15)]
	public int ElderGuardianMapsPrice
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

	[DefaultValue(15)]
	public int ShaperGuardianMapsPrice
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

	[DefaultValue(15)]
	public int AwakenerGuardianMapsPrice
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

	[DefaultValue(false)]
	public bool ShouldCheckExactEssencePrices
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

	[DefaultValue(10)]
	public int MinBulkAmtToCheckForExactPrice
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

	[DefaultValue(1)]
	public int CheckPricesFromResultNumber
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

	[DefaultValue(5)]
	public int CheckPricesToResultNumber
	{
		[CompilerGenerated]
		get
		{
			return MmcbFftiT;
		}
		[CompilerGenerated]
		set
		{
			MmcbFftiT = value;
		}
	}

	[DefaultValue(20)]
	public int FreeInventorySquaresToTradeBeforeStashing
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

	[DefaultValue(1800)]
	public int SecondsBetweenScan
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

	[DefaultValue("Standard")]
	public string LegaueName
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

	[DefaultValue("1")]
	public string StashTabToTrade
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

	[DefaultValue(5)]
	public int MinPriceInChaosToTrade
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

	[DefaultValue(5)]
	public int MinPriceInChaosToList
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

	[DefaultValue(100)]
	public double DefaultExPrice
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

	[DefaultValue(0.95)]
	public double StatReducer
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

	[DefaultValue(false)]
	public bool ShouldIgnoreTradesAfterMapLeaveAmount
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

	[DefaultValue(3)]
	public int MaxPortalsSpentToLeaveMap
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

	[DefaultValue(50)]
	public int MinPriceInChaosToLeaveMapWhenIgnoringTrades
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

	[DefaultValue(300)]
	public int DelayAfterCurrencyTradeSeconds
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

	[DefaultValue(true)]
	public bool ShouldSellRares
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
	public bool ShouldSellMavenInvitations
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

	[DefaultValue(true)]
	public bool ShouldSellUniques
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
	public bool ShouldSellGems
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

	[DefaultValue(true)]
	public bool ShouldSellContracts
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

	[DefaultValue(true)]
	public bool ShouldSellBlueprints
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

	[DefaultValue(true)]
	public bool ShouldSellDeliriumOrbs
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

	[DefaultValue(true)]
	public bool ShouldSellOils
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

	[DefaultValue(true)]
	public bool ShouldSellCatalysts
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
	public bool ShouldSellGambleCurrency
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
	public bool ShouldSellBlessings
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

	[DefaultValue(true)]
	public bool ShouldSellDivCards
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

	[DefaultValue(true)]
	public bool ShouldSellProphecy
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

	[DefaultValue(true)]
	public bool ShouldSellEssences
	{
		[CompilerGenerated]
		get
		{
			return bool_22;
		}
		[CompilerGenerated]
		set
		{
			bool_22 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellDelveCurrency
	{
		[CompilerGenerated]
		get
		{
			return bool_23;
		}
		[CompilerGenerated]
		set
		{
			bool_23 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellMapFragments
	{
		[CompilerGenerated]
		get
		{
			return bool_24;
		}
		[CompilerGenerated]
		set
		{
			bool_24 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellSacFragments
	{
		[CompilerGenerated]
		get
		{
			return bool_25;
		}
		[CompilerGenerated]
		set
		{
			bool_25 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellScarabs
	{
		[CompilerGenerated]
		get
		{
			return bool_26;
		}
		[CompilerGenerated]
		set
		{
			bool_26 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellLogbooks
	{
		[CompilerGenerated]
		get
		{
			return bool_27;
		}
		[CompilerGenerated]
		set
		{
			bool_27 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellStackedDicks
	{
		[CompilerGenerated]
		get
		{
			return bool_28;
		}
		[CompilerGenerated]
		set
		{
			bool_28 = value;
		}
	}

	[DefaultValue(2)]
	public double DefaultStackedDeckPrice
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

	[DefaultValue("~price 100/50 chaos")]
	public string StackedDeckExactNote
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

	[DefaultValue(true)]
	public bool ShouldSellMaps
	{
		[CompilerGenerated]
		get
		{
			return bool_29;
		}
		[CompilerGenerated]
		set
		{
			bool_29 = value;
		}
	}

	[DefaultValue(false)]
	public bool ShouldSellBlightedMaps
	{
		[CompilerGenerated]
		get
		{
			return bool_30;
		}
		[CompilerGenerated]
		set
		{
			bool_30 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSellBlightRavagedMaps
	{
		[CompilerGenerated]
		get
		{
			return bool_31;
		}
		[CompilerGenerated]
		set
		{
			bool_31 = value;
		}
	}

	[DefaultValue(0)]
	public double DefaultRemnantOfCorruptionPrice
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

	[DefaultValue(30)]
	public int DefaultLogbookPrice
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

	[DefaultValue(false)]
	public bool TelemetryEnabled
	{
		[CompilerGenerated]
		get
		{
			return bool_32;
		}
		[CompilerGenerated]
		set
		{
			bool_32 = value;
		}
	}

	[DefaultValue("")]
	public string ApiEndpoint
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

	public ObservableCollection<NameEntry> ItemsToIgnore
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

	public ObservableCollection<NameEntry> ItemsToList
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

	public ObservableCollection<NameEntry> TabNamesToIgnoreOnScan
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

	public ObservableCollection<NameEntry> LocationNamesToIgnoreTradeIn
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

	private TraderPluginSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"TraderPlugin.json"
		}))
	{
		ItemsToIgnore = new ObservableCollection<NameEntry>(from s in ItemsToIgnore
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ItemsToList = new ObservableCollection<NameEntry>(from s in ItemsToList
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		TabNamesToIgnoreOnScan = new ObservableCollection<NameEntry>(from s in TabNamesToIgnoreOnScan
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		LocationNamesToIgnoreTradeIn = new ObservableCollection<NameEntry>(from s in LocationNamesToIgnoreTradeIn
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		if (ItemsToIgnore == null || ItemsToIgnore.Count < 1)
		{
			ItemsToIgnore = new ObservableCollection<NameEntry>
			{
				new NameEntry("Rusted Blight Scarab"),
				new NameEntry("Polished Blight Scarab"),
				new NameEntry("Gilded Blight Scarab"),
				new NameEntry("Black Oil"),
				new NameEntry("Opalescent Oil"),
				new NameEntry("Polished Cartography Scarab")
			};
		}
		if (TabNamesToIgnoreOnScan == null || TabNamesToIgnoreOnScan.Count < 1)
		{
			TabNamesToIgnoreOnScan = new ObservableCollection<NameEntry>
			{
				new NameEntry("IGNORED TAB")
			};
		}
		if (LocationNamesToIgnoreTradeIn == null || LocationNamesToIgnoreTradeIn.Count < 1)
		{
			LocationNamesToIgnoreTradeIn = new ObservableCollection<NameEntry>
			{
				new NameEntry("Absence of Patience and Wisdom"),
				new NameEntry("Absence of Symmetry and Harmony"),
				new NameEntry("Polaric Void"),
				new NameEntry("Seething Chyme")
			};
		}
		if (ItemsToList == null || ItemsToList.Count < 1)
		{
			ItemsToList = new ObservableCollection<NameEntry>
			{
				new NameEntry("Oil Extractor"),
				new NameEntry("Uul-Netol's Breachstone"),
				new NameEntry("Tul's Breachstone"),
				new NameEntry("Xoph's Breachstone"),
				new NameEntry("Chayula's Breachstone"),
				new NameEntry("Esh's Breachstone"),
				new NameEntry("Timeless Eternal Emblem"),
				new NameEntry("Timeless Karui Emblem"),
				new NameEntry("Timeless Vaal Emblem"),
				new NameEntry("Timeless Templar Emblem"),
				new NameEntry("Timeless Maraketh Emblem"),
				new NameEntry("Simulacrum"),
				new NameEntry("Stacked Deck"),
				new NameEntry("Veiled Chaos Orb"),
				new NameEntry("Lesser Eldritch Ichor"),
				new NameEntry("Lesser Eldritch Ember"),
				new NameEntry("Greater Eldritch Ichor"),
				new NameEntry("Grand Eldritch Ichor"),
				new NameEntry("Grand Eldritch Ember"),
				new NameEntry("Exceptional Eldritch Ichor"),
				new NameEntry("Exceptional Eldritch Ember"),
				new NameEntry("Orb of Conflict"),
				new NameEntry("Oil Extractor")
			};
		}
	}
}
