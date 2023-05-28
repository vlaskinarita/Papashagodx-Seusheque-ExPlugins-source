using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class MapExplorationTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static bool bool_0;

	private static bool bool_1;

	private static bool bool_2;

	private static readonly GeneralSettings Config;

	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static NetworkObject BossLever => ObjectManager.GetObjectByMetadata("Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_Counter");

	private static CachedObject CachedBossLever
	{
		get
		{
			return CombatAreaCache.Current.Storage["BossLever"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["BossLever"] = value;
		}
	}

	public static bool MapCompleted
	{
		get
		{
			return bool_1;
		}
		private set
		{
			bool_1 = value;
			if (value)
			{
				TrackMobTask.RestrictRange();
			}
		}
	}

	public string Name => "MapExplorationTask";

	public string Description => "Task that handles map exploration.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.Id.Contains("Affliction"))
		{
			return Config.IsOnRun;
		}
		if (!area.IsMap)
		{
			return false;
		}
		if (!MapCompleted)
		{
			if (bool_0 && World.CurrentArea.Name == "Dungeon" && !LocalData.MapMods.ContainsKey((StatTypeGGG)10342) && CombatAreaCache.Current.AreaTransitions.FirstOrDefault((CachedTransition a) => a.Name == "Opening") == null)
			{
				GlobalLog.Debug("[" + Name + "] Special Dungeon handling");
				tgtPosition_0.Come();
				return true;
			}
			if (World.CurrentArea.Name == "Laboratory" && SpecialObjectTask.LabCounter >= 4)
			{
				CachedTransition arenaPortal = CombatAreaCache.Current.AreaTransitions.FirstOrDefault((CachedTransition a) => a.Name == "Arena");
				if (arenaPortal == null)
				{
					if (!(CachedBossLever != null) || !(BossLever == (NetworkObject)null))
					{
						if (CachedBossLever != null && BossLever != (NetworkObject)null && !BossLever.IsTargetable)
						{
							GlobalLog.Debug("[" + Name + "] Waiting for laboratory event to complete and arena to appear");
						}
						return true;
					}
					GlobalLog.Debug("[" + Name + "] Special Laboratory handling");
					CachedBossLever.Position.Come();
					return true;
				}
			}
			if (bool_0 && World.CurrentArea.Name == "Cursed Crypt" && !LocalData.MapMods.ContainsKey((StatTypeGGG)10342) && CombatAreaCache.Current.Storage["CurseCryptHandling"] == null)
			{
				GlobalLog.Debug("[" + Name + "] Special Cursed Crypt handling");
				if (!tgtPosition_1.IsFar)
				{
					CombatAreaCache.Current.Storage["CurseCryptHandling"] = true;
				}
				tgtPosition_1.Come();
				return true;
			}
			return await CombatAreaCache.Current.Explorer.Execute();
		}
		return false;
	}

	public void Tick()
	{
		if (bool_1 || !interval_0.Elapsed)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!LokiPoe.IsInGame || !currentArea.IsMap)
		{
			return;
		}
		if (!Config.IsOnRun)
		{
			GlobalLog.Warn("[" + Name + "] [Finish currently open map] checkbox disabled. Forcing map finish!");
			MapCompleted = true;
		}
		else
		{
			if (currentArea.Id.Contains("Affliction"))
			{
				return;
			}
			MapData current = MapData.Current;
			MapType mapType = current.Type;
			if (CachedBossLever == null && BossLever != (NetworkObject)null)
			{
				CachedBossLever = new CachedObject(BossLever);
			}
			if (AtlasHelper.IsAtlasBossPresent && mapType == MapType.Regular)
			{
				mapType = MapType.Bossroom;
			}
			if (KillBossTask.BossKilled)
			{
				if (Config.BossRushMode || current.BossRush)
				{
					GlobalLog.Debug("[" + Name + "] Boss rush mode enabled. Boss is killed. Leaving current map.");
					MapCompleted = true;
					return;
				}
				if (bool_0)
				{
					GlobalLog.Debug("[" + Name + "] Boss is killed and map completion point is reached. Map is complete.");
					MapCompleted = true;
					return;
				}
				if (bool_2)
				{
					GlobalLog.Debug("[" + Name + "] Boss is killed and BossInTheEnd flag is true. Map is complete.");
					MapCompleted = true;
					return;
				}
			}
			if (bool_0 && InstanceInfo.MonstersRemaining < 5)
			{
				GlobalLog.Debug("[" + Name + "] Seems like we killed most mobs on this map but couldn't find the boss. Leaving this map to stop wasting time.");
				MapCompleted = true;
			}
			else
			{
				if (bool_0)
				{
					return;
				}
				if (InstanceInfo.MonstersRemaining <= current.MobRemaining)
				{
					GlobalLog.Warn($"[{Name}] Monster remaining limit has been reached ({current.MobRemaining})");
					if (mapType == MapType.Bossroom || AtlasHelper.IsAtlasBossPresent)
					{
						if (!AtlasHelper.IsAtlasBossPresent && current.IgnoreBossroom)
						{
							GlobalLog.Debug("[" + Name + "] Bossroom is ignored. Map is complete.");
							MapCompleted = true;
							return;
						}
						TrackMobTask.RestrictRange();
						CombatAreaCache.Current.Explorer.Settings.FastTransition = true;
					}
					bool_0 = true;
				}
				else
				{
					if ((mapType != 0 && mapType != MapType.Bossroom) || !(CombatAreaCache.Current.Explorer.BasicExplorer.PercentComplete >= (float)current.ExplorationPercent))
					{
						return;
					}
					GlobalLog.Warn($"[{Name}] Exploration limit has been reached ({current.ExplorationPercent}%)");
					if (mapType == MapType.Bossroom || AtlasHelper.IsAtlasBossPresent)
					{
						if (!AtlasHelper.IsAtlasBossPresent && current.IgnoreBossroom)
						{
							GlobalLog.Debug("[" + Name + "] Bossroom is ignored. Map is complete.");
							MapCompleted = true;
							return;
						}
						TrackMobTask.RestrictRange();
						CombatAreaCache.Current.Explorer.Settings.FastTransition = true;
					}
					bool_0 = true;
				}
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (id == "MB_new_map_entered_event")
		{
			GlobalLog.Info("[" + Name + "] Reset.");
			Reset(message.GetInput<string>(0));
			return (MessageResult)0;
		}
		if (id == "explorer_local_transition_entered_event")
		{
			SpecificTweaksOnLocalTransition();
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static void Reset(string areaName)
	{
		MapCompleted = false;
		bool_0 = false;
		bool_2 = false;
		if (areaName == MapNames.Excavation || areaName == MapNames.Arena || areaName == MapNames.Laboratory || areaName == MapNames.Core)
		{
			bool_2 = true;
			GlobalLog.Info($"[MapExplorationTask] BossInTheEnd is set to {bool_2}.");
		}
		else if (areaName == MapNames.VaultsOfAtziri)
		{
			MapData.Current.MobRemaining = -1;
			GlobalLog.Info($"[MapExplorationTask] Monster remaining is set to {MapData.Current.MobRemaining}.");
		}
	}

	private static void SpecificTweaksOnLocalTransition()
	{
		string name = World.CurrentArea.Name;
		if (name == MapNames.Ramparts || name == MapNames.Malformation)
		{
			CachedTransition cachedTransition = (from t in CombatAreaCache.Current.AreaTransitions
				where t.Type == TransitionType.Local && !t.LeadsBack && !t.Visited
				orderby t.Position.Distance descending
				select t).FirstOrDefault();
			if (cachedTransition != null)
			{
				GlobalLog.Info($"[MapExplorationTask] Marking {cachedTransition.Position} as back transition.");
				cachedTransition.LeadsBack = true;
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

	static MapExplorationTask()
	{
		interval_0 = new Interval(100);
		Config = GeneralSettings.Instance;
		tgtPosition_0 = new TgtPosition("Dungeon Entrance", "dungeon_prison_wall_straight_hole_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Sarcophagus", "Art/Models/Terrain/ChurchDungeon/church_ruins_v10_02.tgt");
	}
}
