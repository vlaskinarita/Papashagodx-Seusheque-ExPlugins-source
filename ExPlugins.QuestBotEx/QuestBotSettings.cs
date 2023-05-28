using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;
using Newtonsoft.Json;

namespace ExPlugins.QuestBotEx;

public class QuestBotSettings : JsonSettings
{
	public class ActGroup<T>
	{
		[CompilerGenerated]
		private readonly int int_0;

		[CompilerGenerated]
		private readonly List<T> list_0;

		internal static object object_0;

		public int Act
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
		}

		public List<T> Elements
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
		}

		public ActGroup(int act)
		{
			int_0 = act;
			list_0 = new List<T>();
		}

		internal static bool smethod_0()
		{
			return object_0 == null;
		}

		internal static object smethod_1()
		{
			return object_0;
		}
	}

	public class OptionalQuest : Quest
	{
		[CompilerGenerated]
		private bool bool_0 = true;

		public bool Enabled
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

		public OptionalQuest()
		{
		}

		public OptionalQuest(string name, string id, bool enabled)
		{
			base.Name = name;
			base.Id = id;
			Enabled = enabled;
		}

		public OptionalQuest(DatQuestWrapper quest)
			: base(quest)
		{
		}
	}

	public class Quest
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		public string Name
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

		public string Id
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

		public Quest()
		{
		}

		public Quest(string name, string id)
		{
			Name = name;
			Id = id;
		}

		public Quest(DatQuestWrapper quest)
		{
			Name = quest.Name;
			Id = quest.Id;
		}

		public static implicit operator Quest(DatQuestWrapper quest)
		{
			return new Quest(quest);
		}

		public override string ToString()
		{
			return "\"" + Name + "\" (" + Id + ")";
		}
	}

	public class GrindingRule
	{
		[CompilerGenerated]
		private Quest quest_0 = Quests.EnemyAtTheGate;

		[CompilerGenerated]
		private int int_0 = 100;

		[CompilerGenerated]
		private Area area_0 = new Area(World.Act9.BloodAqueduct);

		public Quest Quest
		{
			[CompilerGenerated]
			get
			{
				return quest_0;
			}
			[CompilerGenerated]
			set
			{
				quest_0 = value;
			}
		}

		public int LevelCap
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

		public Area GrindArea
		{
			[CompilerGenerated]
			get
			{
				return area_0;
			}
			[CompilerGenerated]
			set
			{
				area_0 = value;
			}
		}
	}

	public class Area
	{
		[CompilerGenerated]
		private readonly string string_0;

		[CompilerGenerated]
		private string string_1;

		public string Id
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public string Name
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

		public Area()
		{
		}

		public Area(AreaInfo area)
		{
			string_0 = area.Id;
			Name = area.Name;
		}
	}

	public class RewardQuest : Quest
	{
		[CompilerGenerated]
		private string string_2 = "Any";

		public string SelectedReward
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

		public RewardQuest()
		{
		}

		public RewardQuest(string name, string id)
			: base(name, id)
		{
		}

		public RewardQuest(DatQuestWrapper quest)
			: base(quest)
		{
		}

		public RewardQuest(string name, string id, string reward)
			: base(name, id)
		{
			SelectedReward = reward;
		}

		public RewardQuest(DatQuestWrapper quest, string reward)
			: base(quest)
		{
			SelectedReward = reward;
		}

		public RewardQuest(RewardQuest entry)
			: base(entry.Name, entry.Id)
		{
			SelectedReward = entry.SelectedReward;
		}
	}

	private static QuestBotSettings questBotSettings_0;

	private string string_0;

	private string string_1;

	[CompilerGenerated]
	private int int_0 = 85;

	[CompilerGenerated]
	private int int_1 = 7;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2 = true;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private bool bool_4;

	[CompilerGenerated]
	private ObservableCollection<GrindingRule> observableCollection_0 = new ObservableCollection<GrindingRule>();

	[CompilerGenerated]
	private List<OptionalQuest> list_0 = new List<OptionalQuest>();

	[CompilerGenerated]
	private List<RewardQuest> list_1 = new List<RewardQuest>();

	public static QuestBotSettings Instance => questBotSettings_0 ?? (questBotSettings_0 = new QuestBotSettings());

	[JsonIgnore]
	public string CurrentQuestName
	{
		get
		{
			return string_0;
		}
		set
		{
			if (!(value == string_0))
			{
				string_0 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => CurrentQuestName));
			}
		}
	}

	[JsonIgnore]
	public string CurrentQuestState
	{
		get
		{
			return string_1;
		}
		set
		{
			if (!(value == string_1))
			{
				string_1 = value;
				((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => CurrentQuestState));
			}
		}
	}

	public int ExplorationPercent
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

	public int MaxDeaths
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

	public bool TrackMob
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

	public bool UseHideout
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

	public bool EnterCorruptedAreas
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

	public bool TalkToQuestgivers
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

	public bool CheckGrindingFirst
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

	public ObservableCollection<GrindingRule> GrindingRules
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

	public List<OptionalQuest> OptionalQuests
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

	public List<RewardQuest> RewardQuests
	{
		[CompilerGenerated]
		get
		{
			return list_1;
		}
		[CompilerGenerated]
		set
		{
			list_1 = value;
		}
	}

	[JsonIgnore]
	public List<ActGroup<RewardQuest>> RewardQuestsByAct => GroupByAct(RewardQuests, NextRewardQuestAct);

	[JsonIgnore]
	public List<ActGroup<OptionalQuest>> OptionalQuestsByAct => GroupByAct(OptionalQuests, NextOptionalQuestAct);

	[JsonIgnore]
	public static List<Quest> QuestList
	{
		get
		{
			List<Quest> list = new List<Quest>();
			foreach (DatQuestWrapper item in Quests.All)
			{
				if (item != Quests.RibbonSpool && item != Quests.SwigOfHope && item != Quests.EndToHunger)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	[JsonIgnore]
	public static List<Area> AreaList
	{
		get
		{
			List<Area> list = new List<Area>();
			Type[] nestedTypes = typeof(World).GetNestedTypes();
			foreach (Type type in nestedTypes)
			{
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					AreaInfo areaInfo = fieldInfo.GetValue(fieldInfo) as AreaInfo;
					if (!(areaInfo == null))
					{
						string id = areaInfo.Id;
						if (!(id == World.Act1.TwilightStrand.Id) && !(id == World.Act7.MaligaroSanctum.Id) && !(id == World.Act11.TemplarLaboratory.Id) && !areaInfo.Id.Contains("town"))
						{
							list.Add(new Area(areaInfo));
						}
					}
				}
			}
			return list;
		}
	}

	private QuestBotSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"QuestBotExSettings.json"
		}))
	{
		InitOptionalQuests();
		InitRewardQuests();
	}

	public string GetRewardForQuest(string questId)
	{
		if (CustomQuestIDs().Contains(questId) && RewardQuests.Find((RewardQuest x) => x.Id == questId) == null)
		{
			AddCustomQuestIDs(RewardQuests);
		}
		RewardQuest rewardQuest = RewardQuests.Find((RewardQuest q) => q.Id == questId);
		if (rewardQuest == null)
		{
			GlobalLog.Error("[Settings][GetQuestReward] Unsupported quest id: \"" + questId + "\".");
			ErrorManager.ReportCriticalError();
			return null;
		}
		string text = rewardQuest.SelectedReward;
		if (questId == Quests.DealWithBandits.Id && text == "Any")
		{
			text = "Eramir";
		}
		return text;
	}

	private static List<ActGroup<T>> GroupByAct<T>(List<T> source, Func<T, int> nextAct)
	{
		int num = 0;
		int num2 = 1;
		ActGroup<T> item = new ActGroup<T>(1);
		List<ActGroup<T>> list = new List<ActGroup<T>> { item };
		foreach (T item2 in source)
		{
			list[num].Elements.Add(item2);
			num2 = nextAct(item2);
			if (num2 != 0)
			{
				num++;
				item = new ActGroup<T>(num2);
				list.Add(item);
			}
		}
		return list;
	}

	public bool IsQuestEnabled(DatQuestWrapper quest)
	{
		string string_0 = quest.Id;
		return OptionalQuests.Exists((OptionalQuest q) => q.Enabled && q.Id == string_0);
	}

	private static int NextOptionalQuestAct(OptionalQuest quest)
	{
		string name = quest.Name;
		if (!(name == Quests.MaroonedMariner.Name))
		{
			if (!(name == Quests.ThroughSacredGround.Name))
			{
				if (!(name == Quests.FixtureOfFate.Name))
				{
					if (name == Quests.IndomitableSpirit.Name)
					{
						return 5;
					}
					if (name == Quests.KitavaTorments.Name)
					{
						return 6;
					}
					if (!(name == Quests.PuppetMistress.Name))
					{
						if (name == Quests.KisharaStar.Name)
						{
							return 8;
						}
						if (name == Quests.ReflectionOfTerror.Name)
						{
							return 9;
						}
						if (!(name == Quests.RulerOfHighgate.Name))
						{
							return 0;
						}
						return 10;
					}
					return 7;
				}
				return 4;
			}
			return 3;
		}
		return 2;
	}

	private static List<OptionalQuest> GetDefaultOptionalQuestList()
	{
		return new List<OptionalQuest>
		{
			new OptionalQuest(Quests.MercyMission),
			new OptionalQuest(Quests.DirtyJob),
			new OptionalQuest(Quests.DwellerOfTheDeep),
			new OptionalQuest(Quests.MaroonedMariner),
			new OptionalQuest(Quests.WayForward),
			new OptionalQuest(Quests.GreatWhiteBeast),
			new OptionalQuest(Quests.ThroughSacredGround),
			new OptionalQuest(Quests.VictarioSecrets),
			new OptionalQuest(Quests.SwigOfHope),
			new OptionalQuest(Quests.FixtureOfFate),
			new OptionalQuest(Quests.IndomitableSpirit),
			new OptionalQuest(Quests.InServiceToScience),
			new OptionalQuest(Quests.KingFeast),
			new OptionalQuest(Quests.KitavaTorments),
			new OptionalQuest(Quests.FallenFromGrace),
			new OptionalQuest(Quests.BestelEpic),
			new OptionalQuest(Quests.ClovenOne),
			new OptionalQuest(Quests.PuppetMistress),
			new OptionalQuest(Quests.SilverLocket),
			new OptionalQuest(Quests.InMemoryOfGreust),
			new OptionalQuest(Quests.QueenOfDespair),
			new OptionalQuest(Quests.KisharaStar),
			new OptionalQuest(Quests.LoveIsDead),
			new OptionalQuest(Quests.GemlingLegion),
			new OptionalQuest(Quests.WingsOfVastiri),
			new OptionalQuest(Quests.ReflectionOfTerror),
			new OptionalQuest(Quests.QueenOfSands),
			new OptionalQuest(Quests.FastisFortuna),
			new OptionalQuest(Quests.RulerOfHighgate),
			new OptionalQuest(Quests.NoLoveForOldGhosts),
			new OptionalQuest(Quests.VilentaVengeance),
			new OptionalQuest(Quests.MapToTsoatha)
		};
	}

	private void InitOptionalQuests()
	{
		if (OptionalQuests.Count != 0)
		{
			List<OptionalQuest> defaultOptionalQuestList = GetDefaultOptionalQuestList();
			foreach (OptionalQuest optionalQuest_0 in defaultOptionalQuestList)
			{
				OptionalQuest optionalQuest = OptionalQuests.Find((OptionalQuest q) => q.Id == optionalQuest_0.Id);
				if (optionalQuest != null)
				{
					optionalQuest_0.Enabled = optionalQuest.Enabled;
				}
			}
			OptionalQuests = defaultOptionalQuestList;
		}
		else
		{
			OptionalQuests = GetDefaultOptionalQuestList();
		}
	}

	private static List<string> CustomQuestIDs()
	{
		return new List<string> { "a11q1", "tangle", "cleansing_fire", "uberelder", "maven_boss" };
	}

	private static void AddCustomQuestIDs(ICollection<RewardQuest> list)
	{
		list.Add(new RewardQuest("A Call To Arms", "a11q1"));
		list.Add(new RewardQuest("The Eater of Worlds", "tangle"));
		list.Add(new RewardQuest("The Searing Exarch", "cleansing_fire"));
		list.Add(new RewardQuest("The Elder", "uberelder"));
		list.Add(new RewardQuest("The Maven's Game", "maven_boss"));
	}

	private static int NextRewardQuestAct(RewardQuest quest)
	{
		string name = quest.Name;
		if (name == Quests.SirensCadence.Name)
		{
			return 2;
		}
		if (!(name == Quests.DealWithBandits.Name))
		{
			if (!(name == Quests.FixtureOfFate.Name))
			{
				if (name == Quests.EternalNightmare.Name)
				{
					return 5;
				}
				if (!(name == Quests.KingFeast.Name))
				{
					if (!(name == Quests.EssenceOfUmbra.Name))
					{
						if (!(name == Quests.InMemoryOfGreust.Name))
						{
							if (name == Quests.WingsOfVastiri.Name)
							{
								return 9;
							}
							if (!(name == Quests.StormBlade.Name))
							{
								return 0;
							}
							return 10;
						}
						return 8;
					}
					return 7;
				}
				return 6;
			}
			return 4;
		}
		return 3;
	}

	public static List<RewardQuest> GetDefaultRewardQuestList()
	{
		return new List<RewardQuest>
		{
			new RewardQuest(Quests.EnemyAtTheGate),
			new RewardQuest(Quests.MercyMission.Name + " 1", Quests.MercyMission.Id),
			new RewardQuest(Quests.MercyMission.Name + " 2", Quests.MercyMission.Id + "b"),
			new RewardQuest(Quests.BreakingSomeEggs),
			new RewardQuest(Quests.CagedBrute.Name + " 1", Quests.CagedBrute.Id + "b"),
			new RewardQuest(Quests.CagedBrute.Name + " 2", Quests.CagedBrute.Id),
			new RewardQuest(Quests.SirensCadence),
			new RewardQuest(Quests.SharpAndCruel),
			new RewardQuest(Quests.GreatWhiteBeast),
			new RewardQuest(Quests.IntrudersInBlack),
			new RewardQuest(Quests.ThroughSacredGround.Name, Quests.ThroughSacredGround.Id),
			new RewardQuest(Quests.DealWithBandits, "Eramir"),
			new RewardQuest(Quests.LostInLove),
			new RewardQuest(Quests.RibbonSpool),
			new RewardQuest(Quests.SeverRightHand),
			new RewardQuest(Quests.SwigOfHope),
			new RewardQuest(Quests.FixtureOfFate),
			new RewardQuest(Quests.BreakingSeal),
			new RewardQuest(Quests.EternalNightmare),
			new RewardQuest(Quests.ReturnToOriath),
			new RewardQuest(Quests.KeyToFreedom),
			new RewardQuest(Quests.DeathToPurity),
			new RewardQuest(Quests.KingFeast),
			new RewardQuest(Quests.BestelEpic),
			new RewardQuest(Quests.EssenceOfUmbra),
			new RewardQuest(Quests.SilverLocket),
			new RewardQuest(Quests.EssenceOfArtist),
			new RewardQuest(Quests.InMemoryOfGreust),
			new RewardQuest(Quests.EssenceOfHag),
			new RewardQuest(Quests.WingsOfVastiri),
			new RewardQuest(Quests.StormBlade),
			new RewardQuest(Quests.SafePassage),
			new RewardQuest(Quests.MapToTsoatha),
			new RewardQuest(Quests.DeathAndRebirth)
		};
	}

	private void InitRewardQuests()
	{
		if (RewardQuests.Count == 0)
		{
			RewardQuests = GetDefaultRewardQuestList();
			return;
		}
		List<RewardQuest> defaultRewardQuestList = GetDefaultRewardQuestList();
		foreach (RewardQuest rewardQuest_0 in defaultRewardQuestList)
		{
			RewardQuest rewardQuest = RewardQuests.Find((RewardQuest q) => q.Id == rewardQuest_0.Id);
			if (rewardQuest != null)
			{
				rewardQuest_0.SelectedReward = rewardQuest.SelectedReward;
			}
		}
		RewardQuests = defaultRewardQuestList;
	}
}
