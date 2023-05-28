using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q2_NoLoveForOldGhosts
{
	private static Chest TemplarStash => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2586 }).FirstOrDefault<Chest>();

	private static CachedObject CachedTemplarStash
	{
		get
		{
			return CombatAreaCache.Current.Storage["TemplarStash"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["TemplarStash"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act10.Ossuary.IsCurrentArea && CachedTemplarStash == null)
		{
			Chest templarStash = TemplarStash;
			if ((NetworkObject)(object)templarStash != (NetworkObject)null)
			{
				CachedTemplarStash = new CachedObject((NetworkObject)(object)templarStash);
			}
		}
	}

	public static async Task<bool> GrabElixir()
	{
		if (!Helpers.PlayerHasQuestItem("Potion"))
		{
			if (World.Act10.Ossuary.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedTemplarStash))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act10.Ossuary);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.WeylamRothA10, "Elixir of Allure Reward", null, "Book-a10q4");
	}
}
