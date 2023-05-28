using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.ItemFilterEx;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.MapBotEx.Tasks;

namespace ExPlugins.EXtensions;

public static class Inventories
{
	public enum ClearCursorResults
	{
		None,
		InventoryNotOpened,
		NoSpaceInInventory,
		TriesReached
	}

	private static readonly Dictionary<string, Func<InventoryControlWrapper>> dictionary_0;

	private static readonly Dictionary<string, Func<InventoryControlWrapper>> dictionary_1;

	public static List<Item> InventoryItems => InstanceInfo.GetPlayerInventoryItemsBySlot((InventorySlot)0);

	public static List<Item> StashTabItems => StashUi.InventoryControl.Inventory.Items;

	public static NetworkObject Stash => (NetworkObject)(object)ObjectManager.Objects.FirstOrDefault<Stash>();

	public static CachedObject CachedStash
	{
		get
		{
			return CombatAreaCache.Current.Storage["CachedStash"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CachedStash"] = value;
		}
	}

	public static int AvailableInventorySquares => InstanceInfo.GetPlayerInventoryBySlot((InventorySlot)0).AvailableInventorySquares;

	public static async Task<bool> OpenStash()
	{
		if (!StashUi.IsOpened)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			CachedObject stashObj = CachedStash;
			MinimapIconWrapper minimapStash = Enumerable.FirstOrDefault(InstanceInfo.MinimapIcons, (MinimapIconWrapper i) => i.MinimapIcon.Name.Equals("StashPlayer"));
			Vector2i minimapStashPos = new Vector2i(Vector2i.Zero);
			if (minimapStash != null)
			{
				minimapStashPos = minimapStash.LastSeenPosition;
			}
			WalkablePosition stashPos = new WalkablePosition("EMPTY STASH POS", Vector2i.Zero);
			if (World.CurrentArea.IsTown && minimapStashPos == Vector2i.Zero)
			{
				GlobalLog.Warn("[OpenStash] We're in town. Let's use default positions.");
				stashPos = StaticPositions.GetStashPosByAct();
			}
			if (minimapStashPos != Vector2i.Zero)
			{
				stashPos = new WalkablePosition("Stash minimap icon", minimapStashPos, 5);
			}
			if (stashObj != null)
			{
				stashPos = stashObj.Position;
			}
			if (stashPos.AsVector == Vector2i.Zero)
			{
				GlobalLog.Error("[OpenStash] Fail to find any Stash nearby.");
				if (area.Id.Contains("Affliction"))
				{
					GlobalLog.Warn("[OpenStash] We're in simulacrum. Let's explore a bit.");
					await CombatAreaCache.Current.Explorer.Execute();
					return false;
				}
				return false;
			}
			await Coroutines.CloseBlockingWindows();
			if (stashPos.Distance > 35)
			{
				if (ExtensionsSettings.Instance.HumanizerNew && LokiPoe.CurrentWorldArea.IsTown && Wait.StashPauseProbability(30))
				{
					WorldPosition pos = WorldPosition.FindRandomPositionForMove(stashPos);
					if (pos != null)
					{
						await Move.AtOnce(pos, "To Random Location and pause.");
						await Wait.TownMoveRandomDelay();
					}
				}
				if (!(await stashPos.TryComeAtOnce()))
				{
					return false;
				}
				await Coroutines.FinishCurrentAction(true);
				await Wait.LatencySleep();
			}
			await Coroutines.FinishCurrentAction(true);
			await PlayerAction.Interact(Stash);
			if (!(await Wait.For(() => StashUi.IsOpened && StashUi.StashTabInfo != null, "stash ui to load", 50, 1500)))
			{
				if (((Player)LokiPoe.Me).Hideout != null && !area.IsHideoutArea && !area.Id.Contains("Affliction"))
				{
					GlobalLog.Error("[Inventories] Stash openning failed. Let's try to go to hideout!");
					await PlayerAction.GoToHideout();
					return false;
				}
				return false;
			}
			return true;
		}
		return true;
	}

	public static async Task<bool> OpenGuildStash()
	{
		if (World.CurrentArea.IsMap)
		{
			return false;
		}
		if (!GuildStashUi.IsOpened)
		{
			NetworkObject stashObj = ObjectManager.GetObjectByMetadata("Metadata/MiscellaneousObjects/GuildStash");
			if (stashObj == (NetworkObject)null)
			{
				GlobalLog.Error("[OpenGuildStash] Fail to find any Guild Stash nearby.");
				return false;
			}
			await Coroutines.CloseBlockingWindows();
			WalkablePosition stashPos = stashObj.WalkablePosition();
			if (stashPos.Distance > 35)
			{
				if (ExtensionsSettings.Instance.HumanizerNew && LokiPoe.CurrentWorldArea.IsTown)
				{
					if (Wait.StashPauseProbability(70))
					{
						WorldPosition pos2 = WorldPosition.FindRandomPositionForMove(((NetworkObject)LokiPoe.Me).Position);
						if (pos2 != null)
						{
							await Move.AtOnce(pos2, "To Random Location and pause.");
							await Wait.TownMoveRandomDelay();
						}
					}
					if (LokiPoe.CurrentWorldArea.IsTown && Wait.StashPauseProbability(30))
					{
						WorldPosition pos = WorldPosition.FindRandomPositionForMove(stashPos);
						if (pos != null)
						{
							await Move.AtOnce(pos, "To Random Location and pause.");
							await Wait.TownMoveRandomDelay();
						}
					}
				}
				if (!(await stashPos.TryComeAtOnce()))
				{
					return false;
				}
			}
			await PlayerAction.DisableAlwaysHighlight();
			if (!(await PlayerAction.Interact(stashObj, () => GuildStashUi.IsOpened && GuildStashUi.StashTabInfo != null, "guild stash opening")))
			{
				return false;
			}
			await Wait.SleepSafe(LokiPoe.Random.Next(200, 400));
			await Wait.Sleep(100);
			return true;
		}
		return true;
	}

	public static async Task<bool> OpenStashTab(string tabName, string caller = "")
	{
		if (await OpenStash())
		{
			if (!tabName.Contains("Remove-only"))
			{
				List<string> ignored = ExtensionsSettings.Instance.GeneralStashingRules.Find((ExtensionsSettings.StashingRule r) => r.Name == "Tabs to ignore").TabList;
				if (!ignored.Contains(tabName))
				{
					bool bool_0 = StashUi.StashTabInfo.IsUniqueCollection || StashUi.StashTabInfo.IsPremiumDivination || StashUi.StashTabInfo.IsFolder || StashUi.StashTabInfo.IsPremiumGem || StashUi.StashTabInfo.IsPremiumFlask;
					if (!(StashUi.TabControl.CurrentTabName == tabName))
					{
						GlobalLog.Debug("[OpenStashTab] Now switching to tab \"" + tabName + "\". [" + caller + "]");
						int int_0 = StashUi.StashTabInfo.InventoryId;
						try
						{
							SwitchToTabResult err = ((Math.Abs(StashUi.TabControl.TabNames.FindIndex((string t) => t.Equals(tabName)) - StashUi.TabControl.CurrentTabIndex) <= 1 || ExtensionsSettings.Instance.ForceKeyboardSwitch) ? StashUi.TabControl.SwitchToTabKeyboard(tabName) : StashUi.TabControl.SwitchToTabMouse(tabName));
							if ((int)err > 0)
							{
								GlobalLog.Error($"[OpenStashTab] Fail to switch to tab \"{tabName}\". Error \"{err}\".");
								ExtensionsSettings.Instance.ForceKeyboardSwitch = true;
								return false;
							}
						}
						catch (Exception)
						{
							GlobalLog.Error("Exception while using mouse. Forcing keyboard switch for this session");
							ExtensionsSettings.Instance.ForceKeyboardSwitch = true;
						}
						if (!(await Wait.For(() => bool_0 || StashUi.StashTabInfo.InventoryId != int_0, "stash tab switching")))
						{
							return false;
						}
						if (await Wait.For(() => bool_0 || StashUi.StashTabInfo != null, "stash tab loading"))
						{
							if (StashUi.StashTabInfo.IsPremiumSpecial)
							{
								return true;
							}
							return await Wait.For(() => (RemoteMemoryObject)(object)StashUi.InventoryControl != (RemoteMemoryObject)null, "items loading");
						}
						return false;
					}
					return true;
				}
				GlobalLog.Warn("[OpenStashTab] Ignoring tab [" + tabName + "] because it's name is ignore flag.");
				return true;
			}
			GlobalLog.Debug("[OpenStashTab] Ignoring tab [" + tabName + "] because it's Remove-Only");
			return true;
		}
		return false;
	}

