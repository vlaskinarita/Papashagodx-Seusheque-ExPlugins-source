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

public static class A8_Q3_GemlingLegion
{
	private static bool bool_0;

	private static IEnumerable<Monster> Gemlings => ObjectManager.Objects.Where((Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata.Contains("GemlingLegionnaire"));

	private static List<CachedObject> CachedGemlings
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["Gemlings"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>();
				CombatAreaCache.Current.Storage["Gemlings"] = list;
			}
			return list;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.GemlingLegion) <= 2;
		if (!World.Act8.GrainGate.IsCurrentArea)
		{
			return;
		}
		foreach (Monster monster_0 in Gemlings)
		{
			bool isDead = ((Actor)monster_0).IsDead;
			int id = ((NetworkObject)monster_0).Id;
			List<CachedObject> cachedGemlings = CachedGemlings;
			int num = cachedGemlings.FindIndex((CachedObject c) => c.Id == ((NetworkObject)monster_0).Id);
			if (num >= 0)
			{
				if (!isDead)
				{
					cachedGemlings[num].Position = ((NetworkObject)(object)monster_0).WalkablePosition();
					continue;
				}
				GlobalLog.Warn($"[GemlingLegion] Removing dead {((NetworkObject)(object)monster_0).WalkablePosition()}");
				cachedGemlings.RemoveAt(num);
			}
			else if (!isDead)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)monster_0).WalkablePosition();
				GlobalLog.Warn($"[GemlingLegion] Registering {walkablePosition}");
				cachedGemlings.Add(new CachedObject(id, walkablePosition));
			}
		}
	}

	public static async Task<bool> KillGemlings()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act8.GrainGate.IsCurrentArea)
		{
			CachedObject gemling = CachedGemlings.FirstOrDefault();
			if (!(gemling != null))
			{
				await Helpers.Explore();
				return true;
			}
			WalkablePosition pos = gemling.Position;
			if (pos.Distance > 30)
			{
				pos.Come();
			}
			else
			{
				NetworkObject gemlingObj = gemling.Object;
				if (gemlingObj == (NetworkObject)null)
				{
					GlobalLog.Warn($"[GemlingLegion] Gemling with id {gemling.Id} no longer exist.");
					CachedGemlings.RemoveAt(0);
				}
			}
			return true;
		}
		await Travel.To(World.Act8.GrainGate);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act8.SarnEncampment, TownNpcs.townNpc_2, "Gemling Legion Reward", null, "Book-a8q7");
	}
}
