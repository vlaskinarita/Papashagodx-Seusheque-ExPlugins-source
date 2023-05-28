using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using Newtonsoft.Json;

namespace ExPlugins.AutoLoginEx;

public class AutoLoginSettings : JsonSettings
{
	private static AutoLoginSettings autoLoginSettings_0;

	[JsonIgnore]
	public static readonly CharacterClass[] CharacterValues;

	[JsonIgnore]
	public static readonly int[] DummyValues;

	private bool bool_0;

	private string string_0;

	private ObservableCollection<string> observableCollection_0 = new ObservableCollection<string>();

	private bool bool_1;

	private bool bool_2;

	private string okiRhyOdvg;

	private bool bool_3;

	private bool bool_4;

	private int int_0;

	private string string_1;

	[JsonIgnore]
	private string string_2;

	private string string_3;

	private CharacterClass characterClass_0;

	private GatewayEnum gatewayEnum_0;

	private TimeSpan timeSpan_0;

	private int int_1;

	private bool bool_5;

	private bool bool_6;

	private DateTime dateTime_0;

	private string string_4;

	private int int_2;

	private TimeSpan timeSpan_1;

	[JsonIgnore]
	private string string_5;

	[CompilerGenerated]
	private string string_6;

	[CompilerGenerated]
	private int int_3;

	[CompilerGenerated]
	private ObservableCollection<DummyEntry> observableCollection_1 = new ObservableCollection<DummyEntry>();

	[CompilerGenerated]
	private ObservableCollection<CharacterEntry> observableCollection_2 = new ObservableCollection<CharacterEntry>();

	public string LastLeague
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

	public static AutoLoginSettings Instance => autoLoginSettings_0 ?? (autoLoginSettings_0 = new AutoLoginSettings());

