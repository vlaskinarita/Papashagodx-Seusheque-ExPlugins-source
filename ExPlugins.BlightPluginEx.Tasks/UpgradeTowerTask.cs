using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BlightPluginEx.Classes;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.BlightPluginEx.Tasks;

public class UpgradeTowerTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	internal static Stopwatch stopwatch_0;

	internal static readonly Stopwatch stopwatch_1;

	private readonly Class53 class53_0 = Class53.Instance;

	private readonly Interval interval_0 = new Interval(3500);

	private WeightedTower weightedTower_0;

	public string Name => "UpgradeTowerTask";

	public string Description => "Task that defend the Blight pump.";

	public string Author => "Seusheque";

	public string Version => "2.0";

	public async Task<bool> Run()
	{
		Vector2i position;
		int mobsNearPump;
		bool bossNearPump;
		int num;
		if (LokiPoe.IsInGame)
		{
			if (World.CurrentArea.IsMap)
			{
				if (Blight.IsEncounterRunning && BlightUi.IsOpened && !HandleEventTask.SkipEncounter && !(BlightPluginEx.CachedBlightCore == null))
				{
					if (HandleEventTask.Comeback == null)
					{
						ITask task = ((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).GetTaskByName("HandleEventTask");
						await task.Run();
						return true;
					}
					if (!Blight.IsEncounterRunning || !VisibleTimersUi.IsOpened)
					{
						if (!VisibleTimersUi.IsOpened && BlightUi.RewardsList.Count((Reward r) => !r.LaneCleared) > 2)
						{
							HandleEventTask.EncounterTimeoutSwSmall.Restart();
						}
					}
					else
					{
						HandleEventTask.EncounterTimeoutSwSmall.Restart();
						HandleEventTask.EncounterTimeoutSwBig.Restart();
						StuckDetection.Reset();
					}
					if (!(BlightPluginEx.CachedBlightCore == null) && !HandleEventTask.SkipEncounter)
					{
						if (stopwatch_0 == null)
						{
							stopwatch_0 = Stopwatch.StartNew();
						}
						position = ((NetworkObject)LokiPoe.Me).Position;
						if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) < 30)
						{
							HandleEventTask.Comeback.Restart();
							stopwatch_0.Restart();
						}
						if (class53_0.PrioritizeBosses && stopwatch_0.Elapsed.TotalSeconds < (double)(class53_0.TowerUpgradeTimeout * 2) && HandleEventTask.NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 100f) == 0)
						{
							List<CachedMonster> bosses = CombatAreaCache.Current.Monsters.Where((CachedMonster m) => (int)m.Rarity == 3).ToList();
							CachedMonster boss = null;
							if (bosses.Count <= 2)
							{
								boss = bosses.OrderBy((CachedMonster m) => m.Position.Distance).FirstOrDefault();
							}
							if (boss != null)
							{
								WalkablePosition bossPos = boss.Position;
								if (bossPos.Distance > 70)
								{
									bossPos.TryCome();
									return true;
								}
								if (weightedTower_0 == null || (NetworkObject)(object)weightedTower_0.TowerCached.Object == (NetworkObject)null)
								{
									weightedTower_0 = HandleEventTask.FindTowerToUpgrade(findClosest: true);
								}
								Class53.weightedTower_0 = weightedTower_0;
								if (weightedTower_0 != null)
								{
									await HandleEventTask.UpgradeTower(weightedTower_0);
								}
								return false;
							}
						}
						if (stopwatch_1.ElapsedMilliseconds < 4000L)
						{
							if (class53_0.DebugMode)
							{
								GlobalLog.Warn("[" + Name + "] Continue defending pump!");
							}
							return false;
						}
						mobsNearPump = HandleEventTask.NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 70f);
						bossNearPump = HandleEventTask.NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 80f, boss: true) != 0;
						if (mobsNearPump != 0)
						{
							position = ((NetworkObject)LokiPoe.Me).Position;
							if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) > 25 && HandleEventTask.Comeback.ElapsedMilliseconds > 3500L)
							{
								num = 1;
								goto IL_049a;
							}
						}
						if (bossNearPump)
						{
							position = ((NetworkObject)LokiPoe.Me).Position;
							if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) > 25)
							{
								num = ((HandleEventTask.Comeback.ElapsedMilliseconds > 3500L) ? 1 : 0);
								goto IL_049a;
							}
						}
						num = 0;
						goto IL_049a;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
		IL_049a:
		bool goBackToPump = (byte)num != 0;
		int num2;
		if (stopwatch_0.ElapsedMilliseconds <= class53_0.TowerUpgradeTimeout * 1000)
		{
			num2 = 0;
		}
		else
		{
			position = ((NetworkObject)LokiPoe.Me).Position;
			num2 = ((((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) > 20) ? 1 : 0);
		}
		if (num2 != 0)
		{
			if (class53_0.DebugMode)
			{
				string name = Name;
				position = ((NetworkObject)LokiPoe.Me).Position;
				GlobalLog.Warn($"[{name}] ClosestBlightCore Distance {((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position)}. " + $"Forced comeback Elapsed Seconds: {stopwatch_0.Elapsed.TotalSeconds}");
			}
			ProcessHookManager.ClearAllKeyStates();
			await BlightPluginEx.CachedBlightCore.Position.TryComeAtOnce(10);
			HandleEventTask.Comeback.Restart();
			stopwatch_0.Restart();
			return false;
		}
		bool lowresNoMobs = !class53_0.PrioritizeTowerConstruction && Blight.Resources < 700 && HandleEventTask.NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 100f) == 0;
		if (LocalData.MapMods.ContainsKey((StatTypeGGG)10342) && HandleEventTask.EncounterSw.Elapsed.TotalSeconds < 45.0 && lowresNoMobs && await TrackMobLogic.Execute(-1, trackAll: true, byRarity: true, onlyBlighted: true))
		{
			return false;
		}
		if (!goBackToPump || BlightUi.IsStartBlightVisible)
		{
			if (class53_0.UpgradedTowers >= class53_0.MaxToBuild)
			{
				if (class53_0.DebugMode)
				{
					GlobalLog.Warn($"[{Name}] {class53_0.UpgradedTowers}/{class53_0.MaxToBuild} Towers upgraded. Staying at pump");
				}
				return false;
			}
			WeightedTower twr = (Class53.weightedTower_0 = HandleEventTask.FindTowerToUpgrade());
			bool farAndLongToUpgrade = twr == null || twr.Position.Distance > 38;
			if (!((bossNearPump || mobsNearPump > 3) && farAndLongToUpgrade))
			{
				if (!(mobsNearPump != 0 && farAndLongToUpgrade) || class53_0.PrioritizeTowerConstruction)
				{
					double max = 0.0 + (double)Blight.MaxPumpDurability;
					double actual = 0.0 + (double)Blight.ActualPumpDurability;
					double pumpDurabilityPct = actual / max * 100.0;
					if (!LocalData.MapMods.ContainsKey((StatTypeGGG)10342) || VisibleTimersUi.IsOpened || !(pumpDurabilityPct < 60.0))
					{
						if (Class53.weightedTower_0 == null || interval_0.Elapsed)
						{
							Class53.weightedTower_0 = HandleEventTask.FindTowerToUpgrade();
						}
						HandleEventTask.Enum1 res = await HandleEventTask.UpgradeTower(Class53.weightedTower_0);
						if (!class53_0.PrioritizeTowerConstruction)
						{
							return res switch
							{
								(HandleEventTask.Enum1)3 => HandleEventTask.NumberOfMobsAround((NetworkObject)(object)LokiPoe.Me, 50f) == 0, 
								(HandleEventTask.Enum1)1 => true, 
								_ => false, 
							};
						}
						return res != (HandleEventTask.Enum1)2;
					}
					return false;
				}
				Class53.weightedTower_0 = HandleEventTask.FindTowerToUpgrade(findClosest: true);
				if (class53_0.DebugMode)
				{
					GlobalLog.Warn("[" + Name + "] mobs near pump!");
				}
				position = ((NetworkObject)LokiPoe.Me).Position;
				if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) < 35)
				{
					stopwatch_0.Restart();
				}
				stopwatch_1.Restart();
				HandleEventTask.EncounterTimeoutSwSmall.Restart();
				return false;
			}
			Class53.weightedTower_0 = HandleEventTask.FindTowerToUpgrade(findClosest: true);
			if (class53_0.DebugMode)
			{
				GlobalLog.Warn("[" + Name + "] Defending pump!");
			}
			position = ((NetworkObject)LokiPoe.Me).Position;
			if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) < 35)
			{
				stopwatch_0.Restart();
			}
			stopwatch_1.Restart();
			HandleEventTask.EncounterTimeoutSwSmall.Restart();
			return false;
		}
		if (class53_0.DebugMode)
		{
			string name2 = Name;
			position = ((NetworkObject)LokiPoe.Me).Position;
			GlobalLog.Warn($"[{name2}] ClosestBlightCore Distance {((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position)}. " + $"PumpStopwatch Elapsed Seconds: {HandleEventTask.Comeback.Elapsed.TotalSeconds}");
		}
		ProcessHookManager.ClearAllKeyStates();
		await BlightPluginEx.CachedBlightCore.Position.TryComeAtOnce(10);
		HandleEventTask.Comeback.Restart();
		stopwatch_0.Restart();
		return false;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Tick()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static UpgradeTowerTask()
	{
		stopwatch_1 = Stopwatch.StartNew();
	}
}
