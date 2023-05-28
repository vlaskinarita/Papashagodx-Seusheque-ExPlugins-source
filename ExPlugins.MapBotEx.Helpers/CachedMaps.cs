using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using Newtonsoft.Json;

namespace ExPlugins.MapBotEx.Helpers;

public class CachedMaps
{
	public class MapTierInfo
	{
		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private HashSet<MapInfo> hashSet_0;

		public int Tier
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		public HashSet<MapInfo> MapsInfos
		{
			[CompilerGenerated]
			get
			{
				return hashSet_0;
			}
			[CompilerGenerated]
			set
			{
				hashSet_0 = value;
			}
		}

		public MapTierInfo(int tier, HashSet<MapInfo> mapsInfos)
		{
			Tier = tier;
			MapsInfos = mapsInfos;
		}
	}

	public class MapTabInfo
	{
		[CompilerGenerated]
		private List<MapTierInfo> list_0;

		[CompilerGenerated]
		private HashSet<CachedMapItem> hashSet_0;

		public List<MapTierInfo> MapTierInfos
		{
			[CompilerGenerated]
			get
			{
				return list_0;
			}
			[CompilerGenerated]
			set
			{
				list_0 = value;
			}
		}

		public HashSet<CachedMapItem> Maps
		{
			[CompilerGenerated]
			get
			{
				return hashSet_0;
			}
			[CompilerGenerated]
			set
			{
				hashSet_0 = value;
			}
		}

		public MapTabInfo(List<MapTierInfo> mapsTierInfos, HashSet<CachedMapItem> maps)
		{
			MapTierInfos = mapsTierInfos;
			Maps = maps;
		}
	}

	private static readonly string string_0;

	private static CachedMaps cachedMaps_0;

	[CompilerGenerated]
	private MapTabInfo mapTabInfo_0;

	public static bool Cached;

	public static CachedMaps Instance => cachedMaps_0 ?? (cachedMaps_0 = new CachedMaps());

	public MapTabInfo MapCache
	{
		[CompilerGenerated]
		get
		{
			return mapTabInfo_0;
		}
		[CompilerGenerated]
		set
		{
			mapTabInfo_0 = value;
		}
	}

	private CachedMaps()
	{
	}

	public void Load()
	{
		try
		{
			string text = File.ReadAllText(string_0);
			if (!string.IsNullOrWhiteSpace(text))
			{
				MapTabInfo mapTabInfo = JsonConvert.DeserializeObject<MapTabInfo>(text);
				if (mapTabInfo != null)
				{
					MapCache = mapTabInfo;
					GlobalLog.Warn("[MapBotEx] Map cache loaded.");
				}
				else
				{
					GlobalLog.Error("[MapBotEx] Fail to load \"MapCache.json\". Cache is corrupted.");
				}
			}
			else
			{
				GlobalLog.Error("[MapBotEx] Fail to load \"MapCache.json\". File is empty.");
			}
		}
		catch (Exception ex)
		{
			GlobalLog.Error("[MapBotEx] " + ex.Message);
		}
	}

	public void Save()
	{
		MapTabInfo mapTabInfo_0 = MapCache;
		if (mapTabInfo_0 == null)
		{
			return;
		}
		BackgroundWorker backgroundWorker = new BackgroundWorker();
		backgroundWorker.DoWork += delegate
		{
			mapTabInfo_0.MapTierInfos = mapTabInfo_0.MapTierInfos.OrderBy((MapTierInfo i) => i.Tier).ToList();
			mapTabInfo_0.Maps = mapTabInfo_0.Maps.OrderBy((CachedMapItem m) => m.StashTab).ToHashSet();
			string contents = JsonConvert.SerializeObject((object)mapTabInfo_0, (Formatting)1);
			File.WriteAllText(string_0, contents);
			GlobalLog.Warn("[MapBotEx] \"MapCache.json\" saved.");
		};
		GlobalLog.Warn("[MapBotEx] Saving Map Cache!");
		backgroundWorker.RunWorkerAsync();
	}

	static CachedMaps()
	{
		string_0 = Path.Combine(Configuration.Instance.Path, "MapBotEx", "MapCache.json");
	}
}
