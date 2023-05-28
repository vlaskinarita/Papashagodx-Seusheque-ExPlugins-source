using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class SellMapTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly GeneralSettings Settings;

	public string Name => "SellMapTask";

	public string Description => "Task for selling maps";

	public string Author => "";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (GeneralSettings.Instance.IsOnRun)
		{
			return false;
		}
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsMyHideoutArea)
		{
			if (CachedMaps.Instance.MapCache != null && CachedMaps.Instance.MapCache.Maps.Any())
			{
				if (Settings.SellEnabled)
				{
					if (Inventories.AvailableInventorySquares >= 3)
					{
						string firstMapTab = ExtensionsSettings.Instance.GetTabsForCategory("Maps").First();
						if (!(await Inventories.OpenStashTab(firstMapTab, Name)))
						{
							GlobalLog.Error("[SellMapTask] Fail to open stash tab \"" + firstMapTab + "\".");
							return false;
						}
						List<CachedMapItem> maps = ((!GeneralSettings.Instance.AtlasExplorationEnabled) ? (from m in CachedMaps.Instance.MapCache.Maps
							where m.MapTier >= 1 && m.ShouldSell() && !smethod_0(m)
							orderby m.Priority(), m.MapTier
							select m).ToList() : (from m in CachedMaps.Instance.MapCache.Maps
							where m.MapTier >= 1 && m.ShouldSell() && !smethod_0(m)
							orderby m.MapTier
							select m).ToList());
						GlobalLog.Warn($"[SellMapTask] Map count before grouping: {maps.Count}");
						if (maps.Count != 0)
						{
							List<CachedMapItem[]> mapGroups = new List<CachedMapItem[]>();
							foreach (var mapGroup2 in from m in maps
								group m by new { m.Name, m.MapTier })
							{
								List<CachedMapItem> groupList = mapGroup2.ToList();
								for (int k = 3; k <= groupList.Count; k += 3)
								{
									mapGroups.Add(new CachedMapItem[3]
									{
										groupList[k - 3],
										groupList[k - 2],
										groupList[k - 1]
									});
								}
							}
							if (mapGroups.Count == 0)
							{
								GlobalLog.Info("[SellMapTask] No map group for sale was found.");
								return false;
							}
							GlobalLog.Info($"[SellMapTask] Map groups for sale: {mapGroups.Count}");
							List<Vector2i> forSell = new List<Vector2i>();
							foreach (CachedMapItem[] mapGroup in mapGroups)
							{
								if (Inventories.AvailableInventorySquares >= 3)
								{
									if (!Settings.SellIgnoredMaps || !mapGroup[0].Ignored())
									{
										int mapAmount = maps.Count((CachedMapItem i) => i.MapTier >= 1 && (int)i.Rarity != 3);
										if (mapAmount - 3 < Settings.MinMapAmount)
										{
											GlobalLog.Warn($"[SellMapTask] Min map amount is reached {mapAmount}(-3) from required {Settings.MinMapAmount}");
											break;
										}
									}
									for (int j = 0; j < 3; j++)
									{
										CachedMapItem map = mapGroup[j];
										Item toAdd = await Inventories.TakeMapFromStash(map);
										GlobalLog.Info($"[SellMapTask] Now getting {j + 1}/{3} \"{map.Name}\".");
										if (!((RemoteMemoryObject)(object)toAdd == (RemoteMemoryObject)null))
										{
											forSell.Add(toAdd.LocationTopLeft);
											continue;
										}
										ErrorManager.ReportError();
										return true;
									}
									continue;
								}
								GlobalLog.Error("[SellMapTask] Not enough inventory space.");
								break;
							}
							await Wait.SleepSafe(200);
							if (forSell.Count == 0)
							{
								return false;
							}
							if (!(await TownNpcs.SellItems(forSell)))
							{
								ErrorManager.ReportError();
							}
							return true;
						}
						return false;
					}
					GlobalLog.Error("[SellMapTask] Not enough inventory space.");
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Tick()
	{
	}

	public void Stop()
	{
	}

	static SellMapTask()
	{
		Settings = GeneralSettings.Instance;
	}

	[CompilerGenerated]
	internal static bool smethod_0(CachedMapItem item)
	{
		return item.Stats.ContainsKey((StatTypeGGG)6827) || item.Stats.ContainsKey((StatTypeGGG)13845) || item.Stats.ContainsKey((StatTypeGGG)10342) || item.Name.Contains("Vaal Temple");
	}
}
