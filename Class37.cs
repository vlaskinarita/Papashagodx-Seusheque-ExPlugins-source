using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Components;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

internal class Class37 : Class36
{
	private static string string_0;

	private static bool bool_1;

	public override bool Enabled => Class36.Settings.CardsEnabled;

	public override bool ShouldExecute => (string_0 != null || bool_1) && !base.ErrorLimitReached;

	private static Item ExchangeUiItem => CardTradeUi.InventoryControl.Inventory.Items.FirstOrDefault();

	private static int CardCountInInventory => Inventories.InventoryItems.Count((Item i) => i.Class == "DivinationCard");

	private static Npc Lilly => ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Lilly Roth" && ((NetworkObject)n).IsTargetable);

	public override async Task Execute()
	{
		GlobalLog.Debug("[card] executed");
		AreaInfo area = GetCardExchangeArea();
		if (!(area == null))
		{
			if (bool_1)
			{
				GlobalLog.Warn("[VendorTask] Item was left in ExchangeUi. Now going to take it.");
				bool flag = !area.IsCurrentArea;
				bool flag2 = flag;
				if (flag2)
				{
					flag2 = !(await PlayerAction.TakeWaypoint(area));
				}
				if (flag2)
				{
					ReportError();
				}
				else if (!(await TakeItemFromExchangeUi()))
				{
					ReportError();
				}
				return;
			}
			GlobalLog.Info("[VendorTask] Now going to exchange divination cards.");
			if (await TakeCards())
			{
				bool flag3 = !area.IsCurrentArea;
				bool flag4 = flag3;
				if (flag4)
				{
					flag4 = !(await PlayerAction.TakeWaypoint(area));
				}
				if (flag4)
				{
					ReportError();
				}
				else if (!(await ExchangeCards()))
				{
					ReportError();
				}
			}
			else
			{
				ReportError();
			}
		}
		else
		{
			GlobalLog.Warn("[VendorTask] Divination card exchange is not possible because this character has no access to Highgate and we are not in hideout.");
			ResetData();
		}
	}

