using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;

internal class Class39 : Class36
{
	public class Class46 : IComparable<Class46>
	{
		public readonly List<int> list_0;

		public readonly int int_0;

		public bool CanFit => list_0.Count <= Inventories.AvailableInventorySquares;

		public Class46(List<int> qualities, int totalQuality)
		{
			list_0 = qualities;
			int_0 = totalQuality;
		}

		public int CompareTo(Class46 other)
		{
			int num = int_0 - other.int_0;
			if (num == 0)
			{
				return list_0.Count - other.list_0.Count;
			}
			return num;
		}

		public override string ToString()
		{
			return string.Format("({0})-[{1}]", int_0, string.Join("+", list_0));
		}
	}

	public class Class47
	{
		private readonly List<int> list_0;

		private readonly List<Class46> list_1;

		private Class46 class46_0;

		public Class46 BestSet
		{
			get
			{
				if (class46_0 == null)
				{
					if (list_1.Count <= 0)
					{
						return null;
					}
					list_1.Sort();
					return list_1[0];
				}
				return class46_0;
			}
		}

		public Class47(List<int> numbers)
		{
			list_0 = numbers;
			list_1 = new List<Class46>();
			FindSets(new bool[numbers.Count], 0, 0);
		}

		private void FindSets(bool[] solution, int currentSum, int index)
		{
			if (currentSum != 40)
			{
				if (currentSum > 40)
				{
					if (currentSum > 58)
					{
						GlobalLog.Error(currentSum);
					}
					else
					{
						list_1.Add(CreateGemSet(solution, currentSum));
					}
				}
				else if (class46_0 == null && index != list_0.Count)
				{
					solution[index] = true;
					currentSum += list_0[index];
					FindSets(solution, currentSum, index + 1);
					solution[index] = false;
					currentSum -= list_0[index];
					FindSets(solution, currentSum, index + 1);
				}
			}
			else
			{
				class46_0 = CreateGemSet(solution, currentSum);
			}
		}

