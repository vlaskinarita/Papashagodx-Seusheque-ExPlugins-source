using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Components;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q7_BrineKing
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static bool bool_0;

	private static bool bool_1;

	public static int? OriginalCombatRange;

	private static Chest FlagChest => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2578 }).FirstOrDefault<Chest>();

	private static Monster BrineKing => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2070 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Npc BrineKingWeylam => ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Metadata == "Metadata/NPC/Act6/WeylamReef2");

	private static List<CachedObject> CachedFuelCarts
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["FuelCarts"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(2);
				CombatAreaCache.Current.Storage["FuelCarts"] = list;
			}
			return list;
		}
	}

	private static CachedObject CachedBeaconSwitch
	{
		get
		{
			return CombatAreaCache.Current.Storage["BeaconSwitch"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["BeaconSwitch"] = value;
		}
	}

	private static CachedObject CachedBeaconLighter
	{
		get
		{
			return CombatAreaCache.Current.Storage["BeaconLighter"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["BeaconLighter"] = value;
		}
	}

	private static CachedObject CachedBeaconWeylam
	{
		get
		{
			return CombatAreaCache.Current.Storage["BeaconWeylam"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["BeaconWeylam"] = value;
		}
	}

	public static void Tick()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (!World.Act6.Beacon.IsCurrentArea)
		{
			return;
		}
		int stateInaccurate = QuestManager.GetStateInaccurate(Quests.BrineKing);
		bool_0 = stateInaccurate <= 7;
		bool_1 = stateInaccurate != 7;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			switch (@object.Metadata)
			{
			case "Metadata/QuestObjects/Act6/BeaconPayload":
				if (!IsDelivered(@object))
				{
					int int_0 = @object.Id;
					WalkablePosition walkablePosition = new WalkablePosition("Fuel cart", @object.Position);
					List<CachedObject> cachedFuelCarts = CachedFuelCarts;
					CachedObject cachedObject = cachedFuelCarts.Find((CachedObject p) => p.Id == int_0);
					if (cachedObject == null)
					{
						GlobalLog.Warn($"[BrineKing] Registering {walkablePosition}");
						cachedFuelCarts.Add(new CachedObject(int_0, walkablePosition));
					}
					else
					{
						cachedObject.Position = walkablePosition;
					}
				}
				break;
			case "Metadata/NPC/Act6/WeylamBeacon":
				if (CachedBeaconWeylam == null)
				{
					CachedBeaconWeylam = new CachedObject(@object);
				}
				break;
			case "Metadata/QuestObjects/Act6/BlackCrest_BeaconInteract":
				if (CachedBeaconLighter == null)
				{
					CachedBeaconLighter = new CachedObject(@object);
				}
				break;
			case "Metadata/QuestObjects/Act6/BlackCrest_BeaconLever":
				if (CachedBeaconSwitch == null)
				{
					CachedBeaconSwitch = new CachedObject(@object);
				}
				break;
			}
		}
	}

	public static async Task<bool> GrabBlackFlag()
	{
		if (Helpers.PlayerHasQuestItem("BlackFlag"))
		{
			return false;
		}
		if (World.Act6.CavernOfAnger.IsCurrentArea)
		{
			Chest chest_0 = FlagChest;
			if ((NetworkObject)(object)chest_0 == (NetworkObject)null)
			{
				GlobalLog.Error("[GrabBlackFlag] Unexpected error. We are inside Cavern of Anger but there is no Flag Chest.");
				ErrorManager.ReportError();
				await PlayerAction.TpToTown();
				return true;
			}
			if (chest_0.IsOpened)
			{
				GlobalLog.Debug("[GrabBlackFlag] Flag Chest is opened. Waiting for Black Flag pick up.");
				await Wait.StuckDetectionSleep(200);
				return true;
			}
			await ((NetworkObject)(object)chest_0).WalkablePosition().ComeAtOnce();
			if (!(await PlayerAction.Interact((NetworkObject)(object)chest_0, () => chest_0.Fresh<Chest>().IsOpened, "Flag Chest opening")))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		await Travel.To(World.Act6.CavernOfAnger);
		return true;
	}

	public static async Task<bool> FuelBeacon()
	{
		if (!bool_0)
		{
			if (World.Act6.Beacon.IsCurrentArea)
			{
				List<CachedObject> cachedCarts = CachedFuelCarts;
				CachedObject cachedCart = cachedCarts.FirstOrDefault();
				if (cachedCart != null)
				{
					WalkablePosition pos = cachedCart.Position;
					if (pos.Distance <= 10)
					{
						NetworkObject cartObj = cachedCart.Object;
						if (IsDelivered(cartObj))
						{
							GlobalLog.Warn($"[FuelBeacon] Fuel cart (id: {cachedCart.Id}) has arrived to it's destination.");
							cachedCarts.Remove(cachedCart);
							return true;
						}
						GlobalLog.Debug($"[FuelBeacon] Waiting for Fuel cart (id: {cachedCart.Id})");
						await Wait.StuckDetectionSleep(200);
						return true;
					}
					await pos.ComeAtOnce(5);
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act6.Beacon);
			return true;
		}
		return false;
	}

	public static async Task<bool> LightBeacon()
	{
		if (!bool_1)
		{
			if (World.Act6.Beacon.IsCurrentArea)
			{
				CachedObject beaconSwitch = CachedBeaconSwitch;
				if (beaconSwitch != null)
				{
					WalkablePosition switchPos = beaconSwitch.Position;
					if (switchPos.IsFar)
					{
						switchPos.Come();
						return true;
					}
					NetworkObject networkObject_0 = beaconSwitch.Object;
					if (!networkObject_0.IsTargetable)
					{
						CachedObject beaconLighter = CachedBeaconLighter;
						if (beaconLighter == null)
						{
							GlobalLog.Debug("[BrineKing] We are near Ignition Switch but Beacon object is null.");
							await Wait.StuckDetectionSleep(500);
							return true;
						}
						NetworkObject networkObject_1 = beaconLighter.Object;
						if (!networkObject_1.IsTargetable)
						{
							GlobalLog.Debug("Waiting for Weylam Roth ship");
							await Wait.StuckDetectionSleep(500);
							return true;
						}
						if (!(await PlayerAction.Interact(networkObject_1, () => !networkObject_1.Fresh<NetworkObject>().IsTargetable, "Beacon interaction")))
						{
							await MoveAway(25, 45);
							ErrorManager.ReportError();
						}
						return true;
					}
					if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Ignition Switch interaction", 5000)))
					{
						await MoveAway(25, 45);
						ErrorManager.ReportError();
					}
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act6.Beacon);
			return true;
		}
		return false;
	}

	public static async Task<bool> SailToReef()
	{
		if (!World.Act6.BrineKingReef.IsCurrentArea)
		{
			if (World.Act6.Beacon.IsCurrentArea)
			{
				CachedObject weylam = CachedBeaconWeylam;
				if (!(weylam != null))
				{
					tgtPosition_0.Come();
					return true;
				}
				WalkablePosition pos = weylam.Position;
				if (pos.IsFar)
				{
					pos.Come();
					return true;
				}
				NetworkObject weylamObj = weylam.Object;
				if (!weylamObj.IsTargetable)
				{
					GlobalLog.Debug("Waiting for Weylam Roth");
					await Wait.StuckDetectionSleep(200);
					return true;
				}
				uint hash = LocalData.AreaHash;
				if (!(await weylamObj.AsTownNpc().Converse("Sail to the Brine King's Reef")))
				{
					ErrorManager.ReportError();
					return true;
				}
				await Coroutines.CloseBlockingWindows();
				await Wait.ForAreaChange(hash);
				return true;
			}
			await Travel.To(World.Act6.Beacon);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillBrineKingAndSailToAct7()
	{
		if (!World.Act7.BridgeEncampment.IsCurrentArea)
		{
			if (World.Act6.BrineKingReef.IsCurrentArea)
			{
				Npc weylam = BrineKingWeylam;
				if ((NetworkObject)(object)weylam != (NetworkObject)null && ((NetworkObject)weylam).IsTargetable)
				{
					uint hash = LocalData.AreaHash;
					if (!(await ((NetworkObject)(object)weylam).AsTownNpc().Converse("Sail to the Bridge Encampment")))
					{
						ErrorManager.ReportError();
						return true;
					}
					await Coroutines.CloseBlockingWindows();
					await Wait.ForAreaChange(hash);
					return true;
				}
				Monster brineKing = BrineKing;
				if ((NetworkObject)(object)brineKing != (NetworkObject)null)
				{
					if (!OriginalCombatRange.HasValue)
					{
						ChangeCombatRange();
					}
					WalkablePosition pos = ((NetworkObject)(object)brineKing).WalkablePosition();
					if (pos.Distance <= 20)
					{
						GlobalLog.Debug("Waiting for " + pos.Name);
						await Coroutines.FinishCurrentAction(true);
						await Wait.StuckDetectionSleep(200);
						return true;
					}
					pos.Come();
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_1);
				return true;
			}
			await Travel.To(World.Act6.BrineKingReef);
			return true;
		}
		if (World.Act7.BridgeEncampment.IsWaypointOpened)
		{
			return false;
		}
		if (!(await PlayerAction.OpenWaypoint()))
		{
			ErrorManager.ReportError();
		}
		return true;
	}

	private static void ChangeCombatRange()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		IRoutine current = RoutineManager.Current;
		Message val = new Message("GetCombatRange", (object)null);
		if ((int)((IMessageHandler)current).Message(val) > 0)
		{
			GlobalLog.Error("[BrineKing] " + ((IAuthored)current).Name + " does not support GetCombatRange message.");
			ErrorManager.ReportCriticalError();
		}
		OriginalCombatRange = val.GetOutput<int>(0);
		GlobalLog.Warn($"[BrineKing] Saving original combat range {OriginalCombatRange}.");
		val = new Message("SetCombatRange", (object)null, new object[1] { 25 });
		if ((int)((IMessageHandler)current).Message(val) > 0)
		{
			GlobalLog.Error("[BrineKing] " + ((IAuthored)current).Name + " does not support SetCombatRange message.");
			ErrorManager.ReportCriticalError();
		}
		GlobalLog.Warn($"[BrineKing] Combat range has been set to {25}.");
	}

	public static void RestoreCombatRange()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		IRoutine current = RoutineManager.Current;
		Message val = new Message("SetCombatRange", (object)null, new object[1] { OriginalCombatRange });
		if ((int)((IMessageHandler)current).Message(val) > 0)
		{
			GlobalLog.Error("[BrineKing] Cannot restore original combat range. " + ((IAuthored)current).Name + " does not support SetCombatRange message.");
		}
		else
		{
			GlobalLog.Warn($"[BrineKing] Combat range has been restored to original {OriginalCombatRange}.");
		}
		OriginalCombatRange = null;
	}

	private static bool IsDelivered(NetworkObject cart)
	{
		Transitionable transitionableComponent = cart.Components.TransitionableComponent;
		return ((transitionableComponent != null) ? new ushort?(transitionableComponent.Flag1) : null) == 2;
	}

	public static Task<bool> MoveAway(int min, int max)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		WorldPosition worldPosition = WorldPosition.FindPathablePositionAtDistance(min, max, 5);
		Vector2i asVector = worldPosition.AsVector;
		asVector += new Vector2i(LokiPoe.Random.Next(-2, 3), LokiPoe.Random.Next(-2, 3));
		PlayerMoverManager.Current.MoveTowards(asVector, (object[])null);
		return Task.FromResult(result: true);
	}

	static A6_Q7_BrineKing()
	{
		tgtPosition_0 = new TgtPosition("Weylam Roth location", "weylamwalk_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Brine King room", "beach_island_transition_v01_02.tgt");
	}
}
