using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.StashManager;

internal class Class17 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly Vector2i vector2i_0 = default(Vector2i);

	private readonly Dictionary<int, int> dictionary_0 = new Dictionary<int, int>();

	private Stopwatch qtcgMyCsoX;

	private bool bool_0;

	private static readonly List<string> list_0;

	public JsonSettings Settings => (JsonSettings)(object)StashManagerSettings.Instance;

	public string Name => "ScanTabsAndSellMapsTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (((IAuthored)BotManager.Current).Name != "MapBotEx")
		{
			return false;
		}
		if ((!area.IsTown && !area.IsHideoutArea) || !StashManagerSettings.Instance.ShouldSellMaps)
		{
			return false;
		}
		if (qtcgMyCsoX.Elapsed.TotalSeconds < (double)StashManagerSettings.Instance.SecondsBetweenScan && bool_0)
		{
			return false;
		}
		if (!GeneralSettings.Instance.SellEnabled)
		{
			if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
			{
				await Inventories.OpenStash();
			}
			ExtensionsSettings settings = ExtensionsSettings.Instance;
			List<string> tabList = (from t in settings.GetTabsForCategory("Maps")
				where !list_0.Contains(t)
				select t).ToList();
			if (tabList.Any())
			{
				List<Vector2i> itemsToSell = new List<Vector2i>();
				List<Item> sortedMaps = new List<Item>();
				foreach (string tab in tabList)
				{
					if (await Inventories.OpenStashTab(tab, Name))
					{
						if (StashUi.StashTabInfo.IsPremiumMap)
						{
							list_0.Add(tab);
							continue;
						}
						sortedMaps = (string.IsNullOrEmpty(StashManagerSettings.Instance.MapRegionToSellLast) ? (from m in ClassExtensions.Where(StashUi.InventoryControl.Inventory.Items, (Item m) => m.Class == "Map" && !m.Stats.ContainsKey((StatTypeGGG)10342) && !m.Stats.ContainsKey((StatTypeGGG)6827) && !m.Stats.ContainsKey((StatTypeGGG)13845) && (int)m.Rarity != 3 && ((m.Priority() <= StashManagerSettings.Instance.MaxPriorityToSellMap && !m.Stats.ContainsKey((StatTypeGGG)10055)) || m.Ignored() || (m.IsCorrupted && m.GetBannedAffix() != null)))
							orderby m.IsCorrupted descending
							select m).ToList() : (from m in ClassExtensions.Where(StashUi.InventoryControl.Inventory.Items, (Item m) => m.Class == "Map" && !m.Stats.ContainsKey((StatTypeGGG)10342) && !m.Stats.ContainsKey((StatTypeGGG)6827) && !m.Stats.ContainsKey((StatTypeGGG)13845) && (int)m.Rarity != 3 && ((m.Priority() <= StashManagerSettings.Instance.MaxPriorityToSellMap && !m.Stats.ContainsKey((StatTypeGGG)10055)) || m.Ignored() || (m.IsCorrupted && m.GetBannedAffix() != null)))
							orderby m.IsCorrupted descending, m.MapRegion == StashManagerSettings.Instance.MapRegionToSellLast
							select m).ToList());
					}
					GlobalLog.Debug($"[StashManager(Sell_Maps)] Maps amount: {sortedMaps.Count}");
					foreach (Item map in sortedMaps)
					{
						int tier = map.MapTier;
						if (!dictionary_0.ContainsKey(tier))
						{
							dictionary_0.Add(tier, 1);
						}
						else
						{
							dictionary_0[tier]++;
						}
					}
					foreach (Item map2 in sortedMaps)
					{
						int int_0 = map2.MapTier;
						Vector2i pos = map2.LocationTopLeft;
						if (dictionary_0[int_0] <= ClassExtensions.FirstOrDefault(StashManagerSettings.Instance.MapLimits, (MapEntry m) => m.MapTier == int_0)?.Amount && !map2.Ignored() && (!map2.IsCorrupted || map2.GetBannedAffix() == null))
						{
							continue;
						}
						string name = map2.Name;
						Vector2i posToMove = default(Vector2i);
						bool flag = false;
						for (int i = 0; i < 12; i++)
						{
							for (int j = 0; j < 5; j++)
							{
								if (InventoryUi.InventoryControl_Main.Inventory.CanFitItemSizeAt(1, 1, i, j))
								{
									posToMove = new Vector2i(i, j);
									if (StashManagerSettings.Instance.DebugMode)
									{
										GlobalLog.Debug($"[StashManager(Sell_Maps)] Can fit item on pos {pos.X}, {pos.Y} to pos {posToMove.X}, {posToMove.Y}");
									}
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
						if (!(posToMove != vector2i_0))
						{
							GlobalLog.Error("[StashManager(Sell_Maps)] There was no free space!");
							continue;
						}
						GlobalLog.Debug($"[StashManager(Sell_Maps)] Now picking item at {pos.X}, {pos.Y} in stash");
						await StashUi.InventoryControl.PickItemToCursor(new Vector2i(pos.X, pos.Y));
						await Inventories.WaitForCursorToHaveItem();
						GlobalLog.Debug($"[StashManager(Sell_Maps)] Now placing item at {posToMove.X}, {posToMove.Y} in inventory");
						await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(posToMove.X, posToMove.Y));
						await Inventories.WaitForCursorToBeEmpty();
						dictionary_0[int_0]--;
						if (StashManagerSettings.Instance.DebugMode)
						{
							GlobalLog.Debug($"[StashManager(Sell_Maps)] Map {name}, tier {int_0} was moved from the stash, maps of this tier remaining: {dictionary_0[int_0]}");
						}
						itemsToSell.Add(posToMove);
					}
				}
				GlobalLog.Info($"[StashManager(Sell_Maps)] {itemsToSell.Count} items to sell.");
				dictionary_0.Clear();
				GlobalLog.Debug($"[StashManager(Sell_Maps)] Last scan time: {qtcgMyCsoX.Elapsed.TotalSeconds}");
				qtcgMyCsoX.Restart();
				bool_0 = true;
				if (itemsToSell.Count == 0)
				{
					return false;
				}
				if (!(await TownNpcs.SellItems(itemsToSell)))
				{
					ErrorManager.ReportError();
					return false;
				}
				GlobalLog.Debug("[StashManager(Sell_Maps)] Exceed maps were sucessfully sold!");
				return true;
			}
			return false;
		}
		return false;
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
	}

	public void Disable()
	{
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
		if (qtcgMyCsoX == null)
		{
			qtcgMyCsoX = Stopwatch.StartNew();
		}
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	static Class17()
	{
		list_0 = new List<string>();
	}
}
