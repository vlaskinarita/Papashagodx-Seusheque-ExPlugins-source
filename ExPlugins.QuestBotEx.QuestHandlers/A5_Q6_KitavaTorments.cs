using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public class A5_Q6_KitavaTorments
{
	private static IEnumerable<Chest> RelicCases => ObjectManager.Objects.Where((Chest c) => ((NetworkObject)c).Metadata.Contains("QuestChests/Reliquary/RelicCase"));

	private static List<CachedObject> CachedRelicCases
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["RelicCases"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(3);
				CombatAreaCache.Current.Storage["RelicCases"] = list;
			}
			return list;
		}
	}

	public static void Tick()
	{
		if (!World.Act5.Reliquary.IsCurrentArea)
		{
			return;
		}
		foreach (Chest chest_0 in RelicCases)
		{
			bool isOpened = chest_0.IsOpened;
			int id = ((NetworkObject)chest_0).Id;
			List<CachedObject> cachedRelicCases = CachedRelicCases;
			int num = cachedRelicCases.FindIndex((CachedObject c) => c.Id == ((NetworkObject)chest_0).Id);
			if (num >= 0)
			{
				if (isOpened)
				{
					GlobalLog.Warn($"[KitavaTorments] Removing opened {((NetworkObject)(object)chest_0).WalkablePosition()}");
					cachedRelicCases.RemoveAt(num);
				}
			}
			else if (!isOpened)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)chest_0).WalkablePosition();
				GlobalLog.Warn($"[KitavaTorments] Registering {walkablePosition}");
				cachedRelicCases.Add(new CachedObject(id, walkablePosition));
			}
		}
	}

	public static async Task<bool> GrabTorments()
	{
		if (Helpers.PlayerHasQuestItemAmount("Act5/Torment", 3))
		{
			return false;
		}
		if (World.Act5.Reliquary.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedRelicCases.FirstOrDefault()))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act5.Reliquary);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Lani, "Torments Reward", null, "Book-a5q7");
	}
}
