using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;

namespace ExPlugins.SimulacrumPluginEx.Tasks;

public class StartSimulacrumTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static readonly SimulSett Config;

	public static AflictionInitiator Afflictionator => ObjectManager.Objects.FirstOrDefault<AflictionInitiator>();

	private static NetworkObject Stash => (NetworkObject)(object)ObjectManager.Objects.FirstOrDefault<Stash>();

	public static CachedObject CachedAfflictionator
	{
		get
		{
			return CombatAreaCache.Current.Storage["CachedAfflictionator"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CachedAfflictionator"] = value;
		}
	}

	public static bool SimulacrumWaveStarted => World.CurrentArea.Id.Contains("Affliction") && Stash == (NetworkObject)null && CachedAfflictionator != null && Afflictionator.CurrentWave == -1 && InstanceInfo.MonstersRemaining > 0;

	public string Name => "StartSimulacrumTask";

	public string Description => "Task starts Simulacrum event";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (GeneralSettings.SimulacrumsEnabled)
			{
				DatWorldAreaWrapper area = World.CurrentArea;
				if (!area.Id.Contains("Affliction"))
				{
					return false;
				}
				if (CachedAfflictionator == null || Inventories.CachedStash == null)
				{
					WalkablePosition stashPos = SimulacrumPluginEx.GetCoords(area.Name, findStash: true);
					if (stashPos != null && stashPos.AsVector != Vector2i.Zero)
					{
						stashPos.TryCome();
					}
					return true;
				}
				if (!(Stash != (NetworkObject)null) || !((NetworkObject)(object)Afflictionator == (NetworkObject)null) || SimulacrumPluginEx.CurrentWave < 29 || SimulacrumPluginEx.CurrentWave == 30)
				{
					if (SimulacrumWaveStarted)
					{
						return false;
					}
					await Coroutines.CloseBlockingWindows();
					if (SimulacrumPluginEx.CurrentWave >= Config.MaxWave)
					{
						WalkablePosition stashPos2 = SimulacrumPluginEx.GetCoords(area.Name, findStash: true);
						if (!(stashPos2.PathDistance > 45f))
						{
							if (Stash != (NetworkObject)null)
							{
								GlobalLog.Warn($"[{Name}] Current wave {SimulacrumPluginEx.CurrentWave} >= {Config.MaxWave}. Skip!");
								GlobalLog.Warn(string.Format(arg1: await ((TaskManagerBase<ITask>)(object)SimulacrumPluginEx.BotTaskManager).GetTaskByName("LootItemTask").Run(), format: "[{0}] Looting items. {1}", arg0: Name));
								GeneralSettings.Instance.IsOnRun = false;
							}
							return false;
						}
						await stashPos2.TryComeAtOnce();
						return true;
					}
					if (!(CachedAfflictionator != null) || !(CachedAfflictionator.Object != (NetworkObject)null) || !CachedAfflictionator.Object.IsTargetable)
					{
						return false;
					}
					if (!Config.StopRequest)
					{
						List<CachedWorldItem> validItems = CombatAreaCache.Current.Items.FindAll((CachedWorldItem i) => !i.Ignored && !i.Unwalkable);
						if (!validItems.Any())
						{
							WalkablePosition afflictionatorPos = CachedAfflictionator.Position;
							if (afflictionatorPos.PathDistance > 35f)
							{
								GlobalLog.Debug("[" + Name + "] Moving closer to Afflictionator.");
								if (!afflictionatorPos.TryCome())
								{
									GlobalLog.Error("[" + Name + "] Afflictionator is unwalkable!");
									return true;
								}
								return true;
							}
							if ((NetworkObject)(object)Afflictionator != (NetworkObject)null)
							{
								Afflictionator.ShowUi();
								await Wait.LatencySleep();
								await Coroutines.ReactionWait();
								SimulacrumPluginEx.CurrentWave = Afflictionator.CurrentWave;
								GlobalLog.Warn($"[{Name}] Current wave: {SimulacrumPluginEx.CurrentWave}/{Afflictionator.MaxWave}");
								if (await PlayerAction.Interact((NetworkObject)(object)Afflictionator, () => Afflictionator.CurrentWave == -1, "wave to start", 2000))
								{
									Utility.BroadcastMessage((object)this, "simulacrum_wave_start", new object[1] { $"Started simulacrum wave ({SimulacrumPluginEx.CurrentWave})" });
									await Coroutines.CloseBlockingWindows();
									StayInSimulacrumTask.stopwatch_0.Restart();
									ExilePather.Reload(true);
									CombatAreaCache.Current.Explorer.BasicExplorer.Reset();
									CombatAreaCache.Current.Explorer.Finished = false;
									ErrorManager.Reset();
									StuckDetection.Reset();
									PrepareForWaveTask.Active = true;
								}
							}
							return true;
						}
						return true;
					}
					Config.StopRequest = false;
					await Coroutines.CloseBlockingWindows();
					BotManager.Stop(new StopReasonData("simulacrum_user_stop", "Stopped in simulacrum by user demand", (object)null), false);
					return true;
				}
				SimulacrumPluginEx.CurrentWave = 30;
				return true;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (interval_0.Elapsed)
		{
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			if (currentArea.Id.Contains("Affliction") && CachedAfflictionator == null && (NetworkObject)(object)Afflictionator != (NetworkObject)null)
			{
				CachedAfflictionator = new CachedObject((NetworkObject)(object)Afflictionator);
			}
		}
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

	static StartSimulacrumTask()
	{
		interval_0 = new Interval(500);
		Config = SimulSett.Instance;
	}
}
