using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.StashManager;
using ExPlugins.TraderPlugin;

internal class Class15 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly List<string> list_0 = new List<string>();

	private readonly List<Item> list_1 = new List<Item>();

	private readonly HashSet<string> hashSet_0 = new HashSet<string>();

	private Stopwatch stopwatch_0;

	private bool bool_0;

	public JsonSettings Settings => (JsonSettings)(object)StashManagerSettings.Instance;

	public string Name => "ClusterRecipeTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!StashManagerSettings.Instance.ShouldMakeClusterJewelRecipe)
		{
			return false;
		}
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.IsTown && !area.IsHideoutArea)
		{
			return false;
		}
		if (stopwatch_0.Elapsed.TotalSeconds < (double)StashManagerSettings.Instance.SecondsBetweenScan && bool_0)
		{
			return false;
		}
		if (Inventories.Stash == (NetworkObject)null)
		{
			return false;
		}
		await Inventories.OpenStash();
		int smallCount = 0;
		int mediumCount = 0;
		int largeCount = 0;
		List<string> tabs = ExtensionsSettings.Instance.GetTabsForCategory("Jewels");
		foreach (string tab in tabs)
		{
			await Inventories.OpenStashTab(tab, Name);
			if (tab == TraderPluginSettings.Instance.StashTabToTrade || StashUi.StashTabInfo.IsPremiumSpecial || StashUi.StashTabInfo.IsFolder || StashUi.StashTabInfo.IsPremiumMetamorph || StashManagerSettings.Instance.TabNamesToIgnore.Contains(new NameEntry(tab)))
			{
				GlobalLog.Debug($"[StashManager(ClusterRecipe)] It was either a premium tab or trade tab ({StashUi.StashTabInfo.TabType}), continue");
				hashSet_0.Add(tab);
				continue;
			}
			foreach (Item item3 in StashUi.InventoryControl.Inventory.Items.Where((Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && !list_0.Contains(item.Name) && item.Name.Contains("Cluster")))
			{
				if ((int)item3.Rarity == 3 || PoeNinjaTracker.LookupChaosValue(item3) >= (double)TraderPluginSettings.Instance.MinPriceInChaosToList || item3.ItemLevel < StashManagerSettings.Instance.MinimumItemLevelToMakeClusterJewelRecipe)
				{
					continue;
				}
				if (item3.Name.Contains("Small"))
				{
					smallCount++;
					if (StashManagerSettings.Instance.DebugMode)
					{
						GlobalLog.Debug($"[StashManager(ClusterRecipe)] Found small cluster that meets the requirements! {item3.FullName}, item level {item3.ItemLevel}." + $" There are currently {smallCount} small jewels in the list.");
					}
				}
				if (item3.Name.Contains("Medium"))
				{
					mediumCount++;
					if (StashManagerSettings.Instance.DebugMode)
					{
						GlobalLog.Debug($"[StashManager(ClusterRecipe)] Found medium cluster that meets the requirements! {item3.FullName}, item level {item3.ItemLevel}." + $"There are currently {mediumCount} medium jewels in the list.");
					}
				}
				if (item3.Name.Contains("Large"))
				{
					largeCount++;
					if (StashManagerSettings.Instance.DebugMode)
					{
						GlobalLog.Debug($"[StashManager(ClusterRecipe)] Found large cluster that meets the requirements! {item3.FullName}, item level {item3.ItemLevel}." + $"There are currently {largeCount} large jewels in the list.");
					}
				}
			}
		}
		GlobalLog.Debug($"[StashManager(ClusterRecipe)] First scan is done! We currently have {smallCount} smalls to sell, " + $"{mediumCount} mediums to sell and {largeCount} larges to sell.");
		if (smallCount >= 5 || mediumCount >= 5 || largeCount >= 5)
		{
			int smallRecipes = smallCount / 5;
			int mediumlRecipes = mediumCount / 5;
			int largeRecipes = largeCount / 5;
			smallCount = 0;
			mediumCount = 0;
			largeCount = 0;
			bool smallBlock = false;
			bool mediumBlock = false;
			bool largeBlock = false;
			GlobalLog.Debug($"[StashManager(ClusterRecipe)] Now going to make {smallRecipes} small recipes, {mediumlRecipes} medium recipes" + $" and {largeRecipes} large recipes.");
			foreach (string tab2 in StashUi.TabControl.TabNames.Where((string t) => !hashSet_0.Contains(t) && !StashManagerSettings.Instance.TabNamesToIgnore.Contains(new NameEntry(t)) && ExtensionsSettings.Instance.GetTabsForCategory("Jewels").Contains(t)))
			{
				await Inventories.OpenStashTab(tab2, Name);
				foreach (Item item5 in StashUi.InventoryControl.Inventory.Items.Where((Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && !list_0.Contains(item.Name) && item.Name.Contains("Cluster")))
				{
					if ((int)item5.Rarity == 3 || PoeNinjaTracker.LookupChaosValue(item5) >= (double)TraderPluginSettings.Instance.MinPriceInChaosToList || item5.ItemLevel < StashManagerSettings.Instance.MinimumItemLevelToMakeClusterJewelRecipe)
					{
						continue;
					}
					if (item5.Name.Contains("Small") && smallCount < smallRecipes * 5)
					{
						if (smallBlock)
						{
							continue;
						}
						if (await TakeItemForSale(item5))
						{
							smallCount++;
						}
					}
					if (item5.Name.Contains("Medium") && mediumCount < mediumlRecipes * 5)
					{
						if (mediumBlock)
						{
							continue;
						}
						if (await TakeItemForSale(item5))
						{
							mediumCount++;
						}
					}
					if (item5.Name.Contains("Large") && largeCount < largeRecipes * 5)
					{
						if (largeBlock)
						{
							continue;
						}
						if (await TakeItemForSale(item5))
						{
							largeCount++;
						}
					}
					if (smallCount % 5 == 0 && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < 15)
					{
						smallBlock = true;
					}
					if (mediumCount % 5 == 0 && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < 15)
					{
						mediumBlock = true;
					}
					if (largeCount % 5 == 0 && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < 15)
					{
						largeBlock = true;
					}
				}
			}
			foreach (Item item4 in list_1)
			{
				if (item4.IsIdentified || await Inventories.ApplyOrb(item4.LocationTopLeft, CurrencyNames.Wisdom))
				{
					if ((int)item4.Rarity == 1)
					{
						if (!(await Inventories.ApplyOrb(item4.LocationTopLeft, CurrencyNames.Scouring)))
						{
							GlobalLog.Error("[StashManager(ClusterRecipe)] Could not apply scouring to the jewel! Now aborting.");
							OnFinishScan();
							return true;
						}
						if (!(await Inventories.ApplyOrb(item4.LocationTopLeft, CurrencyNames.Alchemy)))
						{
							GlobalLog.Error("[StashManager(ClusterRecipe)] Could not apply alchemy to the jewel! Now aborting. You might want to check your 'Save Alch' option in MapBotEx.");
							OnFinishScan();
							return true;
						}
					}
					if ((int)item4.Rarity != 0 || await Inventories.ApplyOrb(item4.LocationTopLeft, CurrencyNames.Alchemy))
					{
						continue;
					}
					GlobalLog.Error("[StashManager(ClusterRecipe)] Could not apply alchemy to the jewel! Now aborting. You might want to check your 'Save Alch' option in MapBotEx.");
					OnFinishScan();
					return true;
				}
				GlobalLog.Error("[StashManager(ClusterRecipe)] Could not apply wisdom to the jewel! Now aborting.");
				OnFinishScan();
				return true;
			}
			if (list_1.Count > 0)
			{
				List<Vector2i> coords = (from i in list_1
					orderby i.LocationTopLeft.Y descending, i.LocationTopLeft.X descending
					select i.LocationTopLeft).ToList();
				if (await TownNpcs.SellItems(coords))
				{
					GlobalLog.Debug("[StashManager(ClusterRecipe)] Clusters were sucessfully sold!");
					foreach (Item item2 in InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item it) => !it.IsIdentified))
					{
						await Inventories.OpenInventory();
						await Inventories.ApplyOrb(item2.LocationTopLeft, CurrencyNames.Wisdom);
					}
					list_1.Clear();
					if (!mediumBlock && !smallBlock && !largeBlock)
					{
						OnFinishScan();
					}
					return true;
				}
				OnFinishScan();
				ErrorManager.ReportError();
			}
			OnFinishScan();
			return false;
		}
		GlobalLog.Debug("[StashManager(ClusterRecipe)] There were not enough jewels to make a recipe!");
		OnFinishScan();
		return false;
	}

	public void Tick()
	{
	}

	private async Task<bool> TakeItemForSale(Item item)
	{
		if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item))
		{
			GlobalLog.Error("[StashManager(ClusterRecipe)] Inventory can't fit " + item.FullName);
			return false;
		}
		if (!(await Inventories.FastMoveFromStashTab(item.LocationTopLeft)))
		{
			GlobalLog.Error("[StashManager(ClusterRecipe)] Error while fast moving " + item.FullName);
			return false;
		}
		Item itm = (from i in InventoryUi.InventoryControl_Main.Inventory.Items
			orderby i.LocationTopLeft.Y descending, i.LocationTopLeft.X descending
			select i).FirstOrDefault((Item i) => i.FullName == item.FullName && !list_1.Contains(i));
		if ((RemoteMemoryObject)(object)itm == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[StashManager(ClusterRecipe)] Can't find " + item.FullName + " in inventory!");
			return false;
		}
		if (StashManagerSettings.Instance.DebugMode)
		{
			GlobalLog.Debug("[StashManager(ClusterRecipe)] Item " + item.FullName + " sucessfully added to sell list.");
		}
		list_1.Add(itm);
		return true;
	}

	private void OnFinishScan()
	{
		GlobalLog.Debug($"[StashManager(ClusterRecipe)] Last scan time: {stopwatch_0.Elapsed.TotalSeconds}");
		stopwatch_0.Restart();
		bool_0 = true;
		list_1.Clear();
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
		if (stopwatch_0 == null)
		{
			stopwatch_0 = Stopwatch.StartNew();
		}
		list_0.Clear();
		if (StashManagerSettings.Instance.ItemsToIgnore == null)
		{
			return;
		}
		foreach (NameEntry item in StashManagerSettings.Instance.ItemsToIgnore)
		{
			list_0.Add(item.Name);
		}
	}

	public void Stop()
	{
	}
}
