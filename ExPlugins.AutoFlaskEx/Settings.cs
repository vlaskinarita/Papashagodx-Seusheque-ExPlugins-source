using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game.GameData;
using Newtonsoft.Json;

namespace ExPlugins.AutoFlaskEx;

public class Settings : JsonSettings
{
	public class FlaskEntry
	{
		[CompilerGenerated]
		private readonly string string_0;

		[CompilerGenerated]
		private ObservableCollection<FlaskTrigger> observableCollection_0;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public ObservableCollection<FlaskTrigger> Triggers
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

		public FlaskEntry(string name)
		{
			string_0 = name;
			Triggers = new ObservableCollection<FlaskTrigger>();
		}
	}

	private static Settings settings_0;

	[JsonIgnore]
	public static readonly TriggerType[] TriggerTypes;

	[JsonIgnore]
	public static readonly Rarity[] Rarities;

	private readonly List<FlaskEntry> list_0 = new List<FlaskEntry>();

	private readonly List<FlaskEntry> list_1 = new List<FlaskEntry>();

	[CompilerGenerated]
	private int int_0 = 75;

	[CompilerGenerated]
	private int int_1 = 50;

	[CompilerGenerated]
	private int int_2 = 30;

	[CompilerGenerated]
	private int int_3 = 80;

	[CompilerGenerated]
	private bool bool_0;

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
	private int int_4 = 1;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private int int_5 = 1;

	public static Settings Instance => settings_0 ?? (settings_0 = new Settings());

	public int HpPercent
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

	public int HpPercentInstant
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

	public int MpPercent
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

	public int QsilverRange
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

	public bool RemoveFreeze
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

	public bool RemoveShock
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

	public bool RemoveIgnite
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

	public bool RemoveSilence
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

	public bool RemoveBleed
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

	public bool RemoveCblood
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

	public int MinCbloodStacks
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

	public bool RemovePoison
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

	public int MinPoisonStacks
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

	public List<FlaskEntry> UtilityFlasks => list_1;

	public List<FlaskEntry> UniqueFlasks => list_0;

	private Settings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"AutoFlaskEx.json"
		}))
	{
		InitFlaskList(ref list_1, GetDefaultUtilityFlaskList);
		InitFlaskList(ref list_0, GetDefaultUniqueFlaskList);
	}

	public List<FlaskTrigger> GetFlaskTriggers(string name)
	{
		foreach (FlaskEntry item in list_1)
		{
			if (item.Name == name)
			{
				return item.Triggers.ToList();
			}
		}
		foreach (FlaskEntry item2 in list_0)
		{
			if (item2.Name == name)
			{
				return item2.Triggers.ToList();
			}
		}
		return null;
	}

	private static List<FlaskEntry> GetDefaultUtilityFlaskList()
	{
		List<FlaskEntry> source = new List<FlaskEntry>
		{
			new FlaskEntry("Amethyst Flask"),
			new FlaskEntry("Aquamarine Flask"),
			new FlaskEntry("Basalt Flask"),
			new FlaskEntry("Bismuth Flask"),
			new FlaskEntry("Diamond Flask"),
			new FlaskEntry("Granite Flask"),
			new FlaskEntry("Jade Flask"),
			new FlaskEntry("Quartz Flask"),
			new FlaskEntry("Ruby Flask"),
			new FlaskEntry("Sapphire Flask"),
			new FlaskEntry("Silver Flask"),
			new FlaskEntry("Gold Flask"),
			new FlaskEntry("Stibnite Flask"),
			new FlaskEntry("Sulphur Flask"),
			new FlaskEntry("Topaz Flask"),
			new FlaskEntry("Quicksilver Flask")
		};
		return source.OrderBy((FlaskEntry a) => a.Name).ToList();
	}

	private static List<FlaskEntry> GetDefaultUniqueFlaskList()
	{
		List<FlaskEntry> source = new List<FlaskEntry>
		{
			new FlaskEntry("Atziri's Promise"),
			new FlaskEntry("Coralito's Signature"),
			new FlaskEntry("Coruscating Elixir"),
			new FlaskEntry("Divination Distillate"),
			new FlaskEntry("Dying Sun"),
			new FlaskEntry("Forbidden Taste"),
			new FlaskEntry("Kiara's Determination"),
			new FlaskEntry("Lion's Roar"),
			new FlaskEntry("The Overflowing Chalice"),
			new FlaskEntry("Rumi's Concoction"),
			new FlaskEntry("Sin's Rebirth"),
			new FlaskEntry("The Sorrow of the Divine"),
			new FlaskEntry("Rotgut"),
			new FlaskEntry("Soul Catcher"),
			new FlaskEntry("Taste of Hate"),
			new FlaskEntry("Vessel of Vinktar"),
			new FlaskEntry("The Wise Oak"),
			new FlaskEntry("Witchfire Brew"),
			new FlaskEntry("Cinderswallow Urn"),
			new FlaskEntry("Bottled Faith"),
			new FlaskEntry("The Writhing Jar"),
			new FlaskEntry("Progenesis"),
			new FlaskEntry("Replica Sorrow of the Divine")
		};
		return source.OrderBy((FlaskEntry a) => a.Name).ToList();
	}

	private static void InitFlaskList(ref List<FlaskEntry> jsonList, Func<List<FlaskEntry>> getDefaulList)
	{
		if (jsonList.Count == 0)
		{
			jsonList = getDefaulList();
			return;
		}
		List<FlaskEntry> list = getDefaulList();
		foreach (FlaskEntry flaskEntry_0 in list)
		{
			FlaskEntry flaskEntry = jsonList.Find((FlaskEntry f) => f.Name == flaskEntry_0.Name);
			if (flaskEntry != null)
			{
				flaskEntry_0.Triggers = flaskEntry.Triggers;
			}
		}
		jsonList = list;
	}

	static Settings()
	{
		TriggerTypes = new TriggerType[5]
		{
			TriggerType.Hp,
			TriggerType.Es,
			TriggerType.Mobs,
			TriggerType.Attack,
			TriggerType.Always
		};
		Rarity[] array = new Rarity[4];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		Rarities = (Rarity[])(object)array;
	}
}
