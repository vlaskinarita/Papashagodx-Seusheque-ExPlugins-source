using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class IdTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	public string Name => "IdTask";

	public string Description => "Task that handles item identification.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (ExtensionsSettings.Instance.UnpackStackedDeck)
		{
			Item deck = Enumerable.FirstOrDefault(Inventories.InventoryItems, (Item i) => i.Name.Equals("Stacked Deck"));
			if ((RemoteMemoryObject)(object)deck != (RemoteMemoryObject)null && (Inventories.AvailableInventorySquares >= 1 || deck.StackCount == 1))
			{
				if (!InventoryUi.IsOpened)
				{
					await Inventories.OpenInventory();
				}
				GlobalLog.Debug($"Stacked deck unpack result: {await InventoryUi.InventoryControl_Main.PickItemToCursor(deck.LocationTopLeft, rightClick: true)}");
				await DropItem();
				return true;
			}
		}
		if (!area.IsHideoutArea && !area.Id.Contains("Affliction") && !area.IsTown)
		{
			return false;
		}
		List<Vector2i> itemsToId = new List<Vector2i>();
		IItemEvaluator itemFilter = ItemEvaluator.Instance;
		foreach (Item item in Inventories.InventoryItems)
		{
			if (!itemFilter.Match(item, (EvaluationType)6))
			{
				bool corruptedNonSentinel = !item.Metadata.Contains("Metadata/Items/Sentinel/Sentinel") && item.IsCorrupted;
				if (!(item.IsIdentified || corruptedNonSentinel) && !item.IsMirrored && itemFilter.Match(item, (EvaluationType)5))
				{
					itemsToId.Add(item.LocationTopLeft);
				}
			}
		}
		if (itemsToId.Count != 0)
		{
			GlobalLog.Info($"[IdTask] {itemsToId.Count} items to id.");
			int scrollsAmount = InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)0).ItemAmount(CurrencyNames.Wisdom);
			if (scrollsAmount != 0 || Inventories.AvailableInventorySquares != 0)
			{
				GlobalLog.Info($"[IdTask] {scrollsAmount} id scrolls.");
				if (scrollsAmount < itemsToId.Count)
				{
					GlobalLog.Warn("[IdTask] Not enough id scrolls to identify all items. Now going to take them from stash.");
					switch (await Inventories.WithdrawCurrency(CurrencyNames.Wisdom))
					{
					case WithdrawResult.Error:
						ErrorManager.ReportError();
						return true;
					case WithdrawResult.Unavailable:
						if (Enumerable.Any(PluginManager.EnabledPlugins, (IPlugin x) => ((IAuthored)x).Name == "EquipPluginPro"))
						{
							GlobalLog.Error("[IdTask] There are no id scrolls in all tabs assigned to them. EquipPluginPro is enabled, we will sell the items becouse we are probably leveling.");
							if (!(await TownNpcs.SellItems(itemsToId)))
							{
								ErrorManager.ReportError();
							}
							return false;
						}
						GlobalLog.Error("[IdTask] There are no id scrolls in all tabs assigned to them. Now stopping the bot because it cannot continue.");
						BotManager.Stop(new StopReasonData("no_id_scrolls_in_stash", "There are no id scrolls in all tabs assigned to them.", (object)null), false);
						return true;
					}
				}
				if (await Inventories.OpenInventory())
				{
					foreach (Vector2i pos in itemsToId)
					{
						if (await Identify(pos))
						{
							continue;
						}
						return true;
					}
					if (!area.IsTown && !area.IsHideoutArea)
					{
						await Coroutines.CloseBlockingWindows();
					}
					await DropTrash(area);
					return true;
				}
				ErrorManager.ReportError();
				return true;
			}
			GlobalLog.Error("[IdTask] No id scrolls and no free space in inventory. Now stopping the bot because it cannot continue.");
			BotManager.Stop(new StopReasonData("no_id_scrolls", "No id scrolls and no free space in inventory.", (object)null), false);
			return true;
		}
		await DropTrash(area);
		return false;
	}

	private static async Task DropTrash(DatWorldAreaWrapper area)
	{
		if (!area.Id.Contains("Affliction") || Enumerable.FirstOrDefault(PluginManager.EnabledPlugins, (IPlugin n) => ((IAuthored)n).Name == "ItemFilterEx") == null)
		{
			return;
		}
		Item[] items = Enumerable.Where(Inventories.InventoryItems, (Item i) => (int)i.Rarity == 3).ToArray();
		if (!items.Any())
		{
			return;
		}
		Item[] array = items;
		foreach (Item itm in array)
		{
			if (!ItemEvaluator.Instance.Match(itm, (EvaluationType)3))
			{
				GlobalLog.Debug("[DropTrash] Saving " + itm.FullName);
				continue;
			}
			GlobalLog.Warn("[DropTrash] Dropping " + itm.FullName);
			await DropItem(itm);
		}
	}

	public static async Task<bool> Identify(Vector2i itemPos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			List<Item> inv = Inventories.InventoryItems;
			Item scroll = Enumerable.FirstOrDefault(inv.OrderBy((Item s) => s.StackCount), (Item s) => s.Name == CurrencyNames.Wisdom);
			if ((RemoteMemoryObject)(object)scroll == (RemoteMemoryObject)null)
			{
				GlobalLog.Error("[Identify] No id scrolls.");
				return false;
			}
			string name = item.Name;
			GlobalLog.Debug("[Identify] Now using id scroll on \"" + name + "\".");
			if (!(await InventoryUi.InventoryControl_Main.PickItemToCursor(scroll.LocationTopLeft, rightClick: true)))
			{
				return false;
			}
			if (!(await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(itemPos)))
			{
				return false;
			}
			if (await Wait.For(() => IsIdentified(itemPos), "item identification"))
			{
				GlobalLog.Debug("[Identify] \"" + name + "\" has been successfully identified.");
				Utility.BroadcastMessage((object)null, "ItemIdentified", new object[1] { itemPos });
				return true;
			}
			return false;
		}
		GlobalLog.Error($"[Identify] Fail to find item at {itemPos} in player's inventory.");
		return false;
	}

	public static async Task DropItem(Item item = null)
	{
		if (((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null) || LokiPoe.Me.IsInTown || LokiPoe.Me.IsInHideout)
		{
			return;
		}
		string string_0 = "itemName";
		int rand = LokiPoe.Random.Next(-10, 10);
		if ((RemoteMemoryObject)(object)item != (RemoteMemoryObject)null)
		{
			await Inventories.OpenInventory();
			string_0 = item.FullName;
			await InventoryUi.InventoryControl_Main.PickItemToCursor(item.LocationTopLeft);
			await Inventories.WaitForCursorToHaveItem();
		}
		MouseManager.SetMousePos((string)null, new Vector2i(((NetworkObject)LokiPoe.Me).Position.X + rand, ((NetworkObject)LokiPoe.Me).Position.Y + rand), true);
		await Wait.SleepSafe(15);
		MouseManager.ClickLMB(((NetworkObject)LokiPoe.Me).Position.X + rand, ((NetworkObject)LokiPoe.Me).Position.Y + rand);
		await Wait.SleepSafe(15);
		await Inventories.WaitForCursorToBeEmpty();
		if ((RemoteMemoryObject)(object)item != (RemoteMemoryObject)null)
		{
			await Wait.SleepSafe(80, 100);
			CombatAreaCache.Tick();
			foreach (WorldItem KfDzeuffEi in Enumerable.Where(ObjectManager.GetObjectsByType<WorldItem>(), (WorldItem i) => i.Item.FullName.Equals(string_0)))
			{
				CachedWorldItem groundCached = Enumerable.FirstOrDefault(CombatAreaCache.Current.Items, (CachedWorldItem i) => i.Id == ((NetworkObject)KfDzeuffEi).Id);
				if (groundCached != null)
				{
					groundCached.Ignored = true;
				}
			}
		}
		if (interval_0.Elapsed)
		{
			Vector2i myPos = LokiPoe.MyPosition;
			WalkablePosition walkablePos = new WalkablePosition("random pos", new Vector2i(myPos.X + rand, myPos.Y + rand), 5);
			walkablePos.TryCome();
		}
	}

	private static bool IsIdentified(Vector2i pos)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Item val = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(pos);
		if ((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[Identify] Unexpected error. Item became null while waiting for identification.");
			ErrorManager.ReportError();
			return true;
		}
		return val.IsIdentified;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static IdTask()
	{
		interval_0 = new Interval(3000);
	}
}
