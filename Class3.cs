using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.TraderPlugin;
using ExPlugins.TraderPlugin.Classes;
using Newtonsoft.Json;

internal class Class3 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static Func<ChatEntry, bool> func_0;

		public static Func<ChatEntry, bool> func_1;

		public static Func<ChatEntry, bool> func_2;

		public static Func<ChatEntry, bool> func_3;

		public static Func<ChatRequest, bool> func_4;

		public static Func<ChatRequest, bool> func_5;

		public static Func<ChatRequest, bool> func_6;

		public static Func<Item, bool> func_7;

		public static Func<bool> func_8;

		public static Func<Item, bool> func_9;

		public static Func<Item, int> func_10;

		public static Func<Item, int> func_11;

		public static Func<Item, int> func_12;

		public static Func<Item, Vector2i> func_13;

		public static Func<Item, bool> func_14;

		public static Func<bool> func_15;

		public static Func<Item, bool> func_16;

		public static Func<NotificationInfo, bool> func_17;

		public static Func<NotificationInfo, bool> func_18;

		public static ProcessNotificationEx processNotificationEx_0;

		public static Func<NotificationInfo, bool> func_19;

		public static Func<ChatEntry, bool> func_20;

		public static Func<Player, bool> func_21;

		public static ShouldViewItemDelegate shouldViewItemDelegate_0;

		public static Func<ChatEntry, bool> func_22;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003CRun_003Eb__14_0(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}

		internal bool _003CRun_003Eb__14_2(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}

		internal bool _003CRun_003Eb__14_3(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}

		internal bool _003CRun_003Eb__14_4(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}

		internal bool _003CRun_003Eb__14_5(ChatRequest p)
		{
			return IsInZone(p.Name);
		}

		internal bool _003CRun_003Eb__14_8(ChatRequest p)
		{
			return IsInZone(p.Name);
		}

		internal bool _003CRun_003Eb__14_9(ChatRequest p)
		{
			return IsInZone(p.Name);
		}

		internal bool _003CRun_003Eb__14_11(Item i)
		{
			return i.Name == "Chaos Orb" || i.Name == "Divine Orb";
		}

		internal bool _003CRun_003Eb__14_12()
		{
			return !TradeUi.IsOpened;
		}

		internal bool _003CRun_003Eb__14_13(Item i)
		{
			return i.Name == "Chaos Orb" || i.Name == "Divine Orb";
		}

		internal int _003CRun_003Eb__14_22(Item i)
		{
			return i.StackCount;
		}

		internal int _003CRun_003Eb__14_25(Item i)
		{
			return i.StackCount;
		}

		internal int _003CRun_003Eb__14_17(Item i)
		{
			return i.StackCount;
		}

		internal Vector2i _003CRun_003Eb__14_19(Item i)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return i.LocationTopLeft;
		}

		internal bool _003CRun_003Eb__14_28(Item i)
		{
			return i.Name == "Chaos Orb" || i.Name == "Divine Orb";
		}

		internal bool _003CRun_003Eb__14_29()
		{
			return !TradeUi.IsOpened;
		}

		internal bool _003CRun_003Eb__14_30(Item i)
		{
			return i.Name == "Chaos Orb" || i.Name == "Divine Orb";
		}

		internal bool _003CDeclinePartyInvite_003Eb__15_1(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal bool _003CAcceptTradeRequest_003Eb__16_1(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal bool _003CCloseAllNotifications_003Eb__17_0(NotificationData x, NotificationType y)
		{
			GlobalLog.Debug("[CloseAllNotifications] cancelling trade request from impatient trader");
			return false;
		}

		internal bool _003CCloseAllNotifications_003Eb__17_1(NotificationInfo x)
		{
			return x.IsVisible;
		}

		internal bool _003CFillTradeRequestList_003Eb__19_0(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}

		internal bool _003CIsInZone_003Eb__22_0(Player p)
		{
			return (NetworkObject)(object)p != (NetworkObject)(object)LokiPoe.Me;
		}

		internal bool _003CView_003Eb__23_0(Inventory s, Item item1)
		{
			return true;
		}

		internal bool _003CCheckResponse_003Eb__33_0(ChatEntry m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)m.MessageType == 3;
		}
	}

	private static readonly Interval interval_0;

	private static int int_0;

	public static int int_1;

	private static List<ChatRequest> list_0;

	private static List<ChatRequest> list_1;

	private static readonly List<ChatRequest> list_2;

	private static readonly List<string> list_3;

	private static bool bool_0;

	private static bool bool_1;

	private static readonly int int_2;

	private static int int_3;

	private Thread thread_0;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "HandleTradeProcessTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (TraderPlugin.Stopwatch.Elapsed.TotalSeconds - (double)int_3 > (double)int_2)
			{
				int_3 = (int)TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
				list_0.Clear();
				list_1.Clear();
			}
			if (!bool_1 || !(TraderPlugin.TradeTimeout.Elapsed.TotalSeconds < (double)TraderPluginSettings.Instance.DelayAfterCurrencyTradeSeconds))
			{
				List<string> asd = new List<string>();
				DatWorldAreaWrapper datWorldAreaWrapper_0 = World.CurrentArea;
				if (((IAuthored)BotManager.Current).Name == "LabRunner" || LocalData.MapMods.ContainsKey((StatTypeGGG)10342) || LocalData.MapMods.ContainsKey((StatTypeGGG)14763) || TraderPluginSettings.Instance.LocationNamesToIgnoreTradeIn.Any((TraderPluginSettings.NameEntry x) => x.Name == datWorldAreaWrapper_0.Name))
				{
					if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) != 0)
					{
						await SendChatMsg("/clear");
					}
					return false;
				}
				if (!CombatAreaCache.IsInIncursion)
				{
					if (((IAuthored)BotManager.Current).Name == "NullBot" && !LokiPoe.Me.IsInHideout)
					{
						await PlayerAction.GoToHideout();
					}
					bool doStuff = false;
					if (FillTradeRequestList() && !list_1.Contains(list_0.Last()))
					{
						GlobalLog.Debug("[HandleTrade] Player " + list_0.Last().Name + " has been added to the invite list");
						list_1.Add(list_0.Last());
					}
					if (bool_0)
					{
						TraderPlugin.PortalsSpent += 1f;
						bool_0 = false;
					}
					foreach (ChatRequest player in list_1)
					{
						if (TraderPluginSettings.Instance.ShouldIgnoreTradesAfterMapLeaveAmount && TraderPlugin.PortalsSpent.HasValue && (int)TraderPlugin.PortalsSpent.Value >= TraderPluginSettings.Instance.MaxPortalsSpentToLeaveMap && player.Currency == "chaos" && player.Price < (decimal)TraderPluginSettings.Instance.MinPriceInChaosToLeaveMapWhenIgnoringTrades)
						{
							if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) != 0 && interval_0.Elapsed)
							{
								await SendChatMsg("/clear");
							}
							continue;
						}
						if (!player.WasJsoned)
						{
							CheckItemInJson(player);
						}
						if (player.WasServed || !player.FoundItem || (player.Price < (decimal)TraderPluginSettings.Instance.MinPriceInChaosToTrade && player.Currency == "chaos") || player.Name.Contains("?"))
						{
							continue;
						}
						if (!IsInParty(player.Name) && player.InviteCount < 2 && TraderPlugin.Stopwatch.Elapsed.TotalSeconds - player.LastInviteTime > 10.0 && !list_3.Contains(player.ItemName))
						{
							list_3.Add(player.ItemName);
							await SendChatMsg("/invite " + player.Name);
							GlobalLog.Debug("[HandleTrade] Player " + player.Name + " has been invited");
							player.LastInviteTime = TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
							player.InviteCount++;
							while (true)
							{
								try
								{
									if (datWorldAreaWrapper_0.IsTown || datWorldAreaWrapper_0.IsHideoutArea)
									{
										await DeclinePartyInvite(asd);
										ITask task = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
										CloseAllNotifications();
										await Coroutines.CloseBlockingWindows();
										await task.Run();
										await DeclinePartyInvite(asd);
										if (((IAuthored)BotManager.Current).Name != "NullBot" && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < TraderPluginSettings.Instance.FreeInventorySquaresToTradeBeforeStashing)
										{
											task = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("SellTask");
											CloseAllNotifications();
											await Coroutines.CloseBlockingWindows();
											await task.Run();
											await DeclinePartyInvite(asd);
											task = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("StashTask");
											CloseAllNotifications();
											await Coroutines.CloseBlockingWindows();
											await task.Run();
											await DeclinePartyInvite(asd);
										}
										task = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ClearCursorTask");
										CloseAllNotifications();
										await Coroutines.CloseBlockingWindows();
										await task.Run();
										await DeclinePartyInvite(asd);
										player.WasInvitedInHo = true;
									}
								}
								catch
								{
									continue;
								}
								break;
							}
						}
						while (true)
						{
							try
							{
								if (IsPartyMember(player.Name) && !datWorldAreaWrapper_0.IsTown && !datWorldAreaWrapper_0.IsHideoutArea && TraderPlugin.Stopwatch.Elapsed.TotalSeconds - player.LastInviteTime > 10.0)
								{
									doStuff = true;
								}
							}
							catch
							{
								continue;
							}
							break;
						}
					}
					if (doStuff)
					{
						if (!datWorldAreaWrapper_0.IsHideoutArea)
						{
							GlobalLog.Debug("[HandleTrade] Now traveling to hideout to trade");
							await PlayerAction.TpToTown(forceNewPortal: true);
						}
						await DeclinePartyInvite(asd);
						ITask task5 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
						CloseAllNotifications();
						await Coroutines.CloseBlockingWindows();
						await task5.Run();
						await DeclinePartyInvite(asd);
						if (((IAuthored)BotManager.Current).Name != "NullBot" && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < TraderPluginSettings.Instance.FreeInventorySquaresToTradeBeforeStashing)
						{
							task5 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("SellTask");
							CloseAllNotifications();
							await Coroutines.CloseBlockingWindows();
							await task5.Run();
							await DeclinePartyInvite(asd);
							task5 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("StashTask");
							CloseAllNotifications();
							await Coroutines.CloseBlockingWindows();
							await task5.Run();
							await DeclinePartyInvite(asd);
						}
						task5 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ClearCursorTask");
						CloseAllNotifications();
						await Coroutines.CloseBlockingWindows();
						await task5.Run();
						await DeclinePartyInvite(asd);
					}
					while (true)
					{
						try
						{
							datWorldAreaWrapper_0 = World.CurrentArea;
							if (datWorldAreaWrapper_0.IsTown || datWorldAreaWrapper_0.IsHideoutArea)
							{
								break;
							}
							return false;
						}
						catch
						{
							continue;
						}
					}
					if (int_1 >= 10)
					{
						GlobalLog.Debug("[HandleTrade] ErrorCounter is > 10, now shutting down poe");
						int_1 = 0;
						LokiPoe.Memory.Process.Kill();
						return false;
					}
					if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) != 0)
					{
						await SendChatMsg("/clear");
					}
					Item item_0;
					foreach (ChatRequest chatRequest_0 in list_1.OrderByDescending((ChatRequest p) => IsInZone(p.Name)))
					{
						if (!chatRequest_0.WasJsoned)
						{
							CheckItemInJson(chatRequest_0);
						}
						if (chatRequest_0.WasServed || !chatRequest_0.FoundItem || (chatRequest_0.Price < (decimal)TraderPluginSettings.Instance.MinPriceInChaosToTrade && chatRequest_0.Currency == "chaos") || chatRequest_0.Name.Contains("?"))
						{
							continue;
						}
						if (datWorldAreaWrapper_0.IsTown)
						{
							GlobalLog.Debug("[HandleTrade] 2Now traveling to hideout to trade");
							await SendChatMsg("/hideout");
						}
						await Coroutines.CloseBlockingWindows();
						await DeclinePartyInvite(asd);
						ITask task9 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
						CloseAllNotifications();
						await Coroutines.CloseBlockingWindows();
						await task9.Run();
						await DeclinePartyInvite(asd);
						if (((IAuthored)BotManager.Current).Name != "NullBot" && InventoryUi.InventoryControl_Main.Inventory.AvailableInventorySquares < TraderPluginSettings.Instance.FreeInventorySquaresToTradeBeforeStashing)
						{
							task9 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("SellTask");
							CloseAllNotifications();
							await Coroutines.CloseBlockingWindows();
							await task9.Run();
							await DeclinePartyInvite(asd);
							task9 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("StashTask");
							CloseAllNotifications();
							await Coroutines.CloseBlockingWindows();
							await task9.Run();
							await DeclinePartyInvite(asd);
						}
						task9 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ClearCursorTask");
						CloseAllNotifications();
						await Coroutines.CloseBlockingWindows();
						await task9.Run();
						await DeclinePartyInvite(asd);
						string string_0 = chatRequest_0.Name;
						if (chatRequest_0.WasInvitedInHo)
						{
							await Wait.For(() => list_1.Any((ChatRequest p) => IsInZone(p.Name)) || !IsInParty(string_0), string_0 + " to come to our hideout", 500, 35000);
							if (!IsInParty(chatRequest_0.Name))
							{
								GlobalLog.Debug("[HandleTrade] Player " + string_0 + " did not accepted our invite, now going to kick and ignore him");
								chatRequest_0.WasServed = true;
								if (IsInParty(string_0))
								{
									await SendChatMsg("/kick " + string_0);
								}
								DoAfterTrade();
								continue;
							}
						}
						await DeclinePartyInvite(asd);
						if (!chatRequest_0.WasInvitedInHo)
						{
							await Wait.For(() => list_1.Any((ChatRequest p) => IsInZone(p.Name)) || !IsInParty(string_0), string_0 + " to come to our hideout", 500, 15000);
						}
						if (IsInZone(string_0, log: true))
						{
							GlobalLog.Warn($"[HandleTrade] We're in hideout! Name: {string_0} Item: {chatRequest_0.ItemName} Price: {chatRequest_0.Price}{chatRequest_0.Currency} Stash tab: {chatRequest_0.TabName} Pos: {chatRequest_0.ItemPos}");
							if (World.CurrentArea.IsMyHideoutArea && !StashUi.IsOpened)
							{
								await Inventories.OpenStash();
							}
							bool isTab = false;
							chatRequest_0.CurrentTradePhase = ChatRequest.TradePhase.ItemTaken;
							if (chatRequest_0.SellAmount == 0)
							{
								if (TraderPluginSettings.Instance.StashTabToTrade == chatRequest_0.TabName)
								{
									isTab = true;
								}
								if (isTab)
								{
									await Inventories.OpenStashTab(chatRequest_0.TabName, Name);
									Item item_2 = StashUi.InventoryControl.Inventory.GetItemAtLocation(new Vector2i(chatRequest_0.ItemPos.X, chatRequest_0.ItemPos.Y));
									if (!((RemoteMemoryObject)(object)item_2 == (RemoteMemoryObject)null))
									{
										string note = item_2.DisplayNote.Replace(",", ".");
										note = note.Replace(".", ",");
										decimal price2 = Convert.ToDecimal(note.Split(Convert.ToChar(" "))[1]);
										string currency = note.Split(Convert.ToChar(" "))[2];
										string fullName = item_2.FullName;
										bool rareUniqOrMaven = (int)item_2.Rarity == 3 || (int)item_2.Rarity == 2 || (item_2.Metadata.Contains("Items/MapFragments/Maven") && item_2.Class == "MiscMapItem" && (int)item_2.Rarity > 0);
										string combined = item_2.FullName + " " + item_2.Name;
										GlobalLog.Debug("[HandleTrade] Now checking if we have item and price is correct");
										if (rareUniqOrMaven)
										{
											GlobalLog.Info($"comb: {combined} itemName: {chatRequest_0.ItemName} comb==itemname {chatRequest_0.ItemName.ToLowerInvariant().Equals(combined.ToLowerInvariant(), StringComparison.InvariantCulture)}");
										}
										if (((rareUniqOrMaven && chatRequest_0.ItemName.ToLowerInvariant().Equals(combined.ToLowerInvariant(), StringComparison.InvariantCulture)) || chatRequest_0.ItemName.EqualsIgnorecase(item_2.Name) || chatRequest_0.ItemName.EqualsIgnorecase(item_2.FullName) || (item_2.Stats.ContainsKey((StatTypeGGG)10342) && chatRequest_0.ItemName.ContainsIgnorecase(item_2.Name))) && chatRequest_0.Price >= price2)
										{
											GlobalLog.Debug("[HandleTrade] Now going to trade with player " + string_0);
											if (item_2.StackCount <= 1)
											{
												await Inventories.FastMoveFromStashTab(item_2.LocationTopLeft);
											}
											else
											{
												await Inventories.SplitAndPlaceItemInMainInventory(StashUi.InventoryControl, item_2, 1);
											}
											Vector2i posToMove = InventoryUi.InventoryControl_Main.Inventory.FindItemByFullName(fullName).LocationTopLeft;
											while (true)
											{
												SoldItem soldItem2;
												int ran4;
												int priceInChaos4;
												int offerInChaos2;
												int currentCurrency2;
												if (chatRequest_0.TradeCount <= 1)
												{
													chatRequest_0.TradeCount++;
													await Coroutines.CloseBlockingWindows();
													if (AcceptTradeRequest(chatRequest_0.Name))
													{
														Thread.Sleep(LokiPoe.Random.Next(350, 1500));
														CloseAllNotifications();
													}
													else
													{
														CloseAllNotifications();
														await SendChatMsg("/tradewith " + string_0);
													}
													Stopwatch sw2 = Stopwatch.StartNew();
													int ran2 = LokiPoe.Random.Next(6000, 9000);
													while (!TradeUi.IsOpened && sw2.ElapsedMilliseconds <= ran2)
													{
														await Coroutines.LatencyWait();
													}
													if (!TradeUi.IsOpened)
													{
														GlobalLog.Debug("[HandleTrade] Player is not accepting trade, retrying");
														continue;
													}
													if (!TradeUi.IsOpened)
													{
														break;
													}
													item_2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(new Vector2i(posToMove.X, posToMove.Y));
													string stats2 = "";
													if (item_2.Stats.Count != 0)
													{
														try
														{
															stats2 = string.Join(";", item_2.Stats).Replace("[", "").Replace("]", "")
																.Trim();
														}
														catch (Exception ex3)
														{
															Exception ex2 = ex3;
															GlobalLog.Error("Failed getting item stats: " + ex2.Message);
														}
													}
													soldItem2 = new SoldItem(name: (item_2.Class == "Map" && item_2.Stats.ContainsKey((StatTypeGGG)10342)) ? ((!item_2.Stats.ContainsKey((StatTypeGGG)14763)) ? ("Blighted " + item_2.Name) : ("Blight-ravaged " + item_2.Name)) : ((!(item_2.Name == "Prophecy") && (int)item_2.Rarity != 3 && (int)item_2.Rarity != 2) ? item_2.Name : item_2.FullName), source: ((NetworkObject)LokiPoe.Me).Name, type: item_2.Class, price: 0, stats: stats2);
													ran4 = LokiPoe.Random.Next(1000, 1500);
													await Wait.SleepSafe(ran4);
													InventoryUi.InventoryControl_Main.FastMove(item_2.LocalId, true, true);
													await Coroutines.LatencyWait();
													int slotsInOffer2 = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count();
													Stopwatch offerSw2 = Stopwatch.StartNew();
													Thread.Sleep(LokiPoe.Random.Next(1000, 1500));
													int retCountHardStopper2 = 0;
													while (offerSw2.ElapsedMilliseconds <= 60000L && retCountHardStopper2 <= 4)
													{
														while (TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count() != slotsInOffer2 && offerSw2.ElapsedMilliseconds < 30000L)
														{
															slotsInOffer2 = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count();
															Thread.Sleep(LokiPoe.Random.Next(1000, 1500));
														}
														await Wait.SleepSafe(ran4);
														int chaosCount2 = 0;
														int divCount2 = 0;
														if (TradeUi.IsOpened)
														{
															List<Item> items2 = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items;
															if (items2.Any())
															{
																TradeUi.TradeControl.InventoryControl_OtherOffer.ViewItemsInInventory(View(TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory, items2[0]), (Func<bool>)Open);
																GlobalLog.Debug("Item " + items2[0].FullName + " has been moused over");
															}
															if (TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count() != slotsInOffer2)
															{
																retCountHardStopper2++;
																Thread.Sleep(ran4);
																continue;
															}
															foreach (Item offerItem2 in TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items)
															{
																if (!(offerItem2.Name == "Chaos Orb"))
																{
																	if (offerItem2.Name == "Divine Orb")
																	{
																		divCount2 += offerItem2.StackCount;
																		GlobalLog.Debug($"[HandleTrade] Now adding {offerItem2.StackCount} div to the total div count");
																	}
																}
																else
																{
																	chaosCount2 += offerItem2.StackCount;
																	GlobalLog.Debug($"[HandleTrade] Now adding {offerItem2.StackCount} chaoses to the total chaos count");
																}
															}
															priceInChaos4 = 0;
															priceInChaos4 = ((currency == "divine") ? ((int)(price2 * (decimal)(int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb"))) : (priceInChaos4 + (int)price2));
															offerInChaos2 = divCount2 * (int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb") + chaosCount2;
															GlobalLog.Debug("[HandleTrade] Offer is " + offerInChaos2 + " chaoses, " + divCount2 + " of them are div, " + chaosCount2 + " are chaos");
															GlobalLog.Debug("[HandleTrade] Price is " + priceInChaos4 + " chaoses");
															if (TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count() == slotsInOffer2 && offerInChaos2 < priceInChaos4)
															{
																int ran6 = LokiPoe.Random.Next(2000, 3000);
																Thread.Sleep(ran6);
															}
															if (offerInChaos2 != CountOfferPriceInChaos())
															{
																retCountHardStopper2++;
																continue;
															}
															foreach (Item it2 in TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items)
															{
																if (!Open() || !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(it2.LocalId))
																{
																	continue;
																}
																GlobalLog.Debug("[HandleTrade] There is an item that was not howered over, now going to recount");
																goto IL_3d67;
															}
															if (!((double)offerInChaos2 < (double)priceInChaos4 * 0.95))
															{
																currentCurrency2 = Inventories.InventoryItems.Count((Item i) => i.Name == "Chaos Orb" || i.Name == "Divine Orb");
																Stopwatch timeout2 = Stopwatch.StartNew();
																while (TradeUi.IsOpened && timeout2.ElapsedMilliseconds < 10000L && TradeUi.TradeControl.AcceptButtonText.ToLower() != "cancel accept")
																{
																	await Coroutines.LatencyWait();
																	Thread.Sleep(LokiPoe.Random.Next(500, 800));
																	if (TradeUi.TradeControl.AcceptButtonText.ToLower() == "cancel accept")
																	{
																		break;
																	}
																	if (offerInChaos2 == CountOfferPriceInChaos())
																	{
																		TradeUi.TradeControl.Accept(true);
																		await Coroutines.LatencyWait();
																		continue;
																	}
																	goto IL_3c9d;
																}
																if (TradeUi.IsOpened && TradeUi.TradeControl.AcceptButtonText.ToLower() == "cancel accept")
																{
																	await Wait.For(() => !TradeUi.IsOpened, "buyer to accept the offer", 100, 10000);
																	if (TradeUi.IsOpened && TradeUi.TradeControl.AcceptButtonText.ToLower() != "cancel accept")
																	{
																		Thread.Sleep(ran4);
																		continue;
																	}
																}
																goto IL_3d9b;
															}
															goto IL_3ded;
														}
														goto IL_3f98;
														IL_3c9d:
														retCountHardStopper2 += 5;
														IL_3d67:;
													}
													await Coroutines.CloseBlockingWindows();
													GlobalLog.Debug("[HandleTrade] Player could not put all the currency in 1 minute, now kicking him");
													chatRequest_0.WasServed = true;
													if (IsInParty(string_0))
													{
														await SendChatMsg("/kick " + string_0);
													}
													ItemSearchParams param6 = new ItemSearchParams(chatRequest_0.Price, chatRequest_0.Currency, new Vector2i(posToMove.X, posToMove.Y), forcePutOnCoords: true, new Vector2i(chatRequest_0.ItemPos.X, chatRequest_0.ItemPos.Y));
													TraderPlugin.EvaluatedItems.Enqueue(param6);
													ITask task24 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
													await task24.Run();
													DoAfterTrade();
													break;
												}
												await Coroutines.CloseBlockingWindows();
												GlobalLog.Debug("[HandleTrade] Player is not accepting trade, now kicking him");
												chatRequest_0.WasServed = true;
												if (IsInParty(string_0))
												{
													await SendChatMsg("/kick " + string_0);
												}
												ItemSearchParams param5 = new ItemSearchParams(chatRequest_0.Price, chatRequest_0.Currency, new Vector2i(posToMove.X, posToMove.Y), forcePutOnCoords: true, new Vector2i(chatRequest_0.ItemPos.X, chatRequest_0.ItemPos.Y));
												TraderPlugin.EvaluatedItems.Enqueue(param5);
												ITask task23 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
												await task23.Run();
												DoAfterTrade();
												break;
												IL_3ded:
												Thread.Sleep(ran4);
												TradeUi.TradeControl.Cancel(true);
												GlobalLog.Debug("[HandleTrade] Offer is too low, now going to inform client");
												if (!((double)offerInChaos2 < (double)priceInChaos4 * 0.65))
												{
													if (chatRequest_0.TradeCount >= 2)
													{
														GlobalLog.Debug("[HandleTrade] Kicking without hesitation. He knows why.");
														if (IsInParty(string_0))
														{
															await SendChatMsg("/kick " + string_0);
														}
														chatRequest_0.WasServed = true;
														ItemSearchParams param7 = new ItemSearchParams(chatRequest_0.Price, chatRequest_0.Currency, new Vector2i(posToMove.X, posToMove.Y), forcePutOnCoords: true, new Vector2i(chatRequest_0.ItemPos.X, chatRequest_0.ItemPos.Y));
														TraderPlugin.EvaluatedItems.Enqueue(param7);
														ITask task17 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
														await task17.Run();
														DoAfterTrade();
														break;
													}
													await SendChatMsg($"@{chatRequest_0.Name} {priceInChaos4 - offerInChaos2}c more");
													Thread.Sleep(LokiPoe.Random.Next(7000, 8000));
												}
												else if (offerInChaos2 == 0)
												{
													Thread.Sleep(LokiPoe.Random.Next(4000, 4500));
												}
												else if (chatRequest_0.TradeCount < 2)
												{
													await SendChatMsg("@" + chatRequest_0.Name + " " + RandomQuestionMark());
													Thread.Sleep(LokiPoe.Random.Next(7000, 8000));
												}
												continue;
												IL_3f98:
												await Coroutines.ReactionWait();
												continue;
												IL_3d9b:
												await Coroutines.CloseBlockingWindows();
												int afterTradeCurrency2 = Inventories.InventoryItems.Count((Item i) => i.Name == "Chaos Orb" || i.Name == "Divine Orb");
												if (currentCurrency2 == afterTradeCurrency2)
												{
													continue;
												}
												soldItem2.Price = offerInChaos2;
												GlobalLog.Info($"[HandleTrade]{soldItem2}");
												TraderPlugin.SoldItems.Enqueue(soldItem2);
												Class8.WriteToLog(soldItem2);
												Random rn2 = new Random();
												Thread.Sleep(rn2.Next(1000));
												GlobalLog.Debug("[HandleTrade] Trade is successfully finished.");
												chatRequest_0.WasServed = true;
												try
												{
													if (IsInParty(chatRequest_0.Name))
													{
														await SendChatMsg("/kick " + chatRequest_0.Name);
													}
												}
												catch
												{
												}
												await Coroutines.ReactionWait();
												item_2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(new Vector2i(posToMove.X, posToMove.Y));
												if (item_2.FullName + " " + item_2.Name == chatRequest_0.ItemName || ((item_2.Stats.ContainsKey((StatTypeGGG)14763) || item_2.Stats.ContainsKey((StatTypeGGG)10342)) && chatRequest_0.ItemName.Contains(item_2.Name)))
												{
													ItemSearchParams param8 = new ItemSearchParams(chatRequest_0.Price, chatRequest_0.Currency, new Vector2i(posToMove.X, posToMove.Y), forcePutOnCoords: true, new Vector2i(chatRequest_0.ItemPos.X, chatRequest_0.ItemPos.Y));
													TraderPlugin.EvaluatedItems.Enqueue(param8);
													ITask listTask2 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
													await listTask2.Run();
												}
												else
												{
													await SendChatMsg("@" + string_0 + " " + RandomThanks());
													if (File.Exists(TraderPlugin.FullFileName))
													{
														GlobalLog.Debug("[HandleTrade] Now deleting item from json");
														List<Class9.Class10> mnePohui4 = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
														Class9.Class10 entry4 = mnePohui4.FirstOrDefault((Class9.Class10 i) => i.ItemPos == chatRequest_0.ItemPos && (((item_2.Stats.ContainsKey((StatTypeGGG)14763) || item_2.Stats.ContainsKey((StatTypeGGG)10342)) && chatRequest_0.ItemName.Contains(i.ItemName)) || i.ItemName == chatRequest_0.ItemName));
														mnePohui4.Remove(entry4);
														File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui4, (Formatting)1));
													}
												}
												ITask task20 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
												CloseAllNotifications();
												await task20.Run();
												task20 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("SellTask");
												await task20.Run();
												task20 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("StashTask");
												await task20.Run();
												DoAfterTrade();
												break;
											}
										}
										else
										{
											GlobalLog.Debug("[HandleTrade] Something is wrong, now going to kick this player");
											chatRequest_0.WasServed = true;
											if (IsInParty(string_0))
											{
												await SendChatMsg("/kick " + string_0);
											}
											DoAfterTrade();
										}
										continue;
									}
									GlobalLog.Debug("[HandleTrade] Item was not found");
									chatRequest_0.WasServed = true;
									await Coroutines.ReactionWait();
									await SendChatMsg("@" + string_0 + " sry sold");
									if (IsInParty(string_0))
									{
										await SendChatMsg("/kick " + string_0);
									}
									DoAfterTrade();
									if (File.Exists(TraderPlugin.FullFileName))
									{
										GlobalLog.Debug("[HandleTrade] Now deleting item from json");
										List<Class9.Class10> mnePohui3 = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
										Class9.Class10 entry3 = mnePohui3.FirstOrDefault((Class9.Class10 i) => chatRequest_0.ItemName.ContainsIgnorecase(i.ItemName) && i.ItemPos == chatRequest_0.ItemPos);
										mnePohui3.Remove(entry3);
										File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui3, (Formatting)1));
									}
								}
								else
								{
									GlobalLog.Debug("[HandleTrade] Stash tab was not found");
									chatRequest_0.WasServed = true;
									await Coroutines.ReactionWait();
									if (IsInParty(chatRequest_0.Name))
									{
										await SendChatMsg("/kick " + string_0);
									}
									DoAfterTrade();
								}
								continue;
							}
							await Inventories.OpenStashTab(TraderPluginSettings.Instance.StashTabToTrade, Name);
							int amtFound = 0;
							bool foundEnough = false;
							List<Vector2i> itemsInStashCoords = new List<Vector2i>();
							Item item_ = new Item();
							foreach (Item item6 in StashUi.InventoryControl.Inventory.Items)
							{
								if (TraderPluginSettings.Instance.DebugMode)
								{
									GlobalLog.Debug($"[HandleCurrencyTrade] Now scanning {item6.Name}, stack count {item6.StackCount}");
								}
								InfluenceHelper.InfluenceType? influenceType = chatRequest_0.InfluenceType;
								if ((influenceType.GetValueOrDefault() == InfluenceHelper.InfluenceType.None) & influenceType.HasValue)
								{
									if (item6.Name == chatRequest_0.ItemName || (item6.Stats.ContainsKey((StatTypeGGG)10342) && chatRequest_0.ItemName.Contains(item6.Name)))
									{
										item_ = item6;
										itemsInStashCoords.Add(item6.LocationTopLeft);
										amtFound += item6.StackCount;
										GlobalLog.Debug($"[HandleCurrencyTrade] Found {amtFound} of {item6.Name}");
										if (amtFound >= chatRequest_0.SellAmount)
										{
											GlobalLog.Debug("[HandleCurrencyTrade] Found enough items to trade, now proceeding");
											foundEnough = true;
											break;
										}
									}
								}
								else if (chatRequest_0.InfluenceType == InfluenceHelper.GetInfluence(item6))
								{
									GlobalLog.Debug($"[HandleCurrencyTrade] Found {item6.Name}, influence: {InfluenceHelper.GetInfluence(item6)}");
									item_ = item6;
									itemsInStashCoords.Add(item6.LocationTopLeft);
									amtFound += item6.StackCount;
									if (amtFound >= chatRequest_0.SellAmount)
									{
										GlobalLog.Debug("[HandleCurrencyTrade] Found enough items to trade, now proceeding");
										foundEnough = true;
										break;
									}
								}
							}
							if (amtFound == 0)
							{
								GlobalLog.Debug("[HandleCurrencyTrade] No items found");
								chatRequest_0.WasServed = true;
								await SendChatMsg("@" + string_0 + " sry, sold");
								if (IsInParty(string_0))
								{
									await SendChatMsg("/kick " + string_0);
								}
								DoAfterTrade();
								if (File.Exists(TraderPlugin.FullFileName))
								{
									GlobalLog.Debug("[HandleTrade] Now deleting item from json");
									List<Class9.Class10> mnePohui2 = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
									Class9.Class10 entry2 = mnePohui2.FirstOrDefault((Class9.Class10 i) => chatRequest_0.ItemName.ContainsIgnorecase(i.ItemName) && i.ItemPos == chatRequest_0.ItemPos);
									mnePohui2.Remove(entry2);
									File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui2, (Formatting)1));
								}
								continue;
							}
							if (amtFound < chatRequest_0.SellAmount)
							{
								decimal pricePerUnit = (chatRequest_0.IsSingleItemPrice ? chatRequest_0.Price : (chatRequest_0.Price / (decimal)chatRequest_0.SellAmount));
								chatRequest_0.SellAmount = amtFound;
								chatRequest_0.Price = (decimal)amtFound * pricePerUnit;
								string curr = ((chatRequest_0.Currency == "chaos") ? "c" : "div");
								if (TraderPluginSettings.Instance.DebugMode)
								{
									GlobalLog.Debug($"[HandleCurrencyTrade] New price per unit: {pricePerUnit}, total price: {chatRequest_0.Price}.");
								}
								GlobalLog.Debug("[HandleCurrencyTrade] Not enough items found. Now going to inform the buyer.");
								await SendChatMsg($"@{string_0} only {amtFound}, {chatRequest_0.Price}{curr}");
								foundEnough = true;
							}
							Item itemWithNote = StashUi.InventoryControl.Inventory.Items.FirstOrDefault((Item i) => i.FullName == item_.FullName && !string.IsNullOrEmpty(i.DisplayNote));
							string note4 = itemWithNote.DisplayNote.Replace(",", ".");
							note4 = note4.Replace(".", ",");
							decimal price = ((!item_.Name.Contains("Stacked Deck") || string.IsNullOrEmpty(TraderPluginSettings.Instance.StackedDeckExactNote)) ? Convert.ToDecimal(note4.Split(Convert.ToChar(" "))[1]) : ((decimal)TraderPluginSettings.Instance.DefaultStackedDeckPrice));
							if (chatRequest_0.Price / (decimal)chatRequest_0.SellAmount < price * 0.95m)
							{
								GlobalLog.Debug("[HandleCurrencyTrade] Dolboebik reshil nas nebat, pashel nahui");
								chatRequest_0.WasServed = true;
								if (IsInParty(string_0))
								{
									await SendChatMsg("/kick " + string_0);
								}
								DoAfterTrade();
								continue;
							}
							if (!foundEnough)
							{
								GlobalLog.Debug("[HandleCurrencyTrade] Somehow we did not found enough items to trade");
								chatRequest_0.WasServed = true;
								if (IsInParty(chatRequest_0.Name))
								{
									await SendChatMsg("/kick " + string_0);
								}
								DoAfterTrade();
								continue;
							}
							foreach (Vector2i itemCoords6 in itemsInStashCoords)
							{
								Item item5 = StashUi.InventoryControl.Inventory.FindItemByPos(itemCoords6);
								List<Item> itemsInInventory = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Name == chatRequest_0.ItemName).ToList();
								int int_ = itemsInInventory.Sum((Item i) => i.StackCount);
								int needed = chatRequest_0.SellAmount - int_;
								if (needed <= 0)
								{
									break;
								}
								GlobalLog.Debug($"[{Name}] Taking {item5.Name} from stash. {needed} more needed.");
								await Inventories.SplitAndPlaceItemInMainInventory(StashUi.InventoryControl, item5, needed);
								await Wait.For(() => InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.FullName == chatRequest_0.ItemName).Sum((Item i) => i.StackCount) != int_, "babab", 5);
							}
							int quantityInInventory = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Name == chatRequest_0.ItemName).Sum((Item i) => i.StackCount);
							if (quantityInInventory > chatRequest_0.SellAmount)
							{
								int int_0 = quantityInInventory - chatRequest_0.SellAmount;
								Item itemtoSplit = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item i) => i.Name == chatRequest_0.ItemName && i.StackCount > int_0);
								if ((RemoteMemoryObject)(object)itemtoSplit != (RemoteMemoryObject)null)
								{
									await Inventories.SplitAndPlaceItemInMainInventory(InventoryUi.InventoryControl_Main, itemtoSplit, int_0);
								}
								Item sItem = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item i) => i.Name == chatRequest_0.ItemName && i.StackCount == int_0);
								if ((RemoteMemoryObject)(object)sItem != (RemoteMemoryObject)null)
								{
									await Inventories.FastMoveFromInventory(sItem.LocationTopLeft);
								}
								await Wait.LatencySleep();
							}
							List<Vector2i> itemsInInventoryCoords = (from i in InventoryUi.InventoryControl_Main.Inventory.Items
								where i.FullName == chatRequest_0.ItemName
								select i.LocationTopLeft).ToList();
							await Coroutines.LatencyWait();
							Thread.Sleep(1500);
							while (true)
							{
								SoldItem soldItem;
								int ran3;
								int priceInChaos2;
								int offerInChaos;
								int currentCurrency;
								if (chatRequest_0.TradeCount <= 1)
								{
									chatRequest_0.TradeCount++;
									await Coroutines.CloseBlockingWindows();
									if (AcceptTradeRequest(string_0))
									{
										Thread.Sleep(LokiPoe.Random.Next(350, 1500));
										CloseAllNotifications();
									}
									else
									{
										CloseAllNotifications();
										await SendChatMsg("/tradewith " + string_0);
									}
									Stopwatch sw = Stopwatch.StartNew();
									int ran = LokiPoe.Random.Next(6000, 9000);
									while (!TradeUi.IsOpened && sw.ElapsedMilliseconds <= ran)
									{
										await Coroutines.LatencyWait();
									}
									if (TradeUi.IsOpened)
									{
										if (!TradeUi.IsOpened)
										{
											break;
										}
										int ranBig = LokiPoe.Random.Next(1000, 1500);
										Thread.Sleep(ranBig);
										soldItem = new SoldItem();
										foreach (Vector2i itemCoords4 in itemsInInventoryCoords)
										{
											Item item3 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemCoords4);
											string stats = "";
											if (item3.Stats.Count != 0)
											{
												try
												{
													stats = string.Join(";", item3.Stats).Replace("[", "").Replace("]", "")
														.Trim();
												}
												catch (Exception ex3)
												{
													Exception ex = ex3;
													GlobalLog.Error("Failed getting item stats: " + ex.Message);
												}
											}
											string correctName = ((item3.Class == "Map" && item3.Stats.ContainsKey((StatTypeGGG)10342)) ? (item3.Stats.ContainsKey((StatTypeGGG)14763) ? ("Blight-ravaged " + item3.Name) : ("Blighted " + item3.Name)) : ((!(item3.Name == "Prophecy") && (int)item3.Rarity != 3 && (int)item3.Rarity != 2) ? item3.Name : item3.FullName));
											soldItem = new SoldItem(name: correctName + $" (x{chatRequest_0.SellAmount})", source: ((NetworkObject)LokiPoe.Me).Name, type: item3.Class, price: 0, stats: stats);
											await Coroutines.LatencyWait();
											Thread.Sleep(LokiPoe.Random.Next(20, 50));
											InventoryUi.InventoryControl_Main.FastMove(item3.LocalId, true, true);
										}
										Thread.Sleep(LokiPoe.Random.Next(150, 200));
										int slotsInOffer = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count();
										Stopwatch offerSw = Stopwatch.StartNew();
										Thread.Sleep(LokiPoe.Random.Next(1000, 1500));
										int retCountHardStopper = 0;
										while (offerSw.ElapsedMilliseconds <= 60000L && retCountHardStopper <= 4)
										{
											while (Open() && TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count() != slotsInOffer && offerSw.ElapsedMilliseconds < 30000L)
											{
												slotsInOffer = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count();
												Thread.Sleep(LokiPoe.Random.Next(1200, 1500));
											}
											ran3 = LokiPoe.Random.Next(1000, 1500);
											await Wait.SleepSafe(ran3);
											int chaosCount = 0;
											int divCount = 0;
											if (TradeUi.IsOpened)
											{
												if (Open() && TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count != slotsInOffer)
												{
													retCountHardStopper++;
													Thread.Sleep(ran3);
													continue;
												}
												List<Item> items = TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items;
												if (Open() && items.Any())
												{
													TradeUi.TradeControl.InventoryControl_OtherOffer.ViewItemsInInventory(View(TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory, items[0]), (Func<bool>)Open);
													GlobalLog.Debug("Item " + items[0].FullName + " has been moused over");
												}
												foreach (Item offerItem in TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items)
												{
													if (offerItem.Name == "Chaos Orb")
													{
														chaosCount += offerItem.StackCount;
														GlobalLog.Debug($"[HandleCurrencyTrade] Now adding {offerItem.StackCount} chaoses to the total chaos count");
													}
													else if (offerItem.Name == "Divine Orb")
													{
														divCount += offerItem.StackCount;
														GlobalLog.Debug($"[HandleCurrencyTrade] Now adding {offerItem.StackCount} div to the total div count");
													}
												}
												priceInChaos2 = 0;
												priceInChaos2 = ((!(chatRequest_0.Currency == "divine")) ? (priceInChaos2 + (int)chatRequest_0.Price) : ((int)(chatRequest_0.Price * (decimal)(int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb"))));
												offerInChaos = divCount * (int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb") + chaosCount;
												GlobalLog.Debug("[HandleCurrencyTrade] Offer is " + offerInChaos + " chaoses, " + divCount + " of them are div, " + chaosCount + " are chaos");
												GlobalLog.Debug("[HandleCurrencyTrade] Price is " + priceInChaos2 + " chaoses");
												if (Open() && TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items.Count() == slotsInOffer && offerInChaos < priceInChaos2)
												{
													int ran5 = LokiPoe.Random.Next(2000, 3000);
													Thread.Sleep(ran5);
												}
												if (offerInChaos != CountOfferPriceInChaos())
												{
													retCountHardStopper++;
													Thread.Sleep(ran3);
													continue;
												}
												foreach (Item it in TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items)
												{
													if (!Open() || !TradeUi.TradeControl.InventoryControl_OtherOffer.IsItemTransparent(it.LocalId))
													{
														continue;
													}
													GlobalLog.Debug("[HandleCurrencyTrade] There is an item that was not howered over, now going to recount");
													goto IL_615f;
												}
												if (!((double)offerInChaos < (double)priceInChaos2 * 0.95))
												{
													currentCurrency = Inventories.InventoryItems.Count((Item i) => i.Name == "Chaos Orb" || i.Name == "Divine Orb");
													Stopwatch timeout = Stopwatch.StartNew();
													while (TradeUi.IsOpened && timeout.ElapsedMilliseconds < 10000L && TradeUi.TradeControl.AcceptButtonText.ToLower() != "cancel accept")
													{
														await Coroutines.LatencyWait();
														Thread.Sleep(LokiPoe.Random.Next(500, 800));
														if (TradeUi.TradeControl.AcceptButtonText.ToLower() == "cancel accept")
														{
															break;
														}
														if (offerInChaos == CountOfferPriceInChaos())
														{
															TradeUi.TradeControl.Accept(true);
															await Coroutines.LatencyWait();
															continue;
														}
														goto IL_608f;
													}
													if (TradeUi.IsOpened && TradeUi.TradeControl.AcceptButtonText.ToLower() == "cancel accept")
													{
														await Wait.For(() => !TradeUi.IsOpened, "buyer to accept the offer", 100, 10000);
														if (TradeUi.IsOpened && TradeUi.TradeControl.AcceptButtonText.ToLower() != "cancel accept")
														{
															Thread.Sleep(ran3);
															continue;
														}
													}
													goto IL_6197;
												}
												goto IL_61eb;
											}
											goto IL_638b;
											IL_608f:
											retCountHardStopper += 5;
											IL_615f:;
										}
										await Coroutines.CloseBlockingWindows();
										GlobalLog.Debug("[HandleCurrencyTrade] Player could not put everything in trade in 1 minute, now kicking him");
										chatRequest_0.WasServed = true;
										if (IsInParty(string_0))
										{
											await SendChatMsg("/kick " + string_0);
										}
										foreach (Vector2i itemCoords3 in itemsInInventoryCoords)
										{
											Item item2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemCoords3);
											ItemSearchParams param3 = new ItemSearchParams(item2, item2.LocationTopLeft);
											TraderPlugin.ItemSearch.Enqueue(param3);
											Thread.Sleep(200);
										}
										ITask task21 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
										await task21.Run();
										DoAfterTrade();
										break;
									}
									GlobalLog.Debug("[HandleCurrencyTrade] Player is not accepting trade, retrying");
									continue;
								}
								await Coroutines.CloseBlockingWindows();
								GlobalLog.Debug("[HandleCurrencyTrade] Player is not accepting trade, now kicking him");
								chatRequest_0.WasServed = true;
								if (IsInParty(string_0))
								{
									await SendChatMsg("/kick " + string_0);
								}
								foreach (Vector2i itemCoords5 in itemsInInventoryCoords)
								{
									Item item4 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemCoords5);
									ItemSearchParams param4 = new ItemSearchParams(item4, item4.LocationTopLeft);
									TraderPlugin.ItemSearch.Enqueue(param4);
									Thread.Sleep(200);
								}
								ITask task22 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
								await task22.Run();
								DoAfterTrade();
								break;
								IL_6197:
								await Coroutines.CloseBlockingWindows();
								int afterTradeCurrency = Inventories.InventoryItems.Count((Item i) => i.Name == "Chaos Orb" || i.Name == "Divine Orb");
								if (currentCurrency == afterTradeCurrency)
								{
									continue;
								}
								soldItem.Price = offerInChaos;
								GlobalLog.Info($"[HandleCurrencyTrade]{soldItem}");
								TraderPlugin.SoldItems.Enqueue(soldItem);
								Class8.WriteToLog(soldItem);
								Random rn = new Random();
								Thread.Sleep(rn.Next(1000));
								GlobalLog.Debug("[HandleCurrencyTrade] Trade is successfully finished.");
								chatRequest_0.WasServed = true;
								try
								{
									if (IsInParty(string_0))
									{
										await SendChatMsg("/kick " + string_0);
									}
								}
								catch
								{
								}
								item_0 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemsInInventoryCoords[0]);
								if (!(item_0.Name == chatRequest_0.ItemName) && ((!item_0.Stats.ContainsKey((StatTypeGGG)14763) && !item_0.Stats.ContainsKey((StatTypeGGG)10342)) || !chatRequest_0.ItemName.Contains(item_0.Name)))
								{
									await SendChatMsg("@" + string_0 + " " + RandomThanks());
									if (File.Exists(TraderPlugin.FullFileName))
									{
										GlobalLog.Debug("[HandleTrade] Now deleting item from json");
										List<Class9.Class10> mnePohui = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
										Class9.Class10 entry = mnePohui.FirstOrDefault((Class9.Class10 i) => i.ItemPos == chatRequest_0.ItemPos && (((item_0.Stats.ContainsKey((StatTypeGGG)14763) || item_0.Stats.ContainsKey((StatTypeGGG)10342)) && chatRequest_0.ItemName.Contains(i.ItemName)) || i.ItemName == chatRequest_0.ItemName));
										mnePohui.Remove(entry);
										File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui, (Formatting)1));
									}
								}
								else
								{
									foreach (Vector2i itemCoords2 in itemsInInventoryCoords)
									{
										Item item7 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemCoords2);
										ItemSearchParams param2 = new ItemSearchParams(item7, item7.LocationTopLeft);
										TraderPlugin.ItemSearch.Enqueue(param2);
										Thread.Sleep(200);
									}
									ITask listTask = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
									await listTask.Run();
								}
								ITask task16 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
								CloseAllNotifications();
								await task16.Run();
								task16 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("SellTask");
								await task16.Run();
								task16 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("StashTask");
								await task16.Run();
								DoAfterTrade();
								break;
								IL_61eb:
								Thread.Sleep(ran3);
								TradeUi.TradeControl.Cancel(true);
								GlobalLog.Debug("[HandleCurrencyTrade] Offer is too low, now going to inform client");
								if (!((double)offerInChaos < (double)priceInChaos2 * 0.65))
								{
									if (chatRequest_0.TradeCount >= 2)
									{
										GlobalLog.Debug("[HandleCurrencyTrade] Kicking without hesitation. He knows why.");
										if (IsInParty(chatRequest_0.Name))
										{
											await SendChatMsg("/kick " + string_0);
										}
										chatRequest_0.WasServed = true;
										foreach (Vector2i itemCoords in itemsInInventoryCoords)
										{
											Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemCoords);
											ItemSearchParams param = new ItemSearchParams(item, item.LocationTopLeft);
											TraderPlugin.ItemSearch.Enqueue(param);
											Thread.Sleep(200);
										}
										ITask task13 = ((TaskManagerBase<ITask>)(object)TraderPlugin.BotTaskManager).GetTaskByName("ListItemsTask");
										await task13.Run();
										DoAfterTrade();
										break;
									}
									await SendChatMsg($"@{string_0} {priceInChaos2 - offerInChaos}c more");
									Thread.Sleep(LokiPoe.Random.Next(7000, 8000));
								}
								else if (offerInChaos == 0)
								{
									Thread.Sleep(LokiPoe.Random.Next(4000, 4500));
								}
								else if (chatRequest_0.TradeCount < 2)
								{
									await SendChatMsg("@" + string_0 + " " + RandomQuestionMark());
									Thread.Sleep(LokiPoe.Random.Next(7000, 8000));
								}
								continue;
								IL_638b:
								await Coroutines.ReactionWait();
							}
						}
						else
						{
							GlobalLog.Debug("[HandleTrade] Player " + string_0 + " is not in our hideout, now going to kick and ignore him");
							chatRequest_0.WasServed = true;
							if (IsInParty(string_0))
							{
								await SendChatMsg("/kick " + string_0);
							}
							DoAfterTrade();
						}
					}
					list_1 = list_2;
					DoAfterTrade();
					return false;
				}
				return false;
			}
			if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) != 0)
			{
				await SendChatMsg("/clear");
			}
			return false;
		}
		return false;
	}

	private static async Task DeclinePartyInvite(ICollection<string> acceptednames)
	{
		if (!(TraderPlugin.Stopwatch.Elapsed.TotalSeconds < (double)(5 * int_0)))
		{
			_ = InstanceInfo.PendingPartyInvites;
			int_0++;
			if (NotificationHud.NotificationList.Any((NotificationInfo x) => x.IsVisible))
			{
				await Coroutines.ReactionWait();
				GlobalLog.Debug("[CheckPartyInvite] small wait");
			}
			NotificationHud.HandleNotificationEx((ProcessNotificationEx)delegate(NotificationData x, NotificationType y)
			{
				GlobalLog.Warn($"[checkPartyInvite] Detected {((object)(NotificationType)(ref y)).ToString()} request from {x}");
				return false;
			}, true);
			await Coroutines.ReactionWait();
		}
	}

	private static bool AcceptTradeRequest(string charName)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		try
		{
			if (NotificationHud.NotificationList.Any((NotificationInfo x) => x.IsVisible))
			{
				ProcessNotificationEx val = (ProcessNotificationEx)delegate(NotificationData x, NotificationType y)
				{
					//IL_0060: Unknown result type (might be due to invalid IL or missing references)
					//IL_0062: Invalid comparison between Unknown and I4
					GlobalLog.Warn("[HandleTrade] Detected " + ((object)(NotificationType)(ref y)).ToString() + " request from charname " + x.CharacterName + ", our buyer charname: " + charName);
					return x.CharacterName == charName && (int)y == 0;
				};
				foreach (NotificationInfo notification in NotificationHud.NotificationList)
				{
					_ = notification;
					if ((int)NotificationHud.HandleNotificationEx(val, true) != 0)
					{
						GlobalLog.Warn("[HandleTrade] Decline");
						continue;
					}
					GlobalLog.Warn("[HandleTrade] Accepting");
					return true;
				}
				return false;
			}
			return false;
		}
		catch (Exception)
		{
			GlobalLog.Debug("[CloseAllNotifications] Error suppressed!");
			return false;
		}
	}

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

	public static void ProcessSystemMessage(ChatEntry newmessage)
	{
		if (newmessage.Message == "The operation could not be completed because you are already in a party.")
		{
			int_1++;
		}
		if (newmessage.Message == "The party does not exist.")
		{
			int_1++;
		}
	}

	public static bool FillTradeRequestList()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) == 0)
		{
			return false;
		}
		List<ChatEntry> messages = ChatPanel.Messages;
		bool result = false;
		Vector2i itemPos = default(Vector2i);
		foreach (ChatEntry item in messages)
		{
			try
			{
				if ((int)item.MessageType != 3 || !item.Message.Contains("Hi, I would like to buy your") || !item.Message.Contains("listed for") || !item.Message.Split(Convert.ToChar(" "))[0].Contains("From"))
				{
					if ((int)item.MessageType != 3 || !item.Message.Contains("Hi, I'd like to buy your") || !item.Message.Contains("for my") || !item.Message.Split(Convert.ToChar(" "))[0].Contains("From"))
					{
						continue;
					}
					string text = item.Message.Remove(0, 6);
					string text2 = text.Split(Convert.ToChar(" "))[0];
					if (text2.Contains("<"))
					{
						text2 = text.Split(Convert.ToChar(" "))[1];
					}
					text2 = text2.Replace(":", "");
					string[] array = new string[1] { "in " };
					string text3 = text.Split(array, StringSplitOptions.RemoveEmptyEntries)[0];
					array[0] = "for my ";
					text3 = text3.Split(array, StringSplitOptions.RemoveEmptyEntries)[1];
					string text4 = text3.Split(' ')[0];
					text4 = text4.Replace(".", ",");
					if (int.Parse(text4) < TraderPluginSettings.Instance.MinPriceInChaosToTrade)
					{
						continue;
					}
					if (!text3.Contains("Chaos Orb") && !text3.Contains("chaos"))
					{
						if (text3.Contains("Divine Orb") || text3.Contains("divine"))
						{
							text3 = "divine";
						}
					}
					else
					{
						text3 = "chaos";
					}
					array[0] = " for my";
					string input = text.Split(array, StringSplitOptions.RemoveEmptyEntries)[0];
					input = Regex.Replace(input, "[^\\u0000-\\u007F]+", string.Empty);
					array[0] = "buy your ";
					input = input.Split(array, StringSplitOptions.RemoveEmptyEntries)[1];
					if (input.Contains(","))
					{
						return false;
					}
					string value = input.Split(' ')[0];
					input = input.Replace(", ", " ");
					input = input.Split(new char[1] { ' ' }, 2)[1];
					input = input.Trim();
					ChatRequest chatRequest_ = new ChatRequest
					{
						Name = text2,
						ItemName = input,
						Price = Convert.ToDecimal(text4),
						Currency = text3,
						SellAmount = Convert.ToInt32(value),
						InviteCount = 0,
						WasServed = false,
						WasJsoned = false,
						FoundItem = false,
						IsSingleItemPrice = false,
						InfluenceType = InfluenceHelper.InfluenceType.None,
						CurrentTradePhase = ChatRequest.TradePhase.RequestProcessed
					};
					if (!list_0.Any((ChatRequest r) => r == chatRequest_))
					{
						GlobalLog.Warn($"[HandleTrade] Now adding new request to the list: Name: {chatRequest_.Name} Item: {chatRequest_.ItemName} Price: {chatRequest_.Price} {chatRequest_.Currency}");
						list_0.Add(chatRequest_);
						result = true;
					}
					continue;
				}
				string text5;
				string text6;
				string[] array2;
				string input2;
				string text7;
				if (!(item.Message.Split(Convert.ToChar(")"))[1] != "") || item.Message.Contains("Map"))
				{
					text5 = item.Message.Remove(0, 6);
					text5 = text5.Replace(")", "");
					text6 = text5.Split(Convert.ToChar(" "))[0];
					if (text6.Contains("<"))
					{
						text6 = text5.Split(Convert.ToChar(" "))[1];
					}
					text6 = text6.Replace(":", "");
					array2 = new string[1] { "your " };
					input2 = text5.Split(array2, StringSplitOptions.RemoveEmptyEntries)[1];
					input2 = Regex.Replace(input2, "[^\\u0000-\\u007F]+", string.Empty);
					if (input2.Contains("level"))
					{
						input2 = text5.Split(Convert.ToChar("%"))[1];
						input2 = input2.Remove(0, 1);
					}
					else if (input2.Contains("("))
					{
						input2 = input2.Split(Convert.ToChar("("))[0];
						input2 = input2.Remove(input2.Length - 1, 1);
						input2 = input2.Replace("Blighted ", "");
					}
					array2[0] = " listed";
					input2 = input2.Replace(", ", " ");
					input2 = input2.Split(array2, StringSplitOptions.RemoveEmptyEntries)[0];
					input2 = input2.Trim();
					if (text5.Contains("chaos"))
					{
						text7 = "chaos";
						goto IL_061e;
					}
					if (text5.Contains("divine"))
					{
						text7 = "divine";
						goto IL_061e;
					}
				}
				goto end_IL_0058;
				IL_061e:
				array2[0] = "for ";
				string text8 = text5.Split(array2, StringSplitOptions.RemoveEmptyEntries)[1];
				array2[0] = " " + text7;
				text8 = text8.Split(array2, StringSplitOptions.RemoveEmptyEntries)[0];
				text8 = text8.Replace(".", ",");
				if (int.Parse(text8) < TraderPluginSettings.Instance.MinPriceInChaosToTrade)
				{
					continue;
				}
				array2[0] = "stash tab ";
				string text9 = text5.Split(array2, StringSplitOptions.RemoveEmptyEntries)[1];
				text9 = text9.Split(Convert.ToChar(";"))[0];
				text9 = text9.Replace("\"", "");
				array2[0] = "position: ";
				string text10 = text5.Split(array2, StringSplitOptions.RemoveEmptyEntries)[1];
				text10 = text10.Replace(",", "");
				array2[0] = "left ";
				string text11 = text10.Split(array2, StringSplitOptions.RemoveEmptyEntries)[0];
				text11 = text11.Split(Convert.ToChar(" "))[0];
				array2[0] = "top ";
				string value2 = text10.Split(array2, StringSplitOptions.RemoveEmptyEntries)[1];
				((Vector2i)(ref itemPos))._002Ector(Convert.ToInt32(text11) - 1, Convert.ToInt32(value2) - 1);
				ChatRequest chatRequest_0 = new ChatRequest();
				if (!input2.Any(char.IsDigit))
				{
					chatRequest_0.Name = text6;
					chatRequest_0.ItemName = input2;
					chatRequest_0.Price = Convert.ToDecimal(text8);
					chatRequest_0.Currency = text7;
					chatRequest_0.TabName = text9;
					chatRequest_0.ItemPos = itemPos;
					chatRequest_0.InviteCount = 0;
					chatRequest_0.WasServed = false;
					chatRequest_0.WasJsoned = false;
					chatRequest_0.FoundItem = false;
					chatRequest_0.SellAmount = 0;
					chatRequest_0.IsSingleItemPrice = true;
					chatRequest_0.InfluenceType = InfluenceHelper.InfluenceType.None;
					chatRequest_0.CurrentTradePhase = ChatRequest.TradePhase.RequestProcessed;
					if (!list_0.Any((ChatRequest r) => r == chatRequest_0))
					{
						GlobalLog.Warn($"[HandleTrade] Now adding new request to the list: Name: {chatRequest_0.Name} Item: {chatRequest_0.ItemName} Price: {chatRequest_0.Price} {chatRequest_0.Currency} Stash tab: {chatRequest_0.TabName} Pos: {chatRequest_0.ItemPos}");
						list_0.Add(chatRequest_0);
						result = true;
					}
				}
				end_IL_0058:;
			}
			catch
			{
			}
		}
		return result;
	}

	public static bool IsInParty(string name)
	{
		return InstanceInfo.PartyMembers.Any((PartyMember m) => m.PlayerEntry.Name.EqualsIgnorecase(name));
	}

	public static bool IsPartyMember(string name)
	{
		return InstanceInfo.PartyMembers.Any((PartyMember m) => m.PlayerEntry.Name.EqualsIgnorecase(name) && (int)m.MemberStatus == 2);
	}

	public static bool IsInZone(string name, bool log = false)
	{
		IEnumerable<Player> enumerable = from p in ObjectManager.GetObjectsByType<Player>()
			where (NetworkObject)(object)p != (NetworkObject)(object)LokiPoe.Me
			select p;
		int num = 0;
		Player val = null;
		foreach (Player item in enumerable)
		{
			num++;
			if (((NetworkObject)item).Name.EqualsIgnorecase(name))
			{
				if (log)
				{
					GlobalLog.Warn($"[{num}] {((NetworkObject)(object)item).WalkablePosition()} found.");
				}
				val = item;
				break;
			}
		}
		return (NetworkObject)(object)val != (NetworkObject)null;
	}

	public ShouldViewItemDelegate View(Inventory inventory, Item item)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		object obj = _003C_003Ec.shouldViewItemDelegate_0;
		if (obj == null)
		{
			ShouldViewItemDelegate val = (Inventory s, Item item1) => true;
			obj = (object)val;
			_003C_003Ec.shouldViewItemDelegate_0 = val;
		}
		return (ShouldViewItemDelegate)obj;
	}

	public bool Open()
	{
		return TradeUi.IsOpened;
	}

	public static async Task<ChatResult> SendChatMsg(string msg, bool closeChatUi = true)
	{
		if (string.IsNullOrEmpty(msg))
		{
			return (ChatResult)0;
		}
		return await MapBotEx.SendChatMsg(msg);
	}

	private static string RandomQuestionMark()
	{
		List<string> list = new List<string>
		{
			"?", "??", "???", "Hello?", "hello?", "lol?", "lol", "wtf?", "wtf", "kekw",
			"lul", "lul?", "xd", "XD"
		};
		return list[LokiPoe.Random.Next(list.Count - 1)];
	}

	private static string RandomThanks()
	{
		List<string> list = new List<string> { "ty", "tyty", "thanks", "thanks!", "Thanks!", "t4t", "Thanks for the trade", "ty ss", "" };
		return list[LokiPoe.Random.Next(list.Count - 1)];
	}

	private string RandomOnly()
	{
		List<string> list = new List<string> { "only", "just", "have", "" };
		return list[LokiPoe.Random.Next(list.Count - 1)];
	}

	public static bool CheckItemInJson(ChatRequest player)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (!File.Exists(TraderPlugin.FullFileName))
		{
			player.WasJsoned = true;
			player.FoundItem = false;
			return false;
		}
		List<Class9.Class10> list = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
		if (player.SellAmount == 0)
		{
			if (TraderPluginSettings.Instance.DebugMode)
			{
				GlobalLog.Debug("[HandleTrade] Now checking if item is in the json");
			}
			foreach (Class9.Class10 item in list)
			{
				if (!TraderPluginSettings.Instance.DebugMode)
				{
				}
				if (item != null && player.ItemName.Contains(item.ItemName))
				{
					GlobalLog.Debug($"[HandleTrade] Found item with same name, stack count: {item.StackCount}");
					if (!(item.ItemPos != player.ItemPos) && item.StackCount == 1)
					{
						GlobalLog.Debug($"[HandleTrade] Found item with same coords: {item.ItemPos}");
						player.WasJsoned = true;
						player.FoundItem = true;
						player.CurrentTradePhase = ChatRequest.TradePhase.ItemsFound;
						return true;
					}
				}
			}
			player.WasJsoned = true;
			player.FoundItem = false;
			return false;
		}
		foreach (Class9.Class10 item2 in list)
		{
			if (!TraderPluginSettings.Instance.DebugMode)
			{
			}
			if (item2.ItemName != null && item2.ItemName.Contains(player.ItemName))
			{
				GlobalLog.Debug($"[HandleTrade] Found item with same name, stack count: {item2.StackCount}");
				num += item2.StackCount;
				GlobalLog.Debug($"[HandleTrade] Amount of items found: {num}");
			}
		}
		if (num < player.SellAmount)
		{
			player.WasJsoned = true;
			player.FoundItem = false;
			return false;
		}
		GlobalLog.Debug("[HandleTrade] Found enough items in stash");
		player.WasJsoned = true;
		player.FoundItem = true;
		player.CurrentTradePhase = ChatRequest.TradePhase.ItemsFound;
		return true;
	}

	private static void DoAfterTrade()
	{
		list_3.Clear();
		list_0 = list_2;
	}

	private void StartAIThread()
	{
		thread_0 = new Thread(CheckResponse)
		{
			IsBackground = true
		};
		GlobalLog.Debug("[TradeAI] Enabling conversation AI.");
		thread_0.Start();
	}

	private void StopAIThread()
	{
		thread_0?.Suspend();
	}

	private static void CheckResponse()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		if (ChatPanel.Messages.Count((ChatEntry m) => (int)m.MessageType == 3) == 0)
		{
			return;
		}
		List<ChatEntry> messages = ChatPanel.Messages;
		foreach (ChatEntry item in messages)
		{
			try
			{
				if ((int)item.MessageType != 3 || !item.Message.Split(Convert.ToChar(" "))[0].Contains("From") || item.Message.Contains("Hi, I'd like to buy your") || item.Message.Contains("for my") || item.Message.Contains("Hi, I would like to buy your") || item.Message.Contains("listed for"))
				{
					continue;
				}
				string text = item.Message.Remove(0, 6);
				string OflDluuIq7 = text.Split(Convert.ToChar(" "))[0];
				if (OflDluuIq7.Contains("<"))
				{
					OflDluuIq7 = text.Split(Convert.ToChar(" "))[1];
				}
				OflDluuIq7 = OflDluuIq7.Replace(":", "");
				string[] separator = new string[1] { OflDluuIq7 };
				text = text.Split(separator, StringSplitOptions.RemoveEmptyEntries)[1];
				ChatRequest chatRequest = list_1.FirstOrDefault((ChatRequest x) => x.Name == OflDluuIq7);
				if (!(chatRequest == null))
				{
					if (!ShouldChangeTradeDetails(chatRequest))
					{
						break;
					}
					if (text.ContainsIgnorecase("all sets"))
					{
						GlobalLog.Debug("[TradeAI] Customer wants all sets we have. Now going to count items.");
						chatRequest.SellAmount = CountItemsInJson(chatRequest, 0, shaper: true);
						break;
					}
					if (text.ContainsIgnorecase("set"))
					{
						GlobalLog.Debug("[TradeAI] Customer wants a number of sets. Now going to count items.");
						chatRequest.SellAmount = CountItemsInJson(chatRequest, 0, shaper: true);
						break;
					}
					if (text.ContainsIgnorecase("all shaper"))
					{
						GlobalLog.Debug("[TradeAI] Customer wants all shaper maps. Now going to count items.");
						chatRequest.SellAmount = CountItemsInJson(chatRequest, 0, shaper: true);
						break;
					}
					if (text.ContainsIgnorecase("all elder"))
					{
						GlobalLog.Debug("[TradeAI] Customer wants all elder maps. Now going to count items.");
						chatRequest.SellAmount = CountItemsInJson(chatRequest, 0, shaper: false, elder: true);
						break;
					}
					if (text.ContainsIgnorecase("all"))
					{
						GlobalLog.Debug("[TradeAI] Customer wants all items. Now going to count items.");
						chatRequest.SellAmount = CountItemsInJson(chatRequest);
						break;
					}
					int num = 0;
					num = (text.ContainsIgnorecase("one") ? 1 : num);
					num = (text.ContainsIgnorecase("two") ? 2 : num);
					num = ((!text.ContainsIgnorecase("three")) ? num : 3);
					num = (text.ContainsIgnorecase("four") ? 4 : num);
					num = (text.ContainsIgnorecase("five") ? 5 : num);
					num = ((!text.ContainsIgnorecase("six")) ? num : 6);
					num = ((!text.ContainsIgnorecase("seven")) ? num : 7);
					num = (text.ContainsIgnorecase("eight") ? 8 : num);
					num = (text.ContainsIgnorecase("nine") ? 9 : num);
					num = (text.ContainsIgnorecase("ten") ? 10 : num);
				}
			}
			catch
			{
			}
		}
	}

	private static int CountOfferPriceInChaos()
	{
		if (TradeUi.IsOpened)
		{
			int num = 0;
			int num2 = 0;
			foreach (Item item in TradeUi.TradeControl.InventoryControl_OtherOffer.Inventory.Items)
			{
				if (!(item.Name == "Chaos Orb"))
				{
					if (item.Name == "Divine Orb")
					{
						num2 += item.StackCount;
						GlobalLog.Debug($"[HandleTrade] Now adding {item.StackCount} div to the total div count");
					}
				}
				else
				{
					num += item.StackCount;
					GlobalLog.Debug($"[HandleTrade] Now adding {item.StackCount} chaoses to the total chaos count");
				}
			}
			int num3 = 0;
			int num4 = 0;
			num3 = num2 * (int)PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Divine Orb");
			num3 += num;
			num4 = num3;
			GlobalLog.Debug("[HandleTrade] Offer is " + num4 + " chaoses, " + num2 + " of them are div, " + num + " are chaos");
			GlobalLog.Debug("[HandleTrade] Price is " + num3 + " chaoses");
			return num4;
		}
		return 0;
	}

	private static bool ShouldChangeTradeDetails(ChatRequest player)
	{
		if (player.CurrentTradePhase == ChatRequest.TradePhase.ItemsFound || player.CurrentTradePhase == ChatRequest.TradePhase.WasInvited)
		{
			return true;
		}
		return false;
	}

	public static int CountItemsInJson(ChatRequest player, int amt = 0, bool shaper = false, bool elder = false)
	{
		int num = 0;
		InfluenceHelper.InfluenceType? influenceType = InfluenceHelper.InfluenceType.None;
		if (File.Exists(TraderPlugin.FullFileName))
		{
			List<Class9.Class10> list = JsonConvert.DeserializeObject<List<Class9.Class10>>(File.ReadAllText(TraderPlugin.FullFileName));
			if (TraderPluginSettings.Instance.DebugMode)
			{
				GlobalLog.Debug("[CountAllItemsInJson] Now checking if item is in the json");
			}
			InfluenceHelper.InfluenceType? influenceType2;
			foreach (Class9.Class10 item in list)
			{
				if (TraderPluginSettings.Instance.DebugMode)
				{
				}
				if (item != null && player.ItemName.Contains(item.ItemName))
				{
					GlobalLog.Debug($"[CountAllItemsInJson] Found item with same name, stack count: {item.StackCount}");
					influenceType2 = item.InfluenceType;
					if (!((influenceType2.GetValueOrDefault() == InfluenceHelper.InfluenceType.None) & influenceType2.HasValue))
					{
						GlobalLog.Debug($"[CountAllItemsInJson] Item had an influence! Now going to search for all items with influence {item.InfluenceType}");
						influenceType = item.InfluenceType;
						player.InfluenceType = item.InfluenceType;
						break;
					}
					num += item.StackCount;
					if (num == amt)
					{
						return amt;
					}
				}
			}
			influenceType2 = influenceType;
			if (!((influenceType2.GetValueOrDefault() == InfluenceHelper.InfluenceType.None) & influenceType2.HasValue))
			{
				foreach (Class9.Class10 item2 in list)
				{
					if (TraderPluginSettings.Instance.DebugMode)
					{
					}
					if (elder)
					{
					}
					if (item2 != null && item2.InfluenceType == influenceType && (!elder || item2.InfluenceType == InfluenceHelper.InfluenceType.Enslaver || item2.InfluenceType == InfluenceHelper.InfluenceType.Constrictor || item2.InfluenceType == InfluenceHelper.InfluenceType.Eradicator || item2.InfluenceType == InfluenceHelper.InfluenceType.Purifier) && (!shaper || item2.InfluenceType == InfluenceHelper.InfluenceType.Enslaver || item2.InfluenceType == InfluenceHelper.InfluenceType.Constrictor || item2.InfluenceType == InfluenceHelper.InfluenceType.Eradicator || item2.InfluenceType == InfluenceHelper.InfluenceType.Purifier))
					{
						num += item2.StackCount;
						if (num == amt)
						{
							return amt;
						}
					}
				}
			}
		}
		return num;
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "SuspendTrades")
		{
			bool_1 = true;
			TraderPlugin.TradeTimeout.Restart();
			GlobalLog.Debug("[HandleTrade] Stopwatch Started");
			return (MessageResult)0;
		}
		if (message.Id == "blight_finished")
		{
			int_3 = (int)TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
			list_0.Clear();
			list_1.Clear();
			return (MessageResult)0;
		}
		if (message.Id == "MB_map_portal_entered_event")
		{
			bool_0 = true;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
		if (TraderPluginSettings.Instance.EnableConversationAI)
		{
			StartAIThread();
		}
	}

	public void Stop()
	{
		StopAIThread();
	}

	public void Tick()
	{
	}

	static Class3()
	{
		interval_0 = new Interval(15000);
		list_0 = new List<ChatRequest>();
		list_1 = new List<ChatRequest>();
		list_2 = new List<ChatRequest>();
		list_3 = new List<string>();
		int_2 = 18000;
	}

	[CompilerGenerated]
	internal static bool smethod_0(NotificationData x, NotificationType y)
	{
		GlobalLog.Warn($"[checkPartyInvite] Detected {((object)(NotificationType)(ref y)).ToString()} request from {x}");
		return false;
	}
}
