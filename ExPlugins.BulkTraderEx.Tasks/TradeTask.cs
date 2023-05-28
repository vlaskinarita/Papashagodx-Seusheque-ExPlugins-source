using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BulkTraderEx.Classes;
using ExPlugins.BulkTraderEx.Helpers;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx;

namespace ExPlugins.BulkTraderEx.Tasks;

public class TradeTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static readonly Stopwatch TaskSw;

	private static readonly Interval interval_0;

	private static readonly BulkTraderExSettings Config;

	private static bool bool_0;

	private static bool bool_1;

	public string Name => "TradeTask";

	public string Description => "A Task that perform currency exchange.";

	public string Author => "Alcor75";

	public string Version => "1.0.0.0";

	public void Tick()
	{
		if (interval_0.Elapsed && !Config.ShouldTrade)
		{
			Config.ShouldTrade = TaskSw.Elapsed.TotalMinutes >= (double)BulkTraderEx.PluginCooldown && !GeneralSettings.Instance.IsOnRun;
		}
	}

	public MessageResult Message(Message message)
	{
		if (Config.ShouldTrade && message.Id == "finished_stashing_in_tab")
		{
			string input = message.GetInput<string>(0);
			GlobalLog.Warn("[BulkCache] Finished stashing in " + input + ". Updating cache!");
			BulkTraderExData.UpdateCurrentTab();
		}
		return (MessageResult)1;
	}

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (Config.ShouldTrade)
			{
				if (LokiPoe.CurrentWorldArea.IsTown || LokiPoe.CurrentWorldArea.IsHideoutArea)
				{
					if (!(((IAuthored)BotManager.Current).Name == "LabRunner"))
					{
						if (((Player)LokiPoe.Me).Level < 30)
						{
							return false;
						}
						if (bool_0)
						{
							bool_1 = true;
							bool_0 = false;
						}
						if (await BulkTraderExData.UpdateItemsInTabs(bool_1))
						{
							bool_1 = false;
						}
						List<TradeRecipe> triggeredRecipes = SelectRecipe();
						if (triggeredRecipes.Any())
						{
							foreach (TradeRecipe triggeredRecipe in triggeredRecipes)
							{
								string recipeName = triggeredRecipe.HaveName + " to " + triggeredRecipe.WantName;
								if (BulkTraderEx.BlacklistedRecipes.ContainsKey(recipeName))
								{
									long elapsedMilliseconds = BulkTraderEx.BlacklistedRecipes[recipeName].ElapsedMilliseconds;
									long remainingMs = 1800000L - elapsedMilliseconds;
									GlobalLog.Warn(string.Format(arg2: TimeSpan.FromMilliseconds(remainingMs), format: "[{0}] The recipe {1} is blacklisted. [Recheck in {2:mm\\:ss}]", arg0: Name, arg1: recipeName));
									continue;
								}
								triggeredRecipe.Prices?.Clear();
								TradeRecipe tradeRecipe = triggeredRecipe;
								tradeRecipe.Prices = await RequestPriceCheck(triggeredRecipe);
								if (triggeredRecipe.Prices == null || !triggeredRecipe.Prices.Any())
								{
									GlobalLog.Error("[" + Name + "] Found no prices for " + recipeName + ".");
									BulkTraderEx.BlacklistedRecipes.Add(recipeName, Stopwatch.StartNew());
									continue;
								}
								if (!(await CheckAndBuy(triggeredRecipe)))
								{
									if (!BulkTraderEx.BlacklistedRecipes.ContainsKey(recipeName))
									{
										GlobalLog.Warn("[" + Name + "] Adding " + recipeName + " to recipe blacklist for 30 minutes.");
										BulkTraderEx.BlacklistedRecipes.Add(recipeName, Stopwatch.StartNew());
									}
									return true;
								}
								return true;
							}
						}
						else
						{
							GlobalLog.Error("[" + Name + "] No usable recipe found to exchange currency.");
						}
						GlobalLog.Warn("[" + Name + "] Done.");
						BulkTraderEx.PluginCooldown = Config.PluginCooldown + LokiPoe.Random.Next(-15, 30);
						bool_0 = true;
						Config.ShouldTrade = false;
						TaskSw.Restart();
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static List<TradeRecipe> SelectRecipe()
	{
		List<TradeRecipe> list = new List<TradeRecipe>();
		foreach (TradeRecipe item in from r in Config.Recipes
			orderby r.Priority descending
			select r into x
			where x.IsEnabled
			select x)
		{
			string string_0 = item.HaveName;
			string string_1 = item.WantName;
			string text = item.HaveName + " to " + item.WantName;
			if (BulkTraderEx.BlacklistedRecipes.ContainsKey(text))
			{
				long elapsedMilliseconds = BulkTraderEx.BlacklistedRecipes[text].ElapsedMilliseconds;
				long num = 1800000L - elapsedMilliseconds;
				TimeSpan timeSpan = TimeSpan.FromMilliseconds(num);
				GlobalLog.Debug($"[SelectRecipe] The recipe {text} is blacklisted. [Recheck in {timeSpan:mm\\:ss}]");
				continue;
			}
			if (string_0.Contains("Tier "))
			{
				string[] array = string_0.Split(new char[1] { ' ' }, 2);
				array = array[1].Split(new char[1] { ' ' }, 2);
				string_0 = array[1];
				GlobalLog.Debug("New haveFullName = " + string_0);
			}
			if (string_1.Contains("Tier "))
			{
				string[] array2 = string_1.Split(new char[1] { ' ' }, 2);
				array2 = array2[1].Split(new char[1] { ' ' }, 2);
				string_1 = array2[1];
				GlobalLog.Debug("New wantFullName = " + string_1);
			}
			int num2 = BulkTraderExData.CachedItemsInStash.Where((CachedItemObject c) => c.Name == string_0).Sum((CachedItemObject x) => x.StackCount);
			int num3 = num2 - item.AmountToKeep;
			if (num3 <= 0)
			{
				continue;
			}
			int num4 = BulkTraderExData.CachedItemsInStash.Where((CachedItemObject c) => c.Name == string_1).Sum((CachedItemObject x) => x.StackCount);
			if (item.MaxToBuy < 0 || num4 < item.MaxToBuy)
			{
				double num5 = PoeNinjaTracker.LookupCurrencyChaosValueByFullName(string_0) * (double)num3 * 0.93;
				if (string_1.ContainsIgnorecase("chaos") && num5 < (double)item.MinToBuy)
				{
					GlobalLog.Debug($"[StaticRecipe] Skipping low value recipe: {text}. Value: {num5:0.0}c<{item.MinToBuy}c");
					continue;
				}
				GlobalLog.Warn($"[StaticRecipe] Adding {text}. Value: {num5}c");
				list.Add(item);
			}
		}
		return list;
	}

	public static async Task<List<TradeCurrency>> RequestPriceCheck(TradeRecipe triggeredCurrency)
	{
		string league = "Sanctum";
		if (LokiPoe.IsInGame)
		{
			league = LokiPoe.Me.League;
		}
		return CurrencyHelper.ParseOfficialRequest(await WebHelper.Bulk(triggeredCurrency.HaveName, triggeredCurrency.WantName, league, triggeredCurrency.MinToBuy), triggeredCurrency.HaveName, triggeredCurrency.WantName);
	}

	private static async Task<bool> CheckAndBuy(TradeRecipe triggeredRecipe)
	{
		string string_0 = triggeredRecipe.HaveName;
		string string_1 = triggeredRecipe.WantName;
		if (string_0.Contains("Tier "))
		{
			string[] asd2 = string_0.Split(new char[1] { ' ' }, 2);
			asd2 = asd2[1].Split(new char[1] { ' ' }, 2);
			string_0 = asd2[1];
			GlobalLog.Debug("New haveFullName = " + string_0);
		}
		if (string_1.Contains("Tier "))
		{
			string[] asd4 = string_1.Split(new char[1] { ' ' }, 2);
			asd4 = asd4[1].Split(new char[1] { ' ' }, 2);
			string_1 = asd4[1];
			GlobalLog.Debug("New weBuyFullName = " + string_1);
		}
		int totalSellCurrencyCount = BulkTraderExData.CachedItemsInStash.Where((CachedItemObject c) => c.Name == string_0).Sum((CachedItemObject x) => x.StackCount);
		int totalWeBuyCurrencyCount = BulkTraderExData.CachedItemsInStash.Where((CachedItemObject c) => c.Name == string_1).Sum((CachedItemObject x) => x.StackCount);
		int int_0 = totalSellCurrencyCount - triggeredRecipe.AmountToKeep;
		List<TradeCurrency> usablePrices = triggeredRecipe.Prices.Where((TradeCurrency x) => x.MinBuy <= (double)int_0).ToList();
		if (!usablePrices.Any())
		{
			return false;
		}
		double coef2 = usablePrices.Sum((TradeCurrency p) => p.MinSell / p.MinBuy);
		coef2 /= (double)usablePrices.Count;
		for (int j = usablePrices.Count - 1; j >= 0; j--)
		{
			TradeCurrency usablePrice = usablePrices[j];
			if (Math.Abs(coef2 - usablePrice.MinSell / usablePrice.MinBuy) > coef2 * 0.5)
			{
				usablePrices.RemoveAt(j);
			}
		}
		for (int i = usablePrices.Count - 1; i >= 0; i--)
		{
			TradeCurrency usablePrice2 = usablePrices[i];
			int multiplier = 1;
			if (usablePrice2.MinBuy < (double)int_0)
			{
				int sellItemMaxStack = CurrencyHelper.GetCurrencyMaxStackFromName(triggeredRecipe.HaveName);
				for (int buyItemMaxStack = CurrencyHelper.GetCurrencyMaxStackFromName(triggeredRecipe.WantName); !((double)(multiplier + 1) * usablePrice2.MinBuy / (double)sellItemMaxStack > (double)(InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares - 3)) && !((double)(multiplier + 1) * usablePrice2.MinSell / (double)buyItemMaxStack > (double)(InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares - 3)) && !((double)(multiplier + 1) * usablePrice2.MinSell > (double)usablePrice2.Stock) && !((double)(multiplier + 1) * usablePrice2.MinBuy > (double)int_0) && !((double)(multiplier + 1) * usablePrice2.MinSell > (double)(triggeredRecipe.MaxToBuy - totalWeBuyCurrencyCount)); multiplier++)
				{
				}
				usablePrice2.MinBuy *= multiplier;
				usablePrice2.MinSell *= multiplier;
			}
			if (TradeHelpers.IsInteger(usablePrice2.MinBuy))
			{
				if (!TradeHelpers.IsInteger(usablePrice2.MinSell))
				{
					usablePrices.RemoveAt(i);
				}
				else if (!(usablePrice2.MinBuy / (double)CurrencyHelper.GetCurrencyMaxStackFromName(triggeredRecipe.HaveName) > (double)(InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares - 3)))
				{
					if (!(usablePrice2.MinSell / (double)CurrencyHelper.GetCurrencyMaxStackFromName(triggeredRecipe.WantName) > (double)(InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares - 3)))
					{
						if (!(usablePrice2.MinSell < (double)triggeredRecipe.MinToBuy))
						{
							if (usablePrice2.MinSell > (double)(triggeredRecipe.MaxToBuy - totalWeBuyCurrencyCount))
							{
								usablePrices.RemoveAt(i);
							}
						}
						else
						{
							usablePrices.RemoveAt(i);
						}
					}
					else
					{
						usablePrices.RemoveAt(i);
					}
				}
				else
				{
					usablePrices.RemoveAt(i);
				}
			}
			else
			{
				usablePrices.RemoveAt(i);
			}
		}
		GlobalLog.Warn(string.Format(arg1: triggeredRecipe.HaveName + " to " + triggeredRecipe.WantName, format: "[CheckAndBuy] Found {0} usable prices for [{1}]", arg0: usablePrices.Count));
		if (!usablePrices.Any())
		{
			return false;
		}
		return await RequestAndBuy(usablePrices);
	}

	private static async Task<bool> RequestAndBuy(IList<TradeCurrency> usablePrices)
	{
		Utility.BroadcastMessage((object)null, "SuspendTrades", new object[1] { "" });
		Stopwatch timer = Stopwatch.StartNew();
		List<TradeCurrency> alreadyRequested = new List<TradeCurrency>();
		int requestsCount = 0;
		int randReq = new Random().Next(2, 5);
		Interval requestInterval = new Interval(2000);
		while (true)
		{
			await Wait.SleepSafe(50, 150);
			if (timer.Elapsed.TotalSeconds > 20.0)
			{
				break;
			}
			if (await Class51.AcceptParty(alreadyRequested))
			{
				await Wait.Sleep(1);
			}
			TradeCurrency actualTradeCurrency = alreadyRequested.FirstOrDefault((TradeCurrency x) => InstanceInfo.PartyMembers.Any((PartyMember y) => y.PlayerEntry.Name == x.SellerIgn));
			if (actualTradeCurrency == null)
			{
				if (requestInterval.Elapsed && usablePrices.Count != 0)
				{
					GlobalLog.Debug($"[TradeTask] usablePrices.Count: {usablePrices.Count}");
					TradeCurrency tradeRequestEntry = usablePrices[0];
					usablePrices.RemoveAt(0);
					alreadyRequested.Add(tradeRequestEntry);
					if (Config.UseSsid && !string.IsNullOrEmpty(Config.Poesessid) && !string.IsNullOrEmpty(tradeRequestEntry.WhisperToken))
					{
						await WebHelper.DirectWhisper((int)Math.Floor(tradeRequestEntry.MinSell / (double)tradeRequestEntry.Bulk.MinSell), tradeRequestEntry.WhisperToken);
					}
					else
					{
						string msg = string.Format(tradeRequestEntry.Whisper, $"{tradeRequestEntry.MinSell} {tradeRequestEntry.Sellcurrency}", $"{tradeRequestEntry.MinBuy} {tradeRequestEntry.Buycurrency}");
						GlobalLog.Debug("[TradeTask] Sending msg: " + msg);
						await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg(msg);
					}
					if ((ChatPanel.Messages.Any((ChatEntry m) => m.Message.Contains("DND mode is now ON")) || ChatPanel.Messages.Any((ChatEntry m) => m.Message.Contains("DND mode is active"))) && !World.CurrentArea.IsMap)
					{
						GlobalLog.Warn("[TradeTask] Disabling DND");
						await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("/dnd");
						await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("/clear");
					}
					if (!BulkTraderEx.BlacklistedSellers.ContainsKey(tradeRequestEntry.SellerIgn))
					{
						BulkTraderEx.BlacklistedSellers.Add(tradeRequestEntry.SellerIgn, Stopwatch.StartNew());
					}
					requestsCount++;
					int rand = LokiPoe.Random.Next(500, 2000);
					if (requestsCount >= randReq)
					{
						GlobalLog.Debug($"[TradeTask] Next request in {rand * 6}ms");
						requestInterval = new Interval(rand * 6);
						randReq += 2;
						requestsCount = 0;
					}
					else
					{
						GlobalLog.Debug($"[TradeTask] Next request in {rand}ms");
						requestInterval = new Interval(2 * rand);
					}
					timer.Restart();
				}
				continue;
			}
			return await Buy(actualTradeCurrency);
		}
		return false;
	}

	private static async Task<bool> Buy(TradeCurrency actualTradeCurrency)
	{
		Customers customer = new Customers
		{
			CurrencyHeWantPotCode = actualTradeCurrency.Buycurrency,
			CurrencyHeWantQuantity = (int)actualTradeCurrency.MinBuy,
			CurrencyHeSellPotCode = actualTradeCurrency.Sellcurrency,
			CurrencyHeSellQuantity = (int)actualTradeCurrency.MinSell
		};
		if (!(await TradeHelpers.CurrencyToInventory(customer)))
		{
			GlobalLog.Error("[TradeTask-Buy] Unable to Load the needed currency.");
			await Class51.LeaveGroup();
			return false;
		}
		if (!(await Class51.GotoPartyZone(actualTradeCurrency.SellerIgn)))
		{
			GlobalLog.Error("[TradeTask-Buy] Failed to go to Customer Zone, disband and continue.");
			await Class51.LeaveGroup();
			if (await PlayerAction.GoToHideout())
			{
				return true;
			}
			GlobalLog.Error("[TradeTask-Buy] Unable to Leave the customer hideout, Logging out.");
			EscapeState.LogoutToCharacterSelection();
			return true;
		}
		await Coroutines.ReactionWait();
		Stopwatch sw = Stopwatch.StartNew();
		string string_1 = actualTradeCurrency.SellerAccount;
		string string_2 = actualTradeCurrency.SellerIgn;
		string string_0;
		try
		{
			string_0 = CurrencyHelper.GetCurrencyExactNameFromInt(int.Parse(customer.CurrencyHeSellPotCode));
		}
		catch
		{
			string_0 = customer.CurrencyHeSellPotCode;
		}
		decimal heSellQuantity = customer.CurrencyHeSellQuantity;
		string heBuyExactName;
		try
		{
			heBuyExactName = CurrencyHelper.GetCurrencyExactNameFromInt(int.Parse(customer.CurrencyHeWantPotCode));
		}
		catch
		{
			heBuyExactName = customer.CurrencyHeWantPotCode;
		}
		decimal heBuyQuantity = customer.CurrencyHeWantQuantity;
		int int_0 = 0;
		int counter1 = 0;
		if (string_0.Contains("Tier "))
		{
			string[] asd2 = string_0.Split(new char[1] { ' ' }, 2);
			asd2 = asd2[1].Split(new char[1] { ' ' }, 2);
			string_0 = asd2[1];
			GlobalLog.Debug("New heSellName = " + string_0);
		}
		List<Item> itemToBuyPreTrade = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item it) => it.Name == string_0).ToList();
		while (sw.ElapsedMilliseconds < 100000L)
		{
			if (int_0 <= 1)
			{
				try
				{
					if (!LokiPoe.CurrentWorldArea.IsTown && !LokiPoe.CurrentWorldArea.IsHideoutArea)
					{
						GlobalLog.Debug("[TradeTask-Buy] We are not in Town or HideOut, we probably entered a map portal, getting out.");
						await PlayerAction.TpToTown();
					}
					if (!NotificationHud.IsOpened)
					{
						goto IL_0745;
					}
					await Wait.SleepSafe(1500, 2000);
					HandleNotificationResult result = NotificationHud.HandleNotificationEx((ProcessNotificationEx)delegate(NotificationData x, NotificationType y)
					{
						//IL_0037: Unknown result type (might be due to invalid IL or missing references)
						//IL_0064: Unknown result type (might be due to invalid IL or missing references)
						bool flag = x.AccountName == string_1 || x.CharacterName == string_2;
						GlobalLog.Warn($"[TradeTask] Detected {y} request from char: {x.CharacterName} [AccountName: {x.AccountName}] Accepting? {flag}");
						if ((int)y == 0 && x.AccountName != string_1 && x.CharacterName != string_2)
						{
							int_0++;
						}
						return flag;
					}, true);
					if ((int)result != 0)
					{
						goto IL_0745;
					}
					counter1++;
					if (counter1 <= 2)
					{
						goto IL_0745;
					}
					await Wait.Sleep(LokiPoe.Random.Next(3, 15));
					goto end_IL_05a8;
					IL_0745:
					if (TradeUi.IsOpened)
					{
						await TradeHelpers.CurrencyToTrade(customer);
						switch (await TradeHelpers.CheckAndAcceptTrade(customer))
						{
						case Results.CheckCustomerOffertAndAcceptResult.WrongItem:
							return false;
						default:
							await Coroutines.LatencyWait();
							await Coroutines.ReactionWait();
							break;
						case Results.CheckCustomerOffertAndAcceptResult.None:
							break;
						}
						await Coroutines.LatencyWait();
					}
					List<Item> itemToBuyPostTrade = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item it) => it.Name == string_0).ToList();
					if ((decimal)(itemToBuyPostTrade.Sum((Item x) => x.StackCount) - itemToBuyPreTrade.Sum((Item x) => x.StackCount)) < heSellQuantity)
					{
						continue;
					}
					GlobalLog.Debug($"[TradeTask-Buy] Successfully bought {heSellQuantity} {string_0} for {heBuyQuantity} {heBuyExactName}.");
					end_IL_05a8:;
				}
				catch (Exception)
				{
					continue;
				}
			}
			else
			{
				await Wait.Sleep(1358);
				await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("@" + string_2 + " sry");
			}
			break;
		}
		await Class51.LeaveGroup();
		if (!(await PlayerAction.GoToHideout()))
		{
			GlobalLog.Error("[TradeTask-Buy] Unable to Leave the customer hideout, Logging out.");
			EscapeState.LogoutToCharacterSelection();
			return true;
		}
		return true;
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

	public override string ToString()
	{
		return Name + ": " + Description;
	}

	static TradeTask()
	{
		TaskSw = Stopwatch.StartNew();
		interval_0 = new Interval(500);
		Config = BulkTraderExSettings.Instance;
	}
}
