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

public static class A3_Q7_FixtureOfFate
{
	private static Npc Siosa => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)23 }).FirstOrDefault<Npc>();

	private static IEnumerable<Chest> BookStands => ObjectManager.Objects.Where((Chest c) => ((NetworkObject)c).Metadata.Contains("QuestChests/Siosa/GoldenBookStand"));

	private static List<CachedObject> CachedBookStands
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["BookStands"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(4);
				CombatAreaCache.Current.Storage["BookStands"] = list;
			}
			return list;
		}
	}

	private static CachedObject CachedSiosa
	{
		get
		{
			return CombatAreaCache.Current.Storage["Siosa"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Siosa"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act3.Library.IsCurrentArea)
		{
			if (CachedSiosa == null)
			{
				Npc siosa = Siosa;
				if ((NetworkObject)(object)siosa != (NetworkObject)null)
				{
					CachedSiosa = new CachedObject((NetworkObject)(object)siosa);
				}
			}
		}
		else
		{
			if (!World.Act3.Archives.IsCurrentArea)
			{
				return;
			}
			foreach (Chest chest_0 in BookStands)
			{
				bool isOpened = chest_0.IsOpened;
				int id = ((NetworkObject)chest_0).Id;
				List<CachedObject> cachedBookStands = CachedBookStands;
				int num = cachedBookStands.FindIndex((CachedObject s) => s.Id == ((NetworkObject)chest_0).Id);
				if (num < 0)
				{
					if (!isOpened)
					{
						WalkablePosition walkablePosition = ((NetworkObject)(object)chest_0).WalkablePosition();
						GlobalLog.Warn($"[FixtureOfFate] Registering {walkablePosition}");
						cachedBookStands.Add(new CachedObject(id, walkablePosition));
					}
				}
				else if (isOpened)
				{
					GlobalLog.Warn($"[FixtureOfFate] Removing opened {((NetworkObject)(object)chest_0).WalkablePosition()}");
					cachedBookStands.RemoveAt(num);
				}
			}
		}
	}

	public static async Task<bool> GrabPages()
	{
		if (Helpers.PlayerHasQuestItemAmount("GoldenPages/Page", 4))
		{
			return false;
		}
		if (World.Act3.Archives.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedBookStands.FirstOrDefault()))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act3.Archives);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act3.Library.IsCurrentArea)
		{
			CachedObject siosa = CachedSiosa;
			if (siosa != null)
			{
				WalkablePosition pos = siosa.Position;
				if (!pos.IsFar)
				{
					string reward = QuestBotSettings.Instance.GetRewardForQuest(Quests.FixtureOfFate.Id);
					if (!(await siosa.Object.AsTownNpc().TakeReward(reward, "Golden Pages Reward")))
					{
						ErrorManager.ReportError();
					}
					return false;
				}
				pos.Come();
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act3.Library);
		return true;
	}
}
