using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using ExPlugins.EquipPluginEx.Classes;
using JetBrains.Annotations;
using Newtonsoft.Json;

internal class Class27 : JsonSettings
{
	public class Class28 : INotifyPropertyChanged
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

		public Class28()
		{
		}

		public Class28(string name)
		{
			Name = name;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static Class27 class27_0;

	[JsonIgnore]
	public static List<string> CustomFlaskTypes;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private int int_1;

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
	private ObservableCollection<Class28> observableCollection_0 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> observableCollection_1 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> observableCollection_2 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> otYrvqxafk = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> observableCollection_3 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> observableCollection_4 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private ObservableCollection<Class28> observableCollection_5 = new ObservableCollection<Class28>();

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private string string_3;

	[CompilerGenerated]
	private string string_4;

	[CompilerGenerated]
	private string string_5;

	[CompilerGenerated]
	private string string_6;

	[CompilerGenerated]
	private string string_7;

	[CompilerGenerated]
	private string string_8;

	[CompilerGenerated]
	private string string_9;

	[CompilerGenerated]
	private string string_10;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_0;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_1;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_2;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_3;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_4;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_5;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_6;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_7;

	[CompilerGenerated]
	private ItemPrioritiesClass itemPrioritiesClass_8;

	public static Class27 Instance => class27_0 ?? (class27_0 = new Class27());

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

	public int LevelToStopPickUpAndEquip
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

	public bool ShouldUpgradeMagicToMagic
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

	public int LevelToStopCheckingVendorsForGems
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

	public bool ShouldEquipMainHand
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

	public bool ShouldEquipBows
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

	public bool ShouldEquipDaggers
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

	public bool ShouldEquipClaws
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

	public bool ShouldEquipSceptres
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

	public bool ShouldEquipOneHandedSwords
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

	public bool ShouldEquipOneHandedMaces
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

	public bool ShouldEquipOneHandedAxes
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

	public bool ShouldEquipWands
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

	public bool ShouldEquipWarstaffs
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

	public bool ShouldEquipStaffs
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

	public bool ShouldEquipTwoHandedSwords
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

	public bool ShouldEquipTwoHandedMaces
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

	public bool ShouldEquipTwoHandedAxes
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

	public bool ShouldEquipOffHand
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

	public bool ShouldEquipWeaponInOffHand
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

	public bool ShouldEquipQuiver
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

	public bool ShouldEquipShield
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

	public bool ShouldEquipHelmet
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

	public bool ShouldEquipBodyArmour
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

	public bool ShouldEquipGloves
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

	public bool ShouldEquipBoots
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

	public bool ShouldEquipBelt
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

	public bool ShouldEquipRings
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

	public bool ShouldEquipAmulet
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

	public bool ShouldEquipFlasks
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

	public ObservableCollection<Class28> UniquesToNeverEquip
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

	public ObservableCollection<Class28> HelmetGems
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

	public ObservableCollection<Class28> BodyArmourGems
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

	public ObservableCollection<Class28> GlovesGems
	{
		[CompilerGenerated]
		get
		{
			return otYrvqxafk;
		}
		[CompilerGenerated]
		set
		{
			otYrvqxafk = value;
		}
	}

	public ObservableCollection<Class28> BootsGems
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

	public ObservableCollection<Class28> MainHandGems
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

	public ObservableCollection<Class28> OffHandGems
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

	[DefaultValue("BBB")]
	public string MainHandColors
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

	[DefaultValue("BBB")]
	public string OffHandColors
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

	[DefaultValue("BBB")]
	public string HelmetColors
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

	[DefaultValue("BBB")]
	public string BodyArmourColors
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

	[DefaultValue("BBB")]
	public string GlovesColors
	{
		[CompilerGenerated]
		get
		{
			return string_4;
		}
		[CompilerGenerated]
		set
		{
			string_4 = value;
		}
	}

	[DefaultValue("BBB")]
	public string BootsColors
	{
		[CompilerGenerated]
		get
		{
			return string_5;
		}
		[CompilerGenerated]
		set
		{
			string_5 = value;
		}
	}

	public string FlaskSlot1
	{
		[CompilerGenerated]
		get
		{
			return string_6;
		}
		[CompilerGenerated]
		set
		{
			string_6 = value;
		}
	}

	public string FlaskSlot2
	{
		[CompilerGenerated]
		get
		{
			return string_7;
		}
		[CompilerGenerated]
		set
		{
			string_7 = value;
		}
	}

	public string FlaskSlot3
	{
		[CompilerGenerated]
		get
		{
			return string_8;
		}
		[CompilerGenerated]
		set
		{
			string_8 = value;
		}
	}

	public string FlaskSlot4
	{
		[CompilerGenerated]
		get
		{
			return string_9;
		}
		[CompilerGenerated]
		set
		{
			string_9 = value;
		}
	}

	public string FlaskSlot5
	{
		[CompilerGenerated]
		get
		{
			return string_10;
		}
		[CompilerGenerated]
		set
		{
			string_10 = value;
		}
	}

	public ItemPrioritiesClass HelmetPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_0;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_0 = value;
		}
	}

	public ItemPrioritiesClass BodyArmourPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_1;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_1 = value;
		}
	}

	public ItemPrioritiesClass GlovesPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_2;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_2 = value;
		}
	}

	public ItemPrioritiesClass BootsPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_3;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_3 = value;
		}
	}

	public ItemPrioritiesClass MainHandPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_4;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_4 = value;
		}
	}

	public ItemPrioritiesClass OffHandPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_5;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_5 = value;
		}
	}

	public ItemPrioritiesClass BeltPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_6;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_6 = value;
		}
	}

	public ItemPrioritiesClass AmuletPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_7;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_7 = value;
		}
	}

	public ItemPrioritiesClass RingsPriorities
	{
		[CompilerGenerated]
		get
		{
			return itemPrioritiesClass_8;
		}
		[CompilerGenerated]
		set
		{
			itemPrioritiesClass_8 = value;
		}
	}

	private Class27()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"EquipPluginEx.json"
		}))
	{
		UniquesToNeverEquip = new ObservableCollection<Class28>(from s in UniquesToNeverEquip
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		HelmetGems = new ObservableCollection<Class28>(from s in HelmetGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		BodyArmourGems = new ObservableCollection<Class28>(from s in BodyArmourGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		GlovesGems = new ObservableCollection<Class28>(from s in GlovesGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		BootsGems = new ObservableCollection<Class28>(from s in BootsGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		MainHandGems = new ObservableCollection<Class28>(from s in MainHandGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		OffHandGems = new ObservableCollection<Class28>(from s in OffHandGems
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		if (UniquesToNeverEquip == null || UniquesToNeverEquip.Count < 1)
		{
			UniquesToNeverEquip = new ObservableCollection<Class28>
			{
				new Class28("Veil of the Night")
			};
		}
		if (HelmetPriorities == null)
		{
			HelmetPriorities = new ItemPrioritiesClass();
		}
		if (BodyArmourPriorities == null)
		{
			BodyArmourPriorities = new ItemPrioritiesClass();
		}
		if (GlovesPriorities == null)
		{
			GlovesPriorities = new ItemPrioritiesClass();
		}
		if (BootsPriorities == null)
		{
			BootsPriorities = new ItemPrioritiesClass();
		}
		if (MainHandPriorities == null)
		{
			MainHandPriorities = new ItemPrioritiesClass();
		}
		if (OffHandPriorities == null)
		{
			OffHandPriorities = new ItemPrioritiesClass();
		}
		if (BeltPriorities == null)
		{
			BeltPriorities = new ItemPrioritiesClass();
		}
		if (AmuletPriorities == null)
		{
			AmuletPriorities = new ItemPrioritiesClass();
		}
		if (RingsPriorities == null)
		{
			RingsPriorities = new ItemPrioritiesClass();
		}
	}

	static Class27()
	{
		CustomFlaskTypes = new List<string>
		{
			"Life", "Mana", "Hybrid", "Quicksilver", "Quartz", "Granite", "Basalt", "Jade", "Stibnite", "Sulphur",
			"None"
		};
	}
}
