using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class TransitionTriggerTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static string string_0;

	private static CachedObject cachedObject_0;

	private static int int_0;

	public string Name => "TransitionTriggerTask";

	public string Description => "Task that opens transition triggers.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (string_0 != null && !(cachedObject_0 == null) && !MapExplorationTask.MapCompleted)
		{
			if (World.CurrentArea.IsMap)
			{
				if (World.CurrentArea.Name == MapNames.Core && KillBossTask.BossesKilled < 3)
				{
					return false;
				}
				WalkablePosition pos = cachedObject_0.Position;
				if (pos.IsFar)
				{
					if (!pos.TryCome())
					{
						GlobalLog.Error($"[TransitionTriggerTask] Fail to move to {pos}. Transition trigger is unwalkable. Bot will not be able to enter a bossroom.");
						string_0 = null;
					}
					return true;
				}
				NetworkObject networkObject_0 = cachedObject_0.Object;
				if (networkObject_0 == (NetworkObject)null || !networkObject_0.IsTargetable)
				{
					if (CombatAreaCache.Current.AreaTransitions.Any((CachedTransition a) => a.Type == TransitionType.Local && a.Position.Distance <= 50))
					{
						GlobalLog.Debug("[TransitionTriggerTask] Area transition has been successfully unlocked.");
						string_0 = null;
						return true;
					}
					int_0++;
					if (int_0 <= 50)
					{
						GlobalLog.Debug($"[TransitionTriggerTask] Waiting for area transition spawn {int_0}/{50}");
						await Wait.StuckDetectionSleep(200);
						return true;
					}
					GlobalLog.Error("[TransitionTriggerTask] Unexpected error. Waiting for area transition spawn timeout.");
					string_0 = null;
					return true;
				}
				if (++cachedObject_0.InteractionAttempts > 7)
				{
					GlobalLog.Error("[TransitionTriggerTask] All attempts to interact with transition trigger have been spent.");
					string_0 = null;
					return true;
				}
				if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "transition trigger interaction", 500)))
				{
					await Wait.SleepSafe(500);
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (string_0 == null || cachedObject_0 != null || MapExplorationTask.MapCompleted || !interval_0.Elapsed || !LokiPoe.IsInGame || !World.CurrentArea.IsMap)
		{
			return;
		}
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			if (@object.IsTargetable && @object.Metadata == string_0)
			{
				WalkablePosition walkablePosition = @object.WalkablePosition();
				GlobalLog.Warn($"[TransitionTriggerTask] Registering {walkablePosition}");
				cachedObject_0 = new CachedObject(@object.Id, walkablePosition);
				break;
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "MB_new_map_entered_event")
		{
			GlobalLog.Info("[TransitionTriggerTask] Reset.");
			Reset(message.GetInput<string>(0));
			if (string_0 != null)
			{
				GlobalLog.Info("[TransitionTriggerTask] Enabled.");
			}
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static void Reset(string areaName)
	{
		string_0 = null;
		cachedObject_0 = null;
		int_0 = 0;
		if (!MapData.Current.IgnoreBossroom)
		{
			if (!(areaName == MapNames.Academy) && !(areaName == MapNames.Museum) && !(areaName == MapNames.Scriptorium))
			{
				if (areaName == MapNames.GraveTrough)
				{
					string_0 = "Metadata/Terrain/EndGame/MapBurn/Objects/BossEventSarcophagus";
				}
				else if (!(areaName == MapNames.Core))
				{
					if (!(areaName == MapNames.Necropolis))
					{
						if (areaName == MapNames.WastePool)
						{
							string_0 = "Metadata/QuestObjects/Sewers/SewersGrate";
						}
					}
					else
					{
						string_0 = "Metadata/Chests/Sarcophagi/sarcophagus_door";
					}
				}
				else
				{
					string_0 = "Metadata/QuestObjects/Act4/CoreMouthMap";
				}
			}
			else
			{
				string_0 = "Metadata/QuestObjects/Library/HiddenDoorTrigger";
			}
		}
		else
		{
			GlobalLog.Info("[TransitionTriggerTask] Skipping this task because bossroom is ignored.");
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

	static TransitionTriggerTask()
	{
		interval_0 = new Interval(200);
	}
}