	[DefaultValue("")]
	public string Poesessid
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Poesessid));
		}
	}

	[DefaultValue(false)]
	public bool LoginUsingGateway
	{
		get
		{
			return bool_5;
		}
		set
		{
			if (!value.Equals(bool_5))
			{
				bool_5 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => LoginUsingGateway));
			}
		}
	}

	[DefaultValue(0)]
	public GatewayEnum Gate
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return gatewayEnum_0;
		}
		set
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (!((object)(GatewayEnum)(ref value)).Equals((object)gatewayEnum_0))
			{
				gatewayEnum_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<GatewayEnum>((Expression<Func<GatewayEnum>>)(() => Gate));
			}
		}
	}

	[DefaultValue(true)]
	public bool LoginUsingUserCredentials
	{
		get
		{
			return bool_6;
		}
		set
		{
			if (!value.Equals(bool_6))
			{
				bool_6 = value;
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => LoginUsingUserCredentials));
			}
		}
	}

	[DefaultValue("")]
	public string Email
	{
		get
		{
			return string_3;
		}
		set
		{
			if (!value.Equals(string_3))
			{
				string_3 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Email));
			}
		}
	}

	[DefaultValue("")]
	public string Password
	{
		get
		{
			return string_4;
		}
		set
		{
			if (!value.Equals(string_4))
			{
				string_4 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Password));
			}
		}
	}

	[DefaultValue(false)]
	public bool DelayBeforeLoginAttempt
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DelayBeforeLoginAttempt));
			}
		}
	}

	public TimeSpan LoginAttemptDelay
	{
		get
		{
			return timeSpan_0;
		}
		set
		{
			if (!value.Equals(timeSpan_0))
			{
				timeSpan_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<TimeSpan>((Expression<Func<TimeSpan>>)(() => LoginAttemptDelay));
			}
		}
	}

	[DefaultValue(true)]
	public bool AutoSelectCharacter
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => AutoSelectCharacter));
			}
		}
	}

	public ObservableCollection<string> Characters
	{
		get
		{
			return observableCollection_0;
		}
		set
		{
			observableCollection_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => Characters));
		}
	}

	[DefaultValue("Character")]
	public string Character
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
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Character));
			}
		}
	}

	[DefaultValue(false)]
	public bool DelayBeforeSelectingCharacter
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DelayBeforeSelectingCharacter));
			}
		}
	}

	public TimeSpan SelectCharacterDelay
	{
		get
		{
			return timeSpan_1;
		}
		set
		{
			if (!value.Equals(timeSpan_1))
			{
				timeSpan_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<TimeSpan>((Expression<Func<TimeSpan>>)(() => SelectCharacterDelay));
			}
		}
	}

	[DefaultValue(10)]
	public int MaxLoginAttempts
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

	[JsonIgnore]
	public DateTime NextLoginTime
	{
		get
		{
			return dateTime_0;
		}
		set
		{
			if (!value.Equals(dateTime_0))
			{
				dateTime_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<DateTime>((Expression<Func<DateTime>>)(() => NextLoginTime));
			}
		}
	}

	[JsonIgnore]
	public int LoginAttempts
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
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => LoginAttempts));
			}
		}
	}

	[JsonIgnore]
	public int SelectCharacterAttempts
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
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => SelectCharacterAttempts));
			}
		}
	}

	[DefaultValue("")]
	public string DummyCharacter
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => DummyCharacter));
		}
	}

	[DefaultValue(0)]
	public int DummyAmount
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = ((value <= 4) ? value : 4);
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => DummyAmount));
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public CharacterClass FinalClass
	{
		get
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(okiRhyOdvg))
			{
				string path = Path.Combine(Configuration.Instance.Path, "class.txt");
				if (!File.Exists(path))
				{
					okiRhyOdvg = ((object)(CharacterClass)(ref characterClass_0)).ToString();
					return characterClass_0;
				}
				try
				{
					string value = File.ReadAllText(path);
					CharacterClass result = (FinalClass = (CharacterClass)Enum.Parse(typeof(CharacterClass), value));
					okiRhyOdvg = ((object)(CharacterClass)(ref characterClass_0)).ToString();
					return result;
				}
				catch
				{
				}
			}
			okiRhyOdvg = ((object)(CharacterClass)(ref characterClass_0)).ToString();
			return characterClass_0;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			characterClass_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<CharacterClass>((Expression<Func<CharacterClass>>)(() => FinalClass));
		}
	}

	public ObservableCollection<DummyEntry> DummySetup
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

	public ObservableCollection<CharacterEntry> CharacterSetup
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

	[DefaultValue(true)]
	public bool CreateDummiesOnStandard
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => CreateDummiesOnStandard));
		}
	}

	[DefaultValue(false)]
	public bool CreateMainCharOnStandard
	{
		get
		{
			return bool_2;
		}
		set
		{
			bool_2 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => CreateMainCharOnStandard));
		}
	}

	[DefaultValue("")]
	public string UnlockCode
	{
		get
		{
			return string_5;
		}
		set
		{
			string_5 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => UnlockCode));
		}
	}

	public AutoLoginSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[1] { "AutoLoginEx.json" }))
	{
		NextLoginTime = DateTime.Now;
		if (LoginAttemptDelay == default(TimeSpan))
		{
			LoginAttemptDelay = TimeSpan.FromMilliseconds(LokiPoe.Random.Next(500, 1000));
		}
		if (SelectCharacterDelay == default(TimeSpan))
		{
			SelectCharacterDelay = TimeSpan.FromMilliseconds(LokiPoe.Random.Next(500, 1000));
		}
		if (DummySetup == null)
		{
			DummySetup = new ObservableCollection<DummyEntry>
			{
				new DummyEntry(2, 2)
			};
		}
		if (CharacterSetup == null)
		{
			CharacterSetup = new ObservableCollection<CharacterEntry>();
		}
	}

	static AutoLoginSettings()
	{
		CharacterClass[] array = new CharacterClass[8];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		CharacterValues = (CharacterClass[])(object)array;
		DummyValues = new int[5] { 0, 1, 2, 3, 4 };
	}
}
