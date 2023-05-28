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

public static class A7_Q6_LightingTheWay
{
	private static bool bool_0;

	private static IEnumerable<Chest> FireflyChest => ObjectManager.Objects.Where((Chest c) => ((NetworkObject)c).Metadata.Contains("QuestChests/Fireflies/FireflyChest"));

	private static List<CachedObject> CachedFireflyChests
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["FireflyChests"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(7);
				CombatAreaCache.Current.Storage["FireflyChests"] = list;
			}
			return list;
		}
	}

	public static void Tick()
	{
		bool_0 = Helpers.PlayerHasQuestItemAmount("Act7/Firefly", 7) || QuestManager.GetStateInaccurate(Quests.LightingTheWay) <= 3;
		if (!World.Act7.DreadThicket.IsCurrentArea)
		{
			return;
		}
		foreach (Chest chest_0 in FireflyChest)
		{
			bool isOpened = chest_0.IsOpened;
			bool isTargetable = ((NetworkObject)chest_0).IsTargetable;
			int id = ((NetworkObject)chest_0).Id;
			List<CachedObject> cachedFireflyChests = CachedFireflyChests;
			int num = cachedFireflyChests.FindIndex((CachedObject c) => c.Id == ((NetworkObject)chest_0).Id);
			if (num >= 0)
			{
				if (isOpened)
				{
					GlobalLog.Warn($"[LightingTheWay] Removing opened {((NetworkObject)(object)chest_0).WalkablePosition()}");
					cachedFireflyChests.RemoveAt(num);
				}
			}
			else if (!isOpened && isTargetable)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)chest_0).WalkablePosition();
				GlobalLog.Warn($"[LightingTheWay] Registering {walkablePosition}");
				cachedFireflyChests.Add(new CachedObject(id, walkablePosition));
			}
		}
	}

	public static async Task<bool> GrabFireflies()
	{
		if (!bool_0)
		{
			if (World.Act7.DreadThicket.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedFireflyChests.FirstOrDefault()))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act7.DreadThicket);
			return true;
		}
		return false;
	}
}
