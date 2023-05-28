using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions;

public static class TownNpcs
{
	public static readonly TownNpc Nessa;

	public static readonly TownNpc Bestel;

	public static readonly TownNpc Tarkleigh;

	public static readonly TownNpc Greust;

	public static readonly TownNpc Eramir;

	public static readonly TownNpc Yeena;

	public static readonly TownNpc Silk;

	public static readonly TownNpc Helena;

	public static readonly TownNpc Clarissa;

	public static readonly TownNpc Hargan;

	public static readonly TownNpc Maramoa;

	public static readonly TownNpc Grigor;

	public static readonly TownNpc Kira;

	public static readonly TownNpc PetarusAndVanja;

	public static readonly TownNpc Tasuni;

	public static readonly TownNpc Dialla;

	public static readonly TownNpc Oyun;

	public static readonly TownNpc Utula;

	public static readonly TownNpc Lani;

	public static readonly TownNpc Vilenta;

	public static readonly TownNpc Bannon;

	public static readonly TownNpc BestelA6;

	public static readonly TownNpc townNpc_0;

	public static readonly TownNpc LillyRoth;

	public static readonly TownNpc LillyRothQuest;

	public static readonly TownNpc YeenaA7;

	public static readonly TownNpc EramirA7;

	public static readonly TownNpc HelenaA7;

	public static readonly TownNpc SinA7;

	public static readonly TownNpc WeylamRoth;

	public static readonly TownNpc townNpc_1;

	public static readonly TownNpc HarganA8;

	public static readonly TownNpc townNpc_2;

	public static readonly TownNpc TasuniA9;

	public static readonly TownNpc SinA9;

	public static readonly TownNpc PetarusAndVanjaA9;

	public static readonly TownNpc Irasha;

	public static readonly TownNpc LaniA10;

	public static readonly TownNpc LillyRothA10;

	public static readonly TownNpc WeylamRothA10;

	public static readonly TownNpc LaniA11;

	public static readonly TownNpc WeylamRothA11;

	public static readonly TownNpc KiracA11;

	public static readonly TownNpc LillyRothA11;

