using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A8_Q6_LunarEclipse
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static NetworkObject networkObject_0;

	private static Monster monster_0;

	private static Monster monster_1;

	private static bool bool_0;

	private static Monster Dusk => ObjectManager.GetObjectsByType<Monster>().FirstOrDefault((Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata == "Metadata/Monsters/LunarisSolaris/LunarisCelestialFormDusk");

	private static Monster Dawn => ObjectManager.GetObjectsByType<Monster>().FirstOrDefault((Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata == "Metadata/Monsters/GuardianOfSolaris/GuardianOfSolaris");

	public static void Tick()
	{
		bool_0 = World.Act9.BloodAqueduct.IsWaypointOpened;
	}

	public static async Task<bool> GrabMoonOrb()
	{
		if (!Helpers.PlayerHasQuestItem("LunarisOrb"))
		{
			if (World.Act8.LunarisTemple2.IsCurrentArea)
			{
				if (!(await PickupOrb("LunarisOrb")))
				{
					Monster dusk = Dusk;
					if ((NetworkObject)(object)dusk != (NetworkObject)null)
					{
						await Helpers.MoveAndWait(((NetworkObject)(object)dusk).WalkablePosition());
						return true;
					}
					await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
					return true;
				}
				return true;
			}
			await Travel.To(World.Act8.LunarisTemple2);
			return true;
		}
		return false;
	}

	public static async Task<bool> GrabSunOrb()
	{
		if (Helpers.PlayerHasQuestItem("SolarisOrb"))
		{
			return false;
		}
		if (World.Act8.SolarisTemple2.IsCurrentArea)
		{
			if (await PickupOrb("SolarisOrb"))
			{
				return true;
			}
			Monster dawn = Dawn;
			if ((NetworkObject)(object)dawn != (NetworkObject)null)
			{
				await Helpers.MoveAndWait(((NetworkObject)(object)dawn).WalkablePosition());
				return true;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_1);
			return true;
		}
		await Travel.To(World.Act8.SolarisTemple2);
		return true;
	}

	public static async Task<bool> PickupOrb(string name)
	{
		WorldItem orb = ObjectManager.GetObjectsByType<WorldItem>().FirstOrDefault((WorldItem i) => i.Item.Metadata.ContainsIgnorecase(name));
		if ((NetworkObject)(object)orb != (NetworkObject)null && (RemoteMemoryObject)(object)orb.Item != (RemoteMemoryObject)null && InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)0).CanFitItem(orb.Item.Size))
		{
			GlobalLog.Debug("[A8_Q6_LunarEclipse] " + name + " detected, trying to pickup");
			WalkablePosition orbPos = ((NetworkObject)(object)orb).WalkablePosition();
			if (orbPos.IsFar)
			{
				await orbPos.ComeAtOnce();
				return true;
			}
			if (await PlayerAction.Interact((NetworkObject)(object)orb))
			{
				await Coroutines.ReactionWait();
				await Coroutines.LatencyWait();
			}
			return true;
		}
		return false;
	}

	public static async Task<bool> KillSolarisLunaris()
	{
		if (!bool_0)
		{
			if (World.Act8.HarbourBridge.IsCurrentArea)
			{
				UpdateSolarisLunarisFightObjects();
				if (networkObject_0 != (NetworkObject)null)
				{
					if (!((NetworkObject)(object)monster_0 != (NetworkObject)null) || !monster_0.IsActive)
					{
						if (!((NetworkObject)(object)monster_1 != (NetworkObject)null) || !monster_1.IsActive)
						{
							if (!networkObject_0.IsTargetable)
							{
								GlobalLog.Debug("Waiting for any Solaris and Lunaris fight object");
								await Wait.StuckDetectionSleep(500);
								return true;
							}
							await networkObject_0.WalkablePosition().ComeAtOnce();
							if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Statue interaction")))
							{
								ErrorManager.ReportError();
							}
							return true;
						}
						await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
						return true;
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_0).WalkablePosition());
					return true;
				}
				AreaTransition t = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => (int)a.TransitionType == 1 && ((NetworkObject)a).Name == "The Sky Shrine" && !((NetworkObject)a).Metadata.Contains("IncursionPortal") && !((NetworkObject)a).Metadata.Contains("SynthesisPortal")));
				if ((NetworkObject)(object)t != (NetworkObject)null)
				{
					GlobalLog.Debug($"Should take {((NetworkObject)t).Name} distance: {((NetworkObject)t).Distance}");
					if (!(await PlayerAction.TakeTransition(t)))
					{
						ErrorManager.ReportError();
						return true;
					}
					return true;
				}
			}
			await Travel.To(World.Act9.BloodAqueduct);
			return true;
		}
		return false;
	}

	public static async Task<bool> EnterHighgate()
	{
		if (World.Act9.Highgate.IsCurrentArea)
		{
			if (World.Act9.Highgate.IsWaypointOpened)
			{
				return false;
			}
			if (!(await PlayerAction.OpenWaypoint()))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		if (World.Act8.HarbourBridge.IsCurrentArea && World.Act9.BloodAqueduct.IsWaypointOpened)
		{
			GlobalLog.Debug("waypoint detected, logout");
		}
		await Travel.To(World.Act9.Highgate);
		return true;
	}

	private static void UpdateSolarisLunarisFightObjects()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		networkObject_0 = null;
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			string metadata = @object.Metadata;
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && (int)val.Rarity == 3)
			{
				if (metadata == "Metadata/Monsters/LunarisSolaris/Solaris")
				{
					monster_0 = val;
				}
				else if (metadata == "Metadata/Monsters/LunarisSolaris/Lunaris")
				{
					monster_1 = val;
				}
			}
			else if (metadata == "Metadata/QuestObjects/Act8/GoddessFightStarter")
			{
				networkObject_0 = @object;
			}
		}
	}

	static A8_Q6_LunarEclipse()
	{
		tgtPosition_0 = new TgtPosition("Dusk room", "templeclean_prepiety_roundtop_center_01.tgt");
		tgtPosition_1 = new TgtPosition("Dawn room", "gemling_queen_throne_v01_01.tgt");
	}
}
