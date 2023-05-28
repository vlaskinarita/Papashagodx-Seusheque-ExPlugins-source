using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.FilesInMemory;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class KillBossTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class50 : CachedObject
	{
		[CompilerGenerated]
		private bool bool_2;

		public bool IsDead
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

		public Class50(int id, WalkablePosition position, bool isDead)
			: base(id, position)
		{
			IsDead = isDead;
		}
	}

	private static readonly Interval interval_0;

	private static readonly Interval interval_1;

	public static int BossesKilled;

	private static readonly List<Class50> list_0;

	private static Class50 class50_0;

	private static bool bool_0;

	private static bool bool_1;

	private static bool bool_2;

	private static string string_0;

	private static int int_0;

	private static Func<Monster, bool> func_0;

	private static readonly Dictionary<int, float> bbqYdUaQum;

	[CompilerGenerated]
	private static bool bool_3;

	public static bool BossKilled
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
		[CompilerGenerated]
		private set
		{
			bool_3 = value;
		}
	}

	public static bool IsCompleted => (MapExplorationTask.MapCompleted || bool_3) && (bool_0 || !AtlasHelper.IsAtlasBossPresent);

	private static int BossAmountForMap
	{
		get
		{
			HashSet<string> value;
			int num = ((!MapData.MapBossesNames.TryGetValue(World.CurrentArea.Name, out value)) ? 1 : value.Count);
			if (LocalData.MapMods.ContainsKey((StatTypeGGG)981))
			{
				num *= 2;
			}
			if (AtlasHelper.IsAtlasBossPresent)
			{
				num++;
			}
			return Math.Max(num, list_0.Count);
		}
	}

	public string Name => "KillBossTask";

	public string Description => "Task for killing map boss.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.Id.Contains("Affliction"))
		{
			if (area.IsMap)
			{
				if (!IsCompleted)
				{
					if (AtlasHelper.IsAtlasBossPresent && !bool_0)
					{
						NetworkObject sirusJournal = ObjectManager.Objects.FirstOrDefault((NetworkObject o) => o.Metadata.Contains("GlyphSirus"));
						if (sirusJournal != (NetworkObject)null)
						{
							GlobalLog.Warn("[KillBossTask] Found " + sirusJournal.Name + ". Conqueror is killed.");
							bool_0 = true;
							BossKilled = true;
						}
					}
					if (class50_0 == null && (class50_0 = list_0.ClosestValid((Class50 b) => !b.IsDead)) == null)
					{
						return false;
					}
					if (!Blacklist.Contains(class50_0.Id))
					{
						if (string_0 != null && class50_0.Position.Name != string_0)
						{
							Class50 priorityBoss = list_0.ClosestValid((Class50 b) => !b.IsDead && b.Position.Name == string_0);
							if (priorityBoss != null)
							{
								GlobalLog.Debug($"[KillBossTask] Switching current target to \"{priorityBoss}\".");
								class50_0 = priorityBoss;
								return true;
							}
						}
						if (class50_0.IsDead)
						{
							Class50 newBoss = list_0.Valid().FirstOrDefault((Class50 b) => !b.IsDead);
							if (newBoss != null)
							{
								class50_0 = newBoss;
							}
						}
						WalkablePosition pos = class50_0.Position;
						if (pos.Distance <= 50 && pos.PathDistance <= 55f)
						{
							NetworkObject @object = class50_0.Object;
							Monster bossObj = (Monster)(object)((@object is Monster) ? @object : null);
							if ((NetworkObject)(object)bossObj == (NetworkObject)null)
							{
								if (bool_2)
								{
									list_0.Remove(class50_0);
									class50_0 = null;
									return true;
								}
								GlobalLog.Debug("[KillBossTask] We are close to last know position of map boss, but boss object does not exist anymore.");
								GlobalLog.Debug("[KillBossTask] Most likely this boss does not spawn a corpse or was shattered/exploded.");
								class50_0.IsDead = true;
								class50_0 = null;
								RegisterDeath();
								return true;
							}
						}
						if (pos.Distance <= int_0)
						{
							int attempts = ++class50_0.InteractionAttempts;
							if (attempts == 12)
							{
								GlobalLog.Debug("[KillBossTask] Trying to move around to trigger a boss.");
								WorldPosition distantPos = WorldPosition.FindPathablePositionAtDistance(40, 70, 5);
								if (distantPos != null)
								{
									await Move.AtOnce(distantPos, "distant position", 10);
								}
							}
							if (attempts > 50)
							{
								GlobalLog.Error("[KillBossTask] Boss did not become active. Now ignoring it.");
								class50_0.Ignored = true;
								class50_0 = null;
								RegisterDeath();
								return true;
							}
							await PlayerAction.MoveAway(10, 50);
							GlobalLog.Debug($"[KillBossTask] Waiting for map boss to become active ({attempts}/{50})");
							await Wait.StuckDetectionSleep(200);
							return true;
						}
						if (interval_0.Elapsed)
						{
							GlobalLog.Debug($"[KillBossTask] Going to {pos}");
						}
						if (!pos.TryCome())
						{
							GlobalLog.Error((MapData.Current.Type != 0) ? ("[KillBossTask] Fail to move to the map boss \"" + pos.Name + "\". Will try again after area transition.") : ("[KillBossTask] Unexpected error. Fail to move to map boss (" + pos.Name + ") in a regular map."));
							class50_0.Unwalkable = true;
							class50_0 = null;
						}
						return true;
					}
					GlobalLog.Warn("[KillBossTask] Boss is in global blacklist. Now marking it as killed.");
					class50_0.IsDead = true;
					class50_0 = null;
					RegisterDeath();
					return true;
				}
				return false;
			}
			return false;
		}
		return GeneralSettings.Instance.IsOnRun;
	}

	public void Tick()
	{
		if (IsCompleted || !interval_1.Elapsed)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!LokiPoe.IsInGame || !currentArea.IsMap || (currentArea.Id.Contains("Affliction") && GeneralSettings.Instance.IsOnRun))
		{
			return;
		}
		FillBossData();
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if ((NetworkObject)(object)val == (NetworkObject)null || !func_0(val) || ((NetworkObject)val).Metadata.Contains("LeagueAffliction") || ((NetworkObject)val).Metadata.Contains("Avatar") || ((NetworkObject)val).Metadata.Contains("Blight") || ((NetworkObject)val).Metadata.Contains("League") || ((NetworkObject)val).Name == "The Maven" || ((NetworkObject)val).Name == "Metamorph")
			{
				continue;
			}
			int int_0 = ((NetworkObject)val).Id;
			Class50 @class = list_0.Find((Class50 b) => b.Id == int_0);
			if (((Actor)val).IsDead)
			{
				if (!(@class == null))
				{
					if (!@class.IsDead)
					{
						GlobalLog.Warn("[KillBossTask] Registering death of \"" + ((NetworkObject)val).Name + "\".");
						@class.IsDead = true;
						if (!bool_1)
						{
							class50_0 = null;
						}
						RegisterDeath(((NetworkObject)val).Name);
					}
				}
				else
				{
					GlobalLog.Warn("[KillBossTask] Registering dead map boss \"" + ((NetworkObject)val).Name + "\".");
					list_0.Add(new Class50(int_0, ((NetworkObject)(object)val).WalkablePosition(), isDead: true));
					RegisterDeath(((NetworkObject)val).Name);
				}
			}
			else
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)val).WalkablePosition(5, 20);
				if (!(@class != null))
				{
					list_0.Add(new Class50(int_0, walkablePosition, isDead: false));
					GlobalLog.Warn($"[KillBossTask] Registering {walkablePosition}");
				}
				else
				{
					@class.Position = walkablePosition;
				}
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		switch (message.Id)
		{
		case "explorer_local_transition_entered_event":
			GlobalLog.Info("[KillBossTask] Resetting unwalkable flags.");
			foreach (Class50 item in list_0)
			{
				item.Unwalkable = false;
			}
			return (MessageResult)0;
		case "combat_area_new_transition_event":
			if (LokiPoe.CurrentWorldArea.Name == MapNames.GraveTrough)
			{
				class50_0 = null;
			}
			goto default;
		default:
			return (MessageResult)1;
		case "MB_new_map_entered_event":
		{
			GlobalLog.Info("[KillBossTask] Reset.");
			string input = message.GetInput<string>(0);
			BossesKilled = 0;
			class50_0 = null;
			bool_0 = false;
			bool_1 = false;
			bool_2 = false;
			BossKilled = false;
			bbqYdUaQum.Clear();
			list_0.Clear();
			SetBossSelector(input);
			SetPriorityBossName(input);
			SetBossRange(input);
			if (!(input == MapNames.VaultsOfAtziri))
			{
				if (!(input == MapNames.MineralPools) && !(input == MapNames.Palace) && !(input == MapNames.Basilica) && !(input == MapNames.UndergroundSea) && !(input == MapNames.MaelstromOfChaos))
				{
					if (!LocalData.MapMods.ContainsKey((StatTypeGGG)6548) && !(input == MapNames.Pen) && !(input == MapNames.Pier) && !(input == MapNames.Shrine) && !(input == MapNames.Laboratory) && !(input == MapNames.DesertSpring) && !(input == MapNames.Summit) && !(input == MapNames.DarkForest) && !(input == MapNames.Core) && !(input == MapNames.GraveTrough) && !(input == MapNames.PutridCloister))
					{
						return (MessageResult)0;
					}
					bool_2 = true;
					GlobalLog.Info("[KillBossTask] TeleportingBoss is set to true (" + input + ")");
					return (MessageResult)0;
				}
				bool_1 = true;
				GlobalLog.Info("[KillBossTask] MultiPhaseBoss is set to true (" + input + ")");
				return (MessageResult)0;
			}
			BossKilled = true;
			GlobalLog.Info("[KillBossTask] BossKilled is set to true (" + input + ")");
			return (MessageResult)0;
		}
		}
	}

	private static void RegisterDeath(string name = "")
	{
		BossesKilled++;
		int bossAmountForMap = BossAmountForMap;
		GlobalLog.Warn($"[KillBossTask] Bosses killed: {BossesKilled} out of {bossAmountForMap}.");
		if (MapData.AtlasBossNames.Contains(name))
		{
			GlobalLog.Info("[KillBossTask] Atlas boss was killed");
			bool_0 = true;
		}
		if (BossesKilled >= BossAmountForMap && (bool_0 || !AtlasHelper.IsAtlasBossPresent))
		{
			BossKilled = true;
		}
	}

	private static void SetPriorityBossName(string areaName)
	{
		if (areaName == MapNames.Core)
		{
			string_0 = "Eater of Souls";
			GlobalLog.Info("[KillBossTask] Priority boss name is set to \"Eater of Souls\".");
		}
		if (areaName == MapNames.Racecourse)
		{
			string_0 = "Bringer of Blood";
			GlobalLog.Info("[KillBossTask] Priority boss name is set to \"Bringer of Blood\".");
		}
		else if (areaName == MapNames.Siege)
		{
			string_0 = "Tukohama's Protection";
			GlobalLog.Info("[KillBossTask] Priority boss name is set to \"Tukohama's Protection\".");
		}
		else
		{
			string_0 = null;
		}
	}

	private static void SetBossRange(string areaName)
	{
		if (AtlasHelper.IsAtlasBossPresent)
		{
			int_0 = 10;
		}
		if (areaName == MapNames.Tower || areaName == MapNames.SunkenCity)
		{
			int_0 = 35;
		}
		if (areaName == MapNames.LavaLake || areaName == MapNames.Belfry)
		{
			int_0 = 25;
		}
		else if (areaName == MapNames.Fields)
		{
			int_0 = 30;
		}
		else if (areaName == MapNames.Glacier || areaName == MapNames.UndergroundRiver || areaName == MapNames.Caldera)
		{
			int_0 = 7;
		}
		else
		{
			int_0 = 15;
		}
		GlobalLog.Info($"[KillBossTask] Boss range: {int_0}");
	}

	private static void SetBossSelector(string areaName)
	{
		if (areaName == MapNames.Core && BossesKilled >= 3)
		{
			GlobalLog.Info("[KillBossTask] Malachai is only unlocked after killing his minions.");
			func_0 = (Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Name.Equals("Eater of Souls");
		}
		else if (!(areaName == MapNames.Precinct))
		{
			if (areaName == MapNames.Courthouse)
			{
				GlobalLog.Info("[KillBossTask] This map has a group of dark Rogue Exiles as map bosses.");
				func_0 = (Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata.EndsWith("Kitava") && ((NetworkObject)m).Metadata.Contains("/Exiles/");
			}
			else if (!(areaName == MapNames.InfestedValley))
			{
				if (!(areaName == MapNames.Siege))
				{
					if (areaName == MapNames.WhakawairuaTuahu)
					{
						GlobalLog.Info("[KillBossTask] This map has Shade of a player as one of the map bosses.");
						func_0 = (Monster m) => m.IsMapBoss || ((int)m.Rarity == 3 && ((NetworkObject)m).Metadata.Contains("DarkExile"));
					}
					else if (!(areaName == MapNames.Port))
					{
						if (!(areaName == MapNames.Shipyard) && !(areaName == MapNames.Lighthouse) && !(areaName == MapNames.Iceberg) && !(areaName == MapNames.AcidCaverns))
						{
							func_0 = DefaultBossSelector;
							return;
						}
						GlobalLog.Info("[KillBossTask] This map has a Warband leader as a map boss.");
						func_0 = (Monster m) => (int)m.Rarity == 3 && m.ExplicitAffixes.Any((ModRecord a) => a.InternalName == "MonsterWbLeader");
					}
					else
					{
						GlobalLog.Info("[KillBossTask] Some phases of boss are not counted as IsMapBoss. Have to force it.");
						func_0 = (Monster m) => m.IsMapBoss || ((int)m.Rarity == 3 && ((NetworkObject)m).Name == "Unravelling Horror");
					}
				}
				else
				{
					GlobalLog.Info("[KillBossTask] Totems must be killed to remove boss immunity on this map.");
					func_0 = (Monster m) => m.IsMapBoss || ((NetworkObject)m).Name == "Tukohama's Protection";
				}
			}
			else
			{
				GlobalLog.Info("[KillBossTask] Nests have to be killed to activate boss on this map.");
				func_0 = (Monster m) => m.IsMapBoss || ((NetworkObject)m).Name == "Gorulis' Nest";
			}
		}
		else
		{
			GlobalLog.Info("[KillBossTask] This map has a group of Rogue Exiles as map bosses.");
			func_0 = (Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata.Contains("MapBoss");
		}
	}

	private static bool DefaultBossSelector(Monster m)
	{
		if (!MapData.MapBossesNames.TryGetValue(World.CurrentArea.Name, out var value))
		{
			return m.IsMapBoss || ((NetworkObject)m).Metadata.Contains("AtlasBosses/TheElder");
		}
		if (LocalData.MapMods.TryGetValue((StatTypeGGG)13845, out var value2))
		{
			switch (value2)
			{
			case 1:
				value.Add("Baran, The Crusader");
				break;
			case 2:
				value.Add("Veritania, The Redeemer");
				break;
			case 3:
				value.Add("Al-Hezmin, The Hunter");
				break;
			case 4:
				value.Add("Drox, The Warlord");
				break;
			}
		}
		if (value == null || !value.Any())
		{
			return m.IsMapBoss || ((NetworkObject)m).Metadata.Contains("AtlasBosses/TheElder");
		}
		return value.Any((string e) => e.ContainsIgnorecase(((NetworkObject)m).Name)) && m.IsMapBoss;
	}

	private static void FillBossData()
	{
		string name = World.CurrentArea.Name;
		switch (name)
		{
		case "Absence of Symmetry and Harmony":
			return;
		case "Seething Chyme":
			return;
		}
		if (name == "Polaric Void")
		{
			return;
		}
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if ((NetworkObject)(object)val == (NetworkObject)null || ((Actor)val).IsDead || ((NetworkObject)(object)val).WalkablePosition().Distance > 100 || ((NetworkObject)val).Metadata.Contains("LeagueAffliction") || ((NetworkObject)val).Metadata.Contains("Avatar") || ((NetworkObject)val).Metadata.Contains("Blight") || ((NetworkObject)val).Metadata.Contains("League") || ((NetworkObject)val).Name == "The Maven" || ((NetworkObject)val).Name == "Metamorph")
			{
				continue;
			}
			if (!func_0(val))
			{
				if (!(class50_0?.Object != (NetworkObject)null) || !(class50_0.Object == (NetworkObject)(object)val))
				{
					continue;
				}
				GlobalLog.Warn("[KillBossTask] Adding " + @object.Name + " to watchlist.");
			}
			if (!(((Actor)val).HealthPercent >= 99f))
			{
				int id = ((NetworkObject)val).Id;
				if (!bbqYdUaQum.ContainsKey(id))
				{
					bbqYdUaQum.Add(id, ((Actor)val).HealthPercent);
				}
			}
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static KillBossTask()
	{
		interval_0 = new Interval(1000);
		interval_1 = new Interval(200);
		list_0 = new List<Class50>();
		func_0 = DefaultBossSelector;
		bbqYdUaQum = new Dictionary<int, float>();
	}
}