	public static async Task<bool> OpenGuildStashTab(string tabName)
	{
		if (!(await OpenGuildStash()))
		{
			return false;
		}
		if (!tabName.Contains("Remove-only"))
		{
			if (GuildStashUi.TabControl.CurrentTabName != tabName)
			{
				GlobalLog.Debug("[OpenStashTab] Now switching to tab \"" + tabName + "\".");
				int int_0 = GuildStashUi.StashTabInfo.InventoryId;
				SwitchToTabResult err = GuildStashUi.TabControl.SwitchToTabKeyboard(tabName);
				if ((int)err > 0)
				{
					GlobalLog.Error($"[OpenStashTab] Fail to switch to tab \"{tabName}\". Error \"{err}\".");
					return false;
				}
				if (!(await Wait.For(() => GuildStashUi.StashTabInfo != null && GuildStashUi.StashTabInfo.InventoryId != int_0, "guild stash tab switching")))
				{
					return false;
				}
				await Wait.SleepSafe(LokiPoe.Random.Next(200, 400));
				await Wait.Sleep(100);
			}
			if (await Wait.For(() => GuildStashUi.StashTabInfo != null, "stash tab loading"))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public static async Task<int> CountMapsInTabs()
	{
		List<string> tabs = ExtensionsSettings.Instance.GetTabsForCategory("Maps");
		List<string> tabs3 = ExtensionsSettings.Instance.GetTabsForCategory("Simulacrums");
		List<string> tabs4 = ExtensionsSettings.Instance.GetTabsForCategory("Blighted Maps");
		tabs = Enumerable.Concat(second: ExtensionsSettings.Instance.GetTabsForCategory("Influenced Maps"), first: tabs.Concat(tabs3).Concat(tabs4)).Distinct().ToList();
		int temp = 0;
		int[] tiers = (from _ in Enumerable.Where(Enumerable.Range(1, 18), (int t) => t != 17)
			orderby Guid.NewGuid()
			select _).ToArray();
		foreach (string tab in tabs)
		{
			await OpenStashTab(tab, "CountMapsInTabs");
			temp = ((!StashUi.StashTabInfo.IsPremiumMap) ? (temp + StashTabItems.Count(MapExtensions.IsMap)) : (temp + ((IEnumerable<int>)tiers).Sum((Func<int, int>)MapsTab.GetMapsCountPerTier)));
		}
		return temp;
	}

	public static async Task<CachedMaps.MapTabInfo> CacheMapTabs(bool full = false)
	{
		int[] tiers = (from _ in Enumerable.Range(1, 18)
			orderby Guid.NewGuid()
			select _).ToArray();
		List<CachedMaps.MapTierInfo> temp = new List<CachedMaps.MapTierInfo>();
		HashSet<CachedMapItem> allMaps = new HashSet<CachedMapItem>();
		List<string> tabs = ExtensionsSettings.Instance.GetTabsForCategory("Maps");
		List<string> tabs3 = ExtensionsSettings.Instance.GetTabsForCategory("Simulacrums");
		List<string> tabs4 = ExtensionsSettings.Instance.GetTabsForCategory("Blighted Maps");
		tabs = Enumerable.Concat(second: ExtensionsSettings.Instance.GetTabsForCategory("Influenced Maps"), first: tabs.Concat(tabs3).Concat(tabs4)).Distinct().ToList();
		foreach (string tab in tabs)
		{
			await OpenStashTab(tab, "CacheMapTabs");
			if (!StashUi.StashTabInfo.IsPremiumMap)
			{
				if (StashUi.StashTabInfo.IsPremiumSpecial)
				{
					string errorText = $"[CacheMapTabs] {StashUi.TabControl.CurrentTabName} [type:{StashUi.StashTabInfo.TabType}] is not supported for maps. Please remove it from stashing rules.";
					GlobalLog.Error(errorText);
					BotManager.Stop(new StopReasonData("unsupported_tab", errorText, (object)null), false);
					continue;
				}
				List<Item> list_ = StashTabItems.Where(MapExtensions.IsMap).ToList();
				foreach (CachedMapItem map3 in list_.Select((Item m) => new CachedMapItem(m)))
				{
					allMaps.Add(map3);
				}
				int[] array = tiers;
				foreach (int int_0 in array)
				{
					HashSet<MapInfo> hashset2 = Enumerable.Where(list_, (Item m) => m.MapTier == int_0).Select((Func<Item, MapInfo>)((Item map) => new MapInfo
					{
						Name = map.Name,
						IsWitnessedByTheMaven = false,
						MapsCount = list_.Count((Item m) => m.MapTier == int_0)
					})).ToHashSet();
					if (hashset2.Any())
					{
						temp.Add(new CachedMaps.MapTierInfo(int_0, hashset2));
					}
				}
				continue;
			}
			foreach (int tier in Enumerable.Where(tiers, (int t) => t != 17))
			{
				List<Item> list_0;
				while (true)
				{
					IL_09a6:
					int count = MapsTab.GetMapsCountPerTier(tier);
					if (count <= 0)
					{
						break;
					}
					SelectTierResult err = MapsTab.SelectTier(tier);
					if ((int)err > 0)
					{
						break;
					}
					HashSet<MapInfo> hashset = Enumerable.Where(MapsTab.GetMapInfoForVisibleTier, (MapInfo i) => i.MapsCount > 0).ToHashSet();
					list_0 = new List<Item>();
					foreach (MapInfo mapInfo in Enumerable.Where(hashset, (MapInfo m) => full))
					{
						SelectMapResult switchErr = MapsTab.SelectMap(mapInfo.Name);
						if ((int)switchErr <= 0)
						{
							try
							{
								if (!(await Wait.For(delegate
								{
									int result;
									if ((RemoteMemoryObject)(object)MapsTab.InventoryControl != (RemoteMemoryObject)null)
									{
										InventoryControlWrapper inventoryControl = MapsTab.InventoryControl;
										object obj2;
										if (inventoryControl == null)
										{
											obj2 = null;
										}
										else
										{
											Inventory inventory = inventoryControl.Inventory;
											obj2 = ((inventory != null) ? inventory.Items : null);
										}
										result = ((obj2 != list_0) ? 1 : 0);
									}
									else
									{
										result = 0;
									}
									return (byte)result != 0;
								}, "maps to load", 50, 5000)))
								{
									await Wait.SleepSafe(100);
									goto IL_09a6;
								}
							}
							catch
							{
								await Wait.SleepSafe(100);
								goto IL_09a6;
							}
							List<Item> invItems = MapsTab.InventoryControl.Inventory.Items;
							if (invItems.Count == mapInfo.MapsCount)
							{
								list_0 = invItems;
								foreach (CachedMapItem map2 in invItems.Select((Item i) => new CachedMapItem(i)))
								{
									allMaps.Add(map2);
								}
								continue;
							}
							GlobalLog.Error($"I: {invItems.Count} != {mapInfo.MapsCount}");
							await Wait.SleepSafe(100);
						}
						else
						{
							GlobalLog.Error($"[CacheMapTabs:{mapInfo.Name}] Error switching maps: {switchErr}");
							await Wait.SleepSafe(100);
						}
						goto IL_09a6;
					}
					GlobalLog.Warn($"[CacheMapTabs] Adding info for T{tier} : [{count}]");
					temp.Add(new CachedMaps.MapTierInfo(tier, hashset));
					break;
				}
			}
		}
		return new CachedMaps.MapTabInfo(temp, allMaps);
	}

	public static async Task<bool> OpenInventory()
	{
		if (!InventoryUi.IsOpened || PurchaseUi.IsOpened || SellUi.IsOpened)
		{
			if (!AtlasUi.IsOpened)
			{
				await Coroutines.CloseBlockingWindows();
			}
			Input.SimulateKeyEvent(Binding.open_inventory_panel, true, false, false, Keys.None);
			if (!(await Wait.For(() => InventoryUi.IsOpened, "inventory panel opening")))
			{
				return false;
			}
			await Wait.Sleep(20);
			return true;
		}
		return true;
	}

	public static IEnumerable<Item> GetExcessCurrency(string name)
	{
		List<Item> list = InventoryItems.FindAll((Item c) => c.Name == name);
		IEnumerable<Item> result;
		if (list.Count > 1)
		{
			IEnumerable<Item> enumerable = list.OrderByDescending((Item c) => c.StackCount).Skip(1).ToList();
			result = enumerable;
		}
		else
		{
			result = Enumerable.Empty<Item>();
		}
		return result;
	}

	public static bool StashTabCanFitItem(Vector2i itemPos)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Invalid comparison between Unknown and I4
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Invalid comparison between Unknown and I4
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Invalid comparison between Unknown and I4
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Invalid comparison between Unknown and I4
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		Item val = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
		if ((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null)
		{
			GlobalLog.Error($"[StashCanFitItem] Fail to find item at {itemPos} in player's inventory.");
			return false;
		}
		InventoryTabType tabType = StashUi.StashTabInfo.TabType;
		if ((int)tabType == 3)
		{
			string string_2 = val.Name;
			int int_ = val.StackCount;
			InventoryControlWrapper currencyControl = GetCurrencyControl(string_2);
			if (!((RemoteMemoryObject)(object)currencyControl != (RemoteMemoryObject)null) || !currencyControl.CanFit(string_2, int_))
			{
				return Enumerable.Any(CurrencyTab.NonCurrency, (InventoryControlWrapper miscControl) => (RemoteMemoryObject)(object)miscControl != (RemoteMemoryObject)null && miscControl.CanFit(string_2, int_));
			}
			return true;
		}
		if ((int)tabType != 8)
		{
			if ((int)tabType != 6)
			{
				if ((int)tabType != 9)
				{
					if ((int)tabType == 5)
					{
						if (val.MapTier < 1)
						{
							GlobalLog.Error("[StashTabCanFitItem] " + val.FullName + " is not a map. We can't place it in map tab.");
							return false;
						}
						int num = (val.IsInfluencedMap() ? 18 : (((int)val.Rarity != 3) ? val.MapTier : 17));
						string string_ = InfluenceHelper.GetInfluenceName(val);
						SelectTierResult val2 = MapsTab.SelectTier(num);
						if ((int)val2 <= 0)
						{
							MapInfo val3 = Enumerable.FirstOrDefault(MapsTab.GetMapInfoForVisibleTier, (MapInfo m) => m.Name == string_);
							return val3 == null || val3.MapsCount < 72;
						}
						return false;
					}
					return StashUi.InventoryControl.Inventory.CanFitItem(val.Size);
				}
				InventoryControlWrapper inventoryControlForMetadata = FragmentTab.GetInventoryControlForMetadata(val.Metadata);
				return (RemoteMemoryObject)(object)inventoryControlForMetadata != (RemoteMemoryObject)null && inventoryControlForMetadata.CanFit(val.Name, val.StackCount);
			}
			InventoryControlWrapper inventoryControlForMetadata2 = DivinationTab.GetInventoryControlForMetadata(val.Metadata);
			return (RemoteMemoryObject)(object)inventoryControlForMetadata2 != (RemoteMemoryObject)null && inventoryControlForMetadata2.CanFit(val.Name, val.StackCount);
		}
		string string_0 = val.Name;
		int int_0 = val.StackCount;
		InventoryControlWrapper inventoryControlForMetadata3 = EssenceTab.GetInventoryControlForMetadata(val.Metadata);
		if ((RemoteMemoryObject)(object)inventoryControlForMetadata3 != (RemoteMemoryObject)null && inventoryControlForMetadata3.CanFit(string_0, int_0))
		{
			return true;
		}
		return Enumerable.Any(EssenceTab.NonEssences, (InventoryControlWrapper miscControl) => (RemoteMemoryObject)(object)miscControl != (RemoteMemoryObject)null && miscControl.CanFit(string_0, int_0));
	}

	public static int GetCurrencyAmountInStashTab(string currencyName)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		int num = 0;
		InventoryTabType tabType = StashUi.StashTabInfo.TabType;
		if ((int)tabType != 3)
		{
			if ((int)tabType != 8)
			{
				if ((int)tabType != 6 && (int)tabType != 5 && (int)tabType != 9)
				{
					return num + Enumerable.Where(StashTabItems, (Item item) => item.Name == currencyName).Sum((Item item) => item.StackCount);
				}
				return 0;
			}
			return num + Enumerable.Where(EssenceTab.NonEssences.Select((InventoryControlWrapper miscControl) => miscControl.CustomTabItem), (Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && item.Name == currencyName).Sum((Item item) => item.StackCount);
		}
		InventoryControlWrapper currencyControl = GetCurrencyControl(currencyName);
		if ((RemoteMemoryObject)(object)currencyControl != (RemoteMemoryObject)null)
		{
			Item customTabItem = currencyControl.CustomTabItem;
			if ((RemoteMemoryObject)(object)customTabItem != (RemoteMemoryObject)null)
			{
				num += customTabItem.StackCount;
			}
		}
		return num + Enumerable.Where(CurrencyTab.NonCurrency.Select((InventoryControlWrapper miscControl) => miscControl.CustomTabItem), (Item item) => (RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && item.Name == currencyName).Sum((Item item) => item.StackCount);
	}

	public static async Task<WithdrawResult> WithdrawCurrency(string name)
	{
		foreach (string tab in ExtensionsSettings.Instance.GetTabsForCurrency(name))
		{
			if (await OpenStashTab(tab, "WithdrawCurrency"))
			{
				InventoryTabType tabType = StashUi.StashTabInfo.TabType;
				if ((int)tabType == 3)
				{
					InventoryControlWrapper control2 = GetControlWithCurrency(name);
					if (!((RemoteMemoryObject)(object)control2 == (RemoteMemoryObject)null))
					{
						if (await FastMoveCustomTabItem(control2))
						{
							GlobalLog.Debug("[WithdrawCurrency] \"" + name + "\" have been successfully taken from \"" + tab + "\" tab.");
							return WithdrawResult.Success;
						}
						return WithdrawResult.Error;
					}
					continue;
				}
				if ((int)tabType != 8)
				{
					if ((int)tabType == 9)
					{
						InventoryControlWrapper control3 = Enumerable.FirstOrDefault(FragmentTab.All, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name == name);
						if (!((RemoteMemoryObject)(object)control3 == (RemoteMemoryObject)null))
						{
							if (!(await FastMoveCustomTabItem(control3)))
							{
								return WithdrawResult.Error;
							}
							GlobalLog.Debug("[WithdrawCurrency] \"" + name + "\" have been successfully taken from \"" + tab + "\" tab.");
							return WithdrawResult.Success;
						}
					}
					else if ((int)tabType != 6 && (int)tabType != 5)
					{
						Item item = (from i in Enumerable.Where(StashTabItems, (Item i) => i.Name == name)
							orderby i.StackCount descending
							select i).FirstOrDefault();
						if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
						{
							if (!(await FastMoveFromStashTab(item.LocationTopLeft)))
							{
								return WithdrawResult.Error;
							}
							GlobalLog.Debug("[WithdrawCurrency] \"" + name + "\" have been successfully taken from \"" + tab + "\" tab.");
							return WithdrawResult.Success;
						}
					}
					else
					{
						GlobalLog.Error($"[WithdrawCurrency] Unsupported behavior. Current stash tab is {tabType}.");
					}
					continue;
				}
				InventoryControlWrapper control = Enumerable.FirstOrDefault(EssenceTab.All, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name == name);
				if ((RemoteMemoryObject)(object)control == (RemoteMemoryObject)null)
				{
					GlobalLog.Debug("[WithdrawCurrency] There are no \"" + name + "\" in \"" + tab + "\" tab.");
					continue;
				}
				if (await FastMoveCustomTabItem(control))
				{
					GlobalLog.Debug("[WithdrawCurrency] \"" + name + "\" have been successfully taken from \"" + tab + "\" tab.");
					return WithdrawResult.Success;
				}
				return WithdrawResult.Error;
			}
			return WithdrawResult.Error;
		}
		return WithdrawResult.Unavailable;
	}

	public static async Task<bool> FindTabWithCurrency(string name)
	{
		List<string> tabs = new List<string>(ExtensionsSettings.Instance.GetTabsForCurrency(name));
		if (tabs.Count > 1 && StashUi.IsOpened)
		{
			string string_0 = StashUi.StashTabInfo.DisplayName;
			tabs = tabs.OrderByDescending((string t) => t.Contains(string_0)).ToList();
		}
		foreach (string tab in tabs)
		{
			if (!(await OpenStashTab(tab, "FindTabWithCurrency")))
			{
				continue;
			}
			if (!StashUi.StashTabInfo.IsPublicFlagged || StashUi.StashTabInfo.IsPremiumCurrency)
			{
				int amount = GetCurrencyAmountInStashTab(name);
				if (amount > 0)
				{
					GlobalLog.Debug($"[FindTabWithCurrency] Found {amount} \"{name}\" in \"{tab}\" tab.");
					return true;
				}
				GlobalLog.Debug("[FindTabWithCurrency] There are no \"" + name + "\" in \"" + tab + "\" tab.");
			}
			else
			{
				GlobalLog.Error("[FindTabWithCurrency] Stash tab \"" + tab + "\" is public. Cannot use currency from it.");
			}
		}
		return false;
	}

	public static InventoryControlWrapper GetControlWithCurrency(string currencyName)
	{
		return GetControlsWithCurrency(currencyName).FirstOrDefault();
	}

	public static List<InventoryControlWrapper> GetControlsWithCurrency(string currencyName)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		List<InventoryControlWrapper> list = new List<InventoryControlWrapper>();
		InventoryTabType tabType = StashUi.StashTabInfo.TabType;
		InventoryTabType val = tabType;
		if ((int)val != 3)
		{
			if ((int)val != 8)
			{
				if ((int)val != 9)
				{
					return list;
				}
				List<InventoryControlWrapper> list2 = new List<InventoryControlWrapper>();
				if (currencyName.ContainsIgnorecase("invitation"))
				{
					list2.Add(currencyName.ContainsIgnorecase("maven") ? EldrichMaven.MavenInventory : EldrichMaven.EldrichInventory);
				}
				else if (currencyName.ContainsIgnorecase("scarab"))
				{
					list2.AddRange(Enumerable.Where(FragmentTab.AllScarab, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name.Equals(currencyName)));
				}
				else
				{
					list2.Add(Enumerable.FirstOrDefault(FragmentTab.All, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name.Equals(currencyName)));
				}
				return list2;
			}
			return Enumerable.Where(EssenceTab.All, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name.Equals(currencyName)).ToList();
		}
		InventoryControlWrapper currencyControl = GetCurrencyControl(currencyName);
		if ((RemoteMemoryObject)(object)((currencyControl != null) ? currencyControl.CustomTabItem : null) != (RemoteMemoryObject)null)
		{
			list.Add(currencyControl);
		}
		List<InventoryControlWrapper> list3 = Enumerable.Where(CurrencyTab.NonCurrency, (InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && c.CustomTabItem.Name.Equals(currencyName)).ToList();
		if (list3.Any())
		{
			return list3;
		}
		return list.OrderBy((InventoryControlWrapper i) => i.CustomTabItem.StackCount).ToList();
	}

	public static async Task<Item> TakeMapFromStash(CachedMapItem map)
	{
		if (await OpenStashTab(map.StashTab, "TakeMapFromStash"))
		{
			GlobalLog.Debug($"[TakeMapFromStash] Now trying to take T{map.MapTier} {map.Name} tab: {map.StashTab} from stash.");
			IEnumerable<Vector2i> ienumerable_0 = from i in InventoryItems.Where(MapExtensions.IsMap)
				select i.LocationTopLeft;
			try
			{
				if (StashUi.StashTabInfo.IsPremiumMap)
				{
					int tier = map.MapTier;
					string name = map.Name;
					if ((int)map.Rarity == 3)
					{
						tier = 17;
						name = map.FullName;
					}
					if (map.IsInfluencedMap())
					{
						tier = 18;
						name = InfluenceHelper.GetInfluenceName(map);
					}
					SelectTierResult err = MapsTab.SelectTier(tier);
					if ((int)err > 0)
					{
						GlobalLog.Error($"[TakeMapFromStash] SelectTier:{err}");
						return null;
					}
					await Wait.LatencySleep();
					SelectMapResult err3 = MapsTab.SelectMap(name);
					if ((int)err3 == 1)
					{
						err3 = MapsTab.SelectMap(name + " " + map.Name);
						if ((int)err3 > 0)
						{
							GlobalLog.Error($"[TakeMapFromStash] SelectMap [{name}]:{err3}");
							CachedMaps.Instance.MapCache.Maps.Remove(map);
							return null;
						}
					}
					await Wait.LatencySleep();
					if (!(await Wait.For(() => (RemoteMemoryObject)(object)MapsTab.InventoryControl != (RemoteMemoryObject)null && Enumerable.Any(MapsTab.InventoryControl.Inventory.Items, (Item m) => m.FullName.Equals(map.FullName)), "map inventory to load")))
					{
						GlobalLog.Error($"[TakeMapFromStash] Can't find T{map.MapTier} {map.Name} tab: {map.StashTab}.");
						CachedMaps.Instance.MapCache.Maps.Remove(map);
						return null;
					}
					Item item2 = Enumerable.FirstOrDefault(MapsTab.InventoryControl.Inventory.Items, (Item i) => StatStringEqual(i, map) && i.FullName.Equals(map.FullName));
					if ((RemoteMemoryObject)(object)item2 == (RemoteMemoryObject)null || (int)MapsTab.InventoryControl.FastMove(item2.LocalId, true, true) > 0)
					{
						GlobalLog.Error($"[TakeMapFromStash] Can't find T{map.MapTier} {map.Name} tab: {map.StashTab}.");
						CachedMaps.Instance.MapCache.Maps.Remove(map);
						return null;
					}
				}
				else
				{
					Item item = Enumerable.FirstOrDefault(StashUi.InventoryControl.Inventory.Items, (Item i) => StatStringEqual(i, map) && i.FullName.Equals(map.FullName));
					bool flag = (RemoteMemoryObject)(object)item == (RemoteMemoryObject)null;
					bool flag2 = flag;
					if (!flag2)
					{
						flag2 = !(await FastMoveFromStashTab(item.LocationTopLeft));
					}
					if (flag2)
					{
						CachedMaps.Instance.MapCache.Maps.Remove(map);
						return null;
					}
				}
			}
			catch
			{
				GlobalLog.Error("[TakeMapFromStash] Something is wrong. Map cache corrupted. Recaching");
				CachedMaps.Instance.MapCache = null;
				CachedMaps.Cached = false;
				return null;
			}
			await Wait.For(() => ienumerable_0.Count() < InventoryItems.Count(MapExtensions.IsMap), "map appear in inventory");
			Item invMap = Enumerable.FirstOrDefault(InventoryItems, (Item i) => MapExtensions.IsMap(i) && !ienumerable_0.Contains(i.LocationTopLeft));
			if ((RemoteMemoryObject)(object)invMap != (RemoteMemoryObject)null)
			{
				CachedMaps.Instance.MapCache.Maps.Remove(map);
			}
			return invMap;
		}
		return null;
	}

	public static async Task<bool> FastMoveFromInventory(Vector2i itemPos, bool waitForStackDecrease = false, bool ignoreAffinity = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			int int_0 = item.StackCount;
			string itemName = item.FullName;
			string logStr = $"[FastMoveFromInventory] Fast moving \"{itemName}\" at {itemPos} from player's inventory. Ignoring Affinity? {ignoreAffinity}";
			if (int_0 > 1)
			{
				logStr = $"[FastMoveFromInventory] Fast moving \"{itemName}\"({int_0}) at {itemPos} from player's inventory. Ignoring Affinity? {ignoreAffinity}";
			}
			GlobalLog.Debug(logStr);
			if (!ignoreAffinity)
			{
				FastMoveResult err2 = InventoryUi.InventoryControl_Main.FastMove(item.LocalId, true, true);
				if ((int)err2 > 0)
				{
					GlobalLog.Error($"[FastMoveFromInventory] Fast move error: \"{err2}\".");
					return false;
				}
			}
			else
			{
				ProcessHookManager.ClearAllKeyStates();
				ProcessHookManager.SetKeyState(Keys.ShiftKey, short.MinValue, Keys.None);
				FastMoveResult err = InventoryUi.InventoryControl_Main.FastMove(item.LocalId, true, true, true);
				ProcessHookManager.SetKeyState(Keys.ShiftKey, (short)0, Keys.None);
				if ((int)err > 0)
				{
					GlobalLog.Error($"[FastMoveFromInventory] Fast move error: \"{err}\".");
					return false;
				}
			}
			if (waitForStackDecrease)
			{
				return await Wait.For(() => (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos) == (RemoteMemoryObject)null || InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos).StackCount != int_0, "fast move from inventory", 30);
			}
			return await Wait.For(() => (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos) == (RemoteMemoryObject)null, "fast move from inventory", 30);
		}
		GlobalLog.Error($"[FastMoveFromInventory] Fail to find item at {itemPos} in player's inventory.");
		return false;
	}

	public static async Task<bool> FastMoveToVendor(Vector2i itemPos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			string itemName = item.FullName;
			GlobalLog.Debug($"[FastMoveToVendor] Fast moving \"{itemName}\" at {itemPos} from player's inventory.");
			FastMoveResult err = InventoryUi.InventoryControl_Main.FastMove(item.LocalId, true, true);
			if ((int)err != 0 && (int)err != 4)
			{
				GlobalLog.Error($"[FastMoveToVendor] Fast move error: \"{err}\".");
				return false;
			}
			if (!(await Wait.For(delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Item val = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(itemPos);
				if ((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null)
				{
					GlobalLog.Error("[FastMoveToVendor] Unexpected error. Item became null instead of transparent.");
					return false;
				}
				return InventoryUi.InventoryControl_Main.IsItemTransparent(val.LocalId);
			}, "fast move to vendor", 30)))
			{
				GlobalLog.Error($"[FastMoveToVendor] Fast move timeout for \"{itemName}\" at {itemPos} in player's inventory.");
				return false;
			}
			return true;
		}
		GlobalLog.Error($"[FastMoveToVendor] Fail to find item at {itemPos} in player's inventory.");
		return false;
	}

