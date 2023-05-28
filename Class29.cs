using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EquipPluginEx.Helpers;
using ExPlugins.EquipPluginEx.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

internal class Class29 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Class27 class27_0;

	private bool bool_0;

	private bool bool_1;

	public string Name => "BuyGemsFromVendor";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			if (World.CurrentArea.IsHideoutArea || World.CurrentArea.IsTown)
			{
				if (((Player)LokiPoe.Me).Level >= class27_0.LevelToStopCheckingVendorsForGems)
				{
					return false;
				}
				if (!bool_1 && !World.Act2.SouthernForest.IsWaypointOpened && ((Player)LokiPoe.Me).Level >= 8)
				{
					Item lRing = InventoryUi.InventoryControl_LeftRing.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Sapphire Ring");
					Item rRing = InventoryUi.InventoryControl_LeftRing.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Sapphire Ring");
					if (!((RemoteMemoryObject)(object)lRing != (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)rRing != (RemoteMemoryObject)null))
					{
						await BuyItem((Item item) => (int)item.ItemType == 6 && item.Name == "Sapphire Ring", World.Act1.LioneyeWatch, TownNpcs.Nessa);
						bool_1 = true;
					}
					else
					{
						bool_1 = true;
					}
				}
				if (!bool_0)
				{
					bool_0 = true;
					AreaInfo lastOpenedAct = World.Act1.LioneyeWatch;
					if (!World.Act2.SouthernForest.IsWaypointOpened)
					{
						lastOpenedAct = World.Act1.LioneyeWatch;
					}
					else if (!World.Act3.CityOfSarn.IsWaypointOpened)
					{
						lastOpenedAct = World.Act2.ForestEncampment;
					}
					else if (World.Act4.Aqueduct.IsWaypointOpened)
					{
						if (!World.Act5.SlavePens.IsWaypointOpened)
						{
							lastOpenedAct = World.Act4.Highgate;
						}
						else if (World.Act6.LioneyeWatch.IsWaypointOpened)
						{
							lastOpenedAct = World.Act6.LioneyeWatch;
						}
					}
					else
					{
						lastOpenedAct = World.Act3.SarnEncampment;
					}
					TownNpc lastTownNpc = TownNpcs.Nessa;
					if (!(lastOpenedAct == World.Act1.LioneyeWatch))
					{
						if (lastOpenedAct == World.Act2.ForestEncampment)
						{
							lastTownNpc = TownNpcs.Yeena;
						}
						else if (lastOpenedAct == World.Act3.SarnEncampment)
						{
							lastTownNpc = TownNpcs.Clarissa;
						}
						else if (!(lastOpenedAct == World.Act4.Highgate))
						{
							if (lastOpenedAct == World.Act6.LioneyeWatch)
							{
								lastTownNpc = TownNpcs.LillyRoth;
							}
						}
						else
						{
							lastTownNpc = TownNpcs.PetarusAndVanja;
						}
					}
					else
					{
						lastTownNpc = TownNpcs.Nessa;
					}
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Helmet.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.HelmetGems, InventoryUi.InventoryControl_Head.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Body Armour.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.BodyArmourGems, InventoryUi.InventoryControl_Chest.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Gloves.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.GlovesGems, InventoryUi.InventoryControl_Gloves.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Boots.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.BootsGems, InventoryUi.InventoryControl_Boots.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Main Hand.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.MainHandGems, InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] New going to check and buy gems for Off-Hand.");
					}
					await CheckAndBuyAllGemsForSlot(class27_0.OffHandGems, InventoryUi.InventoryControl_PrimaryOffHand.Inventory.Items.FirstOrDefault(), lastOpenedAct, lastTownNpc, leftSide: false);
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static async Task<bool> CheckAndBuyAllGemsForSlot(ObservableCollection<Class27.Class28> slotGems, Item equippedItem, AreaInfo lastOpenedAct, TownNpc townNpc, bool leftSide = true)
	{
		if (!((RemoteMemoryObject)(object)equippedItem == (RemoteMemoryObject)null))
		{
			if (equippedItem.Components.ModsComponent.ExplicitStrings.Count() == 0 || equippedItem.Components.ModsComponent.ImplicitStrings.Count() == 0)
			{
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] Need to view equipped item");
				}
				await Inventories.OpenInventory();
				InventoryControlWrapper inventory = CheckAndEquipItems.GetInventoryByItem(equippedItem, leftSide);
				inventory.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory y, Item x) => x.LocationTopLeft == equippedItem.LocationTopLeft), (Func<bool>)(() => InventoryUi.IsOpened));
			}
			foreach (Class27.Class28 class28_0 in slotGems)
			{
				if (!GemHelper.HaveGemInItem(equippedItem, class28_0.Name) && !GemHelper.HaveGemInIinventory(class28_0.Name))
				{
					if (Class27.Instance.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] Found no " + class28_0.Name + " in inventory, now going to buy it.");
					}
					await BuyItem((Item item) => item.Name.ToLower() == class28_0.Name.ToLower(), lastOpenedAct, townNpc, equippedItem);
				}
			}
			return false;
		}
		return false;
	}

	private static async Task<bool> BuyItem(Func<Item, bool> match, AreaInfo areaInfo, TownNpc vendor, Item itemToSocketGems = null)
	{
		if (!areaInfo.IsCurrentArea)
		{
			await Travel.To(areaInfo);
		}
		if (!(await vendor.OpenPurchasePanel()))
		{
			return false;
		}
		List<KeyValuePair<string, int>> list = default(List<KeyValuePair<string, int>>);
		bool canAfford = default(bool);
		for (int i = 0; i <= PurchaseUi.TabControl.LastTabIndex; i++)
		{
			if (PurchaseUi.TabControl.CurrentTabIndex != i)
			{
				PurchaseUi.TabControl.SwitchToTabKeyboard(i);
			}
			string tabName = PurchaseUi.TabControl.CurrentTabName;
			if (Class27.Instance.DebugMode)
			{
				GlobalLog.Debug("[BuyGemsFromVendor] Searching for items in tab " + tabName);
			}
			Item item_0 = PurchaseUi.InventoryControl.Inventory.Items.FirstOrDefault(match);
			if (!((RemoteMemoryObject)(object)item_0 != (RemoteMemoryObject)null))
			{
				continue;
			}
			if (Class27.Instance.DebugMode)
			{
				GlobalLog.Debug("[BuyGemsFromVendor] Found " + item_0.Name + ".");
			}
			if ((int)item_0.Rarity != 4 || !((RemoteMemoryObject)(object)itemToSocketGems == (RemoteMemoryObject)null))
			{
				if ((int)item_0.Rarity != 4 || GemHelper.GetFirstUsableSocketIndex(itemToSocketGems, GemHelper.GetGemColor(item_0)) != -1)
				{
					PurchaseUi.InventoryControl.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory inventory, Item it) => it.LocalId == item_0.LocalId), (Func<bool>)(() => PurchaseUi.IsOpened));
					PurchaseUi.InventoryControl.GetItemCostEx(item_0.LocalId, ref list, ref canAfford);
					if (!canAfford)
					{
						if (Class27.Instance.DebugMode)
						{
							GlobalLog.Debug("[BuyGemsFromVendor] We can't afford " + item_0.Name + ".");
						}
						continue;
					}
					if (Class27.Instance.DebugMode)
					{
						GlobalLog.Debug("[BuyGemsFromVendor] We can afford it!");
					}
					FastMoveResult moved = PurchaseUi.InventoryControl.FastMove(item_0.LocalId, true, true);
					await Wait.LatencySleep();
					if ((int)moved > 0)
					{
						if (Class27.Instance.DebugMode)
						{
							GlobalLog.Error($"[BuyGemsFromVendor] Failed: {moved}");
						}
						return true;
					}
					GlobalLog.Debug("[BuyGemsFromVendor] Successfully bought " + item_0.Name + ".");
					break;
				}
				if (Class27.Instance.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] Found no slots in " + itemToSocketGems.FullName + " " + itemToSocketGems.Name + " to place " + item_0.Name + ".");
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		if (message.Id == "player_leveled_event")
		{
			bool_1 = false;
			bool_0 = false;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	static Class29()
	{
		class27_0 = Class27.Instance;
	}
}
