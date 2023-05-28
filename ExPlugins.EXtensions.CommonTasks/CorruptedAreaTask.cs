using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.FilesInMemory;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class CorruptedAreaTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public class CorruptedAreaData
	{
		public CachedObject Boss;

		public bool IsAreaFinished;

		public bool IsBossKilled;

		public CachedObject Vessel;
	}

	private static readonly Interval interval_0;

	private static CachedTransition cachedTransition_0;

	private readonly bool bool_0;

	private static bool ExitExists => ObjectManager.Objects.Any((AreaTransition a) => ((NetworkObject)a).IsTargetable && ((NetworkObject)a).Metadata == "Metadata/MiscellaneousObjects/VaalSideAreaReturnPortal");

	private static NetworkObject TransitionBlockage => ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Metadata == "Metadata/QuestObjects/Sewers/SewersGrate");

	public static CorruptedAreaData CachedData
	{
		get
		{
			CorruptedAreaData corruptedAreaData = CombatAreaCache.Current.Storage["CorruptedAreaData"] as CorruptedAreaData;
			if (corruptedAreaData == null)
			{
				corruptedAreaData = new CorruptedAreaData();
				CombatAreaCache.Current.Storage["CorruptedAreaData"] = corruptedAreaData;
			}
			return corruptedAreaData;
		}
	}

	public string Name => "CorruptedAreaTask";

	public string Description => "Task that handles corrupted side areas.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public CorruptedAreaTask(bool enterCorruptedAreas)
	{
		bool_0 = enterCorruptedAreas;
	}

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame || ((Actor)LokiPoe.Me).IsDead)
		{
			return false;
		}
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx" || ((IAuthored)BotManager.Current).Name == "MapBotEx")
		{
			if (bool_0)
			{
				DatWorldAreaWrapper area = World.CurrentArea;
				if (area.IsOverworldArea || area.IsMap)
				{
					CachedTransition transition = CombatAreaCache.Current.AreaTransitions.Find(ValidCorruptedAreaTransition);
					if (transition != null)
					{
						await EnterCorruptedArea(transition);
						return true;
					}
				}
				else if (area.IsCorruptedArea)
				{
					await HandleCorruptedArea();
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		if (!World.CurrentArea.IsCorruptedArea || !interval_0.Elapsed)
		{
			return;
		}
		CorruptedAreaData cachedData = CachedData;
		if (cachedData.IsAreaFinished)
		{
			return;
		}
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Chest val = (Chest)(object)((@object is Chest) ? @object : null);
			if (val == null || !(@object.Metadata == "Metadata/Chests/SideArea/SideAreaChest"))
			{
				if (cachedData.IsBossKilled)
				{
					continue;
				}
				Monster val2 = (Monster)(object)((@object is Monster) ? @object : null);
				if (val2 == null || (int)val2.Rarity != 3 || !Enumerable.Any(val2.ExplicitAffixes, (ModRecord a) => a.Category == "MonsterSideAreaBoss"))
				{
					continue;
				}
				if (!Blacklist.Contains(@object.Id))
				{
					if (((Actor)val2).IsDead)
					{
						GlobalLog.Warn("[CorruptedAreaTask] Corrupted area boss has been killed.");
						cachedData.IsBossKilled = true;
					}
					else if (cachedData.Boss == null)
					{
						cachedData.Boss = new CachedObject(@object);
						GlobalLog.Warn($"[CorruptedAreaTask] Registering {cachedData.Boss}");
					}
					else
					{
						cachedData.Boss.Position = @object.WalkablePosition();
					}
				}
				else
				{
					GlobalLog.Warn("[CorruptedAreaTask] Boss was blacklisted from outside. We will not be able to finish this area.");
					cachedData.IsAreaFinished = true;
				}
			}
			else if (!val.IsOpened)
			{
				if (cachedData.Vessel == null)
				{
					cachedData.Vessel = new CachedObject(@object);
					GlobalLog.Warn($"[CorruptedAreaTask] Registering {cachedData.Vessel}");
				}
			}
			else
			{
				GlobalLog.Warn("[CorruptedAreaTask] Vaal Vessel has been opened. Area is finished.");
				cachedData.IsAreaFinished = true;
			}
		}
	}

	private static async Task EnterCorruptedArea(CachedTransition transition)
	{
		WalkablePosition pos = transition.Position;
		if (!pos.IsFar)
		{
			AreaTransition areaTransition_0 = transition.Object;
			if ((NetworkObject)(object)areaTransition_0 == (NetworkObject)null)
			{
				GlobalLog.Error("[CorruptedAreaTask] Unexpected error. Transition object is null.");
				transition.Ignored = true;
			}
			else if (++transition.InteractionAttempts > 5)
			{
				GlobalLog.Error("[CorruptedAreaTask] All attempts to enter corrupted area transition have been spent. Now ignoring it.");
				transition.Ignored = true;
			}
			else if (!((NetworkObject)areaTransition_0).IsTargetable)
			{
				NetworkObject blockage = TransitionBlockage;
				if (!(blockage != (NetworkObject)null))
				{
					GlobalLog.Error("[CorruptedAreaTask] Unexpected error. Transition object is untargetable.");
					transition.Ignored = true;
				}
				else
				{
					await PlayerAction.Interact(blockage, () => ((NetworkObject)areaTransition_0.Fresh<AreaTransition>()).IsTargetable, "transition become targetable");
				}
			}
			else
			{
				cachedTransition_0 = transition;
				if (!(await PlayerAction.TakeTransition(areaTransition_0)))
				{
					await Wait.SleepSafe(500);
				}
			}
		}
		else if (!pos.TryCome())
		{
			GlobalLog.Debug($"[CorruptedAreaTask] Fail to move to {pos}. Marking this transition as unwalkable.");
			transition.Unwalkable = true;
		}
	}

	private static async Task HandleCorruptedArea()
	{
		CorruptedAreaData cachedData = CachedData;
		if (cachedData.IsAreaFinished)
		{
			GlobalLog.Warn("[CorruptedAreaTask] Now leaving the corrupted area because area is finished.");
			DisableEntrance();
			AreaTransition exit = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => ((NetworkObject)a).IsTargetable));
			bool flag = (NetworkObject)(object)exit != (NetworkObject)null;
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await PlayerAction.TakeTransition(exit);
			}
			if (flag2)
			{
				return;
			}
		}
		CachedObject vessel = cachedData.Vessel;
		if (!(vessel != null) || !cachedData.IsBossKilled)
		{
			CachedObject boss = cachedData.Boss;
			if (boss != null)
			{
				WalkablePosition pos2 = boss.Position;
				if (!pos2.IsFar && !pos2.IsFarByPath)
				{
					NetworkObject bossObj = boss.Object;
					if (!(bossObj == (NetworkObject)null))
					{
						GlobalLog.Debug("[CorruptedAreaTask] Waiting for combat routine to kill the boss.");
						await Wait.StuckDetectionSleep(500);
					}
					else
					{
						GlobalLog.Warn("[CorruptedAreaTask] There is no boss object near cached position. Marking it as dead.");
						cachedData.IsBossKilled = true;
					}
				}
				else if (!pos2.TryCome())
				{
					GlobalLog.Error("[CorruptedAreaTask] Unexpected error. Corrupted area boss is unwalkable.");
					cachedData.IsAreaFinished = true;
				}
			}
			else if (!(await CombatAreaCache.Current.Explorer.Execute()))
			{
				GlobalLog.Warn("[CorruptedAreaTask] Now leaving the corrupted area because it is fully explored.");
				cachedData.IsAreaFinished = true;
			}
			return;
		}
		WalkablePosition pos = vessel.Position;
		if (pos.IsFar || pos.IsFarByPath)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Error("[CorruptedAreaTask] Unexpected error. Vaal Vessel position is unwalkable.");
				cachedData.IsAreaFinished = true;
			}
			return;
		}
		Chest chest_0 = default(Chest);
		ref Chest reference = ref chest_0;
		NetworkObject @object = vessel.Object;
		reference = (Chest)(object)((@object is Chest) ? @object : null);
		if (!((NetworkObject)(object)chest_0 == (NetworkObject)null))
		{
			if (++vessel.InteractionAttempts > 5)
			{
				GlobalLog.Error("[CorruptedAreaTask] All attempts to open Vaal Vessel have been spent.");
				cachedData.IsAreaFinished = true;
			}
			else if (!(await PlayerAction.Interact((NetworkObject)(object)chest_0, () => chest_0.Fresh<Chest>().IsOpened, "Vaal Vessel opening")))
			{
				await Wait.SleepSafe(500);
			}
			else
			{
				await Wait.For(() => ExitExists, "corrupted area exit spawning", 500, 5000);
			}
		}
		else
		{
			GlobalLog.Error("[CorruptedAreaTask] Unexpected error. Vaal Vessel object is null.");
			cachedData.IsAreaFinished = true;
		}
	}

	private static void DisableEntrance()
	{
		if (cachedTransition_0 != null)
		{
			GlobalLog.Warn("[CorruptedAreaTask] Marking \"" + cachedTransition_0.Position.Name + "\" transition as visited.");
			cachedTransition_0.Visited = true;
			cachedTransition_0 = null;
		}
	}

	private static bool ValidCorruptedAreaTransition(CachedTransition t)
	{
		return t.Type == TransitionType.Vaal && !t.Visited && !t.Ignored && !t.Unwalkable;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static CorruptedAreaTask()
	{
		interval_0 = new Interval(400);
	}
}
