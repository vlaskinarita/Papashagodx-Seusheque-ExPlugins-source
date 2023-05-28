using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class SortInventoryTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class44
	{
		public readonly Vector2i vector2i_0;

		public readonly string Name;

		public readonly Vector2i vector2i_1;

		public Class44(string name, Vector2i from, Vector2i to)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Name = name;
			vector2i_0 = from;
			vector2i_1 = to;
		}
	}

	[CompilerGenerated]
	private bool bool_0;

	public bool ShouldSort
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		set
		{
			bool_0 = value;
		}
	}

	public string Name => "SortInventoryTask";

	public string Author => "Alcor75";

	public string Description => "Task for organizing items in inventory";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!ShouldSort || (!area.IsTown && !area.IsHideoutArea && !area.Id.Contains("Affliction")))
		{
			return false;
		}
		if (!(Inventories.Stash == (NetworkObject)null))
		{
			ShouldSort = false;
			if (Inventories.AvailableInventorySquares == 0)
			{
				GlobalLog.Error("[SortInventoryTask] Unexpected state. Main inventory is full. Nothing will be sorted.");
				return false;
			}
			while (true)
			{
				Item cursorItem = CursorItemOverlay.Item;
				if ((RemoteMemoryObject)(object)cursorItem != (RemoteMemoryObject)null)
				{
					Position pos = GetMovePosForCursorItem(cursorItem);
					GlobalLog.Debug($"[SortInventoryTask] Now moving \"{cursorItem.Name}\" from cursor to {pos}");
					if (await MoveCursorItem(pos))
					{
						continue;
					}
					ErrorManager.ReportError();
					return true;
				}
				Class44 item = GetItemToMove();
				if (item != null)
				{
					GlobalLog.Debug($"[SortInventoryTask] Now moving \"{item.Name}\" from {item.vector2i_0} to {item.vector2i_1}");
					if (!(await MoveInventoryItem(item.vector2i_0, item.vector2i_1)))
					{
						break;
					}
					continue;
				}
				return false;
			}
			ErrorManager.ReportError();
			return true;
		}
		return false;
	}

	private static Class44 GetItemToMove()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		foreach (Item inventoryItem in Inventories.InventoryItems)
		{
			Position movePosForInventoryItem = GetMovePosForInventoryItem(inventoryItem);
			if (movePosForInventoryItem != null)
			{
				return new Class44(inventoryItem.Name, inventoryItem.LocationTopLeft, movePosForInventoryItem);
			}
		}
		return null;
	}

	private static Position GetMovePosForInventoryItem(Item item)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		string string_0 = item.Name;
		ExtensionsSettings.InventoryCurrency inventoryCurrency = ExtensionsSettings.Instance.InventoryCurrencies.FirstOrDefault((ExtensionsSettings.InventoryCurrency i) => i.Name == string_0);
		if (inventoryCurrency != null && HasFixedPosition(inventoryCurrency))
		{
			Position position = new Position(inventoryCurrency.Column - 1, inventoryCurrency.Row - 1);
			if ((Vector2i)position != item.LocationTopLeft)
			{
				if (!OccupiedBySameItem(string_0, position))
				{
					return position;
				}
				GlobalLog.Error("[SortInventoryTask] Unexpected error. \"" + string_0 + "\" will not be sorted correctly because destination position is already occupied by the same item.");
				return GetMovePosLessThanCurrent(item);
			}
			return null;
		}
		return GetMovePosLessThanCurrent(item);
	}

	private static Position GetMovePosForCursorItem(Item item)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		string string_0 = item.Name;
		ExtensionsSettings.InventoryCurrency inventoryCurrency = ExtensionsSettings.Instance.InventoryCurrencies.FirstOrDefault((ExtensionsSettings.InventoryCurrency i) => i.Name == string_0);
		if (inventoryCurrency != null && HasFixedPosition(inventoryCurrency))
		{
			Position position = new Position(inventoryCurrency.Column - 1, inventoryCurrency.Row - 1);
			if (OccupiedBySameItem(string_0, position))
			{
				GlobalLog.Error("[SortInventoryTask] Unexpected error. \"" + string_0 + "\" will not be sorted correctly because destination position is already occupied by the same item.");
				return GetMovePosFirstAvailable(item);
			}
			return position;
		}
		return GetMovePosFirstAvailable(item);
	}

	private static Position GetMovePosLessThanCurrent(Item item)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Position movePosFirstAvailable = GetMovePosFirstAvailable(item);
		return (Position.Comparer.Instance.Compare(movePosFirstAvailable, item.LocationTopLeft) < 0) ? movePosFirstAvailable : null;
	}

	private static Position GetMovePosFirstAvailable(Item item)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		int x = default(int);
		int y = default(int);
		if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item.Size, ref x, ref y))
		{
			GlobalLog.Error("[SortInventoryTask] Unexpected error. Cannot fit item anywhere in main inventory.");
			ErrorManager.ReportCriticalError();
		}
		return new Position(x, y);
	}

	private static bool HasFixedPosition(ExtensionsSettings.InventoryCurrency item)
	{
		return item.Row > 0 && item.Column > 0;
	}

	private static bool OccupiedBySameItem(string name, Vector2i pos)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Item val = Inventories.InventoryItems.Find((Item i) => i.LocationTopLeft == pos);
		return (RemoteMemoryObject)(object)val != (RemoteMemoryObject)null && val.Name == name;
	}

	private static async Task<bool> MoveCursorItem(Vector2i to)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (!(await Inventories.OpenInventory()))
		{
			return false;
		}
		if (await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(to))
		{
			return true;
		}
		return false;
	}

	private static async Task<bool> MoveInventoryItem(Vector2i from, Vector2i to)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!(await Inventories.OpenInventory()))
		{
			return false;
		}
		if (!(await InventoryUi.InventoryControl_Main.PickItemToCursor(from)))
		{
			return false;
		}
		if (await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(to))
		{
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id != "items_stashed_event")
		{
			return (MessageResult)1;
		}
		ShouldSort = true;
		return (MessageResult)0;
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
		ShouldSort = true;
	}

	public void Stop()
	{
	}
}