		private Class46 CreateGemSet(bool[] solution, int sum)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < solution.Length; i++)
			{
				if (solution[i])
				{
					list.Add(list_0[i]);
				}
			}
			return new Class46(list, sum);
		}
	}

	private static string string_0;

	private static Class46 class46_0;

	public override bool Enabled => Class36.Settings.GcpEnabled;

	public override bool ShouldExecute => string_0 != null && !base.ErrorLimitReached;

	private static IEnumerable<Item> StashGems => Inventories.StashTabItems.Where((Item i) => (RemoteMemoryObject)(object)i.Components.SkillGemComponent != (RemoteMemoryObject)null);

	private static List<int> GemQualitiesInCurrentTab => (from g in StashGems.Where(smethod_1)
		select g.Quality).ToList();

	public override async Task Execute()
	{
		GlobalLog.Info("[VendorTask] Now going to sell quality skill gems for Gemcutter's Prism.");
		if (!(await TakeGcpSets()))
		{
			ReportError();
		}
		else if (!(await smethod_0()))
		{
			ReportError();
		}
		else if (!(await SellGems()))
		{
			ReportError();
		}
	}

	public override void OnStashing(CachedItem item)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (string_0 != null || (int)item.Type.ItemType != 9 || item.Quality < 1)
		{
			return;
		}
		List<int> gemQualitiesInCurrentTab = GemQualitiesInCurrentTab;
		if (gemQualitiesInCurrentTab.Count != 0)
		{
			Class47 @class = new Class47(gemQualitiesInCurrentTab);
			class46_0 = @class.BestSet;
			if (class46_0 == null)
			{
				GlobalLog.Info("[OnQGemStash] Gem set for gcp recipe was not found.");
				return;
			}
			GlobalLog.Info($"[OnQGemStash] Found gem set for gcp recipe {class46_0}");
			string_0 = StashUi.TabControl.CurrentTabName;
		}
	}

	public override void ResetData()
	{
		string_0 = null;
		class46_0 = null;
	}

	private static async Task<bool> TakeGcpSets()
	{
		if (await Inventories.OpenStashTab(string_0, "GcpRecipe"))
		{
			while (true)
			{
				if (class46_0 == null)
				{
					List<int> qualities = GemQualitiesInCurrentTab;
					if (qualities.Count == 0)
					{
						break;
					}
					Class47 finder = new Class47(qualities);
					class46_0 = finder.BestSet;
					if (class46_0 == null)
					{
						GlobalLog.Info("[TakeGcpSets] No more gem sets for gcp recipe were found in \"" + string_0 + "\" tab.");
						string_0 = null;
						return true;
					}
				}
				if (class46_0.CanFit)
				{
					GlobalLog.Warn($"[TakeGcpSets] Now taking gcp set {class46_0}");
					foreach (int int_0 in class46_0.list_0)
					{
						Item gem = StashGems.FirstOrDefault((Item g) => smethod_1(g) && g.Quality == int_0);
						if (!((RemoteMemoryObject)(object)gem == (RemoteMemoryObject)null))
						{
							if (await Inventories.FastMoveFromStashTab(gem.LocationTopLeft))
							{
								continue;
							}
							return false;
						}
						GlobalLog.Error($"[TakeGcpSets] Unexpected error. Fail to find gem with quality {int_0} as a part of {class46_0} gcp set.");
						string_0 = null;
						class46_0 = null;
						return false;
					}
					class46_0 = null;
					continue;
				}
				GlobalLog.Warn("[TakeGcpSets] Not enough inventory space for current gcp set.");
				class46_0 = null;
				return true;
			}
			GlobalLog.Info("[TakeGcpSets] No quality gems were found in \"" + string_0 + "\" tab.");
			string_0 = null;
			return true;
		}
		return false;
	}

	private static async Task<bool> smethod_0()
	{
		if (!StashUi.IsOpened)
		{
			GlobalLog.Error("[TakeQ20Gems] Unexpected error. Stash is closed.");
			return false;
		}
		while (true)
		{
			Item gem = StashGems.FirstOrDefault(smethod_2);
			if ((RemoteMemoryObject)(object)gem == (RemoteMemoryObject)null)
			{
				break;
			}
			if (Inventories.AvailableInventorySquares != 0)
			{
				GlobalLog.Warn("[TakeQ20Gems] Now taking \"" + gem.Name + "\".");
				if (!(await Inventories.FastMoveFromStashTab(gem.LocationTopLeft)))
				{
					return false;
				}
				continue;
			}
			GlobalLog.Warn("[TakeQ20Gems] Inventory is full.");
			return true;
		}
		return true;
	}

	private static async Task<bool> SellGems()
	{
		List<Vector2i> inventoryGems = (from i in Inventories.InventoryItems
			where (RemoteMemoryObject)(object)i.Components.SkillGemComponent != (RemoteMemoryObject)null
			select i.LocationTopLeft).ToList();
		if (inventoryGems.Count == 0)
		{
			GlobalLog.Error("[SellGems] Unexpected error. Fail to find any skill gem in inventory.");
			return false;
		}
		return await TownNpcs.SellItems(inventoryGems);
	}

	private static bool smethod_1(Item gem)
	{
		int quality = gem.Quality;
		double num = PoeNinjaTracker.LookupChaosValue(gem);
		return quality > 0 && num <= (double)Class36.Settings.GcpMaxPrice && !IsRareGem(gem);
	}

	private static bool smethod_2(Item gem)
	{
		int quality = gem.Quality;
		double num = PoeNinjaTracker.LookupChaosValue(gem);
		return quality >= 20 && num <= (double)Class36.Settings.GcpMaxPrice && !IsRareGem(gem);
	}

	private static bool IsRareGem(Item gem)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		string name = gem.Name;
		bool flag = (int)gem.SkillGemQualityType > 0;
		int num;
		switch (name)
		{
		default:
			num = (name.Contains("Awakened") ? 1 : 0);
			break;
		case "Enlighten Support":
		case "Empower Support":
		case "Enhance Support":
			num = 1;
			break;
		}
		return (byte)((uint)num | (flag ? 1u : 0u)) != 0;
	}
}
