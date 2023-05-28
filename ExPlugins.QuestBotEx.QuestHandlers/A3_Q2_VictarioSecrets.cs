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

public static class A3_Q2_VictarioSecrets
{
	public const int MinHaveAllBustsState = 3;

	private static bool UunuKxwuhc;

	public static readonly HashSet<int> HaveAllBustsStates;

	private static bool HaveAllBustsByState
	{
		get
		{
			int stateInaccurate = QuestManager.GetStateInaccurate(Quests.VictarioSecrets);
			return stateInaccurate <= 3 || HaveAllBustsStates.Contains(stateInaccurate);
		}
	}

	private static IEnumerable<Chest> VictarioStashes => ObjectManager.Objects.Where((Chest c) => ((NetworkObject)c).Metadata.Contains("QuestChests/Victario/Stash"));

	private static List<CachedObject> CachedVictarioStashes
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["VictarioStashes"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(3);
				CombatAreaCache.Current.Storage["VictarioStashes"] = list;
			}
			return list;
		}
	}

	public static void Tick()
	{
		UunuKxwuhc = Helpers.PlayerHasQuestItemAmount("Busts/Bust", 3) || HaveAllBustsByState;
		if (!World.Act3.Sewers.IsCurrentArea)
		{
			return;
		}
		foreach (Chest chest_0 in VictarioStashes)
		{
			bool isOpened = chest_0.IsOpened;
			int id = ((NetworkObject)chest_0).Id;
			List<CachedObject> cachedVictarioStashes = CachedVictarioStashes;
			int num = cachedVictarioStashes.FindIndex((CachedObject c) => c.Id == ((NetworkObject)chest_0).Id);
			if (num < 0)
			{
				if (!isOpened)
				{
					WalkablePosition walkablePosition = ((NetworkObject)(object)chest_0).WalkablePosition();
					GlobalLog.Warn($"[VictarioSecrets] Registering {walkablePosition}");
					cachedVictarioStashes.Add(new CachedObject(id, walkablePosition));
				}
			}
			else if (isOpened)
			{
				GlobalLog.Warn($"[VictarioSecrets] Removing opened {((NetworkObject)(object)chest_0).WalkablePosition()}");
				cachedVictarioStashes.RemoveAt(num);
			}
		}
	}

	public static async Task<bool> GrabBusts()
	{
		if (UunuKxwuhc)
		{
			return false;
		}
		if (World.Act3.Sewers.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedVictarioStashes.FirstOrDefault()))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act3.Sewers);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act3.SarnEncampment, TownNpcs.Hargan, "Platinum Bust Reward", null, "Book-a3q11");
	}

	static A3_Q2_VictarioSecrets()
	{
		HaveAllBustsStates = new HashSet<int> { 5, 7, 9, 13, 17, 21 };
	}
}
