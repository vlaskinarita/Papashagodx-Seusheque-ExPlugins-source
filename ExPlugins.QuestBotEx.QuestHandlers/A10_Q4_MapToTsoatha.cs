using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q4_MapToTsoatha
{
	private static Chest PearlCase => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2587 }).FirstOrDefault<Chest>();

	private static CachedObject CachedPearlCase
	{
		get
		{
			return CombatAreaCache.Current.Storage["PearlCase"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["PearlCase"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act10.Reliquary.IsCurrentArea && CachedPearlCase == null)
		{
			Chest pearlCase = PearlCase;
			if ((NetworkObject)(object)pearlCase != (NetworkObject)null)
			{
				CachedPearlCase = new CachedObject((NetworkObject)(object)pearlCase);
			}
		}
	}

	public static async Task<bool> GrabTeardrop()
	{
		if (!Helpers.PlayerHasQuestItem("Teardrop"))
		{
			if (World.Act10.Reliquary.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedPearlCase))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act10.Reliquary);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.LillyRothA10, "Tsoatha Reward", Quests.MapToTsoatha.Id, null, shouldLogOut: true);
	}
}
