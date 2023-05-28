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
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using Newtonsoft.Json;

namespace ExPlugins.BasicGemLeveler;

public class BasicGemLevelerSettings : JsonSettings
{
	public class SkillGemEntry
	{
		public InventorySlot InventorySlot;

		public string Name;

		public int SocketIndex;

		[CompilerGenerated]
		private readonly string string_0;

		public string SerializationString
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public Item InventoryItem => (from ui in UsableInventories
			where ui.PageSlot == InventorySlot
			select ui.Items.FirstOrDefault()).FirstOrDefault();

		public Item SkillGem
		{
			get
			{
				Item inventoryItem = InventoryItem;
				if (!((RemoteMemoryObject)(object)inventoryItem == (RemoteMemoryObject)null) && !((RemoteMemoryObject)(object)inventoryItem.Components.SocketsComponent == (RemoteMemoryObject)null))
				{
					Item val = inventoryItem.SocketedGems[SocketIndex];
					if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null))
					{
						if (!(val.Name != Name))
						{
							return val;
						}
						return null;
					}
					return null;
				}
				return null;
			}
		}

		public SkillGemEntry(string name, InventorySlot slot, int socketIndex)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Name = name;
			InventorySlot = slot;
			SocketIndex = socketIndex;
			string_0 = $"{Name} [{InventorySlot}: {SocketIndex}]";
		}
	}

	private static BasicGemLevelerSettings basicGemLevelerSettings_0;

	private bool bool_0;

	private bool bool_1;

	private ObservableCollection<string> observableCollection_0 = new ObservableCollection<string>();

	private ObservableCollection<string> observableCollection_1 = new ObservableCollection<string>();

	public static BasicGemLevelerSettings Instance => basicGemLevelerSettings_0 ?? (basicGemLevelerSettings_0 = new BasicGemLevelerSettings());

	[DefaultValue(false)]
	public bool DebugStatements
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => DebugStatements));
			}
		}
	}

	public ObservableCollection<string> GlobalNameIgnoreList
	{
		get
		{
			return observableCollection_0;
		}
		set
		{
			if (!value.Equals(observableCollection_0))
			{
				observableCollection_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => GlobalNameIgnoreList));
			}
		}
	}

	public ObservableCollection<string> SkillGemsToLevelList
	{
		get
		{
			return observableCollection_1;
		}
		set
		{
			if (!value.Equals(observableCollection_1))
			{
				observableCollection_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => SkillGemsToLevelList));
			}
		}
	}

	[JsonIgnore]
	public ObservableCollection<string> AllSkillGems => new ObservableCollection<string>(UserSkillGems.Select((SkillGemEntry sge) => sge.SerializationString).ToList());

	[JsonIgnore]
	public ObservableCollection<SkillGemEntry> UserSkillGems
	{
		get
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			ObservableCollection<SkillGemEntry> observableCollection = new ObservableCollection<SkillGemEntry>();
			if (!LokiPoe.IsInGame)
			{
				return observableCollection;
			}
			foreach (Inventory usableInventory in UsableInventories)
			{
				foreach (Item item in usableInventory.Items)
				{
					if ((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null || (RemoteMemoryObject)(object)item.Components.SocketsComponent == (RemoteMemoryObject)null)
					{
						continue;
					}
					for (int i = 0; i < item.SocketedGems.Length; i++)
					{
						Item val = item.SocketedGems[i];
						if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null))
						{
							observableCollection.Add(new SkillGemEntry(val.Name, usableInventory.PageSlot, i));
						}
					}
				}
			}
			return observableCollection;
		}
	}

	public bool AutoLevel
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
				((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => AutoLevel));
			}
		}
	}

	private static IEnumerable<Inventory> UsableInventories => (IEnumerable<Inventory>)(object)new Inventory[11]
	{
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)2),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)3),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)14),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)15),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)4),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)1),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)8),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)9),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)6),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)7),
		InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)5)
	};

	public BasicGemLevelerSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"BasicGemLevelerSettings.json"
		}))
	{
	}

	public void RefreshSkillGemsList()
	{
		ObservableCollection<SkillGemEntry> userSkillGems = UserSkillGems;
		int x = 0;
		while (x < SkillGemsToLevelList.Count)
		{
			SkillGemEntry skillGemEntry = userSkillGems.FirstOrDefault((SkillGemEntry sge) => sge.SerializationString == SkillGemsToLevelList[x]);
			if (skillGemEntry == null)
			{
				SkillGemsToLevelList.RemoveAt(x);
				continue;
			}
			int num = x + 1;
			x = num;
		}
		((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<SkillGemEntry>>((Expression<Func<ObservableCollection<SkillGemEntry>>>)(() => UserSkillGems));
		((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => AllSkillGems));
		((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => SkillGemsToLevelList));
	}

	public void UpdateGlobalNameIgnoreList()
	{
		((NotificationObject)this).NotifyPropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => GlobalNameIgnoreList));
	}
}
