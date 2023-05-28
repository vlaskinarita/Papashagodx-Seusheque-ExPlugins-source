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
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.StashManager;
using ExPlugins.TraderPlugin;

internal class Class16 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private readonly List<string> list_0 = new List<string>();

	private readonly List<string> list_1 = new List<string>();

	private readonly List<string> list_2 = new List<string>();

	private readonly List<string> list_3 = new List<string>();

	private readonly List<string> list_4 = new List<string>();

	private readonly HashSet<string> hashSet_0 = new HashSet<string>();

	private bool bool_0 = true;

	private Stopwatch stopwatch_0;

	private bool bool_1;

	public JsonSettings Settings => (JsonSettings)(object)StashManagerSettings.Instance;

	public string Name => "ScanTabsAndSellItemsTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea || area.Id.Contains("Affliction"))
		{
			if (!area.Id.Contains("Affliction") || !bool_1)
			{
				if (stopwatch_0.Elapsed.TotalSeconds < (double)StashManagerSettings.Instance.SecondsBetweenScan && bool_1)
				{
					return false;
				}
				if (Inventories.Stash == (NetworkObject)null)
				{
					return false;
				}
				await Inventories.OpenStash();
				List<Item> itemsToSell = new List<Item>();
				HashSet<string> allItems = new HashSet<string>();
				List<string> tabs = StashUi.TabControl.TabNames;
				if (area.Id.Contains("Affliction") && list_4.Any())
				{
					tabs = list_4;
				}
				foreach (string tab in tabs.Where((string t) => !hashSet_0.Contains(t)))
				{
					await Inventories.OpenStashTab(tab, Name);
					if (!(tab == TraderPluginSettings.Instance.StashTabToTrade) && !StashUi.StashTabInfo.IsPremiumSpecial && !StashUi.StashTabInfo.IsFolder && !StashUi.StashTabInfo.IsPremiumMetamorph && !StashManagerSettings.Instance.TabNamesToIgnore.Contains(new NameEntry(tab)))
					{
						foreach (Item item_0 in StashUi.InventoryControl.Inventory.Items.Where((Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && !list_1.Contains(item.Name)))
						{
							allItems.Add(item_0.Name);
							if ((!list_2.Contains(item_0.Name) || item_0.StackCount < StashManagerSettings.Instance.MinStackToSell) && (!item_0.Name.Contains("Essence") || item_0.Name.Contains("Deafening") || !StashManagerSettings.Instance.ShouldSellEssences || item_0.StackCount < StashManagerSettings.Instance.MinStackToSell) && (!item_0.Name.Contains("Essence") || !StashManagerSettings.Instance.ShouldSellAllEssences) && (!list_0.Contains(item_0.Name) || !(PoeNinjaTracker.LookupChaosValue(item_0) < 1.0) || !StashManagerSettings.Instance.ShouldSellFossils) && (!item_0.Name.Contains("Offering to the Goddess") || InstanceInfo.TotalAscendencyPoints < 8 || !StashManagerSettings.Instance.ShouldSellUberLabOfferingsWhenUberLabCompleted) && (!item_0.Metadata.Contains("Items/MapFragments/Maven") || !item_0.Name.ContainsIgnorecase("The Atlas") || !(item_0.Class == "MiscMapItem") || !StashManagerSettings.Instance.ShouldSellMavenInvitations) && (!item_0.Name.Contains("Scarab") || !StashManagerSettings.Instance.ShouldSellTrashScarabs || !(PoeNinjaTracker.LookupChaosValue(item_0) < 1.0)) && (!item_0.Class.Contains("Skill Gem") || !StashManagerSettings.Instance.ShouldSellAllGems) && (!item_0.Name.Contains("Incubator") || !StashManagerSettings.Instance.ShouldSellIncubators || !(PoeNinjaTracker.LookupChaosValue(item_0) < (double)TraderPluginSettings.Instance.MinPriceInChaosToList)) && (!item_0.Name.Contains("Incubator") || !StashManagerSettings.Instance.ShoulApplyIncubators || StashTask.GetInventorySlotsWithoutIncubator().Count < 1) && !list_3.Contains(item_0.Name))
							{
								continue;
							}
							string name = item_0.Name;
							int incubatorsInInventory = Inventories.InventoryItems.Where((Item i) => i.Name.Contains("Incubator")).Sum((Item i) => i.StackCount);
							if (name.Contains("Incubator") && !list_4.Contains(tab))
							{
								list_4.Add(tab);
							}
							if ((area.Id.Contains("Affliction") && !name.Contains("Incubator")) || (incubatorsInInventory >= StashTask.GetInventorySlotsWithoutIncubator().Count && name.Contains("Incubator") && !StashManagerSettings.Instance.ShouldSellIncubators))
							{
								continue;
							}
							if ((int)item_0.Rarity >= 2)
							{
								name = item_0.FullName + " " + item_0.Name;
							}
							if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item_0))
							{
								GlobalLog.Error("[StashManager(Sell_Items)] Inventory can't fit " + name);
							}
							else if (await Inventories.FastMoveFromStashTab(item_0.LocationTopLeft))
							{
								Item itm = (from i in InventoryUi.InventoryControl_Main.Inventory.Items
									orderby i.LocationTopLeft.Y descending, i.LocationTopLeft.X descending
									select i).FirstOrDefault((Item i) => i.FullName == item_0.FullName);
								if (!((RemoteMemoryObject)(object)itm == (RemoteMemoryObject)null))
								{
									if (!name.Contains("Incubator") || StashManagerSettings.Instance.ShouldSellIncubators)
									{
										if (StashManagerSettings.Instance.DebugMode)
										{
											GlobalLog.Debug("[StashManager(Sell_Items)] Item " + name + " sucessfully added to sell list.");
										}
										itemsToSell.Add(itm);
									}
								}
								else
								{
									GlobalLog.Error("[StashManager(Sell_Items)] Can't find " + name + " in inventory!");
								}
							}
							else
							{
								GlobalLog.Error("[StashManager(Sell_Items)] Error while fast moving " + name);
							}
						}
						if (itemsToSell.Count > 0)
						{
							GlobalLog.Info($"[StashManager(Sell_Items)] {itemsToSell.Count} items to sell.");
							List<Vector2i> coords = (from i in itemsToSell
								orderby i.LocationTopLeft.Y descending, i.LocationTopLeft.X descending
								select i.LocationTopLeft).ToList();
							if (await TownNpcs.SellItems(coords))
							{
								GlobalLog.Debug("[StashManager(Sell_Items)] Exceed items were sucessfully sold!");
								return true;
							}
							ErrorManager.ReportError();
						}
					}
					else
					{
						GlobalLog.Debug($"[StashManager(Sell_Items)] It was either a premium tab or trade tab ({StashUi.StashTabInfo.TabType}), continue");
						hashSet_0.Add(tab);
					}
				}
				bool_1 = true;
				if (!area.Id.Contains("Affliction"))
				{
					stopwatch_0.Restart();
				}
				else
				{
					bool_0 = allItems.Any((string i) => i.Contains("Incubator"));
				}
				GlobalLog.Debug($"[StashManager(Sell_Items)] Scanned: {bool_1} SW: {stopwatch_0.Elapsed.TotalSeconds}s");
				return true;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (interval_0.Elapsed && !(stopwatch_0.Elapsed.TotalSeconds < 30.0) && World.CurrentArea.Id.Contains("Affliction") && StashTask.GetInventorySlotsWithoutIncubator().Count >= 1 && bool_0)
		{
			bool_1 = false;
		}
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "item_stashed_event")
		{
			CachedItem input = message.GetInput<CachedItem>(0);
			if (input.FullName.Contains("Incubator"))
			{
				bool_0 = true;
			}
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
		if (stopwatch_0 == null)
		{
			stopwatch_0 = Stopwatch.StartNew();
		}
		list_2.Clear();
		if (StashManagerSettings.Instance.ItemsToSellInStack != null)
		{
			foreach (NameEntry item in StashManagerSettings.Instance.ItemsToSellInStack)
			{
				list_2.Add(item.Name);
			}
		}
		list_3.Clear();
		if (StashManagerSettings.Instance.ItemsToForceSell != null)
		{
			foreach (NameEntry item2 in StashManagerSettings.Instance.ItemsToForceSell)
			{
				list_3.Add(item2.Name);
			}
		}
		list_1.Clear();
		if (StashManagerSettings.Instance.ItemsToIgnore != null)
		{
			foreach (NameEntry item3 in StashManagerSettings.Instance.ItemsToIgnore)
			{
				list_1.Add(item3.Name);
			}
		}
		list_0.Clear();
		list_0.Add("Encrusted Fossil");
		list_0.Add("Aetheric Fossil");
		list_0.Add("Perfect Fossil");
		list_0.Add("Metallic Fossil");
		list_0.Add("Encrusted Fossil");
		list_0.Add("Enchanted Fossil");
		list_0.Add("Pristine Fossil");
		list_0.Add("Prismatic Fossil");
		list_0.Add("Scorched Fossil");
		list_0.Add("Sanctified Fossil");
		list_0.Add("Prismatic Fossil");
		list_0.Add("Serrated Fossil");
		list_0.Add("Frigid Fossil");
		list_0.Add("Lucent Fossil");
		list_0.Add("Gilded Fossil");
		list_0.Add("Bound Fossil");
		list_0.Add("Aberrant Fossil");
		list_0.Add("Sanctified Fossil");
		list_0.Add("Corroded Fossil");
		list_0.Add("Jagged Fossil");
		list_0.Add("Bound Fossil");
		list_0.Add("Gilded Fossil");
		list_0.Add("Corroded Fossil");
		list_0.Add("Perfect Fossil");
		list_0.Add("Aberrant Fossil");
		list_0.Add("Dense Fossil");
		list_0.Add("Aetheric Fossil");
		list_0.Add("Pristine Fossil");
		list_0.Add("Serrated Fossil");
		list_0.Add("Metallic Fossil");
		list_3.Clear();
	}

	public void Stop()
	{
	}

	static Class16()
	{
		interval_0 = new Interval(1500);
	}
}
