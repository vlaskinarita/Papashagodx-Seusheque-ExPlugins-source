using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EquipPluginEx.Classes;
using ExPlugins.EquipPluginEx.Helpers;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CommonTasks;

namespace ExPlugins.EquipPluginEx.Tasks;

public class CheckAndEquipItems : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Class27 class27_0;

	private readonly List<int> list_0 = new List<int>();

	private List<int> list_1 = new List<int>();

	private bool bool_0;

	public string Name => "CheckAndEquipItems";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			if (!World.CurrentArea.IsHideoutArea && !World.CurrentArea.IsTown)
			{
				return false;
			}
			if (((Player)LokiPoe.Me).Level >= class27_0.LevelToStopPickUpAndEquip)
			{
				return false;
			}
			return await CheckAndEquipItemsTask();
		}
		return false;
	}

	private async Task<bool> CheckAndEquipItemsTask()
	{
		if (class27_0.DebugMode)
		{
			GlobalLog.Debug("[CheckAndEquipItems] starting");
		}
		foreach (Item inventoryItem in InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => (RemoteMemoryObject)(object)i != (RemoteMemoryObject)null && !class27_0.UniquesToNeverEquip.Contains(new Class27.Class28(i.FullName)) && !list_0.Contains(i.LocalId)))
		{
			if (class27_0.DebugMode)
			{
				GlobalLog.Debug("[CheckAndEquipItems] checking " + inventoryItem.FullName + " " + inventoryItem.Name);
			}
			if (await ShouldEquipNewItem(inventoryItem))
			{
				GlobalLog.Debug("[CheckAndEquipItems] Found better item " + inventoryItem.FullName + inventoryItem.Name);
				if (!inventoryItem.IsIdentified)
				{
					if (inventoryItem.IsMirrored || inventoryItem.IsCorrupted)
					{
						GlobalLog.Debug("[CheckAndEquipItems] Cannot id mirror or corrupted items");
						list_0.Add(inventoryItem.LocalId);
						continue;
					}
					bool id = await IdTask.Identify(inventoryItem.LocationTopLeft);
					if (class27_0.DebugMode)
					{
						GlobalLog.Debug($"[CheckAndEquipItems] Id result: {id}");
					}
					if (!id)
					{
						list_0.Add(inventoryItem.LocalId);
						continue;
					}
				}
				return await EquipItem(inventoryItem);
			}
			list_0.Add(inventoryItem.LocalId);
		}
		return false;
	}

	private async Task<bool> EquipItem(Item item)
	{
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			_ = item.LocalId;
			if (!InventoryUi.IsOpened)
			{
				await Inventories.OpenInventory();
			}
			InventoryControlWrapper invUi = GetInventoryByItem(item, bool_0);
			Item currentEquipped = (((int)item.ItemType == 10) ? invUi.Inventory.Items.FirstOrDefault((Item f) => f.LocationTopLeft.X == GetFlaskSlotToUpgrade(item)) : invUi.Inventory.Items.FirstOrDefault());
			List<string> socketedGemsNames2 = new List<string>();
			ObservableCollection<Class27.Class28> configSlot = new ObservableCollection<Class27.Class28>();
			if (!((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Head))
			{
				if (!((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Chest))
				{
					if ((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Gloves)
					{
						configSlot = class27_0.GlovesGems;
					}
					else if ((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Boots)
					{
						configSlot = class27_0.BootsGems;
					}
					else if ((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_PrimaryMainHand)
					{
						configSlot = class27_0.MainHandGems;
					}
					else if ((RemoteMemoryObject)(object)invUi == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_PrimaryOffHand)
					{
						configSlot = class27_0.OffHandGems;
					}
				}
				else
				{
					configSlot = class27_0.BodyArmourGems;
				}
			}
			else
			{
				configSlot = class27_0.HelmetGems;
			}
			socketedGemsNames2 = CheckAndAddSocketedGemToTheList(socketedGemsNames2, configSlot, invUi, currentEquipped);
			if ((RemoteMemoryObject)(object)currentEquipped != (RemoteMemoryObject)null)
			{
				await ClearInventorySpaceForItemIfNeeded(item);
				if (currentEquipped.HasSkillGemsEquipped)
				{
					await GemHelper.UnequipAllGems(currentEquipped, invUi);
				}
				await invUi.PickItemToCursor(currentEquipped.LocationTopLeft);
				await Inventories.WaitForCursorToHaveItem();
				await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(FirstSlotThatCanFitItem(currentEquipped));
				await Inventories.WaitForCursorToBeEmpty();
				await Coroutines.ReactionWait();
			}
			await ClearInventorySpaceForItemIfNeeded(item);
			await InventoryUi.InventoryControl_Main.PickItemToCursor(item.LocationTopLeft);
			await Inventories.WaitForCursorToHaveItem();
			invUi.PlaceCursorInto(true, true);
			await Inventories.WaitForCursorToBeEmpty();
			await Coroutines.ReactionWait();
			GlobalLog.Debug("[CheckAndEquipItems] Item equip success!");
			await GemHelper.SocketUnsocketedGems(socketedGemsNames2, invUi);
			return true;
		}
		GlobalLog.Error("[CheckAndEquipItems] item is null");
		return true;
	}

	private static async Task ClearInventorySpaceForItemIfNeeded(Item item)
	{
		if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item))
		{
			GlobalLog.Debug("[CheckAndEquipItems] Can't fit item");
			Item inventoryItemToBeDeleted = (from i in InventoryUi.InventoryControl_Main.Inventory.Items
				where (int)i.Rarity != 5 && (int)i.Rarity != 3 && (int)i.Rarity != 4
				orderby i.Rarity, i.MaxLinkCount
				select i).FirstOrDefault();
			if ((RemoteMemoryObject)(object)inventoryItemToBeDeleted == (RemoteMemoryObject)null)
			{
				GlobalLog.Error("[CheckAndEquipItems] Cannot find item to delete, clean your inventory manually");
				BotManager.Stop(new StopReasonData("inventory_full", "Cannot find item to delete, clean your inventory manually", (object)null), false);
			}
			else
			{
				await DestroyItem(inventoryItemToBeDeleted);
				await Wait.LatencySleep();
			}
		}
	}

	private static List<string> CheckAndAddSocketedGemToTheList(List<string> gemsToResocket, ObservableCollection<Class27.Class28> config, InventoryControlWrapper invUi, Item currentEquipped)
	{
		foreach (Class27.Class28 class28_0 in config)
		{
			if (!string.IsNullOrEmpty(class28_0.Name) && currentEquipped.SocketedGems.Any((Item x) => x.Name.ToLower() == class28_0.Name.ToLower()))
			{
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("Found unsocketed gem " + class28_0.Name + " in list for slot. Adding it to the resocket list.");
				}
				gemsToResocket.Add(class28_0.Name);
			}
		}
		return gemsToResocket;
	}

	public static Vector2i FirstSlotThatCanFitItem(Item item)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		Vector2i result = default(Vector2i);
		for (int i = 0; i < 12; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (InventoryUi.InventoryControl_Main.Inventory.CanFitItemSizeAt(item.Size.X, item.Size.Y, i, j))
				{
					((Vector2i)(ref result))._002Ector(i, j);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		return result;
	}

	private async Task<bool> ShouldEquipNewItem(Item item)
	{
		if (!HasStats(item))
		{
			if (class27_0.DebugMode)
			{
				GlobalLog.Debug("[CheckAndEquipItems] Stat requierments are not met for " + item.FullName + item.Name);
			}
			return false;
		}
		if ((int)item.ItemType == 2 && Class27.Instance.ShouldEquipMainHand)
		{
			if (!EquipPluginExEvaluator.CheckIfMainHandTypeEnabled(item))
			{
				return false;
			}
			Item myItem7 = InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault();
			bool flag = (RemoteMemoryObject)(object)myItem7 == (RemoteMemoryObject)null;
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = await IsUpgrade(myItem7, item);
			}
			if (flag2)
			{
				bool_0 = true;
				return true;
			}
		}
		if (((int)item.ItemType == 3 || (int)item.ItemType == 2) && Class27.Instance.ShouldEquipOffHand)
		{
			if (!EquipPluginExEvaluator.CheckIfOffHandTypeEnabled(item))
			{
				return false;
			}
			Item myItem8 = InventoryUi.InventoryControl_PrimaryOffHand.Inventory.Items.FirstOrDefault();
			bool flag3 = (RemoteMemoryObject)(object)myItem8 == (RemoteMemoryObject)null;
			bool flag4 = flag3;
			if (!flag4)
			{
				flag4 = await IsUpgrade(myItem8, item);
			}
			if (flag4)
			{
				bool_0 = false;
				return true;
			}
		}
		if ((int)item.ItemType != 4 || !Class27.Instance.ShouldEquipHelmet)
		{
			if ((int)item.ItemType != 1 || !Class27.Instance.ShouldEquipBodyArmour)
			{
				if ((int)item.ItemType != 7 || !Class27.Instance.ShouldEquipGloves)
				{
					if ((int)item.ItemType != 8 || !Class27.Instance.ShouldEquipBoots)
					{
						if ((int)item.ItemType != 9 || !Class27.Instance.ShouldEquipBelt)
						{
							if ((int)item.ItemType != 5 || !Class27.Instance.ShouldEquipAmulet)
							{
								if ((int)item.ItemType == 6 && Class27.Instance.ShouldEquipRings)
								{
									Item leftRing = InventoryUi.InventoryControl_LeftRing.Inventory.Items.FirstOrDefault();
									Item rightRing = InventoryUi.InventoryControl_RightRing.Inventory.Items.FirstOrDefault();
									bool flag5 = (RemoteMemoryObject)(object)leftRing == (RemoteMemoryObject)null;
									bool flag6 = flag5;
									if (!flag6)
									{
										flag6 = await IsUpgrade(leftRing, item);
									}
									bool isUpgradeLeft = flag6;
									bool flag7 = (RemoteMemoryObject)(object)rightRing == (RemoteMemoryObject)null;
									bool flag8 = flag7;
									if (!flag8)
									{
										flag8 = await IsUpgrade(rightRing, item);
									}
									bool isUpgradeRight = flag8;
									if (isUpgradeLeft)
									{
										bool_0 = true;
										return true;
									}
									if (isUpgradeRight)
									{
										bool_0 = false;
										return true;
									}
								}
								if ((int)item.ItemType != 10 || !Class27.Instance.ShouldEquipFlasks)
								{
									return false;
								}
								return GetFlaskSlotToUpgrade(item) >= 0;
							}
							Item myItem6 = InventoryUi.InventoryControl_Neck.Inventory.Items.FirstOrDefault();
							bool flag9 = (RemoteMemoryObject)(object)myItem6 == (RemoteMemoryObject)null;
							bool flag10 = flag9;
							if (!flag10)
							{
								flag10 = await IsUpgrade(myItem6, item);
							}
							return flag10;
						}
						Item myItem5 = InventoryUi.InventoryControl_Belt.Inventory.Items.FirstOrDefault();
						bool flag11 = (RemoteMemoryObject)(object)myItem5 == (RemoteMemoryObject)null;
						bool flag12 = flag11;
						if (!flag12)
						{
							flag12 = await IsUpgrade(myItem5, item);
						}
						return flag12;
					}
					Item myItem4 = InventoryUi.InventoryControl_Boots.Inventory.Items.FirstOrDefault();
					bool flag13 = (RemoteMemoryObject)(object)myItem4 == (RemoteMemoryObject)null;
					bool flag14 = flag13;
					if (!flag14)
					{
						flag14 = await IsUpgrade(myItem4, item);
					}
					return flag14;
				}
				Item myItem3 = InventoryUi.InventoryControl_Gloves.Inventory.Items.FirstOrDefault();
				bool flag15 = (RemoteMemoryObject)(object)myItem3 == (RemoteMemoryObject)null;
				bool flag16 = flag15;
				if (!flag16)
				{
					flag16 = await IsUpgrade(myItem3, item);
				}
				return flag16;
			}
			Item myItem2 = InventoryUi.InventoryControl_Chest.Inventory.Items.FirstOrDefault();
			bool flag17 = (RemoteMemoryObject)(object)myItem2 == (RemoteMemoryObject)null;
			bool flag18 = flag17;
			if (!flag18)
			{
				flag18 = await IsUpgrade(myItem2, item);
			}
			return flag18;
		}
		Item myItem = InventoryUi.InventoryControl_Head.Inventory.Items.FirstOrDefault();
		bool flag19 = (RemoteMemoryObject)(object)myItem == (RemoteMemoryObject)null;
		bool flag20 = flag19;
		if (!flag20)
		{
			flag20 = await IsUpgrade(myItem, item);
		}
		return flag20;
	}

	private static int GetFlaskSlotToUpgrade(Item newItem)
	{
		int int_0;
		for (int_0 = 0; int_0 < 5; int_0++)
		{
			if (CanBeFlaskUpgrade(QuickFlaskHud.InventoryControl.Inventory.Items.FirstOrDefault((Item f) => f.LocationTopLeft.X == int_0), newItem, int_0 + 1) >= 0)
			{
				return int_0;
			}
		}
		return -1;
	}

	private static int CanBeFlaskUpgrade(Item oldItem, Item newItem, int flaskSlot)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)newItem.ItemType == 10)
		{
			string flaskType = "None";
			if (newItem.Name.ContainsIgnorecase("life"))
			{
				flaskType = "life";
			}
			if (newItem.Name.ContainsIgnorecase("mana"))
			{
				flaskType = "mana";
			}
			if (newItem.Name.ContainsIgnorecase("hybrid"))
			{
				flaskType = "hybrid";
			}
			if (newItem.Name.ContainsIgnorecase("quicksilver"))
			{
				flaskType = "quicksilver";
			}
			if (newItem.Name.ContainsIgnorecase("quartz"))
			{
				flaskType = "quartz";
			}
			if (newItem.Name.ContainsIgnorecase("granite"))
			{
				flaskType = "granite";
			}
			if (newItem.Name.ContainsIgnorecase("basalt"))
			{
				flaskType = "basalt";
			}
			if (newItem.Name.ContainsIgnorecase("jade"))
			{
				flaskType = "jade";
			}
			if (newItem.Name.ContainsIgnorecase("stibnite"))
			{
				flaskType = "stibnite";
			}
			if (newItem.Name.ContainsIgnorecase("sulphur"))
			{
				flaskType = "sulphur";
			}
			return FlaskCheck(oldItem, newItem, flaskSlot, flaskType);
		}
		return -1;
	}

	private static int FlaskCheck(Item oldItem, Item newItem, int flaskSlot, string flaskType)
	{
		switch (flaskSlot)
		{
		case 1:
			if (class27_0.FlaskSlot1.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 0;
			}
			break;
		case 2:
			if (class27_0.FlaskSlot2.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 1;
			}
			break;
		case 3:
			if (class27_0.FlaskSlot3.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 2;
			}
			break;
		case 4:
			if (class27_0.FlaskSlot4.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 3;
			}
			break;
		case 5:
			if (class27_0.FlaskSlot5.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 4;
			}
			break;
		}
		return -1;
	}

	private async Task<bool> IsUpgrade(Item oldItem, Item newItem, int flaskSlot = 0)
	{
		if (newItem.Components.ModsComponent.ImplicitStrings.Count() == 0 || newItem.Components.ModsComponent.ExplicitStrings.Count() == 0 || ((int)newItem.ItemType == 10 && newItem.BaseRequiredLevel == 0))
		{
			if (class27_0.DebugMode)
			{
				GlobalLog.Debug("[CheckAndEquipItems] Need to view new item");
			}
			await Inventories.OpenInventory();
			InventoryUi.InventoryControl_Main.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory y, Item x) => x.LocationTopLeft == newItem.LocationTopLeft), (Func<bool>)(() => InventoryUi.IsOpened));
		}
		if (oldItem.Components.ModsComponent.ExplicitStrings.Count() == 0 || oldItem.Components.ModsComponent.ImplicitStrings.Count() == 0 || ((int)oldItem.ItemType == 10 && oldItem.BaseRequiredLevel == 0))
		{
			if (class27_0.DebugMode)
			{
				GlobalLog.Debug("[CheckAndEquipItems] Need to view old item");
			}
			await Inventories.OpenInventory();
			InventoryControlWrapper inventory = GetInventoryByItem(oldItem, bool_0);
			inventory.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory y, Item x) => x.LocationTopLeft == oldItem.LocationTopLeft), (Func<bool>)(() => InventoryUi.IsOpened));
		}
		if (SocketHelper.HasBetterOrSameColors(oldItem, newItem))
		{
			int oldSocketsWeight = SocketHelper.GetColorsWeight(oldItem);
			int newSocketsWeight = SocketHelper.GetColorsWeight(newItem);
			float totalWeightEquippedItem = await GetWeightsForItem(oldItem);
			float totalWeightNewItem = await GetWeightsForItem(newItem);
			bool shouldEquip = ((totalWeightNewItem == totalWeightEquippedItem) ? (newSocketsWeight > oldSocketsWeight) : (totalWeightNewItem > totalWeightEquippedItem));
			GlobalLog.Info($"[CheckAndEquipItems] New item weight: {totalWeightNewItem}, equipped item weight: {totalWeightEquippedItem}, new item sockets weight: {newSocketsWeight}, old item sockets weight: {oldSocketsWeight}, should equip: {shouldEquip}.");
			return shouldEquip;
		}
		return false;
	}

	public static InventoryControlWrapper GetInventoryByItem(Item item, bool leftSide)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected I4, but got Unknown
		InventoryType itemType = item.ItemType;
		InventoryType val = itemType;
		return (InventoryControlWrapper)((val - 1) switch
		{
			0 => InventoryUi.InventoryControl_Chest, 
			1 => (!leftSide) ? InventoryUi.InventoryControl_PrimaryOffHand : InventoryUi.InventoryControl_PrimaryMainHand, 
			2 => InventoryUi.InventoryControl_PrimaryOffHand, 
			3 => InventoryUi.InventoryControl_Head, 
			4 => InventoryUi.InventoryControl_Neck, 
			5 => leftSide ? InventoryUi.InventoryControl_LeftRing : InventoryUi.InventoryControl_RightRing, 
			6 => InventoryUi.InventoryControl_Gloves, 
			7 => InventoryUi.InventoryControl_Boots, 
			8 => InventoryUi.InventoryControl_Belt, 
			9 => QuickFlaskHud.InventoryControl, 
			_ => null, 
		});
	}

	private static int[] GetModNumbers(string mod)
	{
		int[] array = new int[2];
		string[] array2 = Regex.Split(mod, "\\D+");
		int num = 0;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			if (!string.IsNullOrEmpty(text))
			{
				array[num] = int.Parse(text);
				num++;
			}
		}
		return array;
	}

	private static string ModifyModString(string mod)
	{
		mod = Regex.Replace(mod, "[0-9]{1,}", "#");
		mod = mod.Replace("+", "");
		mod = mod.Replace("-", "");
		mod = mod.ToLower();
		return mod;
	}

	private async Task<float> GetWeightsForItem(Item item)
	{
		float totalWeight = 0f;
		GlobalLog.Info("[CheckAndEquipItems] Now getting weights for " + item.FullName + " " + item.Name + ".");
		if (item.Components.ModsComponent.ImplicitStrings.Count() == 0 || item.Components.ModsComponent.ExplicitStrings.Count() == 0 || ((int)item.ItemType == 10 && item.BaseRequiredLevel == 0))
		{
			if (class27_0.DebugMode)
			{
				GlobalLog.Debug("[CheckAndEquipItems] Need to view new item");
			}
			await Inventories.OpenInventory();
			InventoryControlWrapper inventory = GetInventoryByItem(item, bool_0);
			inventory.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory y, Item x) => x.LocationTopLeft == item.LocationTopLeft), (Func<bool>)(() => InventoryUi.IsOpened));
		}
		ItemPrioritiesClass currentSlotPrios = GetItemSlotPriorities(item);
		totalWeight += (float)item.ArmorValue * currentSlotPrios.BaseArmorWeight;
		totalWeight += (float)item.EvasionValue * currentSlotPrios.BaseEvasionWeight;
		totalWeight += (float)item.EnergyShieldValue * currentSlotPrios.BaseEnergyShieldWeight;
		if ((int)item.ItemType == 2)
		{
			totalWeight += (float)((item.MinPhysicalDamage + item.MaxPhysicalDamage) / 2) * currentSlotPrios.BasePhysDamageWeight;
			totalWeight += (float)((item.MinChaosDamage + item.MaxChaosDamage) / 2) * currentSlotPrios.BaseChaosDamageWeight;
			totalWeight += (float)((item.MinFireDamage + item.MaxFireDamage) / 2) * currentSlotPrios.BaseFireDamageWeight;
			totalWeight += (float)((item.MinColdDamage + item.MaxColdDamage) / 2) * currentSlotPrios.BaseColdDamageWeight;
			totalWeight += (float)((item.MinLightningDamage + item.MaxLightningDamage) / 2) * currentSlotPrios.BaseLightningDamageWeight;
		}
		foreach (string i in item.Components.ModsComponent.ImplicitStrings)
		{
			int[] modNumbers = GetModNumbers(i);
			string mod = ModifyModString(i);
			if (class27_0.DebugMode)
			{
				string str = ((modNumbers.Count() > 1) ? modNumbers[1].ToString() : "");
				GlobalLog.Error(string.Format(arg2: (!(str == "0")) ? str : "", format: "[CheckAndEquipItems] mod: {0}, numbers: {1} {2}", arg0: mod, arg1: modNumbers[0]));
			}
			if (item.Class == "Ring")
			{
				totalWeight += (mod.Contains("cold") ? ((float)modNumbers[0] * currentSlotPrios.ColdResWeight) : 0f);
				totalWeight += ((!mod.Contains("lightning")) ? 0f : ((float)modNumbers[0] * currentSlotPrios.LightningResWeight));
				totalWeight += (mod.Contains("fire") ? ((float)modNumbers[0] * currentSlotPrios.FireResWeight) : 0f);
				totalWeight += (mod.Contains("physical damage to attacks") ? ((float)((modNumbers[0] + modNumbers[1]) / 2) * currentSlotPrios.PhysDamageFlatToAttacksWeight) : 0f);
			}
			totalWeight += ((mod.Contains("intelligence") || mod.Contains("all attributes")) ? ((float)modNumbers[0] * currentSlotPrios.IntWeight) : 0f);
			totalWeight += ((mod.Contains("dexterity") || mod.Contains("all attributes")) ? ((float)modNumbers[0] * currentSlotPrios.DexWeight) : 0f);
			totalWeight += ((mod.Contains("strength") || mod.Contains("all attributes")) ? ((float)modNumbers[0] * currentSlotPrios.StrWeight) : 0f);
			totalWeight += (mod.Contains("to maximum life") ? ((float)modNumbers[0] * currentSlotPrios.LifeWeight) : 0f);
			totalWeight += (mod.Contains("increased maximum life") ? ((float)modNumbers[0] * currentSlotPrios.LifePercentWeight) : 0f);
			totalWeight += (mod.Contains("to maximum mana") ? ((float)modNumbers[0] * currentSlotPrios.ManaWeight) : 0f);
			totalWeight += (mod.Contains("increased maximum mana") ? ((float)modNumbers[0] * currentSlotPrios.ManaPercentWeight) : 0f);
			totalWeight += ((!mod.Contains("to maximum energy shield")) ? 0f : ((float)modNumbers[0] * currentSlotPrios.EnergyShieldWeight));
			totalWeight += ((!mod.Contains("maximum energy shield")) ? 0f : ((float)modNumbers[0] * currentSlotPrios.EnergyShieldPercentWeight));
			totalWeight += (mod.Contains("chaos resistance") ? ((float)modNumbers[0] * currentSlotPrios.ChaosResWeight) : 0f);
			totalWeight += (mod.Contains("all elemental resistances") ? ((float)modNumbers[0] * (currentSlotPrios.ColdResWeight + currentSlotPrios.LightningResWeight + currentSlotPrios.FireResWeight)) : 0f);
			totalWeight += ((!mod.Contains("global physical damage")) ? 0f : ((float)modNumbers[0] * currentSlotPrios.PhysDamagePercentWeight));
		}
		foreach (string j in item.Components.ModsComponent.ExplicitStrings)
		{
			int[] modNumbers2 = GetModNumbers(j);
			string mod2 = ModifyModString(j);
			if (class27_0.DebugMode)
			{
				string str2 = ((modNumbers2.Count() > 1) ? modNumbers2[1].ToString() : "");
				GlobalLog.Error(string.Format(arg2: (str2 == "0") ? "" : str2, format: "[CheckAndEquipItems] mod: {0}, numbers: {1} {2}", arg0: mod2, arg1: modNumbers2[0]));
			}
			totalWeight += ((!mod2.Contains("to maximum life")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.LifeWeight));
			totalWeight += (mod2.Contains("increased maximum life") ? ((float)modNumbers2[0] * currentSlotPrios.LifePercentWeight) : 0f);
			totalWeight += ((!mod2.Contains("to maximum mana")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.ManaWeight));
			totalWeight += ((!mod2.Contains("increased maximum mana")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.ManaPercentWeight));
			totalWeight += (mod2.Contains("to maximum energy shield") ? ((float)modNumbers2[0] * currentSlotPrios.EnergyShieldWeight) : 0f);
			totalWeight += ((!mod2.Contains("increased maximum energy shield")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.EnergyShieldPercentWeight));
			totalWeight += (mod2.Contains("chaos resistance") ? ((float)modNumbers2[0] * currentSlotPrios.ChaosResWeight) : 0f);
			totalWeight += (mod2.Contains("all elemental resistances") ? ((float)modNumbers2[0] * (currentSlotPrios.ColdResWeight + currentSlotPrios.LightningResWeight + currentSlotPrios.FireResWeight)) : 0f);
			totalWeight += ((mod2.Contains("cold resistance") || mod2.Contains("cold and")) ? ((float)modNumbers2[0] * currentSlotPrios.ColdResWeight) : 0f);
			totalWeight += ((mod2.Contains("lightning resistance") || mod2.Contains("lightning and")) ? ((float)modNumbers2[0] * currentSlotPrios.LightningResWeight) : 0f);
			totalWeight += ((mod2.Contains("fire resistance") || mod2.Contains("fire and")) ? ((float)modNumbers2[0] * currentSlotPrios.FireResWeight) : 0f);
			totalWeight += ((mod2.Contains("intelligence") || mod2.Contains("all attributes")) ? ((float)modNumbers2[0] * currentSlotPrios.IntWeight) : 0f);
			totalWeight += ((mod2.Contains("dexterity") || mod2.Contains("all attributes")) ? ((float)modNumbers2[0] * currentSlotPrios.DexWeight) : 0f);
			totalWeight += ((mod2.Contains("strength") || mod2.Contains("all attributes")) ? ((float)modNumbers2[0] * currentSlotPrios.StrWeight) : 0f);
			totalWeight += (mod2.Contains("physical damage to attacks") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.PhysDamageFlatToAttacksWeight) : 0f);
			totalWeight += (mod2.Contains("chaos damage to attacks") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.ChaosDamageFlatToAttacksWeight) : 0f);
			totalWeight += (mod2.Contains("fire damage to attacks") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.FireDamageFlatToAttacksWeight) : 0f);
			totalWeight += (mod2.Contains("cold damage to attacks") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.ColdDamageFlaToAttackstWeight) : 0f);
			totalWeight += (mod2.Contains("lightning damage to attacks") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.LightningDamageFlatToAttacksWeight) : 0f);
			totalWeight += (mod2.Contains("physical damage to spells") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.PhysDamageFlatToSpellsWeight) : 0f);
			totalWeight += ((!mod2.Contains("chaos damage to spells")) ? 0f : ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.ChaosDamageFlatToSpellsWeight));
			totalWeight += (mod2.Contains("fire damage to spells") ? ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.FireDamageFlatToSpellsWeight) : 0f);
			totalWeight += ((!mod2.Contains("cold damage to spells")) ? 0f : ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.ColdDamageFlaToSpellstWeight));
			totalWeight += ((!mod2.Contains("lightning damage to spells")) ? 0f : ((float)((modNumbers2[0] + modNumbers2[1]) / 2) * currentSlotPrios.LightningDamageFlatToSpellsWeight));
			totalWeight += (mod2.Contains("increased damage") ? ((float)modNumbers2[0] * currentSlotPrios.DamagePercentWeight) : 0f);
			totalWeight += (mod2.Contains("increased physical damage") ? ((float)modNumbers2[0] * currentSlotPrios.PhysDamagePercentWeight) : 0f);
			totalWeight += ((!mod2.Contains("increased chaos damage")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.ChaosDamagePercentWeight));
			totalWeight += ((!mod2.Contains("increased fire damage")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.FireDamagePercentWeight));
			totalWeight += (mod2.Contains("increased cold damage") ? ((float)modNumbers2[0] * currentSlotPrios.ColdDamagePercentWeight) : 0f);
			totalWeight += ((!mod2.Contains("increased lightning damage")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.LightningDamagePercentWeight));
			totalWeight += (mod2.Contains("increased spell damage") ? ((float)modNumbers2[0] * currentSlotPrios.DamageWithPoisonWeight) : 0f);
			totalWeight += (mod2.Contains("increased elemental damage with attack skills") ? ((float)modNumbers2[0] * currentSlotPrios.ElementalDamagePercentToAttacksWeight) : 0f);
			totalWeight += ((!mod2.Contains("to global critical strike multiplier")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.CritMultiWeight));
			totalWeight += (mod2.Contains("increased movement speed") ? ((float)modNumbers2[0] * currentSlotPrios.MovementSpeedWeight) : 0f);
			totalWeight += ((!mod2.Contains("increased attack speed")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.AttackSpeedWeight));
			totalWeight += ((!mod2.Contains("increased cast speed")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.CastSpeedWeight));
			totalWeight += (mod2.Contains("to damage over time multiplier") ? ((float)modNumbers2[0] * currentSlotPrios.DamageOverTimeWeight) : 0f);
			totalWeight += (mod2.Contains("increased damage with bleeding") ? ((float)modNumbers2[0] * currentSlotPrios.DamageWithBleedingWeight) : 0f);
			totalWeight += ((!mod2.Contains("increased damage with poison")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.DamageWithPoisonWeight));
			totalWeight += (mod2.Contains("to chaos damage over time multiplier") ? ((float)modNumbers2[0] * currentSlotPrios.ChaosDoTMultiWeight) : 0f);
			totalWeight += ((!mod2.Contains("to fire damage over time multiplier")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.FireDoTMultiWeight));
			totalWeight += ((!mod2.Contains("to cold damage over time multiplier")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.ColdDoTMultiWeight));
			totalWeight += ((!mod2.Contains("to physical damage over time multiplier")) ? 0f : ((float)modNumbers2[0] * currentSlotPrios.PhysDoTMultiWeight));
		}
		GlobalLog.Info($"[CheckAndEquipItems] Final weight: {totalWeight}.");
		return totalWeight;
	}

	private static ItemPrioritiesClass GetItemSlotPriorities(Item item)
	{
		return item.Class switch
		{
			"Body Armour" => class27_0.BodyArmourPriorities, 
			"Shield" => class27_0.OffHandPriorities, 
			"Ring" => class27_0.RingsPriorities, 
			"Sceptre" => class27_0.MainHandPriorities, 
			"Amulet" => class27_0.AmuletPriorities, 
			"Two Hand Sword" => class27_0.MainHandPriorities, 
			"Two Hand Mace" => class27_0.MainHandPriorities, 
			"Dagger" => class27_0.MainHandPriorities, 
			"One Hand Axe" => class27_0.MainHandPriorities, 
			"One Hand Mace" => class27_0.MainHandPriorities, 
			"Staff" => class27_0.MainHandPriorities, 
			"One Hand Sword" => class27_0.MainHandPriorities, 
			"Claw" => class27_0.MainHandPriorities, 
			"Helmet" => class27_0.HelmetPriorities, 
			"Warstaff" => class27_0.MainHandPriorities, 
			"Boots" => class27_0.BootsPriorities, 
			"Wand" => class27_0.MainHandPriorities, 
			"Quiver" => class27_0.OffHandPriorities, 
			"Belt" => class27_0.BeltPriorities, 
			"Two Hand Axe" => class27_0.MainHandPriorities, 
			"Gloves" => class27_0.GlovesPriorities, 
			"Bow" => class27_0.MainHandPriorities, 
			_ => null, 
		};
	}

	public static bool HasStats(Item newItem)
	{
		return ((Player)LokiPoe.Me).Level >= newItem.RequiredLevel && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)576) >= newItem.RequiredInt && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)573) >= newItem.RequiredStr && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)579) >= newItem.RequiredDex;
	}

	public static async Task<bool> DestroyItem(Item item)
	{
		_ = item.FullName;
		await InventoryUi.InventoryControl_Main.PickItemToCursor(item.LocationTopLeft);
		await Inventories.WaitForCursorToHaveItem();
		SendChatMsg("/destroy");
		await Inventories.WaitForCursorToBeEmpty();
		return (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null;
	}

	public static ChatResult SendChatMsg(string msg, bool closeChatUi = true)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(msg))
		{
			if (!ChatPanel.IsOpened)
			{
				ChatPanel.ToggleChat(true);
			}
			if (!ChatPanel.IsOpened)
			{
				return (ChatResult)3;
			}
			ChatResult result = ChatPanel.Chat(msg, true, false);
			if (closeChatUi && ChatPanel.IsOpened)
			{
				ChatPanel.ToggleChat(true);
			}
			return result;
		}
		return (ChatResult)0;
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

	static CheckAndEquipItems()
	{
		class27_0 = Class27.Instance;
	}
}
