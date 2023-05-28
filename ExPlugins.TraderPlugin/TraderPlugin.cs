using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;
using ExPlugins.TraderPlugin.Classes;
using ExPlugins.TraderPlugin.Tasks;
using RestSharp;

namespace ExPlugins.TraderPlugin;

public class TraderPlugin : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider
{
	public struct LogbookModStruct
	{
		public int num;

		public string mod;

		public int value;

		public string bossValue;

		public LogbookModStruct(int num, string mod, int value)
		{
			this.num = num;
			this.mod = mod;
			this.value = value;
			bossValue = "";
		}

		public LogbookModStruct(int num, string mod, string bossValue)
		{
			this.num = num;
			this.mod = mod;
			value = 0;
			this.bossValue = bossValue;
		}
	}

	public static Stopwatch Stopwatch;

	public static TaskManager BotTaskManager;

	private static readonly Interval interval_0;

	private static readonly Interval interval_1;

	public static ConcurrentQueue<ItemSearchParams> ItemSearch;

	public static ConcurrentQueue<ItemSearchParams> EvaluatedItems;

	public static ConcurrentQueue<SoldItem> SoldItems;

	private static double double_0;

	private static int int_0;

	private static bool bool_0;

	public static string FullFileName;

	private static bool bool_1;

	private static bool bool_2;

	private static readonly char[] char_0;

	public static Stopwatch TradeTimeout;

	private readonly TraderPluginSettings traderPluginSettings_0 = TraderPluginSettings.Instance;

	private readonly List<ItemSearchParams> list_0 = new List<ItemSearchParams>();

	private TraderPluginGui traderPluginGui_0;

	private List<ParserClass.Price> list_1 = new List<ParserClass.Price>();

	private Thread thread_0;

	private Thread thread_1;

	public volatile bool ShouldTelemetryWork;

	public volatile bool ShouldWork;

	public static float? PortalsSpent
	{
		get
		{
			return CombatAreaCache.Current.Storage["PortalsSpent"] as float?;
		}
		set
		{
			CombatAreaCache.Current.Storage["PortalsSpent"] = value;
		}
	}

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "TraderPlugin";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "0.1";

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public UserControl Control => traderPluginGui_0 ?? (traderPluginGui_0 = new TraderPluginGui());

	public void Initialize()
	{
		GlobalLog.Debug("[TraderPlugin] Initialize");
		Class9.EvaluatedItemsInstance.Load();
	}

	public void Deinitialize()
	{
		GlobalLog.Debug("[TraderPlugin] Deinitialize");
		ShouldTelemetryWork = false;
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

	public static TaskManager GetCurrentBotTaskManager()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		IBot current = BotManager.Current;
		Message val = new Message("GetTaskManager", (object)null);
		((IMessageHandler)current).Message(val);
		TaskManager output = val.GetOutput<TaskManager>(0);
		if (output != null)
		{
			return output;
		}
		return null;
	}

