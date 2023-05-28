using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q2_EssenceOfArtist
{
	private static readonly TgtPosition tgtPosition_0;

	private static Chest ContainerOfSins => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2579 }).FirstOrDefault<Chest>();

	private static Monster Maligaro => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2120 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Portal MaligaroPortal => ((IEnumerable)ObjectManager.Objects).Closest<Portal>((Func<Portal, bool>)((Portal p) => ((NetworkObject)p).IsTargetable && p.LeadsTo((DatWorldAreaWrapper a) => a.Name == World.Act7.MaligaroSanctum.Name)));

	private static NetworkObject MapDevice => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act7/MaligaroOrrery");

	private static NetworkObject MaligaroRoomObj => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/MiscellaneousObjects/ArenaMiddle");

	private static CachedObject CachedContainerOfSins
	{
		get
		{
			return CombatAreaCache.Current.Storage["ContainerOfSins"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["ContainerOfSins"] = value;
		}
	}

	private static CachedObject CachedMapDevice
	{
		get
		{
			return CombatAreaCache.Current.Storage["MapDevice"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["MapDevice"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act7.Crypt.IsCurrentArea)
		{
			if (CachedContainerOfSins == null)
			{
				Chest containerOfSins = ContainerOfSins;
				if ((NetworkObject)(object)containerOfSins != (NetworkObject)null)
				{
					CachedContainerOfSins = new CachedObject((NetworkObject)(object)containerOfSins);
				}
			}
		}
		else if (World.Act7.ChamberOfSins1.IsCurrentArea && CachedMapDevice == null)
		{
			NetworkObject mapDevice = MapDevice;
			if (mapDevice != (NetworkObject)null)
			{
				CachedMapDevice = new CachedObject(mapDevice);
			}
		}
	}

	public static async Task<bool> GrabMaligaroMap()
	{
		if (!Helpers.PlayerHasQuestItem("MaligaroMap"))
		{
			if (World.Act7.Crypt.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedContainerOfSins))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act7.Crypt);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillMaligaro()
	{
		if (!Helpers.PlayerHasQuestItem("BlackVenom"))
		{
			if (!World.Act7.ChamberOfSins1.IsCurrentArea)
			{
				if (World.Act7.MaligaroSanctum.IsCurrentArea)
				{
					NetworkObject roomObj = MaligaroRoomObj;
					if (roomObj != (NetworkObject)null && roomObj.PathExists())
					{
						Monster maligaro = Maligaro;
						if ((NetworkObject)(object)maligaro != (NetworkObject)null)
						{
							await Helpers.MoveToBossOrAnyMob(maligaro);
							return true;
						}
						await Helpers.MoveAndWait(roomObj, "Waiting for any Maligaro fight object");
						return true;
					}
					await Helpers.Explore();
					return true;
				}
				await Travel.To(World.Act7.ChamberOfSins1);
				return true;
			}
			Portal portal = MaligaroPortal;
			if (!((NetworkObject)(object)portal != (NetworkObject)null))
			{
				CachedObject device = CachedMapDevice;
				if (device != null)
				{
					WalkablePosition pos = device.Position;
					if (pos.IsFar)
					{
						pos.Come();
						return true;
					}
					if (!(await HandleMapDevice(device.Object)))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				tgtPosition_0.Come();
				return true;
			}
			if (!(await PlayerAction.TakePortal(portal)))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		return false;
	}

	public static async Task<bool> TalktoSin()
	{
		return await Helpers.TravelAndTalkTo(World.Act7.BridgeEncampment, TownNpcs.SinA7);
	}

	public static async Task<bool> TalktoHelena()
	{
		return await Helpers.TravelAndTalkTo(World.Act7.BridgeEncampment, TownNpcs.HelenaA7);
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.HelenaA7, "Maligaro Reward", Quests.EssenceOfArtist.Id, null, shouldLogOut: true);
	}

	public static async Task<bool> HandleMapDevice(NetworkObject device)
	{
		if (MapDeviceUi.IsOpened || await PlayerAction.Interact(device, () => MapDeviceUi.IsOpened, "Map Device opening"))
		{
			if (!MapDeviceUi.InventoryControl.Inventory.Items.Exists(IsMaligaroMap))
			{
				Item map = Inventories.InventoryItems.Find(IsMaligaroMap);
				if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null)
				{
					GlobalLog.Error("[EssenceOfArtist] Unexpected error. We must have Maligaro's Map at this point.");
					ErrorManager.ReportCriticalError();
					return false;
				}
				int int_0 = MapDeviceUi.InventoryControl.Inventory.Items.Count;
				if (!(await Inventories.FastMoveFromInventory(map.LocationTopLeft)))
				{
					return false;
				}
				if (!(await Wait.For(() => MapDeviceUi.InventoryControl.Inventory.Items.Count == int_0 + 1, "item count change in Map Device")))
				{
					return false;
				}
			}
			await Wait.SleepSafe(500);
			ActivateResult err = MapDeviceUi.Activate(true);
			if ((int)err > 0)
			{
				GlobalLog.Error($"[EssenceOfArtist] Fail to activate the Map Device. Error: \"{err}\".");
				return false;
			}
			if (!(await Wait.For(() => !MapDeviceUi.IsOpened, "Map Device closing")))
			{
				return false;
			}
			if (!(await Wait.For(() => (NetworkObject)(object)MaligaroPortal != (NetworkObject)null, "portal to Maligaro's Sanctum", 100, 10000)))
			{
				return false;
			}
			await Wait.SleepSafe(1000);
			return true;
		}
		return false;
	}

	private static bool IsMaligaroMap(Item item)
	{
		return item.Class == "QuestItem" && item.Metadata.Contains("MaligaroMap");
	}

	static A7_Q2_EssenceOfArtist()
	{
		tgtPosition_0 = new TgtPosition("Map Device location", "temple_maporrery4.tgt");
	}
}
