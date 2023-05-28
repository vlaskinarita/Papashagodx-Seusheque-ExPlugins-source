using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;

internal class Class5 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private int int_0;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "RepriceTradeTabTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (TraderPluginSettings.Instance.RepriceTradeTab)
		{
			if (!(TraderPlugin.Stopwatch.Elapsed.TotalSeconds - (double)int_0 >= (double)TraderPluginSettings.Instance.SecondsBetweenScan) && int_0 != 0)
			{
				return false;
			}
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsTown && !area.IsHideoutArea)
			{
				return false;
			}
			if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
			{
				await Inventories.OpenStash();
			}
			await Inventories.OpenStashTab(TraderPluginSettings.Instance.StashTabToTrade, Name);
			GlobalLog.Debug("[RepriceTradeTabTask] Path: " + TraderPlugin.FullFileName);
			List<Vector2i> expiredRares = new List<Vector2i>();
			foreach (Item item_0 in StashUi.InventoryControl.Inventory.Items)
			{
				if (TraderPluginSettings.Instance.ItemsToIgnore.Any((TraderPluginSettings.NameEntry x) => x.Name == item_0.Name) || (item_0.MaxStackCount > 1 && StashUi.InventoryControl.Inventory.Items.Any((Item i) => i.FullName == item_0.FullName && !string.IsNullOrEmpty(i.DisplayNote))))
				{
					continue;
				}
				if (TraderPluginSettings.Instance.DebugMode)
				{
					string[] obj = new string[6] { "[RepriceTradeTabTask] Item parsed: ", item_0.Name, "  ", item_0.FullName, ", item position: ", null };
					Vector2i locationTopLeft = item_0.LocationTopLeft;
					obj[5] = ((object)(Vector2i)(ref locationTopLeft)).ToString();
					GlobalLog.Debug(string.Concat(obj));
				}
				List<Class9.Class10> mnePohui = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
				if ((int)item_0.Rarity == 2 && mnePohui != null && mnePohui.Any())
				{
					Class9.Class10 entry = mnePohui.FirstOrDefault((Class9.Class10 e) => e.ItemName == item_0.FullName && e.ItemPos == item_0.LocationTopLeft);
					if ((entry?.ListDate.HasValue ?? false) && DateTime.Now > entry.ListDate.Value.AddDays(3.0))
					{
						await Inventories.FastMoveFromStashTab(item_0.LocationTopLeft);
						Item invItem = InventoryUi.InventoryControl_Main.Inventory.FindItemByFullName(entry.ItemName);
						if (!((RemoteMemoryObject)(object)invItem == (RemoteMemoryObject)null))
						{
							expiredRares.Add(invItem.LocationTopLeft);
							continue;
						}
						GlobalLog.Error("[" + Name + ":VendorExpiredRares] Can't find " + entry.ItemName + " in inventory! Very weird.");
					}
				}
				if (((item_0.Class.Contains("Ring") || item_0.Class.Contains("Amulet") || item_0.Class.Contains("Belt") || item_0.Class.Contains("Hand") || item_0.Class.Contains("Bow") || item_0.Class.Contains("Hand") || item_0.Class.Contains("Wand") || item_0.Class.Contains("Helmet") || item_0.Class.Contains("Body Armour") || item_0.Class.Contains("Gloves") || item_0.Class.Contains("Boots") || item_0.Class.Contains("Flask")) && (int)item_0.Rarity != 3) || (item_0.Class == "Map" && !item_0.Stats.ContainsKey((StatTypeGGG)6827) && !item_0.Stats.ContainsKey((StatTypeGGG)13845) && !item_0.Stats.ContainsKey((StatTypeGGG)10342) && !item_0.Stats.ContainsKey((StatTypeGGG)14763) && (int)item_0.Rarity != 3))
				{
					continue;
				}
				string note = item_0.DisplayNote.Replace(",", ".");
				double price = PoeNinjaTracker.LookupChaosValue(item_0);
				if (TraderPluginSettings.Instance.DebugMode)
				{
					GlobalLog.Debug($"[RepriceTradeTabTask] Price is {price}");
				}
				if (item_0.Name == "Incandescent Invitation" || (item_0.Name == "Screaming Invitation" && TraderPluginSettings.Instance.EaterExarchInvitationPriceChaos != 0))
				{
					price = TraderPluginSettings.Instance.EaterExarchInvitationPriceChaos;
				}
				decimal endPrice;
				string currency;
				if (price >= (double)(int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb"))
				{
					if (TraderPluginSettings.Instance.DebugMode)
					{
						GlobalLog.Debug("[RepriceTradeTabTask] Item price is more than 1 div, gonna list in div");
					}
					endPrice = ((price >= 1.1) ? Math.Round((decimal)price / (decimal)(int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb") - 0.1m, 1) : Math.Round((decimal)price / (decimal)(int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb"), 1));
					currency = "divine";
				}
				else
				{
					if (TraderPluginSettings.Instance.DebugMode)
					{
						GlobalLog.Debug("[RepriceTradeTabTask] Item price is less than 1 div, gonna list in chaos");
					}
					currency = "chaos";
					endPrice = ((price > 15.0) ? ((int)price - 1) : ((int)price));
				}
				if (item_0.Stats.ContainsKey((StatTypeGGG)6827))
				{
					switch (item_0.Stats[(StatTypeGGG)6827])
					{
					case 2:
						endPrice = TraderPluginSettings.Instance.ElderGuardianMapsPrice;
						currency = "chaos";
						break;
					case 1:
						endPrice = TraderPluginSettings.Instance.ShaperGuardianMapsPrice;
						currency = "chaos";
						break;
					}
				}
				if (item_0.Stats.ContainsKey((StatTypeGGG)13845))
				{
					endPrice = TraderPluginSettings.Instance.AwakenerGuardianMapsPrice;
					currency = "chaos";
				}
				if (item_0.Name.Contains("Stacked Deck"))
				{
					if (TraderPluginSettings.Instance.DefaultStackedDeckPrice != 0.0)
					{
						endPrice = (decimal)TraderPluginSettings.Instance.DefaultStackedDeckPrice;
						GlobalLog.Debug($"[TraderPlugin] It was Stacked Dick, endprice {endPrice}");
					}
					else
					{
						endPrice = (decimal)Math.Round(price, 1);
						currency = "chaos";
					}
				}
				if (item_0.Name.Contains("Remnant of Corruption"))
				{
					if (TraderPluginSettings.Instance.DefaultRemnantOfCorruptionPrice == 0.0)
					{
						endPrice = (decimal)Math.Round(price, 1);
						currency = "chaos";
					}
					else
					{
						endPrice = (decimal)TraderPluginSettings.Instance.DefaultRemnantOfCorruptionPrice;
					}
				}
				if (!(endPrice < 1m))
				{
					string endNote2 = "~price " + Convert.ToString(endPrice, CultureInfo.InvariantCulture) + " " + currency;
					endNote2 = endNote2.Replace(",", ".");
					if (item_0.Name.Contains("Stacked Deck") && !string.IsNullOrEmpty(TraderPluginSettings.Instance.StackedDeckExactNote))
					{
						endNote2 = TraderPluginSettings.Instance.StackedDeckExactNote;
					}
					if (!(note == endNote2))
					{
						GlobalLog.Debug(string.Format("[RepriceTradeTabTask] Relisting {0} for {1} {2}", item_0.Name + " " + item_0.FullName, endPrice, currency));
						StashUi.InventoryControl.OpenDisplayNote(item_0.LocalId, true);
						await Coroutines.LatencyWait();
						DisplayNoteUi.Note = endNote2;
						DisplayNoteUi.Accept();
						await Coroutines.LatencyWait();
					}
				}
				else
				{
					GlobalLog.Debug("[RepriceTradeTabTask] EndPrice is less than 1, skipping repricing");
				}
			}
			GlobalLog.Debug("[RepriceTradeTabTask] Trade tab is repriced");
			int_0 = (int)TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
			bool flag = expiredRares.Any();
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await TownNpcs.SellItems(expiredRares);
			}
			return flag2;
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
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}
}