	private static void KillProcessAndChildren(int pid)
	{
		if (pid == 0)
		{
			return;
		}
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
		ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
		foreach (ManagementObject item in managementObjectCollection)
		{
			KillProcessAndChildren(Convert.ToInt32(item["ProcessID"]));
		}
		try
		{
			Process processById = Process.GetProcessById(pid);
			processById.Kill();
		}
		catch (ArgumentException)
		{
		}
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[TraderPlugin] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!(((IAuthored)BotManager.Current).Name == "LabRunner"))
		{
			GlobalLog.Debug("[TraderPlugin] Start.");
			BotTaskManager = GetCurrentBotTaskManager();
			AddTasks();
			Stopwatch.Start();
			ParserClass.Starter();
			ShouldWork = true;
			ShouldTelemetryWork = true;
			bool_0 = false;
			double_0 = 0.0;
			Class2.int_0 = 0;
			if (!bool_2)
			{
				thread_0 = new Thread(StartEvaluate)
				{
					IsBackground = true
				};
				thread_0.Start();
				bool_2 = true;
			}
			if (TraderPluginSettings.Instance.TelemetryEnabled && !bool_1)
			{
				thread_1 = new Thread(StartTelemetry)
				{
					IsBackground = true
				};
				thread_1.Start();
				bool_1 = true;
			}
			Class3.SendChatMsg("/clear").Wait();
		}
	}

	private static void AddTasks()
	{
		if (((IAuthored)BotManager.Current).Name == "NullBot")
		{
			GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class4(), "StashTask")) ? "[TraderPlugin] AddAfter StashTask, ListItemsTask Failed" : "[TraderPlugin] AddAfter StashTask, ListItemsTask Sucess");
			GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class3(), "ListItemsTask") ? "[TraderPlugin] AddAfter StashTask, HandleTradeProcessTask Sucess" : "[TraderPlugin] AddAfter StashTask, HandleTradeProcessTask Failed");
			GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class2(), "ListItemsTask")) ? "[TraderPlugin] AddAfter StashTask, CacheTradeTabTask Failed" : "[TraderPlugin] AddAfter StashTask, CacheTradeTabTask Sucess");
			GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class6(), "StashTask") ? "[TraderPlugin] AddAfter StashTask, ScanTabsAndTakeItemsTask Sucess" : "[TraderPlugin] AddAfter StashTask, ScanTabsAndTakeItemsTask Failed");
			GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new Class5(), "ScanTabsAndTakeItemsTask") ? "[TraderPlugin] AddAfter StashTask, RepriceTradeTabTask Sucess" : "[TraderPlugin] AddAfter StashTask, RepriceTradeTabTask Failed");
			return;
		}
		ITask taskByName = ((TaskManagerBase<ITask>)(object)BotTaskManager).GetTaskByName("IdTask");
		if (!((TaskManagerBase<ITask>)(object)BotTaskManager).TaskList.Contains(taskByName))
		{
			((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new IdTask(), "EvaluateItemTask");
		}
		else
		{
			((TaskManagerBase<ITask>)(object)BotTaskManager).Remove("IdTask");
			GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new IdTask(), "LootItemTask")) ? "[TraderPlugin] AddBefore LootItemTask, IdTask Failed" : "[TraderPlugin] AddBefore LootItemTask, IdTask Sucess");
		}
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new EvaluateItemTask(), "IdTask")) ? "[TraderPlugin] AddAfter IdTask, EvaluateItemTask Failed" : "[TraderPlugin] AddAfter IdTask, EvaluateItemTask Sucess");
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class4(), "EvaluateItemTask")) ? "[TraderPlugin] AddAfter EvaluateItemTask, ListItemsTask Failed" : "[TraderPlugin] AddAfter EvaluateItemTask, ListItemsTask Sucess");
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class3(), "ListItemsTask")) ? "[TraderPlugin] AddAfter ListItemsTask, HandleTradeProcessTask Failed" : "[TraderPlugin] AddAfter ListItemsTask, HandleTradeProcessTask Sucess");
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class2(), "ListItemsTask")) ? "[TraderPlugin] AddAfter ListItemsTask, CacheTradeTabTask Failed" : "[TraderPlugin] AddAfter ListItemsTask, CacheTradeTabTask Sucess");
		GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class6(), "StashTask") ? "[TraderPlugin] AddAfter StashTask, ScanTabsAndTakeItemsTask Sucess" : "[TraderPlugin] AddAfter StashTask, ScanTabsAndTakeItemsTask Failed");
		GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new Class5(), "ScanTabsAndTakeItemsTask") ? "[TraderPlugin] AddBefore ScanTabsAndTakeItemsTask, RepriceTradeTabTask Sucess" : "[TraderPlugin] AddBefore ScanTabsAndTakeItemsTask, RepriceTradeTabTask Failed");
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
			GlobalLog.Error("[TraderPlugin] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (interval_1.Elapsed)
		{
			Class9.EvaluatedItemsInstance.Load();
		}
		if (LokiPoe.IsInGame)
		{
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			if (currentArea.IsMap && !PortalsSpent.HasValue)
			{
				PortalsSpent = 0f;
			}
		}
	}

	public void Stop()
	{
		Stopwatch.Reset();
		bool_0 = true;
	}

	public async void StartEvaluate()
	{
		while (ShouldWork)
		{
			Thread.Sleep(50);
			if (bool_0)
			{
				continue;
			}
			ItemSearchParams requestedItem;
			bool successfull = ItemSearch.TryDequeue(out requestedItem);
			if (this.list_0.Contains(requestedItem))
			{
				continue;
			}
			if (successfull)
			{
				Vector2i itemPos = requestedItem.ItemPos;
				Vector2i emptyVector = Vector2i.Zero;
				if (itemPos == emptyVector)
				{
					continue;
				}
				Item item_0 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
				ParserClass.Item4Search item4Search = new ParserClass.Item4Search
				{
					mods = new List<ParserClass.StatFilter>()
				};
				if ((RemoteMemoryObject)(object)item_0 == (RemoteMemoryObject)null || (item_0.Class == "StackableCurrency" && !item_0.Metadata.Contains("Metadata/Items/Currency/CurrencyDelveCrafting") && !item_0.Name.Contains("Delirium Orb") && !item_0.Name.Contains("Oil") && !item_0.Name.Contains("Catalyst") && !item_0.Name.Contains("Blessing") && !item_0.Name.Contains("Essence") && !item_0.Name.Contains("Remnant of Corruption") && item_0.Name != "Ritual Vessel" && !item_0.Name.Contains("Veiled Chaos Orb") && !item_0.Name.Contains("Stacked Deck") && !item_0.Metadata.Contains("ItemisedProphecy") && !item_0.Metadata.Contains("Items/Currency/CurrencyRefresh") && !item_0.Metadata.Contains("Metadata/Items/Currency/CurrencyEldritch") && !item_0.Name.Contains("Oil Extractor") && TraderPluginSettings.Instance.ItemsToList.All((TraderPluginSettings.NameEntry x) => x.Name != item_0.Name)))
				{
					continue;
				}
				if (((int)item_0.Rarity == 2 || (int)item_0.Rarity == 4 || item_0.Name.ContainsIgnorecase("Blueprint:") || item_0.Name.ContainsIgnorecase("Contract:")) && item_0.Class != "Map" && Stopwatch.Elapsed.TotalSeconds - double_0 <= 10.0 && double_0 != 0.0)
				{
					GlobalLog.Debug("[TraderPlugin] Now moving " + item_0.Name + " to the end of the que, only " + (Stopwatch.Elapsed.TotalSeconds - double_0).ToString(CultureInfo.InvariantCulture) + " second passed");
					ItemSearchParams Params = new ItemSearchParams(item_0, item_0.LocationTopLeft);
					ItemSearch.Enqueue(Params);
					continue;
				}
				GlobalLog.Debug("[TraderPlugin] Now going to evalueate " + item_0.Name);
				if (traderPluginSettings_0.ShouldCheckExactEssencePrices && item_0.Name.Contains("Essence"))
				{
					GlobalLog.Error("[TraderPlugin] Starting poe.trade bulk pricing. This will take 10 seconds between each round, you should know what you're doing!");
					List<ParserClass.BulkPrice> list_0 = new List<ParserClass.BulkPrice>();
					Thread t = new Thread((ThreadStart)async delegate
					{
						list_0 = await ParserClass.Bulk(item_0.Name, "Chaos Orb", traderPluginSettings_0.MinBulkAmtToCheckForExactPrice);
					});
					t.Start();
					Thread.Sleep(10000);
					t.Abort();
					double finalPrice = 0.0;
					for (int j = traderPluginSettings_0.CheckPricesFromResultNumber - 1; j < traderPluginSettings_0.CheckPricesToResultNumber; j++)
					{
						if (traderPluginSettings_0.DebugMode)
						{
							GlobalLog.Debug($"[TraderPlugin] Price number {j} = {list_0[j].worth}");
						}
						finalPrice += list_0[j].worth;
					}
					finalPrice /= (double)(traderPluginSettings_0.CheckPricesFromResultNumber - traderPluginSettings_0.CheckPricesToResultNumber + 1);
					GlobalLog.Debug($"[TraderPlugin] Final price is {finalPrice}");
					int minPiecesAmount = 0;
					if (finalPrice % 1.0 != 0.0)
					{
						finalPrice *= 10.0;
						minPiecesAmount += 10;
					}
					for (; finalPrice * (double)minPiecesAmount < (double)traderPluginSettings_0.MinPriceInChaosToTrade; minPiecesAmount++)
					{
						finalPrice += finalPrice;
					}
					foreach (Item it in InventoryUi.InventoryControl_Main.Inventory.Items)
					{
						if (it.Name == item_0.Name)
						{
							if (traderPluginSettings_0.DebugMode)
							{
								GlobalLog.Debug($"[TraderPlugin] Set price for {it.Name} at {it.LocationTopLeft.X}, {it.LocationTopLeft.Y} at {finalPrice}");
							}
							EvaluatedItems.Enqueue(new ItemSearchParams($"~price {(int)finalPrice}/{minPiecesAmount} chaos", it.LocationTopLeft));
						}
					}
					this.list_0.Add(requestedItem);
					continue;
				}
				if ((int)item_0.Rarity == 3 || item_0.Class == "MapFragment" || item_0.Class == "DivinationCard" || item_0.Name.Contains("Blessing") || (int)item_0.Rarity == 4 || item_0.Name.Contains("Oil") || item_0.Name.Contains("Delirium Orb") || item_0.Name.Contains("Catalyst") || item_0.Name == "Ritual Vessel" || item_0.Name.Contains("Remnant of Corruption") || item_0.Name.Contains("Veiled Chaos Orb") || item_0.Metadata.Contains("ItemisedProphecy") || item_0.Name.Contains("Stacked Deck") || item_0.Name.Contains("Essence") || item_0.Class == "ExpeditionLogbook" || item_0.Metadata.Contains("Metadata/Items/Currency/CurrencyDelveCrafting") || item_0.Metadata.Contains("Metadata/Items/Delve/DelveStackableSocketable") || (item_0.Metadata.Contains("Items/MapFragments/Maven") && item_0.Class == "MiscMapItem") || item_0.Metadata.Contains("Items/Currency/CurrencyRefresh") || item_0.Metadata.Contains("Metadata/Items/Currency/CurrencyEldritch") || item_0.Name.Contains("Oil Extractor") || TraderPluginSettings.Instance.ItemsToList.Any((TraderPluginSettings.NameEntry x) => x.Name == item_0.Name))
				{
					double endPriceDouble = PoeNinjaTracker.LookupChaosValue(item_0);
					bool procede = false;
					if (item_0.MaxLinkCount == 6 && endPriceDouble <= PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb") * 2.0)
					{
						GlobalLog.Debug($"[TraderPlugin] 6L price is less than 2 divines, cost: {endPriceDouble}");
						continue;
					}
					if (TraderPluginSettings.Instance.DebugMode)
					{
						GlobalLog.Debug($"[TraderPlugin] Item price: {endPriceDouble}");
					}
					if (endPriceDouble * (double)item_0.StackCount < (double)TraderPluginSettings.Instance.MinPriceInChaosToList || (endPriceDouble < 2.0 && (item_0.Name.Contains("Oil") || item_0.Name.Contains("Essence"))))
					{
						GlobalLog.Debug($"[TraderPlugin] Item price is {endPriceDouble}, too low");
						continue;
					}
					if (item_0.Name == "Incandescent Invitation" || (item_0.Name == "Screaming Invitation" && traderPluginSettings_0.EaterExarchInvitationPriceChaos != 0))
					{
						endPriceDouble = traderPluginSettings_0.EaterExarchInvitationPriceChaos;
					}
					double exprice = PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb");
					decimal totalPrice = default(decimal);
					string currType;
					if (endPriceDouble >= exprice)
					{
						totalPrice = ((!(totalPrice >= 1.1m)) ? Math.Round((decimal)(endPriceDouble / exprice), 1) : Math.Round((decimal)(endPriceDouble / exprice) - 0.1m, 1));
						currType = "divine";
					}
					else
					{
						totalPrice = ((endPriceDouble > 15.0) ? ((int)endPriceDouble - 1) : ((int)endPriceDouble));
						currType = "chaos";
					}
					if (item_0.Name.Contains("Stacked Deck"))
					{
						if (TraderPluginSettings.Instance.DefaultStackedDeckPrice != 0.0)
						{
							totalPrice = (decimal)TraderPluginSettings.Instance.DefaultStackedDeckPrice;
						}
						else
						{
							totalPrice = (decimal)Math.Round(endPriceDouble, 1);
							currType = "chaos";
							GlobalLog.Debug($"[TraderPlugin] It was Stacked Dick, endprice {totalPrice}");
						}
					}
					if (item_0.Name.Contains("Remnant of Corruption"))
					{
						if (TraderPluginSettings.Instance.DefaultRemnantOfCorruptionPrice == 0.0)
						{
							totalPrice = (decimal)Math.Round(endPriceDouble, 1);
							currType = "chaos";
							GlobalLog.Debug($"[TraderPlugin] It was Stacked Dick, endprice {totalPrice}");
						}
						else
						{
							totalPrice = (decimal)TraderPluginSettings.Instance.DefaultRemnantOfCorruptionPrice;
						}
					}
					if (item_0.Name.ContainsIgnorecase("Blueprint:"))
					{
						item4Search.name = item_0.Name;
						procede = true;
					}
					if (!procede)
					{
						EvaluatedItems.Enqueue(new ItemSearchParams(totalPrice, currType, itemPos));
						continue;
					}
				}
				if (item_0.Class == "Map")
				{
					if (!item_0.Stats.ContainsKey((StatTypeGGG)10342) && !item_0.Stats.ContainsKey((StatTypeGGG)14763))
					{
						if (item_0.Stats.ContainsKey((StatTypeGGG)6827))
						{
							switch (item_0.Stats[(StatTypeGGG)6827])
							{
							case 2:
								if (traderPluginSettings_0.ShouldListElderGuardianMaps)
								{
									EvaluatedItems.Enqueue(new ItemSearchParams(traderPluginSettings_0.ElderGuardianMapsPrice, "chaos", itemPos));
								}
								continue;
							case 1:
								if (traderPluginSettings_0.ShouldListShaperGuardianMaps)
								{
									EvaluatedItems.Enqueue(new ItemSearchParams(traderPluginSettings_0.ShaperGuardianMapsPrice, "chaos", itemPos));
								}
								continue;
							}
						}
						if (item_0.Stats.ContainsKey((StatTypeGGG)13845) && traderPluginSettings_0.ShouldListAwakenerGuardianMaps)
						{
							EvaluatedItems.Enqueue(new ItemSearchParams(traderPluginSettings_0.AwakenerGuardianMapsPrice, "chaos", itemPos));
						}
						continue;
					}
					double endPrice = PoeNinjaTracker.LookupChaosValue(item_0);
					GlobalLog.Debug($"[TraderPlugin] Map price: {endPrice}");
					if (endPrice * (double)item_0.StackCount >= (double)TraderPluginSettings.Instance.MinPriceInChaosToList && !(endPrice < 2.0))
					{
						double exprice2 = PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb");
						decimal totalPrice3 = default(decimal);
						string currType2;
						if (endPrice >= exprice2)
						{
							GlobalLog.Debug("[TraderPlugin] Item price is more than 1 ex, gonna list in ex");
							totalPrice3 = ((totalPrice3 >= 1.1m) ? Math.Round((decimal)(endPrice / exprice2) - 0.1m, 1) : Math.Round((decimal)(endPrice / exprice2), 1));
							currType2 = "divine";
						}
						else
						{
							GlobalLog.Debug("[TraderPlugin] Item price is less than 1 ex, gonna list in chaos");
							totalPrice3 = ((endPrice > 15.0) ? ((int)endPrice - 1) : ((int)endPrice));
							currType2 = "chaos";
						}
						EvaluatedItems.Enqueue(new ItemSearchParams(totalPrice3, currType2, itemPos));
					}
					continue;
				}
				if (item_0.Class.Contains("Ring") || item_0.Class.Contains("Amulet") || item_0.Class.Contains("Belt") || item_0.Class.Contains("Jewel") || item_0.Class.Contains("Hand") || item_0.Class.Contains("Bow") || item_0.Class.Contains("Hand") || item_0.Class.Contains("Wand") || item_0.Class.Contains("Helmet") || item_0.Class.Contains("Body Armour") || item_0.Class.Contains("Gloves") || item_0.Class.Contains("Boots") || item_0.Class.Contains("Flask"))
				{
					if (!TraderPluginSettings.Instance.ShouldSellRares)
					{
						continue;
					}
					if (item_0.Name == "Simplex Amulet" || item_0.Name == "Astrolabe Amulet" || item_0.Name == "Seaglass Amulet" || item_0.Name == "Stygian Vise" || item_0.Name == "Vermillion Ring" || item_0.Name == "Cerulean Ring" || item_0.Name == "Opal Ring")
					{
						item4Search.type = item_0.Name;
					}
					item4Search.category = item_0.Class;
					item4Search.category = (item_0.Class.Contains("Jewel") ? "Any Jewel" : item4Search.category);
					item4Search.category = ((!item_0.Class.Contains("Two Handed")) ? item4Search.category : item4Search.category.Replace("Two Handed", "Two-Handed"));
					item4Search.category = ((!item_0.Class.Contains("One Handed")) ? item4Search.category : item4Search.category.Replace("One Handed", "One-Handed"));
					item4Search.links_min = ((item_0.SocketCount > 0 && item_0.Components.SocketsComponent.LargestLinkSize == 6) ? 6 : 0);
					if (item_0.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }))
					{
						item4Search.item_lvl = ((item_0.ItemLevel > 68) ? 68 : 0);
						item4Search.item_lvl = ((item_0.ItemLevel > 75) ? 75 : item4Search.item_lvl);
						item4Search.item_lvl = ((item_0.ItemLevel > 84) ? 84 : item4Search.item_lvl);
					}
					int lifeTotal = 0;
					int manaTotal = 0;
					int manaPercentTotal = 0;
					int physDamageTotal = 0;
					int fireDamageTotal = 0;
					int coldDamageTotal = 0;
					int lightningDamageTotal = 0;
					int energyShieldTotal = 0;
					int energyShieldPercentTotal = 0;
					int eleResTotal = 0;
					int chaosResTotal = 0;
					int intTotal = 0;
					int strTotal = 0;
					int dexTotal = 0;
					int fireDamagePercentTotal = 0;
					int lightningDamagePercentTotal = 0;
					int coldDamagePercentTotal = 0;
					int physDamagePercentTotal = 0;
					int elementalDamagePercentToAttacksTotal = 0;
					int critMultiTotal = 0;
					int critMultiColdTotal = 0;
					int critMultiFireTotal = 0;
					int critMultiLightningTotal = 0;
					int critMultiForSpellsTotal = 0;
					int attackSpeedTotal = 0;
					int castSpeedTotal = 0;
					int damageTotal = 0;
					int damageOverTimeTotal = 0;
					int damageWithBleedingTotal = 0;
					int damageWithPoisonTotal = 0;
					int projectileDamageTotal = 0;
					int spellDamage = 0;
					int chaosDoTMultiTotal = 0;
					int fireDoTMultiTotal = 0;
					int coldDoTMultiTotal = 0;
					int physDoTMultiTotal = 0;
					int chanceToGainOnslaught = 0;
					int flaskChargesGained = 0;
					int flaskChargesUsed = 0;
					foreach (string k in item_0.Components.ModsComponent.ImplicitStrings)
					{
						int[] modNumbers = GetModNumbers(k);
						string mod2 = ModifyModString(k);
						if (item_0.Class == "Ring")
						{
							eleResTotal += (mod2.Contains("cold") ? modNumbers[0] : 0);
							eleResTotal += (mod2.Contains("lightning") ? modNumbers[0] : 0);
							eleResTotal += (mod2.Contains("fire") ? modNumbers[0] : 0);
							eleResTotal += (mod2.Contains("physical damage to attacks") ? ((modNumbers[0] + modNumbers[1]) / 2) : 0);
						}
						intTotal += ((mod2.Contains("intelligence") || mod2.Contains("all attributes")) ? modNumbers[0] : 0);
						dexTotal += ((mod2.Contains("dexterity") || mod2.Contains("all attributes")) ? modNumbers[0] : 0);
						strTotal += ((mod2.Contains("strength") || mod2.Contains("all attributes")) ? modNumbers[0] : 0);
						lifeTotal += (mod2.Contains("to maximum life") ? modNumbers[0] : 0);
						manaTotal += (mod2.Contains("to maximum mana") ? modNumbers[0] : 0);
						energyShieldTotal += (mod2.Contains("maximum energy shield") ? modNumbers[0] : 0);
						chaosResTotal += (mod2.Contains("chaos resistance") ? modNumbers[0] : 0);
						eleResTotal += (mod2.Contains("all elemental resistances") ? (modNumbers[0] * 3) : 0);
					}
					foreach (string l in item_0.Components.ModsComponent.ExplicitStrings)
					{
						int[] modNumbers2 = GetModNumbers(l);
						string mod3 = ModifyModString(l);
						if (item_0.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }))
						{
							if (mod3.Contains("added passive skill is"))
							{
								ParserClass.StatFilter modStruct = default(ParserClass.StatFilter);
								modStruct.max = "0";
								modStruct.min = "0";
								modStruct.type = "explicit";
								modStruct.name = l;
								modStruct.name?.Trim();
								modStruct.value?.Trim();
								item4Search.mods.Add(modStruct);
							}
							continue;
						}
						eleResTotal += ((mod3.Contains("cold resistance") || mod3.Contains("cold and")) ? modNumbers2[0] : 0);
						eleResTotal += ((mod3.Contains("lightning resistance") || mod3.Contains("lightning and")) ? modNumbers2[0] : 0);
						eleResTotal += ((mod3.Contains("fire resistance") || mod3.Contains("fire and")) ? modNumbers2[0] : 0);
						intTotal += ((mod3.Contains("intelligence") || mod3.Contains("all attributes")) ? modNumbers2[0] : 0);
						dexTotal += ((mod3.Contains("dexterity") || mod3.Contains("all attributes")) ? modNumbers2[0] : 0);
						strTotal += ((mod3.Contains("strength") || mod3.Contains("all attributes")) ? modNumbers2[0] : 0);
						lifeTotal += (mod3.Contains("to maximum life") ? modNumbers2[0] : 0);
						physDamageTotal += (mod3.Contains("physical damage to attacks") ? ((modNumbers2[0] + modNumbers2[1]) / 2) : 0);
						fireDamageTotal += (mod3.Contains("fire damage to attacks") ? ((modNumbers2[0] + modNumbers2[1]) / 2) : 0);
						coldDamageTotal += (mod3.Contains("cold damage to attacks") ? ((modNumbers2[0] + modNumbers2[1]) / 2) : 0);
						lightningDamageTotal += (mod3.Contains("lightning damage to attacks") ? ((modNumbers2[0] + modNumbers2[1]) / 2) : 0);
						manaTotal += (mod3.Contains("to maximum mana") ? modNumbers2[0] : 0);
						manaPercentTotal += (mod3.Contains("increased maximum mana") ? modNumbers2[0] : 0);
						energyShieldTotal += (mod3.Contains("to maximum energy shield") ? modNumbers2[0] : 0);
						energyShieldPercentTotal += (mod3.Contains("increased maximum energy shield") ? modNumbers2[0] : 0);
						chaosResTotal += (mod3.Contains("chaos resistance") ? modNumbers2[0] : 0);
						eleResTotal += (mod3.Contains("all elemental resistances") ? (modNumbers2[0] * 3) : 0);
						elementalDamagePercentToAttacksTotal += (mod3.Contains("increased elemental damage") ? modNumbers2[0] : 0);
						coldDamagePercentTotal += (mod3.Contains("increased cold damage") ? modNumbers2[0] : 0);
						fireDamagePercentTotal += (mod3.Contains("increased fire damage") ? modNumbers2[0] : 0);
						lightningDamagePercentTotal += (mod3.Contains("increased lightning damage") ? modNumbers2[0] : 0);
						critMultiTotal += (mod3.Contains("global critical strike multiplier") ? modNumbers2[0] : 0);
						spellDamage += (mod3.Contains("increased spell damage") ? modNumbers2[0] : 0);
						if (item_0.Class == "Belt")
						{
							flaskChargesGained += (mod3.Contains("increased flask charges gained") ? modNumbers2[0] : 0);
							flaskChargesUsed += (mod3.Contains("reduced flask charges used") ? modNumbers2[0] : 0);
						}
						if (item_0.Class == "Jewel")
						{
							physDamagePercentTotal += (mod3.Contains("increased global physical damage") ? modNumbers2[0] : 0);
							critMultiColdTotal += ((mod3.Contains("to critical strike multiplier with cold skills") || mod3.Contains("critical strike multiplier with elemental skills")) ? modNumbers2[0] : 0);
							critMultiFireTotal += ((mod3.Contains("to critical strike multiplier with fire skills") || mod3.Contains("critical strike multiplier with elemental skills")) ? modNumbers2[0] : 0);
							critMultiLightningTotal += ((mod3.Contains("to critical strike multiplier with lightning skills") || mod3.Contains("critical strike multiplier with elemental skills")) ? modNumbers2[0] : 0);
							critMultiForSpellsTotal += (mod3.Contains("critical strike multiplier for spells") ? modNumbers2[0] : 0);
							attackSpeedTotal += ((mod3.Contains("increased attack speed") || mod3.Contains("increased attack and cast speed")) ? modNumbers2[0] : 0);
							castSpeedTotal += ((mod3.Contains("increased cast speed") || mod3.Contains("increased attack and cast speed")) ? modNumbers2[0] : 0);
							damageTotal += ((mod3 == "increased damage") ? modNumbers2[0] : 0);
							damageOverTimeTotal += (mod3.Contains("increased damage over time") ? modNumbers2[0] : 0);
							damageWithBleedingTotal += (mod3.Contains("increased damage with bleeding") ? modNumbers2[0] : 0);
							damageWithPoisonTotal += (mod3.Contains("increased damage with poison") ? modNumbers2[0] : 0);
							projectileDamageTotal += (mod3.Contains("increased projectile damage") ? modNumbers2[0] : 0);
							physDoTMultiTotal += (mod3.Contains("physical damage over time multiplier") ? modNumbers2[0] : 0);
							chaosDoTMultiTotal += (mod3.Contains("chaos damage over time multiplier") ? modNumbers2[0] : 0);
							coldDoTMultiTotal += (mod3.Contains("cold damage over time multiplier") ? modNumbers2[0] : 0);
							fireDoTMultiTotal += (mod3.Contains("fire damage over time multiplier") ? modNumbers2[0] : 0);
							chanceToGainOnslaught += (mod3.Contains("chance to gain onslaught for") ? modNumbers2[0] : 0);
						}
					}
					if (item_0.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }))
					{
						foreach (string m in item_0.Components.ModsComponent.EnchantmentsString)
						{
							ParserClass.StatFilter modStruct2 = default(ParserClass.StatFilter);
							modStruct2.max = "0";
							modStruct2.min = "0";
							modStruct2.type = "enchant";
							int[] modNumbers3 = GetModNumbers(m);
							string mod4 = ModifyModString(m);
							if (!mod4.Contains("# added passive skill is a jewel socket"))
							{
								modStruct2.name = ((!mod4.Contains("added small passive skills grant")) ? mod4 : "added small passive skills grant: #");
								modStruct2.value = ((!mod4.Contains("added small passive skills grant")) ? "" : m.Split(":".ToCharArray())[1].Trim(char_0));
								GlobalLog.Debug("value " + modStruct2.value);
								if (mod4.Contains("adds # passive skills"))
								{
									modStruct2.max = modNumbers3[0].ToString(CultureInfo.InvariantCulture);
								}
								modStruct2.name?.Trim(char_0);
								modStruct2.value?.Trim(char_0);
								item4Search.mods.Add(modStruct2);
							}
						}
					}
					ParserClass.StatFilter ModStruct = default(ParserClass.StatFilter);
					ModStruct.max = "0";
					ModStruct.min = "0";
					if (item_0.Components.BaseComponent.IsElderItem)
					{
						ModStruct.name = "Has Elder Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					if (item_0.Components.BaseComponent.IsShaperItem)
					{
						ModStruct.name = "Has Shaper Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					if (item_0.Components.BaseComponent.IsHunterItem)
					{
						ModStruct.name = "Has Hunter Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					if (item_0.Components.BaseComponent.IsWarlordItem)
					{
						ModStruct.name = "Has Warlord Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					if (item_0.Components.BaseComponent.IsCrusaderItem)
					{
						ModStruct.name = "Has Crusader Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					if (item_0.Components.BaseComponent.IsRedeemerItem)
					{
						ModStruct.name = "Has Redeemer Influence";
						ModStruct.type = "pseudo";
						item4Search.mods.Add(ModStruct);
					}
					lifeTotal += strTotal / 2;
					manaTotal += intTotal / 2;
					energyShieldPercentTotal += intTotal / 5;
					critMultiColdTotal += ((critMultiColdTotal > 0) ? critMultiTotal : 0);
					critMultiFireTotal += ((critMultiFireTotal > 0) ? critMultiTotal : 0);
					critMultiLightningTotal += ((critMultiLightningTotal > 0) ? critMultiTotal : 0);
					if (!(item_0.Class == "Belt"))
					{
						if (lifeTotal >= 50)
						{
							ModStruct.name = "+# total maximum Life";
							ModStruct.min = Math.Round((double)lifeTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (manaTotal >= 50)
						{
							ModStruct.name = "+# total maximum mana";
							ModStruct.min = Math.Round((double)manaTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (manaPercentTotal >= 8)
						{
							ModStruct.name = "#% increased maximum mana";
							ModStruct.min = Math.Round((double)manaPercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if ((physDamageTotal > 7 && item_0.Class == "Ring") || (physDamageTotal > 11 && item_0.Class == "Amulet"))
						{
							ModStruct.name = "adds # to # physical damage to attacks";
							ModStruct.min = Math.Round((double)physDamageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (fireDamageTotal >= 24 && ((elementalDamagePercentToAttacksTotal >= 21 && item_0.Class == "Ring") || (elementalDamagePercentToAttacksTotal >= 31 && item_0.Class == "Amulet")))
						{
							ModStruct.name = "Adds # to # Fire Damage to Attacks";
							ModStruct.min = Math.Round((double)fireDamageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (coldDamageTotal >= 21 && ((elementalDamagePercentToAttacksTotal >= 21 && item_0.Class == "Ring") || (elementalDamagePercentToAttacksTotal >= 31 && item_0.Class == "Amulet")))
						{
							ModStruct.name = "Adds # to # Cold Damage to Attacks";
							ModStruct.min = Math.Round((double)coldDamageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (lightningDamageTotal >= 30 && ((elementalDamagePercentToAttacksTotal >= 21 && item_0.Class == "Ring") || (elementalDamagePercentToAttacksTotal >= 31 && item_0.Class == "Amulet")))
						{
							ModStruct.name = "Adds # to # Lightning Damage to Attacks";
							ModStruct.min = Math.Round((double)lightningDamageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (energyShieldTotal >= 40)
						{
							ModStruct.name = "+# total maximum Energy Shield";
							ModStruct.min = Math.Round((double)energyShieldTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (energyShieldPercentTotal >= 8 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% total increased maximum Energy Shield";
							ModStruct.min = Math.Round((double)energyShieldPercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if ((eleResTotal >= 80 && item_0.Class == "Ring") || (eleResTotal >= 40 && item_0.Class == "Amulet") || (eleResTotal >= 8 && item_0.Class == "Jewel"))
						{
							ModStruct.name = "+#% total Elemental Resistance";
							ModStruct.min = Math.Round((double)eleResTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (chaosResTotal >= 21)
						{
							ModStruct.name = "+#% total to Chaos Resistance";
							ModStruct.min = Math.Round((double)chaosResTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if ((intTotal >= 30 && item_0.Class == "Ring") || (intTotal >= 40 && item_0.Class == "Amulet") || (intTotal >= 1 && item_0.Class == "Jewel"))
						{
							ModStruct.name = "+# total to Intelligence";
							ModStruct.min = Math.Round((double)intTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if ((strTotal >= 30 && item_0.Class == "Ring") || (strTotal >= 40 && item_0.Class == "Amulet") || (strTotal >= 1 && item_0.Class == "Jewel"))
						{
							ModStruct.name = "+# total to Strength";
							ModStruct.min = Math.Round((double)strTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if ((dexTotal >= 30 && item_0.Class == "Ring") || (dexTotal >= 40 && item_0.Class == "Amulet") || (dexTotal >= 1 && item_0.Class == "Jewel"))
						{
							ModStruct.name = "+# total to Dexterity";
							ModStruct.min = Math.Round((double)dexTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (fireDamagePercentTotal >= 8 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Fire Damage";
							ModStruct.min = Math.Round((double)fireDamagePercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (lightningDamagePercentTotal >= 8 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Cold Damage";
							ModStruct.min = Math.Round((double)lightningDamagePercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (coldDamagePercentTotal >= 8 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Lightning Damage";
							ModStruct.min = Math.Round((double)coldDamagePercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (physDamagePercentTotal >= 8 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Physical Damage";
							ModStruct.min = Math.Round((double)physDamagePercentTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if ((elementalDamagePercentToAttacksTotal >= 31 && item_0.Class == "Ring") || (elementalDamagePercentToAttacksTotal >= 37 && item_0.Class == "Amulet"))
						{
							ModStruct.name = "#% increased Elemental Damage with Attack Skills";
							ModStruct.min = Math.Round((double)elementalDamagePercentToAttacksTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if ((critMultiTotal >= 25 && item_0.Class == "Amulet") || (critMultiTotal >= 1 && item_0.Class == "Jewel"))
						{
							ModStruct.name = "+#% Global Critical Strike Multiplier";
							ModStruct.min = Math.Round((double)critMultiTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (critMultiColdTotal >= 1)
						{
							ModStruct.name = "#% to Critical Strike Multiplier with Cold Skills";
							ModStruct.min = Math.Round((double)critMultiColdTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (critMultiFireTotal >= 1)
						{
							ModStruct.name = "#% to Critical Strike Multiplier with Fire Skills";
							ModStruct.min = Math.Round((double)critMultiFireTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (critMultiLightningTotal >= 1)
						{
							ModStruct.name = "#% to Critical Strike Multiplier with Lightning Skills";
							ModStruct.min = Math.Round((double)critMultiLightningTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (critMultiForSpellsTotal >= 1)
						{
							ModStruct.name = "#% to Critical Strike Multiplier for Spells";
							ModStruct.min = Math.Round((double)critMultiForSpellsTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (attackSpeedTotal >= 1 && item_0.Class == "Jewel")
						{
							ModStruct.name = "+#% total Attack Speed";
							ModStruct.min = Math.Round((double)attackSpeedTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (castSpeedTotal >= 1 && item_0.Class == "Jewel")
						{
							ModStruct.name = "+#% total Cast Speed";
							ModStruct.min = Math.Round((double)castSpeedTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (damageTotal >= 1 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Damage";
							ModStruct.min = Math.Round((double)damageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (damageOverTimeTotal >= 1)
						{
							ModStruct.name = "#% increased Damage over Time";
							ModStruct.min = Math.Round((double)damageOverTimeTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (damageWithBleedingTotal >= 1)
						{
							ModStruct.name = "#% increased Damage with Bleeding";
							ModStruct.min = Math.Round((double)damageWithBleedingTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (damageWithPoisonTotal >= 1)
						{
							ModStruct.name = "#% increased Damage with Poison";
							ModStruct.min = Math.Round((double)damageWithPoisonTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (projectileDamageTotal >= 1)
						{
							ModStruct.name = "#% increased Projectile Damage";
							ModStruct.min = Math.Round((double)projectileDamageTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (spellDamage >= 1 && item_0.Class == "Jewel")
						{
							ModStruct.name = "#% increased Spell Damage";
							ModStruct.min = Math.Round((double)spellDamage * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (chaosDoTMultiTotal >= 1)
						{
							ModStruct.name = "#% to Chaos Damage over Time Multiplier";
							ModStruct.min = Math.Round((double)chaosDoTMultiTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (fireDoTMultiTotal >= 1)
						{
							ModStruct.name = "#% to Fire Damage over Time Multiplier";
							ModStruct.min = Math.Round((double)fireDoTMultiTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (coldDoTMultiTotal >= 1)
						{
							ModStruct.name = "#% to Cold Damage over Time Multiplier";
							ModStruct.min = Math.Round((double)coldDoTMultiTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (physDoTMultiTotal >= 1)
						{
							ModStruct.name = "#% to Physical Damage over Time Multiplier";
							ModStruct.min = Math.Round((double)physDoTMultiTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (chanceToGainOnslaught >= 1)
						{
							ModStruct.name = "#% chance to gain Onslaught for 4 seconds on Kill";
							ModStruct.min = Math.Round((double)chanceToGainOnslaught * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
					}
					else
					{
						bool hpFlag = false;
						bool resFlag = false;
						if (lifeTotal >= 80 || (item_0.Name == "Stygian Vise" && lifeTotal >= 60))
						{
							ModStruct.name = "+# total maximum Life";
							ModStruct.min = Math.Round((double)lifeTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							hpFlag = true;
							item4Search.mods.Add(ModStruct);
						}
						if (eleResTotal >= 80 || (item_0.Name == "Stygian Vise" && eleResTotal >= 60))
						{
							ModStruct.name = "+#% total Elemental Resistance";
							ModStruct.min = Math.Round((double)eleResTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							resFlag = true;
							item4Search.mods.Add(ModStruct);
						}
						if (manaTotal >= 50 && hpFlag && resFlag)
						{
							ModStruct.name = "+# total maximum mana";
							ModStruct.min = Math.Round((double)manaTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (strTotal >= 30 && hpFlag && resFlag)
						{
							ModStruct.name = "+# total to Strength";
							ModStruct.min = Math.Round((double)strTotal * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "pseudo";
							item4Search.mods.Add(ModStruct);
						}
						if (flaskChargesUsed >= 10 && hpFlag && resFlag)
						{
							ModStruct.name = "#% reduced Flask Charges used";
							ModStruct.min = Math.Round((double)flaskChargesUsed * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
						if (flaskChargesGained >= 10 && hpFlag && resFlag)
						{
							ModStruct.name = "#% increased Flask Charges gained";
							ModStruct.min = Math.Round((double)flaskChargesGained * TraderPluginSettings.Instance.StatReducer).ToString(CultureInfo.InvariantCulture);
							ModStruct.type = "explicit";
							item4Search.mods.Add(ModStruct);
						}
					}
				}
				if (item4Search.mods.Count() < 2 && item_0.Class != "Map" && (int)item_0.Rarity != 4 && !item_0.Name.ContainsIgnorecase("Blueprint:") && !item_0.Name.ContainsIgnorecase("Contract:"))
				{
					GlobalLog.Debug("[TraderPlugin] " + item_0.FullName + " is garbage, ignoring it");
					continue;
				}
				GlobalLog.Debug("[TraderPlugin] Now searching for: " + item_0.FullName + ", mod_type: " + item4Search.mods_type);
				foreach (ParserClass.StatFilter mod in item4Search.mods)
				{
					GlobalLog.Debug("[TraderPlugin] Target mod: " + mod.name + ", value " + mod.value + ", type " + mod.type + ", min " + mod.min + ", max " + mod.max);
				}
				if ((int)item_0.Rarity == 4)
				{
					GlobalLog.Debug($"[TraderPlugin] Now searching for gem {item4Search.name}, quality {item4Search.quality}, level {item4Search.gem_lvl}, is corrupted {item4Search.corrupted}");
				}
				list_1 = await ParserClass.SearchListedItems(item4Search);
				double_0 = Stopwatch.Elapsed.TotalSeconds;
				int y = 1;
				int z = 1;
				bool flag = false;
				int exPrice = (int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb");
				if (exPrice == -1)
				{
					GlobalLog.Debug("[TraderPlugin] PoeTracker returned -1, now equating ex price to DefaultExPrice");
					exPrice = (int)TraderPluginSettings.Instance.DefaultExPrice;
				}
				else
				{
					GlobalLog.Debug("[TraderPlugin] Current ex price is " + exPrice);
				}
				List<int> endPriceList = new List<int>();
				foreach (ParserClass.Price price in list_1)
				{
					double amount;
					if (price.currency == "divine")
					{
						int_0 += (int)Math.Round(price.amount * (double)exPrice);
						endPriceList.Add((int)Math.Round(price.amount * (double)exPrice));
						object arg = y;
						amount = price.amount;
						GlobalLog.Debug($"[TraderPlugin] Parsed price {arg}: {amount.ToString(CultureInfo.InvariantCulture)}{price.currency}");
					}
					else
					{
						if (!(price.currency == "chaos"))
						{
							GlobalLog.Debug($"[TraderPlugin] Parsed price {z} is not in ex or chaos, ignoring it");
							z++;
							continue;
						}
						int_0 += (int)Math.Round(price.amount);
						endPriceList.Add((int)Math.Round(price.amount));
					}
					object arg2 = y;
					amount = price.amount;
					GlobalLog.Debug($"[TraderPlugin] Parsed price {arg2}: {amount.ToString(CultureInfo.InvariantCulture)} {price.currency}");
					z++;
					if (flag)
					{
						y++;
					}
					flag = true;
				}
				Vector2i[] itemsPos = (Vector2i[])(object)new Vector2i[60];
				string[] itemsName = new string[60];
				int[] prices = new int[60];
				if (!flag)
				{
					int_0 = -1;
				}
				else
				{
					int count = 0;
					if (endPriceList.Count > 2)
					{
						for (int i = 0; i < endPriceList.Count - 2; i++)
						{
							if ((endPriceList[i] < 50 && endPriceList[i] > endPriceList[i + 1] - 5) || (endPriceList[i] > 50 && endPriceList[i] <= 100 && endPriceList[i] > endPriceList[i + 1] - 10) || (endPriceList[i] > 100 && endPriceList[i] > endPriceList[i + 1] - 15))
							{
								count++;
								GlobalLog.Debug($"[TraderPlugin] Count {count}");
							}
							if (count == 3)
							{
								int_0 = ((endPriceList[0] < 15) ? endPriceList[0] : (endPriceList[0] - 1));
								GlobalLog.Debug($"[TraderPlugin] Parsed price 1 {int_0}");
								break;
							}
						}
					}
					else
					{
						GlobalLog.Debug($"[TraderPlugin] Parsed price 2 {int_0}, {y}");
						int_0 = ((int_0 != 0) ? ((int)Math.Round((double)(int_0 / y))) : 0);
					}
					if (count < 3)
					{
						GlobalLog.Debug($"[TraderPlugin] Parsed price 3 {int_0}, {y}");
						int_0 = ((int_0 != 0) ? ((int)Math.Round((double)(int_0 / y))) : 0);
					}
				}
				if (item_0.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }) && (double)int_0 < PoeNinjaTracker.LookupChaosValue(item_0))
				{
					endPriceList.Clear();
					int_0 = (int)PoeNinjaTracker.LookupChaosValue(item_0);
				}
				GlobalLog.Debug($"[TraderPlugin] End price: {int_0} Chaos Orb");
				if (int_0 < TraderPluginSettings.Instance.MinPriceInChaosToTrade)
				{
					if (!(item_0.Class == "ExpeditionLogbook") || int_0 != -1)
					{
						GlobalLog.Debug("[TraderPlugin] Item price is less than required, will not list it");
						continue;
					}
					GlobalLog.Debug("[TraderPlugin] Logbook price returned -1, listing it for default price");
					int_0 = TraderPluginSettings.Instance.DefaultLogbookPrice;
				}
				itemsPos[0] = itemPos;
				itemsName[0] = item_0.Name;
				prices[0] = int_0;
				decimal totalEndPrice2 = default(decimal);
				string currencyType;
				if (int_0 < exPrice)
				{
					GlobalLog.Debug("[TraderPlugin] Item price is less than 1 ex, gonna list in chaos");
					totalEndPrice2 = ((int_0 < 15) ? int_0 : (int_0 - 1));
					currencyType = "chaos";
				}
				else
				{
					GlobalLog.Debug("[TraderPlugin] Item price is more than 1 ex, gonna list in ex");
					totalEndPrice2 = ((!(totalEndPrice2 >= 1.1m)) ? Math.Round((decimal)int_0 / (decimal)exPrice, 1) : Math.Round((decimal)int_0 / (decimal)exPrice - 0.1m, 1));
					currencyType = "divine";
				}
				EvaluatedItems.Enqueue(new ItemSearchParams(totalEndPrice2, currencyType, itemPos));
				requestedItem = null;
			}
			else
			{
				this.list_0.Clear();
			}
		}
	}

	public void StartTelemetry()
	{
		RestClient restClient = null;
		bool flag = true;
		while (ShouldTelemetryWork)
		{
			Thread.Sleep(1000);
			if (string.IsNullOrEmpty(TraderPluginSettings.Instance.ApiEndpoint))
			{
				continue;
			}
			try
			{
				if (flag)
				{
					restClient = new RestClient(TraderPluginSettings.Instance.ApiEndpoint);
					restClient.RemoteCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
					RestRequest request = new RestRequest("api/", Method.GET);
					IRestResponse restResponse = restClient.Execute(request);
					GlobalLog.Info("[TraderPlugin][StartTelemetry] init response from " + TraderPluginSettings.Instance.ApiEndpoint + ": " + restResponse.Content);
					flag = false;
				}
				if (SoldItems.TryDequeue(out var result))
				{
					RestRequest restRequest = new RestRequest("api/", Method.POST);
					restRequest.AddJsonBody(result);
					IRestResponse restResponse2 = restClient.Execute(restRequest);
					GlobalLog.Info($"[TraderPlugin][StartTelemetry] Pos data result: {restResponse2.StatusCode}");
				}
			}
			catch (Exception arg)
			{
				GlobalLog.Error($"[TraderPlugin][StartTelemetry] {arg}");
			}
		}
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	private int[] GetModNumbers(string mod)
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

	private string ModifyModString(string mod)
	{
		mod = Regex.Replace(mod, "[0-9]{1,}", "#");
		mod = mod.Replace("+", "");
		mod = mod.Replace("-", "");
		mod = mod.ToLower();
		return mod;
	}

	static TraderPlugin()
	{
		Stopwatch = Stopwatch.StartNew();
		interval_0 = new Interval(500);
		interval_1 = new Interval(60000);
		ItemSearch = new ConcurrentQueue<ItemSearchParams>();
		EvaluatedItems = new ConcurrentQueue<ItemSearchParams>();
		SoldItems = new ConcurrentQueue<SoldItem>();
		FullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "State", "CurrentStashPrices.json");
		char_0 = new char[1] { ' ' };
		TradeTimeout = Stopwatch.StartNew();
	}
}
