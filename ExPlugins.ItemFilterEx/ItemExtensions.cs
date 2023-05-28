using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;

namespace ExPlugins.ItemFilterEx;

public static class ItemExtensions
{
	public static bool IsDivinationCard(this Item i)
	{
		return i.Class == "DivinationCard" || i.Metadata.Contains("DivinationCard") || i.RenderArt.Contains("Divination");
	}

	public static bool IsMap(this Item i)
	{
		return i.Class == "Map";
	}

	public static bool IsBlightedMap(this Item i)
	{
		return i.Stats.ContainsKey((StatTypeGGG)10342);
	}

	public static bool IsInfluencedMap(this Item i)
	{
		return i.Stats.ContainsKey((StatTypeGGG)6827) || i.Stats.ContainsKey((StatTypeGGG)13845);
	}

	public static bool IsInfluencedMap(this CachedMapItem i)
	{
		return i.Stats.ContainsKey((StatTypeGGG)6827) || i.Stats.ContainsKey((StatTypeGGG)13845);
	}

	public static bool smethod_0(this Item i)
	{
		return i.SocketCount == 6;
	}

	public static double Price(this Item i)
	{
		if (ItemFilterExSettings.Instance.PriceCheckFracturedItems && i.IsFractured)
		{
			List<ModAffix> list_ = i.Affixes.Where((ModAffix a) => a.IsFractured).ToList();
			bool flag = list_.First().Stats.First().Description.Contains("to Level of all");
			if (list_.First().Level >= 75 && flag)
			{
				Task<double> task = Task.Run(async () => await PoeNinjaTracker.PriceCheckOfficial(i, list_, checkIlvl: true, checkCorrupted: true));
				return task.Result;
			}
			return -1.0;
		}
		OfficialPricecheckEntry officialPricecheckEntry_0 = ItemFilterExSettings.Instance.OfficialPricecheck.FirstOrDefault((OfficialPricecheckEntry e) => e.FullName.Equals(i.FullName));
		if (i.IsIdentified && officialPricecheckEntry_0 != null)
		{
			List<ModAffix> list_0 = new List<ModAffix>();
			if (officialPricecheckEntry_0.CheckStats)
			{
				list_0 = i.Affixes.Where((ModAffix a) => officialPricecheckEntry_0.StatsToCheck.Contains(a.Stats.First().Description.Replace("{0:+d}", "#").Replace("{0}", "#").Replace("\\n", " "))).ToList();
			}
			Task<double> task2 = Task.Run(async () => await PoeNinjaTracker.PriceCheckOfficial(i, list_0, officialPricecheckEntry_0.CheckIlvl, officialPricecheckEntry_0.CheckCorrupted));
			return task2.Result;
		}
		return PoeNinjaTracker.LookupChaosValue(i);
	}

	public static bool IsSmallRgb(this Item i)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return i.IsChromatic && i.Size.X * i.Size.Y <= 4;
	}

	public static bool IsTinyRgb(this Item i)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return i.IsChromatic && i.Size.X * i.Size.Y <= 3;
	}

	public static bool IsEnchanted(this Item i)
	{
		return i.SocketCount >= 1 && i.Affixes.Any((ModAffix a) => a.Category.Equals("SkillEnchantment"));
	}
}
