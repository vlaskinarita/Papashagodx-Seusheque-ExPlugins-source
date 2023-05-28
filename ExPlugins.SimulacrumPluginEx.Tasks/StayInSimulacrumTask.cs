using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;

namespace ExPlugins.SimulacrumPluginEx.Tasks;

public class StayInSimulacrumTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly SimulSett Config;

	internal static readonly Stopwatch stopwatch_0;

	private readonly Interval interval_0 = new Interval(15000);

	private readonly Interval interval_1 = new Interval(10000);

	private readonly Interval interval_2 = new Interval(500);

	private Monster monster_0;

	public string Name => "StayInSimulacrumTask";

	public string Description => "Task stays in Simulacrum";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (GeneralSettings.SimulacrumsEnabled)
			{
				DatWorldAreaWrapper area = World.CurrentArea;
				if (area.Id.Contains("Affliction"))
				{
					if (StartSimulacrumTask.SimulacrumWaveStarted)
					{
						if (CombatAreaCache.Current.Explorer.BasicExplorer.TileSeenRadius != 1)
						{
							CombatAreaCache.Current.Explorer.BasicExplorer.TileSeenRadius = 1;
						}
						if (CombatAreaCache.Current.Explorer.BasicExplorer.TileKnownRadius != 3)
						{
							CombatAreaCache.Current.Explorer.BasicExplorer.TileKnownRadius = 3;
						}
						bool noMobsAround = ObjectManager.GetObjectsByType<Monster>().Any((Monster m) => m.IsAliveHostile && ((NetworkObject)m).Distance < 140f);
						if (stopwatch_0.ElapsedMilliseconds > 40000L || (noMobsAround && stopwatch_0.ElapsedMilliseconds > 15000L))
						{
							if (await TrackMobLogic.Execute(-1, trackAll: true, !Config.KillBossAtTheEnd))
							{
								return true;
							}
							if (await CombatAreaCache.Current.Explorer.Execute(basic: true))
							{
								return true;
							}
							if ((NetworkObject)(object)monster_0 != (NetworkObject)null && ((NetworkObject)monster_0).Distance > 10f)
							{
								WalkablePosition walkPos = ((NetworkObject)(object)monster_0).WalkablePosition();
								GlobalLog.Debug($"[{Name}] Tracking mobs: {walkPos}");
								walkPos.TryCome();
								return true;
							}
							monster_0 = null;
							if (Config.EnableAnchorPoints)
							{
								WalkablePosition point2 = SimulacrumPluginEx.GetCoords(area.Name);
								if (point2 != null && point2.Distance > 25)
								{
									point2.TryCome();
								}
							}
							return true;
						}
						if (Config.EnableAnchorPoints)
						{
							WalkablePosition point = SimulacrumPluginEx.GetCoords(area.Name);
							if (point != null && point.Distance > 25)
							{
								point.TryCome();
							}
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (!interval_2.Elapsed)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.Id.Contains("Affliction"))
		{
			return;
		}
		if (interval_0.Elapsed)
		{
			monster_0 = null;
		}
		if (interval_1.Elapsed && StartSimulacrumTask.SimulacrumWaveStarted && !CombatAreaCache.Current.Explorer.BasicExplorer.HasLocation)
		{
			ExilePather.Reload(true);
			CombatAreaCache.Current.Explorer.BasicExplorer.Reset();
			CombatAreaCache.Current.Explorer.Finished = false;
		}
		if (!((NetworkObject)(object)monster_0 == (NetworkObject)null))
		{
			return;
		}
		List<Monster> source = (from m in ObjectManager.GetObjectsByType<Monster>()
			where m.IsAliveHostile && ((NetworkObject)m).Position != new Vector2i(0, 0)
			select m).ToList();
		Monster val = source.OrderByDescending((Monster m) => m.Rarity).FirstOrDefault();
		if (Config.KillBossAtTheEnd)
		{
			val = source.OrderBy((Monster m) => m.Rarity).FirstOrDefault();
		}
		Monster val2 = source.OrderByDescending((Monster m) => ((NetworkObject)m).Distance).FirstOrDefault();
		if ((NetworkObject)(object)val != (NetworkObject)null)
		{
			monster_0 = val;
		}
		monster_0 = val2;
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

	static StayInSimulacrumTask()
	{
		Config = SimulSett.Instance;
		stopwatch_0 = Stopwatch.StartNew();
	}
}
