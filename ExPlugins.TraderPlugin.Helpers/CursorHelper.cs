using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.TraderPlugin.Helpers;

public static class CursorHelper
{
	public static async Task<Results.ClearCursorResults> ClearCursorTask(int maxTries = 3)
	{
		CursorItemModes cursMode = CursorItemOverlay.Mode;
		if ((int)cursMode != 0)
		{
			if ((int)cursMode == 3 || (int)cursMode == 2)
			{
				GlobalLog.Debug("[ToolPlugin][ClearCursorTask] VirtualMode detected, pressing escape to clear");
				Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
				return Results.ClearCursorResults.None;
			}
			Item cursorhasitem = CursorItemOverlay.Item;
			int attempts = 0;
			int col = default(int);
			int row = default(int);
			while ((RemoteMemoryObject)(object)cursorhasitem != (RemoteMemoryObject)null && attempts < maxTries)
			{
				if (attempts <= maxTries)
				{
					if (!InventoryUi.IsOpened)
					{
						await Inventories.OpenInventory();
						await Coroutines.LatencyWait();
						await Coroutines.ReactionWait();
						if (!InventoryUi.IsOpened)
						{
							return Results.ClearCursorResults.InventoryNotOpened;
						}
					}
					if (InventoryUi.InventoryControl_Main.Inventory.CanFitItem(cursorhasitem.Size, ref col, ref row))
					{
						PlaceCursorIntoResult res = InventoryUi.InventoryControl_Main.PlaceCursorInto(col, row, false, true);
						if ((int)res != 0)
						{
							GlobalLog.Debug($"[ToolPlugin][ClearCursorTask] Placing item into inventory failed, Err : {res}");
							PlaceCursorIntoResult val = res;
							PlaceCursorIntoResult val2 = val;
							if ((int)val2 != 2)
							{
								if ((int)val2 == 4)
								{
									return Results.ClearCursorResults.NoSpaceInInventory;
								}
								await Coroutine.Sleep(3000);
								await Coroutines.LatencyWait();
								await Coroutines.ReactionWait();
								cursorhasitem = CursorItemOverlay.Item;
								attempts++;
								continue;
							}
							return Results.ClearCursorResults.None;
						}
						if (!(await WaitForCursorToBeEmpty()))
						{
							GlobalLog.Error("[ToolPlugin][ClearCursorTask] WaitForCursorToBeEmpty failed.");
						}
						await Coroutines.ReactionWait();
						return Results.ClearCursorResults.None;
					}
					GlobalLog.Error("[ClearCursorTask] Now stopping the bot because it cannot continue.");
					BotManager.Stop(new StopReasonData("inventory_cant_fit", "Inventory can't fit item on cursor", (object)null), false);
					return Results.ClearCursorResults.NoSpaceInInventory;
				}
				return Results.ClearCursorResults.MaxTriesReached;
			}
			return Results.ClearCursorResults.None;
		}
		GlobalLog.Debug("[ToolPlugin][ClearCursorTask] Nothing is on cursor, continue execution");
		return Results.ClearCursorResults.None;
	}

	public static async Task<bool> WaitForCursorToBeEmpty(int timeout = 5000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		do
		{
			if (InstanceInfo.GetPlayerInventoryItemsBySlot((InventorySlot)12).Any())
			{
				GlobalLog.Info("[ToolPlugin][WaitForCursorToBeEmpty] Waiting for the cursor to be empty.");
				await Coroutines.LatencyWait();
				continue;
			}
			return true;
		}
		while (sw.ElapsedMilliseconds <= timeout);
		GlobalLog.Info("[ToolPlugin][WaitForCursorToBeEmpty] Timeout while waiting for the cursor to become empty.");
		return false;
	}

	public static async Task<bool> WaitForCursorToHaveItem(int timeout = 5000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		while (!InstanceInfo.GetPlayerInventoryItemsBySlot((InventorySlot)12).Any())
		{
			GlobalLog.Info("[ToolPlugin][WaitForCursorToHaveItem] Waiting for the cursor to have an item.");
			await Coroutines.LatencyWait();
			if (sw.ElapsedMilliseconds > timeout)
			{
				GlobalLog.Info("[ToolPlugin][WaitForCursorToHaveItem] Timeout while waiting for the cursor to contain an item.");
				return false;
			}
		}
		return true;
	}
}
