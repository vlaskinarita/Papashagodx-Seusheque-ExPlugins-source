using System;
using System.Collections.Generic;
using System.IO;
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
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;

internal class Class4 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private Vector2i vector2i_0;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "ListItemsTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.IsTown && !area.IsHideoutArea)
		{
			return false;
		}
		if (TraderPlugin.EvaluatedItems.TryDequeue(out var itemToList) && !(itemToList.Name == "Chaos Orb") && !(itemToList.Name == "Divine Orb"))
		{
			if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
			{
				await Inventories.OpenStash();
			}
			await Inventories.OpenStashTab(TraderPluginSettings.Instance.StashTabToTrade, Name);
			Item item_0 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(new Vector2i(itemToList.ItemPos.X, itemToList.ItemPos.Y));
			if (!((RemoteMemoryObject)(object)item_0 == (RemoteMemoryObject)null))
			{
				GlobalLog.Debug("[ListItemTask] Now going to list " + item_0.Name);
				if (itemToList.ForcePutToCoords)
				{
					vector2i_0 = itemToList.CoordsToPut;
				}
				else
				{
					bool flag = false;
					int stashx = 12;
					int stashy = 12;
					if (StashUi.StashTabInfo.IsPremiumQuad)
					{
						stashx = 48;
						stashy = 48;
					}
					for (int j = 0; j < stashx; j++)
					{
						for (int k = 0; k < stashy; k++)
						{
							if (StashUi.InventoryControl.Inventory.CanFitItemSizeAt(item_0.Size.X, item_0.Size.Y, j, k))
							{
								vector2i_0 = new Vector2i(j, k);
								GlobalLog.Debug($"[ListItemTask] Can fit item on pos {itemToList.ItemPos.X}, {itemToList.ItemPos.Y} to pos {vector2i_0.X}, {vector2i_0.Y}");
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
				Random rnd = new Random();
				bool fullStack = false;
				bool wasSplited = false;
				bool wasFound = false;
				if (item_0.MaxStackCount <= 1)
				{
					GlobalLog.Debug($"[ListItemTask] Now picking item at {itemToList.ItemPos.X}, {itemToList.ItemPos.Y} in inventory 3");
					await InventoryUi.InventoryControl_Main.PickItemToCursor(itemToList.ItemPos);
					await Inventories.WaitForCursorToHaveItem();
					GlobalLog.Debug($"[ListItemTask] Now placing item at {vector2i_0.X}, {vector2i_0.Y} in stash 3");
					await StashUi.InventoryControl.PlaceItemFromCursor(new Vector2i(vector2i_0.X, vector2i_0.Y));
					await Inventories.WaitForCursorToBeEmpty();
					wasFound = true;
				}
				else
				{
					foreach (Item stashItem in StashUi.InventoryControl.Inventory.Items)
					{
						if (!TraderPluginSettings.Instance.DebugMode)
						{
						}
						if (!(item_0.Name == stashItem.Name))
						{
							continue;
						}
						wasFound = true;
						GlobalLog.Debug($"[ListItemTask] Found! Coords {stashItem.LocationTopLeft}");
						if (stashItem.StackCount >= stashItem.MaxStackCount)
						{
							fullStack = true;
							continue;
						}
						GlobalLog.Debug($"[ListItemTask] Stack is not full! Stack amount {stashItem.StackCount}");
						if (item_0.StackCount > stashItem.MaxStackCount - stashItem.StackCount)
						{
							InventoryUi.InventoryControl_Main.SplitStack(item_0.LocalId, stashItem.MaxStackCount - stashItem.StackCount, true);
							await Inventories.WaitForCursorToHaveItem();
							GlobalLog.Debug($"[ListItemTask] Now placing item at {stashItem.LocationTopLeft.X}, {stashItem.LocationTopLeft.Y} in stash 1");
							await StashUi.InventoryControl.PlaceItemFromCursor(new Vector2i(stashItem.LocationTopLeft.X, stashItem.LocationTopLeft.Y));
							await Inventories.WaitForCursorToBeEmpty();
							wasSplited = true;
							fullStack = false;
						}
						else
						{
							await Inventories.FastMoveFromInventory(item_0.LocationTopLeft);
							Thread.Sleep(10 + rnd.Next(99));
							fullStack = false;
						}
						break;
					}
				}
				if (wasSplited || fullStack || !wasFound)
				{
					GlobalLog.Debug($"[ListItemTask] Now picking item at {itemToList.ItemPos.X}, {itemToList.ItemPos.Y} in inventory 4");
					await InventoryUi.InventoryControl_Main.PickItemToCursor(itemToList.ItemPos);
					await Inventories.WaitForCursorToHaveItem();
					GlobalLog.Debug($"[ListItemTask] Now placing item at {vector2i_0.X}, {vector2i_0.Y} in stash 4");
					await StashUi.InventoryControl.PlaceItemFromCursor(new Vector2i(vector2i_0.X, vector2i_0.Y));
					await Inventories.WaitForCursorToBeEmpty();
				}
				item_0 = StashUi.InventoryControl.Inventory.FindItemByPos(new Vector2i(vector2i_0.X, vector2i_0.Y));
				if (!((RemoteMemoryObject)(object)item_0 == (RemoteMemoryObject)null))
				{
					if (item_0.MaxStackCount <= 1 || !StashUi.InventoryControl.Inventory.Items.Any((Item i) => i.FullName == item_0.FullName && !string.IsNullOrEmpty(i.DisplayNote)))
					{
						StashUi.InventoryControl.OpenDisplayNote(item_0.LocalId, true);
						string price2;
						if (!string.IsNullOrEmpty(itemToList.ExactNote))
						{
							price2 = itemToList.ExactNote;
						}
						else
						{
							price2 = ((itemToList.Currency == "chaos") ? ("~price " + itemToList.Price + " " + itemToList.Currency) : ("~price " + itemToList.Price + " " + itemToList.Currency));
							price2 = price2.Replace(",", ".");
						}
						if (DisplayNoteUi.IsOpened)
						{
							if (item_0.Name.Contains("Stacked Deck") && !string.IsNullOrEmpty(TraderPluginSettings.Instance.StackedDeckExactNote))
							{
								price2 = TraderPluginSettings.Instance.StackedDeckExactNote;
							}
							DisplayNoteUi.Note = price2;
							DisplayNoteUi.Accept();
							Vector2i itemPos = new Vector2i(vector2i_0.X, vector2i_0.Y);
							List<Class9.Class10> mnePohui = new List<Class9.Class10>();
							GlobalLog.Debug("[ListItemsTask] Path: " + TraderPlugin.FullFileName);
							if (File.Exists(TraderPlugin.FullFileName))
							{
								mnePohui = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
								GlobalLog.Debug("[ListItemsTask] Now trying to add " + item_0.FullName + " to json");
								mnePohui.Add(new Class9.Class10(itemPos, item_0.FullName, price2, item_0.StackCount, DateTime.Now, InfluenceHelper.GetInfluence(item_0)));
							}
							File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui, (Formatting)1));
						}
						return true;
					}
					return true;
				}
				return true;
			}
			return true;
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
