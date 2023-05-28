using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;

internal class Class38 : Class36
{
	private class Class45
	{
		public readonly string Name;

		public int Amount;

		public Class45(string name, int amount)
		{
			Name = name;
			Amount = amount;
		}
	}

	private static readonly List<Class45> list_0;

	private static readonly Dictionary<string, int> dictionary_0;

	private static readonly Dictionary<string, Class45> dictionary_1;

	private static readonly AreaInfo[] areaInfo_0;

	public override bool Enabled => true;

	public override bool ShouldExecute => list_0.Count > 0 && !base.ErrorLimitReached;

	public override async Task Execute()
	{
		list_0.Sort((Class45 c1, Class45 c2) => dictionary_0[c1.Name].CompareTo(dictionary_0[c2.Name]));
		Class45 currency = list_0[0];
		GlobalLog.Info($"[VendorTask] Now going to buy {currency.Amount} {currency.Name}.");
		while (!CanBuyInCurrentArea(currency.Name))
		{
			AreaInfo exchangeArea = GetExchangeArea();
			if (!(exchangeArea == null))
			{
				if (!(await PlayerAction.TakeWaypoint(exchangeArea)))
				{
					ReportError();
					return;
				}
				await Coroutines.ReactionWait();
				continue;
			}
			GlobalLog.Warn("[VendorTask] Cannot exchange \"" + currency.Name + "\" in current area and Act 3 has not been opened yet.");
			ResetData();
			return;
		}
		if (!(await CurrencyPurchase(currency)))
		{
			ReportError();
		}
	}

