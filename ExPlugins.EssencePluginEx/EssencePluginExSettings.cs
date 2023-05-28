using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using JetBrains.Annotations;

namespace ExPlugins.EssencePluginEx;

public class EssencePluginExSettings : JsonSettings
{
	public class EssenceEntry : INotifyPropertyChanged
	{
		private string string_0;

		private string string_1;

		private bool bool_0;

		private bool bool_1;

		public string Tier
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				OnPropertyChanged("Tier");
			}
		}

		public string Type
		{
			get
			{
				return string_1;
			}
			set
			{
				string_1 = value;
				OnPropertyChanged("Type");
			}
		}

		public bool Open
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				OnPropertyChanged("Open");
			}
		}

		public bool Corrupt
		{
			get
			{
				return bool_1;
			}
			set
			{
				bool_1 = value;
				OnPropertyChanged("Corrupt");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public EssenceEntry()
		{
		}

		public EssenceEntry(string tier, string type, bool open, bool corrupt)
		{
			Tier = tier;
			Type = type;
			Open = open;
			Corrupt = corrupt;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static EssencePluginExSettings essencePluginExSettings_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private int int_2;

	private ObservableCollection<EssenceEntry> observableCollection_0 = new ObservableCollection<EssenceEntry>();

	public static readonly List<string> EssenceTiers;

	public static readonly List<string> EssenceTypes;

	public static EssencePluginExSettings Instance => essencePluginExSettings_0 ?? (essencePluginExSettings_0 = new EssencePluginExSettings());

	[DefaultValue(-1)]
	public int MinEssence
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

	[DefaultValue(10)]
	public int MaxEssence
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

	[DefaultValue(-1)]
	public int EssencesToUseRemnant
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

	public ObservableCollection<EssenceEntry> SpecificEssenceRules
	{
		get
		{
			return observableCollection_0;
		}
		set
		{
			observableCollection_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<EssenceEntry>>((Expression<Func<ObservableCollection<EssenceEntry>>>)(() => SpecificEssenceRules));
		}
	}

	public EssencePluginExSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"EssencePluginExSettings.json"
		}))
	{
		SpecificEssenceRules = new ObservableCollection<EssenceEntry>(SpecificEssenceRules.Where((EssenceEntry s) => !string.IsNullOrWhiteSpace(s.Tier) && !string.IsNullOrWhiteSpace(s.Type)));
		if (!SpecificEssenceRules.Any())
		{
			SpecificEssenceRules = new ObservableCollection<EssenceEntry>
			{
				new EssenceEntry("Screaming", "Scorn", open: true, corrupt: true),
				new EssenceEntry("Shrieking", "Scorn", open: true, corrupt: true),
				new EssenceEntry("Deafening", "Scorn", open: true, corrupt: true),
				new EssenceEntry("Screaming", "Envy", open: true, corrupt: true),
				new EssenceEntry("Shrieking", "Envy", open: true, corrupt: true),
				new EssenceEntry("Deafening", "Envy", open: true, corrupt: true),
				new EssenceEntry("Screaming", "Misery", open: true, corrupt: true),
				new EssenceEntry("Shrieking", "Misery", open: true, corrupt: true),
				new EssenceEntry("Deafening", "Misery", open: true, corrupt: true),
				new EssenceEntry("Screaming", "Dread", open: true, corrupt: true),
				new EssenceEntry("Shrieking", "Dread", open: true, corrupt: true),
				new EssenceEntry("Deafening", "Dread", open: true, corrupt: true)
			};
		}
	}

	static EssencePluginExSettings()
	{
		EssenceTiers = new List<string> { "Whispering", "Muttering", "Weeping", "Wailing", "Screaming", "Shrieking", "Deafening" };
		EssenceTypes = new List<string>
		{
			"Greed", "Contempt", "Hatred", "Woe", "Fear", "Anger", "Torment", "Sorrow", "Rage", "Suffering",
			"Wrath", "Doubt", "Loathing", "Zeal", "Loathing", "Anguish", "Spite", "Scorn", "Envy", "Misery",
			"Dread"
		};
	}
}
