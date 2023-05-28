using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BulkTraderEx.Classes;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx;

namespace ExPlugins.BulkTraderEx.Helpers;

public static class TradeHelpers
{
	public delegate bool FindItemDelegate(Item item);

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static ProcessNotificationEx processNotificationEx_0;

		public static Func<NotificationInfo, bool> func_0;

		public static Func<CachedItemObject, int> func_1;

		public static Func<Item, int> func_2;

		public static Func<Item, int> func_3;

		public static Func<Item, int> func_4;

		public static Func<Item, int> func_5;

		public static Func<Item, int> func_6;

		public static Func<Item, int> func_7;

		public static Func<Item, int> func_8;

		public static Func<Item, int> func_9;

		public static ProcessNotificationEx processNotificationEx_1;

		public static Func<Item, int> func_10;

		public static Func<Item, bool> func_11;

		public static Func<bool> func_12;

		public static Func<Item, int> func_13;

		public static Func<Item, int> func_14;

		public static Func<Item, int> func_15;

		public static Func<Item, int> func_16;

		public static Func<Item, int> func_17;

		public static Func<Item, int> func_18;

		public static Func<Item, int> func_19;

		public static Func<Item, int> func_20;

		public static Func<Item, int> func_21;

		public static Func<Item, int> func_22;

		public static Func<Item, int> func_23;

		public static Func<Item, int> func_24;

		public static Func<Item, int> func_25;

		public static Func<Item, int> func_26;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003CCloseAllNotifications_003Eb__2_0(NotificationData x, NotificationType y)
		{
			GlobalLog.Debug("[CloseAllNotifications] cancelling trade request from impatient trader");
			return false;
		}

		internal bool _003CCloseAllNotifications_003Eb__2_1(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal int _003CCurrencyToInventory_003Eb__3_1(CachedItemObject i)
		{
			return i.StackCount;
		}

		internal int _003CCurrencyToInventory_003Eb__3_3(Item i)
		{
			return i.StackCount;
		}

		internal int _003CCurrencyToInventory_003Eb__3_8(Item i)
		{
			return i.StackCount;
		}

		internal int _003CCurrencyToInventory_003Eb__3_5(Item item)
		{
			return item.StackCount;
		}

		internal int _003CCurrencyToInventory_003Eb__3_12(Item item)
		{
			return item.StackCount;
		}

		internal int _003CCurrencyToTrade_003Eb__4_0(Item i)
		{
			return i.StackCount;
		}

		internal int _003CCurrencyToTrade_003Eb__4_1(Item x)
		{
			return x.StackCount;
		}

		internal int _003CCurrencyToTrade_003Eb__4_2(Item x)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return x.LocationTopLeft.X;
		}

