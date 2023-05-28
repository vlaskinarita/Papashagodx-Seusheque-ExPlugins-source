using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BulkTraderEx;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.PapashaCore;

namespace ExPlugins.ItemFilterEx;

public class ItemFilterEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	public const string ItemWithdrawnEvent = "item_withdrawn_event";

	private static readonly Interval interval_0;

	public static string CharName;

	public static int CharLevel;

	public static readonly Stopwatch UptimeTimer;

	private ItemFilterExGui itemFilterExGui_0;

	public static long StartExp;

	public JsonSettings Settings => (JsonSettings)(object)ItemFilterExSettings.Instance;

	public UserControl Control => itemFilterExGui_0 ?? (itemFilterExGui_0 = new ItemFilterExGui());

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "ItemFilterEx";

	public string Author => "Lajt //Seusheque mod";

	public string Description => "Simple, Fast and Reliable ItemFilter ;)";

	public string Version => "4.8.0";

	public void Initialize()
	{
		if (LokiPoe.IsInGame && StartExp == 0L)
		{
			StartExp = ((Player)LokiPoe.Me).Experience;
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "area_changed_event")
		{
			return (MessageResult)0;
		}
		if (message.Id == "item_stashed_event")
		{
			HandleCache(message);
			return (MessageResult)0;
		}
		if (message.Id == "items_sold_event")
		{
			List<CachedItem> input = message.GetInput<List<CachedItem>>(1);
			HandleSoldEvent(input);
			return (MessageResult)0;
		}
		if (!(message.Id == "item_withdrawn_event"))
		{
			if (!(message.Id == "player_died_event"))
			{
				if (!(message.Id == "next_trade_in"))
				{
					if (message.Id == "luf_getCurrencyCache")
					{
						message.AddOutput<Dictionary<string, int>>((IMessageHandler)(object)this, ItemFilterExSettings.Instance.CachedCurrency, "");
						return (MessageResult)0;
					}
					if (message.Id == "luf_getFarmedEntries")
					{
						message.AddOutput<ObservableCollection<CurrencyTrackerEntry>>((IMessageHandler)(object)this, ItemFilterExSettings.Instance.ShownEntries, "");
						return (MessageResult)0;
					}
					if (!(message.Id == "luf_getRuntime"))
					{
						return (MessageResult)1;
					}
					message.AddOutput<TimeSpan>((IMessageHandler)(object)this, ItemFilterExSettings.Instance.Runtime.Elapsed, "");
					return (MessageResult)0;
				}
				message.GetInput<int>(0);
				return (MessageResult)0;
			}
			HandleDeath();
			return (MessageResult)0;
		}
		HandleCache(message);
		return (MessageResult)0;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[ItemFilterEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		ItemEvaluator.Instance = (IItemEvaluator)(object)ItemEvaluator.Instance;
		ItemFilterExSettings.Instance.Runtime.Start();
	}

	public void Stop()
	{
		ItemFilterExSettings.Instance.Runtime.Stop();
	}

	public void Tick()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[ItemFilterEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		CharName = ((NetworkObject)LokiPoe.Me).Name;
		CharLevel = ((Player)LokiPoe.Me).Level;
		if (StartExp == 0L)
		{
			StartExp = ((Player)LokiPoe.Me).Experience;
		}
		if (!World.CurrentArea.IsMyHideoutArea || !StashUi.IsOpened)
		{
			return;
		}
		if (!ItemFilterExSettings.Instance.IsCurrencyCachedOnStart)
		{
			DumpCurrencyData();
		}
		if (CachedMaps.Instance.MapCache == null || !CachedMaps.Instance.MapCache.Maps.Any())
		{
			return;
		}
		ItemFilterExSettings.Instance.InfluencedCount = 0;
		ItemFilterExSettings.Instance.WhiteMapAmount = 0;
		ItemFilterExSettings.Instance.YellowMapAmount = 0;
		ItemFilterExSettings.Instance.RedMapAmount = 0;
		List<CachedMapItem> list = CachedMaps.Instance.MapCache.Maps.ToList();
		foreach (CachedMapItem item in list)
		{
			int mapTier = item.MapTier;
			if (item.IsInfluencedMap())
			{
				ItemFilterExSettings.Instance.InfluencedCount++;
			}
			if (mapTier >= 6)
			{
				if (mapTier > 10)
				{
					ItemFilterExSettings.Instance.RedMapAmount++;
				}
				else
				{
					ItemFilterExSettings.Instance.YellowMapAmount++;
				}
			}
			else
			{
				ItemFilterExSettings.Instance.WhiteMapAmount++;
			}
		}
	}

	public void HandleDeath()
	{
		ItemFilterExSettings.Instance.DeathCount++;
	}

	public void HandleCache(Message msg)
	{
		CachedItem cachedItem_0 = msg.GetInput<CachedItem>(0);
		if (!(cachedItem_0.Class == "StackableCurrency"))
		{
			return;
		}
		DumpCurrencyData();
		string string_0 = cachedItem_0.FullName;
		if ((string_0.Contains("Scroll") || string_0.Contains("Scrap") || string_0.Contains("Shard")) && !(string_0 == "Chaos Shard") && !(string_0 == "Harbinger Shard") && !(string_0 == "Annulment Shard") && !(string_0 == "Ancient Shard") && !(string_0 == "Exalted Shard") && !(string_0 == "Mirror Shard"))
		{
			return;
		}
		Application.Current.Dispatcher.Invoke(delegate
		{
			CurrencyTrackerEntry currencyTrackerEntry = ItemFilterExSettings.Instance.ShownEntries.FirstOrDefault((CurrencyTrackerEntry n) => n.CurrencyName == string_0);
			if (currencyTrackerEntry != null)
			{
				currencyTrackerEntry.CurrencyAmount += cachedItem_0.StackCount;
			}
			else
			{
				if (string_0 == "Chaos Orb")
				{
					ItemFilterExSettings.Instance.ShownEntries.Insert(0, new CurrencyTrackerEntry(cachedItem_0.FullName, cachedItem_0.StackCount));
				}
				if (string_0.Contains("Shard"))
				{
					ItemFilterExSettings.Instance.ShownEntries.Add(new CurrencyTrackerEntry(cachedItem_0.FullName, cachedItem_0.StackCount));
				}
				else
				{
					ItemFilterExSettings.Instance.ShownEntries.Add(new CurrencyTrackerEntry(cachedItem_0.FullName, cachedItem_0.StackCount));
				}
			}
		});
	}

	public bool DumpCurrencyData()
	{
		if (StashUi.IsOpened)
		{
			if (!ExtensionsSettings.Instance.GetTabsForCategory("Currency").Any())
			{
				return false;
			}
			if (!(StashUi.TabControl.CurrentTabName != ExtensionsSettings.Instance.GetTabsForCategory("Currency").First()))
			{
				bool flag = ((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).TaskList.Any((ITask t) => ((IAuthored)t).Name.EqualsIgnorecase("currencyRestockTask"));
				int num = ExtensionsSettings.Instance.InventoryCurrencies.Count;
				if (BulkTraderExSettings.Instance.ShouldTrade || !flag)
				{
					num = 0;
				}
				if (Inventories.InventoryItems.Count((Item i) => i.Class == "StackableCurrency") > num)
				{
					return false;
				}
				ItemFilterExSettings.Instance.IsStashCached = true;
				UpdateCurrencyCache();
				ItemFilterExSettings.Instance.CachedCurrency.TryGetValue("Chaos Orb", out var value);
				ItemFilterExSettings.Instance.CachedCurrency.TryGetValue("Divine Orb", out var value2);
				ItemFilterExSettings.Instance.CachedCurrency.TryGetValue("Orb of Alchemy", out var value3);
				ItemFilterExSettings.Instance.DivAmount = value2;
				ItemFilterExSettings.Instance.ChaosAmount = value;
				ItemFilterExSettings.Instance.AlchAmount = value3;
				ItemFilterExSettings.Instance.IsCurrencyCachedOnStart = true;
				return true;
			}
			return false;
		}
		return false;
	}

	private static void UpdateCurrencyCache()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		ItemFilterExSettings.Instance.CachedCurrency.Clear();
		Dictionary<string, int> cachedCurrency = ItemFilterExSettings.Instance.CachedCurrency;
		InventoryTabType tabType = StashUi.StashTabInfo.TabType;
		if ((int)tabType == 3)
		{
			int num = 0;
			{
				foreach (InventoryControlWrapper item in CurrencyTab.All)
				{
					num++;
					if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
					{
						Item customTabItem = item.CustomTabItem;
						if (!((RemoteMemoryObject)(object)customTabItem == (RemoteMemoryObject)null))
						{
							AddToCache(customTabItem, cachedCurrency);
						}
					}
					else
					{
						GlobalLog.Error($"[???] wrapper null! {num}");
					}
				}
				return;
			}
		}
		if ((int)tabType == 8 || (int)tabType == 6 || (int)tabType == 5 || (int)tabType == 9)
		{
			GlobalLog.Error($"[UpdateCurrencyCache] {tabType} is unsupported.");
			return;
		}
		foreach (Item item2 in StashUi.InventoryControl.Inventory.Items)
		{
			AddToCache(item2, cachedCurrency);
		}
	}

	private static void AddToCache(Item item, IDictionary<string, int> dictionary)
	{
		string fullName = item.FullName;
		int stackCount = item.StackCount;
		if (dictionary.ContainsKey(fullName))
		{
			dictionary[fullName] += stackCount;
		}
		else
		{
			dictionary.Add(fullName, stackCount);
		}
	}

	public void HandleSoldEvent(List<CachedItem> items)
	{
		CachedItem cachedItem = items.FirstOrDefault((CachedItem i) => i.Name == "Chaos Orb");
		if (cachedItem != null)
		{
			ItemFilterExSettings.Instance.RecipeCount++;
			GlobalLog.Debug($"[LajtStat] New Recipe detected. In current session: {ItemFilterExSettings.Instance.RecipeCount}");
		}
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		bool flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		bool flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public void Deinitialize()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static ItemFilterEx()
	{
		interval_0 = new Interval(500);
		CharName = "";
		UptimeTimer = Stopwatch.StartNew();
	}
}
