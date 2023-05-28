using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using Newtonsoft.Json;

namespace ExPlugins.MapBotEx;

public class GeneralSettings : JsonSettings
{
	private static GeneralSettings generalSettings_0;

	[JsonIgnore]
	public static List<int> SlotValues;

	[JsonIgnore]
	public static List<string> OilTypes;

	private int int_0;

	private bool bool_0;

	private bool bool_1;

	private bool bool_2;

	private string string_0;

	[CompilerGenerated]
	private bool bool_3;

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
	private bool JafjFayCtS;

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
	private string string_1;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private bool bool_17;

	[CompilerGenerated]
	private bool bool_18;

	[CompilerGenerated]
	private bool bool_19;

	[CompilerGenerated]
	private bool bool_20;

	[CompilerGenerated]
	private int int_1;

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
	private bool bool_29;

	[CompilerGenerated]
	private bool bool_30;

	[CompilerGenerated]
	private int qZwjAsxBlu;

	[CompilerGenerated]
	private int giyjkNyjYV;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private bool bool_31;

	[CompilerGenerated]
	private bool bool_32;

	[CompilerGenerated]
	private bool gkBjzoyrPt;

	[CompilerGenerated]
	private bool bool_33;

	[CompilerGenerated]
	private bool bool_34;

	[CompilerGenerated]
	private bool bool_35;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private int int_4;

	[CompilerGenerated]
	private int int_5;

	[CompilerGenerated]
	private bool bool_36;

	[CompilerGenerated]
	private bool bool_37;

	[CompilerGenerated]
	private bool bool_38;

	[CompilerGenerated]
	private Upgrade upgrade_0 = new Upgrade
	{
		TierEnabled = true
	};

	[CompilerGenerated]
	private Upgrade upgrade_1 = new Upgrade();

	[CompilerGenerated]
	private Upgrade upgrade_2 = new Upgrade();

	[CompilerGenerated]
	private Upgrade upgrade_3 = new Upgrade();

	[CompilerGenerated]
	private Upgrade HfmHemEjLG = new Upgrade();

	[CompilerGenerated]
	private Upgrade upgrade_4 = new Upgrade();

	[CompilerGenerated]
	private int int_6;

	[CompilerGenerated]
	private int int_7;

	[CompilerGenerated]
	private bool bool_39;

	[CompilerGenerated]
	private ExistingRares existingRares_0;

	[CompilerGenerated]
	private RareReroll rareReroll_0;

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_0 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_1 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_2 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_3 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<OilEntry> observableCollection_4 = new ObservableCollection<OilEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_5 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<GolemEntry> observableCollection_6 = new ObservableCollection<GolemEntry>();

	[CompilerGenerated]
	private bool bool_40;

	[CompilerGenerated]
	private bool bool_41;

	[CompilerGenerated]
	private bool bool_42;

	public static GeneralSettings Instance => generalSettings_0 ?? (generalSettings_0 = new GeneralSettings());

