using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.BulkTraderEx.Classes;

public static class BulkTraderExData
{
	private static bool bool_0;

	public static List<CachedItemObject> CachedItemsInStash;

	public static void UpdateCurrentTab()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Invalid comparison between Unknown and I4
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		if (!LokiPoe.IsInGame)
		{
			GlobalLog.Error("[CurrencyExchangePlugin][UpdateCurrentTab] Disconnected?");
		}
		else if (StashUi.StashTabInfo.IsFolder)
		{
			GlobalLog.Debug("[CurrencyExchangePlugin][UpdateCurrentTab] The tab \"" + StashUi.TabControl.CurrentTabName + "\" is Folder and is not gonna be cached");
		}
		else if (!StashUi.StashTabInfo.IsRemoveOnlyFlagged)
		{
			InventoryTabType tabType = StashUi.StashTabInfo.TabType;
			if (!StashUi.StashTabInfo.IsPremiumSpecial || (int)tabType == 3 || (int)tabType == 8 || (int)tabType == 9)
			{
				CachedItemsInStash.RemoveAll((CachedItemObject i) => i.TabName.Equals(StashUi.TabControl.CurrentTabName));
				if ((int)StashUi.StashTabInfo.TabType != 8)
				{
					if ((int)StashUi.StashTabInfo.TabType == 3)
					{
						IEnumerable<InventoryControlWrapper> enumerable = CurrencyTab.All.Where((InventoryControlWrapper wrp) => (RemoteMemoryObject)(object)wrp.CustomTabItem != (RemoteMemoryObject)null);
						foreach (InventoryControlWrapper item in enumerable)
						{
							CachedItemsInStash.Add(new CachedItemObject(item, item.CustomTabItem, StashUi.TabControl.CurrentTabName));
						}
					}
					else if ((int)StashUi.StashTabInfo.TabType == 9)
					{
						IEnumerable<InventoryControlWrapper> enumerable2 = FragmentTab.All.Where((InventoryControlWrapper wrp) => (RemoteMemoryObject)(object)wrp.CustomTabItem != (RemoteMemoryObject)null);
						foreach (InventoryControlWrapper item2 in enumerable2)
						{
							CachedItemsInStash.Add(new CachedItemObject(item2, item2.CustomTabItem, StashUi.TabControl.CurrentTabName));
						}
						InventoryControlWrapper eldrichInventory = EldrichMaven.EldrichInventory;
						foreach (Item item3 in eldrichInventory.Inventory.Items.Where((Item it) => (RemoteMemoryObject)(object)it != (RemoteMemoryObject)null))
						{
							CachedItemsInStash.Add(new CachedItemObject(EldrichMaven.EldrichInventory, item3, StashUi.TabControl.CurrentTabName));
						}
					}
					else
					{
						IEnumerable<Item> enumerable3 = StashUi.InventoryControl.Inventory.Items.Where((Item it) => (RemoteMemoryObject)(object)it != (RemoteMemoryObject)null);
						foreach (Item item4 in enumerable3)
						{
							CachedItemsInStash.Add(new CachedItemObject(StashUi.InventoryControl, item4, StashUi.TabControl.CurrentTabName));
						}
					}
				}
				else
				{
					IEnumerable<InventoryControlWrapper> enumerable4 = EssenceTab.All.Where((InventoryControlWrapper wrp) => (RemoteMemoryObject)(object)wrp != (RemoteMemoryObject)null && (RemoteMemoryObject)(object)wrp.CustomTabItem != (RemoteMemoryObject)null);
					foreach (InventoryControlWrapper item5 in enumerable4)
					{
						CachedItemsInStash.Add(new CachedItemObject(item5, item5.CustomTabItem, StashUi.TabControl.CurrentTabName));
					}
				}
				GlobalLog.Debug($"[CurrencyExchangePlugin][UpdateCurrentTab] Cached {tabType} tab \"{StashUi.TabControl.CurrentTabName}\"");
			}
			else
			{
				GlobalLog.Debug($"[CurrencyExchangePlugin][UpdateCurrentTab] The tab \"{StashUi.TabControl.CurrentTabName}\" is premium tab of unsupported type: {tabType}");
			}
		}
		else
		{
			GlobalLog.Debug("[CurrencyExchangePlugin][UpdateCurrentTab] The tab \"" + StashUi.TabControl.CurrentTabName + "\" is RemoveOnly and is not gonna be cached");
		}
	}

	public static async Task<bool> UpdateItemsInTabs(bool force = false, List<string> tabs = null)
	{
		if (!bool_0 || force)
		{
			if (!LokiPoe.IsInGame)
			{
				GlobalLog.Error("[CurrencyExchangePlugin][UpdateItemsInStash] Disconnected?");
				return false;
			}
			await Inventories.OpenStash();
			List<string> toCheck = tabs ?? StashUi.TabControl.TabNames;
			if (force)
			{
				GlobalLog.Warn("[CurrencyExchangePlugin][UpdateItemsInStash] Clearing cache");
				CachedItemsInStash.Clear();
			}
			foreach (string tab in toCheck)
			{
				await Inventories.OpenStashTab(tab, "BulkTraderExData");
				UpdateCurrentTab();
			}
			if (tabs == null)
			{
				bool_0 = true;
			}
			return true;
		}
		return true;
	}

	static BulkTraderExData()
	{
		CachedItemsInStash = new List<CachedItemObject>();
	}
}
