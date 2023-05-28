using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.BlightPluginEx.Tasks;

public class StartEventTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool Started;

	public static int FailedBlightInteract;

	public string Name => "StartEventTask";

	public string Description => "Task starts blight event";

	public string Author => "Seusheque";

	public string Version => "2.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (Started)
			{
				return false;
			}
			if (HandleEventTask.SkipEncounter)
			{
				return false;
			}
			if (FailedBlightInteract >= 15)
			{
				return false;
			}
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsMap)
			{
				return false;
			}
			if (BlightPluginEx.CachedCassia == null)
			{
				return false;
			}
			if (!(BlightPluginEx.CachedBlightCore == null))
			{
				if (BlightPluginEx.CachedCassia != null && BlightPluginEx.CachedCassia.Object != (NetworkObject)null && ((NetworkObject)BlightPluginEx.Cassia).HasNpcFloatingIcon)
				{
					WalkablePosition cassiaPos = BlightPluginEx.CachedCassia.Position;
					if (cassiaPos.IsFar || cassiaPos.IsFarByPath)
					{
						if (!ExilePather.PathExistsBetween(LokiPoe.MyPosition, (Vector2i)cassiaPos, false))
						{
							return true;
						}
						if (Class53.Instance.DebugMode)
						{
							GlobalLog.Warn("[StartEventTask] Moving closer to Cassia.");
						}
						if (!cassiaPos.TryCome())
						{
							GlobalLog.Error($"[StartEventTask] Fail to move to {cassiaPos}. Cassia is unwalkable.");
							await OpenDoor();
							return true;
						}
					}
					GlobalLog.Warn("[StartEventTask] Cassia has NPC Floationg Icon, going to interact her.");
					if (await PlayerAction.Interact((NetworkObject)(object)BlightPluginEx.Cassia))
					{
						await Wait.Sleep(200);
						await Coroutines.CloseBlockingWindows();
					}
					await Wait.Sleep(100);
					return true;
				}
				if (BlightPluginEx.CachedBlightCore.Object != (NetworkObject)null && BlightPluginEx.CachedBlightCore.Object.Components.TransitionableComponent.Flag1 == 1)
				{
					WalkablePosition corepos = BlightPluginEx.CachedBlightCore.Position;
					if (corepos.IsFar || corepos.IsFarByPath)
					{
						if (ExilePather.PathExistsBetween(LokiPoe.MyPosition, (Vector2i)corepos, false))
						{
							if (Class53.Instance.DebugMode)
							{
								GlobalLog.Warn("[StartEventTask] Moving closer to the Blight Core.");
							}
							if (corepos.TryCome())
							{
								return true;
							}
							GlobalLog.Error($"[StartEventTask] Fail to move to {corepos}. Blight Core is unwalkable.");
							await OpenDoor();
							return true;
						}
						return true;
					}
					await Coroutines.FinishCurrentAction(true);
					if (!(await PlayerAction.Interact(BlightPluginEx.ClosestBlightCore)))
					{
						FailedBlightInteract++;
						GlobalLog.Warn($"[StartEventTask] Core interaction failed [{FailedBlightInteract}/15], moving.");
						Vector2i pos = BlightPluginEx.ClosestBlightCore.Position + new Vector2i(LokiPoe.Random.Next(-20, 20), LokiPoe.Random.Next(-20, 20));
						PlayerMoverManager.MoveTowards(pos, (object)null);
						await Wait.Sleep(400);
						return true;
					}
					if (await Wait.For(() => !BlightPluginEx.ClosestBlightCore.IsTargetable, "Interact Blight Core", 100, 2000, Class53.Instance.DebugMode))
					{
						await Wait.For(() => Blight.IsEncounterRunning, "Encounter start", 200, 3500, Class53.Instance.DebugMode);
						Utility.BroadcastMessage((object)null, "blight_engaged", new object[1] { "Started blight at " + area.Name });
						BlightPluginEx.CachedBlightCore = new CachedObject(BlightPluginEx.ClosestBlightCore);
						if ((NetworkObject)(object)PlayerAction.AnyPortalInRangeOf(100) == (NetworkObject)null)
						{
							WorldPosition walkablePos = WorldPosition.FindPathablePositionAtDistance(40, 100, 10);
							Move.Towards(walkablePos, "portal position");
							await Coroutines.FinishCurrentAction(true);
							await PlayerAction.CreateTownPortal();
						}
						await Wait.SleepSafe(100);
						Started = true;
						HandleEventTask.SkipEncounter = false;
					}
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static async Task OpenDoor()
	{
		TriggerableBlockage triggerableBlockage_0 = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)HandleEventTask.IsClosedDoor);
		if ((NetworkObject)(object)triggerableBlockage_0 == (NetworkObject)null)
		{
			return;
		}
		if (!(await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0)))
		{
			await Wait.SleepSafe(300);
			return;
		}
		await Wait.LatencySleep();
		await Wait.For(() => !((NetworkObject)triggerableBlockage_0).IsTargetable || triggerableBlockage_0.IsOpened, "door opening", 50, 300, Class53.Instance.DebugMode);
	}

	public void Tick()
	{
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
}
