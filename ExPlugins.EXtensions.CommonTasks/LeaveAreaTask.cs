using System;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.MapBotEx;

namespace ExPlugins.EXtensions.CommonTasks;

public class LeaveAreaTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static bool bool_0;

	public static AreaTransition TrialReturn => Enumerable.FirstOrDefault(ObjectManager.GetObjectsByType<AreaTransition>(), (AreaTransition o) => ((NetworkObject)o).Metadata.Contains("Metadata/QuestObjects/Labyrinth/LabyrinthTrialPortal"));

	public static bool IsActive
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			GlobalLog.Debug(value ? "[LeaveAreaTask] Activated." : "[LeaveAreaTask] Deactivated.");
		}
	}

	private static bool AnyMobsNearby => ObjectManager.Objects.Any((Monster m) => m.IsActive && ((NetworkObject)m).Distance <= 50f);

	public string Name => "LeaveAreaTask";

	public string Description => "Task to prematurely leave an area.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsCombatArea)
		{
			if (GeneralSettings.Instance.IsOnRun)
			{
				TimeSpan spent = Statistics.Instance.MapTimer.Elapsed;
				if (area.Id.Contains("Affliction"))
				{
					if (spent.TotalMinutes >= 60.0)
					{
						GlobalLog.Error($"[{Name}] We spent more than {spent:mm\\:ss} on {Statistics.Instance.CurrentMapName}. Seems like bot is stuck. Leaving map and marking it as complete");
						await PlayerAction.TpToTown();
						GeneralSettings.Instance.IsOnRun = false;
						return true;
					}
				}
				else if (spent.TotalMinutes >= 20.0)
				{
					GlobalLog.Error($"[{Name}] We spent more than {spent:mm\\:ss} on {Statistics.Instance.CurrentMapName}. Seems like bot is stuck. Leaving map and marking it as complete");
					await PlayerAction.TpToTown();
					GeneralSettings.Instance.IsOnRun = false;
					return true;
				}
			}
			if (!((NetworkObject)(object)TrialReturn != (NetworkObject)null) || !area.Id.Contains("EndGame_Labyrinth_trials"))
			{
				if (!IsActive)
				{
					return false;
				}
				if (AnyMobsNearby)
				{
					GlobalLog.Warn("[LeaveAreaTask] Now logging out because there are monsters nearby.");
					if (!(await PlayerAction.Logout()))
					{
						ErrorManager.ReportError();
						return true;
					}
				}
				else
				{
					GlobalLog.Debug("[LeaveAreaTask] Now leaving current area.");
					if (!(await PlayerAction.TpToTown(forceNewPortal: true)))
					{
						ErrorManager.ReportError();
						return true;
					}
				}
				IsActive = false;
				return true;
			}
			GlobalLog.Error(string.Format(arg1: ((NetworkObject)(object)TrialReturn).WalkablePosition(), format: "[{0}] Detected {1}! Seems like we entered Lab trial somehow. Trying to leave it", arg0: Name));
			if (!(await PlayerAction.TakeTransition(TrialReturn)))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
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
}
