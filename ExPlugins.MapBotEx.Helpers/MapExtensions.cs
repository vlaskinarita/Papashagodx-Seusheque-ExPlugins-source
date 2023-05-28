using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;

namespace ExPlugins.MapBotEx.Helpers;

public static class MapExtensions
{
	public class AtlasData
	{
		private static readonly HashSet<string> hashSet_0;

		public static bool IsCompleted(Item map)
		{
			if (map.Stats.ContainsKey((StatTypeGGG)10342))
			{
				return true;
			}
			string item = map.CleanName();
			return hashSet_0.Contains(item);
		}

		public static bool IsCompleted(string fullName)
		{
			return hashSet_0.Contains(fullName.Replace(" Map", ""));
		}

		static AtlasData()
		{
			hashSet_0 = Atlas.BonusCompletedAreas.Select((DatWorldAreaWrapper a) => a.Name).ToHashSet();
		}
	}

	private static readonly GeneralSettings generalSettings_0;

	private static readonly Dictionary<string, MapData> dictionary_0;

	private static readonly Dictionary<string, AffixData> dictionary_1;

	public static bool IsMap(this Item item)
	{
		if (item.Metadata.Contains("MapFragments/CurrencyAfflictionFragment"))
		{
			return true;
		}
		return item.Class == "Map";
	}

	public static bool IsMavenInvitation(this Item item)
	{
		return item.Metadata.StartsWith("Metadata/Items/MapFragments/Maven/MavenMap") && item.Class != "MiscMapItem";
	}

	public static bool IsMavenInvitation(this CachedMapItem item)
	{
		return item.Metadata.StartsWith("Metadata/Items/MapFragments/Maven/MavenMap") && item.Class != "MiscMapItem";
	}

