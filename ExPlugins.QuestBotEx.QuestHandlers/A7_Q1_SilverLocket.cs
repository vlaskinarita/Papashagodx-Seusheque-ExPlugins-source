using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q1_SilverLocket
{
	private static Chest DirtyLockbox => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2580 }).FirstOrDefault<Chest>();

	private static CachedObject CachedDirtyLockbox
	{
		get
		{
			return CombatAreaCache.Current.Storage["DirtyLockbox"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["DirtyLockbox"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act7.BrokenBridge.IsCurrentArea && CachedDirtyLockbox == null)
		{
			Chest dirtyLockbox = DirtyLockbox;
			if ((NetworkObject)(object)dirtyLockbox != (NetworkObject)null)
			{
				CachedDirtyLockbox = new CachedObject((NetworkObject)(object)dirtyLockbox);
			}
		}
	}

	public static async Task<bool> GrabSilverLocket()
	{
		if (!Helpers.PlayerHasQuestItem("SilverLocket"))
		{
			if (World.Act7.BrokenBridge.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedDirtyLockbox))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act7.BrokenBridge);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.WeylamRoth, "Silver Locket Reward", Quests.SilverLocket.Id, null, shouldLogOut: true);
	}
}