	public override void OnStashing(CachedItem item)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (string_0 == null && (int)item.Type.ItemType == 13)
		{
			GlobalLog.Debug("[card] onStashing");
			int num = (StashUi.StashTabInfo.IsPremiumDivination ? DivinationTab.All.Sum((Func<InventoryControlWrapper, int>)CardSetsInControl) : StashUi.InventoryControl.Inventory.Items.Count(ItemIsCardSet));
			string currentTabName = StashUi.TabControl.CurrentTabName;
			GlobalLog.Info($"[OnCardStash] Found {num} complete divination card sets in \"{currentTabName}\" tab.");
			if (num >= Class36.Settings.MinCardSets)
			{
				string_0 = currentTabName;
			}
		}
	}

	public override void ResetData()
	{
		string_0 = null;
		bool_1 = false;
	}

	private static void WriteToLog(string cardName, Item result)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"[{DateTime.Now}] \"{cardName}\" exchanged to \"{result.FullName}\" ({result.Class}");
		Mods modsComponent = result.Components.ModsComponent;
		stringBuilder.Append(((RemoteMemoryObject)(object)modsComponent != (RemoteMemoryObject)null) ? $", {modsComponent.Rarity})\n" : ")\n");
		File.AppendAllText("State/" + ((NetworkObject)LokiPoe.Me).Name + "-DivinationCardExchange.txt", stringBuilder.ToString());
	}

	private static async Task<bool> TakeCards()
	{
		if (await Inventories.OpenStashTab(string_0, "CardExchange"))
		{
			if (StashUi.StashTabInfo.IsPremiumDivination)
			{
				GlobalLog.Error("[CardExchange] Divination tab is not supported!");
				BotManager.Stop(new StopReasonData("tab_not_supported", "Divination tab is not supported!", (object)null), false);
			}
			while (true)
			{
				int int_0 = CardCountInInventory;
				if (int_0 < Class36.Settings.MaxCardSets)
				{
					Item card = Inventories.StashTabItems.Where(ItemIsCardSet).OrderBy((Item i) => i.LocationTopLeft, Position.Comparer.Instance).FirstOrDefault();
					if (!((RemoteMemoryObject)(object)card == (RemoteMemoryObject)null))
					{
						GlobalLog.Info("[TakeCards] Now taking \"" + card.Name + "\".");
						if (await Inventories.FastMoveFromStashTab(card.LocationTopLeft))
						{
							if (!(await Wait.For(() => CardCountInInventory > int_0, "cards appear in inventory")))
							{
								break;
							}
							continue;
						}
						return false;
					}
					string_0 = null;
					return true;
				}
				GlobalLog.Warn($"[TakeCards] Max card sets for exchange has been reached ({Class36.Settings.MaxCardSets})");
				return true;
			}
			return false;
		}
		return false;
	}

	private static async Task<bool> ExchangeCards()
	{
		List<Vector2i> cardPositions = (from c in Inventories.InventoryItems.Where(ItemIsCardSet)
			select c.LocationTopLeft).ToList();
		if (cardPositions.Count != 0)
		{
			cardPositions.Sort(Position.Comparer.Instance);
			GlobalLog.Info($"[ExchangeCards] Now going to exchange {cardPositions.Count} divination card sets.");
			bool flag = !CardTradeUi.IsOpened;
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = !(await OpenExchangeUi());
			}
			if (!flag2)
			{
				bool flag3 = (RemoteMemoryObject)(object)ExchangeUiItem != (RemoteMemoryObject)null;
				bool flag4 = flag3;
				if (flag4)
				{
					flag4 = !(await TakeItemFromExchangeUi());
				}
				if (!flag4)
				{
					foreach (Vector2i cardPos in cardPositions)
					{
						if (!bool_1)
						{
							if (await ExchangeCard(cardPos))
							{
								if (!(await TakeItemFromExchangeUi()))
								{
									return false;
								}
								continue;
							}
							return false;
						}
						break;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		GlobalLog.Error("[ExchangeCards] Fail to find any complete divination set in inventory.");
		return false;
	}

	private static async Task<bool> ExchangeCard(Vector2i cardPos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		bool flag = !CardTradeUi.IsOpened;
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = !(await OpenExchangeUi());
		}
		if (!flag2)
		{
			Item card = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(cardPos);
			if (!((RemoteMemoryObject)(object)card == (RemoteMemoryObject)null))
			{
				string cardName = card.Name;
				GlobalLog.Info("[ExchangeCard] Now exchanging \"" + cardName + "\".");
				if (!(await Inventories.FastMoveFromInventory(cardPos)))
				{
					return false;
				}
				if (!(await Wait.For(() => (RemoteMemoryObject)(object)ExchangeUiItem != (RemoteMemoryObject)null, "divination card appear in ExchangeUi")))
				{
					return false;
				}
				await Wait.SleepSafe(200);
				int int_0 = ExchangeUiItem.LocalId;
				ActivateResult activated = CardTradeUi.Activate(true);
				if ((int)activated > 0)
				{
					GlobalLog.Error($"[ExchangeCard] Fail to activate the ExchangeUi. Error: \"{activated}\".");
					return false;
				}
				if (!(await Wait.For(delegate
				{
					Item exchangeUiItem = ExchangeUiItem;
					return (RemoteMemoryObject)(object)exchangeUiItem != (RemoteMemoryObject)null && exchangeUiItem.LocalId != int_0;
				}, "divination card exchanging")))
				{
					return false;
				}
				WriteToLog(cardName, ExchangeUiItem);
				return true;
			}
			GlobalLog.Error($"[ExchangeCard] Fail to find item at {cardPos}.");
			return false;
		}
		return false;
	}

	private static async Task<bool> TakeItemFromExchangeUi()
	{
		bool flag = !CardTradeUi.IsOpened;
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = !(await OpenExchangeUi());
		}
		if (!flag2)
		{
			Item item = ExchangeUiItem;
			if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
			{
				string name = item.FullName;
				int id = item.LocalId;
				GlobalLog.Debug("[TakeItemFromExchangeUi] Now taking \"" + name + "\".");
				if (InventoryUi.InventoryControl_Main.Inventory.CanFitItem(item.Size))
				{
					FastMoveResult moved = CardTradeUi.InventoryControl.FastMove(id, true, true);
					if ((int)moved <= 0)
					{
						if (await Wait.For(() => (RemoteMemoryObject)(object)ExchangeUiItem == (RemoteMemoryObject)null, "item moving from ExchangeUi"))
						{
							bool_1 = false;
							return true;
						}
						return false;
					}
					GlobalLog.Error($"[TakeItemFromExchangeUi] Fast move error: \"{moved}\".");
					return false;
				}
				GlobalLog.Warn("[TakeItemFromExchangeUi] Cannot take \"" + name + "\". Not enough inventory space.");
				bool_1 = true;
				return true;
			}
			GlobalLog.Error("[TakeItemFromExchangeUi] ExchangeUi is empty.");
			return true;
		}
		return false;
	}

	private static async Task<bool> OpenExchangeUi()
	{
		TownNpc npc = GetCardExchangeNpc();
		if (npc == null)
		{
			return false;
		}
		if (await npc.OpenDialogPanel())
		{
			ConverseResult conversed = NpcDialogUi.Converse("Trade Divination Cards", true);
			if ((int)conversed <= 0)
			{
				if (!(await Wait.For(() => CardTradeUi.IsOpened, "ExchangeUi opening")))
				{
					return false;
				}
				GlobalLog.Debug("[OpenExchangeUi] ExchangeUi has been successfully opened.");
				return true;
			}
			GlobalLog.Error($"[OpenExchangeUi] Fail to converse \"Trade Divination Cards\". Error: \"{conversed}\".");
			return false;
		}
		return false;
	}

	private static int CardSetsInControl(InventoryControlWrapper control)
	{
		Item customTabItem = control.CustomTabItem;
		return (!((RemoteMemoryObject)(object)customTabItem == (RemoteMemoryObject)null)) ? (customTabItem.StackCount / customTabItem.MaxStackCount) : 0;
	}

	private static bool ItemIsCardSet(Item item)
	{
		return item.Class == "DivinationCard" && item.StackCount >= item.MaxStackCount;
	}

	private static AreaInfo GetCardExchangeArea()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.IsHideoutArea || !((NetworkObject)(object)Lilly != (NetworkObject)null))
		{
			int act = World.LastOpenedAct.Act;
			if (act >= 9)
			{
				return World.Act9.Highgate;
			}
			if (act >= 4)
			{
				return World.Act4.Highgate;
			}
			return null;
		}
		return currentArea;
	}

	private static TownNpc GetCardExchangeNpc()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.IsHideoutArea)
		{
			if (!(currentArea == World.Act4.Highgate))
			{
				if (!(currentArea == World.Act9.Highgate))
				{
					GlobalLog.Error($"[GetCardExchangeNpc] Unexpected area: {(AreaInfo)currentArea}");
					return null;
				}
				return TownNpcs.TasuniA9;
			}
			return TownNpcs.Tasuni;
		}
		Npc lilly = Lilly;
		if ((NetworkObject)(object)lilly == (NetworkObject)null)
		{
			GlobalLog.Error("[GetCardExchangeNpc] Unexpected error. Fail to find Lilly in hideout.");
			return null;
		}
		return ((NetworkObject)(object)lilly).AsTownNpc();
	}
}
