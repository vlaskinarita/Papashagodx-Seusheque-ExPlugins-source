using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q1_ReturnToOriath
{
	private static readonly TgtPosition tgtPosition_0;

	private static NetworkObject DeviceLever => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Terrain/Act4/Area7/Objects/PortalDeviceLever");

	private static AreaTransition OriathTransition => ObjectManager.Objects.FirstOrDefault((AreaTransition a) => ((NetworkObject)a).Metadata == "Metadata/Terrain/Act4/Area7/Objects/PortalDeviceTransition");

	private static Monster OverseerKrow => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1985 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static CachedObject CachedDeviceLever
	{
		get
		{
			return CombatAreaCache.Current.Storage["DeviceLever"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["DeviceLever"] = value;
		}
	}

	private static WalkablePosition CachedOverseerKrowPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["OverseerKrowPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["OverseerKrowPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act4.Ascent.IsCurrentArea)
		{
			if (CachedDeviceLever == null)
			{
				NetworkObject deviceLever = DeviceLever;
				if (deviceLever != (NetworkObject)null)
				{
					CachedDeviceLever = new CachedObject(deviceLever);
				}
			}
		}
		else if (World.Act5.SlavePens.IsCurrentArea)
		{
			Monster overseerKrow = OverseerKrow;
			if ((NetworkObject)(object)overseerKrow != (NetworkObject)null)
			{
				CachedOverseerKrowPos = (((Actor)overseerKrow).IsDead ? null : ((NetworkObject)(object)overseerKrow).WalkablePosition());
			}
		}
	}

	public static async Task<bool> EnterOverseerTower()
	{
		if (!World.Act5.OverseerTower.IsCurrentArea)
		{
			if (!World.Act4.Ascent.IsCurrentArea)
			{
				if (!World.Act5.SlavePens.IsCurrentArea)
				{
					if (World.Act5.SlavePens.IsWaypointOpened)
					{
						await Travel.To(World.Act5.SlavePens);
						return true;
					}
					await Travel.To(World.Act4.Ascent);
					return true;
				}
				WalkablePosition krowPos = CachedOverseerKrowPos;
				if (krowPos != null)
				{
					await Helpers.MoveAndWait(krowPos);
					return true;
				}
				await Travel.To(World.Act5.OverseerTower);
				return true;
			}
			CachedObject lever = CachedDeviceLever;
			if (lever != null)
			{
				WalkablePosition pos = lever.Position;
				if (!pos.IsFar)
				{
					NetworkObject networkObject_0 = lever.Object;
					if (!networkObject_0.IsTargetable)
					{
						AreaTransition transition = OriathTransition;
						if (!((NetworkObject)(object)transition != (NetworkObject)null) || !((NetworkObject)transition).IsTargetable)
						{
							GlobalLog.Debug("Waiting for portal to Oriath");
							await Wait.StuckDetectionSleep(500);
							return true;
						}
						if (!(await PlayerAction.TakeTransition(transition)))
						{
							ErrorManager.ReportError();
						}
						return true;
					}
					if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Device lever interaction")))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				pos.Come();
				return true;
			}
			tgtPosition_0.Come();
			return true;
		}
		if (World.Act5.OverseerTower.IsWaypointOpened)
		{
			return false;
		}
		if (!(await PlayerAction.OpenWaypoint()))
		{
			ErrorManager.ReportError();
		}
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Lani, "Overseer Reward", Quests.ReturnToOriath.Id, null, shouldLogOut: true);
	}

	static A5_Q1_ReturnToOriath()
	{
		tgtPosition_0 = new TgtPosition("Portal device location", "ascent_summit_v01_01.tgt");
	}
}
