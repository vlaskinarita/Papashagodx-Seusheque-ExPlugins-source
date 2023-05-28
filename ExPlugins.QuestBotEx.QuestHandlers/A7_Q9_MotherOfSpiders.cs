using System.Collections.Generic;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q9_MotherOfSpiders
{
	private static bool bool_0;

	private static Monster Arakaali => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2144 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static NetworkObject ArakaaliRoomObj => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Terrain/Act7/Area12Level2/Objects/ArakaaliArenaMiddle");

	private static Dictionary<Vector2i, WalkablePosition> CachedArakaaliPositions
	{
		get
		{
			Dictionary<Vector2i, WalkablePosition> dictionary = CombatAreaCache.Current.Storage["ArakaaliPositions"] as Dictionary<Vector2i, WalkablePosition>;
			if (dictionary == null)
			{
				dictionary = new Dictionary<Vector2i, WalkablePosition>();
				CombatAreaCache.Current.Storage["ArakaaliPositions"] = dictionary;
			}
			return dictionary;
		}
	}

	public static void Tick()
	{
		bool_0 = World.Act8.SarnRamparts.IsWaypointOpened;
	}

	public static async Task<bool> KillArakaali()
	{
		if (!bool_0)
		{
			if (World.Act7.TempleOfDecay2.IsCurrentArea)
			{
				NetworkObject roomObj = ArakaaliRoomObj;
				if (roomObj != (NetworkObject)null)
				{
					Monster arakaali = Arakaali;
					if ((NetworkObject)(object)arakaali != (NetworkObject)null)
					{
						WalkablePosition pos = GetCachedWalkable(((NetworkObject)arakaali).Position);
						if (!(pos != null))
						{
							await Wait.StuckDetectionSleep(500);
						}
						else
						{
							await Helpers.MoveAndWait(pos, "Waiting for Arakaali", 10);
						}
						return true;
					}
					await Helpers.MoveAndWait(roomObj.WalkablePosition(), "Waiting for any Arakaali fight object");
					return true;
				}
			}
			await Travel.To(World.Act8.SarnRamparts);
			return true;
		}
		return false;
	}

	public static async Task<bool> EnterSarnEncampment()
	{
		if (World.Act8.SarnEncampment.IsCurrentArea)
		{
			if (World.Act8.SarnEncampment.IsWaypointOpened)
			{
				return false;
			}
			if (!(await PlayerAction.OpenWaypoint()))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		await Travel.To(World.Act8.SarnEncampment);
		return true;
	}

	public static WalkablePosition GetCachedWalkable(Vector2i pos)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (!CachedArakaaliPositions.TryGetValue(pos, out var value))
		{
			value = new WalkablePosition("walkable Arakaali position", pos, 5);
			if (value.Initialize())
			{
				GlobalLog.Warn($"[MotherOfSpiders] Registering walkable Arakaali position {value.AsVector}.");
				CachedArakaaliPositions.Add(pos, value);
				return value;
			}
			GlobalLog.Error($"[MotherOfSpiders] Cannot find walkable position for current Arakaali position {pos}.");
			return null;
		}
		return value;
	}
}
