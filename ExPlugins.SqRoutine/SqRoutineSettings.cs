using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using DreamPoeBot.Loki.Game.GameData;
using JetBrains.Annotations;

namespace ExPlugins.SqRoutine;

public class SqRoutineSettings : JsonSettings
{
	public enum TotemUsageCase
	{
		MaxTotems,
		BuffOnly,
		DontUse
	}

	public class TotemSkillEntry : INotifyPropertyChanged
	{
		private Rarity rarity_0;

		private TotemUsageCase totemUsageCase_0;

		private bool bool_0;

		private string string_0;

		[CompilerGenerated]
		private DateTime dateTime_0;

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

		public Rarity MinRarity
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return rarity_0;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				rarity_0 = value;
				OnPropertyChanged("MinRarity");
			}
		}

		public TotemUsageCase UsageCase
		{
			get
			{
				return totemUsageCase_0;
			}
			set
			{
				totemUsageCase_0 = value;
				OnPropertyChanged("UsageCase");
			}
		}

		public bool Every3Sec
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				OnPropertyChanged("Every3Sec");
			}
		}

		public DateTime LastUsed
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

		public event PropertyChangedEventHandler PropertyChanged;

		public TotemSkillEntry(string name, Rarity rarity, TotemUsageCase usageCase, bool bool_1)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			string_0 = name;
			rarity_0 = rarity;
			totemUsageCase_0 = usageCase;
			bool_0 = bool_1;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class VaalSkillEntry : INotifyPropertyChanged
	{
		private Rarity rarity_0;

		private string string_0;

		private bool bool_0;

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

		public Rarity MinRarity
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return rarity_0;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				rarity_0 = value;
				OnPropertyChanged("MinRarity");
			}
		}

		public bool SoulEater
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				OnPropertyChanged("SoulEater");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public VaalSkillEntry(string name, Rarity rarity, bool soulEater)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			string_0 = name;
			rarity_0 = rarity;
			bool_0 = soulEater;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static SqRoutineSettings sqRoutineSettings_0;

	public static List<Rarity> RarityList;

	public static List<TotemUsageCase> CasesList;

	private string string_0;

	private bool bool_0;

	private bool bool_1;

	private bool bool_2;

	private string string_1;

	private bool bool_3;

	private bool bool_4;

	private int int_0;

	private bool bool_5;

	private bool bool_6;

	private bool bool_7;

	[CompilerGenerated]
	private bool bool_8;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private ObservableCollection<VaalSkillEntry> observableCollection_0 = new ObservableCollection<VaalSkillEntry>();

	[CompilerGenerated]
	private ObservableCollection<TotemSkillEntry> gaoGnjfZd8 = new ObservableCollection<TotemSkillEntry>();

	public static SqRoutineSettings Instance => sqRoutineSettings_0 ?? (sqRoutineSettings_0 = new SqRoutineSettings());

	[DefaultValue(false)]
	public bool DebugMode
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

	[DefaultValue(1)]
	public int CursesAllowed
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

	[DefaultValue(70)]
	public int CombatRange
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

	[DefaultValue(70)]
	public int MaxRangeRange
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

	public ObservableCollection<VaalSkillEntry> VaalSkillsList
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

	public ObservableCollection<TotemSkillEntry> TotemSkillsList
	{
		[CompilerGenerated]
		get
		{
			return gaoGnjfZd8;
		}
		[CompilerGenerated]
		set
		{
			gaoGnjfZd8 = value;
		}
	}

	[DefaultValue(true)]
	public bool DefaultSkillEnabled
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DefaultSkillEnabled));
		}
	}

	[DefaultValue("Absolution")]
	public string DefaultSkill
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => DefaultSkill));
		}
	}

	[DefaultValue(true)]
	public bool RarePlusSkillEnabled
	{
		get
		{
			return bool_3;
		}
		set
		{
			bool_3 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => RarePlusSkillEnabled));
		}
	}

	[DefaultValue("Power Siphon")]
	public string RarePlusSkill
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => RarePlusSkill));
		}
	}

	[DefaultValue(false)]
	public bool LootInCombat
	{
		get
		{
			return bool_2;
		}
		set
		{
			bool_2 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => LootInCombat));
		}
	}

	[DefaultValue(false)]
	public bool SkipNormalMobs
	{
		get
		{
			return bool_5;
		}
		set
		{
			bool_5 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => SkipNormalMobs));
		}
	}

	[DefaultValue(false)]
	public bool KiteMobs
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => KiteMobs));
		}
	}

	[DefaultValue(false)]
	public bool SaveAnime
	{
		get
		{
			return bool_4;
		}
		set
		{
			bool_4 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => SaveAnime));
		}
	}

	[DefaultValue(10)]
	public int SaveAnimePct
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => SaveAnimePct));
		}
	}

	[DefaultValue(true)]
	public bool DropBanners
	{
		get
		{
			return bool_7;
		}
		set
		{
			bool_7 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DropBanners));
		}
	}

	[DefaultValue(false)]
	public bool SwitchWeapons
	{
		get
		{
			return bool_6;
		}
		set
		{
			bool_6 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => SwitchWeapons));
		}
	}

	public SqRoutineSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"SqRoutineSettings.json"
		}))
	{
	}

	static SqRoutineSettings()
	{
		RarityList = new List<Rarity>
		{
			(Rarity)0,
			(Rarity)1,
			(Rarity)2,
			(Rarity)3
		};
		CasesList = new List<TotemUsageCase>
		{
			TotemUsageCase.BuffOnly,
			TotemUsageCase.MaxTotems,
			TotemUsageCase.DontUse
		};
	}
}