		internal int _003CCurrencyToTrade_003Eb__4_3(Item x)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return x.LocationTopLeft.Y;
		}

		internal bool _003CCurrencyToTrade_003Eb__4_4(NotificationData x, NotificationType y)
		{
			GlobalLog.Debug("[CurrencyToTrade] cancelling trade request from impatient trader " + x.CharacterName);
			return false;
		}

		internal int _003CCurrencyToTrade_003Eb__4_6(Item i)
		{
			return i.StackCount;
		}

		internal bool _003CCheckAndAcceptTrade_003Eb__5_0(Item it)
		{
			return TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(it.LocalId);
		}

		internal bool _003CCheckAndAcceptTrade_003Eb__5_2()
		{
			return TradeUi.IsOpened;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_4(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_6(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_8(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_10(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_12(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_14(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_16(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_18(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_20(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_22(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_24(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_26(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_28(Item it)
		{
			return it.StackCount;
		}

		internal int _003CCheckAndAcceptTrade_003Eb__5_30(Item it)
		{
			return it.StackCount;
		}
	}

	private static readonly Random random_0;

	private static void CloseAllNotifications()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		while (NotificationHud.NotificationList.Any((NotificationInfo x) => x.IsVisible))
		{
			try
			{
				object obj = _003C_003Ec.processNotificationEx_0;
				if (obj == null)
				{
					ProcessNotificationEx val = delegate
					{
						GlobalLog.Debug("[CloseAllNotifications] cancelling trade request from impatient trader");
						return false;
					};
					obj = (object)val;
					_003C_003Ec.processNotificationEx_0 = val;
				}
				NotificationHud.HandleNotificationEx((ProcessNotificationEx)obj, true);
			}
			catch (Exception)
			{
				GlobalLog.Debug("[CloseAllNotifications] Error suppressed!");
			}
		}
	}

	public static async Task<bool> CurrencyToInventory(Customers customer)
	{
		string string_0;
		try
		{
			string_0 = CurrencyHelper.GetCurrencyExactNameFromInt(int.Parse(customer.CurrencyHeWantPotCode));
		}
		catch
		{
			string_0 = customer.CurrencyHeWantPotCode;
		}
		List<CachedItemObject> namedCurrencyInCache = BulkTraderExData.CachedItemsInStash.Where((CachedItemObject i) => i.Name == string_0).ToList();
		if (namedCurrencyInCache.Any())
		{
			int namedCurrencyInCacheSum = namedCurrencyInCache.Sum((CachedItemObject i) => i.StackCount);
			if (!((decimal)namedCurrencyInCacheSum < customer.CurrencyHeWantQuantity))
			{
				int withdrawnQuantity = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Name == string_0).Sum((Item i) => i.StackCount);
				while ((decimal)withdrawnQuantity < customer.CurrencyHeWantQuantity)
				{
					CachedItemObject it = BulkTraderExData.CachedItemsInStash.FirstOrDefault((CachedItemObject i) => i.Name == string_0);
					CloseAllNotifications();
					if (it != null)
					{
						await Inventories.OpenStashTab(it.TabName, "CurrencyToInventory");
						if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(it.Size))
						{
							GlobalLog.Debug("[CurrencyToInventory] Can't fit anymore in inventory, stopping");
							return false;
						}
						int toTake = Math.Min((int)customer.CurrencyHeWantQuantity - withdrawnQuantity, it.MaxStackCount);
						int retry = 0;
						while (!(await Inventories.SplitAndPlaceItemInMainInventory(it.Name, toTake)))
						{
							retry++;
							if (retry < 3)
							{
								CloseAllNotifications();
								await Coroutines.LatencyWait();
								continue;
							}
							GlobalLog.Error("[CurrencyToInventory] FastMove returned false");
							return false;
						}
					}
					withdrawnQuantity = Inventories.InventoryItems.Where((Item i) => i.Name == string_0).Sum((Item i) => i.StackCount);
					GlobalLog.Debug($"[CurrencyToInventory] We've withdrawn {withdrawnQuantity}/{customer.CurrencyHeWantQuantity} {string_0}.");
				}
				int itemQuantity = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item item) => item.Name == string_0).Sum((Item item) => item.StackCount);
				Stopwatch sw = Stopwatch.StartNew();
				while ((decimal)itemQuantity > customer.CurrencyHeWantQuantity)
				{
					CloseAllNotifications();
					if (sw.ElapsedMilliseconds <= 10000L)
					{
						decimal decimal_0 = (decimal)itemQuantity - customer.CurrencyHeWantQuantity;
						Item itemtoSplit = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item i) => i.Name == string_0 && (decimal)i.StackCount > decimal_0);
						if ((RemoteMemoryObject)(object)itemtoSplit != (RemoteMemoryObject)null)
						{
							await Inventories.SplitAndPlaceItemInMainInventory(InventoryUi.InventoryControl_Main, itemtoSplit, (int)decimal_0);
						}
						if ((RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null)
						{
							await Inventories.ClearCursorLite();
						}
						Item sItem = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item i) => i.Name == string_0 && (decimal)i.StackCount == decimal_0);
						if ((RemoteMemoryObject)(object)sItem != (RemoteMemoryObject)null && await Inventories.FastMoveFromInventory(sItem.LocationTopLeft))
						{
							await BulkTraderExData.UpdateItemsInTabs(force: false, new List<string> { StashUi.TabControl.CurrentTabName });
						}
						itemQuantity = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item item) => item.Name == string_0).Sum((Item item) => item.StackCount);
						continue;
					}
					GlobalLog.Debug("[CurrencyToInventory] We are unable to split " + string_0);
					return false;
				}
				return true;
			}
			GlobalLog.Debug($"[CurrencyToInventory] There's only {namedCurrencyInCacheSum} {string_0} in stash ({customer.CurrencyHeWantQuantity} requested) aborting...");
			return false;
		}
		GlobalLog.Debug("[CurrencyToInventory] There's no currency name \"" + string_0 + "\" in cache.");
		return false;
	}

	public static async Task<bool> CurrencyToTrade(Customers customer)
	{
		using (new InputDelayOverride(5))
		{
			string string_0;
			try
			{
				string_0 = CurrencyHelper.GetCurrencyExactNameFromInt(int.Parse(customer.CurrencyHeWantPotCode));
			}
			catch
			{
				string_0 = customer.CurrencyHeWantPotCode;
			}
			List<Item> namedCurrencyInInv = await FindItems(string_0);
			int namedCurrencyInInvSum = namedCurrencyInInv.Sum((Item i) => i.StackCount);
			if (!((decimal)namedCurrencyInInvSum < customer.CurrencyHeWantQuantity))
			{
				if (TradeUi.IsOpened)
				{
					int humanBeingWait = (int)MathEx.Random(2.0, 4.0) * 250;
					GlobalLog.Info($"Waiting for {humanBeingWait} ms to be seen like human when the window is opened.");
					await Coroutine.Sleep(humanBeingWait);
					foreach (Item it in from x in namedCurrencyInInv
						orderby x.StackCount descending, x.LocationTopLeft.X, x.LocationTopLeft.Y
						select x)
					{
						while (TradeUi.IsOpened)
						{
							if (InventoryUi.InventoryControl_Main.IsItemTransparent(it.LocalId))
							{
								GlobalLog.Warn($"{it.Name}:{it.LocalId} is transparent!");
							}
							else
							{
								if (NotificationHud.IsOpened)
								{
									try
									{
										object obj2 = _003C_003Ec.processNotificationEx_1;
										if (obj2 == null)
										{
											ProcessNotificationEx val = delegate(NotificationData x, NotificationType y)
											{
												GlobalLog.Debug("[CurrencyToTrade] cancelling trade request from impatient trader " + x.CharacterName);
												return false;
											};
											obj2 = (object)val;
											_003C_003Ec.processNotificationEx_1 = val;
										}
										NotificationHud.HandleNotificationEx((ProcessNotificationEx)obj2, true);
									}
									catch (Exception)
									{
										GlobalLog.Debug("[CurrencyToTrade] Error suppressed!");
									}
									await Coroutines.LatencyWait();
									continue;
								}
								int quantityAlreadyinTrade = TradeUi.TradeControl.InventoryControl_YourOffer.Inventory.Items.Where((Item i) => i.Name == string_0).Sum((Item i) => i.StackCount);
								if ((decimal)quantityAlreadyinTrade == customer.CurrencyHeWantQuantity)
								{
									return true;
								}
								int sleepAmount = random_0.Next(100, 500);
								int rand = random_0.Next(1, 100);
								int retrycount = 0;
								while (true)
								{
									FastMoveResult moved = InventoryUi.InventoryControl_Main.FastMove(it.LocalId, true, false);
									if (rand > 95)
									{
										GlobalLog.Debug($"[Random Sleep] {sleepAmount}ms");
										await Wait.SleepSafe(sleepAmount);
									}
									if ((int)moved <= 0)
									{
										break;
									}
									retrycount++;
									if (retrycount <= 3)
									{
										GlobalLog.Debug($"[CurrencyToTrade] FastMoveToVendor returned false, retry [{retrycount}/3].");
										continue;
									}
									GlobalLog.Debug("[CurrencyToTrade] FastMoveToVendor failed 3 times, aborting.");
									return false;
								}
							}
							goto IL_050f;
						}
						break;
						IL_050f:;
					}
					return true;
				}
				GlobalLog.Debug("[CurrencyToTrade] The trade interface is not open, aborting...");
				return false;
			}
			GlobalLog.Debug($"[CurrencyToTrade] There's only {namedCurrencyInInvSum} {string_0} in inventory ({customer.CurrencyHeWantQuantity} requested) aborting...");
			return false;
		}
	}

	public static async Task<Results.CheckCustomerOffertAndAcceptResult> CheckAndAcceptTrade(Customers customer)
	{
		int trynumber = 0;
		Stopwatch timeoutSw = Stopwatch.StartNew();
		string string_0;
		try
		{
			string_0 = CurrencyHelper.GetCurrencyExactNameFromInt(int.Parse(customer.CurrencyHeSellPotCode));
		}
		catch
		{
			string_0 = customer.CurrencyHeSellPotCode;
		}
		if (string_0.Contains("Tier "))
		{
			string[] asd = string_0.Split(new char[1] { ' ' }, 2);
			asd = asd[1].Split(new char[1] { ' ' }, 2);
			string_0 = asd[1];
			GlobalLog.Debug("New heSellName = " + string_0);
		}
		decimal heSellQuantity = customer.CurrencyHeSellQuantity;
		int timer = LokiPoe.Random.Next(40000, 60000);
		while (trynumber <= 2)
		{
			if (TradeUi.IsOpened)
			{
				GlobalLog.Debug($"[CheckAndAcceptTrade] Try: {trynumber} - elapsed ms: {timeoutSw.ElapsedMilliseconds:N1}");
				if (timeoutSw.ElapsedMilliseconds > timer)
				{
					trynumber++;
					try
					{
						if (TradeUi.IsOpened)
						{
							GlobalLog.Error("[CheckAndAcceptTrade] Cancelling the trade.");
							TradeUi.TradeControl.Cancel(true);
							await Coroutine.Sleep(3000);
						}
						await Coroutines.CloseBlockingWindows();
						await Coroutines.LatencyWait();
					}
					catch (Exception)
					{
						await Coroutines.LatencyWait();
					}
					continue;
				}
				Stopwatch sw = Stopwatch.StartNew();
				try
				{
					while (TradeUi.IsOpened)
					{
						await Coroutines.LatencyWait();
						if (!TradeUi.IsOpened || sw.ElapsedMilliseconds > 20000L)
						{
							break;
						}
						if ((RemoteMemoryObject)(object)TradeUi.TradeControl != (RemoteMemoryObject)null)
						{
							try
							{
								TradeControlWrapper tradeControl = TradeUi.TradeControl;
								foreach (Item item in (tradeControl == null) ? null : tradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item it) => TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(it.LocalId)))
								{
									if (!(TradeUi.TradeControl.AcceptButtonText != "accept") || !(TradeUi.TradeControl.AcceptButtonText != "accept (0)"))
									{
										int int_0 = item.LocalId;
										TradeControlWrapper tradeControl2 = TradeUi.TradeControl;
										if (tradeControl2 != null)
										{
											tradeControl2.InventoryControl_OtherOffer.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory inv, Item itm) => itm.LocalId == int_0), (Func<bool>)(() => TradeUi.IsOpened));
										}
										continue;
									}
									await Coroutines.LatencyWait();
									await Coroutine.Sleep(2000);
									goto end_IL_075e;
								}
								goto IL_09a9;
								end_IL_075e:;
							}
							catch (Exception)
							{
								GlobalLog.Debug("[CheckAndAcceptTrade] TradeControl not ready.");
								await Coroutines.LatencyWait();
								continue;
							}
							goto IL_03bd;
						}
						GlobalLog.Error("[CheckAndAcceptTrade] Trade Controll is null");
						continue;
						IL_09a9:
						if (TradeUi.IsOpened && ((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) == heSellQuantity || (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName + " " + i.Name == string_0).Sum((Item it) => it.StackCount) == heSellQuantity || (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName == string_0).Sum((Item it) => it.StackCount) == heSellQuantity))
						{
							await Coroutines.ReactionWait();
							if (!TradeUi.IsOpened || TradeUi.TradeControl.ConfirmLabelText == "")
							{
								break;
							}
						}
						else if (!TradeUi.IsOpened || (!((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) > heSellQuantity) && !((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName + " " + i.Name == string_0).Sum((Item it) => it.StackCount) > heSellQuantity) && !((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName == string_0).Sum((Item it) => it.StackCount) > heSellQuantity)))
						{
							if (TradeUi.IsOpened && (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) >= heSellQuantity * 0.97m && (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name != string_0).Sum((Item it) => it.StackCount) >= heSellQuantity - heSellQuantity * 0.97m)
							{
								await Coroutines.ReactionWait();
								if (!TradeUi.IsOpened || TradeUi.TradeControl.ConfirmLabelText == "")
								{
									break;
								}
							}
						}
						else
						{
							await Coroutines.ReactionWait();
							if (!TradeUi.IsOpened || TradeUi.TradeControl.ConfirmLabelText == "")
							{
								break;
							}
						}
					}
				}
				catch (Exception)
				{
					await Coroutines.LatencyWait(4f);
					await Coroutines.ReactionWait();
					TradeUi.TradeControl.Cancel(true);
					await Coroutines.LatencyWait();
				}
				sw = Stopwatch.StartNew();
				while (TradeUi.IsOpened && !((RemoteMemoryObject)TradeUi.TradeControl).IsValid && sw.ElapsedMilliseconds <= 15000L)
				{
					await Coroutines.LatencyWait();
				}
				await Coroutines.LatencyWait();
				if (TradeUi.IsOpened)
				{
					try
					{
						if ((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) >= heSellQuantity || (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName + " " + i.Name == string_0).Sum((Item it) => it.StackCount) >= heSellQuantity || (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.FullName == string_0).Sum((Item it) => it.StackCount) >= heSellQuantity || ((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) >= heSellQuantity * 0.97m && (decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name != string_0).Sum((Item it) => it.StackCount) >= heSellQuantity - heSellQuantity * 0.97m))
						{
							int num;
							if (!TradeUi.IsOpened)
							{
								num = 0;
							}
							else
							{
								TradeControlWrapper tradeControl3 = TradeUi.TradeControl;
								num = ((((tradeControl3 != null) ? tradeControl3.ConfirmLabelText : null) != "") ? 1 : 0);
							}
							if (num != 0)
							{
								await Coroutines.LatencyWait();
								continue;
							}
							if ((decimal)TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Where((Item i) => !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(i.LocalId) && i.Name == string_0).Sum((Item it) => it.StackCount) > heSellQuantity)
							{
								await Coroutines.LatencyWait();
								await Coroutines.ReactionWait();
							}
							try
							{
								if ((RemoteMemoryObject)(object)TradeUi.TradeControl != (RemoteMemoryObject)null)
								{
									if (TradeUi.TradeControl.AcceptButtonText != "accept")
									{
										await Coroutines.LatencyWait();
										continue;
									}
									TradeUi.TradeControl.Accept(true);
									await Coroutine.Sleep(3000);
								}
							}
							catch (Exception)
							{
								await Coroutines.LatencyWait();
								continue;
							}
							await Coroutines.LatencyWait();
							await Coroutines.ReactionWait();
							if (!TradeUi.IsOpened)
							{
								return Results.CheckCustomerOffertAndAcceptResult.None;
							}
							await Coroutines.LatencyWait();
							continue;
						}
					}
					catch (Exception)
					{
						await Coroutines.ReactionWait();
						await Coroutines.LatencyWait();
						continue;
					}
					await Coroutines.ReactionWait();
					continue;
				}
				return Results.CheckCustomerOffertAndAcceptResult.TradeWindowNotOpen;
			}
			GlobalLog.Error("[CheckAndAcceptTrade] TradeUi Closed  return false.");
			return Results.CheckCustomerOffertAndAcceptResult.TradeWindowNotOpen;
			IL_03bd:;
		}
		if (TradeUi.IsOpened)
		{
			GlobalLog.Warn("[CheckAndAcceptTrade] Cancelling the trade, customer is reluctant, let try again.");
			TradeUi.TradeControl.Cancel(true);
			await Coroutine.Sleep(2968);
		}
		await Coroutines.CloseBlockingWindows();
		await Coroutines.LatencyWait();
		await Class51.LeaveGroup();
		await Coroutine.Sleep(2385);
		await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("/hideout");
		return Results.CheckCustomerOffertAndAcceptResult.WrongItem;
	}

	public static async Task<List<Item>> FindItems(string itemName)
	{
		return await FindItems((Item d) => d.FullName.Equals(itemName));
	}

	public static async Task<List<Item>> FindItems(FindItemDelegate condition)
	{
		if (!InventoryUi.IsOpened)
		{
			await Inventories.OpenInventory();
			await Coroutines.ReactionWait();
		}
		return InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item d) => condition(d)).ToList();
	}

	public static bool IsInteger(double usablePriceBuyvalue)
	{
		int num = (int)usablePriceBuyvalue;
		return usablePriceBuyvalue <= (double)num;
	}

	static TradeHelpers()
	{
		random_0 = new Random();
	}
}