	public override void OnStashing(CachedItem item)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)item.Type.ItemType == 1)
		{
			CheckCurrency(Class36.Settings.TransExchange, CurrencyNames.Transmutation, item.Name);
			CheckCurrency(Class36.Settings.AugsExchange, CurrencyNames.Augmentation, item.Name);
			CheckCurrency(Class36.Settings.AltsExchange, CurrencyNames.Alteration, item.Name);
			CheckCurrency(Class36.Settings.JewsExchange, CurrencyNames.Jeweller, item.Name);
			CheckCurrency(Class36.Settings.ChancesExchange, CurrencyNames.Chance, item.Name);
			CheckCurrency(Class36.Settings.ScoursExchange, CurrencyNames.Scouring, item.Name);
			CheckCurrency(Class36.Settings.RegretExchange, CurrencyNames.Regret, item.Name);
		}
	}

	public override void ResetData()
	{
		list_0.Clear();
	}

	private static async Task<bool> CurrencyPurchase(Class45 currency)
	{
		if (!(await TownNpcs.GetCurrencyVendor().OpenPurchasePanel()))
		{
			return false;
		}
		await Wait.SleepSafe(500);
		string string_0 = currency.Name;
		Item item = PurchaseUi.InventoryControl.Inventory.Items.Find((Item i) => i.Name == string_0);
		if ((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[CurrencyPurchase] Unexpected error. Fail to find \"" + string_0 + "\" in vendor's inventory.");
			return false;
		}
		int id = item.LocalId;
		while (true)
		{
			if (currency.Amount > Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount))
			{
				if (!BotManager.IsStopping)
				{
					if (LokiPoe.IsInGame)
					{
						if (HasInvenotorySpaceForCurrency(string_0))
						{
							int int_0 = Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount);
							GlobalLog.Info($"Purchasing \"{string_0}\" ({currency.Amount - Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount)})");
							if (currency.Amount - Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount) >= item.MaxStackCount)
							{
								if (ProcessHookManager.GetKeyState(Keys.ShiftKey) != short.MinValue)
								{
									ProcessHookManager.SetKeyState(Keys.ShiftKey, short.MinValue, Keys.None);
								}
								PurchaseUi.InventoryControl.FastMove(id, true, true, true);
							}
							else
							{
								ProcessHookManager.SetKeyState(Keys.ShiftKey, (short)0, Keys.None);
								PurchaseUi.InventoryControl.FastMove(id, true, true);
							}
							if (!(await Wait.For(() => Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount) > int_0, "purchase")))
							{
								break;
							}
							continue;
						}
						GlobalLog.Warn("[CurrencyPurchase] Not enough inventory space.");
					}
					else
					{
						GlobalLog.Error("[CurrencyPurchase] Disconnected during currency purchase.");
					}
				}
				else
				{
					GlobalLog.Debug("[CurrencyPurchase] Bot is stopping. Now breaking from purchase loop.");
				}
			}
			if (currency.Amount <= Inventories.InventoryItems.Where((Item i) => i.FullName == string_0).Sum((Item i) => i.StackCount))
			{
				ProcessHookManager.ClearAllKeyStates();
				list_0.RemoveAt(0);
			}
			return true;
		}
		GlobalLog.Error("[CurrencyPurchase] Fail to purchase.");
		return false;
	}

	private static void CheckCurrency(ExtensionsSettings.ExchangeSettings es, string name, string actualName)
	{
		if (!es.Enabled || name != actualName)
		{
			return;
		}
		int currencyAmountInStashTab = Inventories.GetCurrencyAmountInStashTab(name);
		if (currencyAmountInStashTab >= es.Min)
		{
			Class45 class45_0 = dictionary_1[name];
			int num = (currencyAmountInStashTab - es.Save) / class45_0.Amount;
			string currentTabName = StashUi.TabControl.CurrentTabName;
			GlobalLog.Warn($"[VendorTask] {currencyAmountInStashTab - es.Save} {name} in \"{currentTabName}\" tab will be exchanged to {num} {class45_0.Name}.");
			Class45 @class = list_0.Find((Class45 c) => c.Name == class45_0.Name);
			if (@class != null)
			{
				@class.Amount = num;
			}
			else
			{
				list_0.Add(new Class45(class45_0.Name, num));
			}
		}
	}

	private static bool HasInvenotorySpaceForCurrency(string name)
	{
		Inventory inventory = InventoryUi.InventoryControl_Main.Inventory;
		return inventory.AvailableInventorySquares > 0 || inventory.Items.Exists((Item i) => i.Name == name && i.StackCount < i.MaxStackCount);
	}

	private static bool CanBuyInCurrentArea(string name)
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.IsTown)
		{
			return false;
		}
		if (!(currentArea == World.Act1.LioneyeWatch))
		{
			if (currentArea == World.Act2.ForestEncampment)
			{
				return name == CurrencyNames.Jeweller || name == CurrencyNames.Fusing || name == CurrencyNames.Scouring || name == CurrencyNames.Regret;
			}
			return true;
		}
		return name == CurrencyNames.Augmentation || name == CurrencyNames.Alteration;
	}

	private static AreaInfo GetExchangeArea()
	{
		int act = World.LastOpenedAct.Act;
		if (act >= 3)
		{
			string currencyExchangeAct = ExtensionsSettings.Instance.CurrencyExchangeAct;
			if (currencyExchangeAct == "Random")
			{
				int num = LokiPoe.Random.Next(3, act + 1);
				return areaInfo_0[num];
			}
			int num2 = int.Parse(currencyExchangeAct);
			return (num2 <= act) ? areaInfo_0[num2] : areaInfo_0[act];
		}
		return null;
	}

	static Class38()
	{
		list_0 = new List<Class45>();
		dictionary_0 = new Dictionary<string, int>
		{
			[CurrencyNames.Augmentation] = 1,
			[CurrencyNames.Alteration] = 2,
			[CurrencyNames.Jeweller] = 3,
			[CurrencyNames.Fusing] = 4,
			[CurrencyNames.Scouring] = 5,
			[CurrencyNames.Regret] = 6,
			[CurrencyNames.Alchemy] = 7
		};
		dictionary_1 = new Dictionary<string, Class45>
		{
			[CurrencyNames.Transmutation] = new Class45(CurrencyNames.Augmentation, 4),
			[CurrencyNames.Augmentation] = new Class45(CurrencyNames.Alteration, 4),
			[CurrencyNames.Alteration] = new Class45(CurrencyNames.Jeweller, 2),
			[CurrencyNames.Jeweller] = new Class45(CurrencyNames.Fusing, 4),
			[CurrencyNames.Chance] = new Class45(CurrencyNames.Scouring, 4),
			[CurrencyNames.Scouring] = new Class45(CurrencyNames.Regret, 2),
			[CurrencyNames.Regret] = new Class45(CurrencyNames.Alchemy, 1)
		};
		areaInfo_0 = new AreaInfo[12]
		{
			null,
			World.Act1.LioneyeWatch,
			World.Act2.ForestEncampment,
			World.Act3.SarnEncampment,
			World.Act4.Highgate,
			World.Act5.OverseerTower,
			World.Act6.LioneyeWatch,
			World.Act7.BridgeEncampment,
			World.Act8.SarnEncampment,
			World.Act9.Highgate,
			World.Act10.OriathDocks,
			World.Act11.Oriath
		};
	}
}
