using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public class OpenWaypointTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static WalkablePosition walkablePosition_0;

	private static bool bool_0;

	private static bool bool_1;

	private static WalkablePosition CachedWaypointPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["WaypointPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["WaypointPosition"] = value;
		}
	}

	public string Name => "OpenWaypointTask";

	public string Description => "Task that handles waypoint opening.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!bool_1 || !World.CurrentArea.IsOverworldArea)
		{
			return false;
		}
		WalkablePosition wpPos = CachedWaypointPos;
		if (!(wpPos != null))
		{
			if (walkablePosition_0 == null)
			{
				WorldPosition pos = Tgt.FindWaypoint();
				if (pos == null)
				{
					GlobalLog.Error("[OpenWaypointTask] Fail to find any walkable waypoint tgt. Skipping this task for \"" + World.CurrentArea.Name + "\".");
					bool_1 = false;
					return true;
				}
				walkablePosition_0 = new WalkablePosition("Waypoint location", pos);
			}
			walkablePosition_0.Come();
			return true;
		}
		if (wpPos.IsFar)
		{
			wpPos.Come();
			return true;
		}
		if (await PlayerAction.OpenWaypoint())
		{
			bool_1 = false;
			await Coroutines.CloseBlockingWindows();
			return true;
		}
		ErrorManager.ReportError();
		return true;
	}

	public void Tick()
	{
		if ((!bool_1 && !bool_0) || !interval_0.Elapsed || !LokiPoe.IsInGame || !World.CurrentArea.IsOverworldArea || CachedWaypointPos != null)
		{
			return;
		}
		NetworkObject waypoint = ObjectManager.Waypoint;
		if (waypoint != (NetworkObject)null)
		{
			CachedWaypointPos = waypoint.WalkablePosition();
		}
		if (!bool_0)
		{
			return;
		}
		if (!(waypoint != (NetworkObject)null))
		{
			if (ObjectManager.Objects.Exists((NetworkObject o) => o is AreaTransition && o.Name == World.Act3.UpperSceptreOfGod.Name))
			{
				GlobalLog.Warn("[OpenWaypointTask] Enabled (Upper Sceptre of God transition detected)");
				bool_1 = true;
				bool_0 = false;
			}
		}
		else
		{
			GlobalLog.Warn("[OpenWaypointTask] Enabled (waypoint object detected)");
			bool_1 = true;
			bool_0 = false;
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "combat_area_changed_event")
		{
			walkablePosition_0 = null;
			bool_1 = false;
			bool_0 = false;
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			string id = currentArea.Id;
			if (currentArea.IsOverworldArea && currentArea.HasWaypoint && !World.IsWaypointOpened(id))
			{
				if (id == World.Act3.SceptreOfGod.Id)
				{
					bool_0 = true;
				}
				else if (!BlockedByBoss(id))
				{
					GlobalLog.Warn("[OpenWaypointTask] Enabled.");
					bool_1 = true;
				}
			}
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static bool BlockedByBoss(string areaId)
	{
		return areaId == World.Act5.CathedralRooftop.Id || areaId == World.Act8.DoedreCesspool.Id;
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

	static OpenWaypointTask()
	{
		interval_0 = new Interval(500);
	}
}
