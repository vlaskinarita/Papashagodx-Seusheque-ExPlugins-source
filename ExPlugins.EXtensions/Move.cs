using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions;

public static class Move
{
	private static readonly Interval interval_0;

	public static bool Towards(Vector2i pos, string destination)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (interval_0.Elapsed)
		{
			object arg = pos;
			Vector2i myPosition = LokiPoe.MyPosition;
			GlobalLog.Debug($"[MoveTowards] Moving towards {destination} at {arg} (distance: {((Vector2i)(ref myPosition)).Distance(pos)})");
		}
		if (PlayerMoverManager.MoveTowards(pos, (object)null))
		{
			return true;
		}
		GlobalLog.Error($"[MoveTowards] Fail to move towards {destination} at {pos}");
		return false;
	}

	public static void TowardsWalkable(Vector2i pos, string destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (!Towards(pos, destination))
		{
			GlobalLog.Error($"[MoveTowardsWalkable] Unexpected error. Fail to move towards {destination} at {pos}");
			ErrorManager.ReportError();
		}
	}

	public static async Task AtOnce(Vector2i pos, string destination, int minDistance = 20)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Vector2i myPosition = LokiPoe.MyPosition;
		if (((Vector2i)(ref myPosition)).Distance(pos) <= minDistance)
		{
			await Coroutines.FinishCurrentAction(true);
			return;
		}
		while (true)
		{
			myPosition = LokiPoe.MyPosition;
			if (((Vector2i)(ref myPosition)).Distance(pos) <= minDistance)
			{
				break;
			}
			if (interval_0.Elapsed)
			{
				await Coroutines.CloseBlockingWindows();
				object arg = pos;
				myPosition = LokiPoe.MyPosition;
				GlobalLog.Debug($"[MoveAtOnce] Moving to {destination} at {arg} (distance: {((Vector2i)(ref myPosition)).Distance(pos)})");
			}
			if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead && !BotManager.IsStopping)
			{
				if (!(await OpenDoor()))
				{
					GlobalLog.Error($"[MoveAtOnce] Unable to open door while moving to {destination} at {pos}");
					ErrorManager.ReportError();
				}
				else
				{
					TowardsWalkable(pos, destination);
					await Wait.Sleep(50);
				}
				continue;
			}
			return;
		}
		await Coroutines.FinishCurrentAction(true);
	}

	private static async Task<bool> OpenDoor()
	{
		TriggerableBlockage mYrZhsXcEo = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)IsClosedDoor);
		if ((NetworkObject)(object)mYrZhsXcEo == (NetworkObject)null)
		{
			return true;
		}
		if (await PlayerAction.Interact((NetworkObject)(object)mYrZhsXcEo))
		{
			await Wait.For(() => !((NetworkObject)mYrZhsXcEo).IsTargetable || mYrZhsXcEo.IsOpened, "door opening", 50, 300);
			return true;
		}
		await Wait.SleepSafe(300);
		return false;
	}

	private static bool IsClosedDoor(TriggerableBlockage d)
	{
		return ((NetworkObject)d).IsTargetable && !d.IsOpened && ((NetworkObject)d).Distance <= 25f && (((NetworkObject)d).Name == "Door" || ((NetworkObject)d).Metadata == "Metadata/MiscellaneousObjects/Smashable" || ((NetworkObject)d).Metadata.Contains("LabyrinthSmashableDoor"));
	}

	public static bool ClosedDoorBetween(Vector2i start, Vector2i end, int distanceFromPoint = 10, int stride = 10, bool dontLeaveFrame = false)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		List<Vector2i> list = (from d in Enumerable.Where(ObjectManager.AnyDoors, (TriggerableBlockage d) => !d.IsOpened)
			select ((NetworkObject)d).Position).ToList();
		if (list.Any())
		{
			List<Vector2i> pointsOnSegment = ExilePather.GetPointsOnSegment(start, end, dontLeaveFrame);
			for (int i = 0; i < pointsOnSegment.Count; i += stride)
			{
				foreach (Vector2i item in list)
				{
					Vector2i current = item;
					if (((Vector2i)(ref current)).Distance(pointsOnSegment[i]) <= distanceFromPoint)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	static Move()
	{
		interval_0 = new Interval(1000);
	}
}
