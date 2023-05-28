using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx;
using ExPlugins.TraderPlugin;
using ExPlugins.TraderPlugin.Classes;

internal class Class6 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static List<string> ItemsToList;

	private readonly Vector2i vector2i_0 = default(Vector2i);

	private readonly List<string> list_0 = new List<string>();

	private int int_0;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "ScanTabsAndTakeItemsTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea)
		{
			if (TraderPlugin.Stopwatch.Elapsed.TotalSeconds - (double)int_0 < (double)TraderPluginSettings.Instance.SecondsBetweenScan && int_0 != 0)
			{
				return false;
			}
			if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
			{
				await Inventories.OpenStash();
			}
			if (TraderPluginSettings.Instance.TabNamesToIgnoreOnScan == null || !TraderPluginSettings.Instance.TabNamesToIgnoreOnScan.Any())
			{
				GlobalLog.Error("[ScanTabs] Please write something into the TabNamesToIgnoreOnScan options, for example [\"\"]");
				BotManager.Stop(new StopReasonData("tabnames_null", "Please write something into the TabNamesToIgnoreOnScan options, for example [\"\"]", (object)null), false);
				return false;
			}
			foreach (string string_0 in StashUi.TabControl.TabNames)
			{
				await Inventories.OpenStashTab(string_0, Name);
				bool nonFragmentEssence = !StashUi.StashTabInfo.IsPremiumSpecial || StashUi.StashTabInfo.IsPremiumEssence || StashUi.StashTabInfo.IsPremiumFragment;
				if (!(string_0 != TraderPluginSettings.Instance.StashTabToTrade && nonFragmentEssence) || StashUi.StashTabInfo.IsRemoveOnlyFlagged || StashUi.StashTabInfo.IsPublicFlagged || !TraderPluginSettings.Instance.TabNamesToIgnoreOnScan.All((TraderPluginSettings.NameEntry x) => x.Name != string_0))
				{
					GlobalLog.Debug($"[ScanTabs] It was either a premium tab or trade tab ({StashUi.StashTabInfo.TabType}), continue");
					continue;
				}
				List<InventoryControlWrapper> controlList = new List<InventoryControlWrapper>();
				if (StashUi.StashTabInfo.IsPremiumEssence)
				{
					controlList = EssenceTab.All.Where((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null).ToList();
				}
				if (StashUi.StashTabInfo.IsPremiumFragment)
				{
					controlList = (from c in FragmentTab.All
						where (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null
						select c into i
						orderby i.CustomTabItem.Name.ContainsIgnorecase("scarab") descending
						select i).ToList();
				}
				if (StashUi.StashTabInfo.IsPremiumSpecial)
				{
					foreach (InventoryControlWrapper control in controlList)
					{
						Item item3 = control.CustomTabItem;
						if ((RemoteMemoryObject)(object)item3 == (RemoteMemoryObject)null || !ShouldListItem(item3))
						{
							continue;
						}
						if (TraderPluginSettings.Instance.DebugMode)
						{
							GlobalLog.Debug("[ScanTabs] Now scanning item: " + item3.Name);
						}
						if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item3))
						{
							continue;
						}
						double price2 = PoeNinjaTracker.LookupChaosValue(item3);
						if ((!(item3.Class != "Map") && (int)item3.Rarity != 3) || (price2 * (double)item3.StackCount >= (double)TraderPluginSettings.Instance.MinPriceInChaosToList && (!(price2 < 2.0) || (!item3.Name.Contains("Oil") && !item3.Name.Contains("Essence")))))
						{
							if (TraderPluginSettings.Instance.DebugMode)
							{
								GlobalLog.Debug("[ScanTabs] " + item3.Name + " is in the list");
							}
							string fullName2 = item3.FullName;
							string metadata = item3.Metadata;
							InventoryControlWrapper actualControl = control;
							if (StashUi.StashTabInfo.IsPremiumFragment)
							{
								actualControl = FragmentTab.GetInventoryControlForMetadata(metadata);
							}
							await Inventories.FastMoveCustomTabItem(actualControl);
							Item itemToTake2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByFullName(fullName2);
							Vector2i posToMove2 = itemToTake2.LocationTopLeft;
							GlobalLog.Debug($"[ScanTabs] ItemToTake: {itemToTake2.Name}, {itemToTake2.LocationTopLeft}, postomove {posToMove2}");
							ItemSearchParams par2 = new ItemSearchParams(itemToTake2, posToMove2);
							TraderPlugin.ItemSearch.Enqueue(par2);
						}
					}
					continue;
				}
				foreach (Item item2 in StashUi.InventoryControl.Inventory.Items.Where((Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null).Where(ShouldListItem))
				{
					if (TraderPluginSettings.Instance.DebugMode)
					{
						GlobalLog.Debug("[ScanTabs] Now scanning item: " + item2.Name);
					}
					if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item2))
					{
						continue;
					}
					double price = PoeNinjaTracker.LookupChaosValue(item2);
					if ((!(item2.Class != "Map") && (int)item2.Rarity != 3) || (price * (double)item2.StackCount >= (double)TraderPluginSettings.Instance.MinPriceInChaosToList && (!(price < 2.0) || (!item2.Name.Contains("Oil") && !item2.Name.Contains("Essence")))))
					{
						if (TraderPluginSettings.Instance.DebugMode)
						{
							GlobalLog.Debug("[ScanTabs] " + item2.Name + " is in the list");
						}
						string fullName = item2.FullName;
						await Inventories.FastMoveFromStashTab(item2.LocationTopLeft);
						Item itemToTake = InventoryUi.InventoryControl_Main.Inventory.FindItemByFullName(fullName);
						Vector2i posToMove = itemToTake.LocationTopLeft;
						GlobalLog.Debug($"[ScanTabs] ItemToTake: {itemToTake.Name}, {itemToTake.LocationTopLeft}, postomove {posToMove}");
						ItemSearchParams par = new ItemSearchParams(itemToTake, posToMove);
						TraderPlugin.ItemSearch.Enqueue(par);
					}
				}
			}
			int_0 = (int)TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
			Thread.Sleep(1000);
			return true;
		}
		return false;
	}

	private bool ShouldListItem(Item item)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Invalid comparison between Unknown and I4
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Invalid comparison between Unknown and I4
		if (list_0.Contains(item.Name))
		{
			return false;
		}
		if (PoeNinjaTracker.LookupChaosValue(item) < 1.0)
		{
			return false;
		}
		if (!item.Name.Contains("Sacrifice at") || TraderPluginSettings.Instance.ShouldSellSacFragments)
		{
			if (!GeneralSettings.Instance.ScarabsToPrioritize.Any((NameEntry i) => item.Name.Contains(i.Name)))
			{
				if (!GeneralSettings.Instance.AnointOils.Any((OilEntry i) => item.Name.Contains(i.Name)))
				{
					return (ItemsToList.Contains(item.Name) && (int)item.Rarity != 6) || (item.Name.Contains("Stacked Deck") && TraderPluginSettings.Instance.ShouldSellStackedDicks) || (item.Class == "ExpeditionLogbook" && TraderPluginSettings.Instance.ShouldSellLogbooks) || (item.Name.Contains("Delirium Orb") && TraderPluginSettings.Instance.ShouldSellDeliriumOrbs) || (item.Name.Contains("Oil") && TraderPluginSettings.Instance.ShouldSellOils) || (item.Name.Contains("Catalyst") && TraderPluginSettings.Instance.ShouldSellCatalysts) || (item.Name.Contains("Blessing") && TraderPluginSettings.Instance.ShouldSellBlessings) || (item.Class == "DivinationCard" && !item.HasFullStack && TraderPluginSettings.Instance.ShouldSellDivCards) || ((int)item.Rarity == 3 && TraderPluginSettings.Instance.ShouldSellUniques) || (item.Metadata.Contains("ItemisedProphecy") && TraderPluginSettings.Instance.ShouldSellProphecy) || (item.Name.Contains("Essence") && TraderPluginSettings.Instance.ShouldSellEssences) || (item.Name.Contains("Remnant of Corruption") && TraderPluginSettings.Instance.ShouldSellEssences) || (item.Metadata.Contains("Metadata/Items/Currency/CurrencyDelveCrafting") && TraderPluginSettings.Instance.ShouldSellDelveCurrency) || (item.Metadata.Contains("Metadata/Items/Delve/DelveStackableSocketable") && TraderPluginSettings.Instance.ShouldSellDelveCurrency) || (item.Stats.ContainsKey((StatTypeGGG)10342) && TraderPluginSettings.Instance.ShouldSellBlightedMaps) || (item.Stats.ContainsKey((StatTypeGGG)14763) && TraderPluginSettings.Instance.ShouldSellBlightRavagedMaps) || (item.Metadata.Contains("Items/MapFragments/Maven") && item.Class == "MiscMapItem" && TraderPluginSettings.Instance.ShouldSellMavenInvitations) || (item.Metadata.Contains("Items/Currency/CurrencyRefresh") && TraderPluginSettings.Instance.ShouldSellGambleCurrency) || (item.Name.Contains("Scarab") && TraderPluginSettings.Instance.ShouldSellScarabs) || (CheckIfShaperGuardian(item) && TraderPluginSettings.Instance.ShouldListShaperGuardianMaps) || (item.Stats.ContainsKey((StatTypeGGG)6548) && TraderPluginSettings.Instance.ShouldListElderGuardianMaps) || (item.Stats.ContainsKey((StatTypeGGG)13845) && TraderPluginSettings.Instance.ShouldListAwakenerGuardianMaps) || (item.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }) && TraderPluginSettings.Instance.ShouldListClusters);
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static bool CheckIfShaperGuardian(Item map)
	{
		if (map.Stats.ContainsKey((StatTypeGGG)6827) && map.Stats[(StatTypeGGG)6827] == 1)
		{
			return map.Name.Contains("Chimera") || map.Name.Contains("Hydra") || map.Name.Contains("Phoenix") || map.Name.Contains("Minotaur");
		}
		return false;
	}

	public void Initialize()
	{
		GlobalLog.Debug("[TraderPlugin] Initialize");
	}

	public void Deinitialize()
	{
		GlobalLog.Debug("[TraderPlugin] Deinitialize");
	}

	public void Enable()
	{
		GlobalLog.Warn("[TraderPlugin] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[TraderPlugin] Disabled");
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
		ItemsToList.Clear();
		if (TraderPluginSettings.Instance.ItemsToList != null)
		{
			foreach (TraderPluginSettings.NameEntry itemsTo in TraderPluginSettings.Instance.ItemsToList)
			{
				ItemsToList.Add(itemsTo.Name);
			}
		}
		list_0.Clear();
		if (TraderPluginSettings.Instance.ItemsToIgnore == null)
		{
			return;
		}
		foreach (TraderPluginSettings.NameEntry item in TraderPluginSettings.Instance.ItemsToIgnore)
		{
			list_0.Add(item.Name);
		}
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	static Class6()
	{
		ItemsToList = new List<string>();
	}
}
