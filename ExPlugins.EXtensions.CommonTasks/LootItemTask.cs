using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;

namespace ExPlugins.EXtensions.CommonTasks;

public class LootItemTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool IsInPreTownrunMode;

	private static bool bool_0;

	private static readonly Interval interval_0;

	private static CachedWorldItem cachedWorldItem_0;

	private static readonly HashSet<int> hashSet_0;

	private static readonly Interval interval_1;

	private static bool PortalNearby => ObjectManager.Objects.Any((Portal p) => p.IsPlayerPortal() && ((NetworkObject)p).Distance <= 20f);

	private static bool BreachActive => Enumerable.Any(ObjectManager.GetObjectsByType<BreachClientObject>(), (BreachClientObject b) => (int)((NetworkObject)b).Components.BreachObject.State > 0);

	public string Name => "LootItemTask";

	public string Description => "Task that handles item looting.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public void Tick()
	{
		if (interval_0.Elapsed || bool_0)
		{
			bool_0 = false;
			if (StashTask.ForcedStash || interval_1.Elapsed)
			{
				hashSet_0.Clear();
			}
			SkipRecheck();
		}
	}

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame)
		{
			return false;
		}
		if (World.CurrentArea.IsCombatArea)
		{
			if (!Blight.IsEncounterRunning || !BlightUi.IsOpened)
			{
				if (BreachActive)
				{
					return true;
				}
				if (StashTask.ForcedStash)
				{
					ITask task2 = ((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).GetTaskByName("StashTask");
					await task2.Run();
					return true;
				}
				bool marketSeller = Enumerable.FirstOrDefault(PluginManager.EnabledPlugins, (IPlugin n) => ((IAuthored)n).Name == "TraderPlugin") != null;
				List<CachedObject> cystCluster = (from c in Enumerable.Where(CombatAreaCache.Current.SpecialChests, (CachedObject o) => o.Object != (NetworkObject)null && o.Object.Distance < 15f && o.Object.Metadata.Contains("Chests/Blight") && !o.Ignored)
					orderby c.Position.Distance
					select c).ToList();
				List<CachedStrongbox> strongboxes = (from c in Enumerable.Where(CombatAreaCache.Current.Strongboxes, (CachedStrongbox o) => (NetworkObject)(object)o.Object != (NetworkObject)null && ((NetworkObject)o.Object).Distance < 35f && !o.Object.IsLocked && !o.Ignored)
					orderby c.Position.Distance
					select c).ToList();
				if (!LocalData.MapMods.ContainsKey((StatTypeGGG)10342) || (!cystCluster.Any() && !strongboxes.Any()) || CombatAreaCache.Current.Items.Count((CachedWorldItem i) => (NetworkObject)(object)i.Object != (NetworkObject)null && i.Position.Distance < 20 && i.Object.HasVisibleHighlightLabel) >= 3)
				{
					List<CachedWorldItem> validItems = (from i in Enumerable.Where(CombatAreaCache.Current.Items, (CachedWorldItem i) => !i.Ignored && !i.Unwalkable)
						orderby CoordinateExtensions.MapToWorld3(LokiPoe.MyPosition, true).ScreenDistance(i.LabelPos)
						select i).ToList();
					if (validItems.Count != 0)
					{
						HashSet<CachedWorldItem> nearbyItems = Enumerable.Where(validItems, (CachedWorldItem i) => !hashSet_0.Contains(i.Id) && i.Position.Distance < 20 && CanFit(i.Size, Inventories.AvailableInventorySquares)).ToHashSet();
						if (nearbyItems.Count <= 2 || Enumerable.Any(CombatAreaCache.Current.Monsters, (CachedMonster m) => m.Position.Distance < 30))
						{
							if (cachedWorldItem_0 == null)
							{
								if (!validItems.Any())
								{
									return false;
								}
								int scrollAmount = Enumerable.Where(InventoryUi.InventoryControl_Main.Inventory.Items, (Item i) => i.Name.Equals("Scroll of Wisdom")).Sum((Item i) => i.StackCount);
								if (scrollAmount < 2 && ((IAuthored)BotManager.Current).Name == "MapBotEx" && marketSeller)
								{
									await PlayerAction.TpToTown();
									return true;
								}
								if (IsInPreTownrunMode)
								{
									int int_0 = Inventories.AvailableInventorySquares;
									cachedWorldItem_0 = (from i in Enumerable.Where(validItems, (CachedWorldItem i) => CanFit(i.Size, int_0, (int)i.Rarity == 6))
										orderby i.Position.Distance, (int)i.Rarity == 6 descending, i.Size.X * i.Size.Y, (int)i.Rarity == 3 descending, (int)i.Rarity == 5 descending
										select i).FirstOrDefault();
									if (cachedWorldItem_0 == null)
									{
										if (World.CurrentArea.Id.Contains("Affliction"))
										{
											StashTask.ForcedStash = true;
										}
										else
										{
											await PlayerAction.TpToTown();
										}
										ReturnAfterTownrunTask.Enabled = true;
										return true;
									}
								}
								else
								{
									cachedWorldItem_0 = validItems.OrderBy((CachedWorldItem i) => i.Position.Distance).First();
								}
							}
							WorldItem itemObj = cachedWorldItem_0.Object;
							if (!((NetworkObject)(object)itemObj == (NetworkObject)null))
							{
								WalkablePosition pos = cachedWorldItem_0.Position;
								if (!ExtensionsSettings.Instance.SkipItemsFilteredIngame || (!(((NetworkObject)itemObj).Components.RenderComponent.InteractCenterWorld == Vector3.Zero) && !string.IsNullOrEmpty(((NetworkObject)itemObj).AnimatedPropertiesMetadata)))
								{
									Item actualItem = itemObj.Item;
									CachedItem cached = new CachedItem(actualItem);
									double lookupPrice = -1.0;
									if (GeneralSettings.Instance.EnableMapLootStatistics)
									{
										lookupPrice = PoeNinjaTracker.LookupChaosValue(actualItem);
									}
									if (ExtensionsSettings.Instance.SkipLootBasedOnValueAndRange)
									{
										double price2 = ((lookupPrice > -1.0) ? lookupPrice : PoeNinjaTracker.LookupChaosValue(actualItem));
										int dist = pos.Distance;
										if (price2 > -1.0 && price2 <= ExtensionsSettings.Instance.LootSkipPrice && dist > ExtensionsSettings.Instance.LootSkipRange)
										{
											string itemString = ((itemObj.Item.StackCount > 1) ? $"{itemObj.Item.StackCount}X {itemObj.Item.Name}" : (itemObj.Item.Name ?? ""));
											GlobalLog.Debug($"[{Name}] Skipping {itemString}: price: {price2} < {ExtensionsSettings.Instance.LootSkipPrice} | distance: {dist > ExtensionsSettings.Instance.LootSkipRange}");
											CombatAreaCache.Current.Items.Remove(cachedWorldItem_0);
											cachedWorldItem_0 = null;
											return true;
										}
									}
									if (pos.Distance <= 30)
									{
										if (CanFit(cachedWorldItem_0.Size, Inventories.AvailableInventorySquares))
										{
											int attempts = ++cachedWorldItem_0.InteractionAttempts;
											if (attempts > 10)
											{
												double price = ((lookupPrice > -1.0) ? lookupPrice : PoeNinjaTracker.LookupChaosValue(actualItem));
												if (cachedWorldItem_0.Position.Name == CurrencyNames.Mirror || price > 500.0)
												{
													string errorName = $"Fail to pick up the {cachedWorldItem_0.Name} (price: {price}c). Now stopping the bot.";
													GlobalLog.Error("[LootItemTask]" + errorName);
													Utility.BroadcastMessage((object)null, "lootitem_mirror_fail", new object[1] { errorName });
													BotManager.Stop(new StopReasonData("lootitem_mirror_fail", errorName, (object)null), false);
												}
												else
												{
													GlobalLog.Error("[LootItemTask] All attempts to pick up an item have been spent. Now ignoring it.");
													cachedWorldItem_0.Ignored = true;
													hashSet_0.Add(cachedWorldItem_0.Id);
													cachedWorldItem_0 = null;
												}
												return true;
											}
											if (attempts % 3 == 0)
											{
												await PlayerAction.DisableAlwaysHighlight();
											}
											if (attempts == 5)
											{
												if (PortalNearby)
												{
													GlobalLog.Debug("[LootItemTask] There is a portal nearby, which probably blocks current item label.");
													if (await PlayerAction.MoveAway(40, 70))
													{
														GlobalLog.Debug("[LootItemTask] Now going to create a new portal at some distance.");
														await PlayerAction.CreateTownPortal();
													}
												}
												return true;
											}
											await Coroutines.CloseBlockingWindows();
											if (!(await PlayerAction.FastPickup(itemObj)))
											{
												return true;
											}
											if ((NetworkObject)(object)cachedWorldItem_0.Object != (NetworkObject)null)
											{
												return true;
											}
											Utility.BroadcastMessage((object)this, "item_looted_event", new object[1] { cached });
											if (cached.IsIdentified)
											{
												Utility.BroadcastMessage((object)this, "Identified Item Looted", new object[1] { cached });
											}
											if (GeneralSettings.Instance.EnableMapLootStatistics)
											{
												KeyValuePair<Item, double> pricePair = new KeyValuePair<Item, double>(actualItem, lookupPrice * (double)actualItem.StackCount);
												if (pricePair.Value > -1.0)
												{
													Utility.BroadcastMessage((object)null, "priced_item_looted_event", new object[1] { pricePair });
												}
											}
											CombatAreaCache.Current.RemoveItemFromCache(cachedWorldItem_0);
											cachedWorldItem_0 = null;
											return true;
										}
										IsInPreTownrunMode = true;
										cachedWorldItem_0 = null;
										return true;
									}
									if (!pos.TryCome())
									{
										if (!((NetworkObject)(object)cachedWorldItem_0.Object != (NetworkObject)null))
										{
											GlobalLog.Error($"[LootItemTask] Fail to move to {pos}. item Object is null, removing item from the cache and reevaluating it.");
											CombatAreaCache.Current.RemoveItemFromCache(cachedWorldItem_0);
											cachedWorldItem_0 = null;
										}
										else
										{
											GlobalLog.Error($"[LootItemTask] Fail to move to {pos}. Marking this item as unwalkable.");
											cachedWorldItem_0.Unwalkable = true;
											cachedWorldItem_0 = null;
										}
										return true;
									}
									return true;
								}
								SkipRecheck();
								cachedWorldItem_0 = null;
								return true;
							}
							CombatAreaCache.Current.RemoveItemFromCache(cachedWorldItem_0);
							cachedWorldItem_0 = null;
							return true;
						}
						await nearbyItems.First().Position.TryComeAtOnce(5);
						await FastLooting(nearbyItems);
						cachedWorldItem_0 = null;
						return true;
					}
					return false;
				}
				ITask task = ((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).GetTaskByName("OpenChestTask");
				await task.Run();
				return true;
			}
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		switch (message.Id)
		{
		case "area_changed_event":
			cachedWorldItem_0 = null;
			IsInPreTownrunMode = false;
			hashSet_0.Clear();
			UnignoreStrongboxItems();
			return (MessageResult)0;
		case "ResetCurrentItem":
			cachedWorldItem_0 = null;
			return (MessageResult)0;
		default:
			return (MessageResult)1;
		case "SetCurrentItem":
			cachedWorldItem_0 = message.GetInput<CachedWorldItem>(0);
			return (MessageResult)0;
		case "GetCurrentItem":
			message.AddOutput<CachedWorldItem>((IMessageHandler)(object)this, cachedWorldItem_0, "");
			return (MessageResult)0;
		}
	}

	private static void UnignoreStrongboxItems()
	{
		if (!World.CurrentArea.IsCombatArea)
		{
			return;
		}
		foreach (CachedWorldItem item in Enumerable.Where(CombatAreaCache.Current.Items, (CachedWorldItem i) => i.Ignored))
		{
			GlobalLog.Debug("[LootItemTask] Removing ignored flag from " + item.Object.Item.Name + " because we changed area.");
			item.Ignored = false;
		}
	}

	private static void SkipRecheck()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		foreach (CachedWorldItem item in CombatAreaCache.Current.Items.ToList())
		{
			WorldItem @object = item.Object;
			if ((NetworkObject)(object)@object == (NetworkObject)null)
			{
				continue;
			}
			string text = ((@object.Item.StackCount > 1) ? $"{@object.Item.StackCount}X {@object.Item.Name}" : (@object.Item.Name ?? ""));
			if (!ExtensionsSettings.Instance.SkipItemsFilteredIngame || !(((NetworkObject)@object).Components.RenderComponent.InteractCenterWorld == Vector3.Zero))
			{
				if (@object.HasAllocation && @object.IsAllocatedToOther)
				{
					if (!(@object.AllocatedToOtherTime > TimeSpan.FromMinutes(2.0)))
					{
						Blacklist.Add(((NetworkObject)@object).Id, @object.AllocatedToOtherTime, $"{text} is allocated to someone else for {@object.AllocatedToOtherTime.TotalSeconds} sec");
						continue;
					}
					GlobalLog.Warn("[LootItemTask] Skipping " + text + " because it's allocated to someone else!");
					CombatAreaCache.Current.Items.Remove(item);
				}
			}
			else
			{
				GlobalLog.Debug("[LootItemTask] Skipping " + text + " because it's filtered by in-game filter!");
				CombatAreaCache.Current.Items.Remove(item);
			}
		}
	}

	private static bool CanFit(Vector2i size, int availableSquares, bool ignoreReservedSpace = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		int num = size.X * size.Y;
		int num2 = (ignoreReservedSpace ? 1 : ExtensionsSettings.Instance.MinInventorySquares);
		return availableSquares - num >= num2 && InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)0).CanFitItem(size);
	}

	private static async Task FastLooting(IEnumerable<CachedWorldItem> itemsNear)
	{
		HashSet<CachedWorldItem> toRemove = new HashSet<CachedWorldItem>();
		await Coroutines.CloseBlockingWindows();
		foreach (CachedWorldItem item in itemsNear.ToHashSet())
		{
			WalkablePosition pos = item.Position;
			if (StashTask.ForcedStash)
			{
				break;
			}
			hashSet_0.Add(item.Id);
			cachedWorldItem_0 = item;
			WorldItem itemObj = item.Object;
			if ((NetworkObject)(object)itemObj == (NetworkObject)null)
			{
				toRemove.Add(item);
				cachedWorldItem_0 = null;
				continue;
			}
			bool flag = pos.Distance > 12;
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = !(await pos.TryComeAtOnce(5));
			}
			if (!flag2)
			{
				if (ExtensionsSettings.Instance.SkipItemsFilteredIngame && (((NetworkObject)itemObj).Components.RenderComponent.InteractCenterWorld == Vector3.Zero || string.IsNullOrEmpty(((NetworkObject)itemObj).AnimatedPropertiesMetadata)))
				{
					SkipRecheck();
					cachedWorldItem_0 = null;
					continue;
				}
				if (!CanFit(cachedWorldItem_0.Size, Inventories.AvailableInventorySquares))
				{
					if (World.CurrentArea.Id.Contains("Affliction"))
					{
						StashTask.ForcedStash = true;
					}
					IsInPreTownrunMode = true;
					cachedWorldItem_0 = null;
					continue;
				}
				Item actualItem = itemObj.Item;
				CachedItem cached = new CachedItem(actualItem);
				double lookupPrice = -1.0;
				if (GeneralSettings.Instance.EnableMapLootStatistics)
				{
					lookupPrice = PoeNinjaTracker.LookupChaosValue(actualItem);
				}
				if (!(await PlayerAction.FastPickup(itemObj)) || (NetworkObject)(object)item.Object != (NetworkObject)null)
				{
					continue;
				}
				Utility.BroadcastMessage((object)null, "item_looted_event", new object[1] { cached });
				if (cached.IsIdentified)
				{
					Utility.BroadcastMessage((object)null, "Identified Item Looted", new object[1] { cached });
				}
				if (GeneralSettings.Instance.EnableMapLootStatistics)
				{
					KeyValuePair<Item, double> pricePair = new KeyValuePair<Item, double>(actualItem, lookupPrice * (double)actualItem.StackCount);
					if (pricePair.Value > -1.0)
					{
						Utility.BroadcastMessage((object)null, "priced_item_looted_event", new object[1] { pricePair });
					}
				}
				toRemove.Add(item);
				cachedWorldItem_0 = null;
			}
			else
			{
				item.Unwalkable = true;
			}
		}
		foreach (CachedWorldItem id in toRemove)
		{
			CombatAreaCache.Current.RemoveItemFromCache(id);
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static LootItemTask()
	{
		bool_0 = true;
		interval_0 = new Interval(500);
		hashSet_0 = new HashSet<int>();
		interval_1 = new Interval(5000);
	}
}
