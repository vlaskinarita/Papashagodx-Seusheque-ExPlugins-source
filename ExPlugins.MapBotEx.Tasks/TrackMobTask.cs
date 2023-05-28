using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.SqRoutine;

namespace ExPlugins.MapBotEx.Tasks;

public class TrackMobTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static int int_0;

	public string Name => "TrackMobTask";

	public string Description => "Task for tracking monsters.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!(World.CurrentArea.Name == MapNames.MaoKun))
		{
			return false;
		}
		return await TrackMobLogic.Execute(int_0);
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	internal static void RestrictRange()
	{
		GlobalLog.Info($"[TrackMobTask] Restricting monster tracking range to {100}");
		int_0 = 100;
		TrackMobLogic.CurrentTarget = null;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
		if (!interval_0.Elapsed)
		{
			return;
		}
		foreach (CachedMonster monster in CombatAreaCache.Current.Monsters)
		{
			Monster @object = monster.Object;
			if (!((NetworkObject)(object)@object == (NetworkObject)null))
			{
				if (((Actor)(object)@object).IsBlightMob())
				{
					TrackMobLogic.AddBlighted(monster);
				}
				if (((Actor)@object).IsStrongboxMinion || ((Actor)@object).IsBreachMonster || ((Actor)@object).IsHarbingerMinion)
				{
					TrackMobLogic.AddValuable(monster);
				}
			}
		}
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static TrackMobTask()
	{
		interval_0 = new Interval(1000);
		int_0 = -1;
	}
}