	public static async Task<bool> FastMoveFromStashTab(Vector2i itemPos)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		string tabName = StashUi.TabControl.CurrentTabName;
		Item item = StashUi.InventoryControl.Inventory.FindItemByPos(itemPos);
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			string itemName = item.FullName;
			int int_0 = item.StackCount;
			CachedItem cachedItem = new CachedItem(item);
			GlobalLog.Debug($"[FastMoveFromStashTab] Fast moving \"{itemName}\" at {itemPos} from \"{tabName}\" tab.");
			FastMoveResult err = StashUi.InventoryControl.FastMove(item.LocalId, true, true);
			if ((int)err > 0)
			{
				GlobalLog.Error($"[FastMoveFromStashTab] Fast move error: \"{err}\".");
				return false;
			}
			if (!(await Wait.For(delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Item val = StashUi.InventoryControl.Inventory.FindItemByPos(itemPos);
				return (RemoteMemoryObject)(object)val == (RemoteMemoryObject)null || val.StackCount < int_0;
			}, "fast move from stash tab", 30)))
			{
				GlobalLog.Error($"[FastMoveFromStashTab] Fast move timeout for \"{itemName}\" at {itemPos} in \"{tabName}\" tab.");
				return false;
			}
			GlobalLog.Info("[Events] Item withdrawn (" + itemName + ")");
			Utility.BroadcastMessage((object)null, "item_withdrawn_event", new object[1] { cachedItem });
			return true;
		}
		GlobalLog.Error($"[FastMoveFromStashTab] Fail to find item at {itemPos} in \"{tabName}\" tab.");
		return false;
	}

	public static async Task<bool> SplitAndPlaceItemInMainInventory(InventoryControlWrapper wrapper, Item item, int pickupAmount)
	{
		if (!((RemoteMemoryObject)(object)wrapper == (RemoteMemoryObject)null))
		{
			GlobalLog.Debug($"[SplitAndPlaceItemInMainInventory] Spliting up stacks. Getting {pickupAmount} {item.FullName}. Count in stack: {item.StackCount}");
			SplitStackResult error;
			if (!wrapper.HasCustomTabOverride)
			{
				if (pickupAmount >= item.StackCount || pickupAmount >= item.MaxStackCount)
				{
					return await FastMoveFromStashTab(item.LocationTopLeft);
				}
				error = wrapper.SplitStack(item.LocalId, pickupAmount, true);
			}
			else
			{
				if (pickupAmount >= item.StackCount || pickupAmount >= item.MaxStackCount)
				{
					return await FastMoveCustomTabItem(wrapper);
				}
				error = wrapper.SplitStack(pickupAmount, true);
			}
			if ((int)error > 0)
			{
				GlobalLog.Error($"[SplitAndPlaceItemInMainInventory] Failed to split failed. Split Error: {error}");
				return false;
			}
			await WaitForCursorToHaveItem();
			await Coroutines.LatencyWait();
			await ClearCursorLite();
			return true;
		}
		throw new ArgumentNullException("wrapper");
	}

	public static async Task<bool> SplitAndPlaceItemInMainInventory(string itemName, int pickupAmount)
	{
		bool isPremiumTab = StashUi.StashTabInfo.IsPremiumSpecial;
		Item item = null;
		int stackCount = 0;
		InventoryControlWrapper wrapper;
		if (isPremiumTab)
		{
			wrapper = GetControlWithCurrency(itemName);
			if (!itemName.ContainsIgnorecase("invitation"))
			{
				if ((RemoteMemoryObject)(object)wrapper != (RemoteMemoryObject)null)
				{
					item = wrapper.CustomTabItem;
					stackCount = item.StackCount;
				}
			}
			else if ((RemoteMemoryObject)(object)wrapper != (RemoteMemoryObject)null)
			{
				item = Enumerable.FirstOrDefault(wrapper.Inventory.Items, (Item i) => i.Name.Equals(itemName));
				stackCount = 1;
			}
		}
		else
		{
			wrapper = StashUi.InventoryControl;
			item = Enumerable.FirstOrDefault(StashUi.InventoryControl.Inventory.Items.OrderByDescending((Item i) => i.StackCount), (Item i) => i.Name == itemName);
			if ((RemoteMemoryObject)(object)item != (RemoteMemoryObject)null)
			{
				stackCount = item.StackCount;
			}
		}
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			if (pickupAmount < item.StackCount && pickupAmount < item.MaxStackCount)
			{
				GlobalLog.Debug($"[SplitAndPlaceItemInMainInventory] Spliting up stacks. Getting {pickupAmount} {item.FullName}. Count in stack: {stackCount}");
			}
			GlobalLog.Debug($"[SplitAndPlaceItemInMainInventory] Getting {pickupAmount} {item.FullName}. Count in stack: {stackCount}");
			SplitStackResult error;
			if (isPremiumTab && !itemName.ContainsIgnorecase("invitation"))
			{
				if (pickupAmount >= item.StackCount || pickupAmount >= item.MaxStackCount)
				{
					return await FastMoveCustomTabItem(wrapper);
				}
				error = wrapper.SplitStack(pickupAmount, true);
			}
			else
			{
				if (itemName.ContainsIgnorecase("invitation"))
				{
					FastMoveResult err = wrapper.FastMove(item.LocalId, true, true);
					if ((int)err > 0)
					{
						GlobalLog.Error($"[SplitAndPlaceItemInMainInventory] Error while fastmoving {item.FullName}: {err}");
						return false;
					}
					return true;
				}
				if (pickupAmount >= item.StackCount || pickupAmount >= item.MaxStackCount)
				{
					return await FastMoveFromStashTab(item.LocationTopLeft);
				}
				error = wrapper.SplitStack(item.LocalId, pickupAmount, true);
			}
			if ((int)error > 0)
			{
				GlobalLog.Error($"[SplitAndPlaceItemInMainInventory] Failed to split failed. Split Error: {error}");
				return false;
			}
			await WaitForCursorToHaveItem();
			await Coroutines.LatencyWait();
			await ClearCursorLite();
			return true;
		}
		return false;
	}

	public static async Task<bool> FastMoveCustomTabItem(InventoryControlWrapper control)
	{
		if ((RemoteMemoryObject)(object)control == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[FastMoveCustomTabItem] Inventory control is null.");
			return false;
		}
		Item item = control.CustomTabItem;
		if ((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[FastMoveCustomTabItem] Inventory control has no item.");
			return false;
		}
		string itemName = item.Name;
		int int_0 = item.StackCount;
		string tabName = StashUi.TabControl.CurrentTabName;
		GlobalLog.Debug("[FastMoveCustomTabItem] Fast moving \"" + itemName + "\" from \"" + tabName + "\" tab.");
		FastMoveResult moved = control.FastMove(true, true, false);
		if ((int)moved <= 0)
		{
			if (await Wait.For(delegate
			{
				Item customTabItem = control.CustomTabItem;
				return (RemoteMemoryObject)(object)customTabItem == (RemoteMemoryObject)null || customTabItem.StackCount < int_0;
			}, "fast move from premium stash tab", 30))
			{
				return true;
			}
			GlobalLog.Error("[FastMoveCustomTabItem] Fast move timeout for \"" + itemName + "\" in \"" + tabName + "\" tab.");
			return false;
		}
		GlobalLog.Error($"[FastMoveCustomTabItem] Fast move error: \"{moved}\".");
		return false;
	}

	public static async Task<bool> ApplyOrb(Vector2i targetPos, string orbName)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Item found2 = Enumerable.FirstOrDefault(InventoryUi.InventoryControl_Main.Inventory.Items, (Item i) => i.Name.Equals(orbName));
		if (orbName.Equals(CurrencyNames.Alchemy) || orbName.Equals(CurrencyNames.Binding))
		{
			found2 = (from i in Enumerable.Where(InventoryUi.InventoryControl_Main.Inventory.Items, (Item i) => i.Name.Equals(CurrencyNames.Alchemy) || i.Name.Equals(CurrencyNames.Binding))
				orderby i.StackCount descending
				select i).FirstOrDefault();
			if ((RemoteMemoryObject)(object)found2 != (RemoteMemoryObject)null)
			{
				orbName = found2.Name;
			}
		}
		if ((RemoteMemoryObject)(object)found2 == (RemoteMemoryObject)null)
		{
			if (!(await FindTabWithCurrency(orbName)))
			{
				GlobalLog.Warn("[TakeMapTask] There are no \"" + orbName + "\" in all tabs assigned to them. Now marking this currency as unavailable.");
				TakeMapTask.AvailableCurrency[orbName] = false;
				return false;
			}
			if ((int)StashUi.StashTabInfo.TabType == 3)
			{
				InventoryControlWrapper control = GetControlWithCurrency(orbName);
				if (orbName.Equals(CurrencyNames.Alchemy))
				{
					int bindControlSum = GetControlsWithCurrency(CurrencyNames.Binding).Sum((InventoryControlWrapper x) => x.Inventory.Items.Sum((Item y) => y.StackCount));
					if (bindControlSum > 20)
					{
						orbName = CurrencyNames.Binding;
					}
					control = GetControlWithCurrency(orbName);
				}
				if (!StashUi.StashTabInfo.IsPublicFlagged)
				{
					if (!(await control.PickItemToCursor(rightClick: true)))
					{
						ErrorManager.ReportError();
						return false;
					}
				}
				else
				{
					if (!(await FastMoveCustomTabItem(control)))
					{
						ErrorManager.ReportError();
						return false;
					}
					found2 = Enumerable.FirstOrDefault(InventoryUi.InventoryControl_Main.Inventory.Items, (Item i) => i.Name.Equals(orbName));
					if (!((RemoteMemoryObject)(object)found2 != (RemoteMemoryObject)null))
					{
						GlobalLog.Error("Found null!!!");
					}
					else if (!(await InventoryUi.InventoryControl_Main.PickItemToCursor(found2.LocationTopLeft, rightClick: true)))
					{
						ErrorManager.ReportError();
						return false;
					}
				}
			}
			else if (!(await PickItemToCursor(itemPos: StashTabItems.Find((Item i) => i.Name == orbName).LocationTopLeft, inventory: StashUi.InventoryControl, rightClick: true)))
			{
				ErrorManager.ReportError();
				return false;
			}
		}
		else if (!(await InventoryUi.InventoryControl_Main.PickItemToCursor(found2.LocationTopLeft, rightClick: true)))
		{
			ErrorManager.ReportError();
			return false;
		}
		if (!(await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(targetPos)))
		{
			ErrorManager.ReportError();
			return false;
		}
		await WaitForCursorToBeEmpty();
		return true;
	}

	public static int ItemAmount(this Inventory inventory, string itemName)
	{
		int num = 0;
		foreach (Item item in inventory.Items)
		{
			if (item.Name == itemName)
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	public static async Task<bool> PickItemToCursor(this InventoryControlWrapper inventory, Vector2i itemPos, bool rightClick = false)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Item item = inventory.Inventory.FindItemByPos(itemPos);
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			int id = item.LocalId;
			if (rightClick)
			{
				UseItemResult err2 = inventory.UseItem(id, true);
				if ((int)err2 > 0)
				{
					GlobalLog.Error($"[PickItemToCursor] Fail to pick item to cursor. Error: \"{err2}\".");
					return false;
				}
			}
			else
			{
				PickupResult err = inventory.Pickup(id, true);
				if ((int)err > 0)
				{
					GlobalLog.Error($"[PickItemToCursor] Fail to pick item to cursor. Error: \"{err}\".");
					return false;
				}
			}
			return await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null, "item appear under cursor");
		}
		GlobalLog.Error($"[PickItemToCursor] Cannot find item at {itemPos}");
		return false;
	}

	public static async Task<bool> PickItemToCursor(this InventoryControlWrapper inventory, bool rightClick = false)
	{
		Item item = inventory.CustomTabItem;
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			if (!rightClick)
			{
				PickupResult err2 = inventory.Pickup(true);
				if ((int)err2 > 0)
				{
					GlobalLog.Error($"[PickItemToCursor] Fail to pick item to cursor. Error: \"{err2}\".");
					return false;
				}
			}
			else
			{
				UseItemResult err = inventory.UseItem(true);
				if ((int)err > 0)
				{
					GlobalLog.Error($"[PickItemToCursor] Fail to pick item to cursor. Error: \"{err}\".");
					return false;
				}
			}
			return await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null, "item appear under cursor");
		}
		GlobalLog.Error("[PickItemToCursor] Custom inventory control is empty.");
		return false;
	}

	public static async Task<bool> PlaceItemFromCursor(this InventoryControlWrapper inventory, Vector2i pos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Item cursorItem = CursorItemOverlay.Item;
		if (!((RemoteMemoryObject)(object)cursorItem == (RemoteMemoryObject)null))
		{
			if ((int)CursorItemOverlay.Mode == 2)
			{
				Item destItem = inventory.Inventory.FindItemByPos(pos);
				if (!((RemoteMemoryObject)(object)destItem == (RemoteMemoryObject)null))
				{
					int int_0 = destItem.LocalId;
					ApplyCursorResult applied = inventory.ApplyCursorTo(destItem.LocalId, true);
					if ((int)applied > 0)
					{
						GlobalLog.Error($"[PlaceItemFromCursor] Fail to place item from cursor. Error: \"{applied}\".");
						return false;
					}
					return await Wait.For(delegate
					{
						//IL_0016: Unknown result type (might be due to invalid IL or missing references)
						Item val = inventory.Inventory.FindItemByPos(pos);
						return (RemoteMemoryObject)(object)val != (RemoteMemoryObject)null && val.LocalId != int_0;
					}, "destination item change", 5);
				}
				GlobalLog.Error("[PlaceItemFromCursor] Destination item is null.");
				return false;
			}
			int YheZbrPlhw = cursorItem.LocalId;
			PlaceCursorIntoResult placed = inventory.PlaceCursorInto(pos.X, pos.Y, true, true);
			if ((int)placed > 0)
			{
				GlobalLog.Error($"[PlaceItemFromCursor] Fail to place item from cursor. Error: \"{placed}\".");
				return false;
			}
			if (await Wait.For(delegate
			{
				Item item = CursorItemOverlay.Item;
				return (RemoteMemoryObject)(object)item == (RemoteMemoryObject)null || item.LocalId != YheZbrPlhw;
			}, "cursor item change"))
			{
				return true;
			}
			return false;
		}
		GlobalLog.Error("[PlaceItemFromCursor] Cursor item is null.");
		return false;
	}

	public static async Task<bool> WaitForCursorToHaveItem(int timeout = 5000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		do
		{
			if (!InstanceInfo.GetPlayerInventoryItemsBySlot((InventorySlot)12).Any())
			{
				GlobalLog.Debug("[WaitForCursorToHaveItem] Waiting for the cursor to have an item.");
				await Coroutines.LatencyWait();
				continue;
			}
			return true;
		}
		while (sw.ElapsedMilliseconds <= timeout);
		GlobalLog.Error("[WaitForCursorToHaveItem] Timeout while waiting for the cursor to contain an item.");
		return false;
	}

	public static async Task<ClearCursorResults> ClearCursorLite(int maxTries = 3)
	{
		CursorItemModes cursMode = CursorItemOverlay.Mode;
		if ((int)cursMode == 0)
		{
			GlobalLog.Debug("[ClearCursorLite] Nothing is on cursor, continue execution");
			return ClearCursorResults.None;
		}
		if ((int)cursMode == 3 || (int)cursMode == 2)
		{
			GlobalLog.Debug("[ClearCursorLite] VirtualMode detected, pressing escape to clear");
			Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
			return ClearCursorResults.None;
		}
		Item cursorhasitem = CursorItemOverlay.Item;
		int attempts = 0;
		int col = default(int);
		int row = default(int);
		while ((RemoteMemoryObject)(object)cursorhasitem != (RemoteMemoryObject)null && attempts < maxTries)
		{
			if (attempts <= maxTries)
			{
				if (!InventoryUi.IsOpened)
				{
					await OpenInventory();
					await Coroutines.LatencyWait();
					if (!InventoryUi.IsOpened)
					{
						return ClearCursorResults.InventoryNotOpened;
					}
				}
				if (InventoryUi.InventoryControl_Main.Inventory.CanFitItem(cursorhasitem.Size, ref col, ref row))
				{
					PlaceCursorIntoResult res = InventoryUi.InventoryControl_Main.PlaceCursorInto(col, row, false, true);
					if ((int)res != 0)
					{
						GlobalLog.Debug($"[ClearCursorLite] Placing item into inventory failed, Err : {res}");
						PlaceCursorIntoResult val = res;
						PlaceCursorIntoResult val2 = val;
						if ((int)val2 != 2)
						{
							if ((int)val2 == 4)
							{
								return ClearCursorResults.NoSpaceInInventory;
							}
							await Wait.SleepSafe(3000);
							await Coroutines.LatencyWait();
							await Coroutines.ReactionWait();
							cursorhasitem = CursorItemOverlay.Item;
							attempts++;
							continue;
						}
						return ClearCursorResults.None;
					}
					if (!(await WaitForCursorToBeEmpty()))
					{
						GlobalLog.Error("[ClearCursorLite] WaitForCursorToBeEmpty failed.");
					}
					await Coroutines.ReactionWait();
					return ClearCursorResults.None;
				}
				GlobalLog.Error("[ClearCursorLite] Now stopping the bot because it cannot continue.");
				BotManager.Stop(new StopReasonData("invenory_cant_fit", "Inventory can't fit item", (object)null), false);
				return ClearCursorResults.NoSpaceInInventory;
			}
			return ClearCursorResults.TriesReached;
		}
		return ClearCursorResults.None;
	}

	public static async Task<bool> WaitForCursorToBeEmpty(int timeout = 5000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		while (InstanceInfo.GetPlayerInventoryItemsBySlot((InventorySlot)12).Any())
		{
			GlobalLog.Info("[WaitForCursorToBeEmpty] Waiting for the cursor to be empty.");
			await Coroutines.LatencyWait();
			if (sw.ElapsedMilliseconds > timeout)
			{
				GlobalLog.Info("[WaitForCursorToBeEmpty] Timeout while waiting for the cursor to become empty.");
				return false;
			}
		}
		return true;
	}

	public static async Task<Vector2i> TakeItemFromStashUsingLeftClick(Item item, int amount = 1)
	{
		bool flag1 = false;
		Vector2i posToMove = default(Vector2i);
		await ClearCursorLite();
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				if (InventoryUi.InventoryControl_Main.Inventory.CanFitItemSizeAt(item.Size.X, item.Size.Y, i, j))
				{
					posToMove = new Vector2i(i, j);
					flag1 = true;
					break;
				}
			}
			if (flag1)
			{
				break;
			}
		}
		if (item.StackCount > amount)
		{
			StashUi.InventoryControl.SplitStack(item.LocalId, 1, true);
			await WaitForCursorToHaveItem();
			await Coroutines.ReactionWait();
		}
		else
		{
			GlobalLog.Info($"[TakeItemFromStash] Now picking item at {item.LocationTopLeft.X}, {item.LocationTopLeft.Y} in stash");
			await StashUi.InventoryControl.PickItemToCursor(new Vector2i(item.LocationTopLeft.X, item.LocationTopLeft.Y));
			await WaitForCursorToHaveItem();
			await Coroutines.ReactionWait();
		}
		GlobalLog.Info($"[TakeItemFromStash] Now placing item at {posToMove.X}, {posToMove.Y} in inventory");
		await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(posToMove.X, posToMove.Y));
		await WaitForCursorToBeEmpty();
		await Coroutines.ReactionWait();
		return posToMove;
	}

	private static bool StatStringEqual(Item item, CachedMapItem map)
	{
		List<string> values = (from s in item.Stats
			orderby s.Key
			select s into a
			select $"{a.Key}[{a.Value}]").ToList();
		string text = string.Join("|", values).Trim();
		List<string> values2 = (from s in map.Stats
			orderby s.Key
			select s into a
			select $"{a.Key}[{a.Value}]").ToList();
		string value = string.Join("|", values2).Trim();
		return text.Equals(value);
	}

	private static bool CanFit(this InventoryControlWrapper control, string itemName, int amount)
	{
		Item customTabItem = control.CustomTabItem;
		if ((RemoteMemoryObject)(object)customTabItem == (RemoteMemoryObject)null)
		{
			return true;
		}
		return customTabItem.Name == itemName && customTabItem.StackCount + amount <= 5000;
	}

	private static InventoryControlWrapper GetCurrencyControl(string currencyName)
	{
		if (dictionary_0.TryGetValue(currencyName, out var value))
		{
			return value();
		}
		if (!dictionary_1.TryGetValue(currencyName, out var value2))
		{
			return null;
		}
		return value2();
	}

	static Inventories()
	{
		dictionary_0 = new Dictionary<string, Func<InventoryControlWrapper>>
		{
			[CurrencyNames.IchorLesser] = () => CurrencyTab.LesserEldritchIchor,
			[CurrencyNames.IchorGreater] = () => CurrencyTab.GreaterEldritchIchor,
			[CurrencyNames.TaintedChaos] = () => CurrencyTab.TaintedChaosOrb,
			[CurrencyNames.TaintedExalted] = () => CurrencyTab.TaintedExaltedOrb,
			[CurrencyNames.TaintedWhetstone] = () => CurrencyTab.TaintedBlacksmithsWhetstone,
			[CurrencyNames.HuntersExalted] = () => CurrencyTab.HuntersExaltedOrb,
			[CurrencyNames.CrusadersExalted] = () => CurrencyTab.CrusadersExaltedOrb,
			[CurrencyNames.IchorGrand] = () => CurrencyTab.GrandEldritchIchor,
			[CurrencyNames.IchorExceptional] = () => CurrencyTab.ExceptionalEldritchIchor,
			[CurrencyNames.TaintedScrap] = () => CurrencyTab.TaintedArmourersScrap,
			[CurrencyNames.TaintedDivine] = () => CurrencyTab.TaintedDivineTeardrop,
			[CurrencyNames.TaintedMythic] = () => CurrencyTab.TaintedMythicOrb,
			[CurrencyNames.RedeemersExalted] = () => CurrencyTab.RedeemersExaltedOrb,
			[CurrencyNames.WarlordsExalted] = () => CurrencyTab.WarlordsExaltedOrb,
			[CurrencyNames.EmberLesser] = () => CurrencyTab.LesserEldritchEmber,
			[CurrencyNames.EmberGreater] = () => CurrencyTab.GreaterEldritchEmber,
			[CurrencyNames.TaintedFusing] = () => CurrencyTab.TaintedOrbOfFusing,
			[CurrencyNames.TaintedJeweller] = () => CurrencyTab.TaintedJewellersOrb,
			[CurrencyNames.TaintedChromatic] = () => CurrencyTab.TaintedChromaticOrb,
			[CurrencyNames.Dominance] = () => CurrencyTab.OrbofDominance,
			[CurrencyNames.Awakeners] = () => CurrencyTab.AwakenersOrb,
			[CurrencyNames.EmberGrand] = () => CurrencyTab.GrandEldritchEmber,
			[CurrencyNames.EmberExceptional] = () => CurrencyTab.ExceptionalEldritchEmber,
			[CurrencyNames.EldrichChaos] = () => CurrencyTab.EldritchChaosOrb,
			[CurrencyNames.EldrichAnnulment] = () => CurrencyTab.EldritchOrbofAnnulment,
			[CurrencyNames.Conflict] = () => CurrencyTab.OrbofConflict,
			[CurrencyNames.EldrichExalted] = () => CurrencyTab.EldritchExaltedOrb,
			[CurrencyNames.WildLifeforce] = () => CurrencyTab.WildCrystallisedLifeforce,
			[CurrencyNames.VividLifeforce] = () => CurrencyTab.VividCrystallisedLifeforce,
			[CurrencyNames.PrimalLifeforce] = () => CurrencyTab.PrimalCrystallisedLifeforce,
			[CurrencyNames.SacredLifeforce] = () => CurrencyTab.SacredCrystallisedLifeforce
		};
		dictionary_1 = new Dictionary<string, Func<InventoryControlWrapper>>
		{
			[CurrencyNames.ScrollFragment] = () => CurrencyTab.ScrollFragment,
			[CurrencyNames.Wisdom] = () => CurrencyTab.ScrollOfWisdom,
			[CurrencyNames.Portal] = () => CurrencyTab.PortalScroll,
			[CurrencyNames.Enkindling] = () => CurrencyTab.EnkindlingOrb,
			[CurrencyNames.Instilling] = () => CurrencyTab.InstillingOrb,
			[CurrencyNames.Whetstone] = () => CurrencyTab.BlacksmithsWhetstone,
			[CurrencyNames.Scrap] = () => CurrencyTab.ArmourersScrap,
			[CurrencyNames.Glassblower] = () => CurrencyTab.GlassblowersBauble,
			[CurrencyNames.Gemcutter] = () => CurrencyTab.GemcuttersPrism,
			[CurrencyNames.Chisel] = () => CurrencyTab.CartographersChisel,
			[CurrencyNames.Transmutation] = () => CurrencyTab.OrbOfTransmutation,
			[CurrencyNames.Alteration] = () => CurrencyTab.OrbOfAlteration,
			[CurrencyNames.Annulment] = () => CurrencyTab.OrbOfAnnulment,
			[CurrencyNames.Chance] = () => CurrencyTab.OrbOfChance,
			[CurrencyNames.Exalted] = () => CurrencyTab.ExaltedOrb,
			[CurrencyNames.Mirror] = () => CurrencyTab.MirrorOfKalandra,
			[CurrencyNames.Regal] = () => CurrencyTab.RegalOrb,
			[CurrencyNames.Alchemy] = () => CurrencyTab.OrbOfAlchemy,
			[CurrencyNames.Chaos] = () => CurrencyTab.ChaosOrb,
			[CurrencyNames.VeiledChaos] = () => CurrencyTab.VeiledChaosOrb,
			[CurrencyNames.TransmutationShard] = () => CurrencyTab.TransmutationShard,
			[CurrencyNames.AlterationShard] = () => CurrencyTab.AlterationShard,
			[CurrencyNames.AnnulmentShard] = () => CurrencyTab.AnnulmentShard,
			[CurrencyNames.Augmentation] = () => CurrencyTab.OrbOfAugmentation,
			[CurrencyNames.ExaltedShard] = () => CurrencyTab.ExaltedShard,
			[CurrencyNames.MirrorShard] = () => CurrencyTab.MirrorShard,
			[CurrencyNames.RegalShard] = () => CurrencyTab.RegalShard,
			[CurrencyNames.AlchemyShard] = () => CurrencyTab.AlchemyShard,
			[CurrencyNames.ChaosShard] = () => CurrencyTab.ChaosShard,
			[CurrencyNames.Divine] = () => CurrencyTab.DivineOrb,
			[CurrencyNames.Jeweller] = () => CurrencyTab.JewellersOrb,
			[CurrencyNames.Fusing] = () => CurrencyTab.OrbOfFusing,
			[CurrencyNames.Chromatic] = () => CurrencyTab.ChromaticOrb,
			[CurrencyNames.AwakenedSextant] = () => CurrencyTab.AwakenedSextant,
			[CurrencyNames.ElevatedSextant] = () => CurrencyTab.ElevatedSextant,
			[CurrencyNames.Harbinger] = () => CurrencyTab.HarbingersOrb,
			[CurrencyNames.Horizon] = () => CurrencyTab.OrbOfHorizon,
			[CurrencyNames.FracturingOrb] = () => CurrencyTab.FracturingOrb,
			[CurrencyNames.Ancient] = () => CurrencyTab.AncientOrb,
			[CurrencyNames.Binding] = () => CurrencyTab.OrbofBinding,
			[CurrencyNames.Engineer] = () => CurrencyTab.EngineersOrb,
			[CurrencyNames.Regret] = () => CurrencyTab.OrbOfRegret,
			[CurrencyNames.Unmaking] = () => CurrencyTab.OrbOfUnmaking,
			[CurrencyNames.HarbingerShard] = () => CurrencyTab.HarbingersShard,
			[CurrencyNames.HorizonShard] = () => CurrencyTab.HorizonShard,
			[CurrencyNames.FracturingShard] = () => CurrencyTab.FracturingShard,
			[CurrencyNames.AncientShard] = () => CurrencyTab.AncientShard,
			[CurrencyNames.BindingShard] = () => CurrencyTab.BindingShard,
			[CurrencyNames.EngineerShard] = () => CurrencyTab.EngineersShard,
			[CurrencyNames.Scouring] = () => CurrencyTab.OrbOfScouring,
			[CurrencyNames.Sacred] = () => CurrencyTab.SacredOrb,
			[CurrencyNames.Blessed] = () => CurrencyTab.BlessedOrb,
			[CurrencyNames.Vaal] = () => CurrencyTab.VaalOrb
		};
	}
}