	public static string CleanName(this CachedMapItem map)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return ((int)map.Rarity == 3) ? map.FullName : map.Name.Replace(" Map", "");
	}

	public static string CleanName(this Item map)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return ((int)map.Rarity == 3) ? map.FullName : map.Name.Replace(" Map", "");
	}

	public static bool BelowTierLimit(this Item map)
	{
		return map.MapTier <= generalSettings_0.MaxMapTier;
	}

	public static bool BelowTierLimit(this CachedMapItem map)
	{
		return map.MapTier <= generalSettings_0.MaxMapTier;
	}

	public static int Priority(this Item map)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		if (map.IsMavenInvitation() || map.Metadata.Contains("MapFragments/CurrencyAfflictionFragment"))
		{
			return 10001;
		}
		if (!dictionary_0.TryGetValue(map.CleanName(), out var value))
		{
			return int.MinValue;
		}
		int num = value.Priority;
		if (generalSettings_0.AtlasExplorationEnabled && !value.UnsupportedBossroom && !map.Ignored())
		{
			int mapTier = map.MapTier;
			num += 10000;
			num += mapTier * 3;
			if (!map.Completed())
			{
				num += 400;
				if (mapTier < 5 && (int)map.Rarity >= 1)
				{
					num += 50;
				}
				if (mapTier >= 5 && (int)map.Rarity == 2)
				{
					num += 100;
				}
				if (mapTier >= 11 && map.IsCorrupted)
				{
					num += 300;
				}
			}
		}
		return num;
	}

	public static int Priority(this CachedMapItem map)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		if (map.IsMavenInvitation() || map.Metadata.Contains("MapFragments/CurrencyAfflictionFragment"))
		{
			return 10001;
		}
		if (dictionary_0.TryGetValue(map.CleanName(), out var value))
		{
			int num = value.Priority;
			if (generalSettings_0.AtlasExplorationEnabled && !value.UnsupportedBossroom && !map.Ignored())
			{
				int mapTier = map.MapTier;
				num += 10000;
				num += mapTier * 3;
				if (!map.Completed())
				{
					num += 400;
					if (mapTier < 5 && (int)map.Rarity >= 1)
					{
						num += 50;
					}
					if (mapTier >= 5 && (int)map.Rarity == 2)
					{
						num += 100;
					}
					if (mapTier >= 11 && map.IsCorrupted)
					{
						num += 300;
					}
				}
			}
			return num;
		}
		return int.MinValue;
	}

	public static bool Ignored(this Item map)
	{
		MapData value;
		return !dictionary_0.TryGetValue(map.CleanName(), out value) || value.Ignored;
	}

	public static bool Ignored(this CachedMapItem map)
	{
		MapData value;
		return !dictionary_0.TryGetValue(map.CleanName(), out value) || value.Ignored;
	}

	public static bool Completed(this Item map)
	{
		return map.CleanName().Equals("Vaal Temple") || AtlasData.IsCompleted(map.CleanName());
	}

	public static bool Completed(this CachedMapItem map)
	{
		return map.CleanName().Equals("Vaal Temple") || AtlasData.IsCompleted(map.CleanName());
	}

	public static string GetBannedAffix(this CachedMapItem map)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Invalid comparison between Unknown and I4
		Rarity rarity = map.Rarity;
		if (map.Stats.ContainsKey((StatTypeGGG)10342) && map.IsCorrupted)
		{
			return null;
		}
		if ((int)rarity == 1 || (int)rarity == 2)
		{
			bool flag = map.CleanName() == MapNames.Peninsula;
			foreach (KeyValuePair<string, string> affix in map.AffixList)
			{
				if (!flag || !(affix.Key == "Twinned"))
				{
					if (!string.IsNullOrWhiteSpace(affix.Key))
					{
						if (!dictionary_1.TryGetValue(affix.Key, out var value))
						{
							GlobalLog.Error($"[GetBannedAffix] Unknown map affix \"{affix.Key}\" [{affix.Value}]. Map: T{map.MapTier} {map.FullName} {map.Name}");
						}
						else
						{
							if ((int)rarity == 1 && value.RerollMagic)
							{
								return affix.Key;
							}
							if (value.RerollRare)
							{
								return affix.Key;
							}
						}
						continue;
					}
					return null;
				}
				return affix.Key;
			}
			return null;
		}
		return null;
	}

	public static string GetBannedAffix(this Item map)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		Rarity rarity = map.Rarity;
		if (!map.Stats.ContainsKey((StatTypeGGG)10342) || !map.IsCorrupted)
		{
			if ((int)rarity == 1 || (int)rarity == 2)
			{
				bool flag = map.CleanName() == MapNames.Peninsula;
				foreach (ModAffix explicitAffix in map.ExplicitAffixes)
				{
					string displayName = explicitAffix.DisplayName;
					if (!flag || !(displayName == "Twinned"))
					{
						if (!dictionary_1.TryGetValue(displayName, out var value))
						{
							GlobalLog.Debug("[GetBannedAffix] Unknown map affix \"" + displayName + "\".");
							continue;
						}
						if ((int)rarity != 1 || !value.RerollMagic)
						{
							if (!value.RerollRare)
							{
								continue;
							}
							return displayName;
						}
						return displayName;
					}
					return displayName;
				}
				return null;
			}
			return null;
		}
		return null;
	}

	public static int IIQ(this Item map)
	{
		int quality = map.Quality;
		map.Stats.TryGetValue((StatTypeGGG)1038, out var value);
		return quality + value;
	}

	public static bool CanAugment(this Item map)
	{
		return map.ExplicitAffixes.Count() < 2;
	}

	public static bool ShouldUpgrade(this Item map, Upgrade upgrade)
	{
		int mapTier = map.MapTier;
		if (generalSettings_0.AtlasExplorationEnabled && !map.Stats.ContainsKey((StatTypeGGG)10342) && !AtlasData.IsCompleted(map) && dictionary_0.TryGetValue(map.CleanName(), out var value) && !value.IgnoreBossroom)
		{
			if (mapTier >= 6 && (upgrade == generalSettings_0.RareUpgrade || upgrade == generalSettings_0.MagicRareUpgrade))
			{
				return true;
			}
			if (mapTier >= 11 && upgrade == generalSettings_0.VaalUpgrade)
			{
				return true;
			}
		}
		if (map.Stats.ContainsKey((StatTypeGGG)10342))
		{
			if (upgrade == generalSettings_0.ChiselUpgrade)
			{
				return true;
			}
			if (upgrade == generalSettings_0.RareUpgrade)
			{
				return map.Stats.ContainsKey((StatTypeGGG)14763) ? GeneralSettings.Instance.AlchRavagedMaps : GeneralSettings.Instance.AlchBlightedMaps;
			}
			if (upgrade == generalSettings_0.VaalUpgrade)
			{
				return GeneralSettings.Instance.VaalBlightedMaps;
			}
		}
		if (!upgrade.TierEnabled || mapTier < upgrade.Tier)
		{
			if (!upgrade.PriorityEnabled || map.Priority() < upgrade.Priority)
			{
				return false;
			}
			if (upgrade.TierEnabled)
			{
				return mapTier >= upgrade.Tier;
			}
			return true;
		}
		if (!upgrade.PriorityEnabled)
		{
			return true;
		}
		return map.Priority() >= upgrade.Priority;
	}

	public static bool ShouldUpgrade(this CachedMapItem map, Upgrade upgrade)
	{
		int mapTier = map.MapTier;
		if (generalSettings_0.AtlasExplorationEnabled && !map.Stats.ContainsKey((StatTypeGGG)10342) && !AtlasData.IsCompleted(map.CleanName()) && dictionary_0.TryGetValue(map.CleanName(), out var value) && !value.IgnoreBossroom)
		{
			if (mapTier >= 6 && (upgrade == generalSettings_0.RareUpgrade || upgrade == generalSettings_0.MagicRareUpgrade))
			{
				return true;
			}
			if (mapTier >= 11 && upgrade == generalSettings_0.VaalUpgrade)
			{
				return true;
			}
		}
		if (upgrade != generalSettings_0.VaalUpgrade || !map.Stats.ContainsKey((StatTypeGGG)10342))
		{
			if (upgrade.TierEnabled && mapTier >= upgrade.Tier)
			{
				return true;
			}
			if (!upgrade.PriorityEnabled || map.Priority() < upgrade.Priority)
			{
				return false;
			}
			return true;
		}
		return GeneralSettings.Instance.VaalBlightedMaps;
	}

	public static bool ShouldSell(this CachedMapItem map)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		if (!map.Stats.ContainsKey((StatTypeGGG)10342))
		{
			if (!map.FullName.Equals("Simulacrum"))
			{
				if ((int)map.Rarity != 3)
				{
					if (generalSettings_0.SellIgnoredMaps && map.Ignored())
					{
						return true;
					}
					if (map.MapTier > generalSettings_0.MaxSellTier)
					{
						return false;
					}
					bool atlasExplorationEnabled;
					if (!(atlasExplorationEnabled = GeneralSettings.Instance.AtlasExplorationEnabled) || map.Completed())
					{
						if (!atlasExplorationEnabled && map.Priority() > generalSettings_0.MaxSellPriority)
						{
							return false;
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public static bool IsSacrificeFragment(this Item item)
	{
		if (!item.Metadata.Contains("CurrencyVaalFragment"))
		{
			return false;
		}
		double num = PoeNinjaTracker.LookupChaosValue(item);
		return num != 0.0 && num <= GeneralSettings.Instance.MaxFragmentCost;
	}

	static MapExtensions()
	{
		generalSettings_0 = GeneralSettings.Instance;
		dictionary_0 = MapSettings.Instance.MapDict;
		dictionary_1 = AffixSettings.Instance.AffixDict;
	}
}