	public static TownNpc GetSellVendor()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (currentArea.IsHideoutArea)
		{
			Npc val = ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Lilly Roth");
			if (!((NetworkObject)(object)val != (NetworkObject)null))
			{
				Npc val2 = ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Zana, Master Cartographer");
				if ((NetworkObject)(object)val2 != (NetworkObject)null)
				{
					return new TownNpc(((NetworkObject)(object)val2).WalkablePosition());
				}
				Npc val3 = ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Einhar, Beastmaster");
				if (!((NetworkObject)(object)val3 != (NetworkObject)null))
				{
					Npc val4 = ((IEnumerable)ObjectManager.Objects).Closest<Npc>((Func<Npc, bool>)((Npc n) => ((NetworkObject)n).IsTargetable));
					if ((NetworkObject)(object)val4 != (NetworkObject)null)
					{
						return new TownNpc(((NetworkObject)(object)val4).WalkablePosition());
					}
					GlobalLog.Error("[GetSellVendor] Fail to find any targetable NPC in hideout.");
					return null;
				}
				return new TownNpc(((NetworkObject)(object)val3).WalkablePosition());
			}
			return new TownNpc(((NetworkObject)(object)val).WalkablePosition());
		}
		if (currentArea.IsTown)
		{
			switch (currentArea.Act)
			{
			case 1:
				return RandomSellNpc(Nessa, Tarkleigh);
			case 2:
				return RandomSellNpc(Greust, Yeena);
			case 3:
				return RandomSellNpc(Clarissa, Hargan);
			case 4:
				return RandomSellNpc(Kira, PetarusAndVanja);
			case 5:
				return RandomSellNpc(Lani, Bannon, Utula);
			case 6:
				return RandomSellNpc(BestelA6, townNpc_0);
			case 7:
				return RandomSellNpc(YeenaA7, HelenaA7);
			case 8:
				return RandomSellNpc(townNpc_1, HarganA8);
			case 9:
				return RandomSellNpc(PetarusAndVanjaA9, Irasha);
			case 10:
				return RandomSellNpc(LaniA10, WeylamRothA10);
			case 11:
				return RandomSellNpc(LaniA11, WeylamRothA11);
			}
		}
		GlobalLog.Error("[GetSellVendor] Unsupported area for sell vendor \"" + currentArea.Name + "\".");
		return null;
	}

	public static TownNpc RandomSellNpc(TownNpc a, TownNpc b, TownNpc c = null)
	{
		List<TownNpc> list = new List<TownNpc>();
		if (a != null && a.NpcObject != (NetworkObject)null && ObjectManager.GetObjectByName(a.NpcObject.Name) != (NetworkObject)null)
		{
			list.Add(a);
		}
		if (b != null && b.NpcObject != (NetworkObject)null && ObjectManager.GetObjectByName(b.NpcObject.Name) != (NetworkObject)null)
		{
			list.Add(b);
		}
		if (c != null && c.NpcObject != (NetworkObject)null && ObjectManager.GetObjectByName(c.NpcObject.Name) != (NetworkObject)null)
		{
			list.Add(c);
		}
		if (list.Count != 0)
		{
			return list[LokiPoe.Random.Next(list.Count)];
		}
		return a;
	}

	public static TownNpc GetCurrencyVendor()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (currentArea.IsHideoutArea)
		{
			GlobalLog.Error("[GetCurrencyVendor] Cannot buy currency in hideout.");
			return null;
		}
		if (currentArea.IsTown)
		{
			switch (currentArea.Act)
			{
			case 1:
				return Nessa;
			case 2:
				return Yeena;
			case 3:
				return Clarissa;
			case 4:
				return PetarusAndVanja;
			case 5:
				return Lani;
			case 6:
				return BestelA6;
			case 7:
				return YeenaA7;
			case 8:
				return townNpc_1;
			case 9:
				return PetarusAndVanjaA9;
			case 10:
				return LaniA10;
			case 11:
				return LaniA11;
			}
		}
		GlobalLog.Error("[GetCurrencyVendor] Unsupported area for currency vendor \"" + currentArea.Name + "\".");
		return null;
	}

	public static async Task<bool> SellItems(List<Vector2i> itemPositions)
	{
		List<Item> list_0;
		while (itemPositions != null)
		{
			if (itemPositions.Count != 0)
			{
				TownNpc vendor = GetSellVendor();
				if (vendor != null)
				{
					if (!(vendor.Position.Name == "Lilly Roth"))
					{
						if (!(await vendor.OpenSellPanel()))
						{
							return false;
						}
					}
					else if (!(await vendor.OpenDefaultCtrlSell()))
					{
						return false;
					}
					itemPositions.Sort(Position.Comparer.Instance);
					ListExtensions.Shuffle<Vector2i>((IList<Vector2i>)itemPositions);
					List<CachedItem> soldItems = new List<CachedItem>(itemPositions.Count);
					List<Vector2i> notSoldItems = new List<Vector2i>();
					list_0 = Inventories.InventoryItems;
					foreach (Vector2i itemPos in itemPositions)
					{
						Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
						if ((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null)
						{
							GlobalLog.Error($"[SellItems] Fail to find item at {itemPos}. Skipping it.");
							continue;
						}
						soldItems.Add(new CachedItem(item));
						if (SellUi.TradeControl.InventoryControl_YourOffer.Inventory.CanFitItem(item.Size))
						{
							if (!(await Inventories.FastMoveToVendor(itemPos)))
							{
								return false;
							}
							continue;
						}
						break;
					}
					List<CachedItem> gainedItems = SellUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Select((Item i) => new CachedItem(i)).ToList();
					TradeResult accepted = SellUi.TradeControl.Accept(true);
					if ((int)accepted <= 0)
					{
						if (!(await Wait.For(() => !SellUi.IsOpened, "sell panel closing")))
						{
							await Coroutines.CloseBlockingWindows();
							return false;
						}
						await Wait.For(() => list_0 != Inventories.InventoryItems, "items to sell", 20, 1000);
						GlobalLog.Info($"[Events] Items sold ({soldItems.Count})");
						Utility.BroadcastMessage((object)null, "items_sold_event", new object[2] { soldItems, gainedItems });
						if (notSoldItems.Any())
						{
							itemPositions = notSoldItems;
							continue;
						}
						return true;
					}
					GlobalLog.Error($"[SellItems] Fail to accept sell. Error: \"{accepted}\".");
					return false;
				}
				return false;
			}
			GlobalLog.Error("[SellItems] Item list for sell is empty.");
			return false;
		}
		GlobalLog.Error("[SellItems] Item list for sell is null.");
		return false;
	}

	static TownNpcs()
	{
		Nessa = new TownNpc(new WalkablePosition("Nessa", 340, 261));
		Bestel = new TownNpc(new WalkablePosition("Bestel", 354, 245));
		Tarkleigh = new TownNpc(new WalkablePosition("Tarkleigh", 381, 189));
		Greust = new TownNpc(new WalkablePosition("Greust", 193, 266));
		Eramir = new TownNpc(new WalkablePosition("Eramir", 162, 262));
		Yeena = new TownNpc(new WalkablePosition("Yeena", 162, 333));
		Silk = new TownNpc(new WalkablePosition("Silk", 170, 332));
		Helena = new TownNpc(new WalkablePosition("Helena", 167, 339));
		Clarissa = new TownNpc(new WalkablePosition("Clarissa", 239, 439));
		Hargan = new TownNpc(new WalkablePosition("Hargan", 363, 465));
		Maramoa = new TownNpc(new WalkablePosition("Maramoa", 365, 419));
		Grigor = new TownNpc(new WalkablePosition("Grigor", 212, 493));
		Kira = new TownNpc(new WalkablePosition("Kira", 169, 503));
		PetarusAndVanja = new TownNpc(new WalkablePosition("Petarus and Vanja", 204, 546));
		Tasuni = new TownNpc(new WalkablePosition("Tasuni", 398, 449));
		Dialla = new TownNpc(new WalkablePosition("Dialla", 544, 502));
		Oyun = new TownNpc(new WalkablePosition("Oyun", 557, 496));
		Utula = new TownNpc(new WalkablePosition("Utula", 350, 310));
		Lani = new TownNpc(new WalkablePosition("Lani", 364, 305));
		Vilenta = new TownNpc(new WalkablePosition("Vilenta", 404, 316));
		Bannon = new TownNpc(new WalkablePosition("Bannon", 411, 341));
		BestelA6 = new TownNpc(new WalkablePosition("Bestel", 354, 383));
		townNpc_0 = new TownNpc(new WalkablePosition("Tarkleigh", 385, 327));
		LillyRoth = new TownNpc(new WalkablePosition("Lilly Roth", 336, 395));
		LillyRothQuest = new TownNpc(new WalkablePosition("Lilly Roth", 229, 143));
		YeenaA7 = new TownNpc(new WalkablePosition("Yeena", 560, 623));
		EramirA7 = new TownNpc(new WalkablePosition("Eramir", 564, 576));
		HelenaA7 = new TownNpc(new WalkablePosition("Helena", 579, 619));
		SinA7 = new TownNpc(new WalkablePosition("Sin", 566, 414));
		WeylamRoth = new TownNpc(new WalkablePosition("Weylam Roth", 541, 404));
		townNpc_1 = new TownNpc(new WalkablePosition("Clarissa", 230, 445));
		HarganA8 = Hargan;
		townNpc_2 = Maramoa;
		TasuniA9 = Tasuni;
		SinA9 = new TownNpc(new WalkablePosition("Sin", 184, 461));
		PetarusAndVanjaA9 = new TownNpc(new WalkablePosition("Petarus and Vanja", 169, 523));
		Irasha = new TownNpc(new WalkablePosition("Irasha", 190, 551));
		LaniA10 = new TownNpc(new WalkablePosition("Lani", 439, 281));
		LillyRothA10 = new TownNpc(new WalkablePosition("Lilly Roth", 447, 355));
		WeylamRothA10 = new TownNpc(new WalkablePosition("Weylam Roth", 448, 350));
		LaniA11 = new TownNpc(new WalkablePosition("Lani", 801, 856));
		WeylamRothA11 = new TownNpc(new WalkablePosition("Weylam Roth", 784, 850));
		KiracA11 = new TownNpc(new WalkablePosition("Commander Kirac", 877, 761));
		LillyRothA11 = new TownNpc(new WalkablePosition("Lilly Roth", 270, 463));
	}
}
