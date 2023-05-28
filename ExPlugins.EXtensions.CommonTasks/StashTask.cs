using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BulkTraderEx;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.MapBotEx.Tasks;
using ExPlugins.SimulacrumPluginEx.Tasks;

namespace ExPlugins.EXtensions.CommonTasks;

public class StashTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public class StashItem : CachedItem
	{
		[CompilerGenerated]
		private readonly int int_4;

		[CompilerGenerated]
		private readonly Dictionary<StatTypeGGG, int> dictionary_0;

		[CompilerGenerated]
		private readonly Vector2i vector2i_1;

		[CompilerGenerated]
		private string string_4;

		public int MapTier
		{
			[CompilerGenerated]
			get
			{
				return int_4;
			}
		}

		public Dictionary<StatTypeGGG, int> Stats
		{
			[CompilerGenerated]
			get
			{
				return dictionary_0;
			}
		}

		public Vector2i Position
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return vector2i_1;
			}
		}

		public string StashTab
		{
			[CompilerGenerated]
			get
			{
				return string_4;
			}
			[CompilerGenerated]
			set
			{
				string_4 = value;
			}
		}

		public StashItem(Item item)
			: base(item)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			vector2i_1 = item.LocationTopLeft;
			dictionary_0 = item.Stats;
			int_4 = ((!item.IsMap()) ? (-1) : item.MapTier);
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static Func<bool> func_0;

		public static Func<Item, bool> func_1;

		public static Func<bool> func_2;

		public static Func<Item, StashItem> func_3;

		public static Predicate<ExtensionsSettings.StashingRule> predicate_0;

		public static Predicate<ExtensionsSettings.StashingRule> predicate_1;

		public static Func<StashItem, string> func_4;

		public static Func<StashItem, int> func_5;

		public static Func<StashItem, int> func_6;

		public static Func<StashItem, string> func_7;

		public static Func<StashItem, string> func_8;

		public static Func<NotificationInfo, bool> func_9;

		public static ProcessNotificationEx processNotificationEx_0;

		public static Func<InventoryControlWrapper, int> func_10;

		public static Func<Item, int> func_11;

		public static Func<Item, int> func_12;

		public static ProcessNotificationEx processNotificationEx_1;

		public static Func<NotificationInfo, bool> func_13;

		public static Func<ExtensionsSettings.StashingRule, bool> func_14;

		public static Func<ExtensionsSettings.TogglableStashingRule, bool> func_15;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003CRun_003Eb__7_5()
		{
			return (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null;
		}

		internal bool _003CRun_003Eb__7_6(Item i)
		{
			return i.Metadata.StartsWith("Metadata/Items/Currency/CurrencyIncubation");
		}

		internal bool _003CRun_003Eb__7_7()
		{
			return (RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null;
		}

		internal StashItem _003CRun_003Eb__7_9(Item i)
		{
			return new StashItem(i);
		}

		internal bool _003CRun_003Eb__7_10(ExtensionsSettings.StashingRule r)
		{
			return r.Name == "AltCurrency";
		}

		internal bool _003CRun_003Eb__7_11(ExtensionsSettings.StashingRule r)
		{
			return r.Name == "AltCurrency";
		}

		internal string _003CRun_003Eb__7_0(StashItem i)
		{
			return i.StashTab;
		}

		internal int _003CRun_003Eb__7_1(StashItem i)
		{
			return i.MapTier;
		}

		internal int _003CRun_003Eb__7_2(StashItem i)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return i.Size.X * i.Size.Y;
		}

		internal string _003CRun_003Eb__7_3(StashItem i)
		{
			return i.Metadata;
		}

		internal string _003CRun_003Eb__7_4(StashItem i)
		{
			return i.FullName;
		}

		internal bool _003CRun_003Eb__7_15(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal bool _003CRun_003Eb__7_12(NotificationData x, NotificationType y)
		{
			return false;
		}

		internal int _003CRun_003Eb__7_17(InventoryControlWrapper i)
		{
			return i.CustomTabItem.StackCount;
		}

		internal int _003CRun_003Eb__7_20(Item i)
		{
			return i.StackCount;
		}

		internal int _003CRun_003Eb__7_23(Item i)
		{
			return i.StackCount;
		}

		internal bool _003CStashCard_003Eb__9_0(NotificationData x, NotificationType y)
		{
			return false;
		}

		internal bool _003CStashCard_003Eb__9_2(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal bool _003CGetNonexistentTabs_003Eb__15_0(ExtensionsSettings.StashingRule r)
		{
			return !r.Name.Equals("Tabs to ignore");
		}

		internal bool _003CGetNonexistentTabs_003Eb__15_1(ExtensionsSettings.TogglableStashingRule r)
		{
			return r.Enabled;
		}
	}

	private static bool bool_0;

	private static bool bool_1;

	private static bool bool_2;

	private static bool bool_3;

	private static readonly Interval interval_0;

	public static bool ForcedStash;

	public static Vector2i ProtectedSlot;

	public string Name => "StashTask";

	public string Description => "Task that handles item stashing.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea || area.Id.Contains("Affliction"))
		{
			Item cursorItem = CursorItemOverlay.Item;
			if (!((RemoteMemoryObject)(object)cursorItem != (RemoteMemoryObject)null))
			{
				if (ExtensionsSettings.Instance.ApplyIncubators)
				{
					Item incubator = Enumerable.FirstOrDefault(Inventories.InventoryItems, (Item i) => i.Metadata.StartsWith("Metadata/Items/Currency/CurrencyIncubation"));
					if ((RemoteMemoryObject)(object)incubator != (RemoteMemoryObject)null)
					{
						InventoryControlWrapper slot = GetInventorySlotsWithoutIncubator().FirstOrDefault();
						if ((RemoteMemoryObject)(object)slot != (RemoteMemoryObject)null)
						{
							GlobalLog.Debug("[StashTask] We have " + incubator.FullName + ", lets try to apply it");
							await Inventories.OpenInventory();
							Inventory inventory = slot.Inventory;
							Item targetItem = ((inventory == null) ? null : inventory.Items?.FirstOrDefault());
							if ((RemoteMemoryObject)(object)targetItem != (RemoteMemoryObject)null)
							{
								int retries = 0;
								while (true)
								{
									await InventoryUi.InventoryControl_Main.PickItemToCursor(incubator.LocationTopLeft, rightClick: true);
									await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null, $"pick {incubator}", 5, 1000);
									cursorItem = CursorItemOverlay.Item;
									if (!((RemoteMemoryObject)(object)cursorItem == (RemoteMemoryObject)null))
									{
										break;
									}
									GlobalLog.Debug("[StashTask] cursor is null, retry");
									retries++;
									if (retries > 3)
									{
										return true;
									}
								}
								GlobalLog.Debug("[StashTask] Item on cursor: " + cursorItem.Name);
								await slot.PlaceItemFromCursor(targetItem.LocationTopLeft);
								await Inventories.WaitForCursorToBeEmpty();
								return true;
							}
						}
					}
				}
				if (Inventories.CachedStash == null)
				{
					return false;
				}
				List<ExtensionsSettings.InventoryCurrency> inventoryCurrency = ExtensionsSettings.Instance.InventoryCurrencies.ToList();
				if (BulkTraderExSettings.Instance.ShouldTrade && World.CurrentArea.IsHideoutArea)
				{
					inventoryCurrency.Clear();
				}
				if (area.Id.Contains("Affliction") && (!ForcedStash || StartSimulacrumTask.SimulacrumWaveStarted))
				{
					return false;
				}
				List<StashItem> itemsToStash = new List<StashItem>();
				foreach (Item item_0 in Inventories.InventoryItems)
				{
					if (item_0.Metadata.Contains("MapFragments/Maven") && item_0.Class == "MiscMapItem")
					{
						itemsToStash.Add(new StashItem(item_0));
					}
					if (!(item_0.LocationTopLeft == ProtectedSlot) && (item_0.MaxLinkCount != 6 || !World.CurrentArea.Id.Contains("Affliction")) && !(item_0.Class == "QuestItem") && !(item_0.Class == "PantheonSoul") && !item_0.Metadata.Contains("HeistContractTutorial") && !item_0.Metadata.Contains("HeistContractQuest") && !item_0.Metadata.Contains("AtlasRegionUpgrade") && !item_0.Metadata.Contains("Metadata/Items/AtlasUpgrades") && (!item_0.Name.Contains("Quicksilver Flask") || ((Player)LokiPoe.Me).Level >= 12) && (!item_0.Name.Contains("Bismuth Flask") || ((Player)LokiPoe.Me).Level >= 24) && (!(item_0.Class == "StackableCurrency") || !Enumerable.Any(inventoryCurrency, (ExtensionsSettings.InventoryCurrency i) => i.Name == item_0.Name)))
					{
						itemsToStash.Add(new StashItem(item_0));
					}
				}
				foreach (ExtensionsSettings.InventoryCurrency currency in inventoryCurrency)
				{
					itemsToStash.AddRange(from i in Inventories.GetExcessCurrency(currency.Name)
						select new StashItem(i));
				}
				if (bool_2)
				{
					if (!(await Inventories.OpenStash()))
					{
						ErrorManager.ReportError();
						return true;
					}
					List<string> currencyStashTabs;
					try
					{
						currencyStashTabs = await GetCurrencyStashTabs();
						GlobalLog.Debug("[StashTask] currencyStashTabs: " + string.Join(",", currencyStashTabs));
					}
					catch (Exception e2)
					{
						GlobalLog.Error("[StashTask] Exception while trying to search for a CurrencyStash:");
						GlobalLog.Error($"[StashTask] {e2}");
						currencyStashTabs = new List<string>();
					}
					if (currencyStashTabs.Count != 0)
					{
						GlobalLog.Debug("[StashTask] Found CurrencyStashTabs: " + string.Join(",", currencyStashTabs) + ".");
						ExtensionsSettings.Instance.SetGeneralStashingRule("Currency", currencyStashTabs);
					}
					else
					{
						List<string> altCurrency = ExtensionsSettings.Instance.GeneralStashingRules.Find((ExtensionsSettings.StashingRule r) => r.Name == "AltCurrency").TabList;
						GlobalLog.Error("[StashTask] No CurrencyStashTabs found, setting Currency stash to: " + string.Join(",", altCurrency) + ".");
						ExtensionsSettings.Instance.SetGeneralStashingRule("Currency", ExtensionsSettings.Instance.GeneralStashingRules.Find((ExtensionsSettings.StashingRule r) => r.Name == "AltCurrency").TabList);
					}
					bool_2 = false;
				}
				if (bool_0)
				{
					if (!(await Inventories.OpenStash()))
					{
						ErrorManager.ReportError();
						return true;
					}
					HashSet<string> wrongTabs = GetNonexistentTabs();
					if (wrongTabs.Count > 0)
					{
						GlobalLog.Error("[StashTask] The following tabs are specified in stashing rules but do not exist in stash:");
						GlobalLog.Error(string.Join(", ", wrongTabs) ?? "");
						GlobalLog.Error("[StashTask] Please provide correct tab names.");
						BotManager.Stop(new StopReasonData("incorrect_tab_names", "The following tabs are specified in stashing rules but do not exist in stash: " + string.Join(", ", wrongTabs), (object)null), false);
						return true;
					}
					GlobalLog.Debug("[StashTask] All tabs specified in stashing rules exist in stash.");
					bool_0 = false;
				}
				if (bool_1)
				{
					if (ExtensionsSettings.Instance.FullTabsList.Count > 0 && !(await FullTabCheck()))
					{
						ErrorManager.ReportError();
						return true;
					}
					bool_1 = false;
				}
				if (itemsToStash.Count > 0)
				{
					GlobalLog.Info($"[StashTask] {itemsToStash.Count} items to stash.");
				}
				AssignStashTabs(itemsToStash);
				if (itemsToStash.Count != 0)
				{
					List<StashItem> sorted = (from i in itemsToStash
						orderby i.StashTab, i.MapTier descending, i.Size.X * i.Size.Y descending, i.Metadata, i.FullName
						select i).ToList();
					string tabCache = sorted.First().StashTab;
					foreach (StashItem stashItem_0 in sorted)
					{
						if ((int)stashItem_0.Type.ItemType == 13)
						{
							if (await StashCard(stashItem_0, "Cards"))
							{
								continue;
							}
							GlobalLog.Error("No room for \"" + stashItem_0.Name + "\" in all the stash.");
							BotManager.Stop(new StopReasonData("no_space_in_stash", "No room for \"" + stashItem_0.Name + "\" in all the stash.", (object)null), false);
							return true;
						}
						string itemName = stashItem_0.Name;
						Vector2i itemPos = stashItem_0.Position;
						string tabName = stashItem_0.StashTab;
						if (tabName != tabCache)
						{
							Utility.BroadcastMessage((object)this, "finished_stashing_in_tab", new object[1] { tabCache });
						}
						if (await Inventories.OpenStashTab(tabName, Name))
						{
							if (Inventories.StashTabCanFitItem(itemPos))
							{
								if (!NotificationHud.IsOpened || !Enumerable.Any(NotificationHud.NotificationList, (NotificationInfo x) => x.IsVisible))
								{
									bool isMap = stashItem_0.MapTier != -1 || stashItem_0.FullName.Equals("Simulacrum");
									bool specific = Enumerable.Any(ExtensionsSettings.Instance.SpecificStashingByName, (SpecialStashingRule e) => stashItem_0.Name.EqualsIgnorecase(e.Name));
									bool ignoreAffinity = specific || isMap || BulkTraderExSettings.Instance.ShouldTrade;
									CachedMapItem cached = null;
									if (isMap)
									{
										cached = new CachedMapItem(Enumerable.FirstOrDefault(Inventories.InventoryItems, (Item x) => x.LocationTopLeft == stashItem_0.Position));
									}
									if (await Inventories.FastMoveFromInventory(itemPos, waitForStackDecrease: false, ignoreAffinity))
									{
										if (isMap && CachedMaps.Instance.MapCache != null)
										{
											CachedMaps.Instance.MapCache.Maps.Add(cached);
										}
										Utility.BroadcastMessage((object)this, "item_stashed_event", new object[1] { stashItem_0 });
										tabCache = tabName;
										if ((bool_3 && !stashItem_0.Name.Contains("Oil")) || !(tabName == ExtensionsSettings.Instance.GetTabsForCategory("Currency").First()) || !GeneralSettings.Instance.AnointOils.Any())
										{
											continue;
										}
										GlobalLog.Warn("[CacheOils] Caching oils");
										foreach (OilEntry oilEntry_0 in GeneralSettings.Instance.AnointOils)
										{
											if ((int)StashUi.StashTabInfo.TabType == 3)
											{
												InventoryControlWrapper oil2 = (from i in Enumerable.Where(CurrencyTab.NonCurrency, (InventoryControlWrapper i) => (RemoteMemoryObject)(object)i.CustomTabItem != (RemoteMemoryObject)null && i.CustomTabItem.Name.Equals(oilEntry_0.Name))
													orderby i.CustomTabItem.StackCount descending
													select i).FirstOrDefault();
												if (!((RemoteMemoryObject)(object)oil2 == (RemoteMemoryObject)null))
												{
													int oilSum2 = oil2.CustomTabItem.StackCount;
													if (GeneralSettings.Instance.AnointOils.First((OilEntry o) => o.Name.EqualsIgnorecase(oilEntry_0.Name)).CurrentAmount != oilSum2)
													{
														GlobalLog.Debug($"[CacheOils] {oilEntry_0.Name} new amount: {oilSum2}");
														GeneralSettings.Instance.AnointOils.First((OilEntry o) => o.Name.EqualsIgnorecase(oilEntry_0.Name)).CurrentAmount = oilSum2;
														TakeMapTask.TempIgnoreBlighted = false;
														TakeMapTask.TempIgnoreRavaged = false;
													}
												}
												else
												{
													oilEntry_0.CurrentAmount = 0;
												}
												continue;
											}
											Item oil = Enumerable.FirstOrDefault(StashUi.InventoryControl.Inventory.Items.OrderByDescending((Item i) => i.StackCount), (Item i) => i.Name.Equals(oilEntry_0.Name));
											if ((RemoteMemoryObject)(object)oil == (RemoteMemoryObject)null)
											{
												oilEntry_0.CurrentAmount = 0;
												continue;
											}
											int oilSum = Enumerable.Where(StashUi.InventoryControl.Inventory.Items, (Item i) => i.FullName.EqualsIgnorecase(oilEntry_0.Name)).Sum((Item i) => i.StackCount);
											if (GeneralSettings.Instance.AnointOils.First((OilEntry o) => o.Name.EqualsIgnorecase(oilEntry_0.Name)).CurrentAmount != oilSum)
											{
												GlobalLog.Debug($"[CacheOils] {oilEntry_0.Name} new amount: {oilSum}");
												GeneralSettings.Instance.AnointOils.First((OilEntry o) => o.Name.EqualsIgnorecase(oilEntry_0.Name)).CurrentAmount = oilSum;
												TakeMapTask.TempIgnoreBlighted = false;
												TakeMapTask.TempIgnoreRavaged = false;
											}
										}
										bool_3 = true;
										continue;
									}
									ErrorManager.ReportError();
									return true;
								}
								object obj = _003C_003Ec.processNotificationEx_0;
								if (obj == null)
								{
									ProcessNotificationEx val = (NotificationData x, NotificationType y) => false;
									obj = (object)val;
									_003C_003Ec.processNotificationEx_0 = val;
								}
								NotificationHud.HandleNotificationEx((ProcessNotificationEx)obj, true);
								await Coroutines.ReactionWait();
								await Coroutines.LatencyWait();
								return true;
							}
							Item lajtItem = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
							if ((RemoteMemoryObject)(object)lajtItem == (RemoteMemoryObject)null)
							{
								GlobalLog.Warn($"[StashTask] There is no item at {itemPos}. No reason to mark stash tab as full.");
								return true;
							}
							if (StashUi.StashTabInfo.IsPremiumSpecial)
							{
								string metadata = stashItem_0.Metadata;
								GlobalLog.Warn("[StashTask] Cannot fit \"" + itemName + "\" to \"" + tabName + "\" tab.");
								GlobalLog.Warn("[StashTask] Now marking inventory control for \"" + metadata + "\" as full.");
								ExtensionsSettings.Instance.MarkTabAsFull(tabName, metadata);
							}
							else
							{
								GlobalLog.Warn("[StashTask] Cannot fit \"" + itemName + "\" to \"" + tabName + "\" tab. Now marking this tab as full.");
								ExtensionsSettings.Instance.MarkTabAsFull(tabName, null);
							}
							return true;
						}
						ErrorManager.ReportError();
						return true;
					}
					Utility.BroadcastMessage((object)this, "items_stashed_event", Array.Empty<object>());
					Utility.BroadcastMessage((object)this, "finished_stashing_in_tab", new object[1] { tabCache });
					return true;
				}
				LootItemTask.IsInPreTownrunMode = false;
				ForcedStash = false;
				return false;
			}
			int x2 = default(int);
			int y2 = default(int);
			if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(cursorItem.Size, ref x2, ref y2))
			{
				GlobalLog.Error("[SortInventoryTask] Unexpected error. Cannot fit item anywhere in main inventory.");
				ErrorManager.ReportCriticalError();
				return false;
			}
			await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(x2, y2));
			await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null, "Cleaning cursor");
			return true;
		}
		return false;
	}

	public void Start()
	{
		RequestFullTabCheck();
	}

	private async Task<bool> StashCard(StashItem item, string category)
	{
		while (true)
		{
			string string_0 = item.Name;
			string string_1 = item.FullName;
			Vector2i itemPos = item.Position;
			List<string> tabs = ExtensionsSettings.Instance.GetTabsForCategory(category);
			foreach (string tab in tabs)
			{
				GlobalLog.Debug("[StashCard] checking stash \"" + tab + "\".");
				if (await Inventories.OpenStashTab(tab, Name))
				{
					InventoryControlWrapper invCtrl = StashUi.InventoryControl;
					if (!((RemoteMemoryObject)(object)invCtrl == (RemoteMemoryObject)null))
					{
						Inventory inv = invCtrl.Inventory;
						if (inv.AvailableInventorySquares < 1)
						{
							List<Item> similarItems = Enumerable.Where(inv.Items, (Item x) => x.Name == string_0 && x.FullName == string_1 && x.StackCount < x.MaxStackCount).ToList();
							if (similarItems.Count <= 0)
							{
								GlobalLog.Debug("[StashCard] Tab \"" + tab + "\" cant host \"" + string_0 + "\".");
								continue;
							}
						}
						while (NotificationHud.IsOpened && Enumerable.Any(NotificationHud.NotificationList, (NotificationInfo x) => x.IsVisible))
						{
							object obj = _003C_003Ec.processNotificationEx_1;
							if (obj == null)
							{
								ProcessNotificationEx val = (NotificationData x, NotificationType y) => false;
								obj = (object)val;
								_003C_003Ec.processNotificationEx_1 = val;
							}
							NotificationHud.HandleNotificationEx((ProcessNotificationEx)obj, true);
							await Coroutines.ReactionWait();
							await Coroutines.LatencyWait();
						}
						if (!(await Inventories.FastMoveFromInventory(itemPos)))
						{
							ErrorManager.ReportError();
							return true;
						}
						GlobalLog.Warn("[Events] Item stashed (" + string_1 + ")");
						Utility.BroadcastMessage((object)this, "item_stashed_event", new object[1] { item });
						return true;
					}
					GlobalLog.Debug("[StashCard] Tab \"" + tab + "\" cant host \"" + string_0 + "\", probably wrong tab type. Check if tab names are not unique.");
					ErrorManager.ReportError();
					continue;
				}
				ErrorManager.ReportError();
				return true;
			}
			if (category == "Other")
			{
				break;
			}
			category = "Other";
		}
		return false;
	}

	private static async Task<List<string>> GetCurrencyStashTabs()
	{
		List<string> result = new List<string>();
		List<string> actualTabs = StashUi.TabControl.TabNames;
		foreach (string tabName in actualTabs)
		{
			int retryCount = 0;
			do
			{
				if (!(await Inventories.OpenStashTab(tabName, "GetCurrencyStashTabs")))
				{
					retryCount++;
					ErrorManager.ReportError();
					continue;
				}
				if ((int)StashUi.StashTabInfo.TabType == 3 && !StashUi.StashTabInfo.IsRemoveOnlyFlagged)
				{
					result.Add(tabName);
				}
				break;
			}
			while (retryCount < 3);
		}
		return result;
	}

	public static List<InventoryControlWrapper> GetInventorySlotsWithoutIncubator()
	{
		List<InventoryControlWrapper> list = new List<InventoryControlWrapper>();
		foreach (InventoryControlWrapper allInventoryControl in InventoryUi.AllInventoryControls)
		{
			if (!((RemoteMemoryObject)(object)allInventoryControl == (RemoteMemoryObject)null) && !((RemoteMemoryObject)(object)allInventoryControl == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryMainHand) && !((RemoteMemoryObject)(object)allInventoryControl == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryOffHand) && !((RemoteMemoryObject)(object)allInventoryControl == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main) && !((RemoteMemoryObject)(object)allInventoryControl == (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Flasks))
			{
				Inventory inventory = allInventoryControl.Inventory;
				Item val = ((inventory == null) ? null : inventory.Items?.FirstOrDefault());
				if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null) && !val.HasIncubator)
				{
					list.Add(allInventoryControl);
				}
			}
		}
		return list;
	}

	private static void AssignStashTabs(IEnumerable<StashItem> items)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Invalid comparison between Unknown and I4
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Invalid comparison between Unknown and I4
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Invalid comparison between Unknown and I4
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Invalid comparison between Unknown and I4
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Invalid comparison between Unknown and I4
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Invalid comparison between Unknown and I4
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Invalid comparison between Unknown and I4
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Invalid comparison between Unknown and I4
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Invalid comparison between Unknown and I4
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Invalid comparison between Unknown and I4
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		foreach (StashItem stashItem_0 in items)
		{
			ItemTypes itemType = stashItem_0.Type.ItemType;
			string metadata = stashItem_0.Metadata;
			SpecialStashingRule specialStashingRule = Enumerable.FirstOrDefault(ExtensionsSettings.Instance.SpecificStashingByName, (SpecialStashingRule e) => stashItem_0.Name.EqualsIgnorecase(e.Name));
			if (specialStashingRule != null)
			{
				GlobalLog.Debug("[StashTask] " + stashItem_0.Name + " will be stashed to " + specialStashingRule.TabName + " according to specific rules!");
				stashItem_0.StashTab = specialStashingRule.TabName;
			}
			else if ((int)itemType == 1 || stashItem_0.Metadata.Contains("CurrencyHellscape"))
			{
				if (metadata.Contains("CurrencyItemisedSextantModifier"))
				{
					stashItem_0.StashTab = GetTabForStashing("Charged Compass", metadata);
				}
				else if (metadata.Contains("Essence") && !metadata.Contains("CorruptMonolith"))
				{
					stashItem_0.StashTab = GetTabForStashing("Essences", metadata);
				}
				else if (metadata.StartsWith("Metadata/Items/Currency/Mushrune"))
				{
					if (Enumerable.Any(GeneralSettings.Instance.AnointOils, (OilEntry o) => stashItem_0.FullName.Equals(o.Name) && o.CurrentAmount < 200))
					{
						stashItem_0.StashTab = GetTabForStashing("Currency", metadata);
					}
					else
					{
						stashItem_0.StashTab = GetTabForStashing("Oils", metadata);
					}
				}
				else if (metadata.Contains("Metadata/Items/Currency/CurrencyEldritch"))
				{
					stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
				}
				else if (stashItem_0.Name.Contains("Oil Extractor"))
				{
					stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
				}
				else if (metadata.Contains("Currency/CurrencyRefresh"))
				{
					stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
				}
				else if (metadata.Contains("CurrencyJewelleryQuality"))
				{
					stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
				}
				else if (metadata.Contains("CurrencyDelve") || metadata.Contains("DelveStackable"))
				{
					stashItem_0.StashTab = GetTabForStashing("Delve", metadata);
				}
				else if (metadata.Contains("CurrencyRerollRareVeiled"))
				{
					stashItem_0.StashTab = GetTabForStashing("Currency", metadata);
				}
				else if (metadata.StartsWith("Metadata/Items/Currency/CurrencyLegion"))
				{
					stashItem_0.StashTab = GetTabForStashing("Splinters", metadata);
				}
				else if (metadata.Contains("CurrencyBreach"))
				{
					stashItem_0.StashTab = GetTabForStashing("Splinters", metadata);
				}
				else if (stashItem_0.Name.Equals("Stacked Deck"))
				{
					stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
				}
				else if (metadata.Contains("CurrencyAffliction"))
				{
					stashItem_0.StashTab = GetTabForStashing(stashItem_0.Name.ContainsIgnorecase("splinter") ? "Simulacrums" : "Uniques", metadata);
				}
				else if (metadata.Contains("CurrencyRitual"))
				{
					stashItem_0.StashTab = GetTabForStashing("Splinters", metadata);
				}
				else
				{
					stashItem_0.StashTab = GetTabForStashing(stashItem_0.Name, metadata, forCurrency: true);
				}
			}
			else if (metadata.Contains("MapFragments/CurrencyAfflictionFragment") || stashItem_0.FullName.Equals("Simulacrum"))
			{
				stashItem_0.StashTab = GetTabForStashing("Simulacrums", metadata);
			}
			else if (stashItem_0.Stats.ContainsKey((StatTypeGGG)6827) || stashItem_0.Stats.ContainsKey((StatTypeGGG)13845))
			{
				stashItem_0.StashTab = GetTabForStashing("Influenced Maps", metadata);
			}
			else if (stashItem_0.Stats.ContainsKey((StatTypeGGG)10342))
			{
				stashItem_0.StashTab = GetTabForStashing("Blighted Maps", metadata);
			}
			else if (metadata.Contains("ItemisedTemple") || stashItem_0.Name.Contains("Reliquary Key") || metadata.Contains("Heist/HeistContract") || metadata.Contains("Heist/HeistBlueprint"))
			{
				stashItem_0.StashTab = GetTabForStashing("Uniques", metadata);
			}
			else if ((int)itemType == 9)
			{
				stashItem_0.StashTab = GetTabForStashing("Gems", metadata);
			}
			else if (stashItem_0.Class == "MetamorphosisDNA")
			{
				stashItem_0.StashTab = GetTabForStashing("AltCurrency", metadata);
			}
			else if ((int)itemType == 13)
			{
				stashItem_0.StashTab = GetTabForStashing("Cards", metadata);
			}
			else if ((int)itemType == 12 || (int)itemType == 23)
			{
				Item item = Enumerable.FirstOrDefault(Inventories.InventoryItems, (Item i) => i.LocationTopLeft == stashItem_0.Position);
				stashItem_0.StashTab = GetTabForStashing((PoeNinjaTracker.LookupChaosValue(item) > 50.0) ? "Expensive Jewels [>50c worth]" : "Jewels", metadata);
			}
			else if ((int)itemType == 10)
			{
				stashItem_0.StashTab = GetTabForStashing("Maps", metadata);
			}
			else if ((int)itemType == 11)
			{
				stashItem_0.StashTab = GetTabForStashing("Fragments", metadata);
			}
			else if ((int)itemType == 17)
			{
				stashItem_0.StashTab = GetTabForStashing("Splinters", metadata);
			}
			else
			{
				Rarity rarity = stashItem_0.Rarity;
				if ((int)rarity == 2)
				{
					stashItem_0.StashTab = GetTabForStashing("Rares", metadata);
					continue;
				}
				if ((int)rarity == 3)
				{
					stashItem_0.StashTab = GetTabForStashing("Uniques", metadata);
					continue;
				}
				GlobalLog.Warn($"[StashTask] Cannot determine stash tab for \"{stashItem_0.FullName}\" ({rarity}). It will be stashed to \"Other\" tabs.");
				stashItem_0.StashTab = GetTabForOther(metadata);
			}
		}
	}

	private static string GetTabForStashing(string name, string metadata, bool forCurrency = false)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		ExtensionsSettings extensionsSettings_0 = ExtensionsSettings.Instance;
		List<string> list = ((!forCurrency) ? extensionsSettings_0.GetTabsForCategory(name) : extensionsSettings_0.GetTabsForCurrency(name));
		string text = list.Find((string t) => !extensionsSettings_0.IsTabFull(t, metadata));
		if (text == null)
		{
			GlobalLog.Warn("[StashTask] All tabs for \"" + name + "\" are full. This item will be stashed to \"Other\" tabs.");
			list = extensionsSettings_0.GetTabsForCategory("Other");
			text = list.Find((string t) => !extensionsSettings_0.IsTabFull(t, metadata));
			if (text == null)
			{
				GlobalLog.Error("[StashTask] All tabs for \"" + name + "\" are full and all \"Other\" tabs are full. Now stopping the bot because it cannot continue.");
				GlobalLog.Error("[StashTask] Please clean your tabs.");
				Utility.BroadcastMessage((object)null, "mapbot_outofmaps", new object[1] { "[StashTask] All tabs for \"" + name + "\" are full and all \"Other\" tabs are full. Now stopping the bot because it cannot continue." });
				BotManager.Stop(new StopReasonData("tabs_full", "All tabs for \"" + name + "\" are full and all \"Other\" tabs are full. Now stopping the bot because it cannot continue.", (object)null), false);
				return null;
			}
		}
		return text;
	}

	private static string GetTabForOther(string metadata)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		ExtensionsSettings extensionsSettings_0 = ExtensionsSettings.Instance;
		List<string> tabsForCategory = extensionsSettings_0.GetTabsForCategory("Other");
		string text = tabsForCategory.Find((string t) => !extensionsSettings_0.IsTabFull(t, metadata));
		if (text != null)
		{
			return text;
		}
		GlobalLog.Error("[StashTask] All tabs for \"Other\" are full. Now stopping the bot because it cannot continue.");
		GlobalLog.Error("[StashTask] Please clean your tabs.");
		Utility.BroadcastMessage((object)null, "mapbot_outofmaps", new object[1] { "[StashTask] All tabs for \"Other\" are full. Now stopping the bot because it cannot continue." });
		BotManager.Stop(new StopReasonData("tabs_full", "All tabs for \"Other\" are full.", (object)null), false);
		return null;
	}

	private static HashSet<string> GetNonexistentTabs()
	{
		HashSet<string> hashSet = new HashSet<string>();
		List<string> tabNames = StashUi.TabControl.TabNames;
		foreach (ExtensionsSettings.StashingRule item in Enumerable.Where(ExtensionsSettings.Instance.GeneralStashingRules, (ExtensionsSettings.StashingRule r) => !r.Name.Equals("Tabs to ignore")))
		{
			foreach (string tab in item.TabList)
			{
				if (!tabNames.Contains(tab))
				{
					hashSet.Add(tab);
				}
			}
		}
		foreach (ExtensionsSettings.TogglableStashingRule item2 in Enumerable.Where(ExtensionsSettings.Instance.CurrencyStashingRules, (ExtensionsSettings.TogglableStashingRule r) => r.Enabled))
		{
			foreach (string tab2 in item2.TabList)
			{
				if (!tabNames.Contains(tab2))
				{
					hashSet.Add(tab2);
				}
			}
		}
		return hashSet;
	}

	public static async Task<bool> FullTabCheck()
	{
		if (!(await Inventories.OpenStash()))
		{
			return false;
		}
		List<string> actualTabs = StashUi.TabControl.TabNames;
		List<ExtensionsSettings.FullTabInfo> cleanedTabs = new List<ExtensionsSettings.FullTabInfo>();
		foreach (ExtensionsSettings.FullTabInfo tab2 in ExtensionsSettings.Instance.FullTabsList)
		{
			string tabName = tab2.Name;
			if (!actualTabs.Contains(tabName))
			{
				GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab no longer exists. Removing it from full tab list.");
				cleanedTabs.Add(tab2);
				continue;
			}
			if (await Inventories.OpenStashTab(tabName, "FullTabCheck"))
			{
				if (StashUi.StashTabInfo.IsPremiumMap)
				{
					cleanedTabs.Add(tab2);
				}
				else if (StashUi.StashTabInfo.IsPremiumSpecial && !StashUi.StashTabInfo.IsPremiumQuad)
				{
					List<string> controlsMetadata = tab2.ControlsMetadata;
					List<string> cleanedControls = new List<string>();
					foreach (string metadata in controlsMetadata)
					{
						if (PremiumTabCanFit(metadata))
						{
							GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab is no longer full for \"" + metadata + "\".");
							cleanedControls.Add(metadata);
						}
						else
						{
							GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab is still full for \"" + metadata + "\".");
						}
					}
					foreach (string cleaned in cleanedControls)
					{
						controlsMetadata.Remove(cleaned);
					}
					if (controlsMetadata.Count == 0)
					{
						GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab does not contain any controls metadata. Removing it from full tab list.");
						cleanedTabs.Add(tab2);
					}
				}
				else if (StashUi.InventoryControl.Inventory.AvailableInventorySquares >= 8)
				{
					GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab is no longer full.");
					cleanedTabs.Add(tab2);
				}
				else
				{
					GlobalLog.Warn("[FullTabCheck] \"" + tabName + "\" tab is still full.");
				}
				continue;
			}
			return false;
		}
		foreach (ExtensionsSettings.FullTabInfo tab in cleanedTabs)
		{
			ExtensionsSettings.Instance.FullTabsList.Remove(tab);
		}
		return true;
	}

	public static bool PremiumTabCanFit(string metadata)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		InventoryTabType tabType = StashUi.StashTabInfo.TabType;
		if ((int)tabType == 3)
		{
			InventoryControlWrapper inventoryControlForMetadata = CurrencyTab.GetInventoryControlForMetadata(metadata);
			if (!((RemoteMemoryObject)(object)inventoryControlForMetadata != (RemoteMemoryObject)null) || !ControlCanFit(inventoryControlForMetadata, metadata))
			{
				return Enumerable.Any(CurrencyTab.NonCurrency, (InventoryControlWrapper miscControl) => ControlCanFit(miscControl, metadata));
			}
			return true;
		}
		if ((int)tabType != 8)
		{
			if ((int)tabType != 6)
			{
				if ((int)tabType == 9)
				{
					InventoryControlWrapper inventoryControlForMetadata2 = FragmentTab.GetInventoryControlForMetadata(metadata);
					if (metadata.Contains("Metadata/Items/MapFragments/Maven"))
					{
						return EldrichMaven.MavenInventory.Inventory.AvailableInventorySquares > 0;
					}
					if (metadata.Contains("Metadata/Items/MapFragments/Primordial/Currency"))
					{
						return EldrichMaven.EldrichInventory.Inventory.AvailableInventorySquares > 0;
					}
					if ((RemoteMemoryObject)(object)inventoryControlForMetadata2 != (RemoteMemoryObject)null && ControlCanFit(inventoryControlForMetadata2, metadata))
					{
						return true;
					}
				}
				GlobalLog.Error($"[StashTask] PremiumTabCanFit was called for unknown premium tab type. ({StashUi.StashTabInfo.TabType})");
				return false;
			}
			return metadata.Contains("Metadata/Items/DivinationCards");
		}
		InventoryControlWrapper inventoryControlForMetadata3 = EssenceTab.GetInventoryControlForMetadata(metadata);
		if (!((RemoteMemoryObject)(object)inventoryControlForMetadata3 != (RemoteMemoryObject)null) || !ControlCanFit(inventoryControlForMetadata3, metadata))
		{
			return Enumerable.Any(EssenceTab.NonEssences, (InventoryControlWrapper miscControl) => ControlCanFit(miscControl, metadata));
		}
		return true;
	}

	private static bool ControlCanFit(InventoryControlWrapper control, string metadata)
	{
		Item customTabItem = control.CustomTabItem;
		if (!((RemoteMemoryObject)(object)customTabItem == (RemoteMemoryObject)null))
		{
			if (metadata == "Metadata/Items/Currency/CurrencyItemisedProphecy")
			{
				return false;
			}
			return customTabItem.Metadata == metadata && customTabItem.StackCount < 5000;
		}
		return true;
	}

	public static void RequestInvalidTabCheck()
	{
		bool_0 = true;
	}

	public static void RequestFullTabCheck()
	{
		bool_1 = true;
	}

	public MessageResult Message(Message message)
	{
		if (message.Id == "area_changed_event")
		{
			Inventories.CachedStash = null;
		}
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
		if (interval_0.Elapsed && Inventories.CachedStash == null && Inventories.Stash != (NetworkObject)null)
		{
			Inventories.CachedStash = new CachedObject(Inventories.Stash);
		}
	}

	public void Stop()
	{
	}

	static StashTask()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		bool_0 = true;
		bool_1 = true;
		bool_2 = true;
		interval_0 = new Interval(450);
		ProtectedSlot = new Vector2i(100, 100);
	}
}
