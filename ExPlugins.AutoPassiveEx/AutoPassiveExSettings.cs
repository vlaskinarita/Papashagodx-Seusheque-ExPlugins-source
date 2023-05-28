using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;
using JetBrains.Annotations;

namespace ExPlugins.AutoPassiveEx;

public class AutoPassiveExSettings : JsonSettings
{
	public class PassiveEntry : INotifyPropertyChanged
	{
		private ushort ushort_0;

		private string string_0;

		private int int_0;

		public int Number
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				OnPropertyChanged("Number");
			}
		}

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

		public ushort Id
		{
			get
			{
				return ushort_0;
			}
			set
			{
				ushort_0 = value;
				OnPropertyChanged("Id");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public PassiveEntry()
		{
		}

		public PassiveEntry(int number, string name, ushort id)
		{
			Number = number;
			Name = name;
			Id = id;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static AutoPassiveExSettings autoPassiveExSettings_0;

	private bool bool_0;

	private bool bool_1;

	private List<PassiveEntry> list_0 = new List<PassiveEntry>();

	private List<PassiveEntry> list_1 = new List<PassiveEntry>();

	private int int_0;

	public static AutoPassiveExSettings Instance => autoPassiveExSettings_0 ?? (autoPassiveExSettings_0 = new AutoPassiveExSettings());

	[DefaultValue(false)]
	public bool AllocateOnlyInTown
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => AllocateOnlyInTown));
			}
		}
	}

	[DefaultValue(100)]
	public int DisabledAtLevel
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
				((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => DisabledAtLevel));
			}
		}
	}

	public bool AddNode
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => AddNode));
			}
		}
	}

	public List<PassiveEntry> CharacterPassives
	{
		get
		{
			return list_1;
		}
		set
		{
			if (value != list_1)
			{
				list_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<List<PassiveEntry>>((Expression<Func<List<PassiveEntry>>>)(() => CharacterPassives));
			}
		}
	}

	public List<PassiveEntry> AtlasPassives
	{
		get
		{
			return list_0;
		}
		set
		{
			if (value != list_0)
			{
				list_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<List<PassiveEntry>>((Expression<Func<List<PassiveEntry>>>)(() => AtlasPassives));
			}
		}
	}

	private AutoPassiveExSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"AutoPassiveEx.json"
		}))
	{
	}

	public void AddCharacterPoint(DatPassiveSkillWrapper passive)
	{
		int num = ((!CharacterPassives.Any() || CharacterPassives.First() == null) ? 1 : (CharacterPassives.First().Number + 1));
		PassiveEntry passiveEntry_0 = new PassiveEntry(num, passive.Name, (ushort)passive.PassiveId);
		if (!CharacterPassives.Any((PassiveEntry x) => x.Id == passiveEntry_0.Id))
		{
			List<PassiveEntry> source = new List<PassiveEntry>(CharacterPassives) { passiveEntry_0 };
			CharacterPassives = source.OrderByDescending((PassiveEntry p) => p.Number).ToList();
			GlobalLog.Warn($"[AutoPassiveEx(Character)] {passive.Name} [{passive.PassiveId}] Added to the list with number {num}");
		}
		else
		{
			GlobalLog.Error($"[AutoPassiveEx(Character)] {passive.Name} [{passive.PassiveId}] is already added. Number {CharacterPassives.First((PassiveEntry x) => x.Id == passiveEntry_0.Id).Number}.");
		}
	}

	public void AddAtlasPoint(DatPassiveSkillWrapper passive)
	{
		int num = ((!AtlasPassives.Any() || AtlasPassives.First() == null) ? 1 : (AtlasPassives.First().Number + 1));
		PassiveEntry passiveEntry_0 = new PassiveEntry(num, passive.Name, (ushort)passive.PassiveId);
		if (AtlasPassives.Any((PassiveEntry x) => x.Id == passiveEntry_0.Id))
		{
			GlobalLog.Error($"[AutoPassiveEx(Atlas)] {passive.Name} [{passive.PassiveId}] is already added. Number {AtlasPassives.First((PassiveEntry x) => x.Id == passiveEntry_0.Id).Number}.");
			return;
		}
		List<PassiveEntry> source = new List<PassiveEntry>(AtlasPassives) { passiveEntry_0 };
		AtlasPassives = source.OrderByDescending((PassiveEntry p) => p.Number).ToList();
		GlobalLog.Warn($"[AutoPassiveEx(Atlas)] {passive.Name} [{passive.PassiveId}] Added to the list with number {num}.");
	}
}
