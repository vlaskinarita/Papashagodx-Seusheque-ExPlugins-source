using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.Game.Utilities;
using ExPlugins.EXtensions;

namespace ExPlugins.MapBotEx.Helpers;

public static class AtlasHelper
{
	private static readonly List<int> list_0;

	public static PerFramesCachedValue<List<RegionInfo>> RegionCache;

	public static bool IsAtlasBossPresent => LocalData.MapMods.ContainsKey((StatTypeGGG)13845) || LocalData.MapMods.ContainsKey((StatTypeGGG)6548);

	public static bool HasMavenWitnessedMap(string mapName)
	{
		return Atlas.AreasWhereMavenHoldARecreation.Any((DatWorldAreaWrapper m) => m.Name == mapName);
	}

	public static async Task<bool> OpenAtlasUi()
	{
		if (!AtlasUi.IsOpened)
		{
			GlobalLog.Info("[OpenAtlasUi] Opening Atlas.");
			Input.SimulateKeyEvent(Binding.open_atlas_screen, true, false, false, Keys.None);
			if (!(await Wait.For(() => AtlasUi.IsOpened, "atlas ui opening")))
			{
				GlobalLog.Error("Atlas failed to open.");
				return false;
			}
			await Wait.Sleep(20);
			return true;
		}
		return true;
	}

	public static async Task<bool> CloseAtlasUi()
	{
		if (AtlasUi.IsOpened)
		{
			GlobalLog.Info("[CloseAtlasUi] Closing Atlas.");
			Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
			if (!(await Wait.For(() => !AtlasUi.IsOpened, "atlas ui closing")))
			{
				GlobalLog.Error("Atlas failed to close.");
				return false;
			}
			return true;
		}
		return true;
	}

	public static async Task<bool> RollSextantsOnVoidstones()
	{
		bool foundElevated = false;
		int errCount = 0;
		if (!GeneralSettings.Instance.UseAwakenedSextants && !GeneralSettings.Instance.UseElevatedSextants)
		{
			return false;
		}
		for (int i = 0; i < list_0.Count; i++)
		{
			if (list_0[i] > 0)
			{
				list_0[i]--;
			}
		}
		bool foundEmpty = false;
		foreach (int item in list_0)
		{
			if (item == 0)
			{
				foundEmpty = true;
			}
		}
		if (list_0.Any() && !foundEmpty)
		{
			return true;
		}
		while (true)
		{
			errCount++;
			if (errCount <= 5)
			{
				if (GeneralSettings.Instance.UseElevatedSextants && await TakeSextant())
				{
					foundElevated = true;
				}
				if (GeneralSettings.Instance.UseAwakenedSextants && !foundElevated && !(await TakeSextant(awakened: true)))
				{
					GlobalLog.Info("[RollSextantsOnVoidstones] No sextants found in stash.");
					return false;
				}
				await OpenAtlasUi();
				bool flag = (RemoteMemoryObject)(object)AtlasUi.GraspingVoidstone.Item != (RemoteMemoryObject)null;
				bool flag2 = flag;
				if (flag2)
				{
					flag2 = !(await RollVoidstone("Grasping Voidstone"));
				}
				if (flag2)
				{
					continue;
				}
				bool flag3 = (RemoteMemoryObject)(object)AtlasUi.DecayedVoidstone.Item != (RemoteMemoryObject)null;
				bool flag4 = flag3;
				if (flag4)
				{
					flag4 = !(await RollVoidstone("Decayed Voidstone"));
				}
				if (flag4)
				{
					continue;
				}
				bool flag5 = (RemoteMemoryObject)(object)AtlasUi.OmniscientVoidstone.Item != (RemoteMemoryObject)null;
				bool flag6 = flag5;
				if (flag6)
				{
					flag6 = !(await RollVoidstone("Omniscient Voidstone"));
				}
				if (!flag6)
				{
					bool flag7 = (RemoteMemoryObject)(object)AtlasUi.CerimonialVoidstone.Item != (RemoteMemoryObject)null;
					bool flag8 = flag7;
					if (flag8)
					{
						flag8 = !(await RollVoidstone("Ceremonial Voidstone"));
					}
					if (!flag8)
					{
						break;
					}
				}
				continue;
			}
			return false;
		}
		list_0.Clear();
		if ((RemoteMemoryObject)(object)AtlasUi.GraspingVoidstone.Item != (RemoteMemoryObject)null)
		{
			RememberSextantUsesAmount("Grasping Voidstone");
		}
		if ((RemoteMemoryObject)(object)AtlasUi.DecayedVoidstone.Item != (RemoteMemoryObject)null)
		{
			RememberSextantUsesAmount("Decayed Voidstone");
		}
		if ((RemoteMemoryObject)(object)AtlasUi.OmniscientVoidstone.Item != (RemoteMemoryObject)null)
		{
			RememberSextantUsesAmount("Omniscient Voidstone");
		}
		if ((RemoteMemoryObject)(object)AtlasUi.CerimonialVoidstone.Item != (RemoteMemoryObject)null)
		{
			RememberSextantUsesAmount("Ceremonial Voidstone");
		}
		ITask task = ((TaskManagerBase<ITask>)(object)MapBotEx.BotTaskManager).GetTaskByName("StashTask");
		await Coroutines.CloseBlockingWindows();
		await task.Run();
		return true;
	}

