using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EquipPluginEx.Tasks;
using ExPlugins.EXtensions;

namespace ExPlugins.EquipPluginEx.Helpers;

public static class GemHelper
{
	public static async Task<bool> UnequipAllGems(Item item, InventoryControlWrapper inventoryControlWrapper)
	{
		List<Item> gems = item.SocketedGems.Where((Item g) => (RemoteMemoryObject)(object)g != (RemoteMemoryObject)null && !string.IsNullOrEmpty(g.Name)).ToList();
		if (gems.Count < 1)
		{
			GlobalLog.Error("[GemHelper] No gems in item");
			return true;
		}
		foreach (Item gem in gems)
		{
			int gemIndex = item.GetSocketIndexOfGem(gem);
			if (gemIndex < 0)
			{
				GlobalLog.Error($"[GemHelper] gemIndex invalid: {gemIndex}");
				continue;
			}
			inventoryControlWrapper.UnequipSkillGem(gemIndex, true);
			await Inventories.WaitForCursorToHaveItem();
			await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(CheckAndEquipItems.FirstSlotThatCanFitItem(gem));
			await Inventories.WaitForCursorToBeEmpty();
			await Wait.LatencySleep();
		}
		return true;
	}

	public static int GetEmptySocketIndexInLargestLink(Item item, SocketColor color)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Invalid comparison between I4 and Unknown
		SocketColor[] array = item.LinkedSocketColors.OrderByDescending((SocketColor[] x) => x.Count()).FirstOrDefault();
		string text = "";
		SocketColor[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			SocketColor val = array2[i];
			text += $"{((object)(SocketColor)(ref val)).ToString()[0]}-";
		}
		text = text.Replace("-", "");
		int num = 0;
		int num2 = -1;
		List<int> list = new List<int>();
		for (int j = 0; j < item.SocketedDisplayString.Count(); j++)
		{
			num2++;
			if (item.SocketedDisplayString[j] == ' ')
			{
				num = 0;
			}
			if (item.SocketedDisplayString[j] == ' ' || item.SocketedDisplayString[j] == '-')
			{
				num2--;
				continue;
			}
			if (item.SocketedDisplayString[j] == text[num])
			{
				num++;
				list.Add(num2);
			}
			if (num == text.Count())
			{
				break;
			}
		}
		foreach (int item2 in list)
		{
			if ((int)item.SocketColors[item2] == (int)color || (((object)(SocketColor)(ref item.SocketColors[item2])).ToString()[0] == 'W' && (RemoteMemoryObject)(object)item.GetGemInSocket(item2) == (RemoteMemoryObject)null))
			{
				if (Class27.Instance.DebugMode)
				{
					GlobalLog.Debug($"[GemHelper] Found socket in largest link: {item2}");
				}
				return item2;
			}
		}
		return -1;
	}

	public static int GetFirstUsableSocketIndex(Item item, SocketColor color)
	{
		int num = -1;
		new List<int>();
		for (int i = 0; i < item.SocketedDisplayString.Count(); i++)
		{
			num++;
			if (item.SocketedDisplayString[i] != ' ' && item.SocketedDisplayString[i] != '-')
			{
				if (item.SocketedDisplayString[i] == ((object)(SocketColor)(ref color)).ToString()[0] || (item.SocketedDisplayString[i] == 'W' && (RemoteMemoryObject)(object)item.GetGemInSocket(num) == (RemoteMemoryObject)null))
				{
					if (Class27.Instance.DebugMode)
					{
						GlobalLog.Debug($"[GemHelper] Found first usable socket: {num}");
					}
					return num;
				}
			}
			else
			{
				num--;
			}
		}
		return -1;
	}

	public static async Task<bool> SocketUnsocketedGems(List<string> gemNamesList, InventoryControlWrapper slotWrapper)
	{
		if (gemNamesList != null)
		{
			foreach (string string_0 in gemNamesList)
			{
				if (string.IsNullOrEmpty(string_0))
				{
					continue;
				}
				if (Class27.Instance.DebugMode)
				{
					GlobalLog.Debug("[GemHelper] Now will try to socket " + string_0 + ".");
				}
				Item gem = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name.ToLower() == string_0.ToLower());
				if ((RemoteMemoryObject)(object)gem == (RemoteMemoryObject)null)
				{
					GlobalLog.Error("[GemHelper] Failed to find unsocketed gem " + string_0 + ".");
					continue;
				}
				int index = GetEmptySocketIndexInLargestLink(slotWrapper.Inventory.Items.FirstOrDefault(), GetGemColor(gem));
				if (index == -1)
				{
					index = GetFirstUsableSocketIndex(slotWrapper.Inventory.Items.FirstOrDefault(), GetGemColor(gem));
				}
				if (index == -1)
				{
					GlobalLog.Error("[GemHelper] Found no sockets to socket unsocketed gems. Now stopping the bot.");
					BotManager.Stop(new StopReasonData("sockets_not_found", "Found no sockets to socket unsocketed gems.", (object)null), false);
					return false;
				}
				await InventoryUi.InventoryControl_Main.PickItemToCursor(gem.LocationTopLeft);
				await Inventories.WaitForCursorToHaveItem();
				slotWrapper.EquipSkillGem(index, true);
				await Inventories.WaitForCursorToBeEmpty();
			}
			return true;
		}
		return true;
	}

	public static async Task<bool> SocketGems(List<string> gemNamesList, InventoryControlWrapper slotWrapper)
	{
		if (gemNamesList != null)
		{
			foreach (string string_0 in gemNamesList)
			{
				if (string.IsNullOrEmpty(string_0))
				{
					continue;
				}
				if (Class27.Instance.DebugMode)
				{
					GlobalLog.Debug("[GemHelper] Now will try to socket " + string_0 + ".");
				}
				Item gem = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name.ToLower() == string_0.ToLower());
				if ((RemoteMemoryObject)(object)gem == (RemoteMemoryObject)null)
				{
					if (Class27.Instance.DebugMode)
					{
						GlobalLog.Error("[GemHelper] Failed to find gem " + string_0 + ".");
					}
					continue;
				}
				int index = GetEmptySocketIndexInLargestLink(slotWrapper.Inventory.Items.FirstOrDefault(), GetGemColor(gem));
				if (index == -1)
				{
					index = GetFirstUsableSocketIndex(slotWrapper.Inventory.Items.FirstOrDefault(), GetGemColor(gem));
				}
				if (index == -1)
				{
					GlobalLog.Error("[GemHelper] Found no sockets to socket gem " + string_0 + ".");
					continue;
				}
				await InventoryUi.InventoryControl_Main.PickItemToCursor(gem.LocationTopLeft);
				await Inventories.WaitForCursorToHaveItem();
				slotWrapper.EquipSkillGem(index, true);
				await Inventories.WaitForCursorToBeEmpty();
			}
			return true;
		}
		return true;
	}

	public static bool CheckIfHaveSocketForGemInSlot(Item gem, InventoryControlWrapper slotWrapper)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int firstUsableSocketIndex = GetFirstUsableSocketIndex(slotWrapper.Inventory.Items.FirstOrDefault(), GetGemColor(gem));
		if (firstUsableSocketIndex != -1)
		{
			return true;
		}
		GlobalLog.Error("[GemHelper] Found no sockets to socket gem " + gem.Name + ".");
		return false;
	}

	public static bool HaveGemInItem(Item item, string gemName)
	{
		if (string.IsNullOrEmpty(gemName))
		{
			return false;
		}
		Item[] socketedGems = item.SocketedGems;
		foreach (Item val in socketedGems)
		{
			if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null) && string.Equals(val.Name, gemName, StringComparison.CurrentCultureIgnoreCase))
			{
				if (Class27.Instance.DebugMode)
				{
					GlobalLog.Debug("[GemHelper] Found gem " + gemName + " in " + item.FullName + " " + item.Name);
				}
				return true;
			}
		}
		return false;
	}

	public static bool HaveGemInIinventory(string gemName)
	{
		if (!string.IsNullOrEmpty(gemName))
		{
			foreach (Item item in InventoryUi.InventoryControl_Main.Inventory.Items)
			{
				if (item.Name.ToLower() == gemName.ToLower())
				{
					if (Class27.Instance.DebugMode)
					{
						GlobalLog.Debug("[GemHelper] Found gem " + gemName + " in inventory.");
					}
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public static SocketColor GetGemColor(Item gem)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (!gem.GemTypes.Any((string x) => x == "intelligence"))
		{
			if (gem.GemTypes.Any((string x) => x == "strength"))
			{
				return (SocketColor)1;
			}
			if (!gem.GemTypes.Any((string x) => x == "dexterity"))
			{
				return (SocketColor)0;
			}
			return (SocketColor)2;
		}
		return (SocketColor)3;
	}
}
