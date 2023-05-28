using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using ExPlugins.BlightPluginEx.Classes;
using Newtonsoft.Json;

internal class Class53 : JsonSettings
{
	private static Class53 class53_0;

	[JsonIgnore]
	public static List<string> AllDpsOptions;

	[JsonIgnore]
	public static List<string> AllControlOptions;

	public static WeightedTower weightedTower_0;

	private int int_0;

	private int int_1 = 0;

	private int int_2;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private bool bool_4;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private bool bool_5;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private bool bool_7;

	[CompilerGenerated]
	private bool bool_8;

	[CompilerGenerated]
	private List<string> list_0;

	public static Class53 Instance => class53_0 ?? (class53_0 = new Class53());

	[DefaultValue(100)]
	public int TowerUpgradeDistance
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
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => TowerUpgradeDistance));
			}
		}
	}

	[DefaultValue(8)]
	public int TowerUpgradeTimeout
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
	public bool PrioritizeTowerConstruction
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

	[DefaultValue(false)]
	public bool PrioritizeBosses
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
	public bool DebugMode
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

	[DefaultValue("Both")]
	public string DefaultControlOnBlightedMaps
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

	[DefaultValue("Stun")]
	public string DefaultControlOnRegularMaps
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

	[DefaultValue(true)]
	public bool BuildEmpowerTowersOnBlightedMaps
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
	public bool BuildEmpowerTowersOnRegularMaps
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

	[DefaultValue("Meteor")]
	public string DpsOption
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

	[DefaultValue(999)]
	public int MaxToBuild
	{
		get
		{
			return int_2;
		}
		set
		{
			if (!value.Equals(int_2))
			{
				int_2 = value;
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => MaxToBuild));
			}
		}
	}

	[JsonIgnore]
	public bool BuildStunTowersBlighted
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

	[JsonIgnore]
	public bool BuildChillTowersBlighted
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

	[JsonIgnore]
	public bool BuildStunTowersRegular
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

	[JsonIgnore]
	public bool BuildChillTowersRegular
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

	[JsonIgnore]
	public List<string> UpgradeOpt
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

	[JsonIgnore]
	public int UpgradedTowers
	{
		get
		{
			return int_1;
		}
		set
		{
			if (!value.Equals(int_1))
			{
				int_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => UpgradedTowers));
			}
		}
	}

	private Class53()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"BlightPluginEx.json"
		}))
	{
	}

	public void SelectDpsTower()
	{
		switch (DpsOption)
		{
		case "None":
			UpgradeOpt = new List<string> { "Pidor" };
			return;
		case "Arc":
			UpgradeOpt = new List<string> { "ShockingTower1", "ShockingTower2", "ShockingTower3", "ArcingTower" };
			return;
		case "Meteor":
			UpgradeOpt = new List<string> { "FlameTower1", "FlameTower2", "FlameTower3", "MeteorTower" };
			return;
		case "Minion":
			UpgradeOpt = new List<string> { "MinionTower1", "MinionTower2", "MinionTower3", "FlyingMinionTower" };
			return;
		}
		if (string.IsNullOrEmpty(DpsOption))
		{
			UpgradeOpt = new List<string> { "MinionTower1", "MinionTower2", "MinionTower3", "FlyingMinionTower" };
		}
	}

	public void SelectRegularControlTower()
	{
		switch (DefaultControlOnRegularMaps)
		{
		case "Stun":
			BuildStunTowersRegular = true;
			BuildChillTowersRegular = false;
			break;
		case "Chill":
			BuildStunTowersRegular = false;
			BuildChillTowersRegular = true;
			break;
		case "Nothing":
			BuildStunTowersRegular = false;
			BuildChillTowersRegular = false;
			break;
		case "Both":
			BuildStunTowersRegular = true;
			BuildChillTowersRegular = true;
			break;
		}
	}

	public void SelectBlightControlTower()
	{
		switch (DefaultControlOnBlightedMaps)
		{
		case "Chill":
			BuildStunTowersBlighted = false;
			BuildChillTowersBlighted = true;
			break;
		case "Nothing":
			BuildStunTowersBlighted = false;
			BuildChillTowersBlighted = false;
			break;
		case "Both":
			BuildStunTowersBlighted = true;
			BuildChillTowersBlighted = true;
			break;
		case "Stun":
			BuildStunTowersBlighted = true;
			BuildChillTowersBlighted = false;
			break;
		}
	}

	static Class53()
	{
		AllDpsOptions = new List<string> { "Minion", "Meteor", "Arc", "None" };
		AllControlOptions = new List<string> { "Stun", "Chill", "Both", "Nothing" };
	}
}
