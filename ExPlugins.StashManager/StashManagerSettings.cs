using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using Newtonsoft.Json;

namespace ExPlugins.StashManager;

public class StashManagerSettings : JsonSettings
{
	private static StashManagerSettings stashManagerSettings_0;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

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
	private bool bool_9;

	[CompilerGenerated]
	private bool bool_10;

	[CompilerGenerated]
	private bool bool_11;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_0 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> zHwgJkuJti = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_1 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_2 = new ObservableCollection<NameEntry>();

	[CompilerGenerated]
	private ObservableCollection<MapEntry> observableCollection_3 = new ObservableCollection<MapEntry>();

	public static StashManagerSettings Instance => stashManagerSettings_0 ?? (stashManagerSettings_0 = new StashManagerSettings());

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

	[DefaultValue(1800)]
	public int SecondsBetweenScan
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

	[DefaultValue(3)]
	public int MinStackToSell
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

	[DefaultValue(10001)]
	public int MaxPriorityToSellMap
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

	[JsonIgnore]
	[DefaultValue("")]
	public string MapRegionToSellLast
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

	[DefaultValue(false)]
	public bool ShouldSellAllEssences
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
	public bool ShouldSellAllGems
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

	[DefaultValue(false)]
	public bool ShouldSellTrashScarabs
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
	public bool ShouldSellMaps
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
	public bool ShouldSellEssences
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
	public bool ShouldSellIncubators
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
	public bool ShoulApplyIncubators
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
	public bool ShouldSellFossils
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
	public bool ShouldSellMavenInvitations
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
	public bool ShouldSellUberLabOfferingsWhenUberLabCompleted
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
	public bool ShouldMakeClusterJewelRecipe
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

	[DefaultValue(84)]
	public int MinimumItemLevelToMakeClusterJewelRecipe
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

	public ObservableCollection<NameEntry> TabNamesToIgnore
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

	public ObservableCollection<NameEntry> ItemsToIgnore
	{
		[CompilerGenerated]
		get
		{
			return zHwgJkuJti;
		}
		[CompilerGenerated]
		set
		{
			zHwgJkuJti = value;
		}
	}

	public ObservableCollection<NameEntry> ItemsToForceSell
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

	public ObservableCollection<NameEntry> ItemsToSellInStack
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

	public ObservableCollection<MapEntry> MapLimits
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

	private StashManagerSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"StashManager.json"
		}))
	{
		TabNamesToIgnore = new ObservableCollection<NameEntry>(from s in TabNamesToIgnore
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ItemsToIgnore = new ObservableCollection<NameEntry>(from s in ItemsToIgnore
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ItemsToForceSell = new ObservableCollection<NameEntry>(from s in ItemsToForceSell
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		ItemsToSellInStack = new ObservableCollection<NameEntry>(from s in ItemsToSellInStack
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
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
		if (TabNamesToIgnore == null || TabNamesToIgnore.Count < 1)
		{
			TabNamesToIgnore = new ObservableCollection<NameEntry>
			{
				new NameEntry("IGNORED TAB")
			};
		}
		if (ItemsToIgnore == null || ItemsToIgnore.Count < 1)
		{
			ItemsToIgnore = new ObservableCollection<NameEntry>
			{
				new NameEntry("Headhunter")
			};
		}
		if (ItemsToSellInStack == null || ItemsToSellInStack.Count < 1)
		{
			ItemsToSellInStack = new ObservableCollection<NameEntry>
			{
				new NameEntry("Clear Oil"),
				new NameEntry("Sepia Oil"),
				new NameEntry("Amber Oil")
			};
		}
		if (ItemsToForceSell == null || ItemsToForceSell.Count < 1)
		{
			ItemsToForceSell = new ObservableCollection<NameEntry>
			{
				new NameEntry("Lesser Eldritch Ember"),
				new NameEntry("Lesser Eldritch Ichor")
			};
		}
	}
}