	private static async Task<bool> RollVoidstone(string voidStoneName)
	{
		int retcount = 0;
		Voidstone voidstone = AtlasUi.GraspingVoidstone;
		switch (voidStoneName)
		{
		case "Grasping Voidstone":
			voidstone = AtlasUi.GraspingVoidstone;
			break;
		case "Omniscient Voidstone":
			voidstone = AtlasUi.OmniscientVoidstone;
			break;
		case "Ceremonial Voidstone":
			voidstone = AtlasUi.CerimonialVoidstone;
			break;
		case "Decayed Voidstone":
			voidstone = AtlasUi.DecayedVoidstone;
			break;
		}
		while (true)
		{
			if (retcount <= 3)
			{
				if (!(await OpenAtlasUi()))
				{
					await Coroutines.LatencyWait();
					retcount++;
					continue;
				}
				if (await Inventories.OpenInventory())
				{
					break;
				}
				await Coroutines.LatencyWait();
				retcount++;
				continue;
			}
			return false;
		}
		while (true)
		{
			if (GeneralSettings.Instance.SextantModsToSave == null || GeneralSettings.Instance.SextantModsToSave.Count < 1)
			{
				if (voidstone.Item.Stats.Any())
				{
					GlobalLog.Info("[RollVoidstone] " + voidStoneName + " rolled successfully.");
					return true;
				}
			}
			else
			{
				foreach (NameEntry nameEntry_0 in GeneralSettings.Instance.SextantModsToSave)
				{
					StatTypeGGG key = voidstone.Item.Stats.FirstOrDefault(delegate(KeyValuePair<StatTypeGGG, int> x)
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						StatTypeGGG key2 = x.Key;
						return ((object)(StatTypeGGG)(ref key2)).ToString() == nameEntry_0.Name;
					}).Key;
					if (((object)(StatTypeGGG)(ref key)).ToString() == nameEntry_0.Name)
					{
						GlobalLog.Info("[RollVoidstone] " + voidStoneName + " rolled successfully. Mod matched: " + nameEntry_0.Name);
						return true;
					}
				}
			}
			Item sex = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Awakened Sextant" || x.Name == "Elevated Sextant");
			if ((RemoteMemoryObject)(object)sex == (RemoteMemoryObject)null)
			{
				break;
			}
			await InventoryUi.InventoryControl_Main.PickItemToCursor(sex.LocationTopLeft, rightClick: true);
			await Coroutines.ReactionWait();
			await Coroutines.LatencyWait();
			voidstone.LeftClick();
			await Coroutines.ReactionWait();
			await Coroutines.LatencyWait();
		}
		GlobalLog.Info("[RollVoidstone] Out of sextants in inventory. Now going to restock.");
		return false;
	}

	private static async Task<bool> TakeSextant(bool awakened = false)
	{
		string string_0 = (awakened ? "Awakened Sextant" : "Elevated Sextant");
		if (InventoryUi.InventoryControl_Main.Inventory.Items.All((Item x) => x.Name != string_0))
		{
			int retcount = 0;
			while (retcount <= 3)
			{
				if (await Inventories.OpenStash())
				{
					if (await Inventories.FindTabWithCurrency(string_0))
					{
						if ((int)StashUi.StashTabInfo.TabType == 3)
						{
							InventoryControlWrapper sex2 = (from i in Inventories.GetControlsWithCurrency(string_0)
								orderby i.CustomTabItem.StackCount descending
								select i).FirstOrDefault();
							if ((RemoteMemoryObject)(object)sex2 == (RemoteMemoryObject)null)
							{
								GlobalLog.Info("No " + string_0 + " found in stash");
								return false;
							}
							if (!(await Inventories.FastMoveCustomTabItem(sex2)))
							{
								ErrorManager.ReportError();
								await Coroutines.LatencyWait();
								retcount++;
								continue;
							}
						}
						else
						{
							Item sex = StashUi.InventoryControl.Inventory.Items.FirstOrDefault((Item i) => i.Name == string_0);
							if ((RemoteMemoryObject)(object)sex == (RemoteMemoryObject)null)
							{
								GlobalLog.Info("No " + string_0 + " found in stash");
								return false;
							}
							if (!(await Inventories.FastMoveFromStashTab(sex.LocationTopLeft)))
							{
								await Coroutines.LatencyWait();
								retcount++;
								continue;
							}
						}
						return true;
					}
					return false;
				}
				await Coroutines.LatencyWait();
				retcount++;
			}
			return false;
		}
		return true;
	}

	private static void RememberSextantUsesAmount(string voidStoneName)
	{
		Voidstone val = AtlasUi.GraspingVoidstone;
		switch (voidStoneName)
		{
		case "Decayed Voidstone":
			val = AtlasUi.DecayedVoidstone;
			break;
		case "Omniscient Voidstone":
			val = AtlasUi.OmniscientVoidstone;
			break;
		case "Ceremonial Voidstone":
			val = AtlasUi.CerimonialVoidstone;
			break;
		case "Grasping Voidstone":
			val = AtlasUi.GraspingVoidstone;
			break;
		}
		int value = val.Item.Stats.FirstOrDefault(delegate(KeyValuePair<StatTypeGGG, int> x)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			StatTypeGGG key = x.Key;
			return ((object)(StatTypeGGG)(ref key)).ToString() == "SextantUsesRemaining";
		}).Value;
		list_0.Add(value);
	}

	public static async Task<bool> SocketVoidstones()
	{
		if (!GeneralSettings.Instance.SocketVoidstonesFromInventory)
		{
			return false;
		}
		if (InventoryUi.InventoryControl_Main.Inventory.Items.Any((Item x) => x.Metadata.Contains("Metadata/Items/AtlasUpgrades")))
		{
			int retcount = 0;
			List<Item> items;
			Func<Item, bool> predicate;
			Item voidstone2;
			while (true)
			{
				if (retcount <= 3)
				{
					if (!(await OpenAtlasUi()))
					{
						await Coroutines.LatencyWait();
						retcount++;
						continue;
					}
					if (await Inventories.OpenInventory())
					{
						voidstone2 = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Grasping Voidstone");
						await ActuallySocketVoidstone(voidstone2);
						items = InventoryUi.InventoryControl_Main.Inventory.Items;
						predicate = (Item x) => x.Name == "Decayed Voidstone";
						break;
					}
					await Coroutines.LatencyWait();
					retcount++;
					continue;
				}
				return false;
			}
			voidstone2 = items.FirstOrDefault(predicate);
			await ActuallySocketVoidstone(voidstone2);
			voidstone2 = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Omniscient Voidstone");
			await ActuallySocketVoidstone(voidstone2);
			voidstone2 = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item x) => x.Name == "Ceremonial Voidstone");
			await ActuallySocketVoidstone(voidstone2);
			return true;
		}
		return false;
	}

	private static async Task ActuallySocketVoidstone(Item voidstone)
	{
		if ((RemoteMemoryObject)(object)voidstone != (RemoteMemoryObject)null)
		{
			GlobalLog.Info($"[SocketVoidstones] Now picking item at {voidstone.LocationTopLeft.X}, {voidstone.LocationTopLeft.Y} in the inventory");
			await InventoryUi.InventoryControl_Main.PickItemToCursor(new Vector2i(voidstone.LocationTopLeft.X, voidstone.LocationTopLeft.Y));
			await Inventories.WaitForCursorToHaveItem();
			AtlasUi.CerimonialVoidstone.LeftClick();
			await Inventories.WaitForCursorToBeEmpty();
		}
	}

	static AtlasHelper()
	{
		list_0 = new List<int>();
		RegionCache = new PerFramesCachedValue<List<RegionInfo>>((Func<List<RegionInfo>>)(() => Atlas.RegionsInfo), 60);
	}
}
