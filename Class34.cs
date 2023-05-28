using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.ItemFilterEx;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Helpers;

internal class Class34 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static Func<Npc, bool> func_0;

		public static ShouldViewItemDelegate shouldViewItemDelegate_0;

		public static Func<bool> func_1;

		public static Func<Item, bool> func_2;

		public static Func<Item, string> func_3;

		public static Func<IGrouping<string, Item>, Item> func_4;

		public static Func<Item, int> func_5;

		public static Func<Item, int> func_6;

		public static Func<Item, bool> func_7;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003Cget_Kirac_003Eb__6_0(Npc n)
		{
			return ((NetworkObject)n).Name == "Commander Kirac" && ((NetworkObject)n).IsTargetable;
		}

		internal bool _003CBuyMaps_003Eb__10_0(Inventory inv, Item item)
		{
			return MapExtensions.IsMap(item);
		}

		internal bool _003CBuyMaps_003Eb__10_1()
		{
			return PurchaseUi.IsOpened;
		}

		internal bool _003CBuyMaps_003Eb__10_2(Item i)
		{
			_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass10_0
			{
				item_0 = i
			};
			return MapExtensions.IsMap(CS_0024_003C_003E8__locals0.item_0) && !hashSet_0.Any((string s) => s.Equals(CS_0024_003C_003E8__locals0.item_0.Name)) && !CS_0024_003C_003E8__locals0.item_0.Completed() && !CS_0024_003C_003E8__locals0.item_0.Ignored() && CS_0024_003C_003E8__locals0.item_0.BelowTierLimit();
		}

		internal string _003CBuyMaps_003Eb__10_3(Item m)
		{
			return m.Name;
		}

		internal Item _003CBuyMaps_003Eb__10_4(IGrouping<string, Item> g)
		{
			return g.First();
		}

		internal int _003CBuyMaps_003Eb__10_5(Item m)
		{
			return m.Priority();
		}

		internal int _003CBuyMaps_003Eb__10_6(Item m)
		{
			return m.MapTier;
		}

		internal bool _003CBuyMaps_003Eb__10_7(Item i)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			return i.IsInfluencedMap() || ((int)i.Rarity == 3 && PoeNinjaTracker.LookupChaosValue(i) > 20.0);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Item item_0;

		internal bool _003CBuyMaps_003Eb__8(string s)
		{
			return s.Equals(item_0.Name);
		}
	}

	private static readonly Interval interval_0;

	public static HashSet<string> hashSet_0;

	private int int_0 = 9000;

	private int int_1;

	private bool bool_0;

	private static Npc Kirac => ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Commander Kirac" && ((NetworkObject)n).IsTargetable);

	public string Name => "BuyMapsFromKiracTask";

	public string Author => "Seusheque";

	public string Description => "Task for buying maps from Kirac after missions.";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!LokiPoe.IsInGame)
		{
			return false;
		}
		if (!area.IsMyHideoutArea)
		{
			return false;
		}
		if (!((NetworkObject)(object)Kirac == (NetworkObject)null))
		{
			if (!GeneralSettings.Instance.BuyMapsFromKirac)
			{
				return false;
			}
			if (int_0 == 9000)
			{
				Recache();
				GlobalLog.Debug("[BuyMapsFromKiracTask] Cache read success.");
				return true;
			}
			if (int_1 == int_0 || bool_0)
			{
				if (!GeneralSettings.Instance.KiracChecked || bool_0)
				{
					if (await BuyMaps())
					{
						GeneralSettings.Instance.KiracChecked = true;
					}
					return true;
				}
				return false;
			}
			bool_0 = true;
			return true;
		}
		return false;
	}

	public void Tick()
	{
		if (interval_0.Elapsed)
		{
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			if (currentArea.IsMyHideoutArea)
			{
				int_1 = Atlas.KiracNormalMissionsLeft + Atlas.KiracYellowMissionsLeft + Atlas.KiracRedMissionsLeft;
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "MB_kirac_mission_opened_event")
		{
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private async Task<bool> BuyMaps()
	{
		int retry = 0;
		while (!(await ((NetworkObject)(object)Kirac).AsTownNpc().OpenDefaultCtrlPurchase()))
		{
			retry++;
			if (retry > 3)
			{
				GlobalLog.Error("[BuyMapsFromKiracTask] Can't open purchase pannel after 3 tries.");
				return false;
			}
			await Coroutines.LatencyWait();
			await Coroutines.CloseBlockingWindows();
		}
		InventoryControlWrapper inventoryControl = PurchaseUi.InventoryControl;
		object obj = _003C_003Ec.shouldViewItemDelegate_0;
		if (obj == null)
		{
			ShouldViewItemDelegate val = (Inventory inv, Item item) => MapExtensions.IsMap(item);
			obj = (object)val;
			_003C_003Ec.shouldViewItemDelegate_0 = val;
		}
		inventoryControl.ViewItemsInInventory((ShouldViewItemDelegate)obj, (Func<bool>)(() => PurchaseUi.IsOpened));
		await Wait.SleepSafe(500);
		IEnumerable<Item> maps = PurchaseUi.InventoryControl.Inventory.Items.Where((Item i) => MapExtensions.IsMap(i) && !hashSet_0.Any((string s) => s.Equals(i.Name)) && !i.Completed() && !i.Ignored() && i.BelowTierLimit());
		List<Item> orderedMaps = (from m in (from m in maps
				group m by m.Name into g
				select g.First()).ToList()
			orderby m.Priority() descending, m.MapTier descending
			select m).ToList();
		List<Item> expensiveMaps = PurchaseUi.InventoryControl.Inventory.Items.Where((Item i) => i.IsInfluencedMap() || ((int)i.Rarity == 3 && PoeNinjaTracker.LookupChaosValue(i) > 20.0)).ToList();
		if (expensiveMaps.Any())
		{
			orderedMaps.AddRange(expensiveMaps);
		}
		if (!GeneralSettings.Instance.AtlasExplorationEnabled)
		{
			orderedMaps = expensiveMaps;
		}
		foreach (Item map2 in PurchaseUi.InventoryControl.Inventory.Items.Where(MapExtensions.IsMap))
		{
			GlobalLog.Debug("=====================");
			GlobalLog.Debug("[BuyMapsFromKiracTask] Name: " + map2.CleanName());
			if (map2.Completed())
			{
				GlobalLog.Debug($"[BuyMapsFromKiracTask] Completed? {map2.Completed()}");
			}
			else
			{
				GlobalLog.Warn($"[BuyMapsFromKiracTask] Completed? {map2.Completed()}");
			}
			GlobalLog.Debug($"[BuyMapsFromKiracTask] Ignored? {map2.Ignored()}");
			GlobalLog.Debug($"[BuyMapsFromKiracTask] Prio: {map2.Priority()}");
			GlobalLog.Debug($"[BuyMapsFromKiracTask] Tier: {map2.MapTier}");
			if ((int)map2.Rarity == 3)
			{
				GlobalLog.Debug($"[BuyMapsFromKiracTask] PoeNinja estimated value: {PoeNinjaTracker.LookupChaosValue(map2)}");
			}
			GlobalLog.Debug("=====================");
		}
		if (!orderedMaps.Any())
		{
			GlobalLog.Debug("[BuyMapsFromKiracTask] Kirac has no maps of interest.");
			Recache();
			bool_0 = false;
			return true;
		}
		List<KeyValuePair<string, int>> itemCost = default(List<KeyValuePair<string, int>>);
		bool canAfford = default(bool);
		foreach (Item map in orderedMaps)
		{
			PurchaseUi.InventoryControl.GetItemCostEx(map.LocalId, ref itemCost, ref canAfford);
			GlobalLog.Info($"{map.FullName} rarity: {map.Rarity} completed: {map.Completed()} -> canAfford:{canAfford} Price: {itemCost.FirstOrDefault()}");
			if (canAfford)
			{
				GlobalLog.Debug("[BuyMapsFromKiracTask] Now buying " + map.Name);
				FastMoveResult moveResult = PurchaseUi.InventoryControl.FastMove(map.LocalId, true, true);
				if ((int)moveResult == 0)
				{
					await Coroutines.ReactionWait();
					await Coroutines.LatencyWait();
				}
			}
			itemCost = null;
		}
		Recache();
		bool_0 = false;
		return true;
	}

	private void Recache()
	{
		if (File.Exists("State/" + ((NetworkObject)LokiPoe.Me).Name + "-kiracCache.txt"))
		{
			int_0 = short.Parse(File.ReadAllText("State/" + ((NetworkObject)LokiPoe.Me).Name + "-kiracCache.txt"));
			SaveToFile(int_1.ToString());
		}
		else
		{
			SaveToFile(int_1.ToString());
		}
	}

	public static void SaveToFile(string text = "")
	{
		try
		{
			File.WriteAllText("State/" + ((NetworkObject)LokiPoe.Me).Name + "-kiracCache.txt", text);
		}
		catch (Exception ex)
		{
			GlobalLog.Error("[BuyMapsFromKiracTask] (State/" + ((NetworkObject)LokiPoe.Me).Name + "-kiracCache.txt) " + ex.Message);
		}
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static Class34()
	{
		interval_0 = new Interval(500);
		hashSet_0 = new HashSet<string>();
	}
}
