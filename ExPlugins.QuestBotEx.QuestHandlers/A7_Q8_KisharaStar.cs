using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q8_KisharaStar
{
	private static Chest KisharaLockbox => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2581 }).FirstOrDefault<Chest>();

	private static CachedObject CachedKisharaLockbox
	{
		get
		{
			return CombatAreaCache.Current.Storage["KisharaLockbox"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["KisharaLockbox"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act7.Causeway.IsCurrentArea && CachedKisharaLockbox == null)
		{
			Chest kisharaLockbox = KisharaLockbox;
			if ((NetworkObject)(object)kisharaLockbox != (NetworkObject)null)
			{
				CachedKisharaLockbox = new CachedObject((NetworkObject)(object)kisharaLockbox);
			}
		}
	}

	public static async Task<bool> GrabKisharaStar()
	{
		if (!Helpers.PlayerHasQuestItem("KisharaStar"))
		{
			if (World.Act7.Causeway.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedKisharaLockbox))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act7.Causeway);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act7.Causeway.IsCurrentArea)
		{
			await Travel.To(World.Act7.VaalCity);
			return true;
		}
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.WeylamRoth, "Kishara's Star Reward", null, "Book-a7q6");
	}
}
