using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using log4net;

namespace ExPlugins.QuestBotEx.TrialHandler;

public class TrialDoorTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly ILog ilog_0;

	private static TriggerableBlockage TrialDoor => ObjectManager.TrialOfAscendancy;

	public string Name => "TrialDoorTask";

	public string Author => "";

	public string Description => "";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame || ((Actor)LokiPoe.Me).IsDead || !LokiPoe.CurrentWorldArea.IsCombatArea)
		{
			return false;
		}
		if (TrialSolverTask.AreaContainsTrial())
		{
			if (PluginManager.EnabledPlugins.Any((IPlugin p) => ((IAuthored)p).Name == "DeadlyTrials"))
			{
				return false;
			}
			if (!((NetworkObject)(object)TrialDoor != (NetworkObject)null) || !((NetworkObject)TrialDoor).IsTargetable || !(((NetworkObject)TrialDoor).Distance < 30f))
			{
				int int_0 = 60;
				if (LokiPoe.CurrentWorldArea.Id == "2_7_4" || LokiPoe.CurrentWorldArea.Id == "1_3_6")
				{
					int_0 = 20;
				}
				List<TriggerableBlockage> lockedDoors = (from o in ObjectManager.GetObjectsByType<TriggerableBlockage>()
					where (((NetworkObject)o).Metadata.Contains("Puzzle_Parts/Door_Closed") || ((NetworkObject)o).Metadata.Contains("Puzzle_Parts/Door_Counter")) && !o.IsOpened && ExilePather.IsWalkable(((NetworkObject)o).Position) && ((NetworkObject)o).Distance < (float)int_0
					select o).ToList();
				if (lockedDoors.Any())
				{
					foreach (TriggerableBlockage lockedDoor in lockedDoors)
					{
						if (((NetworkObject)lockedDoor).Distance < 10f)
						{
							Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
							Vector2i pos = ((Vector2i)(ref position)).GetPointAtDistanceBeforeThis(((NetworkObject)lockedDoor).Position, 10f);
							await Coroutines.FinishCurrentAction(true);
							Move.Towards(pos, "step away from door");
							await Coroutines.LatencyWait();
							await Coroutines.FinishCurrentAction(true);
						}
						ExilePather.PolyPathfinder.AddObstacle(((NetworkObject)lockedDoor).Position, 5f);
					}
					ExilePather.PolyPathfinder.UpdateObstacles();
					return false;
				}
				return false;
			}
			await PlayerAction.Interact((NetworkObject)(object)TrialDoor, 3);
			if (!(await Wait.For(() => TrialDoor.IsOpened, "Trial Door")))
			{
				ilog_0.ErrorFormat("[DeadlyTrialsTask] Can't open Trial Door.", Array.Empty<object>());
				return false;
			}
			return true;
		}
		return false;
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

	static TrialDoorTask()
	{
		ilog_0 = Logger.GetLoggerInstanceForType();
	}
}
