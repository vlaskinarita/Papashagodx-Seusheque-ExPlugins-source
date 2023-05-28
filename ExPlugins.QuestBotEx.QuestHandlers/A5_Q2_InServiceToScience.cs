using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q2_InServiceToScience
{
	private static Chest ExperimentalSupplies => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2573 }).FirstOrDefault<Chest>();

	private static CachedObject CachedSupplies
	{
		get
		{
			return CombatAreaCache.Current.Storage["ExperimentalSupplies"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["ExperimentalSupplies"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act5.ControlBlocks.IsCurrentArea && CachedSupplies == null)
		{
			Chest experimentalSupplies = ExperimentalSupplies;
			if ((NetworkObject)(object)experimentalSupplies != (NetworkObject)null)
			{
				CachedSupplies = new CachedObject((NetworkObject)(object)experimentalSupplies);
			}
		}
	}

	public static async Task<bool> GrabMiasmeter()
	{
		if (Helpers.PlayerHasQuestItem("Miasmeter"))
		{
			return false;
		}
		if (World.Act5.ControlBlocks.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedSupplies))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act5.ControlBlocks);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Vilenta, "Miasmeter Reward", null, "Book-a5q3");
	}
}