	[DefaultValue(false)]
	public bool UseFiveSlotMapDevice
	{
		get
		{
			return bool_0;
		}
		set
		{
			if (!value.Equals(bool_0))
			{
				bool_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => UseFiveSlotMapDevice));
			}
		}
	}

	[DefaultValue(false)]
	public bool IsOnRun
	{
		get
		{
			return bool_1;
		}
		set
		{
			if (!value.Equals(bool_1))
			{
				bool_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => IsOnRun));
			}
		}
	}

	public static bool SimulacrumsEnabled => PluginManager.EnabledPlugins.Any((IPlugin p) => ((IAuthored)p).Name.Equals("SimulacrumPluginEx"));

	[DefaultValue(false)]
	public bool EnableMapLootStatistics
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
	public bool OpenPortals
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
	public bool AnointMaps
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
	public bool StartLegions
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

	[DefaultValue(true)]
	public bool ActivateDelirium
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
	public bool OpenBreaches
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
	public bool OnlyRunEnchantedMaps
	{
		[CompilerGenerated]
		get
		{
			return JafjFayCtS;
		}
		[CompilerGenerated]
		set
		{
			JafjFayCtS = value;
		}
	}

	[DefaultValue(false)]
	public bool BuildMetamorph
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
	public bool EnableHarvest
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
	public bool PickSulphite
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
	public bool RunConqMaps
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
	public bool SocketVoidstonesFromInventory
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
	public bool UseAwakenedSextants
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
	public bool UseElevatedSextants
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
	public bool UseMapDeviceMods
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

	[DefaultValue("Free")]
	public string MapDeviceModName
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

	[DefaultValue(1)]
	public double MaxFragmentCost
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

	[DefaultValue(true)]
	public bool BuyMapsFromKirac
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
	public bool OpenMavenIvitations
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
	public bool OpenMiniBossQuestInvitations
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
	public bool OpenBossQuestInvitations
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

	[DefaultValue(12)]
	public int MinTierToUsePrioritisedScarabs
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

	[DefaultValue(true)]
	public bool RunBlightedMaps
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

	[DefaultValue(false)]
	public bool RunBlightRavagedMaps
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

	[DefaultValue(false)]
	public bool AlchBlightedMaps
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

	[DefaultValue(false)]
	public bool AlchRavagedMaps
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

	[DefaultValue(false)]
	public bool VaalBlightedMaps
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
	public bool RunAlvaMissions
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
	public bool RunEinhardMissions
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
	public bool RunJunMissions
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

	[DefaultValue(true)]
	public bool RunNikoMissions
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

	[DefaultValue(true)]
	public bool RunKiracMissions
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

	[DefaultValue(10)]
	public int MaxMapTier
	{
		[CompilerGenerated]
		get
		{
			return qZwjAsxBlu;
		}
		[CompilerGenerated]
		set
		{
			qZwjAsxBlu = value;
		}
	}

	[DefaultValue(20)]
	public int MobRemaining
	{
		[CompilerGenerated]
		get
		{
			return giyjkNyjYV;
		}
		[CompilerGenerated]
		set
		{
			giyjkNyjYV = value;
		}
	}

	[DefaultValue(85)]
	public int ExplorationPercent
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

	[DefaultValue(false)]
	public bool EnterCorruptedAreas
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

	[DefaultValue(false)]
	public bool FastTransition
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

	[DefaultValue(false)]
	public bool RunUnId
	{
		[CompilerGenerated]
		get
		{
			return gkBjzoyrPt;
		}
		[CompilerGenerated]
		set
		{
			gkBjzoyrPt = value;
		}
	}

	[DefaultValue(false)]
	public bool ReturnForChaosRecipe
	{
		[CompilerGenerated]
		get
		{
			return bool_33;
		}
		[CompilerGenerated]
		set
		{
			bool_33 = value;
		}
	}

	[DefaultValue(true)]
	public bool SellEnabled
	{
		[CompilerGenerated]
		get
		{
			return bool_34;
		}
		[CompilerGenerated]
		set
		{
			bool_34 = value;
		}
	}

	[DefaultValue(true)]
	public bool SellIgnoredMaps
	{
		[CompilerGenerated]
		get
		{
			return bool_35;
		}
		[CompilerGenerated]
		set
		{
			bool_35 = value;
		}
	}

	[DefaultValue(10)]
	public int MaxSellTier
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

	[DefaultValue(10)]
	public int MaxSellPriority
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

	[DefaultValue(7)]
	public int MinMapAmount
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

	[DefaultValue(true)]
	public bool AtlasExplorationEnabled
	{
		[CompilerGenerated]
		get
		{
			return bool_36;
		}
		[CompilerGenerated]
		set
		{
			bool_36 = value;
		}
	}

	[DefaultValue(false)]
	public bool KiracChecked
	{
		[CompilerGenerated]
		get
		{
			return bool_37;
		}
		[CompilerGenerated]
		set
		{
			bool_37 = value;
		}
	}

	[DefaultValue(false)]
	public bool BossRushMode
	{
		[CompilerGenerated]
		get
		{
			return bool_38;
		}
		[CompilerGenerated]
		set
		{
			bool_38 = value;
		}
	}

	public Upgrade MagicUpgrade
	{
		[CompilerGenerated]
		get
		{
			return upgrade_0;
		}
		[CompilerGenerated]
		set
		{
			upgrade_0 = value;
		}
	}

	public Upgrade RareUpgrade
	{
		[CompilerGenerated]
		get
		{
			return upgrade_1;
		}
		[CompilerGenerated]
		set
		{
			upgrade_1 = value;
		}
	}

	public Upgrade ChiselUpgrade
	{
		[CompilerGenerated]
		get
		{
			return upgrade_2;
		}
		[CompilerGenerated]
		set
		{
			upgrade_2 = value;
		}
	}

	public Upgrade VaalUpgrade
	{
		[CompilerGenerated]
		get
		{
			return upgrade_3;
		}
		[CompilerGenerated]
		set
		{
			upgrade_3 = value;
		}
	}

	public Upgrade FragmentUpgrade
	{
		[CompilerGenerated]
		get
		{
			return HfmHemEjLG;
		}
		[CompilerGenerated]
		set
		{
			HfmHemEjLG = value;
		}
	}

	public Upgrade MagicRareUpgrade
	{
		[CompilerGenerated]
		get
		{
			return upgrade_4;
		}
		[CompilerGenerated]
		set
		{
			upgrade_4 = value;
		}
	}

	[DefaultValue(16)]
	public int MinMapIIQMinTier
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

	[DefaultValue(40)]
	public int MinMapIIQ
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
	public bool UseChargedCompass
	{
		[CompilerGenerated]
		get
		{
			return bool_39;
		}
		[CompilerGenerated]
		set
		{
			bool_39 = value;
		}
	}

	[DefaultValue(12)]
	public int AuraSwapSlot
	{
		get
		{
			return int_0;
		}
		set
		{
			if (!value.Equals(int_0))
			{
				int_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => AuraSwapSlot));
			}
		}
	}

	[DefaultValue("")]
	public string ReplaceAuraSkillName
	{
		get
		{
			return string_0;
		}
		set
		{
			if (!value.Equals(string_0))
			{
				string_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => ReplaceAuraSkillName));
			}
		}
	}

	public ExistingRares ExistingRares
	{
		[CompilerGenerated]
		get
		{
			return existingRares_0;
		}
		[CompilerGenerated]
		set
		{
			existingRares_0 = value;
		}
	}

	public RareReroll RerollMethod
	{
		[CompilerGenerated]
		get
		{
			return rareReroll_0;
		}
		[CompilerGenerated]
		set
		{
			rareReroll_0 = value;
		}
	}

	public ObservableCollection<NameEntry> ScarabsToIgnore
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

	public ObservableCollection<NameEntry> ScarabsToPrioritize
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

	public ObservableCollection<NameEntry> EnchantsToPrioritize
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

	public ObservableCollection<NameEntry> ItemNamesToIgnoreInTotalChaosValueCalc
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

	public ObservableCollection<OilEntry> AnointOils
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

	public ObservableCollection<NameEntry> SextantModsToSave
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

	public ObservableCollection<GolemEntry> GolemsToSummon
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

	public bool MavenInfluence
	{
		[CompilerGenerated]
		get
		{
			return bool_40;
		}
		[CompilerGenerated]
		set
		{
			bool_40 = value;
		}
	}

	public bool SearingExarchInfluence
	{
		[CompilerGenerated]
		get
		{
			return bool_41;
		}
		[CompilerGenerated]
		set
		{
			bool_41 = value;
		}
	}

	public bool EaterOfWorldsInfluence
	{
		[CompilerGenerated]
		get
		{
			return bool_42;
		}
		[CompilerGenerated]
		set
		{
			bool_42 = value;
		}
	}

	[JsonIgnore]
	public bool StopRequested
	{
		get
		{
			return bool_2;
		}
		set
		{
			if (value != bool_2)
			{
				bool_2 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => StopRequested));
			}
		}
	}

	private GeneralSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[3]
		{
			Configuration.Instance.Name,
			"MapBotEx",
			"GeneralSettings.json"
		}))
	{
		ScarabsToIgnore = new ObservableCollection<NameEntry>(from s in ScarabsToIgnore
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		EnchantsToPrioritize = new ObservableCollection<NameEntry>(from s in EnchantsToPrioritize
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ScarabsToPrioritize = new ObservableCollection<NameEntry>(from s in ScarabsToPrioritize
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ItemNamesToIgnoreInTotalChaosValueCalc = new ObservableCollection<NameEntry>(from s in ItemNamesToIgnoreInTotalChaosValueCalc
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		AnointOils = new ObservableCollection<OilEntry>(from s in AnointOils
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		SextantModsToSave = new ObservableCollection<NameEntry>(from s in SextantModsToSave
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		GolemsToSummon = new ObservableCollection<GolemEntry>(from s in GolemsToSummon
			where !string.IsNullOrWhiteSpace(s.Golem)
			select s into e
			group e by e.Golem into g
			select g.First());
		if (ScarabsToPrioritize == null)
		{
			ScarabsToPrioritize = new ObservableCollection<NameEntry>
			{
				new NameEntry("Blight"),
				new NameEntry("Cartography")
			};
		}
		if (ScarabsToIgnore == null)
		{
			ScarabsToIgnore = new ObservableCollection<NameEntry>
			{
				new NameEntry("Shaper"),
				new NameEntry("Elder")
			};
		}
		if (ItemNamesToIgnoreInTotalChaosValueCalc == null)
		{
			ItemNamesToIgnoreInTotalChaosValueCalc = new ObservableCollection<NameEntry>
			{
				new NameEntry("Clear Oil"),
				new NameEntry("Sepia Oil")
			};
		}
		if (AnointOils == null)
		{
			AnointOils = new ObservableCollection<OilEntry>
			{
				new OilEntry("Crimson Oil", 3, -1, blighted: true, ravaged: true),
				new OilEntry("Amber Oil", 1, -1, blighted: false, ravaged: true),
				new OilEntry("Silver Oil", 3, -1, blighted: false, ravaged: true),
				new OilEntry("Black Oil", 2, -1, blighted: false, ravaged: true)
			};
		}
	}

	static GeneralSettings()
	{
		SlotValues = new List<int>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13
		};
		OilTypes = new List<string>
		{
			"Clear Oil", "Sepia Oil", "Amber Oil", "Verdant Oil", "Teal Oil", "Azure Oil", "Indigo Oil", "Violet Oil", "Crimson Oil", "Black Oil",
			"Opalescent Oil", "Silver Oil", "Golden Oil"
		};
	}
}
